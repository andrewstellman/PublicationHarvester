using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Com.StellmanGreene.PubMed;
using System.Data;
using System.IO;

namespace Com.StellmanGreene.SocialNetworking
{
    /// <summary>
    /// Generate report rows
    /// </summary>
    public class Report
    {
        private Database DB;
        private string FirstDegreeDB;
        private string SecondDegreeDB;
        private List<string> ColleaguesToInclude;
        private List<string> ColleaguesToExclude;

        public Report(Database DB, string FirstDegreeDB, string SecondDegreeDB, List<string> ColleaguesToInclude, List<string> ColleaguesToExclude)
        {
            this.DB = DB;
            this.FirstDegreeDB = FirstDegreeDB;
            this.SecondDegreeDB = SecondDegreeDB;

            this.ColleaguesToInclude = new List<string>();
            if (ColleaguesToInclude != null)
                foreach (string colleague in ColleaguesToInclude)
                    this.ColleaguesToInclude.Add(colleague.ToLower());

            this.ColleaguesToExclude = new List<string>();
            if (ColleaguesToExclude != null)
                foreach (string colleague in ColleaguesToExclude)
                    this.ColleaguesToExclude.Add(colleague.ToLower());
        }

        /// <summary>
        /// Callback function to return the status
        /// </summary>
        /// <param name="number">Number of colleagues processed</param>
        /// <param name="total">Total colleagues</param>
        /// <param name="Setnb">Setnb of the current colleague</param>
        /// <param name="Name">Name of the current colleague</param>
        public delegate void Status(int Number, int Total, string Setnb, string Name);


        /// <summary>
        /// Callback to give detailed progress
        /// </summary>
        /// <param name="FirstDegreeStar">Which first degree colleague is currently being processed</param>
        /// <param name="TotalFirstDegreeStars">Total number of first degree colleagues for the current person</param>
        /// <param name="SecondDegreeStar">Which second degree colleague is currently being processed</param>
        /// <param name="TotalSecondDegreeStars">Total number of second degree colleagues for the current first degree colleague</param>
        public delegate void DetailedProgress(int FirstDegree, int TotalFirstDegree, 
            int SecondDegree, int TotalSecondDegree);


        /// <summary>
        /// Callback function to write a subset of the table
        /// </summary>
        /// <param name="RowsToWrite">DataTable that contains the rows to write to the report file</param>
        public delegate void WriteRows(DataTable RowsToWrite);


        /// <summary>
        /// Generate the social networking report
        /// </summary>
        public void Generate(Status StatusCallback, WriteRows WriteRowsCallback, DetailedProgress DetailedProgressCallback)
        {
            // Look up each colleague in the database
            DataTable ColleagueRows = DB.ExecuteQuery("SELECT Setnb, First, Middle, Last FROM " + FirstDegreeDB + ".Colleagues ORDER BY Setnb");
            int RowCount = ColleagueRows.Rows.Count;
            for (int Row = 0; Row < RowCount; Row++)
            {
                // Get the social network for the colleague
                DataRow ColleagueRow = ColleagueRows.Rows[Row];

                if ((ColleaguesToInclude.Count == 0 || ColleaguesToInclude.Contains(ColleagueRow["Setnb"].ToString().ToLower()))
                    && (!ColleaguesToExclude.Contains(ColleagueRow["Setnb"].ToString().ToLower())))
                {

                    string Colleague = ColleagueRow["Setnb"].ToString();
                    Hashtable Network = GetSocialNetwork(Colleague);

                    // Send back status
                    string Name = ColleagueRow["First"].ToString();
                    if (ColleagueRow["Middle"].ToString().Trim() != "")
                        Name += " " + ColleagueRow["Middle"].ToString();
                    Name += " " + ColleagueRow["Last"].ToString();

                    if (StatusCallback != null)
                        StatusCallback(Row + 1, RowCount, Colleague, Name);

                    // The report must be in ascending order.
                    string[] Keys = new string[Network.Keys.Count];
                    Network.Keys.CopyTo(Keys, 0);
                    Array.Sort(Keys);

                    // Declare a DataTable to store the current subset of the report
                    DataTable Results = new DataTable();

                    // Generate the rows for each colleague and roll them onto the end of the report
                    for (int i = 0; i < Network.Keys.Count; i++)
                    {
                        // Get the second degere stars
                        string Star = Keys[i].ToString();
                        SecondDegreeStars secondDegreeStars = new SecondDegreeStars(DB, Colleague, FirstDegreeDB, Star, SecondDegreeDB);

                        // Make sure the second degere stars are processed in order
                        for (int j = 0; j < secondDegreeStars.Setnbs.Count; j++)
                        {
                            if (DetailedProgressCallback != null)
                                DetailedProgressCallback(i + 1, Network.Keys.Count + 1, j + 1, secondDegreeStars.Setnbs.Count + 1);

                            // Roll the report rows onto the end of the report
                            string SecondDegree = secondDegreeStars.Setnbs[j].ToString();
                            Results = RollUpReportRows(Results, RowsForColleagueStarSecondDegree(Colleague, Star, SecondDegree));
                        }

                        if (DetailedProgressCallback != null)
                            DetailedProgressCallback(Network.Keys.Count + 1, Network.Keys.Count + 1, 
                                secondDegreeStars.Setnbs.Count + 1, secondDegreeStars.Setnbs.Count + 1);
                    }

                    // Write the report rows
                    if (WriteRowsCallback != null)
                        WriteRowsCallback(Results);
                }
            }
        }



