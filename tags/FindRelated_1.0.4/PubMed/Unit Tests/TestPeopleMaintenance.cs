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
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.Odbc;
using NUnit.Framework;

namespace Com.StellmanGreene.PubMed.Unit_Tests
{
    /// <summary>
    /// Test the PeopleMaintenance class
    /// </summary>
    [TestFixture]
    public class TestPeopleMaintenance
    {
        private Database DB;

        private void ResetDatabase()
        {
            // Import "TestPeopleMaintenance/input1 plus testhyphens.xls" into the People table
            People PeopleFromFile = new People(
                AppDomain.CurrentDomain.BaseDirectory + "\\Unit Tests\\TestPeopleMaintenance",
                "input1 plus testhypens.xls");
            DB = new Database("Publication Harvester Unit Test");
            Harvester harvester = new Harvester(DB);
            harvester.CreateTables();
            MockNCBI mockNCBI = new MockNCBI("medline");
            mockNCBI.SearchThrowsAnError = false;
            PublicationTypes ptc = new PublicationTypes(
                AppDomain.CurrentDomain.BaseDirectory + "\\Unit Tests\\TestPublicationTypes",
                "PublicationTypes.csv"
            );
            ptc.WriteToDB(DB);


            // Anonymous callback functions for GetPublications
            Harvester.GetPublicationsStatus StatusCallback = delegate(int number, int total, int averageTime) 
            {
                //
            };
            Harvester.GetPublicationsMessage MessageCallback = delegate(string Message, bool StatusBarOnly)
            {
                //
            };
            Harvester.CheckForInterrupt InterruptCallback = delegate() 
            { 
                return false; 
            };
            
            // Write the people, then "harvest" the publications using MockNCBI
            double AverageMilliseconds;
            foreach (Person person in PeopleFromFile.PersonList)
            {
                person.WriteToDB(DB);
                harvester.GetPublications(mockNCBI, ptc, person, StatusCallback, MessageCallback, InterruptCallback, out AverageMilliseconds);
            }

            People PeopleFromDB = new People(DB);
            Assert.AreEqual(PeopleFromDB.PersonList.Count, 4);
        }
            
        

        /// <summary>
        /// Add/update rows from a people file
        /// </summary>
        [Test]
        public void TestAddUpdate()
        {
            ResetDatabase();

            // Add/update the rows in "TestPeople/input1.xls"
            int Count = PeopleMaintenance.AddUpdate(DB, 
                AppDomain.CurrentDomain.BaseDirectory 
                + "\\Unit Tests\\TestPeople\\input1.xls"
            );
            Assert.AreEqual(Count, 4);
            VerifyAddUpdateResults();

            // Delete the rows in "different setnb.xls", verify that nothing happened
            PeopleMaintenance.Remove(DB, 
                AppDomain.CurrentDomain.BaseDirectory
                + "\\Unit Tests\\TestPeopleMaintenance\\different setnb.xls"
            );
            VerifyAddUpdateResults();
        }

        // Verify the results for TestAddUpdate()
        private void VerifyAddUpdateResults()
        {
            People PeopleFromDB = new People(DB);
            Assert.AreEqual(PeopleFromDB.PersonList.Count, 5);
            int Count = 0; // verify that we found everything we're looking for
            foreach (Person person in PeopleFromDB.PersonList)
            {
                switch (person.Setnb)
                {
                    // Verify that Guillemin (A5702471) was added
                    // Verify that Tobian (A5401532) and Van Eys (A6009400) were updated
                    case "A5702471":
                    case "A5401532":
                    case "A6009400":
                        TestPeople.TestInput1People(person); // leverage this test

                        // Verify that there are no publications for these people
                        ArrayList Parameters = new ArrayList();
                        Parameters.Add(Database.Parameter(person.Setnb));
                        int NumPubs = DB.GetIntValue("SELECT Count(*) FROM PeoplePublications WHERE Setnb = ?", Parameters);
                        Assert.AreEqual(NumPubs, 0);
                        Count++;
                        break;
                }
            }
            Assert.AreEqual(Count, 3);
        }


        [Test]
        public void TestDelete()
        {
            ResetDatabase();

            // Delete the rows in "TestPeople/test hyphens.xls"
            int Count = PeopleMaintenance.Remove(DB, 
                AppDomain.CurrentDomain.BaseDirectory
                + "\\Unit Tests\\TestPeople\\test hyphens.xls"
            );
            Assert.AreEqual(Count, 1);

            // Verify that Wassertheil-Smoller (A7809652) was deleted
            People PeopleFromDB = new People(DB);
            Assert.AreEqual(PeopleFromDB.PersonList.Count, 3);
            foreach (Person person in PeopleFromDB.PersonList)
            {
                string Setnb = person.Setnb;
                Assert.IsTrue(
                    Setnb == "A6009400" ||
                    Setnb == "A5401532" ||
                    Setnb == "A5501586");
            }

            // Delete the rows in "different setnb.xls", verify that nothing happened
            PeopleMaintenance.Remove(DB, 
                AppDomain.CurrentDomain.BaseDirectory
                + "\\Unit Tests\\TestPeopleMaintenance\\different setnb.xls"
            );
            PeopleFromDB = new People(DB);
            Assert.AreEqual(PeopleFromDB.PersonList.Count, 3);
            foreach (Person person in PeopleFromDB.PersonList)
            {
                string Setnb = person.Setnb;
                Assert.IsTrue(
                    Setnb == "A6009400" ||
                    Setnb == "A5401532" ||
                    Setnb == "A5501586");
            }
        }

    }
}
