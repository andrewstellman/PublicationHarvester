using System;
using System.Collections.Generic;
using System.Text;
using Com.StellmanGreene.SocialNetworking;
using System.Collections;
using System.Data;
using System.IO;
using NUnit.Framework;
using Com.StellmanGreene.PubMed;


namespace Com.StellmanGreene.SocialNetworking.Unit_Tests
{
    /// <summary>
    /// Test the Report class
    /// </summary>
    [TestFixture]
    public class TestReport
    {
        private Database DB;

        [TestFixtureSetUp]
        public void TestReportSetUp()
        {
            UnitTestData.Create();
            DB = UnitTestData.GetDB();
        }

        [Test]
        public void TestRowsForAliceCarolLisa()
        {
            /* Alice, Carol and Lisa have the following publications in common:
             * 
             * Alice and Carol: 1991 x 1	1993 x 1	1994 x 5	1995 x 1
             * Carol and Lisa:  1996 x 1	1997 x 1	1999 x 3
             * Alice and Lisa:  None
             * 
             * The report rows for them should have the following values:
             * year flow0to1    stk0to1 flow1to2    stk1to2 flow0to2    stk0to2
             * 1991 1           1       0           0       0           0
             * 1992 0           1       0           0       0           0
             * 1993 1           2       0           0       0           0
             * 1994 5           7       0           0       0           0
             * 1995 1           8       0           0       0           0
             * 1996 0           8       1           1       0           0
             * 1997 0           8       1           2       0           0
             * 1998 0           8       0           2       0           0
             * 1999 0           8       3           5       0           0
             */

            int[][] Expected = { 
                new int[] { 1991, 1, 1, 0, 0, 0, 0 },
                new int[] { 1992, 0, 1, 0, 0, 0, 0 },
                new int[] { 1993, 1, 2, 0, 0, 0, 0 },
                new int[] { 1994, 5, 7, 0, 0, 0, 0 },
                new int[] { 1995, 1, 8, 0, 0, 0, 0 },
                new int[] { 1996, 0, 8, 1, 1, 0, 0 },
                new int[] { 1997, 0, 8, 1, 2, 0, 0 },
                new int[] { 1998, 0, 8, 0, 2, 0, 0 },
                new int[] { 1999, 0, 8, 3, 5, 0, 0 },
            };
            Report report = new Report(DB, "social_unit_test_firstdegree", "social_unit_test_seconddegree", null, null);
            DataTable Results = report.RowsForColleagueStarSecondDegree("Alice", "Carol", "Lisa");
            VerifyColumnNames(Results);
            CheckResults(Results, Expected, "Alice", "Carol", "Lisa");
        }


        [Test]
        public void TestRowsForAliceJohnJustin()
        {
            /* Alice, Carol and Lisa have the following publications in common:
             * 
             * Alice and Justin: 1994 x 2	1995 x 1	2003 x 1
             * John and Justn:   1996 x 1	1999 x 1	2003 x 1	2005 x 2
             * Alice and John:   2002 x 1	2003 x 1
             * 
             * The report rows for them should have the following values:
             * year flow0to1    stk0to1 flow1to2    stk1to2 flow0to2    stk0to2
             * 1994 2           2       0           0       0           0
             * 1995 1           3       0           0       0           0
             * 1996 0           3       1           1       0           0
             * 1997 0           3       0           1       0           0
             * 1998 0           3       0           1       0           0
             * 1999 0           3       1           2       0           0
             * 2000 0           3       0           2       0           0
             * 2001 0           3       0           2       0           0
             * 2002 0           3       0           2       1           1
             * 2003 1           4       1           3       1           2
             * 2004 0           4       0           3       0           2
             * 2005 0           4       2           5       0           2
             */

            int[][] Expected = { 
                new int[] { 1994, 2, 2, 0, 0, 0, 0 },
                new int[] { 1995, 1, 3, 0, 0, 0, 0 },
                new int[] { 1996, 0, 3, 1, 1, 0, 0 },
                new int[] { 1997, 0, 3, 0, 1, 0, 0 },
                new int[] { 1998, 0, 3, 0, 1, 0, 0 },
                new int[] { 1999, 0, 3, 1, 2, 0, 0 },
                new int[] { 2000, 0, 3, 0, 2, 0, 0 },
                new int[] { 2001, 0, 3, 0, 2, 0, 0 },
                new int[] { 2002, 0, 3, 0, 2, 1, 1 },
                new int[] { 2003, 1, 4, 1, 3, 1, 2 },
                new int[] { 2004, 0, 4, 0, 3, 0, 2 },
                new int[] { 2005, 0, 4, 2, 5, 0, 2 },
            };
            Report report = new Report(DB, "social_unit_test_firstdegree", "social_unit_test_seconddegree", null, null);
            DataTable Results = report.RowsForColleagueStarSecondDegree("Alice", "Justin", "John");
            VerifyColumnNames(Results);
            CheckResults(Results, Expected, "Alice", "Justin", "John");
        }

