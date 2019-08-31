/*
 *                                FindRelated
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
using System.Data;
using System.Diagnostics;
using Com.StellmanGreene.PubMed;
using System.Linq;

namespace Com.StellmanGreene.FindRelated
{
    internal class RelatedFinder
    {
        struct RankAndScore
        {
            public int Rank { get; set; }
            public int Score { get; set; }
        }

        /// <summary>
        /// URL of the NCBI eLink API
        /// </summary>
        const string ELINK_URL = "https://www.ncbi.nlm.nih.gov/entrez/eutils/elink.fcgi";

        /// <summary>
        /// DB to use with the NCBI eLink API
        /// </summary>
        const string ELINK_DB = "pubmed";

        /// <summary>
        /// DBFROM to use with the NCBI eLink API
        /// </summary>
        const string ELINK_DBFROM = "pubmed";

        /// <summary>
        /// Number of IDs to ask for in every NCBI eLink request
        /// </summary>
        const int ELINK_IDS_PER_REQUEST = 10;

        private NCBI ncbi = new NCBI("medline");

        /// <summary>
        /// Number of PMIDs processed
        /// </summary>
        private int _pmidsProcessed = 0;

        public System.ComponentModel.BackgroundWorker BackgroundWorker { get; set; }

        /// <summary>
        /// Execute the FindRelated search, create and populate the tables
        /// </summary>
        /// <param name="odbcDsn">ODBC DSN to access the SQL server</param>
        /// <param name="relatedTableName">Name of the FindRelated SQL table to create</param>
        /// <param name="inputFileInfo">FileInfo object with information about the input CSV file</param>
        /// <param name="resume">True if resuming a previous run</param>
        /// <param name="outputFilename">Output filename</param>
        public void Go(
            string odbcDsn,
            string relatedTableName,
            FileInfo inputFileInfo,
            bool resume,
            string outputFilename)
        {
            if (NCBI.ApiKeyExists)
            {
                Trace.WriteLine("Using API key: " + NCBI.ApiKeyPath);
            }
            else
            {
                Trace.WriteLine("Performance is limited to under 3 requests per second.");
                Trace.WriteLine("Consider pasting an API key into " + NCBI.ApiKeyPath);
                Trace.WriteLine("Or set the NCBI_API_KEY_FILE environemnt variable to the API key file path");
                Trace.WriteLine("For more information, see https://ncbiinsights.ncbi.nlm.nih.gov/2017/11/02/new-api-keys-for-the-e-utilities/");
            }

            Trace.WriteLine($"Requesting up to {ELINK_IDS_PER_REQUEST} PMIDs per NCBI eLink API request");

            Database db = new Database(odbcDsn);

            string queueTableName = relatedTableName + "_queue";

            InputQueue inputQueue;
            if (!resume)
            {
                if (!CreateOutputFile(outputFilename))
                    return;

                CreateTables(db, relatedTableName, queueTableName);
                inputQueue = new InputQueue(inputFileInfo, db, queueTableName);
            }
            else
            {
                inputQueue = new InputQueue(db, queueTableName);
            }

            while (inputQueue.Next(ELINK_IDS_PER_REQUEST) > 0)
            {
                BackgroundWorker.ReportProgress((100 * inputQueue.Progress) / inputQueue.TotalPmidsAdded);

                Trace.WriteLine($"{DateTime.Now} - executing API query for related articles for {inputQueue.CurrentPmids.Count()} PMIDs");

                // Do the linked publication search for the author's PMIDs and process the results.
                // This returns a Dictionary that maps author publications (from the PeoplePublications table)
                // to linked publications, so each key is one of the author publications read from the DB originally.
                string xml = null;
                bool success = false;
                bool failed = false;
                while (!success)
                {
                    try
                    {
                        xml = ExecuteRelatedSearch(inputQueue.CurrentPmids);
                        success = true;
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(DateTime.Now + " - an error occurred while executing the related query, attempting to repeat the search");
                        Trace.WriteLine(ex.Message);
                        Trace.WriteLine(ex.StackTrace);
                        failed = true;
                    }
                }
                if (failed)
                    Trace.WriteLine(DateTime.Now + " - successfully recovered from the error, continuing execution");

                Dictionary<int, List<int>> relatedSearchResults = GetIdsFromXml(xml, out Dictionary<int, Dictionary<int, RankAndScore>> relatedRanks);

                bool completed;

                completed = WriteRelatedRanksToOutputFileAndDatabase(db, relatedTableName, relatedSearchResults, relatedRanks, outputFilename, inputQueue);
                if (!completed) // WriteRelatedRankToOutputFile() returns false if the user stopped the operation
                    break;
            }
            BackgroundWorker.ReportProgress(100);
        }

        /// <summary>
        ///  Create the output file
        /// </summary>
        /// <returns></returns>
        private bool CreateOutputFile(string filename)
        {
            try
            {
                if (File.Exists(filename))
                {
                    string outputFileName = Path.GetFileName(filename);
                    if (File.Exists(filename + ".bak"))
                    {
                        Trace.WriteLine(DateTime.Now + " - deleting old output .bak file '" + outputFileName + ".bak'");
                        File.Delete(filename + ".bak");
                    }
                    Trace.WriteLine(DateTime.Now + " - renaming old output file '" + outputFileName + "' to '" + outputFileName + ".bak'");
                    File.Move(filename, filename + ".bak");
                }

                string header = "pmid,rltd_pmid,rltd_rank,rltd_score" + Environment.NewLine;
                File.WriteAllText(filename, header);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(DateTime.Now + " - unable to create the output file: " + ex.Message);
                Trace.WriteLine(ex.StackTrace);
            }
            return true;
        }

        /// <summary>
        /// Go through all of the ranks and scores retrieved from the server for each PMID and write them to the output file and the database.
        /// </summary>
        /// <param name="db">Database to write to</param>
        /// <param name="relatedTableName">Name of the related table</param>
        /// <param name="relatedSearchResults">NCBI search results parsed into a dictionary that maps queried PMIDs to a list of related PMIDs</param>
        /// <param name="relatedRanks">Dictionary parsed from NCBI search results that maps each queried PMID to a dictionary of related PMIDs and their ranks and scores</param>
        /// <param name="outputFilename">Output file to append to</param>
        /// <param name="inputQueue">Input queue for marking success or error</param>
        /// <returns>True if a lines were successfully added to the file and table, false if an error occurred</returns>
        private bool WriteRelatedRanksToOutputFileAndDatabase(Database db, string relatedTableName, 
            Dictionary<int, List<int>> relatedSearchResults, Dictionary<int, Dictionary<int, RankAndScore>> relatedRanks,
            string outputFilename, InputQueue inputQueue)
        {
            if (BackgroundWorker != null && BackgroundWorker.CancellationPending)
            {
                Trace.WriteLine(DateTime.Now + " - stopped");
                return false;
            }

            foreach (int pmid in relatedSearchResults.Keys)
            {
                List<int> relatedPmids = relatedSearchResults[pmid];

                if (relatedPmids == null)
                {
                    Trace.WriteLine($"{DateTime.Now} - found empty related PMID list for PMID {pmid} ({++_pmidsProcessed} of {inputQueue.TotalPmidsAdded})");
                }
                else if (!relatedRanks.ContainsKey(pmid))
                {
                    Trace.WriteLine($"{DateTime.Now} - no ranks or scores found  for PMID {pmid} ({++_pmidsProcessed} of {inputQueue.TotalPmidsAdded})");
                }
                else
                {
                    Trace.WriteLine($"{DateTime.Now} - found {relatedPmids.Count} results for PMID {pmid} ({++_pmidsProcessed} of {inputQueue.TotalPmidsAdded})");

                    Dictionary<int, RankAndScore> ranksAndScores = relatedRanks[pmid];

                    foreach (int relatedPmid in relatedPmids)
                    {
                        if (!ranksAndScores.ContainsKey(relatedPmid))
                            Trace.WriteLine(DateTime.Now + " - unable to find related ranks and scores for PMID " + pmid + ", related PMID " + relatedPmid);
                        else
                        {
                            RankAndScore rankAndScore = ranksAndScores[relatedPmid];
                            string line = String.Format("{0},{1},{2},{3}", pmid, relatedPmid, rankAndScore.Rank, rankAndScore.Score);
                            string output = line + Environment.NewLine;
                            try
                            {
                                File.AppendAllText(outputFilename, output);
                            }
                            catch (Exception ex)
                            {
                                Trace.WriteLine(DateTime.Now + " - unable to append '" + line + "' to the output file: " + ex.Message);
                                Trace.WriteLine(ex.StackTrace);
                                Trace.WriteLine(DateTime.Now + " - stopping the run, use the Resume button to resume");

                                inputQueue.MarkError(pmid);

                                return false;
                            }

                            bool written = WriteRelatedRankToDatabase(db, relatedTableName, pmid, relatedPmid, rankAndScore.Rank, rankAndScore.Score);
                            if (!written)
                                return false;
                        }
                    }
                }

                // Mark the PMID processed in the queue
                inputQueue.MarkProcessed(pmid);
            }

            return true;
        }

        private static bool WriteRelatedRankToDatabase(Database db, string relatedTableName, int authorPublicationPmid, 
            int relatedPublicationPmid, int rank, int score)
        {
            try
            {
                // Write the pmid/relatedPmid pair to the related publications table.

                db.ExecuteNonQuery(
                     "INSERT INTO " + relatedTableName + " (PMID, RelatedPMID, `Rank`, Score) VALUES (?, ?, ?, ?)",
                     new System.Collections.ArrayList() { 
                                            Database.Parameter(authorPublicationPmid), 
                                            Database.Parameter(relatedPublicationPmid),
                                            Database.Parameter(rank),
                                            Database.Parameter(score),
                                        });

                return true;
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Unable to add related article " + relatedPublicationPmid + ", error message follows");
                Trace.WriteLine(ex.Message);

                return false;
            }
        }

        /// <summary>
        /// Search PubMed for all of the related publications and add them to the database, keep trying until search is successful
        /// </summary>
        /// <param name="relatedPmids">Related publications</param>
        /// <returns>NCBI search results</returns>
        private string SearchPubMedForRelatedPublications(List<int> relatedPmids)
        {
            StringBuilder searchQuery = new StringBuilder();
            foreach (int relatedPmid in relatedPmids)
            {
                // Build the search query to issue the PubMed search for related IDs
                searchQuery.AppendFormat("{0}{1}[uid]", searchQuery.Length == 0 ? String.Empty : " OR ", relatedPmid);
            }
            NCBI.UsePostRequest = true;

            // If ncbi.Search() throws an exception, retry -- web connection may be temporarily down
            bool searchSuccessful = false;
            string searchResults = null;
            while (!searchSuccessful)
            {
                try
                {
                    searchResults = ncbi.Search(searchQuery.ToString());
                    searchSuccessful = true;
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(DateTime.Now + " - web request error during NCBI search, retrying search. Error message: " + ex.Message);
                    System.Threading.Thread.Sleep(2000);
                }
            }
            return searchResults;
        }
        

        /// <summary>
        /// Create the related publications table and the queue table
        /// </summary>
        /// <param name="relatedTableName">Name of the talbe to create</param>
        /// <param name="queueTableName">Name of the queue table created</param>
        private static void CreateTables(Database db, string relatedTableName, string queueTableName)
        {
            // Create the related table
            db.ExecuteNonQuery("DROP TABLE IF EXISTS " + relatedTableName);
            db.ExecuteNonQuery("CREATE TABLE " + relatedTableName + @" (
                PMID int(11) NOT NULL,
                RelatedPMID int(11) NOT NULL,
                `Rank` int NOT NULL,
                Score int NOT NULL,
                PRIMARY KEY (PMID, RelatedPMID)
            ) CHARSET=utf8;
            ");

            // Create the queue table
            db.ExecuteNonQuery("DROP TABLE IF EXISTS " + queueTableName);
            db.ExecuteNonQuery("CREATE TABLE " + queueTableName + @" (
                PMID int(11) NOT NULL,
                Processed bit(1) default 0 NOT NULL,
                Error bit(1) default 0 NOT NULL,
                PRIMARY KEY (PMID)
            ) CHARSET=utf8;
            ");
        }

        /// <summary>
        /// Use the NCBI Elink request to retrieve related IDs for one or more publication IDs
        /// </summary>
        /// <param name="ids">IDs to retrieve</param>
        /// <returns>A string with XML results from elink.fcgi</returns>
        private static string ExecuteRelatedSearch(IEnumerable<int> ids)
        {
            if (ids == null)
                throw new ArgumentNullException("ids");

            StringBuilder query = new StringBuilder();
            query.AppendFormat("dbfrom={0}&db={1}&id=", ELINK_DBFROM, ELINK_DB);
            bool first = true;
            foreach (int id in ids)
            {
                if (!first)
                    query.Append("&id=");
                else
                    first = false;
                query.Append(id);
            }

            // Add "&cmd=neighbor_score" to get the <Score> elements
            query.Append("&cmd=neighbor_score");

            query.Append(NCBI.ApiKeyParam);
            
            WebRequest request = WebRequest.Create(ELINK_URL);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            byte[] byteArray = UTF8Encoding.UTF8.GetBytes(query.ToString());
            request.ContentLength = byteArray.Length;
            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }

            string result = NCBI.ExecuteWebRequest(request);
            return result;
        }

        /// <summary>
        /// Retrieve the IDs from the XML results from ELink
        /// </summary>
        /// <param name="xml">XML results from ELink</param>
        /// <param name="relatedRanks">Ouput - dictionary that maps PMIDs to map for looking up rank in related results</param>
        /// <returns>Dictionary that maps source PMIds to a list of IDs extracted from the XML (or an empty list of none)</returns>
        private static Dictionary<int, List<int>> GetIdsFromXml(string xml, out Dictionary<int, Dictionary<int, RankAndScore>> relatedRanks)
        {
            Dictionary<int, List<int>> result = new Dictionary<int, List<int>>();
            relatedRanks = new Dictionary<int, Dictionary<int, RankAndScore>>();

            List<int> ids = new List<int>();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            XmlNodeList linkSets = xmlDoc["eLinkResult"].ChildNodes;
            foreach (XmlNode linkSet in linkSets)
            {
                // There's one <LinkSet> for each PMID from the search
                int pmid;
                if (int.TryParse(linkSet["IdList"]["Id"].InnerText, out pmid)) {
                    Dictionary<int, RankAndScore> relatedRank = null;
                    if (relatedRanks.ContainsKey(pmid))
                        relatedRank = relatedRanks[pmid];
                    else
                    {
                        relatedRank = new Dictionary<int, RankAndScore>();
                        relatedRanks.Add(pmid, relatedRank);
                    }

                    XmlNodeList linkSetDbs = linkSet.SelectNodes("LinkSetDb");

                    // Find the "pubmed_pubmed" link set
                    foreach (XmlNode linkSetDb in linkSetDbs)
                    {
                        if (linkSetDb["LinkName"].InnerText == "pubmed_pubmed")
                        {
                            // We've found the link set of related PubMed publications. Add it to the results.
                            List<int> linkList;
                            if (result.ContainsKey(pmid))
                                linkList = result[pmid] as List<int>;
                            else
                            {
                                linkList = new List<int>();
                                result[pmid] = linkList;
                            }

                            int rank = 0;
                            foreach (XmlNode link in linkSetDb.SelectNodes("Link"))
                            {

                                int score;
                                if (!int.TryParse(link["Score"].InnerText, out score))
                                {
                                    score = -1;
                                }

                                int relatedPmid;
                                if (int.TryParse(link["Id"].InnerText, out relatedPmid))
                                {
                                    linkList.Add(relatedPmid);
                                    RankAndScore rankAndScore = new RankAndScore() { Rank = ++rank, Score = score };
                                    relatedRank.Add(relatedPmid, rankAndScore);
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }

    }
}
