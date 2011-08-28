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
    /// Test copying publications from another database
    /// </summary>
    [TestFixture]
    public class TestCopyPublications
    {
        [SetUp]
        public void TestCopyPublicationsSetUp()
        {
            /* 
             * Drop and recreate the CGUnitTestCopyPublications database
             * 
             * Note that CGUnitTestCopyPublications.sql contains the structure
             * and data of a database populated with the Publication Harvester.
             * 
             * This database will contain publications for three people: Bintliff,
             * Bunn and Fakerson. Fakerson should never get copied -- he should
             * always be ignored.
             */

            Database DB = new Database("Colleague Generator Unit Test");
            StreamReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory
                + "\\Unit Tests\\TestCopyPublications\\CGUnitTestCopyPublications.sql");
            string Contents = reader.ReadToEnd();
            foreach (string SQL in Contents.Split(';'))
            {
                DB.ExecuteNonQuery(SQL);
            }



            // Create the test data using the setup in TestStarColleaguesReport
            // This will create two harvested colleagues: Bintliff and Bunn
            Harvester harvester;
            PublicationTypes PubTypes;
            NCBI ncbi;
            string[] Languages = { "eng" };
            TestStarColleaguesReport.DoSetUp(out DB, out harvester, out PubTypes, out ncbi, Languages);

            // Bintliff doesn't have any publications -- she's a false colleague.
            // So we can "unharvest" her without removing any data from the database.
            DB.ExecuteNonQuery("UPDATE Colleagues SET Harvested = 0 WHERE LAST = 'BINTLIFF'");

            // Here's the data we now have:
            //
            //   1. In the CGUnitTestCopyPublications database, there are three 
            //   publications for Bintliff, two for Bunn and one for Fakerson.

            // verify that there are three distinct people in CGUnitTestCopyPublications 
            Assert.AreEqual(
                DB.GetIntValue(@"SELECT Count(DISTINCT p.Last) 
                                   FROM CGUnitTestCopyPublications.PeoplePublications pp,
                                        CGUnitTestCopyPublications.People p
                                  WHERE pp.Setnb = p.Setnb"),

                                3);

            // verify that Bintliff has three publications
            Assert.AreEqual(
                DB.GetIntValue(@"SELECT Count(*) 
                                   FROM CGUnitTestCopyPublications.PeoplePublications pp,
                                        CGUnitTestCopyPublications.People p
                                  WHERE p.Last = 'BINTLIFF' 
                                    AND pp.Setnb = p.Setnb"),
                                3);

            // verify that Bunn has two publications
            Assert.AreEqual(
                DB.GetIntValue(@"SELECT Count(*) 
                                   FROM CGUnitTestCopyPublications.PeoplePublications pp,
                                        CGUnitTestCopyPublications.People p
                                  WHERE p.Last = 'BUNN' 
                                    AND pp.Setnb = p.Setnb"),
                                2);

            // verify that Fakerson has one publication
            Assert.AreEqual(
                DB.GetIntValue(@"SELECT Count(*) 
                                   FROM CGUnitTestCopyPublications.PeoplePublications pp,
                                        CGUnitTestCopyPublications.People p
                                  WHERE p.Last = 'FAKERSON' 
                                    AND pp.Setnb = p.Setnb"),
                                1);




            //   2. The Colleagues table contains two colleagues, Bintliff (who has no
            //   publications and is unharvested) and Bunn (who has publications and
            //   is harvested).
            Assert.AreEqual(DB.GetIntValue("SELECT Count(*) FROM Colleagues"), 2);
            Assert.AreEqual(
                DB.GetIntValue(@"SELECT Count(*) FROM Colleagues
                                  WHERE LAST = 'BUNN' AND Harvested = 1")
                , 1);
            Assert.AreEqual(
                DB.GetIntValue(@"SELECT Count(*) FROM Colleagues
                                  WHERE LAST = 'BINTLIFF' AND Harvested = 0")
                , 1);
        }
        

        /// <summary>
        /// Call CopyPublications.DoCopy() and verify the results
        /// </summary>
        [Test]
        public void VerifyCopyPublications()
        {
            // Run DoCopy() to copy the publications. 
            //
            // This should copy Bintliff's three publications, ignore Bunn's
            // two publications (because he's harvested), and ignore Fakerson (because
            // he's not a colleague.)
            Database DB = new Database("Colleague Generator Unit Test");
            CopyPublications.DoCopy(DB, "CGUnitTestCopyPublications", "1,2,3,4");
            

            // Verify that Bintliff's publications were copied (including their
            // MeSH headings, authors, and grants; also, PeoplePublications must
            // be up to date as well.). 

            // Verify that ColleaguePublications contains exactly the same data
            // as PeoplePublications for Bintliff's setnb. The way this works is
            // by first counting all of Bintliff's rows in both databases that
            // match exactly, and then verifying that number is the same as the
            // number that correspond whether or not they match
            Assert.AreEqual(
                DB.GetIntValue(
                    @"SELECT Count(*)
                        FROM ColleaguePublications cp, 
                             CGUnitTestCopyPublications.PeoplePublications pp
                       WHERE cp.Setnb = 'A2700156'
                         AND cp.Setnb = pp.Setnb
                         AND cp.PMID = pp.PMID
                         AND cp.AuthorPosition = pp.AuthorPosition
                         AND cp.PositionType = pp.PositionType"),
                DB.GetIntValue(@"SELECT Count(*) 
                        FROM CGUnitTestCopyPublications.PeoplePublications pp
                       WHERE pp.Setnb = 'A2700156'"));

            // Verify that PublicationAuthors contains the same data for 
            // all of Bintliff's publications in both databases
            Assert.AreEqual(
                DB.GetIntValue(
                    @"SELECT Count(*)
                        FROM ColleaguePublications cp, 
                             CGUnitTestCopyPublications.PublicationAuthors cgpa
                       WHERE cp.Setnb = 'A2700156'
                         AND cp.PMID = cgpa.PMID"),
                DB.GetIntValue(
                    @"SELECT Count(*)
                        FROM ColleaguePublications cp, PublicationAuthors pa
                       WHERE cp.Setnb = 'A2700156'
                         AND cp.PMID = pa.PMID"));


            // Verify that no publications were copied for Bunn
            Assert.AreEqual(DB.GetIntValue(
                @"SELECT Count(*)
                    FROM ColleaguePublications cp,
                         CGUnitTestCopyPublications.PeoplePublications pp
                   WHERE pp.Setnb = 'A4800524'
                     AND pp.PMID = cp.PMID"), 0);


            // Verify that no publications were copied for Fakerson
            Assert.AreEqual(DB.GetIntValue(
                @"SELECT Count(*)
                    FROM ColleaguePublications cp,
                         CGUnitTestCopyPublications.PeoplePublications pp
                   WHERE pp.Setnb = 'A1234567'
                     AND pp.PMID = cp.PMID"), 0);
            
            

            // Now let's 'unharvest' Bunn by removing his PeoplePublication rows -- but 
            // only one of his publications will actually be removed from the Publications, 
            // PublicationGrants, PublicationMeSHHeadings and PublicationAuthors
            // tables. This will verify that duplicate publications won't cause problems.
            DB.ExecuteNonQuery("UPDATE Colleagues SET Harvested = 0 WHERE LAST = 'BUNN'");
            DB.ExecuteNonQuery("DELETE FROM ColleaguePublications WHERE Setnb = 'A4800524' AND PMID = 8931843");

            // Verify that Bunn has five publications
            Assert.AreEqual(DB.GetIntValue(
                @"SELECT Count(*)
                    FROM ColleaguePublications c
                   WHERE Setnb = 'A4800524'"), 5);

            // Make sure it doesn't copy publications with nonmatching publication types
            CopyPublications.DoCopy(DB, "CGUnitTestCopyPublications", "5,6,7,8");

            // Verify that Bunn STILL has five publications
            Assert.AreEqual(DB.GetIntValue(
                @"SELECT Count(*)
                    FROM ColleaguePublications c
                   WHERE Setnb = 'A4800524'"), 5);

            // Copy the publications with the right publication types
            CopyPublications.DoCopy(DB, "CGUnitTestCopyPublications", "1,2,3,4");

            // Verify that Bunn now has seven publications (two were copied 
            // from CGUnitTestCopyPublications)
            Assert.AreEqual(DB.GetIntValue(
                @"SELECT Count(*)
                    FROM ColleaguePublications c
                   WHERE Setnb = 'A4800524'"), 7);
        }

   
    }
}
