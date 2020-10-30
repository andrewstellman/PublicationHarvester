/*
 *                           Publication Harvester
 *              Copyright © 2003-2019 Stellman & Greene Consulting
 *      Developed for Joshua Zivin and Pierre Azoulay, Columbia University
 *            http://www.stellman-greene.com/PublicationHarvester
 *
 * This program is free software; you can redistribute it and/or modify it under
 * the terms of the GNU General Public License as published by the Free Software 
 * Foundation; either version 2 of the License, or (at your option) any later 
 * version.
 *
 * This program is distributed in the hope that it will be useful, but WITHOUT 
 * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS 
 * FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License along with 
 * this program (GPL.txt); if not, write to the Free Software Foundation, Inc., 51 
 * Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Net;
using System.IO;

namespace Com.StellmanGreene.PubMed
{
    /// <summary>
    /// The NCBI class issues web queries to the NCBI server. The NCBI server
    /// expects two queries. First, it wants a query to the esearch page, which
    /// finds the data and puts it in a cache on the server. The second query
    /// to efetch retrieves that data in the format specified by FetchMethod.
    /// </summary>
    public class NCBI
    {
        public static bool UsePostRequest { get; set; }

        private string FetchMethod;

        /// <summary>
        /// The "&api_key=" parameter to add to an API Request,
        /// will be blank if no API key is being used
        /// </summary>
        public static string ApiKeyParam = "";

        /// <summary>
        /// Path of the api_key.txt file to read the API key from
        /// </summary>
        public static string ApiKeyPath { get; private set; }

        /// <summary>
        /// True if an API key is being used
        /// </summary>
        public static bool ApiKeyExists { get; private set; }

        /// <summary>
        /// For throttling NCBI requests, depends on whether an API key is specified
        /// </summary>
        private static int requestsPerSecond = 2;

        static NCBI()
        {
            GetApiKey();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="FetchMethod">The fetch method ("docsum", "medline", "xml", etc.)</param>
        public NCBI(string FetchMethod)
        {
            this.FetchMethod = FetchMethod;
            NCBI.UsePostRequest = false;
        }

        /// <summary>
        /// Get the API key from api_key.txt in the executing assembly directory
        /// </summary>
        /// <remarks>
        /// See https://ncbiinsights.ncbi.nlm.nih.gov/2017/11/02/new-api-keys-for-the-e-utilities/
        /// </remarks>
        public static void GetApiKey(string overrideApiKeyPath = "")
        {
            if (!string.IsNullOrWhiteSpace(overrideApiKeyPath) && File.Exists(overrideApiKeyPath.Trim()))
            {
                ApiKeyPath = overrideApiKeyPath.Trim();
            }
            else if (Environment.GetEnvironmentVariables().Contains("NCBI_API_KEY_FILE"))
            {
                ApiKeyPath = Environment.GetEnvironmentVariables()["NCBI_API_KEY_FILE"].ToString();
            }
            else
            {
                string directory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                ApiKeyPath = directory + "\\api_key.txt";
            }
            if (File.Exists(ApiKeyPath))
            {
                string apiKey = System.Text.RegularExpressions.Regex.Replace(File.ReadAllText(ApiKeyPath), @"\s+", string.Empty);
                if (apiKey.Length > 2)
                {
                    ApiKeyParam = "&api_key=" + apiKey;
                    requestsPerSecond = 8;
                    ApiKeyExists = true;
                }
                else
                {
                    ApiKeyExists = false;
                }
            }
        }

        /// <summary>
        /// Execute a query against NCBI
        /// This is a virtual function because MockNCBI must override it
        /// </summary>
        /// <param name="query">The query string to search for</param>
        /// <returns>The results of the search in the format specified when the instance was initializd</returns>
        public virtual string Search(string query)
        {
            EsearchResults esearchResults = ExecuteEsearch(query, ApiKeyParam);
            return ExecuteFetch(esearchResults);
        }

        /// <summary>
        /// Issue the first NCBI query to initialize the search.
        /// </summary>
        /// <param name="query">The Medline query to issue</param>
        /// <returns>A string containing the XML result header</returns>
        private static EsearchResults ExecuteEsearch(string query, string apiKeyParam)
        {
            string sURL = "https://www.ncbi.nlm.nih.gov/entrez/eutils/esearch.fcgi";

            WebRequest request = null;

            // Set the securty protocol to avoid SSL errors ("Could not create SSL/TLS secure channel")
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072; // SecurityProtocolType.Tls12

            // If we're using a GET request (default) instead of a POST request, create the query with the request term
            if (!UsePostRequest)
            {
                sURL += "?";
                sURL += "&db=Pubmed&retmax=1&usehistory=y";
                sURL += apiKeyParam;
                sURL += "&term=" + query;
                request = WebRequest.Create(sURL);
            }
            else
            {
                // Create the POST request
                request = WebRequest.Create(sURL);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                byte[] byteArray = UTF8Encoding.UTF8.GetBytes("db=Pubmed&retmax=1&usehistory=y" + apiKeyParam + "&term=" + query);
                request.ContentLength = byteArray.Length;
                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }
            }

            string results = ExecuteWebRequest(request);
            return ParseSearchResults(results);
        }

        /// <summary>
        /// Issue the second NCBI query to fetch the results.
        /// </summary>
        /// <param name="results">The results of the first query.</param>
        /// <returns>A string containing the results in NCBI text format</returns>
        private string ExecuteFetch(EsearchResults results)
        {
            // Set the securty protocol to avoid SSL errors ("Could not create SSL/TLS secure channel")
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072; // SecurityProtocolType.Tls12

            string sURL = "https://www.ncbi.nlm.nih.gov/entrez/eutils/efetch.fcgi?rettype=" + this.FetchMethod + "&retmode=text&restart=0&db=Pubmed";
            sURL = sURL + ApiKeyParam;
            sURL = sURL + "&retmax=" + results.Count;
            sURL = sURL + "&query_key=" + results.QueryKey;
            sURL = sURL + "&WebEnv=" + results.WebEnv;
            WebRequest request = WebRequest.Create(sURL);

            return ExecuteWebRequest(request);
        }

        /// <summary>
        /// Track the request times in ticks to throttle requesets
        /// </summary>
        private static List<long> requestTimes = new List<long>();

        /// <summary>
        /// Execute a web request, throttling requests to under 3/sec if there's no API
        /// key specified, or under 10/sec if there is
        /// </summary>
        /// <param name="request">WebRequest to execute</param>
        /// <returns>Results of the request, or throws an exception</returns>
        public static string ExecuteWebRequest(WebRequest request)
        {
            long oneSecondAgo = DateTime.Now.Ticks - 8999999;
            long oneSecondFromNow = DateTime.Now.Ticks + 11000000;

            do
            {
                List<long> newRequestTimes = new List<long>();
                foreach(long t in requestTimes)
                {
                    if (t > oneSecondAgo) newRequestTimes.Add(t);
                }
                requestTimes = newRequestTimes;
            } while ((requestTimes.Count >= requestsPerSecond) && (DateTime.Now.Ticks < oneSecondFromNow));

            requestTimes.Add(DateTime.Now.Ticks);

            try
            {
                using (WebResponse response = request.GetResponse())
                using (Stream responseStream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(responseStream))
                {
                    string Results = reader.ReadToEnd();
                    return Results;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Parse the results from ExecuteFetch()
        /// </summary>
        /// <param name="ResultString">The string containing the XML returned by the NCBI server</param>
        /// <returns></returns>
        internal static EsearchResults ParseSearchResults(string ResultString)
        {
            XmlDocument xml = new XmlDocument();
            try
            {
                xml.LoadXml(ResultString);
            }
            catch
            {
                // If the XML is malformed, write it to a log file called pubharvester_error.log and throw an exception
                using (StreamWriter writer = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\pubharvester_error.log", true))
                {
                    writer.WriteLine("XML data received " + System.DateTime.Now.ToString());
                    writer.WriteLine("--- begin data ---");
                    writer.WriteLine(ResultString);
                    writer.WriteLine("--- end data ---");
                    writer.WriteLine();
                    writer.WriteLine();
                }
                throw new Exception("Unable to process XML returned by the NCBI server. Offending XML has been written to pubharvester_error.log.");
            }

            // Build the EsearchResults
            NCBI.EsearchResults results = new NCBI.EsearchResults();
            if (xml.DocumentElement.FirstChild.Name == "ERROR")
            {
                // No results were found
                results.Found = false;
                results.Count = 0;
                results.WebEnv = "";
                return results;
            }

            // Go through the XML and pull out WebEnv, QueryKey and Count
            int i;
            for (i = 0; i <= xml.DocumentElement.ChildNodes.Count - 1; i++)
            {
                if (xml.DocumentElement.ChildNodes.Item(i).Name == "WebEnv")
                {
                    results.WebEnv = xml.DocumentElement.ChildNodes.Item(i).InnerText;
                }
                else if (xml.DocumentElement.ChildNodes.Item(i).Name == "QueryKey")
                {
                    results.QueryKey = System.Convert.ToInt32(xml.DocumentElement.ChildNodes.Item(i).InnerText);
                }
                else if (xml.DocumentElement.ChildNodes.Item(i).Name == "Count")
                {
                    results.Count = System.Convert.ToInt32(xml.DocumentElement.ChildNodes.Item(i).InnerText);
                }
            }

            // Set Found to true if there were results found
            if (results.Count > 0)
            {
                results.Found = true;
            }
            else
            {
                results.Found = false;
            }
            return results;
        }



        /// <summary>EsearchResults is returned by ExecuteEsearch(), and used as input for ExecuteFetch()
        /// <para>The NCBI web search requires two or more queries. The first one feeds it the search terms, while the rest of the queries page through the results.</para>
        /// </summary>
        internal class EsearchResults
        {

            /// <summary>
            /// Count contains the number of results found.
            /// </summary>
            private int _count;
            public int Count
            {
                get { return _count; }
                set { _count = value; }
            }

            /// <summary>
            /// Found is set to false if no results are returned.
            /// </summary>
            private bool _found;
            public bool Found
            {
                get { return _found; }
                set { _found = value; }
            }

            /// <summary>
            /// The query key is a parameter returned by NCBI to identify the results.
            /// </summary>
            private int _queryKey;
            public int QueryKey
            {
                get { return _queryKey; }
                set { _queryKey = value; }
            }

            /// <summary>
            /// WebEnv is a parameter returned by NCBI to identify the results.
            /// </summary>
            private string _webEnv;
            public string WebEnv
            {
                get { return _webEnv; }
                set { _webEnv = value; }
            }
        }

    }

 
}
