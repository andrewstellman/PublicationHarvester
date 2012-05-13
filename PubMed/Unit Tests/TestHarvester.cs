/*
 *                           Publication Harvester
 *              Copyright (c) 2003-2006 Stellman & Greene Consulting
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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using NUnit.Framework;

namespace Com.StellmanGreene.PubMed.Unit_Tests
{
    /// <summary>
    /// Test the harvester class
    /// Note that CreateTables() is tested thoroughly in the other unit tests
    /// </summary>
    [TestFixture]
    public class TestHarvester
    {


        /// <summary>
        /// Set up the database with data from Input1.XLS using the Mock NCBI object
        /// (this is also called from TestReports())
        /// </summary>
        /// <param name="NCBISearchThrowsAnError">True if the MockNCBI object is supposed to throw an error</param>
        public static void GetPublicationsFromInput1XLS_Using_MockNCBI(bool NCBISearchThrowsAnError, string[] Languages, int ExpectedPublications)
        {
            bool TablesCreated;
            int NumPeople;
            int NumHarvestedPeople;
            int NumPublications;
            int NumErrors;

            Database DB = new Database("Publication Harvester Unit Test");

            // Drop all tables and make sure the database reports as empty 
            foreach (string Table in new string[] {
                "meshheadings", "people", "peoplepublications", "publicationauthors",
                "publicationmeshheadings", "publications", "pubtypecategories"
            })
            {
                DB.ExecuteNonQuery("DROP TABLE IF EXISTS " + Table);
                DB.GetStatus(out TablesCreated, out NumPeople, out NumHarvestedPeople, out NumPublications, out NumErrors);
                Assert.IsFalse(TablesCreated);
                Assert.AreEqual(NumPeople, 0);
                Assert.AreEqual(NumHarvestedPeople, 0);
                Assert.AreEqual(NumPublications, 0);
                Assert.AreEqual(NumErrors, 0);
            }

            // Create and populate the tables
            Harvester harvester = new Harvester(DB);
            harvester.Languages = Languages;
            MockNCBI mockNCBI = new MockNCBI("medline");
            mockNCBI.SearchThrowsAnError = NCBISearchThrowsAnError;
            PublicationTypes ptc = new PublicationTypes(
                AppDomain.CurrentDomain.BaseDirectory + "\\Unit Tests\\TestPublicationTypes",
                "PublicationTypes.csv"
            );

            // Reinitialize the database
            harvester.CreateTables();
            ptc.WriteToDB(DB);

            // Retrieve the publications for each person in input1.xls using GetPublications()
            People PeopleFromFile = new People(
                AppDomain.CurrentDomain.BaseDirectory + "\\Unit Tests\\TestPeople",
                "input1.xls");

            // Make an anonymous callback function that keeps track of the callback data
            int Callbacks = 0; // this will count all of the publications
            Harvester.GetPublicationsStatus StatusCallback = delegate(int number, int total, int averageTime)
            {
                Callbacks++;
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


            // Verify that the database was created and populated properly
            DB.GetStatus(out TablesCreated, out NumPeople, out NumHarvestedPeople, out NumPublications, out NumErrors);
            Assert.IsTrue(TablesCreated);
            Assert.AreEqual(NumPeople, 0);
            Assert.AreEqual(NumHarvestedPeople, 0);
            Assert.AreEqual(NumPublications, 0);
            Assert.AreEqual(NumErrors, 0);

            int PeopleCount = 0;
            int HarvestedCount = 0;
            int PubCount = 0;
            foreach (Person person in PeopleFromFile.PersonList)
            {
                double AverageMilliseconds;

                // First write the person to the database
                person.WriteToDB(DB);
                PeopleCount++;

                // Check that the database status is updated properly
                DB.GetStatus(out TablesCreated, out NumPeople, out NumHarvestedPeople, out NumPublications, out NumErrors);
                Assert.IsTrue(TablesCreated);
                Assert.AreEqual(NumPeople, PeopleCount);
                if (!NCBISearchThrowsAnError)
                    Assert.AreEqual(NumHarvestedPeople, HarvestedCount);
                else
                    Assert.AreEqual(NumHarvestedPeople, 0);
                Assert.AreEqual(NumPublications, PubCount);
                if (!NCBISearchThrowsAnError)
                    Assert.AreEqual(NumErrors, 0);
                else
                    Assert.AreEqual(NumErrors, PeopleCount - 1);

                // Harvest the person's publications
                PubCount += harvester.GetPublications(mockNCBI, ptc, person, StatusCallback, MessageCallback, InterruptCallback, out AverageMilliseconds);
                HarvestedCount++;

                // Check the status again after the people were harvested
                DB.GetStatus(out TablesCreated, out NumPeople, out NumHarvestedPeople, out NumPublications, out NumErrors);
                Assert.IsTrue(TablesCreated);
                Assert.AreEqual(NumPeople, PeopleCount);
                if (!NCBISearchThrowsAnError)
                    Assert.AreEqual(NumHarvestedPeople, HarvestedCount);
                else
                    Assert.AreEqual(NumHarvestedPeople, 0);
                Assert.AreEqual(NumPublications, PubCount);
                if (!NCBISearchThrowsAnError)
                    Assert.AreEqual(NumErrors, 0);
                else
                    Assert.AreEqual(NumErrors, PeopleCount);
            }

            // Verify that the database was written properly
            if (!NCBISearchThrowsAnError)
                Assert.IsTrue(Callbacks == 24);
            else
                Assert.IsTrue(Callbacks == 0);
            DB.GetStatus(out TablesCreated, out NumPeople, out NumHarvestedPeople, out NumPublications, out NumErrors);
            Assert.IsTrue(TablesCreated);
            Assert.AreEqual(NumPeople, 4);
            if (!NCBISearchThrowsAnError)
            {
                Assert.AreEqual(NumHarvestedPeople, 4);
                Assert.AreEqual(NumPublications, ExpectedPublications);
                Assert.AreEqual(NumErrors, 0);
            }
            else
            {
                Assert.AreEqual(NumHarvestedPeople, 0);
                Assert.AreEqual(NumPublications, 0);
                Assert.AreEqual(NumErrors, 4);
            }
        }




        /// <summary>
        /// Import people from a file, then verify they were written correcty
        /// Use input1.xls from TestPeople
        /// </summary>
        [Test]
        public void GetPeopleFromInputXLS()
        {
            // Import input1.xls into the database
            Database DB = new Database("Publication Harvester Unit Test");
            Harvester harvester = new Harvester(DB);
            harvester.CreateTables();
            harvester.ImportPeople(AppDomain.CurrentDomain.BaseDirectory
                + "\\Unit Tests\\TestPeople\\input1.xls");
            DataTable Results = DB.ExecuteQuery(
                @"SELECT Setnb, First, Middle, Last, Name1, Name2, Name3, Name4, 
                        MedlineSearch, Harvested, Error, ErrorMessage
                   FROM People"
            );

            // Test each person 
            for (int Row = 0; Row < Results.Rows.Count; Row++)
            {
                Person person = new Person(Results.Rows[Row], Results.Columns);
                switch (person.Setnb)
                {
                    case "A6009400":
                        Assert.IsTrue(person.First == "Jan");
                        Assert.IsTrue(person.Middle == "");
                        Assert.IsTrue(person.Last == "Van Eys");
                        Assert.IsTrue(person.Names.Length == 3);
                        Assert.IsTrue(person.Names[0] == "van eys j");
                        Assert.IsTrue(person.Names[1] == "vaneys j");
                        Assert.IsTrue(person.Names[2] == "eys jv");
                        Assert.IsTrue(person.MedlineSearch == "(\"van eys j\"[au] OR \"vaneys j\"[au] OR \"eys jv\"[au])");
                        break;
                    case "A5401532":
                        Assert.IsTrue(person.First == "Louis");
                        Assert.IsTrue(person.Middle == "");
                        Assert.IsTrue(person.Last == "Tobian");
                        Assert.IsTrue(person.Names.Length == 3);
                        Assert.IsTrue(person.Names[0] == "tobian l");
                        Assert.IsTrue(person.Names[1] == "tobian l jr");
                        Assert.IsTrue(person.Names[2] == "tobian lj");
                        Assert.IsTrue(person.MedlineSearch == "(\"tobian l\"[au] OR \"tobian l jr\"[au] OR \"tobian lj\"[au])");
                        break;
                    case "A5501586":
                        Assert.IsTrue(person.First == "Keith");
                        Assert.IsTrue(person.Middle == "B");
                        Assert.IsTrue(person.Last == "Reemtsma");
                        Assert.IsTrue(person.Names.Length == 4);
                        Assert.IsTrue(person.Names[0] == "reemtsma k");
                        Assert.IsTrue(person.Names[1] == "reemtsma kb");
                        Assert.IsTrue(person.Names[2] == "test data");
                        Assert.IsTrue(person.Names[3] == "more test data");
                        Assert.IsTrue(person.MedlineSearch == "((\"reemtsma k\"[au] OR \"reemtsma kb\"[au]) AND 1956:2000[dp])");
                        break;
                    case "A5702471":
                        Assert.IsTrue(person.First == "Roger");
                        Assert.IsTrue(person.Middle == "");
                        Assert.IsTrue(person.Last == "Guillemin");
                        Assert.IsTrue(person.Names.Length == 2);
                        Assert.IsTrue(person.Names[0] == "guillemin r");
                        Assert.IsTrue(person.Names[1] == "guillemin rc");
                        Assert.IsTrue(person.MedlineSearch ==
                                          "(\"guillemin rc\"[au] OR (\"guillemin r\"[au] NOT (Electrodiagn Ther[ta] OR Phys Rev Lett[ta] OR vegas[ad] OR lindle[au])))"
                                      );
                        break;
                }
            }
        }


        /// <summary>
        /// Test GetPublications using a mock NCBI object
        /// </summary>
        [Test]
        public void GetPublications()
        {
            // Set up the database
            TestHarvester.GetPublicationsFromInput1XLS_Using_MockNCBI(false, new string[] { "eng" }, 22);

            Database DB = new Database("Publication Harvester Unit Test");
            PublicationTypes ptc = new PublicationTypes(
                AppDomain.CurrentDomain.BaseDirectory + "\\Unit Tests\\TestPublicationTypes",
                "PublicationTypes.csv"
                );

            // Verify the correct publications were written (including publication type and author position)           
            People people = new People(DB);
            Assert.IsTrue(people.PersonList.Count == 4);
            int FoundPublications = 0;
            foreach (Person person in people.PersonList)
            {
                Publications pubs = new Publications(DB, person, false);
                switch (person.Setnb)
                {
                    case "A6009400": // Van Eys
                        Assert.IsTrue(pubs.PublicationList.Length == 8);
                        foreach (Publication pub in pubs.PublicationList)
                            switch (pub.PMID)
                            {
                                case 9876482:
                                    FoundPublications++;
                                    Assert.IsTrue(pub.Title == "Benefits of nutritional intervention on nutritional status, quality of life and survival.");
                                    Assert.IsTrue(pub.Pages == "66-8");
                                    Assert.IsTrue(pub.Year == 1998);
                                    Assert.IsTrue(pub.Month == null);
                                    Assert.IsTrue(pub.Day == null);
                                    Assert.IsTrue(pub.Journal == "Int J Cancer Suppl");
                                    Assert.IsTrue(pub.Volume == "11");
                                    Assert.IsTrue(pub.Issue == null);

                                    Assert.IsTrue(pub.Authors.Length == 1);
                                    Assert.IsTrue(pub.Authors[0] == "Van Eys J");

                                    // Verify publication type
                                    Assert.IsTrue(pub.PubType == "Journal Article");
                                    Assert.IsTrue(ptc.GetCategoryNumber(pub.PubType) == 3);

                                    // Verify MeSH headings
                                    Assert.IsTrue(pub.MeSHHeadings.Count == 8);
                                    Assert.IsTrue(pub.MeSHHeadings.Contains("Child"));
                                    Assert.IsTrue(pub.MeSHHeadings.Contains("Nutrition Disorders/*complications/*therapy"));
                                    Assert.IsTrue(pub.MeSHHeadings.Contains("Survival Rate"));

                                    // Verify position type
                                    Harvester.AuthorPositions PositionType;
                                    int AuthorPosition = Publications.GetAuthorPosition(DB, pub.PMID, person, out PositionType, "PeoplePublications");
                                    Assert.IsTrue(AuthorPosition == 1);
                                    Assert.IsTrue(PositionType == Harvester.AuthorPositions.First);
                                    AuthorPosition = person.GetAuthorPosition(DB, pub, out PositionType);
                                    Assert.IsTrue(AuthorPosition == 1);
                                    Assert.IsTrue(PositionType == Harvester.AuthorPositions.First);
                                    break;



                                case 8403744:
                                    FoundPublications++;
                                    Assert.IsTrue(pub.Title == "Early hospital discharge and the timing of newborn metabolic screening.");
                                    Assert.IsTrue(pub.Pages == "463-6");
                                    Assert.IsTrue(pub.Year == 1993);
                                    Assert.IsTrue(pub.Month == "Aug");
                                    Assert.IsTrue(pub.Day == null);
                                    Assert.IsTrue(pub.Journal == "Clin Pediatr (Phila)");
                                    Assert.IsTrue(pub.Volume == "32");
                                    Assert.IsTrue(pub.Issue == "8");

                                    Assert.IsTrue(pub.Authors.Length == 4);
                                    Assert.IsTrue(pub.Authors[0] == "Coody D");
                                    Assert.IsTrue(pub.Authors[1] == "Yetman RJ");
                                    Assert.IsTrue(pub.Authors[2] == "Montgomery D");
                                    Assert.IsTrue(pub.Authors[3] == "van Eys J");

                                    // Verify publication type
                                    Assert.IsTrue(pub.PubType == "Consensus Development Conference, NIH");
                                    Assert.IsTrue(ptc.GetCategoryNumber(pub.PubType) == 1);

                                    // Verify MeSH headings
                                    Assert.IsTrue(pub.MeSHHeadings.Count == 15);
                                    Assert.IsTrue(pub.MeSHHeadings.Contains("Cesarean Section"));
                                    Assert.IsTrue(pub.MeSHHeadings.Contains("Hospitals, Private"));
                                    Assert.IsTrue(pub.MeSHHeadings.Contains("*Insurance, Health"));
                                    Assert.IsTrue(pub.MeSHHeadings.Contains("United States"));

                                    // Verify position type
                                    AuthorPosition = Publications.GetAuthorPosition(DB, pub.PMID, person, out PositionType, "PeoplePublications");
                                    Assert.IsTrue(AuthorPosition == 4);
                                    Assert.IsTrue(PositionType == Harvester.AuthorPositions.Last);
                                    AuthorPosition = person.GetAuthorPosition(DB, pub, out PositionType);
                                    Assert.IsTrue(AuthorPosition == 4);
                                    Assert.IsTrue(PositionType == Harvester.AuthorPositions.Last);
                                    break;
                            }
                        break;
                    case "A5401532": // Tobian
                        Assert.IsTrue(pubs.PublicationList.Length == 5);
                        foreach (Publication pub in pubs.PublicationList)
                            switch (pub.PMID)
                            {
                                case 9931073:
                                    FoundPublications++;
                                    Assert.IsTrue(pub.Title == "Story of the birth of the journal called Hypertension.");
                                    Assert.IsTrue(pub.Pages == "7");
                                    Assert.IsTrue(pub.Year == 1999);
                                    Assert.IsTrue(pub.Month == "Jan");
                                    Assert.IsTrue(pub.Day == null);
                                    Assert.IsTrue(pub.Volume == "33");
                                    Assert.IsTrue(pub.Issue == "1");

                                    Assert.IsTrue(pub.Authors.Length == 1);
                                    Assert.IsTrue(pub.Authors[0] == "Tobian L");

                                    // Verify publication type
                                    Assert.IsTrue(pub.PubType == "Historical Article");
                                    Assert.IsTrue(ptc.GetCategoryNumber(pub.PubType) == 0);

                                    // Verify MeSH headings
                                    Assert.IsTrue(pub.MeSHHeadings.Count == 5);
                                    Assert.IsTrue(pub.MeSHHeadings.Contains("American Heart Association/*history"));
                                    Assert.IsTrue(pub.MeSHHeadings.Contains("*Hypertension"));
                                    Assert.IsTrue(pub.MeSHHeadings.Contains("United States"));

                                    // Verify position type
                                    Harvester.AuthorPositions PositionType;
                                    int AuthorPosition = Publications.GetAuthorPosition(DB, pub.PMID, person, out PositionType, "PeoplePublications");
                                    Assert.IsTrue(AuthorPosition == 1);
                                    Assert.IsTrue(PositionType == Harvester.AuthorPositions.First);
                                    AuthorPosition = person.GetAuthorPosition(DB, pub, out PositionType);
                                    Assert.IsTrue(AuthorPosition == 1);
                                    Assert.IsTrue(PositionType == Harvester.AuthorPositions.First);
                                    break;
                            }
                        break;

                    case "A5501586": // Reemtsma
                        Assert.IsTrue(pubs.PublicationList.Length == 3);
                        foreach (Publication pub in pubs.PublicationList)
                            switch (pub.PMID)
                            {
                                case 11528018:
                                    FoundPublications++;
                                    Assert.IsTrue(pub.Title == "Xenotransplantation: A Historical Perspective.");
                                    Assert.IsTrue(pub.Pages == "9-12");
                                    Assert.IsTrue(pub.Year == 1995);
                                    Assert.IsTrue(pub.Month == null);
                                    Assert.IsTrue(pub.Day == null);
                                    Assert.IsTrue(pub.Volume == "37");
                                    Assert.IsTrue(pub.Issue == "1");

                                    Assert.IsTrue(pub.Authors.Length == 1);
                                    Assert.IsTrue(pub.Authors[0] == "Reemtsma K");

                                    // Verify publication type
                                    Assert.IsTrue(pub.PubType == "JOURNAL ARTICLE");
                                    Assert.IsTrue(ptc.GetCategoryNumber(pub.PubType) == 3);

                                    // Verify MeSH headings
                                    Assert.IsTrue(pub.MeSHHeadings == null);

                                    // Verify position type
                                    Harvester.AuthorPositions PositionType;
                                    int AuthorPosition = Publications.GetAuthorPosition(DB, pub.PMID, person, out PositionType, "PeoplePublications");
                                    Assert.IsTrue(AuthorPosition == 1);
                                    Assert.IsTrue(PositionType == Harvester.AuthorPositions.First);
                                    AuthorPosition = person.GetAuthorPosition(DB, pub, out PositionType);
                                    Assert.IsTrue(AuthorPosition == 1);
                                    Assert.IsTrue(PositionType == Harvester.AuthorPositions.First);
                                    break;
                            }
                        break;



                    case "A5702471": // Guillemin
                        Assert.IsTrue(pubs.PublicationList.Length == 6);
                        foreach (Publication pub in pubs.PublicationList)
                            switch (pub.PMID)
                            {
                                case 15642779:
                                    // For this publication, we're just concerned that
                                    // the publication type is "Review" -- even though
                                    // it's the second publication type in the citation,
                                    // it's flagged as an "override first pubtype" 
                                    // in PublicationTypes.csv
                                    Assert.IsTrue(pub.PubType == "Review");
                                    break;


                                    // NOTE: The title has a quote (laureates') that gets stripped off
                                case 12462241:
                                    FoundPublications++;
                                    Assert.IsTrue(pub.Title == "Nobel laureates letter to President Bush."); 
                                    Assert.IsTrue(pub.Pages == "A02");
                                    Assert.IsTrue(pub.Year == 2001);
                                    Assert.IsTrue(pub.Month == "Feb");
                                    Assert.IsTrue(pub.Day == "22");
                                    Assert.IsTrue(pub.Journal == "Washington Post");
                                    Assert.IsTrue(pub.Volume == null);
                                    Assert.IsTrue(pub.Issue == null);

                                    Assert.IsTrue(pub.Authors.Length == 82);
                                    Assert.IsTrue(pub.Authors[0] == "Arrow KJ");
                                    Assert.IsTrue(pub.Authors[26] == "Guillemin R");
                                    Assert.IsTrue(pub.Authors[81] == "Wilson RW");

                                    // Verify publication type
                                    Assert.IsTrue(pub.PubType == "Newspaper Article");
                                    Assert.IsTrue(ptc.GetCategoryNumber(pub.PubType) == 0);

                                    // Verify MeSH headings
                                    Assert.IsTrue(pub.MeSHHeadings.Count == 9);
                                    Assert.IsTrue(pub.MeSHHeadings.Contains("Embryo Disposition"));
                                    Assert.IsTrue(pub.MeSHHeadings.Contains("National Institutes of Health (U.S.)"));
                                    Assert.IsTrue(pub.MeSHHeadings.Contains("United States"));

                                    // Verify position type
                                    Harvester.AuthorPositions PositionType;
                                    int AuthorPosition = Publications.GetAuthorPosition(DB, pub.PMID, person, out PositionType, "PeoplePublications");
                                    Assert.IsTrue(AuthorPosition == 27);
                                    Assert.IsTrue(PositionType == Harvester.AuthorPositions.Middle);
                                    AuthorPosition = person.GetAuthorPosition(DB, pub, out PositionType);
                                    Assert.IsTrue(AuthorPosition == 27);
                                    Assert.IsTrue(PositionType == Harvester.AuthorPositions.Middle);
                                    break;


                                case 3086749:
                                    // This publication was altered to contain six GrantIDs in order to
                                    // test the GrantID column length in the database
                                    FoundPublications++;
                                    Assert.IsTrue(pub.Title == "Pituitary FSH is released by a heterodimer of the beta-subunits from the two forms of inhibin.");
                                    Assert.IsTrue(pub.Pages == "779-82");
                                    Assert.IsTrue(pub.Year == 1986);
                                    Assert.IsTrue(pub.Month == "Jun");
                                    Assert.IsTrue(pub.Day == "19-25");
                                    Assert.IsTrue(pub.Journal == "Nature");
                                    Assert.IsTrue(pub.Volume == "321");
                                    Assert.IsTrue(pub.Issue == "6072");

                                    Assert.IsTrue(pub.Authors.Length == 7);
                                    Assert.IsTrue(pub.Authors[0] == "Ling N");
                                    Assert.IsTrue(pub.Authors[4] == "Esch F");
                                    Assert.IsTrue(pub.Authors[6] == "Guillemin R");

                                    // Verify publication type
                                    Assert.IsTrue(pub.PubType == "Journal Article");
                                    Assert.IsTrue(ptc.GetCategoryNumber(pub.PubType) == 3);

                                    // Verify MeSH headings
                                    Assert.IsTrue(pub.MeSHHeadings.Count == 14);
                                    Assert.IsTrue(pub.MeSHHeadings.Contains("Amino Acid Sequence"));
                                    Assert.IsTrue(pub.MeSHHeadings.Contains("Follicle Stimulating Hormone/*secretion"));
                                    Assert.IsTrue(pub.MeSHHeadings.Contains("Swine"));

                                    // Verify position type
                                    AuthorPosition = Publications.GetAuthorPosition(DB, pub.PMID, person, out PositionType, "PeoplePublications");
                                    Assert.IsTrue(AuthorPosition == 7);
                                    Assert.IsTrue(PositionType == Harvester.AuthorPositions.Last);
                                    AuthorPosition = person.GetAuthorPosition(DB, pub, out PositionType);
                                    Assert.IsTrue(AuthorPosition == 7);
                                    Assert.IsTrue(PositionType == Harvester.AuthorPositions.Last);
                                    break;
                            }
                        break;
                }
            }
            Assert.IsTrue(FoundPublications == 6);

            // Verify that People.Harvested has been updated for each person
            DataTable Results = DB.ExecuteQuery("SELECT Setnb, Harvested FROM People");
            Assert.IsTrue(Results.Rows.Count == 4);
            foreach (DataRow Row in Results.Rows)
            {
                Assert.IsTrue((bool)Row["Harvested"] == true);
            }
        }




        /// <summary>
        /// Use OtherPeople.dat to make sure the data is processed properly
        /// OtherPeople.dat contains Medline citations for boundary cases
        /// that required bug fixes.
        /// </summary>
        [Test]
        public void TestOtherPeople()
        {
            // Note: Publication 14560782 was added to OtherPeople.dat to verify
            // that the software handles the situation where a publication has
            // no authors listed.

            Database DB = new Database("Publication Harvester Unit Test");
            Harvester harvester = new Harvester(DB);
            MockNCBI mockNCBI = new MockNCBI("medline");
            PublicationTypes ptc = new PublicationTypes(
                AppDomain.CurrentDomain.BaseDirectory + "\\Unit Tests\\TestPublicationTypes",
                "PublicationTypes.csv"
                );

            // Reinitialize the database
            harvester.CreateTables();

            // Make an anonymous callback function that keeps track of the callback data
            int Callbacks = 0; // this will count all of the publications
            Harvester.GetPublicationsStatus StatusCallback = delegate(int number, int total, int averageTime)
            {
                Callbacks++;
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

            // Create a new person to test
            string[] names = new string[2];
            names[0] = "Klein RG";
            names[1] = "Guillemin R";
            Person person = new Person("A1234567", "FIRST", "MIDDLE", "LAST",
                false, names, "Special query for OtherPeople.dat");
            person.WriteToDB(DB);
            double AverageMilliseconds;
            harvester.GetPublications(mockNCBI, ptc, person, StatusCallback, MessageCallback, InterruptCallback, out AverageMilliseconds);

            // Verify that the data was written properly
            int FoundPublications = 0;
            Publications pubs = new Publications(DB, person, false);
            foreach (Publication pub in pubs.PublicationList)
            {
                FoundPublications++;
                switch (pub.PMID)
                {
                    case 12679283:
                        // The weird part of this publication is the second MeSH heading, which is very long
                        Assert.IsTrue(pub.MeSHHeadings.Count == 23);
                        Assert.IsTrue(pub.MeSHHeadings.Contains(
                            "Attention Deficit and Disruptive Behavior Disorders/etiology/*prevention & control/psychology"));
                        break;
                    case 2417121:
                        // The weird part of this publication is the date, which has a weird format that causes 
                        // the day to be long
                        Assert.IsTrue(pub.Day == "19-1986 Jan 1");
                        break;
                    case 6148773:
                        // One of the headers is long
                        Assert.IsTrue(pub.MeSHHeadings.Contains("Peptide Fragments/antagonists & inhibitors/chemical synthesis/diagnostic use/isolation & purification/pharmacology/*physiology"));
                        break;
                    case 16291338:
                        // One of the authors is long
                        Assert.IsTrue(pub.Authors.Length == 8);
                        Assert.IsTrue(pub.Authors[7] == "For The Michigan Alliance For The National Children's Study");
                        break;
                    case 15451956:
                        // Volume is long
                        Assert.IsTrue(pub.Volume == "Suppl Web Exclusives");
                        break;
                    case 14653276:
                        // Issue is long
                        Assert.IsTrue(pub.Issue == "5 Suppl Nitric Oxide");
                        break;
                    case 9965612:
                        // Journal name is long
                        Assert.IsTrue(pub.Journal == "PHYSICAL REVIEW. E. STATISTICAL PHYSICS, PLASMAS, FLUIDS, AND RELATED INTERDISCIPLINARY TOPICS");
                        break;
                    case 9469584:
                        // Title is long
                        Assert.IsTrue(pub.Title ==
                            Database.Left("Down-regulation of cholesterol biosynthesis in sitosterolemia: diminished activities of acetoacetyl-CoA thiolase, 3-hydroxy-3-methylglutaryl-CoA synthase, reductase, squalene synthase, and 7-dehydrocholesterol delta7-reductase in liver and mononuclear leukocytes."
                            , 244));
                        break;
                    case 2545230:
                        // Month is long
                        Assert.IsTrue(pub.Month == "Spring-Summer");
                        break;
                    default:
                        break;
                }
            }
            Assert.IsTrue(FoundPublications == 13);
        }


        /// <summary>
        /// Verify that ClearDataAfterInterruption() actually clears data properly
        /// (using data inserted in Test
        /// </summary>
        [Test]
        public void TestClearingDataAfterInterruption()
        {
            // Set up the database 
            TestHarvester.GetPublicationsFromInput1XLS_Using_MockNCBI(false, new string[] { "eng" }, 22);

            Database DB = new Database("Publication Harvester Unit Test");
            Harvester harvester = new Harvester(DB);

            // Add a grant for publication 12462241, since it doesn't have one
            ArrayList Parameters = new ArrayList();
            Parameters.Add(Database.Parameter(12462241));
            Parameters.Add(Database.Parameter("Fake grant ID"));
            DB.ExecuteNonQuery("INSERT INTO PublicationGrants (PMID, GrantID) VALUES ( ? , ? )", Parameters);


            // Verify that InterruptedDataExists and UnharvestedPeopleExist both return false
            Assert.IsFalse(harvester.InterruptedDataExists());
            Assert.IsFalse(harvester.UnharvestedPeopleExist());

            // Verify that there are publications for Tobian (setnb A5401532)
            // (there should be 5 publications)

            Assert.IsTrue(DB.GetIntValue(
                @"SELECT Count(*) 
                    FROM PeoplePublications
                   WHERE Setnb = 'A5401532'") == 5);
            int TotalPublicationsBeforeClear
                = DB.GetIntValue("SELECT Count(*) FROM PeoplePublications");

            // Verify that there are authors and MeSH headings for publication 12462241
            // (which belongs to Guillemin, and should have 82 authors and 9 headings)

            Assert.IsTrue(DB.GetIntValue(
                @"SELECT Count(*) 
                    FROM PublicationMeSHHeadings
                   WHERE PMID = 12462241") == 9);
            int TotalHeadingsBeforeClear
                = DB.GetIntValue("SELECT Count(*) FROM PublicationMeSHHeadings");

            Assert.IsTrue(DB.GetIntValue(
                @"SELECT Count(*) 
                    FROM PublicationAuthors
                   WHERE PMID = 12462241") == 82);
            int TotalAuthorsBeforeClear
                = DB.GetIntValue("SELECT Count(*) FROM PublicationAuthors");

            Assert.IsTrue(DB.GetIntValue(
                @"SELECT Count(*) 
                    FROM PublicationGrants
                   WHERE PMID = 12462241") == 1);
            int TotalGrantsBeforeClear
                = DB.GetIntValue("SELECT Count(*) FROM PublicationGrants");

            // Set Tobian's Harvested to 0
            DB.ExecuteNonQuery("UPDATE People SET Harvested = 0 WHERE Setnb = 'A5401532'");

            // Remove publication 12462241
            DB.ExecuteNonQuery("DELETE FROM Publications WHERE PMID = 12462241");


            // Verify that InterruptedDataExists and UnharvestedPeopleExist both return true
            Assert.IsTrue(harvester.InterruptedDataExists());
            Assert.IsTrue(harvester.UnharvestedPeopleExist());

            // Execute ClearDataAfterInterruption
            harvester.ClearDataAfterInterruption();

            // Verify that InterruptedDataExists returns false again, now that
            // the intterrupted data has been cleared, but that there are still
            // unharvested people
            Assert.IsFalse(harvester.InterruptedDataExists());
            Assert.IsTrue(harvester.UnharvestedPeopleExist());

            // Verify that only Tobian's publications have been removed
            Assert.IsTrue(
                DB.GetIntValue("SELECT Count(*) FROM PeoplePublications")
                  == TotalPublicationsBeforeClear - 5);

            // Verify that only publication 12462241's grants, headings and authors have been removed
            Assert.IsTrue(
                DB.GetIntValue("SELECT Count(*) FROM PublicationMeSHHeadings")
                  == TotalHeadingsBeforeClear - 9);

            Assert.IsTrue(
                DB.GetIntValue("SELECT Count(*) FROM PublicationAuthors")
                  == TotalAuthorsBeforeClear - 82);

            Assert.IsTrue(
                DB.GetIntValue("SELECT Count(*) FROM PublicationGrants")
                  == TotalGrantsBeforeClear - 1);
        }

        /// <summary>
        /// Verify that when the NCBI object throws an error, the correct error
        /// values are recorded in the database.
        /// </summary>
        [Test]
        public void TestNCBIError()
        {
            // Set up the database -- tell MockNCBI to throw an error
            // This function does all of the necessary testing.
            TestHarvester.GetPublicationsFromInput1XLS_Using_MockNCBI(false, new string[] { "eng" }, 22);
        }

        /// <summary>
        /// Verify that two people with the same names and search criteria will
        /// both be retrieved when one of them is harvested
        /// </summary>
        [Test]
        public void TestTwoPeopleWithSameNames()
        {
            Database DB = new Database("Publication Harvester Unit Test");

            // Set up the database
            TestHarvester.GetPublicationsFromInput1XLS_Using_MockNCBI(false, new string[] { "eng" }, 22);


            // Add two people to the database with the same names and search criteria
            // (where the search should make MockNCBI use OtherPeople.dat)
            string[] names = new string[2];
            names[0] = "Guy JF";
            names[1] = "Guy J";
            Person Joe = new Person("A1234567", "JOE", "FIRST", "GUY",
                false, names, "Special query for OtherPeople.dat");
            Joe.WriteToDB(DB);

            Person Jane = new Person("Z7654321", "JANE", "FIFTH", "GUY",
                false, names, "Special query for OtherPeople.dat");
            Jane.WriteToDB(DB);


            // Also add Jim, but give him an error so we can make sure it's cleared
            Person Jim = new Person("Q2222222", "JIM", "FOURTEENTH", "GUY",
                false, names, "Special query for OtherPeople.dat");
            Jim.WriteToDB(DB);
            Jim.WriteErrorToDB(DB, "This is an error message");


            PublicationTypes ptc = new PublicationTypes(
                AppDomain.CurrentDomain.BaseDirectory + "\\Unit Tests\\TestPublicationTypes",
                "PublicationTypes.csv"
                );


            // Make an anonymous callback function that keeps track of the callback data
            int Callbacks = 0; // this will count all of the publications
            Harvester.GetPublicationsStatus StatusCallback = delegate(int number, int total, int averageTime)
            {
                Callbacks++;
            };

            // Make an anonymous callback function to do nothing for GetPublicationsMessage
            int MessageCallbacks = 0;
            Harvester.GetPublicationsMessage MessageCallback = delegate(string Message, bool StatusBarOnly)
            {
                // Only increment MessageCallbacks if the message contains Joe's Setnb
                // and the word "same"
                if ((Message.Contains("A1234567") || (Message.Contains("Q2222222"))
                    && Message.Contains("same")))
                    MessageCallbacks++;
            };

            // Make an anonymous callback function to return false for CheckForInterrupt
            Harvester.CheckForInterrupt InterruptCallback = delegate()
            {
                return false;
            };

            // More stuff for the harvester
            Harvester harvester = new Harvester(DB);
            MockNCBI mockNCBI = new MockNCBI("medline");
            double AverageMilliseconds;


            // Harvest the people
            harvester.GetPublications(mockNCBI, ptc, Jane, StatusCallback, MessageCallback, InterruptCallback, out AverageMilliseconds);

            // Make sure the harvester got Jane's publications
            DataTable Results = DB.ExecuteQuery("SELECT PMID FROM PeoplePublications WHERE Setnb = 'Z7654321'");
            Assert.AreEqual(Results.Rows.Count, 3);
            foreach (DataRow Row in Results.Rows)
            {
                Assert.IsTrue(
                    (Row["PMID"].ToString() == "2417121")
                || (Row["PMID"].ToString() == "12679283")
                || (Row["PMID"].ToString() == "14653276"));
            }
            ArrayList Parameters = new ArrayList();
            Parameters.Add(Database.Parameter(Jane.Setnb));
            Results = DB.ExecuteQuery("SELECT Harvested, Error, ErrorMessage FROM People WHERE Setnb = ?", Parameters);
            Assert.AreEqual(Results.Rows[0]["Harvested"], true);
            Assert.AreEqual(Results.Rows[0]["Error"], DBNull.Value);
            Assert.AreEqual(Results.Rows[0]["ErrorMessage"].ToString(), "");


            // It should also get Joe's publications. It should call MessageCallback()
            // twice to let us know Joe'and Jim's s publications were found, and it 
            // should add the appropriate rows to PeoplePublications. 
            Assert.AreEqual(MessageCallbacks, 2);
            Results = DB.ExecuteQuery("SELECT PMID FROM PeoplePublications WHERE Setnb = 'A1234567'");
            Assert.AreEqual(Results.Rows.Count, 3);
            foreach (DataRow Row in Results.Rows)
            {
                Assert.IsTrue(
                    (Row["PMID"].ToString() == "2417121")
                || (Row["PMID"].ToString() == "12679283")
                || (Row["PMID"].ToString() == "14653276"));
            }
            Parameters = new ArrayList();
            Parameters.Add(Database.Parameter(Joe.Setnb));
            Results = DB.ExecuteQuery("SELECT " + Database.PEOPLE_COLUMNS + " FROM People WHERE Setnb = ?", Parameters);
            bool boolValue; // needed for GetBoolValue workaround for bit field bug in MySQL
            Assert.IsTrue(Database.GetBoolValue(Results.Rows[0]["Harvested"], out boolValue));
            Assert.IsTrue(boolValue);
            Assert.IsTrue(Database.GetBoolValue(Results.Rows[0]["Error"], out boolValue));
            Assert.IsFalse(boolValue);
            Assert.AreEqual(Results.Rows[0]["Error"], DBNull.Value);
            Assert.AreEqual(Results.Rows[0]["ErrorMessage"].ToString(), "");



            // It should also get Jim's publications -- and it should also clear his error.
            Assert.AreEqual(MessageCallbacks, 2);
            Results = DB.ExecuteQuery("SELECT PMID FROM PeoplePublications WHERE Setnb = 'A1234567'");
            Assert.AreEqual(Results.Rows.Count, 3);
            foreach (DataRow Row in Results.Rows)
            {
                Assert.IsTrue(
                    (Row["PMID"].ToString() == "2417121")
                || (Row["PMID"].ToString() == "12679283")
                || (Row["PMID"].ToString() == "14653276"));
            }
            Parameters = new ArrayList();
            Parameters.Add(Database.Parameter(Jim.Setnb));
            Results = DB.ExecuteQuery("SELECT " + Database.PEOPLE_COLUMNS + "FROM People WHERE Setnb = ?", Parameters);

            Assert.IsTrue(Database.GetBoolValue(Results.Rows[0]["Harvested"], out boolValue));
            Assert.AreEqual(boolValue, true);
            Assert.AreEqual(Results.Rows[0]["Error"], DBNull.Value);
            Assert.AreEqual(Results.Rows[0]["ErrorMessage"].ToString(), "");

        }


        /// <summary>
        /// Verify that the Languages parameter works
        /// </summary>
        [Test]
        public void TestLanguages()
        {
            // GetPublicationsFromInput1XLS_Using_MockNCBI reads four people from
            // Input1.xls and writes their publications to the datbase using 
            // MockNCBI. MockNCBI gets its test data from TestHarvester\*.dat.

            // There are 22 English publications that belong to the four people
            string[] Languages = { "eng" };
            int ExpectedPublications = 22;
            TestHarvester.GetPublicationsFromInput1XLS_Using_MockNCBI(false, Languages, ExpectedPublications);

            // There's one Russian publication
            Languages = new string[] { "rus" };
            ExpectedPublications = 1;
            TestHarvester.GetPublicationsFromInput1XLS_Using_MockNCBI(false, Languages, ExpectedPublications);

            // There's one Spanish publication
            Languages = new string[] { "spa" };
            ExpectedPublications = 1;
            TestHarvester.GetPublicationsFromInput1XLS_Using_MockNCBI(false, Languages, ExpectedPublications);

            // There's one Russian and one Spanish publication -- two total
            Languages = new string[] { "spa", "rus" };
            ExpectedPublications = 2;
            TestHarvester.GetPublicationsFromInput1XLS_Using_MockNCBI(false, Languages, ExpectedPublications);

            // There are no French or Portugese publications
            Languages = new string[] { "fre", "por" };
            ExpectedPublications = 0;
            TestHarvester.GetPublicationsFromInput1XLS_Using_MockNCBI(false, Languages, ExpectedPublications);

            // There are 22 English and one Russian publication
            Languages = new string[] { "eng", "rus" };
            ExpectedPublications = 23;
            TestHarvester.GetPublicationsFromInput1XLS_Using_MockNCBI(false, Languages, ExpectedPublications);


            Languages = null;
            ExpectedPublications = 24;
            TestHarvester.GetPublicationsFromInput1XLS_Using_MockNCBI(false, Languages, ExpectedPublications);

        }

    }


}