        /// <summary>
        /// Verify the results against an array of expected values
        /// </summary>
        /// <param name="Results">Results to check</param>
        /// <param name="Expected">Expected values</param>
        /// <param name="Colleague">Colleague Sentb</param>
        /// <param name="Star">Star Setnb</param>
        /// <param name="SecondDegree">2nd degree star Setnb</param>
        private static void CheckResults(DataTable Results, int[][] Expected, string Colleague, string Star, string SecondDegree)
        {
            Assert.AreEqual(Results.Rows.Count, Expected.Length);
            CheckResultRows(Results, Expected, Colleague, Star, SecondDegree, 1, Results.Rows.Count);
        }


        /// <summary>
        /// Verify the results against an certain rows array of expected values
        /// </summary>
        /// <param name="Results">Results to check</param>
        /// <param name="Expected">Expected values</param>
        /// <param name="Colleague">Colleague Sentb</param>
        /// <param name="Star">Star Setnb</param>
        /// <param name="SecondDegree">2nd degree star Setnb</param>
        /// <param name="FirstRow">First row to check</param>
        /// <param name="LastRow">Last row to check</param>
        private static void CheckResultRows(DataTable Results, int[][] Expected, string Colleague, string Star, string SecondDegree,
            int FirstRow, int LastRow)
        {
            for (int Row = FirstRow - 1; Row < LastRow; Row++)
            {
                Assert.AreEqual(Results.Rows[Row][0].ToString(), Colleague);
                Assert.AreEqual(Results.Rows[Row][1].ToString(), Star);
                Assert.AreEqual(Results.Rows[Row][2].ToString(), SecondDegree);
                for (int Col = 3; Col < 10; Col++)
                {
                    Assert.AreEqual((int)Results.Rows[Row][Col],
                        (int)Expected[Row][Col - 3],
                        "row = " + Row.ToString() + ", col = " + Col.ToString());
                }
            }
        }





        /// <summary>
        /// Verify the column names for report
        /// </summary>
        /// <param name="Results">DataTable that contains the report</param>
        private static void VerifyColumnNames(DataTable Results)
        {
            // Verify the column names
            Assert.AreEqual(Results.Columns.Count, 10);
            Assert.AreEqual(Results.Columns[0].ToString(), "setnb0");
            Assert.AreEqual(Results.Columns[1].ToString(), "setnb1");
            Assert.AreEqual(Results.Columns[2].ToString(), "setnb2");
            Assert.AreEqual(Results.Columns[3].ToString(), "year");
            Assert.AreEqual(Results.Columns[4].ToString(), "flow0to1");
            Assert.AreEqual(Results.Columns[5].ToString(), "stk0to1");
            Assert.AreEqual(Results.Columns[6].ToString(), "flow1to2");
            Assert.AreEqual(Results.Columns[7].ToString(), "stk1to2");
            Assert.AreEqual(Results.Columns[8].ToString(), "flow0to2");
            Assert.AreEqual(Results.Columns[9].ToString(), "stk0to2");
        }