        /// <summary>
        /// Write the header row for the report
        /// </summary>
        /// <param name="Report">Report to write</param>
        /// <param name="Writer">StreamWriter that's writing the report</param>
        public static void WriteHeader(DataTable Report, StreamWriter Writer)
        {
            // Write the column names
            for (int Col = 0; Col < Report.Columns.Count; Col++)
            {
                if (Col > 0)
                    Writer.Write(",");
                string Column = Report.Columns[Col].ToString();
                if (Column.Contains("\"") || Column.Contains(","))
                {
                    Column = "\"" + Column.Replace("\"", "\"\"") + "\"";
                }
                Writer.Write(Column);
            }
            Writer.WriteLine();
        }


        /// <summary>
        /// Write a report to a CSV file
        /// </summary>
        /// <param name="Report">DataTable containing the report</param>
        /// <param name="Path">File to write</param>
        public static void WriteCSV(DataTable Report, StreamWriter Writer)
        {
            // Loop through each row in the report
            for (int RowNum = 0; RowNum < Report.Rows.Count; RowNum++)
            {
                DataRow Row = Report.Rows[RowNum];
                for (int Col = 0; Col < Report.Columns.Count; Col++)
                {
                    if (Col > 0)
                        Writer.Write(",");
                    string Column = Row[Col].ToString();
                    if (Column.Contains("\""))
                    {
                        Column = "\"" + Column.Replace("\"", "\"\"") + "\"";
                    }
                    Writer.Write(Column);
                }
                Writer.WriteLine();
            }

            return;
        }

        /// <summary>
        /// Given a colleague's setnb, retrieve the social network for each of the associated 1st and 2nd degree stars
        /// </summary>
        /// <param name="ColleagueSetnb">Colleague to retrieve</param>
        /// <returns>Hashtable that maps 1st degree star Setnbs to SecondDegreeStars objects</returns>
        public Hashtable GetSocialNetwork(string ColleagueSetnb)
        {
            // Create the hashtable to return
            Hashtable Network = new Hashtable();

            // Get the first degree stars -- make sure they're sorted in ascending order
            ArrayList Parameters = new ArrayList();
            Parameters.Add(Database.Parameter(ColleagueSetnb));
            DataTable FirstDegreeStars = DB.ExecuteQuery("SELECT DISTINCT StarSetnb FROM " + FirstDegreeDB + ".StarColleagues WHERE Setnb = ? ORDER BY Setnb ASC", Parameters);

            // Add each row to the end of the report.
            for (int Row = 0; Row < FirstDegreeStars.Rows.Count; Row++)
            {
                DataRow StarRow = FirstDegreeStars.Rows[Row];
                string StarSetnb = StarRow["StarSetnb"].ToString();
                Network.Add(StarSetnb, new SecondDegreeStars(DB, ColleagueSetnb, FirstDegreeDB, StarSetnb, SecondDegreeDB));
            }

            // Return the hashtable
            return Network;
        }


        /// <summary>
        /// Roll up two datatables containing partial reports into one datatable
        /// </summary>
        /// <param name="Results1">DataTable that contains the first part of the report</param>
        /// <param name="Results2">DataTable that contains the second part of the report</param>
        /// <returns></returns>
        public static DataTable RollUpReportRows(DataTable Results1, DataTable Results2)
        {
            // Create the DataTable to return
            DataTable Results = null;
            if (Results1 != null && Results1.Rows.Count > 0)
                Results = Results1.Clone();
            else if (Results2 != null && Results2.Rows.Count > 0)
                Results = Results2.Clone();
            else if (Results1 != null)
                return Results1;
            else
                return Results2;
    

            // Add the rows from Results1
            if (Results1 != null && Results1.Rows.Count > 0)
            {
                for (int Row = 0; Row < Results1.Rows.Count; Row++)
                    Results.Rows.Add(Results1.Rows[Row].ItemArray);
            }

            // Add the rows from Results2
            if (Results2 != null && Results2.Rows.Count > 0)
            {
                for (int Row = 0; Row < Results2.Rows.Count; Row++)
                {
                    Results.Rows.Add(Results2.Rows[Row].ItemArray);
                }
            }

            return Results;
        }

