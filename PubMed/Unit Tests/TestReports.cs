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
using System.Text;
using System.Collections;
using System.Data;
using System.Data.Odbc;
using System.IO;
using NUnit.Framework;
using System.Linq;

namespace Com.StellmanGreene.PubMed.Unit_Tests
{
    /// <summary>
    /// Test the reports class
    /// </summary>
    [TestFixture]
    public class TestReports
    {
        private Database DB;
        private Reports reports;
        private People people;



        /// <summary>
        /// Test individual rows in the people report
        /// </summary>
        [Test]
        public void TestPeopleReportRows()
        {
            // Set up the database with test publications (and don't forget to add the 
            // publication types!)
            DB = new Database("Publication Harvester Unit Test");
            Harvester harvester = new Harvester(DB);
            harvester.CreateTables();
            PublicationTypes PubTypes = new PublicationTypes(
                AppDomain.CurrentDomain.BaseDirectory + "\\Unit Tests\\TestPublicationTypes",
                "PublicationTypes.csv"
                );
            PubTypes.WriteToDB(DB);
            reports = new Reports(DB, AppDomain.CurrentDomain.BaseDirectory + "\\Unit Tests\\TestReports\\pubmed_jifs.xls");
            Assert.IsTrue(reports.Weights.Count == 10);
            TestHarvester.GetPublicationsFromInput1XLS_Using_MockNCBI(false, new string[] { "eng" }, 22);
            people = new People(DB);


            foreach (Person person in people.PersonList)
            {
                Publications pubs = new Publications(DB, person, false);
                switch (person.Setnb)
                {
                    case "A6009400": 
                        // Van Eys has two publications in 1998, both have zero weight
                        DataRow Row = WriteAndReadBackCSVRow(reports.ReportRow(person, pubs, 1998));

                        Assert.IsTrue(Row.ItemArray.Length == 74);
                        Assert.IsTrue(Row.ItemArray[0].ToString() == "A6009400");
                        Assert.IsTrue(Row.ItemArray[1].ToString() == "1998");

                        // Verify that all values are zero, except for pubcount (#3),
                        // pubcount_pos1 (#5), 123pubcount (#15), 123pubcount_pos1 (#17), 
                        // 3pubcount (#51), 3pubcount_pos1 (#53)
                        for (int i = 2; i <= 73; i++)
                        {
                            if ((i == 2) || (i == 4) || (i == 14) || (i == 16) ||
                                (i == 50) || (i == 52))
                                Assert.IsTrue(Row.ItemArray[i].ToString() == "2", "Failed at i == " + i.ToString());
                            else
                                Assert.IsTrue(Row.ItemArray[i].ToString() == "0", "Failed at i == " + i.ToString());
                        }
                        break;

                    case "A5401532": 
                        // Tobian has two publications in 1997 of type 3 with a 
                        // combined weight of 4.602
                        Row = WriteAndReadBackCSVRow(reports.ReportRow(person, pubs, 1997));

                        Assert.IsTrue(Row.ItemArray.Length == 74);
                        Assert.IsTrue(Row.ItemArray[0].ToString() == "A5401532");
                        Assert.IsTrue(Row.ItemArray[1].ToString() == "1997");

                        // Verify that all values are zero, except for pubcount (#3),
                        // pubcount_pos1 (#5), 123pubcount (#15), 123pubcount_pos1 (#17), 
                        // 3pubcount (#51), 3pubcount_pos1 (#53), which should be 2
                        //
                        // and wghtd_pubcount (#4), wghtd_pubcount_pos1 (#6), 
                        // wghtd_123pubcount (#16), wghtd_123pubcount_pos1 (#18), 
                        // wghtd_3pubcount (#52), wghtd_3pubcount_pos1 (#54),
                        // which should be 4.602
                        for (int i = 2; i <= 73; i++)
                        {
                            if ((i == 2) || (i == 4) || (i == 14) || (i == 16) ||
                                (i == 50) || (i == 52))
                                Assert.IsTrue(Row.ItemArray[i].ToString() == "2", "Failed at i == " + i.ToString());
                            else if ((i == 3) || (i == 5) || (i == 15) || (i == 17) ||
                                (i == 51) || (i == 53))
                                Assert.IsTrue(Row.ItemArray[i].ToString() == "4.602", "Failed at i == " + i.ToString());
                            else
                                Assert.IsTrue(Row.ItemArray[i].ToString() == "0", "Failed at i == " + i.ToString());
                        }
                        break;
                }
            }
            
        }

