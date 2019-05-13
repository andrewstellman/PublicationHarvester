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
    /// Test the "include colleagues" feature
    /// </summary>
    [TestFixture]
    public class TestIncludeColleagues
    {

        private Database DB;
        int CallbackCalled;


        [OneTimeSetUp]
        public void TestReportSetUp()
        {
            UnitTestData.Create();
            UnitTestData.AddExtraColleagues();
            DB = UnitTestData.GetDB();
        }



        /// <summary>
        /// Write the report for Jimmy
        /// </summary>
        [Test]
        public void TestReportWithJimmy()
        {
            CallbackCalled = 0;

            // CSV file to write the report to
            string ReportPath = AppDomain.CurrentDomain.BaseDirectory + "\\JimmyReport.csv";

            // Remove the CSV file if it exists
            if (File.Exists(ReportPath))
                File.Delete(ReportPath);

            // Set up an anonymous callback function to receive status
            Report.Status StatusCallback = delegate(int Number, int Total, string Setnb, string Name)
            {
                Assert.AreEqual(Setnb, "Jimmy", "Setnb");
                Assert.AreEqual(Name, "Jimmy Johnson", "Name");
                CallbackCalled++;
            };

            // Write the CSV file

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

                Report report = new Report(DB, "social_unit_test_firstdegree", "social_unit_test_seconddegree", new List<string> { "Jimmy" }, null);
                report.Generate(StatusCallback, WriteRowsCallback, null);
                Assert.AreEqual(CallbackCalled, 1, "CallbackCalled");

            }

            // Compare it with the test report file
            string TestDataPath = AppDomain.CurrentDomain.BaseDirectory + "\\Test Data\\Report for Jimmy.csv";
            TestReport.CompareReport(ReportPath, TestDataPath);

            // Remove the CSV file
            if (File.Exists(ReportPath))
                File.Delete(ReportPath);
        }

        /// <summary>
        /// Write the report for Frank and Bob
        /// </summary>
        [Test]
        public void TestReportWithFrankAndBob()
        {
            CallbackCalled = 0;

            // CSV file to write the report to
            string ReportPath = AppDomain.CurrentDomain.BaseDirectory + "\\FrankAndBobReport.csv";

            // Remove the CSV file if it exists
            if (File.Exists(ReportPath))
                File.Delete(ReportPath);

            // Set up an anonymous callback function to receive status
            Report.Status StatusCallback = delegate(int Number, int Total, string Setnb, string Name)
            {
                if (CallbackCalled == 0)
                {
                    Assert.AreEqual(Setnb, "Bob", "Setnb");
                    Assert.AreEqual(Name, "Bob Bingo", "Name");
                }
                else
                {
                    Assert.AreEqual(Setnb, "Frank", "Setnb");
                    Assert.AreEqual(Name, "Frank Flat", "Name");
                }
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

                // Write the CSV file -- use incorrect case to make sure the match is case-insensitive
                Report report = new Report(DB, "social_unit_test_firstdegree", "social_unit_test_seconddegree", new List<string> { "frank", "BOB" }, null);
                report.Generate(StatusCallback, WriteRowsCallback, null);
                Assert.AreEqual(CallbackCalled, 2, "CallbackCalled");
            }

            // Compare it with the test report file
            string TestDataPath = AppDomain.CurrentDomain.BaseDirectory + "\\Test Data\\Report for Frank and Bob.csv";
            TestReport.CompareReport(ReportPath, TestDataPath);

            // Remove the CSV file
            if (File.Exists(ReportPath))
                File.Delete(ReportPath);

        }


        /// <summary>
        /// Write the report for everyone
        /// </summary>
        [Test]
        public void TestReportWithAliceJimmyFrankAndBob()
        {

            for (int iteration = 0; iteration < 2; iteration++)
            {

                CallbackCalled = 0;

                // CSV file to write the report to
                string ReportPath = AppDomain.CurrentDomain.BaseDirectory + "\\EveryoneReport.csv";

                // Remove the CSV file if it exists
                if (File.Exists(ReportPath))
                    File.Delete(ReportPath);

                // Set up an anonymous callback function to receive status
                Report.Status StatusCallback = delegate(int Number, int Total, string Setnb, string Name)
                {
                    switch (CallbackCalled)
                    {
                        case 0:
                            Assert.AreEqual(Setnb, "Alice", "Setnb");
                            Assert.AreEqual(Name, "Alice Anderson", "Name");
                            break;
                        case 1:
                            if (iteration == 0)
                            {
                                Assert.AreEqual(Setnb, "Bob", "Setnb");
                                Assert.AreEqual(Name, "Bob Bingo", "Name");
                            }
                            else
                            {
                                Assert.AreEqual(Setnb, "Jimmy", "Setnb");
                                Assert.AreEqual(Name, "Jimmy Johnson", "Name");
                            }
                            break;
                        case 2:
                            Assert.AreEqual(iteration, 0, "Callback should not be called twice for second iteration");
                            Assert.AreEqual(Setnb, "Frank", "Setnb");
                            Assert.AreEqual(Name, "Frank Flat", "Name");
                            break;
                        case 3:
                            Assert.AreEqual(iteration, 0, "Callback should not be called three times for second iteration");
                            Assert.AreEqual(Setnb, "Jimmy", "Setnb");
                            Assert.AreEqual(Name, "Jimmy Johnson", "Name");
                            break;
                        default:
                            Assert.Fail("CallbackCalled should only be 0, 1, 2 or 3");
                            break;
                    }
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


                    // This will be one twice. The first time we'll do it for everyone, the second time we'll exclude 
                    // Bob and Frank to test the ColleaguesToExclude parameter.
                    List<string> ColleaguesToExclude;
                    if (iteration == 0)
                        ColleaguesToExclude = new List<string>();
                    else
                        ColleaguesToExclude = new List<string>() { "FRANK", "Bob" }; 

                    // Write the CSV file -- including extra items in the list of colleages to include
                    Report report = new Report(DB, "social_unit_test_firstdegree", "social_unit_test_seconddegree", new List<string> { "Blah blah blah", "alice", "frank", "BOB", "jimmy", "bozo", "boffo" }, ColleaguesToExclude);
                    report.Generate(StatusCallback, WriteRowsCallback, null);
                    if (iteration == 0)
                        Assert.AreEqual(CallbackCalled, 4, "CallbackCalled");
                    else
                        Assert.AreEqual(CallbackCalled, 2, "CallbackCalled");
                }

                // Compare it with the test report file -- use the full report for the first iteration (where
                // ColleaguesToExclude is null) and a report without Bob and Frank for the second one (where
                // ColleaguesToExclude contains Bob and Frank)
                string TestDataPath = AppDomain.CurrentDomain.BaseDirectory;
                if (iteration == 0)
                    TestDataPath += "\\Test Data\\Report for Jimmy, Bob, Frank and Alice.csv";
                else
                    TestDataPath += "\\Test Data\\Report for Jimmy and Alice.csv";

                TestReport.CompareReport(ReportPath, TestDataPath);


                // Remove the CSV file
                if (File.Exists(ReportPath))
                    File.Delete(ReportPath);

            }
        }




    }
}
