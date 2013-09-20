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
    internal class RelatedFinder
    {
        struct RankAndScore
        {
            public int Rank { get; set; }
            public int Score { get; set; }
        }

        const string ELINK_URL = "http://www.ncbi.nlm.nih.gov/entrez/eutils/elink.fcgi";
        const string ELINK_DB = "pubmed";
        const string ELINK_DBFROM = "pubmed";

        private NCBI ncbi = new NCBI("medline");

        public System.ComponentModel.BackgroundWorker BackgroundWorker { get; set; }

        /// <summary>
        /// Execute the FindRelated search, create and populate the tables
        /// </summary>
        /// <param name="odbcDsn">ODBC DSN to access the SQL server</param>
        /// <param name="relatedTable">Name of the FindRelated SQL table to create</param>
        /// <param name="inputFileInfo">FileInfo object with information about the input CSV file</param>
        /// <param name="publicationFilter">PublicationFilter object to use for filtering publications</param>
        /// <param name="resume">True if resuming a previous run</param>
        /// <param name="liteMode">True if in "lite" mode, where it only runs the FindRelated search and does not do additional processing</param>
        /// <param name="liteModeOutputFile">Output filename for "lite" mode (ignored when not in "lite" mode)</param>
        public void Go(string odbcDsn, string relatedTable, FileInfo inputFileInfo, PublicationFilter publicationFilter, bool resume, bool liteMode, string liteModeOutputFile)
        {
            Database db = new Database(odbcDsn);

            string queueTableName = relatedTable + "_queue";
            string extremeRelevanceTableName = relatedTable + "_extremerelevance";

            InputQueue inputQueue;
            if (!resume)
            {
                if (liteMode && !CreateLiteModeOutputFile(liteModeOutputFile))
                    return;

                CreateRelatedTable(db, relatedTable, queueTableName, extremeRelevanceTableName, liteMode);
                inputQueue = new InputQueue(inputFileInfo, db, queueTableName);
            }
            else
            {
                inputQueue = new InputQueue(db, queueTableName);
            }

            int setnbCount = 0;
            while (inputQueue.Next())
            {
                BackgroundWorker.ReportProgress((100 * setnbCount) / inputQueue.Count);
                Trace.WriteLine(DateTime.Now + " - querying for related articles for setnb " + inputQueue.CurrentSetnb + " (" + ++setnbCount + " of " + inputQueue.Count + ")");

                // Do the linked publication search for the author's PMIDs and process the results.
                // This returns a Dictionary that maps author publications (from the PeoplePublications table)
                // to linked publications, so each key is one of the author publications read from the DB originally.
                string xml = ExecuteRelatedSearch(inputQueue.CurrentPmids);
                Dictionary<int, Dictionary<int, RankAndScore>> relatedRanks;
                Dictionary<int, List<int>> relatedSearchResults = GetIdsFromXml(xml, out relatedRanks);

                bool completed;

                if (liteMode)
                {
                    Trace.WriteLine(DateTime.Now + " - found " + relatedSearchResults.Count + " PMIDs for setnb " + inputQueue.CurrentSetnb);

                    completed = false;
                    completed = WriteRelatedRankToOutputFile(relatedSearchResults, relatedRanks, liteModeOutputFile);
                    if (!completed) // WriteRelatedRankToOutputFile() returns false if the user cancelled the operation
                        break;
                }
                else
                {

                    int total = 0;
                    foreach (int key in relatedSearchResults.Keys)
                        total += relatedSearchResults[key].Count;
                    Trace.WriteLine(DateTime.Now + " - found " + total + " related to " + relatedSearchResults.Keys.Count + " publications");

                    completed = ProcessSearchResults(relatedTable, publicationFilter, db, extremeRelevanceTableName, relatedRanks, relatedSearchResults, inputQueue);
                    if (!completed) // ProcessSearchResults() returns false if the user cancelled the operation
                        break;
                }
            }
            BackgroundWorker.ReportProgress(100);
        }

        /// <summary>
        ///  Create the "lite" mode output file
        /// </summary>
        /// <returns></returns>
        private bool CreateLiteModeOutputFile(string liteModeOutputFile)
        {
            try
            {
                if (File.Exists(liteModeOutputFile))
                    File.Delete(liteModeOutputFile);

                string header = "pmid,rltd_pmid,rltd_rank,rltd_score" + Environment.NewLine;
                File.WriteAllText(liteModeOutputFile, header);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(DateTime.Now + " - unable to create the \"lite\" mode output file: " + ex.Message);
                Trace.WriteLine(ex.StackTrace);
            }
            return true;
        }

        private bool WriteRelatedRankToOutputFile(Dictionary<int, List<int>> relatedSearchResults, Dictionary<int, Dictionary<int, RankAndScore>> relatedRanks, string liteModeOutputFile)
        {
            if (BackgroundWorker != null && BackgroundWorker.CancellationPending)
            {
                Trace.WriteLine(DateTime.Now + " - cancelled");
                return false;
            }

            foreach (int pmid in relatedSearchResults.Keys)
            {
                List<int> relatedPmids = relatedSearchResults[pmid];
                if (relatedPmids == null)
                    Trace.WriteLine(DateTime.Now + " - found empty related PMID list for PMID " + pmid);
                else if (!relatedRanks.ContainsKey(pmid))
                    Trace.WriteLine(DateTime.Now + " - no ranks or scores found for PMID " + pmid);
                else 
                {
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
                                File.AppendAllText(liteModeOutputFile, output);
                            }
                            catch (Exception ex)
                            {
                                Trace.WriteLine(DateTime.Now + " - unable to append '" + line + "' to the \"lite\" mode output file: " + ex.Message);
                                Trace.WriteLine(ex.StackTrace);
                                Trace.WriteLine(DateTime.Now + " - cancelling the run, use the Resume button to resume");
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// For each of the author's publications in the results, do a PubMed search for the linked publications
        /// (constructed from the results) and add each of them to the database.
        /// </summary>
        /// <param name="relatedTable"></param>
        /// <param name="publicationFilter"></param>
        /// <param name="db"></param>
        /// <param name="extremeRelevanceTableName"></param>
        /// <param name="pubTypes"></param>
        /// <param name="relatedRanks"></param>
        /// <param name="relatedSearchResults"></param>
        /// <returns>True if completed, false if cancelled</returns>
        private bool ProcessSearchResults(string relatedTable, PublicationFilter publicationFilter, Database db, string extremeRelevanceTableName, 
            Dictionary<int, Dictionary<int, RankAndScore>> relatedRanks, Dictionary<int, List<int>> relatedSearchResults,
            InputQueue inputQueue)
        {
            int count = 0;

            PublicationTypes pubTypes = new PublicationTypes(db);

            foreach (int authorPublicationPmid in relatedSearchResults.Keys)
            {
                bool error = false;

                if (BackgroundWorker != null && BackgroundWorker.CancellationPending)
                {
                    Trace.WriteLine(DateTime.Now + " - cancelled");
                    return false;
                }

                // Read the author publication from the database -- skipping MeSH headings and grants because we don't use them
                Publication authorPublication;
                bool retrievedPublication;
                try
                {
                    retrievedPublication = Publications.GetPublication(db, authorPublicationPmid, out authorPublication, true);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(DateTime.Now + " - " + ex.Message);
                    retrievedPublication = false;
                    authorPublication = new Publication();
                }
                if (!retrievedPublication)
                {
                    Trace.WriteLine(DateTime.Now + " - unable to read publication " + authorPublicationPmid + " from the database");

                    inputQueue.MarkError(authorPublicationPmid);

                    continue;
                }

                // Only write this article's related publications to the database if they haven't already been added.
                // (Multiple authors might link to the same publication, and each will add the same links.)
                int relatedCount = db.GetIntValue("SELECT Count(*) FROM " + relatedTable + " WHERE PMID = (?)",
                    new System.Collections.ArrayList() { Database.Parameter(authorPublicationPmid) });
                if (relatedCount != 0)
                {
                    Trace.WriteLine(DateTime.Now + " - [" + ++count + "/" + relatedSearchResults.Keys.Count + "] database already contains related articles for " + authorPublicationPmid);
                } 
                else
                {
                    // Get the list of related PMIDs and their ranks from the search results
                    List<int> relatedPmids = relatedSearchResults[authorPublicationPmid];
                    Dictionary<int, RankAndScore> relatedRank;
                    if (relatedRanks.ContainsKey(authorPublicationPmid))
                        relatedRank = relatedRanks[authorPublicationPmid];
                    else
                        relatedRank = new Dictionary<int, RankAndScore>();

                    Trace.WriteLine(DateTime.Now + " - [" + ++count + "/" + relatedSearchResults.Keys.Count + "] adding " + relatedPmids.Count + " related articles found for " + authorPublicationPmid);

                    string searchResults = SearchPubMedForRelatedPublications(relatedPmids);

                    int publicationsWritten = 0;
                    int publicationsExcluded = 0;
                    int publicationsNullAuthors = 0;

                    // Track the most relevant publication (eg. the one with the highest score) so it can be added to relatedpublications_extremerelevance
                    Publication? mostRelevantPublication = null;
                    int mostRelevantPublicationScore = int.MinValue;

                    // Track the least relevant publication (eg. the one with the highest score) for relatedpublications_leastrelevant and relatedpubliactions_leastrelevantscore
                    Publication? leastRelevantPublication = null;
                    int leastRelevantPublicationScore = int.MaxValue;
                    int leastRelevantPublicationRank = 0;

                    // Write each publication to the database
                    Publications publications = new Publications(searchResults, pubTypes);
                    foreach (Publication relatedPublication in publications.PublicationList)
                    {
                        if (BackgroundWorker != null && BackgroundWorker.CancellationPending)
                        {
                            Trace.WriteLine(DateTime.Now + " - cancelled");
                            return false;
                        }

                        int rank;
                        int score;
                        if (relatedRank.ContainsKey(relatedPublication.PMID))
                        {
                            rank = relatedRank[relatedPublication.PMID].Rank;
                            score = relatedRank[relatedPublication.PMID].Score;
                        }
                        else
                        {
                            rank = -1;
                            score = -1;
                            Trace.WriteLine(DateTime.Now + " - publication " + authorPublicationPmid + " could not find rank for related " + relatedPublication.PMID);
                        }

                        // A small number of publications come back with a null set of authors, which the database schema doesn't support 
                        if (relatedPublication.Authors == null)
                        {
                            publicationsNullAuthors++;
                            Trace.WriteLine(DateTime.Now + " - publication " + authorPublicationPmid + ": found related publication " + relatedPublication.PMID + " with no author list");
                        }

                        // Use the publication filter to include only publications that match the filter
                        if (publicationFilter.FilterPublication(relatedPublication, rank, authorPublication, pubTypes))
                        {
                            try
                            {
                                // Add the publication to the publications table
                                // (this will only add it if it's not already there)
                                Publications.WriteToDB(relatedPublication, db, pubTypes, null);

                                // Write the pmid/relatedPmid pair to the related publications table.

                                db.ExecuteNonQuery(
                                     "INSERT INTO " + relatedTable + " (PMID, RelatedPMID, Rank, Score) VALUES (?, ?, ?, ?)",
                                     new System.Collections.ArrayList() { 
                                            Database.Parameter(authorPublicationPmid), 
                                            Database.Parameter(relatedPublication.PMID),
                                            Database.Parameter(rank),
                                            Database.Parameter(score),
                                        });

                                publicationsWritten++;
                            }
                            catch (Exception ex)
                            {
                                Trace.WriteLine("Unable to add related article " + relatedPublication.PMID + ", error message follows");
                                Trace.WriteLine(ex.Message);

                                error = true;
                            }
                        }
                        else
                        {
                            publicationsExcluded++;
                        }

                        // We're keeping track of the score of the most relevant pub (even when it is filtered out).
                        if (!mostRelevantPublication.HasValue || score > mostRelevantPublicationScore)
                        {
                            mostRelevantPublication = relatedPublication;
                            mostRelevantPublicationScore = score;
                        }

                        // We're keeping track of the score of the least relevant pub too.
                        if (!leastRelevantPublication.HasValue || score < leastRelevantPublicationScore)
                        {
                            leastRelevantPublication = relatedPublication;
                            leastRelevantPublicationScore = score;
                            leastRelevantPublicationRank = rank;
                        }
                    }

                    // Write the most and least relevant pmid/relatedPmid pairs to the _extremerelevance table (if found).
                    if (mostRelevantPublication.HasValue && leastRelevantPublication.HasValue)
                    {
                        try
                        {
                            db.ExecuteNonQuery(
                                "INSERT INTO " + extremeRelevanceTableName + " (PMID, MostRelevantPMID, MostRelevantScore, LeastRelevantPMID, LeastRelevantScore, LeastRelevantRank) VALUES (?, ?, ?, ?, ?, ?)",
                                new System.Collections.ArrayList() { 
                                            Database.Parameter(authorPublicationPmid), 
                                            Database.Parameter(mostRelevantPublication.Value.PMID),
                                            Database.Parameter(mostRelevantPublicationScore),
                                            Database.Parameter(leastRelevantPublication.Value.PMID),
                                            Database.Parameter(leastRelevantPublicationScore),
                                            Database.Parameter(leastRelevantPublicationRank)
                                        });
                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLine(DateTime.Now + " - " +
                                String.Format("Error writing {0}/{1}/{2} to {3}: {4}",
                                authorPublicationPmid, mostRelevantPublication.Value.PMID, leastRelevantPublication.Value.PMID, extremeRelevanceTableName, ex.Message));

                            error = true;
                        }
                    }

                    Trace.WriteLine(DateTime.Now + " - " +
                        String.Format("Wrote {0}, excluded {1}{2}", publicationsWritten, publicationsExcluded,
                        publicationsNullAuthors == 0 ? String.Empty : ", " + publicationsNullAuthors + " had no author list"));
                }

                if (!error)
                    inputQueue.MarkProcessed(authorPublicationPmid);
                else
                    inputQueue.MarkError(authorPublicationPmid);
            } 

            return true;
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
        /// Create the related publications table and its PeoplePublications view
        /// </summary>
        /// <param name="tableName">Name of the talbe to create</param>
        /// <param name="queueTableName">Name of the _queue table created</param>
        /// <param name="extremeRelevanceTableName">Name of the _extremerelevance table created</param>
        /// <param name="liteMode">In "lite" mode, only create the related publications table, not the other tables</param>
        private static void CreateRelatedTable(Database db, string tableName, string queueTableName, string extremeRelevanceTableName, bool liteMode)
        {
            // Create the queue -- for both regular and "lite" modes
            db.ExecuteNonQuery("DROP TABLE IF EXISTS " + queueTableName);
            db.ExecuteNonQuery("CREATE TABLE " + queueTableName + @" (
                Setnb char(8) NOT NULL,
                PMID int(11) NOT NULL,
                Processed bit(1) default NULL,
                Error bit(1) default NULL,
                PRIMARY KEY (Setnb, PMID)
            ) ENGINE=MyISAM DEFAULT CHARSET=utf8;
            ");

            if (!liteMode)
            {
                // Create the related table
                db.ExecuteNonQuery("DROP TABLE IF EXISTS " + tableName);
                db.ExecuteNonQuery("CREATE TABLE " + tableName + @" (
                  PMID int(11) NOT NULL,
                  RelatedPMID int(11) NOT NULL,
                  Rank int NOT NULL,
                  Score int NOT NULL,
                  PRIMARY KEY (PMID, RelatedPMID)
                ) ENGINE=MyISAM DEFAULT CHARSET=utf8;
                ");

                // Create the view (table name + "_peoplepublications")
                db.ExecuteNonQuery("CREATE OR REPLACE VIEW " + tableName + @"_peoplepublications AS
                  SELECT p.Setnb, rp.RelatedPMID AS PMID, -1 AS AuthorPosition, 6 AS PositionType
                  FROM people p, peoplepublications pp, relatedpublications rp
                  WHERE p.Setnb = pp.Setnb
                  AND pp.PMID = rp.PMID;
                ");

                // Create the most/least relevant publications table (table name + "_extremerelevance")
                db.ExecuteNonQuery("DROP TABLE IF EXISTS " + extremeRelevanceTableName);
                db.ExecuteNonQuery("CREATE TABLE " + extremeRelevanceTableName + @" (
                  PMID int(11) NOT NULL,
                  MostRelevantPMID int(11) NOT NULL,
                  MostRelevantScore int NOT NULL,
                  LeastRelevantPMID int(11) NOT NULL,
                  LeastRelevantScore int NOT NULL,
                  LeastRelevantRank int NOT NULL,
                  PRIMARY KEY (PMID, MostRelevantPMID)
                ) ENGINE=MyISAM DEFAULT CHARSET=utf8;
                ");
            }
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

            // Add "&cmd=neighbor_score" to get the <Score> elements
            query.Append("&cmd=neighbor_score");
            
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
