using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Com.StellmanGreene.PubMed;
using NUnit.Framework;

namespace SCGen.Unit_Tests
{
    /// <summary>
    /// Test the Colleagues class
    /// </summary>
    [TestFixture]
    public class TestColleagues
    {
        // Objects used in tests
        Roster roster;
        Database DB;
        Harvester harvester;
        PublicationTypes PubTypes;
        NCBI ncbi;

        /// <summary>
        /// Create a new database and populate it using the mock NCBI object from
        /// Unit Tests\TestColleagues\PeopleFile.xls -- note that we've seeded this
        /// file with four people whose publications will be found, and one whose
        /// publication won't be found. The program should handle all of those cases
        /// gracefully.
        /// </summary>
        [TestFixtureSetUp]
        public void TestColleaguesSetUp()
        {
            // Create the AAMC roster object
            roster = new Roster(AppDomain.CurrentDomain.BaseDirectory + "\\Unit Tests\\TestRoster\\testroster.csv");


            // Stuff for GetPublications()
            
            // Make an anonymous callback function that keeps track of the callback data
            Harvester.GetPublicationsStatus StatusCallback = delegate(int number, int total, int averageTime)
            {
                //
            };
            // Make an anonymous callback function to do nothing for GetPublicationsMessage
            Harvester.GetPublicationsMessage MessageCallback = delegate(string Message, bool StatusBarOnly)
            {
                //
            };
            // Make an anonymous callback function to return false for CheckForInterrupt
            Harvester.CheckForInterrupt InterruptCallback = delegate()
            {
                return false;
            };
            double AverageMilliseconds;

            

            // Read the people file
            People PeopleFromFile = new People(
                AppDomain.CurrentDomain.BaseDirectory + "\\Unit Tests\\TestColleagues",
                "PeopleFile.xls");

            // Drop all tables from the test database
            DB = new Database("Colleague Generator Unit Test");
            foreach (string Table in new string[] 
                { 
                    "colleaguepublications", "colleagues", "meshheadings",
                    "people", "peoplepublications", "publicationauthors",
                    "publicationgrants", "publicationmeshheadings", "publications",
                    "pubtypecategories", "starcolleagues"                
                }
                )
            {
                DB.ExecuteNonQuery("DROP TABLE IF EXISTS " + Table + ";");
            }

            // Create the test database
            harvester = new Harvester(DB);
            harvester.CreateTables();
            ColleagueFinder.CreateTables(DB, "ColleaguePublications");

            // Populate it using the Mock NCBI object
            ncbi = new MockNCBI("Medline");
            PubTypes = new PublicationTypes(
                AppDomain.CurrentDomain.BaseDirectory + "\\Unit Tests\\TestColleagues",
                "PublicationTypes.csv"
                );

            // Write each person and his publications to the database
            foreach (Person person in PeopleFromFile.PersonList) {
                person.WriteToDB(DB);
                harvester.GetPublications(ncbi, PubTypes, person, StatusCallback, MessageCallback, InterruptCallback, out AverageMilliseconds);
            }
        }