        /// <summary>
        /// Test Report.RollUpReportRows()
        /// </summary>
        [Test]
        public void TestRollUp()
        {
            // Create the first DataTable to roll up using data from TestRowsFromAliceCarolLisa()
            Report report = new Report(DB, "social_unit_test_firstdegree", "social_unit_test_seconddegree", null, null);
            DataTable Results1 = report.RowsForColleagueStarSecondDegree("Alice", "Justin", "John");

            // Create the second DataTable to roll up using data from TestRowsFromAliceJustinJohn()
            DataTable Results2 = report.RowsForColleagueStarSecondDegree("Alice", "Carol", "Lisa");

            // Roll up the results
            DataTable Results = Report.RollUpReportRows(Results1, Results2);

            // Verify the expected results, which concatenate the results from the two tests
            int[][] Expected = { 
                new int[] { 1994, 2, 2, 0, 0, 0, 0 }, // Rows 1 to 12 are for Alice/Justin/John
                new int[] { 1995, 1, 3, 0, 0, 0, 0 },
                new int[] { 1996, 0, 3, 1, 1, 0, 0 },
                new int[] { 1997, 0, 3, 0, 1, 0, 0 },
                new int[] { 1998, 0, 3, 0, 1, 0, 0 },
                new int[] { 1999, 0, 3, 1, 2, 0, 0 },
                new int[] { 2000, 0, 3, 0, 2, 0, 0 },
                new int[] { 2001, 0, 3, 0, 2, 0, 0 },
                new int[] { 2002, 0, 3, 0, 2, 1, 1 },
                new int[] { 2003, 1, 4, 1, 3, 1, 2 },
                new int[] { 2004, 0, 4, 0, 3, 0, 2 },
                new int[] { 2005, 0, 4, 2, 5, 0, 2 },
                new int[] { 1991, 1, 1, 0, 0, 0, 0 }, // Rows 13 to 21 are for Alice/Carol/Lisa
                new int[] { 1992, 0, 1, 0, 0, 0, 0 },
                new int[] { 1993, 1, 2, 0, 0, 0, 0 },
                new int[] { 1994, 5, 7, 0, 0, 0, 0 },
                new int[] { 1995, 1, 8, 0, 0, 0, 0 },
                new int[] { 1996, 0, 8, 1, 1, 0, 0 },
                new int[] { 1997, 0, 8, 1, 2, 0, 0 },
                new int[] { 1998, 0, 8, 0, 2, 0, 0 },
                new int[] { 1999, 0, 8, 3, 5, 0, 0 },
            };
            VerifyColumnNames(Results);
            CheckResultRows(Results, Expected, "Alice", "Justin", "John", 1, 12);
            CheckResultRows(Results, Expected, "Alice", "Carol", "Lisa", 13, 21);
        }


        /// <summary>
        /// Verify that Report.RollUpReportRows() works with an empty Report1 object
        /// </summary>
        [Test]
        public void TestRollUpWithEmptyReport1()
        {
            int[][] Expected = { 
                new int[] { 1991, 1, 1, 0, 0, 0, 0 },
                new int[] { 1992, 0, 1, 0, 0, 0, 0 },
                new int[] { 1993, 1, 2, 0, 0, 0, 0 },
                new int[] { 1994, 5, 7, 0, 0, 0, 0 },
                new int[] { 1995, 1, 8, 0, 0, 0, 0 },
                new int[] { 1996, 0, 8, 1, 1, 0, 0 },
                new int[] { 1997, 0, 8, 1, 2, 0, 0 },
                new int[] { 1998, 0, 8, 0, 2, 0, 0 },
                new int[] { 1999, 0, 8, 3, 5, 0, 0 },
            };
            Report report = new Report(DB, "social_unit_test_firstdegree", "social_unit_test_seconddegree", null, null);
            DataTable Results1 = new DataTable();
            DataTable Results2 = report.RowsForColleagueStarSecondDegree("Alice", "Carol", "Lisa");
            DataTable Results = Report.RollUpReportRows(Results1, Results2);
            VerifyColumnNames(Results);
            CheckResults(Results, Expected, "Alice", "Carol", "Lisa");

            // Make sure it still works with a null report
            Results = Report.RollUpReportRows(null, Results2);
            VerifyColumnNames(Results);
            CheckResults(Results, Expected, "Alice", "Carol", "Lisa");
        }