        /// <summary>
        /// Test the publications report
        /// </summary>
        [Test]
        public void TestPubsReportRow() 
        {
            // Set up the database with test publications (and don't forget to add the 
            // publication types!)
            DB = new Database("Publication Harvester Unit Test");
            Harvester harvester = new Harvester(DB);
            harvester.CreateTables();
            PublicationTypes PubTypes = new PublicationTypes(
                AppDomain.CurrentDomain.BaseDirectory + "\\Unit Tests\\TestPublicationTypes",
                "PublicationTypes.csv"
                );
            PubTypes.WriteToDB(DB);
            reports = new Reports(DB, AppDomain.CurrentDomain.BaseDirectory + "\\Unit Tests\\TestReports\\pubmed_jifs.xls");
            Assert.IsTrue(reports.Weights.Count == 10);
            TestHarvester.GetPublicationsFromInput1XLS_Using_MockNCBI(false, new string[] { "eng" }, 22);
            people = new People(DB);


            foreach (Person person in people.PersonList)
            {
                Publications pubs = new Publications(DB, person, false);

                if (pubs.PublicationList != null) foreach (Publication pub in pubs.PublicationList)
                {


                    DataRow Row = WriteAndReadBackCSVRow(reports.PubsReportRow(person, pub));

                    switch (pub.PMID)
                    {
                        case 15249795:
                            // 0. setnb
                            Assert.IsTrue(Row.ItemArray[0].ToString() == "A5401532");

                            // 1. pmid
                            Assert.IsTrue(Row.ItemArray[1].ToString() == "15249795");

                            // 2. journal_name
                            Assert.IsTrue(Row.ItemArray[2].ToString() == "J Clin Hypertens (Greenwich)");

                            // 3. year
                            Assert.IsTrue(Row.ItemArray[3].ToString() == "2004");

                            // 4. Month
                            Assert.IsTrue(Row.ItemArray[4].ToString() == "Jul");

                            // 5. day
                            Assert.IsTrue(Row.ItemArray[5].ToString() == "");

                            // 6. title
                            Assert.IsTrue(Row.ItemArray[6].ToString() == "Interview with Louis Tobian, MD. Interview by Marvin Moser.");

                            // 7. Volume
                            Assert.IsTrue(Row.ItemArray[7].ToString() == "6");

                            // 8. issue
                            Assert.IsTrue(Row.ItemArray[8].ToString() == "7");

                            // 9. position
                            Assert.IsTrue(Row.ItemArray[9].ToString() == "1");

                            // 10. nbauthors
                            Assert.IsTrue(Row.ItemArray[10].ToString() == "1");

                            // 11. Bin
                            Assert.IsTrue(Row.ItemArray[11].ToString() == "0");

                            // 12. Pages
                            Assert.IsTrue(Row.ItemArray[12].ToString() == "391-2");

                            // 13. Publication_type
                            Assert.IsTrue(Row.ItemArray[13].ToString() == "Historical Article");


                            break;
                    }
                }
            }

        }


