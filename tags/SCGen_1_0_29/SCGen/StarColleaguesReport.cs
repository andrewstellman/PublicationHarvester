using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;
using System.Data;
using Com.StellmanGreene.PubMed;

namespace SCGen
{
    /// <summary>
    /// Class to generate the Star Colleagues Report
    /// </summary>
    class StarColleaguesReport
    {

        public static string[] ReportColumns = { 
                "1L", "L1", "1M", "M1", "MM", "LM", "ML", "21", "12", "2M", "M2", 
                "2L", "L2", "2NTL", "NTL2", "NTL1", "1NTL", "NTLM", "MNTL", 
                "NTLL", "LNTL"
            };
        /// <summary>
        /// Generate the Star Colleagues report
        /// </summary>
        public static void Generate(TextWriter writer, Database DB, string JournalWeightsFilename, ArrayList SetnbsToSkip, ReportsDialog ParentForm, bool WriteHeaderRow)
        {
            Reports reports = new Reports(DB, JournalWeightsFilename);

            if (WriteHeaderRow)
                WriteHeaderRowToReport(writer);

            // Retrieve all of the star/colleague pairs from the StarColleagues table.
            // The report is generated colleague by colleague (so that it's sorted by
            // the first column -- for the fault tolerance).
            DataTable ColleagueStarPairs = DB.ExecuteQuery("SELECT Setnb, StarSetnb FROM StarColleagues ORDER BY Setnb, StarSetnb");
            if (ColleagueStarPairs.Rows.Count == 0)
            {
                if (ParentForm != null)
                    ParentForm.AddLogEntry("Unable to generate StarColleagues report: No colleagues found!");
                return;
            }

            // Go through the publications and collect the counts. 
            Hashtable CountsPerYear = new Hashtable();
            Hashtable WeightsPerYear = new Hashtable();

            // Create hashtables for the four global counts for each row
            Hashtable Nbcoauth1 = new Hashtable();
            Hashtable Wghtd_Nbcoauth1 = new Hashtable();
            Hashtable Nbcoauth2 = new Hashtable();
            Hashtable Wghtd_Nbcoauth2 = new Hashtable();

            // For each star/colleague pair, produce the report rows.
            // Note that report rows are only produced for years where there
            // are collaborations.
            int Pairs = ColleagueStarPairs.Rows.Count;
            for (int Row = 0; Row < Pairs; Row++)
            {
                string Setnb = ColleagueStarPairs.Rows[Row]["Setnb"].ToString();
                string StarSetnb = ColleagueStarPairs.Rows[Row]["StarSetnb"].ToString();
                
                // If this is a continuation of a previous report, SetnbsToSkip will be 
                // populated with colleague Setnb values from the rows that were already
                // in the file.
                if (!SetnbsToSkip.Contains(Setnb))
                {
                    // Generate the row for the colleague

                    /*
                     * This query retrieves all of the publications that a star and colleague
                     * have in common. It takes two parameters, the star setnb and the
                     * colleague setnb, and returns all of the data necessary to produce the
                     * Star Colleagues Report rows for that star/colleague pair. It's sorted
                     * by year so that it can be processed efficiently, and so that the first
                     * and last rows in the results can be used to find the years of first and
                     * last collaboration.
                     * 
                     * SELECT p.Year, p.PMID, pp.PositionType AS StarPositionType, 
                     *        cp.PositionType AS ColleaguePositionType, p.Journal
                     *   FROM Publications p, ColleaguePublications cp, PeoplePublications pp
                     *  WHERE pp.Setnb = ?
                     *    AND cp.Setnb = ?
                     *    AND p.PMID = pp.PMID
                     *    AND p.PMID = cp.PMID
                     *  ORDER BY p.Year ASC
                     * 
                     */

                    ArrayList Parameters = new ArrayList();
                    Parameters.Add(Database.Parameter(StarSetnb));
                    Parameters.Add(Database.Parameter(Setnb));
                    using (DataTable PubData =
                        DB.ExecuteQuery(
                        @"SELECT p.Year, p.PMID, pp.PositionType AS StarPositionType, 
                                 cp.PositionType AS ColleaguePositionType, p.Journal
                            FROM Publications p, ColleaguePublications cp, PeoplePublications pp
                           WHERE pp.Setnb = ?
                             AND cp.Setnb = ?
                             AND p.PMID = pp.PMID
                             AND p.PMID = cp.PMID
                           ORDER BY p.Year ASC", Parameters))
                    {
                        if (PubData.Rows.Count == 0)
                        {
                            if (ParentForm != null)
                                ParentForm.AddLogEntry("No publications found for colleague " + Setnb + ", star " + StarSetnb);
                        }
                        else
                        {
                            // Get the first and last years of collaboration
                            int FirstCollabYear = Convert.ToInt32(PubData.Rows[0]["Year"]);
                            int LastCollabYear = Convert.ToInt32(PubData.Rows[PubData.Rows.Count - 1]["Year"]);

                            // Go through the publications and collect the counts. 
                            CountsPerYear.Clear();
                            WeightsPerYear.Clear();

                            // Create hashtables for the four global counts for each row
                            Nbcoauth1.Clear();
                            Wghtd_Nbcoauth1.Clear();
                            Nbcoauth2.Clear();
                            Wghtd_Nbcoauth2.Clear();
                            
                            for (int RowNum = 0; RowNum < PubData.Rows.Count; RowNum++)
                            {
                                // Get the information about the publication from the dataset
                                DataRow PubRow = PubData.Rows[RowNum];
                                UpdateCounts(reports, PubRow, CountsPerYear, WeightsPerYear,
                                    Nbcoauth1, Wghtd_Nbcoauth1, Nbcoauth2, Wghtd_Nbcoauth2);
                            }

                            // Write the rows to the report
                            int RowsWritten = WriteReportrows(Setnb, StarSetnb, FirstCollabYear, LastCollabYear,
                                Nbcoauth1, Wghtd_Nbcoauth1, Nbcoauth2, Wghtd_Nbcoauth2,
                                writer, CountsPerYear, WeightsPerYear);

                            //ParentForm.AddLogEntry("Wrote " + RowsWritten.ToString() + " rows for colleague " + Setnb + ", star " + StarSetnb + " (" + Row.ToString() + " of " + Pairs.ToString() + ")");
                            if (ParentForm != null)
                                ParentForm.SetProgressBar(0, Pairs, Row);
                        }
                    }

                }
            }

            
            

        }


        /// <summary>
        /// Write the header row to the report
        /// </summary>
        /// <param name="writer">TextWriter to write to</param>
        private static void WriteHeaderRowToReport(TextWriter writer)
        {
            writer.Write("setnb,star_setnb,year,Nbcoauth1,Wghtd_Nbcoauth1,Nbcoauth2,Wghtd_Nbcoauth2,");
            for (int Column = 0; Column < ReportColumns.Length; Column++)
            {
                writer.Write("Nbcoauth_" + ReportColumns[Column] + ",Wghtd_Nbcoauth_" + ReportColumns[Column] + ",");
            }
            writer.WriteLine("Frst_collab_year,Last_collab_year");
        }


        /// <summary>
        /// Updated the counts for a publication, given a row from the query in Generate().
        /// The counts are collected in a hashtable indexed by year. Each item in the 
        /// hashtable is another hashtable, indexed by the report column category 
        /// ("NTL2", "LM", etc.). This in turn contains the count. There are two master 
        /// hashtables of these counts, one for weighted and the other for unweighted.
        /// </summary>
        /// <param name="reports">Reports object initialized with journal weights</param>
        /// <param name="Row">Row from the query in Generate()</param>
        /// <param name="CountsPerYear">Hashtable that contains the counts for each year</param>
        /// <param name="WeightsPerYear">Hashtable that containst he weights for each year</param>
        private static void UpdateCounts(Reports reports, DataRow Row, Hashtable CountsPerYear, Hashtable WeightsPerYear,
            Hashtable Nbcoauth1, Hashtable Wghtd_Nbcoauth1, Hashtable Nbcoauth2, Hashtable Wghtd_Nbcoauth2)
        {
            int Year = Convert.ToInt32(Row["Year"]);
            Harvester.AuthorPositions ColleaguePositionType = 
                (Harvester.AuthorPositions) Convert.ToInt32(Row["ColleaguePositionType"]);
            Harvester.AuthorPositions StarPositionType = 
                (Harvester.AuthorPositions) Convert.ToInt32(Row["StarPositionType"]);
            string Journal = Row["Journal"].ToString();

            // Use the Reports object to get the publication's journal weight.
            Single Weight;
            if (reports.Weights.ContainsKey(Journal))
                Weight = (Single)reports.Weights[Journal];
            else
                Weight = 0;

            // Create or retrieve the correct counts hashtable
            if (!CountsPerYear.ContainsKey(Year))
                CountsPerYear[Year] = new Hashtable();
            Hashtable Counts = (Hashtable) CountsPerYear[Year];

            // Create or retrieve the correct weights hashtables
            if (!WeightsPerYear.ContainsKey(Year))
                WeightsPerYear[Year] = new Hashtable();
            Hashtable Weights = (Hashtable) WeightsPerYear[Year];

            // Update Nbcoauth1 and Wghtd_Nbcoauth1 for every publication
            if (!Nbcoauth1.ContainsKey(Year))
                Nbcoauth1[Year] = (int)0;
            int Nbcoauth1_value = (int) Nbcoauth1[Year];
            Nbcoauth1_value ++;
            Nbcoauth1[Year] = Nbcoauth1_value;
            if (!Wghtd_Nbcoauth1.ContainsKey(Year))
                Wghtd_Nbcoauth1[Year] = (Single)0.0;
            Single Wghtd_Nbcoauth1_value = (Single)Wghtd_Nbcoauth1[Year];
            Wghtd_Nbcoauth1_value += Weight;
            Wghtd_Nbcoauth1[Year] = Wghtd_Nbcoauth1_value;


            // Update Nbcoauth2 and Wghtd_Nbcoauth2 for publications where either
            // the star or colleague are first or last
            if ((ColleaguePositionType == Harvester.AuthorPositions.First)
                || (ColleaguePositionType == Harvester.AuthorPositions.Last)
                || (StarPositionType == Harvester.AuthorPositions.First)
                || (StarPositionType == Harvester.AuthorPositions.Last))
            {
                if (!Nbcoauth2.ContainsKey(Year))
                    Nbcoauth2[Year] = (int)0;
                int Nbcoauth2_value = (int)Nbcoauth2[Year];
                Nbcoauth2_value++;
                Nbcoauth2[Year] = Nbcoauth2_value;
                if (!Wghtd_Nbcoauth2.ContainsKey(Year))
                    Wghtd_Nbcoauth2[Year] = (Single)0.0;
                Single Wghtd_Nbcoauth2_value = (Single)Wghtd_Nbcoauth2[Year];
                Wghtd_Nbcoauth2_value += Weight;
                Wghtd_Nbcoauth2[Year] = Wghtd_Nbcoauth2_value;
            }
            
            // Update the count and weight for each column
            for (int ColumnNum = 0; ColumnNum < ReportColumns.Length; ColumnNum++)
            {
                // Figure out which colleague position needs to be counted for the column.
                // The column will indicate which articles are to be counted, based
                // on author position: 1 (first), L (last), 2 (seond), NTL (next-to-last),
                // M (middle). The column starts with the colleague value, and ends
                // with the star value.
                string Column = ReportColumns[ColumnNum];
                Harvester.AuthorPositions ColleagueExpected;
                if (Column.StartsWith("NTL")) // look for NTL first to disambiguate from L
                    ColleagueExpected = Harvester.AuthorPositions.NextToLast;
                else if (Column.StartsWith("1"))
                    ColleagueExpected = Harvester.AuthorPositions.First;
                else if (Column.StartsWith("L"))
                    ColleagueExpected = Harvester.AuthorPositions.Last;
                else if (Column.StartsWith("2"))
                    ColleagueExpected = Harvester.AuthorPositions.Second;
                else
                    ColleagueExpected = Harvester.AuthorPositions.Middle;

                // Figure out which star position needs to be counted for the column.
                Harvester.AuthorPositions StarExpected;
                if (Column.EndsWith("NTL")) // look for NTL first to disambiguate from L
                    StarExpected = Harvester.AuthorPositions.NextToLast;
                else if (Column.EndsWith("1"))
                    StarExpected = Harvester.AuthorPositions.First;
                else if (Column.EndsWith("L"))
                    StarExpected = Harvester.AuthorPositions.Last;
                else if (Column.EndsWith("2"))
                    StarExpected = Harvester.AuthorPositions.Second;
                else
                    StarExpected = Harvester.AuthorPositions.Middle;


                // Only the weight to the correct hash for the star and colleague 
                // position types.
                if ((ColleaguePositionType == ColleagueExpected)
                    && (StarPositionType == StarExpected))
                {
                    // Retrieve or initialize the Counts and Weights hashes
                    if (!Counts.ContainsKey(Column))
                        Counts[Column] = (int) 0;
                    if (!Weights.ContainsKey(Column))
                        Weights[Column] = (Single) 0.0;

                    // Update the count and weight.
                    int ColumnCount = (int) Counts[Column];
                    ColumnCount ++;
                    Counts[Column] = ColumnCount;

                    Single ColumnWeight = (Single) Weights[Column];
                    ColumnWeight += Weight;
                    Weights[Column] = ColumnWeight;
                }

            }

            // Save the updated hashtables
            CountsPerYear[Year] = Counts;
            WeightsPerYear[Year] = Weights;
        }


        /// <summary>
        /// Write the report rows for a set of publications
        /// </summary>
        /// <param name="writer">Writer to add the report rows to</param>
        /// <param name="CountsPerYear">Hashtable that contains the counts</param>
        /// <param name="WeightsPerYear">Hashtable that contains the weights</param>
        /// <returns>Number of rows written to the report</returns>
        public static int WriteReportrows(string Setnb, string StarSetnb, int FirstCollabYear, int LastCollabYear,
            Hashtable Nbcoauth1, Hashtable Wghtd_Nbcoauth1, Hashtable Nbcoauth2, Hashtable Wghtd_Nbcoauth2,
            TextWriter writer, Hashtable CountsPerYear, Hashtable WeightsPerYear)
        {
            int RowsWritten = 0;

            // The report contains one row per year for which there are publications.
            // The hashes are keyed by year, so they can be retrieved from the keys.
            int[] Years = new int[CountsPerYear.Keys.Count];
            CountsPerYear.Keys.CopyTo(Years, 0);
            Array.Sort(Years);

            for (int i = 0; i < Years.Length; i++)
            {
                // The CountsPerYear and WeightsPerYear hashtables are keyed on year.
                // They contain hashtables keyed on the column name from ReportsColumns
                // and contain the counts and weights.
                int Year = Years[i];
                Hashtable Counts = (Hashtable)CountsPerYear[Year];
                Hashtable Weights = (Hashtable)WeightsPerYear[Year];

                writer.Write(Setnb + "," + StarSetnb + "," + Year.ToString() + ",");


                // Write Nbcoauth1, Wghtd_Nbcoath1, Nbcoauth2, Wghtd_Nbcoath2
                writer.Write(HashValueOrZero(Nbcoauth1, Year) + ",");
                writer.Write(HashValueOrZero(Wghtd_Nbcoauth1, Year) + ",");
                writer.Write(HashValueOrZero(Nbcoauth2, Year) + ",");
                writer.Write(HashValueOrZero(Wghtd_Nbcoauth2, Year) + ",");

                for (int j = 0; j < ReportColumns.Length; j++)
                {
                    string Column = ReportColumns[j];

                    // Write the count for the column
                    writer.Write(HashValueOrZero(Counts, Column));
                    writer.Write(",");

                    // Write the weight for the column
                    writer.Write(HashValueOrZero(Weights, Column));
                    writer.Write(",");
                }

                writer.WriteLine(FirstCollabYear.ToString() + "," + LastCollabYear.ToString());
                RowsWritten++;
            }

            return RowsWritten;
        }


        /// <summary>
        /// Look up the key in the hashtable, return the string value (or "0" if 
        /// the hashtable doesn't contain the key)
        /// </summary>
        /// <param name="hashtable">Hashtable to look up the value</param>
        /// <param name="Key">Key to look up</param>
        /// <returns></returns>
        private static string HashValueOrZero(Hashtable hashtable, object Key)
        {
            if (hashtable.ContainsKey(Key))
                return hashtable[Key].ToString();
            else
                return "0";
        }

    }
}