        /// <summary>
        /// Verify that Report.RollUpReportRows() works with an empty Report2 object
        /// </summary>
        [Test]
        public void TestRollUpWithEmptyReport2()
        {
            int[][] Expected = { 
                new int[] { 1994, 2, 2, 0, 0, 0, 0 },
                new int[] { 1995, 1, 3, 0, 0, 0, 0 },
                new int[] { 1996, 0, 3, 1, 1, 0, 0 },
                new int[] { 1997, 0, 3, 0, 1, 0, 0 },
                new int[] { 1998, 0, 3, 0, 1, 0, 0 },
                new int[] { 1999, 0, 3, 1, 2, 0, 0 },
                new int[] { 2000, 0, 3, 0, 2, 0, 0 },
                new int[] { 2001, 0, 3, 0, 2, 0, 0 },
                new int[] { 2002, 0, 3, 0, 2, 1, 1 },
                new int[] { 2003, 1, 4, 1, 3, 1, 2 },
                new int[] { 2004, 0, 4, 0, 3, 0, 2 },
                new int[] { 2005, 0, 4, 2, 5, 0, 2 },
            };
            Report report = new Report(DB, "social_unit_test_firstdegree", "social_unit_test_seconddegree", null, null);
            DataTable Results1 = report.RowsForColleagueStarSecondDegree("Alice", "Justin", "John");
            DataTable Results2 = new DataTable();
            DataTable Results = Report.RollUpReportRows(Results1, Results2);
            VerifyColumnNames(Results);
            CheckResults(Results, Expected, "Alice", "Justin", "John");

            // Make sure it still works with a null report
            Results = Report.RollUpReportRows(Results1, null);
            VerifyColumnNames(Results);
            CheckResults(Results, Expected, "Alice", "Justin", "John");
        }


        /// <summary>
        /// Test Report.GetSocialNetwork() for Alice
        /// </summary>
        [Test]
        public void TestSocialNetworkForAlice()
        {
            // Verify that the report returns a valid social network
            Report report = new Report(DB, "social_unit_test_firstdegree", "social_unit_test_seconddegree", null, null);
            Hashtable Results = report.GetSocialNetwork("Alice");
            Assert.AreEqual(Results.Count, 2);
            Assert.AreEqual(Results["Carol"].GetType(), Type.GetType("Com.StellmanGreene.SocialNetworking.SecondDegreeStars"));
            Assert.AreEqual(Results["Justin"].GetType(), Type.GetType("Com.StellmanGreene.SocialNetworking.SecondDegreeStars"));

            // Test the Carol network
            SecondDegreeStars Carol = (SecondDegreeStars) Results["Carol"];
            Assert.AreEqual(Carol.ColleagueSetnb, "Alice");
            Assert.AreEqual(Carol.FirstDegreeStarSetnb, "Carol");
            Assert.AreEqual(Carol.Setnbs.Count, 4);
            Assert.Contains("Joe", Carol.Setnbs);
            Assert.Contains("Mallory", Carol.Setnbs);
            Assert.Contains("Lisa", Carol.Setnbs);
            Assert.Contains("Effie", Carol.Setnbs);


            // Test the Justin network
            SecondDegreeStars Justin = (SecondDegreeStars) Results["Justin"];
            Assert.AreEqual(Justin.ColleagueSetnb, "Alice");
            Assert.AreEqual(Justin.FirstDegreeStarSetnb, "Justin");
            Assert.AreEqual(Justin.Setnbs.Count, 4);
            Assert.Contains("John", Justin.Setnbs);
            Assert.Contains("Roger", Justin.Setnbs);
            Assert.Contains("Constanc", Justin.Setnbs);
            Assert.Contains("Effie", Justin.Setnbs);
        }

