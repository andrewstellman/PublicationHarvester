using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using NUnit.Framework;

namespace SCGen
{
    class MockNCBI : Com.StellmanGreene.PubMed.NCBI
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
                default: // all other queries should go to OtherPeople.dat
                    Filename = "OtherPeople.dat";
                    break;
            }
            StreamReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory 
                + "\\Unit Tests\\MockNCBI\\" + Filename);
            String results = reader.ReadToEnd();
            reader.Close();
            return results;
        }
    }
}