        /// <summary>
        /// Get the colleagues for Tobian
        /// </summary>
        [Test]
        public void GetColleaguesForTobian()
        {
            // We've seeded Tobian.dat with colleagues: 
            // Paul A. Bunn is in PMID 8931843. He has two articles in OtherPeople.dat, including 
            // article 8931843. (Also, PMID 15451956)
            ColleagueFinder finder = new ColleagueFinder(DB, roster, ncbi, "ColleaguePublications");
            People people = new People(DB);

            Person[] found;
            foreach (Person person in people.PersonList)
            {
                // Only look for a star's colleagues we've seeded that star's colleagues in the DAT files
                switch (person.Last) {
                    case "Tobian":
                        // Tobian has two apparent colleagues. Paul Bunn is a true colleague 
                        // who has publications in common with Tobian. Sharon J. Bintliff
                        // is a false colleague with no common publications.
                        found = finder.FindPotentialColleagues(person);
                        Assert.AreEqual(found.Length, 2);
                        Person PaulBunn = found[0];
                        Assert.AreEqual(PaulBunn.Setnb, "A4800524");
                        Assert.AreEqual(PaulBunn.Last, "BUNN");
                        Assert.AreEqual(PaulBunn.First, "PAUL");
                        Assert.AreEqual(PaulBunn.Middle, "A.");
                        Assert.AreEqual(PaulBunn.Names.Length, 4);
                        Assert.AreEqual(PaulBunn.Names[0], "bunn p jr");
                        Assert.AreEqual(PaulBunn.Names[1], "bunn pa jr");
                        Assert.AreEqual(PaulBunn.Names[2], "bunn pa");
                        Assert.AreEqual(PaulBunn.Names[3], "bunn p");
                        Assert.AreEqual(PaulBunn.MedlineSearch, "((\"bunn pa jr\"[au] or \"bunn p jr\"[au]) or ((\"bunn p\"[au] or \"bunn pa\"[au]) and (lymphoma or cancer)) and 1970:2005[dp])");
                        Person SharonBintliff = found[1];
                        Assert.AreEqual(SharonBintliff.Setnb, "A2700156");
                        Assert.AreEqual(SharonBintliff.Last, "BINTLIFF");
                        Assert.AreEqual(SharonBintliff.First, "SHARON");
                        Assert.AreEqual(SharonBintliff.Middle, "J");
                        Assert.AreEqual(SharonBintliff.Names.Length, 1);
                        Assert.AreEqual(SharonBintliff.Names[0], "bintliff sj");
                        Assert.AreEqual(SharonBintliff.MedlineSearch, "\"bintliff sj\"[au]");

                        // Make sure that Paul and Sharon were really added as a colleague
                        DataTable result = DB.ExecuteQuery("SELECT StarSetnb, Setnb FROM StarColleagues ORDER BY Setnb DESC");
                        Assert.AreEqual(result.Rows.Count, 2);
                        DataRow row = result.Rows[0];
                        Assert.AreEqual(row[0].ToString(), "A5401532");
                        Assert.AreEqual(row[1].ToString(), "A4800524");
                        row = result.Rows[1];
                        Assert.AreEqual(row[0].ToString(), "A5401532");
                        Assert.AreEqual(row[1].ToString(), "A2700156");

                        // Get the colleague's publications, make sure they're written to the database
                        finder.GetColleaguePublications(found, new string[] { "eng" }, new List<int> { 0, 1, 2, 3, 4, 5, 6 });
                        Assert.AreEqual(DB.GetIntValue("SELECT Count(*) FROM Colleagues"), 2);
                        Assert.AreEqual(DB.GetIntValue("SELECT Count(*) FROM StarColleagues"), 2);
                        Assert.AreEqual(DB.GetIntValue("SELECT Count(*) FROM ColleaguePublications"), 2);
                        Assert.AreEqual(DB.GetIntValue("SELECT Count(*) FROM ColleaguePublications WHERE PMID = 8931843"), 1);
                        Assert.AreEqual(DB.GetIntValue("SELECT Count(*) FROM ColleaguePublications WHERE PMID = 15451956"), 1);
                        
                        // Remove false colleagues, make sure Sharon was deleted
                        ColleagueFinder.RemoveFalseColleagues(DB, null, "ColleaguePublications");
                        result = DB.ExecuteQuery("SELECT StarSetnb, Setnb FROM StarColleagues ORDER BY Setnb DESC");
                        Assert.AreEqual(result.Rows.Count, 1);
                        row = result.Rows[0];
                        Assert.AreEqual(row[0].ToString(), "A5401532");
                        Assert.AreEqual(row[1].ToString(), "A4800524");

                        break;
                }
            }
        }


    }
}
