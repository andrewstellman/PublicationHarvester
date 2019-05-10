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
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Com.StellmanGreene.PubMed.Unit_Tests
{
    /// <summary>
    /// Test the People class
    /// </summary>
    [TestFixture]
    public class TestPeople
    {
        /// <summary>
        /// Read the test file input1.xls
        /// Write input1.xls to the database and read it back
        /// </summary>
        [Test]
        public void ReadExcelFileAndWritetoDB()
        {
            // Read the input file input1.xls
            People PeopleFromFile = new People(
                AppDomain.CurrentDomain.BaseDirectory + "\\Unit Tests\\TestPeople",
                "input1.xls");

            VerifyInput1File(PeopleFromFile);
        }

        /// <summary>
        /// Read the test file input1.csv
        /// Write input1.csv to the database and read it back
        /// </summary>
        [Test]
        public void ReadCSVFileAndWritetoDB()
        {
            // Read the input file input1.xls
            People PeopleFromFile = new People(
                AppDomain.CurrentDomain.BaseDirectory + "\\Unit Tests\\TestPeople",
                "input1.csv");

            VerifyInput1File(PeopleFromFile);
        }

        /// <summary>
        /// Verify that input1.* was read properly
        /// </summary>
        /// <param name="PeopleFromFile">People object that contains contents of input1.* file</param>
        private static void VerifyInput1File(People PeopleFromFile)
        {
            int Count = 0;

            // Verify that each PersonToWrite was read properly -- note that the order people
            // are returned is not guaranteed
            foreach (Person person in PeopleFromFile.PersonList)
            {
                Count++;
                TestInput1People(person);
            }
            // Verify that all four people were read from input1.xls
            Assert.AreEqual(Count, 4);

            // Write the people to the database -- first initialize it
            Database DB = new Database("Publication Harvester Unit Test");
            Harvester harvester = new Harvester(DB);
            harvester.CreateTables();
            foreach (Person person in PeopleFromFile.PersonList)
            {
                person.WriteToDB(DB);
            }

            // Read the people back from the database
            People PeopleFromDB = new People(DB);
            Count = 0;
            // Verify that each PersonToWrite was read properly -- note that the order people
            // are returned is not guaranteed
            foreach (Person person in PeopleFromDB.PersonList)
            {
                Count++;
                TestInput1People(person);
            }
            // Verify that all four people were read from input1.xls
            Assert.AreEqual(Count, 4);
        }

        /// <summary>
        /// Test each of the people from Input1.xls.
        /// </summary>
        /// <param name="PersonToWrite">A PersonToWrite from Input1.xls</param>
        public static void TestInput1People(Person person)
        {
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
                    Assert.IsTrue(person.Names.Length == 6);
                    Assert.IsTrue(person.Names[0] == "reemtsma k");
                    Assert.IsTrue(person.Names[1] == "reemtsma kb");
                    Assert.IsTrue(person.Names[2] == "test data");
                    Assert.IsTrue(person.Names[3] == "more test data");
                    Assert.IsTrue(person.Names[4] == "test data name 5");
                    Assert.IsTrue(person.Names[5] == "test data name 6");
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


        /// <summary>
        /// Verify that People throws an error when there is no setnb value
        /// </summary>
        [Test]
        public void NoSetnb()
        {
            try
            {
                // Read the input file "no setnb.xls" -- this will throw an error
                People PeopleFromFile = new People(
                    AppDomain.CurrentDomain.BaseDirectory + "\\Unit Tests\\TestPeople",
                    "no setnb.xls");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message == "People file contains a blank setnb");
                return;
            }
            Assert.Fail("People constructor failed to throw an error");
        }

    }
}
