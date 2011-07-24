/*
 *                                FindRelated
 *              Copyright (c) 2003-2011 Stellman & Greene Consulting
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

namespace Com.StellmanGreene.FindRelated
{
    public class RelatedFinder
    {
        const string ELINK_URL = "http://www.ncbi.nlm.nih.gov/entrez/eutils/elink.fcgi";
        const string ELINK_DB = "pubmed";
        const string ELINK_DBFROM = "pubmed";

        private NCBI ncbi = new NCBI("medline");

        public System.ComponentModel.BackgroundWorker BackgroundWorker { get; set; }

        public void Go(string odbcDsn, string relatedTable, FileInfo inputFileInfo)
        {
            DataTable input = null;
            try
            {
                input = new Com.StellmanGreene.CSVReader.CSVReader(inputFileInfo).CreateDataTable(true);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(DateTime.Now + " - error occurred reading input file " + inputFileInfo);
                Trace.WriteLine(ex.Message);
                return;
            }

            Database db = new Database(odbcDsn);

            CreateRelatedTable(db, relatedTable);
            PublicationTypes pubTypes = new PublicationTypes(db);

            Dictionary<string, List<int>> peopleIds = new Dictionary<string, List<int>>();
            foreach (DataRow row in input.Rows)
            {
                string setnb = (string)row["Setnb"];
                int pmid = (int)row["PMID"];
                List<int> ids;
                if (!peopleIds.ContainsKey(setnb))
                {
                    ids = new List<int>();
                    peopleIds[setnb] = ids;
                }
                else
                    ids = peopleIds[setnb];
                ids.Add(pmid);
            }

            int setnbCount = 0;
            foreach (string setnb in peopleIds.Keys)
            {
                BackgroundWorker.ReportProgress((100 * setnbCount) / peopleIds.Keys.Count);
                Trace.WriteLine(DateTime.Now + " - querying for related articles for setnb " + setnb + " (" + ++setnbCount + " of " + peopleIds.Keys.Count + ")");

                // Start with the list of PMIDs for a person
                List<int> authorPmids = peopleIds[setnb];

                // Do the linked publication search for the author's PMIDs and process the results.
                // This returns a Dictionary that maps author publications (from the PeoplePublications table)
                // to linked publications, so each key is one of the author publications read from the DB originally.
                string xml = ExecuteRelatedSearch(authorPmids);
                Dictionary<int, Dictionary<int, int>> relatedRanks;
                Dictionary<int, List<int>> relatedSearchResults = GetIdsFromXml(xml, out relatedRanks);

                int total = 0;
                foreach (int key in relatedSearchResults.Keys)
                    total += relatedSearchResults[key].Count;
                Trace.WriteLine(DateTime.Now + " - found " + total + " related to " + relatedSearchResults.Keys.Count + " publications");

                int count = 0;

                // For each of the author's publications in the results, do a PubMed search for the linked publications
                // (constructed from the results) and add each of them to the database.
                foreach (int authorPublicationPmid in relatedSearchResults.Keys)
                {
                    if (BackgroundWorker != null && BackgroundWorker.CancellationPending)
                    {
                        Trace.WriteLine(DateTime.Now + " - cancelled");
                        return;
                    }

                    // Only write this article's related publications to the database if they haven't already been added.
                    // (Multiple authors might link to the same publication, and each will add the same links.)
                    int relatedCount = db.GetIntValue("SELECT Count(*) FROM " + relatedTable + " WHERE PMID = (?)",
                        new System.Collections.ArrayList() { Database.Parameter(authorPublicationPmid) });
                    if (relatedCount == 0)
                    {
                        // Get the list of related PMIDs and their ranks from the search results
                        List<int> relatedPmids = relatedSearchResults[authorPublicationPmid];
                        Dictionary<int, int> relatedRank;
                        if (relatedRanks.ContainsKey(authorPublicationPmid))
                            relatedRank = relatedRanks[authorPublicationPmid];
                        else
                            relatedRank = new Dictionary<int, int>();

                        Trace.WriteLine(DateTime.Now + " - [" + ++count + "/" + relatedSearchResults.Keys.Count + "] adding " + relatedPmids.Count + " related articles found for " + authorPublicationPmid);

                        // Search PubMed for all of the related publications and add them to the database
                        StringBuilder searchQuery = new StringBuilder();
                        foreach (int relatedPmid in relatedPmids)
                        {
                            // Build the search query to issue the PubMed search for related IDs
                            searchQuery.AppendFormat("{0}{1}[uid]", searchQuery.Length == 0 ? String.Empty : " OR ", relatedPmid);
                        }
                        NCBI.UsePostRequest = true;
                        string searchResults = ncbi.Search(searchQuery.ToString());

                        // Write each publication to the database
                        Publications publications = new Publications(searchResults, pubTypes);
                        foreach (Publication relatedPublication in publications.PublicationList)
                        {
                            if (BackgroundWorker != null && BackgroundWorker.CancellationPending)
                            {
                                Trace.WriteLine(DateTime.Now + " - cancelled");
                                return;
                            }

                            // A small number of publications come back with a null set of authors, which the database schema doesn't support.
                            if (relatedPublication.Authors != null)
                            {
                                try
                                {
                                    // Add the publication to the publications table
                                    // (this will only add it if it's not already there)
                                    Publications.WriteToDB(relatedPublication, db, pubTypes, null);

                                    int rank;
                                    if (relatedRank.ContainsKey(relatedPublication.PMID))
                                        rank = relatedRank[relatedPublication.PMID];
                                    else
                                    {
                                        rank = -1;
                                        Trace.WriteLine(DateTime.Now + " - publication " + authorPublicationPmid + " could not find rank for related " + relatedPublication.PMID);
                                    }

                                    // Write the pmid/relatedPmid pair to the related publications table.
                                    db.ExecuteNonQuery(
                                        "INSERT INTO " + relatedTable + " (PMID, RelatedPMID, Rank) VALUES (?, ?, ?)",
                                        new System.Collections.ArrayList() { 
                                            Database.Parameter(authorPublicationPmid), 
                                            Database.Parameter(relatedPublication.PMID),
                                            Database.Parameter(rank),
                                        });
                                }
                                catch (Exception ex)
                                {
                                    Trace.WriteLine("Unable to add related article " + relatedPublication.PMID + ", error message follows");
                                    Trace.WriteLine(ex.Message);

                                }
                            }
                        }
                    }
                }
            }
            BackgroundWorker.ReportProgress(100);
        }
        

        /// <summary>
        /// Create the related publications table
        /// </summary>
        /// <param name="tableName">Name of the talbe to create</param>
        private static void CreateRelatedTable(Database db, string tableName)
        {
            db.ExecuteNonQuery("DROP TABLE IF EXISTS " + tableName);
            db.ExecuteNonQuery("CREATE TABLE " + tableName + @" (
              PMID int(11) NOT NULL,
              RelatedPMID int(11) NOT NULL,
              Rank int NOT NULL,
              PRIMARY KEY  (PMID,RelatedPMID)
            ) ENGINE=MyISAM DEFAULT CHARSET=latin1;
            ");
        }


        /// <summary>
        /// Use the NCBI Elink request to retrieve related IDs for one or more publication IDs
        /// </summary>
        /// <param name="ids">IDs to retrieve</param>
        /// <param name="mindate">Optional minimum date</param>
        /// <param name="maxdate">Optional maximum date</param>
        /// <returns>A string with XML results from elink.fcgi</returns>
        private static string ExecuteRelatedSearch(IEnumerable<int> ids, string mindate = null, string maxdate = null)
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
            if (!string.IsNullOrEmpty(mindate))
                query.AppendFormat("&mindate={0}", mindate);
            if (!string.IsNullOrEmpty(maxdate))
                query.AppendFormat("&mindate={0}", maxdate);

            WebRequest request = WebRequest.Create(ELINK_URL);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            byte[] byteArray = UTF8Encoding.UTF8.GetBytes(query.ToString());
            request.ContentLength = byteArray.Length;
            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }

            using (WebResponse response = request.GetResponse())
            using (Stream responseStream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(responseStream))
            {
                string result = reader.ReadToEnd();
                return result;
            }
        }

        /// <summary>
        /// Retrieve the IDs from the XML results from ELink
        /// </summary>
        /// <param name="xml">XML results from ELink</param>
        /// <param name="relatedRansk">Ouput - dictionary that maps PMIDs to map for looking up rank in related results</param>
        /// <returns>Dictionary that maps source PMIds to a list of IDs extracted from the XML (or an empty list of none)</returns>
        private static Dictionary<int, List<int>> GetIdsFromXml(string xml, out Dictionary<int, Dictionary<int, int>> relatedRanks)
        {
            Dictionary<int, List<int>> result = new Dictionary<int, List<int>>();
            relatedRanks = new Dictionary<int, Dictionary<int, int>>();

            List<int> ids = new List<int>();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            XmlNodeList linkSets = xmlDoc["eLinkResult"].ChildNodes;
            foreach (XmlNode linkSet in linkSets)
            {
                // There's one <LinkSet> for each PMID from the search
                int pmid;
                if (int.TryParse(linkSet["IdList"]["Id"].InnerText, out pmid)) {
                    Dictionary<int, int> relatedRank = null;
                    if (relatedRanks.ContainsKey(pmid))
                        relatedRank = relatedRanks[pmid];
                    else
                    {
                        relatedRank = new Dictionary<int, int>();
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
                                int relatedPmid;
                                if (int.TryParse(link.InnerText, out relatedPmid))
                                {
                                    linkList.Add(relatedPmid);
                                    relatedRank.Add(relatedPmid, ++rank);
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
