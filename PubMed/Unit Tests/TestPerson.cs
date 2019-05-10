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
using System.Data;
using System.Data.Odbc;
using NUnit.Framework;

namespace Com.StellmanGreene.PubMed.Unit_Tests
{
    /// <summary>
    /// Test the Person class
    /// Note: A lot of Person functionality is verified in TestPeople
    /// GetAuthorPosition is verified in TestHarvester
    /// </summary>
    [TestFixture]
    public class TestPerson
    {
        /// <summary>
        /// Write a PersonToWrite, then write him again to make sure that the existing
        /// row is updated
        /// </summary>
        [Test]
        public void WriteAPersonTwice()
        {
            string[] Names = {"a", "b", "c"};
            Person PersonToWrite = new Person("1234ABCD", "First", "Middle", "Last", 
                false, Names, "Medline search query");
            Assert.IsTrue(PersonToWrite.Setnb == "1234ABCD");
            Assert.IsTrue(PersonToWrite.First == "First");
            Assert.IsTrue(PersonToWrite.Middle == "Middle");
            Assert.IsTrue(PersonToWrite.Last == "Last");
            Assert.IsTrue(PersonToWrite.Harvested == false);
            Assert.IsTrue(PersonToWrite.Names.Length == 3);
            Assert.IsTrue(PersonToWrite.Names[0] == "a");
            Assert.IsTrue(PersonToWrite.Names[1] == "b");
            Assert.IsTrue(PersonToWrite.Names[2] == "c");
            Assert.IsTrue(PersonToWrite.MedlineSearch == "Medline search query");

            // Write the person to the database
            Database DB = new Database("Publication Harvester Unit Test");
            Harvester harvester = new Harvester(DB);
            harvester.CreateTables();
            PersonToWrite.WriteToDB(DB);

            // Read the person back from the database
            DataTable PeopleFileData = new DataTable();
            PeopleFileData = DB.ExecuteQuery("SELECT " + Database.PEOPLE_COLUMNS + " FROM People WHERE Setnb = '1234ABCD'");
            Assert.IsTrue(PeopleFileData.Rows.Count == 1);
            Person PersonToRead = new Person(PeopleFileData.Rows[0], PeopleFileData.Columns);
            Assert.IsTrue(PersonToRead.Setnb == "1234ABCD");
            Assert.IsTrue(PersonToRead.First == "First");
            Assert.IsTrue(PersonToRead.Middle == "Middle");
            Assert.IsTrue(PersonToRead.Last == "Last");
            Assert.IsTrue(PersonToRead.Harvested == false);
            Assert.IsTrue(PersonToRead.Names.Length == 3);
            Assert.IsTrue(PersonToRead.Names[0] == "a");
            Assert.IsTrue(PersonToRead.Names[1] == "b");
            Assert.IsTrue(PersonToRead.Names[2] == "c");
            Assert.IsTrue(PersonToRead.MedlineSearch == "Medline search query");

            // Update the database, read him back again, and make sure there's still
            // one row in People
            PersonToRead.First = "NewFirst";
            PersonToRead.Middle = "NewMiddle";
            PersonToRead.Last = "NewLast";
            PersonToRead.Names = new string[] {"d", "e", "f", "g"};
            PersonToRead.MedlineSearch = "new query";
            PersonToRead.Harvested = true;
            PersonToRead.WriteToDB(DB);
            DataTable Results = DB.ExecuteQuery("SELECT " + Database.PEOPLE_COLUMNS + " FROM People WHERE Setnb = '1234ABCD'");
            Assert.IsTrue(Results.Rows.Count == 1);
            Assert.IsTrue(Results.Rows[0]["Setnb"].ToString() == "1234ABCD");
            Assert.IsTrue(Results.Rows[0]["First"].ToString() == "NewFirst");
            Assert.IsTrue(Results.Rows[0]["Middle"].ToString() == "NewMiddle");
            Assert.IsTrue(Results.Rows[0]["Last"].ToString() == "NewLast");
            Assert.IsTrue(Results.Rows[0]["Name1"].ToString() == "d");
            Assert.IsTrue(Results.Rows[0]["Name2"].ToString() == "e");
            Assert.IsTrue(Results.Rows[0]["Name3"].ToString() == "f");
            Assert.IsTrue(Results.Rows[0]["Name4"].ToString() == "g");
            Assert.IsTrue(Results.Rows[0]["MedlineSearch"].ToString() == "new query");

            bool boolValue;
            Assert.IsTrue(Database.GetBoolValue(Results.Rows[0]["Harvested"], out boolValue));
            Assert.IsTrue(boolValue);

            // Make sure that Name2 through Name4 are updated properly when nulls are inserted
            PersonToRead.Names = new string[] { "a name" };
            PersonToRead.WriteToDB(DB);
            Results = DB.ExecuteQuery("SELECT " + Database.PEOPLE_COLUMNS + " FROM People WHERE Setnb = '1234ABCD'");
            Assert.IsTrue(Results.Rows[0]["Name1"].ToString() == "a name");
            Assert.IsTrue(Results.Rows[0]["Name2"].Equals(DBNull.Value));
            Assert.IsTrue(Results.Rows[0]["Name3"].Equals(DBNull.Value));
            Assert.IsTrue(Results.Rows[0]["Name4"].Equals(DBNull.Value));
        }