        /// <summary>
        /// Make sure that Report.GetSocialNetwork() works even with an invalid colleague
        /// </summary>
        [Test]
        public void TestEmptySocialNetwork()
        {
            // Verify that the report returns a valid social network
            Report report = new Report(DB, "social_unit_test_firstdegree", "social_unit_test_seconddegree", null, null);
            Hashtable Results = report.GetSocialNetwork("Fail");
            Assert.AreEqual(Results.Count, 0);
        }

        /// <summary>
        /// Test Report.GetSocialNetwork() for Effie, using the second-degree database in
        /// place of the first-degree one. This should be the same as Alice's network,
        /// except that Effie shouldn't appear as his own second degree star.
        /// </summary>
        [Test]
        public void TestSocialNetworkForEffie()
        {
            // Verify that the report returns a valid social network
            Report report = new Report(DB, "social_unit_test_seconddegree", "social_unit_test_seconddegree", null, null);
            Hashtable Results = report.GetSocialNetwork("Effie");
            Assert.AreEqual(Results.Count, 2);
            Assert.AreEqual(Results["Carol"].GetType(), Type.GetType("Com.StellmanGreene.SocialNetworking.SecondDegreeStars"));
            Assert.AreEqual(Results["Justin"].GetType(), Type.GetType("Com.StellmanGreene.SocialNetworking.SecondDegreeStars"));

            // Test the Carol network
            SecondDegreeStars Carol = (SecondDegreeStars)Results["Carol"];
            Assert.AreEqual(Carol.ColleagueSetnb, "Effie");
            Assert.AreEqual(Carol.FirstDegreeStarSetnb, "Carol");
            Assert.AreEqual(Carol.Setnbs.Count, 3);
            Assert.Contains("Joe", Carol.Setnbs);
            Assert.Contains("Mallory", Carol.Setnbs);
            Assert.Contains("Lisa", Carol.Setnbs);
            Assert.AreEqual(Carol.Setnbs.Contains("Effie"), false);


            // Test the Justin network
            SecondDegreeStars Justin = (SecondDegreeStars)Results["Justin"];
            Assert.AreEqual(Justin.ColleagueSetnb, "Effie");
            Assert.AreEqual(Justin.FirstDegreeStarSetnb, "Justin");
            Assert.AreEqual(Justin.Setnbs.Count, 3);
            Assert.Contains("John", Justin.Setnbs);
            Assert.Contains("Roger", Justin.Setnbs);
            Assert.Contains("Constanc", Justin.Setnbs);
            Assert.AreEqual(Justin.Setnbs.Contains("Effie"), false);
        }

        /// <summary>
        /// Test Report.GetSocialNetwork() for Lisa, using the square database in
        /// place of the first-degree one. Lisa only has one first-degree star, Carol,
        /// and shouldn't appear as her own second-degree star.
        /// </summary>
        [Test]
        public void TestSocialNetworkForLisa()
        {
            // Verify that the report returns a valid social network
            Report report = new Report(DB, "social_unit_test_seconddegree", "social_unit_test_seconddegree", null, null);
            Hashtable Results = report.GetSocialNetwork("Lisa");
            Assert.AreEqual(Results.Count, 1);
            Assert.AreEqual(Results["Carol"].GetType(), Type.GetType("Com.StellmanGreene.SocialNetworking.SecondDegreeStars"));

            // Test the Carol network
            SecondDegreeStars Carol = (SecondDegreeStars)Results["Carol"];
            Assert.AreEqual(Carol.ColleagueSetnb, "Lisa");
            Assert.AreEqual(Carol.FirstDegreeStarSetnb, "Carol");
            Assert.AreEqual(Carol.Setnbs.Count, 3);
            Assert.Contains("Joe", Carol.Setnbs);
            Assert.Contains("Mallory", Carol.Setnbs);
            Assert.Contains("Effie", Carol.Setnbs);
            Assert.AreEqual(Carol.Setnbs.Contains("Lisa"), false);
        }

