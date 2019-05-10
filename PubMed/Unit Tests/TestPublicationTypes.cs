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
    [TestFixture]
    public class TestPublicationTypes
    {
        /// <summary>
        /// 1. Read the publication types from a CSV file
        /// 2. Write the publication types to the database
        /// 3. Read the publication types back from the database
        /// </summary>
        [Test]
        public void ReadAndWritePublicationTypes()
        {
            // Read the publication types from the CSV file
            PublicationTypes ptc = new PublicationTypes(
                AppDomain.CurrentDomain.BaseDirectory + "\\Unit Tests\\TestPublicationTypes",
                "PublicationTypes.csv"
                );
            Assert.AreEqual (ptc.Categories.Count , 52);
            Assert.AreEqual(ptc.GetCategoryNumber("Legislation") , 0);
            Assert.AreEqual(ptc.GetCategoryNumber("Consensus Development Conference, NIH") , 1);
            Assert.AreEqual(ptc.GetCategoryNumber("Review, Multicase") , 2);
            Assert.AreEqual(ptc.GetCategoryNumber("Technical Report") , 3);
            Assert.AreEqual(ptc.GetCategoryNumber("Comment") , 4);

            // Verify OverrideFirstCategory values
            Assert.IsTrue(ptc.OverrideFirstCategory.ContainsKey("Review"));
            Assert.IsTrue(ptc.OverrideFirstCategory.ContainsKey("Review, Multicase"));
            Assert.AreEqual(ptc.OverrideFirstCategory.ContainsKey("Comment") , false);


            // First recreate the database, then write the publication types to it
            Database DB = new Database("Publication Harvester Unit Test");
            Harvester harvester = new Harvester(DB);
            harvester.CreateTables();
            ptc.WriteToDB(DB);

            // Read the publication types from the database
            PublicationTypes ptcFromDB = 
                new PublicationTypes(DB);
            Assert.AreEqual(ptcFromDB.Categories.Count , 52);
            Assert.AreEqual(ptcFromDB.GetCategoryNumber("Overall"), 0);
            Assert.AreEqual(ptcFromDB.GetCategoryNumber("Clinical Trial, Phase II"), 1);
            Assert.AreEqual(ptcFromDB.GetCategoryNumber("Review of Reported Cases"), 2);
            Assert.AreEqual(ptcFromDB.GetCategoryNumber("Technical Report"), 3);
            Assert.AreEqual(ptcFromDB.GetCategoryNumber("Letter"), 4);
            Assert.AreEqual(ptcFromDB.GetCategoryNumber("Comment"), 4);

            // Verify OverrideFirstCategory values
            Assert.IsTrue(ptcFromDB.OverrideFirstCategory.ContainsKey("Review"));
            Assert.IsTrue(ptcFromDB.OverrideFirstCategory.ContainsKey("Review, Multicase"));
            Assert.AreEqual(ptcFromDB.OverrideFirstCategory.ContainsKey("Comment"), false);
        }


        /// <summary>
        /// Verify that a duplicate type in the file throws an error
        /// </summary>
        [Test]
        public void DuplicateType()
        {
            try
            {
                // Read the publication types from the CSV file
                PublicationTypes ptc = new PublicationTypes(
                    AppDomain.CurrentDomain.BaseDirectory + "\\Unit Tests\\TestPublicationTypes",
                    "Duplicate Type.csv"
                    );
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains("Historical Article"));
            }
        }


        /// <summary>
        /// Verify that an invalid category in the file throws an error
        /// </summary>
        [Test]
        public void InvalidCategory()
        {
            try {
            // Read the publication types from the CSV file
            PublicationTypes ptc = new PublicationTypes(
                AppDomain.CurrentDomain.BaseDirectory + "\\Unit Tests\\TestPublicationTypes",
                "Invalid Category.csv"
                );
            Assert.Fail();
        }
        catch (Exception ex)
        {
            Assert.IsTrue(ex.Message.Contains("Newspaper Article"));
        }
    }


        /// <summary>
        /// Verify that PublicationTypes will raise an error when passed an invalid folder
        /// </summary>
        [Test]
        public void InvalidFolder()
        {
            try
            {
                PublicationTypes ptc = new PublicationTypes(
                    "xyz1234",
                    "PublicationTypes.csv"
                    );
                Assert.Fail();
            }
            catch 
            {
                Assert.IsTrue(true);
            }
        }


        /// <summary>
        /// Verify that PublicationTypes will raise an error when passed an invalid filename
        /// </summary>
        [Test]
        public void InvalidFilename()
        {
            try
            {
                PublicationTypes ptc = new PublicationTypes(
                    AppDomain.CurrentDomain.BaseDirectory + "\\Unit Tests\\TestPublicationTypes",
                    "this is an invalid filename"
                    );
                Assert.Fail();
            }
            catch 
            {
                Assert.IsTrue(true);
            }
        }
    
    }
}
