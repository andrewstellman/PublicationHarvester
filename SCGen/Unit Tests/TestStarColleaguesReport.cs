using System;
using System.Collections.Generic;
using System.Text;
using Com.StellmanGreene.PubMed;
using System.Collections;
using System.Data;
using System.IO;
using NUnit.Framework;

namespace SCGen.Unit_Tests
{
    /// <summary>
    /// Test the StarColleagues report
    /// </summary>
    [TestFixture]
    public class TestStarColleaguesReport
    {
        // Objects used in tests
        Database DB;
        Harvester harvester;
        PublicationTypes PubTypes;
        NCBI ncbi;
        Roster roster;

        /// <summary>
        /// Use the tests from TestColleagues to set up the database,
        /// then find the colleagues, get their publications and
        /// remove false colleagues.
        /// </summary>
        [OneTimeSetUp]
        public void TestStarColleaguesSetUp() {
            string[] Languages = { "eng" };
            DoSetUp(out DB, out harvester, out PubTypes, out ncbi, Languages);
            roster = new Roster(AppDomain.CurrentDomain.BaseDirectory + "\\Unit Tests\\TestRoster\\testroster.csv");
        }

        /// <summary>
        /// Use the tests from TestColleagues to set up the database,
        /// then find the colleagues, get their publications and
        /// remove false colleagues. 
        /// 
        /// This is a static void so that it can be called by other tests.
        /// </summary>
        public static void DoSetUp(out Database DB, out Harvester harvester, out PublicationTypes PubTypes, out NCBI ncbi, string[] Languages)
        {
            // First recreate the database
            DB = new Database("Colleague Generator Unit Test");
            ColleagueFinder.CreateTables(DB);

            // Then use the test fixture setup in TestColleagues to populate it
            TestColleagues testColleagues = new TestColleagues();
            testColleagues.TestColleaguesSetUp();

            // Write the publication types to the database
            PubTypes = new PublicationTypes(
                AppDomain.CurrentDomain.BaseDirectory + "\\Unit Tests\\TestColleagues",
                "PublicationTypes.csv"
                );
            PubTypes.WriteToDB(DB);

            // Create the other objects from the database
            harvester = new Harvester(DB);
            Roster roster = new Roster(AppDomain.CurrentDomain.BaseDirectory + "\\Unit Tests\\TestRoster\\testroster.csv");
            ncbi = new MockNCBI("Medline");

            // Find the colleagues and publications
            ColleagueFinder finder = new ColleagueFinder(DB, roster, ncbi, null);
            People people = new People(DB);
            foreach (Person person in people.PersonList)
            {
                Person[] found = finder.FindPotentialColleagues(person);
                if (found != null)
                    finder.GetColleaguePublications(found, new string[] { "eng" }, new List<int> { 1, 2, 3 });
            }

            // Remove false colleagues
            ColleagueFinder.RemoveFalseColleagues(DB, null, "PeoplePublications");


            // Create the extra articles for Bunn and Tobian.
            // Verify that Bunn and Tobian have five articles in common, with years
            // ranging from 1993 to 2001.
            CreateExtraArticlesForTobianAndBunn(DB, PubTypes, Languages);
            DataTable Result = DB.ExecuteQuery(
              @"SELECT p.Year, p.PMID, pp.PositionType AS StarPositionType, 
                       cp.PositionType AS ColleaguePositionType, p.Journal
                  FROM Publications p, ColleaguePublications cp, PeoplePublications pp
                 WHERE pp.Setnb = 'A5401532'
                   AND cp.Setnb = 'A4800524'
                   AND p.PMID = pp.PMID
                   AND p.PMID = cp.PMID
                 ORDER BY p.Year ASC");
            Assert.AreEqual(Result.Rows.Count, 5);
            Assert.AreEqual(Result.Rows[0]["Year"], 1993);
            Assert.AreEqual(Result.Rows[4]["Year"], 2001);
        }


        /// <summary>
        /// Generate a Star Colleagues report
        /// </summary>
        [Test]
        public void TestReportGeneration()
        {
            // The database now contains one star (Tobian, A5401532), with one 
            // colleague (Bunn, A4800524). Only Bunn is a true colleague, and he 
            // has one publication in common with Tobian.

            // Generate the report 
            string JournalWeightsFilename = AppDomain.CurrentDomain.BaseDirectory
                + "\\Unit Tests\\TestStarColleaguesReport\\"
                + "pubmed_jifs.xls";

            ArrayList SetnbsToSkip = new ArrayList();
            StringWriter writer = new StringWriter();
            StarColleaguesReport.Generate(writer, DB, JournalWeightsFilename, SetnbsToSkip, null, true);
            StringReader actual = new StringReader(writer.ToString());

            // Read the expected output from the test file
            StreamReader expected = new StreamReader(
                AppDomain.CurrentDomain.BaseDirectory
                + "\\Unit Tests\\TestStarColleaguesReport\\"
                + "TobianBunnReport.csv"
                );

            // Verify that each row was written correctly column by column (to provide
            // good debug data)
            string ActualLine;
            string ExpectedLine;
            int Row = 0;
            while ((ActualLine = actual.ReadLine()) != null)
            {
                Row++;
                string[] ActualValues = ActualLine.Split(new char[] { ',' });
                Assert.IsFalse(expected.EndOfStream);
                ExpectedLine = expected.ReadLine();
                string[] ExpectedValues = ExpectedLine.Split(new char[] { ',' });
                Assert.AreEqual(ActualValues.Length, ExpectedValues.Length, "Row " + Row.ToString() + " has incorrect number of columns\nExpected: " + ExpectedLine + "\n  Actual: " + ActualLine);
                for (int Col = 0; Col < ActualValues.Length; Col++)
                {
                    Assert.AreEqual(ActualValues[Col], ExpectedValues[Col], "Row " + Row.ToString() + ", column " + (Col + 1).ToString());
                }
            }
            Assert.IsTrue(expected.EndOfStream);
        }



        /// <summary>
        /// Generate an empty Star Colleagues report by skipping Bunn's Setnb
        /// </summary>
        [Test]
        public void TestSkipSetnbs()
        {
            // The database now contains one star (Tobian, A5401532), with one 
            // colleague (Bunn, A4800524). Only Bunn is a true colleague, and he 
            // has one publication in common with Tobian.

            // Generate the report 
            string JournalWeightsFilename = AppDomain.CurrentDomain.BaseDirectory
                + "\\Unit Tests\\TestStarColleaguesReport\\"
                + "pubmed_jifs.xls";

            // Add Bunn's setnb to SetnbsToSkip
            ArrayList SetnbsToSkip = new ArrayList();
            SetnbsToSkip.Add("A4800524");

            StringWriter writer = new StringWriter();
            StarColleaguesReport.Generate(writer, DB, JournalWeightsFilename, SetnbsToSkip, null, true);
            StringReader actual = new StringReader(writer.ToString());

            // Read the expected output from the test file
            StreamReader expected = new StreamReader(
                AppDomain.CurrentDomain.BaseDirectory
                + "\\Unit Tests\\TestStarColleaguesReport\\"
                + "TobianBunnReport.csv"
                );

            // Verify that the header is intact but that there are no other rows.
            string ActualLine;
            string ExpectedLine;
            ActualLine = actual.ReadLine();
            Assert.IsNotNull(ActualLine);
            string[] ActualValues = ActualLine.Split(new char[] { ',' });
            Assert.IsFalse(expected.EndOfStream);
            ExpectedLine = expected.ReadLine();
            string[] ExpectedValues = ExpectedLine.Split(new char[] { ',' });
            Assert.AreEqual(ActualValues.Length, ExpectedValues.Length);
            for (int i = 0; i < ActualValues.Length; i++)
            {
                Assert.AreEqual(ActualValues[i], ExpectedValues[i]);
            }

            // Verify that the report contains no other rows after the header.
            Assert.IsTrue(actual.ReadToEnd() == "");
        }

        
        /// <summary>
        /// Add five extra articles to the database for Tobian and Bunn
        /// </summary>
        public static void CreateExtraArticlesForTobianAndBunn(Database DB, PublicationTypes PubTypes, string[] Languages)
        {
            // Create people objects for Tobian and Bunn
            Person Tobian = new Person("A5401532", "Louis", "", "Tobian", true,
                new String[] { "tobian l", "tobian l jr", "tobian lj" },
                "(\"tobian l\"[au] OR \"tobian l jr\"[au] OR \"tobian lj\"[au])");

            Person Bunn = new Person("A4800524", "PAUL", "A.", "BUNN", true,
                new String[] { "bunn p jr", "bunn pa jr", "bunn pa", "bunn p" },
                "((\"bunn pa jr\"[au] or \"bunn p jr\"[au]) or ((\"bunn p\"[au] or \"bunn pa\"[au]) and (lymphoma or cancer)) and 1970:2005[dp])");

            // First, add a few more publications to the colleague.
            Publication pub = new Publication();
            string[] Authors = new String[] { "TOBIAN L", "BUNN P" };
            pub.Year = 1993;
            pub.Journal = "Fake Journal";
            pub.Authors = Authors;
            pub.PMID = 22222222;
            pub.Title = "Fake article #1";
            pub.Language = "eng";
            pub.PubType = "Journal Article";
            Publications.WriteToDB(pub, DB, PubTypes, Languages);
            Publications.WritePeoplePublicationsToDB(DB, Tobian, pub);
            ColleagueFinder.WriteColleaguePublicationsToDB(DB, Bunn, pub, PubTypes, new string[] { "eng" });

            pub = new Publication();
            Authors = new String[] { "BUNN P", "TOBIAN L" };
            pub.Year = 1996;
            pub.Journal = "Xenotransplantation";
            pub.Authors = Authors;
            pub.PMID = 12345678;
            pub.Title = "Fake article #2";
            pub.Language = "eng";
            pub.PubType = "Journal Article";
            Publications.WriteToDB(pub, DB, PubTypes, Languages);
            Publications.WritePeoplePublicationsToDB(DB, Tobian, pub);
            ColleagueFinder.WriteColleaguePublicationsToDB(DB, Bunn, pub, PubTypes, new string[] { "eng" });

            pub = new Publication();
            Authors = new String[] { "TOBIAN L", "BUNN P" };
            pub.Year = 1996;
            pub.Journal = "Fake Journal";
            pub.Authors = Authors;
            pub.PMID = 98765432;
            pub.Title = "Fake article #3";
            pub.Language = "eng";
            pub.PubType = "Journal Article";
            Publications.WriteToDB(pub, DB, PubTypes, Languages);
            Publications.WritePeoplePublicationsToDB(DB, Tobian, pub);
            ColleagueFinder.WriteColleaguePublicationsToDB(DB, Bunn, pub, PubTypes, new string[] { "eng" });

            pub = new Publication();
            Authors = new String[] { "TOBIAN L", "BUNN P", "SCHMOE J" };
            pub.Year = 2001;
            pub.Journal = "Nature";
            pub.Authors = Authors;
            pub.PMID = 55555555;
            pub.Title = "Fake article #4";
            pub.Language = "eng";
            pub.PubType = "Journal Article";
            Publications.WriteToDB(pub, DB, PubTypes, Languages);
            Publications.WritePeoplePublicationsToDB(DB, Tobian, pub);
            ColleagueFinder.WriteColleaguePublicationsToDB(DB, Bunn, pub, PubTypes, new string[] { "eng" });
        }

    }
}