        /// <summary>
        /// Verify that the report generates properly
        /// </summary>
        [Test]
        public void TestGenerate()
        {
            CallbackCalled = 0;
            // Set up an anonymous callback function to receive status
            Report.Status StatusCallback = delegate(int Number, int Total, string Setnb, string Name)
            {
                Assert.AreEqual(Number, 1);
                Assert.AreEqual(Total, 1);
                Assert.AreEqual(Setnb, "Alice");
                Assert.AreEqual(Name, "Alice Anderson");
                CallbackCalled++;
            };


            // Create the report
            Report report = new Report(DB, "social_unit_test_firstdegree", "social_unit_test_seconddegree", null, null);

            DataTable Results = null;
            Report.WriteRows WriteRowsCallback = delegate(DataTable RowsToWrite)
            {
                Results = RowsToWrite;
            };

            report.Generate(StatusCallback, WriteRowsCallback, null);
            Assert.AreEqual(CallbackCalled, 1);
            Assert.IsNotNull(Results);

            // Expected rows for Alice/Carol/Lisa
            int[][] ExpectedAliceCarolLisa = { 
                new int[] { 1991, 1, 1, 0, 0, 0, 0 }, 
                new int[] { 1992, 0, 1, 0, 0, 0, 0 },
                new int[] { 1993, 1, 2, 0, 0, 0, 0 },
                new int[] { 1994, 5, 7, 0, 0, 0, 0 },
                new int[] { 1995, 1, 8, 0, 0, 0, 0 },
                new int[] { 1996, 0, 8, 1, 1, 0, 0 },
                new int[] { 1997, 0, 8, 1, 2, 0, 0 },
                new int[] { 1998, 0, 8, 0, 2, 0, 0 },
                new int[] { 1999, 0, 8, 3, 5, 0, 0 },
            };

            VerifyColumnNames(Results);

            // Expected rows for Alice/Justin/John
            int[][] ExpectedAliceJustinJohn = { 
                new int[] { 1994, 2, 2, 0, 0, 0, 0 }, 
                new int[] { 1995, 1, 3, 0, 0, 0, 0 },
                new int[] { 1996, 0, 3, 1, 1, 0, 0 },
                new int[] { 1997, 0, 3, 0, 1, 0, 0 },
                new int[] { 1998, 0, 3, 0, 1, 0, 0 },
                new int[] { 1999, 0, 3, 1, 2, 0, 0 },
                new int[] { 2000, 0, 3, 0, 2, 0, 0 },
                new int[] { 2001, 0, 3, 0, 2, 0, 0 },
                new int[] { 2002, 0, 3, 0, 2, 1, 1 },
                new int[] { 2003, 1, 4, 1, 3, 1, 2 },
                new int[] { 2004, 0, 4, 0, 3, 0, 2 },
                new int[] { 2005, 0, 4, 2, 5, 0, 2 },
            };

            // Extract the rows from the results for Alice/Carol/Lisa
            DataTable AliceCarolLisa = null;
            for (int i = 0; i < Results.Rows.Count; i++)
            {
                if (Results.Rows[i][0].ToString() == "Alice"
                    && Results.Rows[i][1].ToString() == "Carol"
                    && Results.Rows[i][2].ToString() == "Lisa") {

                    // Pull out the results
                    AliceCarolLisa = Results.Clone();
                    for (int j = 0; j < ExpectedAliceCarolLisa.Length; j++)
                    {
                        AliceCarolLisa.Rows.Add(Results.Rows[i + j].ItemArray);
                    }
                    i += ExpectedAliceCarolLisa.Length;
                }
            }
            CheckResultRows(AliceCarolLisa, ExpectedAliceCarolLisa, "Alice", "Carol", "Lisa", 1, ExpectedAliceCarolLisa.Length);



            // Extract the rows from the results for Alice/Justin/John
            DataTable AliceJustinJohn = null;
            for (int i = 0; i < Results.Rows.Count; i++)
            {
                if (Results.Rows[i][0].ToString() == "Alice"
                    && Results.Rows[i][1].ToString() == "Justin"
                    && Results.Rows[i][2].ToString() == "John")
                {

                    // Pull out the results
                    AliceJustinJohn = Results.Clone();
                    for (int j = 0; j < ExpectedAliceJustinJohn.Length; j++)
                    {
                        AliceJustinJohn.Rows.Add(Results.Rows[i + j].ItemArray);
                    }
                    i += ExpectedAliceJustinJohn.Length;
                }
            }
            CheckResultRows(AliceJustinJohn, ExpectedAliceJustinJohn, "Alice", "Justin", "John", 1, ExpectedAliceJustinJohn.Length);
        }