        /// <summary>
        /// Write and clear an error
        /// </summary>
        [Test]
        public void WriteAndClearError()
        {
            string[] Names = { "a", "b", "c" };
            Person PersonToWrite = new Person("1234ABCD", "First", "Middle", "Last",
                false, Names, "Medline search query");

            // Write the person to the database
            Database DB = new Database("Publication Harvester Unit Test");
            Harvester harvester = new Harvester(DB);
            harvester.CreateTables();
            PersonToWrite.WriteToDB(DB);

            // Write an error
            PersonToWrite.WriteErrorToDB(DB, "This is the error message");
            DataTable Results = DB.ExecuteQuery("SELECT Error, ErrorMessage FROM People WHERE Setnb = '1234ABCD'");
            Assert.IsTrue(Results.Rows[0]["Error"].Equals(true));
            Assert.IsTrue(Results.Rows[0]["ErrorMessage"].ToString() == "This is the error message");

            // Clear the error -- note that we need to use GetBoolValue() to get around the MySQL bug
            PersonToWrite.ClearErrorInDB(DB);
            Results = DB.ExecuteQuery("SELECT " + Database.PEOPLE_COLUMNS + " FROM People WHERE Setnb = '1234ABCD'");
            bool boolValue;
            Assert.IsTrue(Database.GetBoolValue(Results.Rows[0]["Error"], out boolValue));
            Assert.IsTrue(boolValue == false);
            Assert.IsTrue(Results.Rows[0]["ErrorMessage"].ToString() == "");
        }

        /// <summary>
        /// Verify that long hyphenated names read properly from the name* 
        /// columns in the People file, and that they are written properly
        /// to the database.
        /// </summary>
        [Test]
        public void TestHyphens()
        {
            // Retrieve the publications for each person in input1.xls using GetPublications()
            People PeopleFromFile = new People(
                AppDomain.CurrentDomain.BaseDirectory + "\\Unit Tests\\TestPeople",
                "test hyphens.xls");
            Assert.IsTrue(PeopleFromFile.PersonList.Count == 1);
            Person PersonToWrite = PeopleFromFile.PersonList[0];
            Assert.IsTrue(PersonToWrite.Names.Length == 3);
            Assert.IsTrue(PersonToWrite.Names[0] == "wassertheil-smoller s");
            Assert.IsTrue(PersonToWrite.Names[1] == "wassertheil s");
            Assert.IsTrue(PersonToWrite.Names[2] == "wassertheil-smoller sm");

            // Write them to the database and read them back
            Database DB = new Database("Publication Harvester Unit Test");
            Harvester harvester = new Harvester(DB);
            harvester.CreateTables();
            PersonToWrite.WriteToDB(DB);
            People PeopleFromDB = new People(DB);
            Assert.IsTrue(PeopleFromDB.PersonList.Count == 1);
            Person PersonToRead = PeopleFromDB.PersonList[0];
            Assert.IsTrue(PersonToRead.Names.Length == 3);
            Assert.IsTrue(PersonToRead.Names[0] == "wassertheil-smoller s");
            Assert.IsTrue(PersonToRead.Names[1] == "wassertheil s");
            Assert.IsTrue(PersonToRead.Names[2] == "wassertheil-smoller sm");
        }
    }
}
