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
using System.IO;
using NUnit.Framework;

namespace Com.StellmanGreene.PubMed.Unit_Tests
{

    [TestFixture]
    public class TestNCBI
    {
        private NCBI ncbi;

        [OneTimeSetUp]
        public void TestNCBIWebQuerySetUp()
        {
            ncbi = new NCBI("medline");
        }

        /// <summary>
        /// Verify that typical results that would be returned by the web server
        /// </summary>
        [Test]
        public void NormalResults()
        {
            string xml = @"<?xml version=""1.0""?>
<!DOCTYPE eSearchResult PUBLIC ""-//NLM//DTD eSearchResult, 11 May 2002//EN"" ""http://www.ncbi.nlm.nih.gov/entrez/query/DTD/eSearch_020511.dtd"">
<eSearchResult>
	<Count>99</Count>
	<RetMax>1</RetMax>
	<RetStart>0</RetStart>
	<QueryKey>1</QueryKey>
	<WebEnv>01jHC0pmRm0V5DX0SCaTpJ0OqIA1N2LSKc2-Uus4KHDqRMj7m9Lz@@u66F4IOFk0AAH4@OH4AAAAQ</WebEnv>

	<IdList>
		<Id>15904469</Id>
	</IdList>
	<TranslationSet>
		<Translation>
			<From>STELLMAN SD</From>
			<To>STELLMAN SD[Author]</To>

		</Translation>
	</TranslationSet>
	<TranslationStack>
		<TermSet>
			<Term>STELLMAN SD[Author]</Term>
			<Field>Author</Field>
			<Count>99</Count>

			<Explode>Y</Explode>
		</TermSet>
		<OP>GROUP</OP>
	</TranslationStack>
</eSearchResult>";

            NCBI.EsearchResults results = NCBI.ParseSearchResults(xml);
            Assert.IsTrue(results.WebEnv == "01jHC0pmRm0V5DX0SCaTpJ0OqIA1N2LSKc2-Uus4KHDqRMj7m9Lz@@u66F4IOFk0AAH4@OH4AAAAQ");
            Assert.IsTrue(results.QueryKey == 1);
            Assert.IsTrue(results.Count == 99);
            Assert.IsTrue(results.Found == true);
        }

        /// <summary>
        /// When ParseSearchResults() gets malformed XML, verify that an exception is thrown
        /// It should throw a System.Exception: 
        ///    "Unable to process XML returned by the NCBI server. Offending XML has been written to pubharvester_error.log."
        /// and then write the offending XML to pubharvester_error.log. This test verifies that the
        /// correct error message is thrown, and that the data is actually written to pubharvester_error.log.
        /// </summary>
        [Test]
        public void MalformedXML()
        {
            string malformed = "this is some malformed xml!";
            try
            {
                NCBI.EsearchResults results = NCBI.ParseSearchResults(malformed);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                // Verify the error is thrown
                Assert.IsTrue(ex.Message == "Unable to process XML returned by the NCBI server. Offending XML has been written to pubharvester_error.log.");

                // Read the last 6 lines of pubharvester_error.log
                StreamReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\pubharvester_error.log");
                string[] lines = reader.ReadToEnd().Split('\n');
                Assert.IsTrue(lines[lines.Length - 5].Trim() == malformed.Trim());
            }
        }
    }
}