        int CallbackCalled;

        /// <summary>
        /// Write the whole CSV file and read it back in
        /// </summary>
        [Test]
        public void TestWholeReport()
        {
            CallbackCalled = 0;

            // CSV file to write the report to
            string ReportPath = AppDomain.CurrentDomain.BaseDirectory + "\\TestReport.csv";

            // Remove the CSV file if it exists
            if (File.Exists(ReportPath))
                File.Delete(ReportPath);

            // Set up an anonymous callback function to receive status
            Report.Status StatusCallback = delegate(int Number, int Total, string Setnb, string Name)
            {
                Assert.AreEqual(Number, 1);
                Assert.AreEqual(Total, 1);
                Assert.AreEqual(Setnb, "Alice");
                Assert.AreEqual(Name, "Alice Anderson");
                CallbackCalled++;
            };


            // Open the file to write the report to
            using (StreamWriter Writer = File.CreateText(ReportPath))
            {

                // Set up an anonymous callback function to write the rows to the database
                bool HeaderWritten = false;
                Report.WriteRows WriteRowsCallback = delegate(DataTable RowsToWrite)
                {
                    if (!HeaderWritten)
                    {
                        Report.WriteHeader(RowsToWrite, Writer);
                        HeaderWritten = true;
                    }
                    Report.WriteCSV(RowsToWrite, Writer);
                };



                // Write the CSV file
                Report report = new Report(DB, "social_unit_test_firstdegree", "social_unit_test_seconddegree", null, null);
                report.Generate(StatusCallback, WriteRowsCallback, null);
                Assert.AreEqual(CallbackCalled, 1);
            }

            // Compare it with the test report file
            string TestDataPath = AppDomain.CurrentDomain.BaseDirectory + "\\Unit Tests\\Test Data\\Full report.csv";
            CompareReport(ReportPath, TestDataPath);


            // Remove the CSV file
            if (File.Exists(ReportPath))
                File.Delete(ReportPath);
        }



        public static void CompareReport(string ReportPath, string TestDataPath)
        {
            StreamReader ReportCSV = new StreamReader(ReportPath);
            StreamReader CompareCSV = new StreamReader(TestDataPath);

            // Compare each data row -- don't forget that the header is row #1
            // The report shouldn't contain any quote or comma characters, so we
            // don't have to handle them here.
            int Row = 0;
            while (!ReportCSV.EndOfStream && !CompareCSV.EndOfStream)
            {
                Row++;
                string[] ReportRow = ReportCSV.ReadLine().Split(',');
                string[] CompareRow = CompareCSV.ReadLine().Split(',');
                if (ReportRow.Length != CompareRow.Length)
                    Assert.Fail("Row " + Row.ToString() + ": report has "
                        + ReportRow.Length.ToString() + " items, test data has "
                        + CompareRow.Length.ToString() + " items.");
                for (int Col = 0; Col < ReportRow.Length; Col++)
                {
                    if (ReportRow[Col] != CompareRow[Col])
                        Assert.Fail("Row " + Row.ToString() + ", column " + Col.ToString() + ": "
                            + "report has '" + ReportRow[Col] + "', test data has '" + CompareRow[Col] + "'");
                }
            }

            // Make sure both files were read completely
            if (!ReportCSV.EndOfStream)
                Assert.Fail("Additional rows are at the end of the report (" + Row+ " rows):\r\n" + ReportCSV.ReadToEnd());
            if (!CompareCSV.EndOfStream)
                Assert.Fail("Rows are missing from the report (" + Row + " rows):\r\n" + CompareCSV.ReadToEnd());

            ReportCSV.Close();
            CompareCSV.Close();
        }
    }
}