        /// <summary>
        /// Generate the report rows for a colleague, star and second degree star
        /// </summary>
        /// <param name="Colleague">Setnb of the colleague</param>
        /// <param name="Star">Setnb of the star</param>
        /// <param name="SecondDegree">Setnb of the second degree star</param>
        /// <returns>DataTable that contains the report rows</returns>
        public DataTable RowsForColleagueStarSecondDegree(string Colleague, string Star, string SecondDegree)
        {
            // create the rows for each pair of star / second degree star
            //    - get the two sets of common publications
            //    - find the earliest and latest year between the two sets
            //    - for each year, create the individual row, keeping the running totals
            //    - add the rows to the output datatable 

            // Create the DataTable to return results
            DataTable Results = new DataTable();
            Results.Columns.Add("setnb0", Type.GetType("System.String"));
            Results.Columns.Add("setnb1", Type.GetType("System.String"));
            Results.Columns.Add("setnb2", Type.GetType("System.String"));
            Results.Columns.Add("year", Type.GetType("System.Int32"));
            Results.Columns.Add("flow0to1", Type.GetType("System.Int32"));
            Results.Columns.Add("stk0to1", Type.GetType("System.Int32"));
            Results.Columns.Add("flow1to2", Type.GetType("System.Int32"));
            Results.Columns.Add("stk1to2", Type.GetType("System.Int32"));
            Results.Columns.Add("flow0to2", Type.GetType("System.Int32"));
            Results.Columns.Add("stk0to2", Type.GetType("System.Int32"));


            // Find the common publicatinos between the colleague and star
            Hashtable Common0to1 = CommonPublications.Find(Colleague, FirstDegreeDB, false, Star, SecondDegreeDB, true, DB);
            int FirstYear0to1 = CommonPublications.EarliestYear(Common0to1);
            int LastYear0to1 = CommonPublications.LatestYear(Common0to1);

            // Find the common publicatinos between the star and 2nd degree star
            Hashtable Common1to2 = CommonPublications.Find(Star, SecondDegreeDB, true, SecondDegree, SecondDegreeDB, false, DB);
            int FirstYear1to2 = CommonPublications.EarliestYear(Common1to2);
            int LastYear1to2 = CommonPublications.LatestYear(Common1to2);

            // Find the common publicatinos between the colleague and 2nd degree star
            Hashtable Common0to2 = CommonPublications.Find(Colleague, FirstDegreeDB, false, SecondDegree, SecondDegreeDB, false, DB);
            int FirstYear0to2 = CommonPublications.EarliestYear(Common0to2);
            int LastYear0to2 = CommonPublications.LatestYear(Common0to2);

            // Find the earliest year of all common publications
            int FirstYear = FirstYear0to1;
            if (FirstYear == 0 || (FirstYear1to2 > 0 && FirstYear1to2 < FirstYear))
                FirstYear = FirstYear1to2;
            if (FirstYear == 0 || (FirstYear0to2 > 0 && FirstYear0to2 < FirstYear))
                FirstYear = FirstYear0to2;

            // Find the latest year of all common publications
            int LastYear = LastYear0to1;
            if (LastYear1to2 > LastYear)
                LastYear = LastYear1to2;
            if (LastYear0to2 > LastYear)
                LastYear = LastYear0to2;

            // Populate each row
            int stk0to1 = 0;
            int stk1to2 = 0;
            int stk0to2 = 0;
            for (int Year = FirstYear; Year <= LastYear; Year++)
            {
                // Create the row and add the names
                DataRow Row = Results.NewRow();
                Row["setnb0"] = Colleague;
                Row["setnb1"] = Star;
                Row["setnb2"] = SecondDegree;
                Row["year"] = Year;

                // Values for colleague/star
                if (FirstYear0to1 > 0 && FirstYear0to1 <= Year && Year <= LastYear0to1)
                {
                    Row["flow0to1"] = Common0to1[Year];
                    stk0to1 += (int)Common0to1[Year];
                }
                else
                    Row["flow0to1"] = 0;
                Row["stk0to1"] = stk0to1;

                // Values for star/2nd
                if (FirstYear1to2 > 0 && FirstYear1to2 <= Year && Year <= LastYear1to2)
                {
                    Row["flow1to2"] = Common1to2[Year];
                    stk1to2 += (int)Common1to2[Year];
                }
                else
                    Row["flow1to2"] = 0;
                Row["stk1to2"] = stk1to2;

                // Values for colleague/2nd
                if (FirstYear0to2 > 0 && FirstYear0to2 <= Year && Year <= LastYear0to2)
                {
                    Row["flow0to2"] = Common0to2[Year];
                    stk0to2 += (int)Common0to2[Year];
                }
                else
                    Row["flow0to2"] = 0;
                Row["stk0to2"] = stk0to2;

                // Add the row to the results
                Results.Rows.Add(Row);
            }

            return Results;
        }

    }
}