        /// <summary>
        /// Take report row data for a single row, write it to a CSV file, and
        /// then read it back
        /// </summary>
        /// <param name="rowData">String containing the CSV file row</param>
        /// <returns>A DataRow object containing the CSV data</returns>
        private DataRow WriteAndReadBackCSVRow(string rowData)
        {
            // Create a temporary filename and write the row to it
            string BaseFilename = "WriteAndReadBackCSVRow";
            string Filename;
            int i = 0;
            do
            {
                Filename = BaseFilename + i.ToString() + ".csv";
                i++;
            } while (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\" + Filename));
            StreamWriter Writer = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\" + Filename);

            // Write a fake header row
            int NumColumns = rowData.Split(new string[] { "," }, StringSplitOptions.None).Length;
            Writer.Write("Column0");
            for (int col = 1; col < NumColumns; col++)
            {
                Writer.Write(",Column" + col.ToString());
            }
            Writer.WriteLine();
            Writer.WriteLine(rowData); 
            Writer.Close();

            // Read the row back from the file
            string ConnectionString =
                "Driver={Microsoft Text Driver (*.txt; *.csv)};Dbq="
                + AppDomain.CurrentDomain.BaseDirectory + ";";
            OdbcConnection Connection = new OdbcConnection(ConnectionString);
            OdbcDataAdapter DataAdapter = new OdbcDataAdapter
                ("SELECT * FROM [" + Filename + "]", Connection);
            DataTable Results = new DataTable();
            int Rows = DataAdapter.Fill(Results);
            Connection.Close();

            // Make the string contained sure exactly one row
            Assert.IsTrue(Rows == 1);

            // Delete the temporary file
            File.Delete(AppDomain.CurrentDomain.BaseDirectory + "\\" + Filename);

            return Results.Rows[0];
        }


        /// <summary>
        /// Verify the full report against test data generated by hand
        /// </summary>
        [Test]
        public void TestEntireReport()
        {
            string Setnb = "";

            // Set up the database with test publications (and don't forget to add the 
            // publication types!)
            DB = new Database("Publication Harvester Unit Test");
            Harvester harvester = new Harvester(DB);
            harvester.CreateTables();
            PublicationTypes PubTypes = new PublicationTypes(
                AppDomain.CurrentDomain.BaseDirectory + "\\Unit Tests\\TestPublicationTypes",
                "PublicationTypes.csv"
                );
            PubTypes.WriteToDB(DB);
            reports = new Reports(DB, AppDomain.CurrentDomain.BaseDirectory + "\\Unit Tests\\TestReports\\pubmed_jifs.xls");
            Assert.IsTrue(reports.Weights.Count == 10);
            TestHarvester.GetPublicationsFromInput1XLS_Using_MockNCBI(false, new string[] { "eng" }, 22);
            people = new People(DB);


            // Write the report
            StreamWriter writer = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\TestEntireReport.csv");
            Reports.ReportStatus StatusCallback = delegate (int number, int total, Person person, bool ProgressBarOnly) {
                //
            };
            Reports.ReportMessage MessageCallback = delegate (string Message)
            {
                //
            };
            reports.PeopleReport(null, writer, StatusCallback, MessageCallback);
            writer.Close();

            // Read the report into an array
            var lines = File.ReadAllLines($"{AppDomain.CurrentDomain.BaseDirectory}\\TestEntireReport.csv")
                .Select(line => line.Split(new char[] { ',' }));
            var header = lines.First();
            var data = lines.Skip(1).ToList();
            Assert.AreEqual(85, data.Count);

            string ReportData(string setnb, string year, string column)
            {
                var index = Array.IndexOf(header, column);
                var row = data.Where(line => line[0] == setnb && line[1] == year);
                Assert.AreEqual(1, row.Count(), $"Unable to find setnb={setnb} year={year} in TestEntireReport.csv");
                return row.First()[index];
            }

            var q = ReportData("A5401532", "1997", "wghtd_pubcount_pos1");

            // Read the report file that was generated by hand (TestEntireReport_Data.xls)
            // and check each value against the report that was generated by Reports()
            string[] Columns = {
                "setnb", "year", "pubcount", "wghtd_pubcount", "pubcount_pos1", 
                "wghtd_pubcount_pos1", "pubcount_posN", "wghtd_pubcount_posN", 
                "pubcount_posM", "wghtd_pubcount_posM", "pubcount_posNTL", 
                "wghtd_pubcount_posNTL", "pubcount_pos2", "wghtd_pubcount_pos2", 
                "123pubcount", "wghtd_123pubcount", "123pubcount_pos1", 
                "wghtd_123pubcount_pos1", "123pubcount_posN", "wghtd_123pubcount_posN", 
                "123pubcount_posM", "wghtd_123pubcount_posM", "123pubcount_posNTL", 
                "wghtd_123pubcount_posNTL", "123pubcount_pos2", "wghtd_123pubcount_pos2", 
                "1pubcount", "wghtd_1pubcount", "1pubcount_pos1", "wghtd_1pubcount_pos1", 
                "1pubcount_posN", "wghtd_1pubcount_posN", "1pubcount_posM", 
                "wghtd_1pubcount_posM", "1pubcount_posNTL", "wghtd_1pubcount_posNTL", 
                "1pubcount_pos2", "wghtd_1pubcount_pos2", "2pubcount", "wghtd_2pubcount", 
                "2pubcount_pos1", "wghtd_2pubcount_pos1", "2pubcount_posN", 
                "wghtd_2pubcount_posN", "2pubcount_posM", "wghtd_2pubcount_posM", 
                "2pubcount_posNTL", "wghtd_2pubcount_posNTL", "2pubcount_pos2", 
                "wghtd_2pubcount_pos2", "3pubcount", "wghtd_3pubcount", "3pubcount_pos1", 
                "wghtd_3pubcount_pos1", "3pubcount_posN", //"wghtd_3pubcount_posN", 
                "3pubcount_posM", "wghtd_3pubcount_posM", "3pubcount_posNTL", 
                "wghtd_3pubcount_posNTL", "3pubcount_pos2", "wghtd_3pubcount_pos2", 
                "4pubcount", "wghtd_4pubcount", "4pubcount_pos1", "wghtd_4pubcount_pos1", 
                "4pubcount_posN", "wghtd_4pubcount_posN", "4pubcount_posM", 
                "wghtd_4pubcount_posM", "4pubcount_posNTL", "wghtd_4pubcount_posNTL", 
                "4pubcount_pos2", "wghtd_4pubcount_pos2"
            };

            DataTable HandGeneratedData = NpoiHelper.ReadExcelFileToDataTable(AppDomain.CurrentDomain.BaseDirectory +
                "\\Unit Tests\\TestReports", "TestEntireReport_Data.xls");

            Assert.AreEqual(HandGeneratedData.Rows.Count, 85);

            var valuesChecked = 0;

            for (int RowNum = 0; RowNum < HandGeneratedData.Rows.Count; RowNum++)
            {
                // Find the rows in the hand-generated data and the report
                DataRow HandGeneratedRow = HandGeneratedData.Rows[RowNum];
                Setnb = HandGeneratedRow[0].ToString();
                int Year = Convert.ToInt32(HandGeneratedRow[1]);

                for (int i = 2; i < Columns.Length; i++)
                {
                    valuesChecked++;
                    String columnName = Columns[i];
                    var actualValue = ReportData(Setnb, Year.ToString(), columnName);
                    string expectedValue = HandGeneratedRow[columnName].ToString();
                    Assert.AreEqual(expectedValue, actualValue, Setnb + "/" + Year.ToString() + "/" + columnName + " -- hand generated has " + expectedValue + ", report has" + actualValue);
                }
            }

            Assert.AreEqual((HandGeneratedData.Rows.Count) * (Columns.Length - 2), valuesChecked);

            // Use BackupReportAndGetSetnbs to back up the report -- check that it
            // returns the correct list of Setnbs and removes the last one from
            // the file. The last Setnb should still be in Setnb.
            ArrayList Setnbs = Reports.BackupReportAndGetSetnbs(AppDomain.CurrentDomain.BaseDirectory + "\\TestEntireReport.csv");
            // Read the backup file that was created, make sure that the last setnb in the 
            // file isn't contained and the others are
            StreamReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\TestEntireReport.csv.bak");
            string Line = reader.ReadLine(); //skip the header row
            while ((Line = reader.ReadLine()) != null) // Find the last setnb in the original file
            {
                Setnb = Line.Substring(0, 8);
            }
            string RemovedSetnb = Setnb;
            Assert.IsFalse(Setnbs.Contains(RemovedSetnb));
            reader.Close();

            // Verify that the new file contains only the other setnbs
            Assert.IsTrue(Setnbs.Count == 3);
            Assert.IsFalse(Setnbs.Contains(RemovedSetnb));
            reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\TestEntireReport.csv");
            Line = reader.ReadLine(); //skip the header row
            while ((Line = reader.ReadLine()) != null)
            {
                Setnb = Line.Substring(0, 8);
                Assert.IsTrue(Setnbs.Contains(Setnb));
                Assert.IsFalse(Setnb == RemovedSetnb);
            }
            reader.Close();

            // Delete the temporary files
            File.Delete(AppDomain.CurrentDomain.BaseDirectory + "\\TestEntireReport.csv");
            File.Delete(AppDomain.CurrentDomain.BaseDirectory + "\\TestEntireReport.csv.bak");
        }

        /// <summary>
        /// Verify that the reports will skip Setnbs
        /// </summary>
        [Test]
        public void TestSkipSetnbs()
        {
            // Set up the database with test publications (and don't forget to add the 
            // publication types!)
            DB = new Database("Publication Harvester Unit Test");
            Harvester harvester = new Harvester(DB);
            harvester.CreateTables();
            PublicationTypes PubTypes = new PublicationTypes(
                AppDomain.CurrentDomain.BaseDirectory + "\\Unit Tests\\TestPublicationTypes",
                "PublicationTypes.csv"
                );
            PubTypes.WriteToDB(DB);
            reports = new Reports(DB, AppDomain.CurrentDomain.BaseDirectory + "\\Unit Tests\\TestReports\\pubmed_jifs.xls");
            Assert.IsTrue(reports.Weights.Count == 10);
            TestHarvester.GetPublicationsFromInput1XLS_Using_MockNCBI(false, new string[] { "eng" }, 22);
            people = new People(DB);


            Reports.ReportStatus StatusCallback = delegate(int number, int total, Person person, bool ProgressBarOnly)
            {
                //
            };
            Reports.ReportMessage MessageCallback = delegate(string Message)
            {
                //
            };

            // Set up the Setnbs to skip
            ArrayList SetnbsToSkip = new ArrayList();
            SetnbsToSkip.Add("A5401532");
            SetnbsToSkip.Add("A6009400");

            // Verify that the people report skips the setnbs
            StreamWriter writer = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\TestSkipSetnbs.csv");
            reports.PeopleReport(SetnbsToSkip, writer, StatusCallback, MessageCallback);
            writer.Close();
            StreamReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\TestSkipSetnbs.csv");
            string Line = reader.ReadLine(); // skip the header row
            while ((Line = reader.ReadLine()) != null)
            {
                Assert.IsFalse(Line.StartsWith("A5401532"));
                Assert.IsFalse(Line.StartsWith("A6009400"));
            }
            reader.Close();

            // Verify that the people report skips the setnbs
            writer = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\TestSkipSetnbs.csv", false);
            reports.PubsReport(SetnbsToSkip, writer, StatusCallback, MessageCallback);
            writer.Close();
            reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\TestSkipSetnbs.csv");
            Line = reader.ReadLine(); // skip the header row
            while ((Line = reader.ReadLine()) != null)
            {
                Assert.IsFalse(Line.StartsWith("A5401532"));
                Assert.IsFalse(Line.StartsWith("A6009400"));
            }
            reader.Close();

            File.Delete(AppDomain.CurrentDomain.BaseDirectory + "\\TestSkipSetnbs.csv");
        }


        /// <summary>
        /// Test the MeSH Heading report
        /// </summary>
        [Test]
        public void TestMeSHHeadingReport()
        {
            // Set up the database with test publications (and don't forget to add the 
            // publication types!)
            DB = new Database("Publication Harvester Unit Test");
            Harvester harvester = new Harvester(DB);
            harvester.CreateTables();
            PublicationTypes PubTypes = new PublicationTypes(
                AppDomain.CurrentDomain.BaseDirectory + "\\Unit Tests\\TestPublicationTypes",
                "PublicationTypes.csv"
                );
            PubTypes.WriteToDB(DB);
            reports = new Reports(DB, AppDomain.CurrentDomain.BaseDirectory + "\\Unit Tests\\TestReports\\pubmed_jifs.xls");
            Assert.IsTrue(reports.Weights.Count == 10);
            TestHarvester.GetPublicationsFromInput1XLS_Using_MockNCBI(false, new string[] { "eng" }, 22);

            // Write the MeSH Heading report
            StreamWriter writer = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\MeSHHeadingReport.csv");
            Reports.ReportStatus StatusCallback = delegate(int number, int total, Person person, bool ProgressBarOnly)
            {
                //
            };
            Reports.ReportMessage MessageCallback = delegate(string Message)
            {
                //
            };
            reports.MeSHHeadingReport(writer, StatusCallback, MessageCallback);
            writer.Close();

            // Verify that the MeSH headings were written properly

            // Read the rows back from the file
            string ConnectionString =
                "Driver={Microsoft Text Driver (*.txt; *.csv)};Dbq="
                + AppDomain.CurrentDomain.BaseDirectory + ";";
            OdbcConnection Connection = new OdbcConnection(ConnectionString);
            OdbcDataAdapter DataAdapter = new OdbcDataAdapter
                ("SELECT * FROM [MeSHHeadingReport.csv]", Connection);
            DataTable Results = new DataTable();
            int Rows = DataAdapter.Fill(Results);
            Connection.Close();

            int numChecked = 0;

            // Check a few selected results
            foreach (DataRow Row in Results.Rows)
            {
                string Setnb = Row[0].ToString();
                int Year = Convert.ToInt32(Row[1]);
                string Heading = Row[2].ToString();
                int Count = Convert.ToInt32(Row[3]);

                switch (Setnb)
                {
                    case "A6009400": // Van Eys
                        if ((Year == 1998) && (Heading == "Humans"))
                        {
                            Assert.IsTrue(Count == 2);
                            numChecked++;
                        }
                        if ((Year == 1998) && (Heading == "Child")) 
                        {
                            Assert.IsTrue(Count == 1);
                            numChecked++;
                        }
                        if ((Year == 2001) && (Heading == "Humans"))
                        {
                            Assert.IsTrue(Count == 1);
                            numChecked++;
                        }
                        break;

                    case "A5702471": // Guillemin
                        if ((Year == 2005) && (Heading == "Hypothalamic Hormones/*physiology"))
                        {
                            Assert.IsTrue(Count == 1);
                            numChecked++;
                        }
                        break;
                }
            }
            Assert.IsTrue(numChecked == 4);

            File.Delete(AppDomain.CurrentDomain.BaseDirectory + "\\MeSHHeadingReport.csv");
        }

        /// <summary>
        /// Test the Grants report
        /// </summary>
        [Test]
        public void TestGrantsReport()
        {
            // Set up the database with test publications (and don't forget to add the 
            // publication types!)
            DB = new Database("Publication Harvester Unit Test");
            Harvester harvester = new Harvester(DB);
            harvester.CreateTables();
            PublicationTypes PubTypes = new PublicationTypes(
                AppDomain.CurrentDomain.BaseDirectory + "\\Unit Tests\\TestPublicationTypes",
                "PublicationTypes.csv"
                );
            PubTypes.WriteToDB(DB);
            reports = new Reports(DB, AppDomain.CurrentDomain.BaseDirectory + "\\Unit Tests\\TestReports\\pubmed_jifs.xls");
            Assert.IsTrue(reports.Weights.Count == 10);
            TestHarvester.GetPublicationsFromInput1XLS_Using_MockNCBI(false, new string[] { "eng" }, 22);

            // Write the grants report
            StreamWriter writer = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\GrantsReport.csv");
            reports.GrantsReport(writer);
            writer.Close();

            // Verify that the grants were written properly

            // Read the rows back from the file
            string ConnectionString =
                "Driver={Microsoft Text Driver (*.txt; *.csv)};Dbq="
                + AppDomain.CurrentDomain.BaseDirectory + ";";
            OdbcConnection Connection = new OdbcConnection(ConnectionString);
            OdbcDataAdapter DataAdapter = new OdbcDataAdapter
                ("SELECT * FROM [GrantsReport.csv]", Connection);
            DataTable Results = new DataTable();
            int Rows = DataAdapter.Fill(Results);
            Connection.Close();

            int numChecked = 0;

            // Check a few selected results
            foreach (DataRow Row in Results.Rows)
            {
                int Year = Convert.ToInt32(Row[0]);
                int PMID = Convert.ToInt32(Row[1]);
                string GrantID = Row[2].ToString();

                switch (PMID)
                {
                    case 3086749: // Guillemin
                        numChecked++;
                        Assert.IsTrue((Year == 1986) && (
                            (GrantID == "AM-18811/AM/NIADDK") ||
                            (GrantID == "HD-09690/HD/NICHD") ||
                            (GrantID == "MH-00663/MH/NIMH") ||
                            (GrantID == "AG 03106/AG/NIA") ||
                            (GrantID == "DK-26741/DK/NIDDK")));
                        break;

                    case 9049886: // Van Eys
                        numChecked++;
                        Assert.IsTrue((Year == 1997) && (GrantID == "RO1-CA33097/CA/NCI"));
                        break;
                }
            }
            Assert.IsTrue(numChecked == 6);

            File.Delete(AppDomain.CurrentDomain.BaseDirectory + "\\GrantsReport.csv");
        }
    }
}
