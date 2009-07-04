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
using System.Text;
using System.IO;
using NUnit.Framework;

namespace Com.StellmanGreene.PubMed.Unit_Tests
{
    class MockNCBI : NCBI
    {
        /// <summary>
        /// If this is true, Search() throws an error
        /// </summary>
        public bool SearchThrowsAnError;

        public MockNCBI(string FetchMethod) : base(FetchMethod)
        {
            SearchThrowsAnError = false;
        }

        /// <summary>
        /// Instead of doing a search, read a file for the expected output
        /// </summary>
        /// <param name="Query">Query that indicates what's being searched for</param>
        /// <returns></returns>
        public override string Search(string Query) 
        {
            if (SearchThrowsAnError)
                throw new Exception("MockNCBI was told to throw an error");

            string Filename;
            switch (Query) 
            {
                case "(\"van eys j\"[au] OR \"vaneys j\"[au] OR \"eys jv\"[au])":
                    Filename = "Van Eys.dat";
                    break;
                case "(\"tobian l\"[au] OR \"tobian l jr\"[au] OR \"tobian lj\"[au])":
                    Filename = "Tobian.dat";
                    break;
                case "((\"reemtsma k\"[au] OR \"reemtsma kb\"[au]) AND 1956:2000[dp])":
                    Filename = "Reemtsma.dat";
                    break;
                case "(\"guillemin rc\"[au] OR (\"guillemin r\"[au] NOT (Electrodiagn Ther[ta] OR Phys Rev Lett[ta] OR vegas[ad] OR lindle[au])))":
                    Filename = "Guillemin.dat";
                    break;
                case "Special query for OtherPeople.dat":
                    Filename = "OtherPeople.dat";
                    break;
                default:
                    Filename = "";
                    Assert.Fail("Invalid query: " + Query);
                    break;
            }
            StreamReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory 
                + "\\Unit Tests\\TestHarvester\\" + Filename);
            String results = reader.ReadToEnd();
            reader.Close();
            return results;
        }
    }
}
