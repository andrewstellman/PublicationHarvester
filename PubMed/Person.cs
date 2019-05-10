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
using System.Collections;

namespace Com.StellmanGreene.PubMed
{
    /// <summary>
    /// Store information about a PersonToWrite and write it to the database
    /// </summary>
    public class Person
    {
        /// <summary>
        /// Table that contains the people to read (usually "People")
        /// </summary>
        public string PeopleTable = "People";

        /// <summary>
        /// Table contains the publications for the people
        /// </summary>
        public string PeoplePublicationsTable = "PeoplePublications";

        #region Properties      
        public string Setnb;
        public string First;
        public string Middle;
        public string Last;
        public bool Harvested;
        public string[] Names;
        public string MedlineSearch;
        #endregion  

        /// <summary>
        /// Create a new PersonToWrite object
        /// </summary>
        public Person(string Setnb, string First, string Middle, string Last,
            bool Harvested, string[] Names, string MedlineSearch)
        {
            this.Setnb = Setnb;
            this.First = First;
            this.Middle = Middle;
            this.Last = Last;
            this.Harvested = Harvested;
            this.Names = Names;
            this.MedlineSearch = MedlineSearch;
        }

        /// <summary>
        /// Read a PersonToWrite out of an Excel file or database
        /// </summary>
        /// <param name="PeopleFileData">DataTable that contains the Excel or SQL results</param>
        /// <param name="RowNumber">Number of the row that contains the PersonToWrite to read</param>
        public Person(DataRow PeopleFileData, DataColumnCollection Columns)
        {
            // The only difference between the Excel row and the database row is the name
            // of the medline_search1 (or MedlineSearch) column, and the existence of
            // the Harvested column
            Setnb = PeopleFileData["setnb"].ToString();
            if (Setnb.Length == 0)
                throw new Exception("People file contains a blank setnb");
            First = PeopleFileData["first"].ToString();
            Middle = PeopleFileData["middle"].ToString();
            Last = PeopleFileData["last"].ToString();
            Names = new string[1];
            Names[0] = PeopleFileData["name1"].ToString();
            for (int Num = 2; Num <= 6; Num++)
            {
                string Name = PeopleFileData["Name" + Num.ToString()].ToString();
                if (Name.Length != 0)
                {
                    Array.Resize(ref Names, Names.Length + 1);
                    Names[Names.GetUpperBound(0)] = Name;
                }
            }
            
            // Check for the Harvested column
            bool boolValue;
            if (Columns.Contains("Harvested"))
                if (Database.GetBoolValue(PeopleFileData["Harvested"], out boolValue))
                    Harvested = boolValue;
            

            // Check for the MedlineSearch column
            if (Columns.Contains("MedlineSearch"))
                MedlineSearch = PeopleFileData["MedlineSearch"].ToString();
            else
                MedlineSearch = PeopleFileData["medline_search1"].ToString();
        }

       

        /// <summary>
        /// Write the PersonToWrite to the database. If an error occurs, write the error to
        /// the database and then throw it.
        /// </summary>
        /// <param name="DB">Database to write to</param>
        public void WriteToDB(Database DB) 
        {
            ArrayList Parameters = new ArrayList();
            try
            {
                Parameters.Clear();

                // Check if the PersonToWrite is already in the database
                Parameters.Add(Database.Parameter(Setnb));
                int Count = DB.GetIntValue("SELECT Count(*) FROM " + PeopleTable + " WHERE Setnb = ?", Parameters);

                if (Count > 0)
                {
                    // If the PersonToWrite already exists in the database, update him
                    Parameters.Clear();
                    Parameters.Add(Database.Parameter(Database.Left(First, 20)));
                    Parameters.Add(Database.Parameter(Database.Left(Middle, 20)));
                    Parameters.Add(Database.Parameter(Database.Left(Last, 20)));
                    Parameters.Add(Database.Parameter(Database.Left(Names[0], 36)));
                    Parameters.Add(Database.Parameter(Names.Length >= 2 ? Database.Left(Names[1], 36) : null));
                    Parameters.Add(Database.Parameter(Names.Length >= 3 ? Database.Left(Names[2], 36) : null));
                    Parameters.Add(Database.Parameter(Names.Length >= 4 ? Database.Left(Names[3], 36) : null));
                    Parameters.Add(Database.Parameter(Names.Length >= 5 ? Database.Left(Names[4], 36) : null));
                    Parameters.Add(Database.Parameter(Names.Length >= 6 ? Database.Left(Names[5], 36) : null));
                    Parameters.Add(Database.Parameter(Harvested));
                    Parameters.Add(Database.Parameter(Database.Left(MedlineSearch, 10000)));
                    Parameters.Add(Database.Parameter(Database.Left(Setnb, 8)));
                    DB.ExecuteNonQuery(
                        @"UPDATE " + PeopleTable + @" 
                             SET First = ?, Middle = ?, Last = ?, 
                                 Name1 = ?, Name2 = ?, Name3 = ?, 
                                 Name4 = ?, Name5 = ?, Name6 = ?, 
                                 Harvested = ?, MedlineSearch = ?
                           WHERE Setnb = ?", Parameters);
                }
                else
                {
                    // The PersonToWrite doesn't exist yet -- add him
                    Parameters.Clear();
                    Parameters.Add(Database.Parameter(Database.Left(Setnb, 8)));
                    Parameters.Add(Database.Parameter(Database.Left(First, 20)));
                    Parameters.Add(Database.Parameter(Database.Left(Middle, 20)));
                    Parameters.Add(Database.Parameter(Database.Left(Last, 20)));
                    Parameters.Add(Database.Parameter(Database.Left(Names[0], 36)));
                    Parameters.Add(Database.Parameter(Names.Length >= 2 ? Database.Left(Names[1], 36) : null));
                    Parameters.Add(Database.Parameter(Names.Length >= 3 ? Database.Left(Names[2], 36) : null));
                    Parameters.Add(Database.Parameter(Names.Length >= 4 ? Database.Left(Names[3], 36) : null));
                    Parameters.Add(Database.Parameter(Names.Length >= 5 ? Database.Left(Names[4], 36) : null));
                    Parameters.Add(Database.Parameter(Names.Length >= 6 ? Database.Left(Names[5], 36) : null));
                    Parameters.Add(Database.Parameter(Database.Left(MedlineSearch, 10000)));
                    DB.ExecuteNonQuery(
                        @"INSERT INTO " + PeopleTable + @" 
                                 (Setnb, First, Middle, Last, Name1, Name2, Name3, Name4, Name5, Name6, MedlineSearch)
                          VALUES ( ? , ? , ? , ? , ? , ? , ? , ? , ? , ? , ?)", Parameters);
                }

            }
            catch (Exception ex)
            {
                try
                {
                    // Attempt to write the error to the database
                    Parameters.Clear();
                    Parameters.Add(Database.Parameter(1));
                    Parameters.Add(Database.Parameter("Unable to update People row: " + ex.Message));
                    Parameters.Add(Database.Parameter(Setnb));
                    DB.ExecuteQuery(
                        @"UPDATE " + PeopleTable + @"
                             SET Error = ?, ErrorMessage = ?
                           WHERE Setnb = ?", Parameters);
                }
                catch
                {
                    // Do nothing if the attempt to write Error and ErrorMessage fails
                }
                throw new Exception("Error writing person " + Setnb + ": " + ex.Message, ex);
            }
        }


        /// <summary>
        /// Write an error message for this person in the database
        /// </summary>
        /// <param name="DB">Database to write to</param>
        /// <param name="ErrorMessage">Error message to add</param>
        public void WriteErrorToDB(Database DB, string ErrorMessage) {
            try
            {
                ArrayList Parameters = new ArrayList();
                Parameters.Add(Database.Parameter(true));
                if (ErrorMessage.Length > 512)
                    ErrorMessage = ErrorMessage.Substring(0, 511);
                Parameters.Add(Database.Parameter(ErrorMessage));
                Parameters.Add(Database.Parameter(this.Setnb));
                DB.ExecuteNonQuery(
                    @"UPDATE " + PeopleTable + @" 
                     SET Error = ? , ErrorMessage = ?
                   WHERE Setnb = ?", Parameters);
            }
            catch
            {
                // if there's an error, do nothing
            }
        }

        public void ClearErrorInDB(Database DB)
        {
            try
            {
                ArrayList Parameters = new ArrayList();
                Parameters.Add(Database.Parameter(false));
                Parameters.Add(Database.Parameter(""));
                Parameters.Add(Database.Parameter(this.Setnb));
                DB.ExecuteNonQuery(
                    @"UPDATE " + PeopleTable + @" 
                     SET Error = ? , ErrorMessage = ?
                   WHERE Setnb = ?", Parameters);
            }
            catch
            {
                // if there's an error, do nothing
            }
        }


        /// <summary>
        /// Get the author position from the PeoplePublications table
        /// </summary>
        /// <param name="publication">The publication to search for</param>
        /// <param name="PositionType">The value in PeoplePublications.PositionType</param>
        /// <returns>The author position (or 0 if not found)</returns>
        public int GetAuthorPosition(Database DB, Publication publication, out Harvester.AuthorPositions PositionType)
        {
            ArrayList Parameters = new ArrayList();
            Parameters.Add(Database.Parameter(publication.PMID));
            Parameters.Add(Database.Parameter(Setnb));
            DataTable Results = DB.ExecuteQuery(
                @"SELECT AuthorPosition, PositionType 
                    FROM " + PeoplePublicationsTable + @"
                   WHERE PMID = ? 
                     AND Setnb = ?", Parameters);
            if (Results.Rows.Count == 0)
            {
                PositionType = 0;
                return 0;
            }
            else
            {
                PositionType = (Harvester.AuthorPositions) Convert.ToInt32(Results.Rows[0]["PositionType"]);
                return Convert.ToInt32(Results.Rows[0]["AuthorPosition"]);
            }
        }

        /// <summary>
        /// ToString prints the name and setnb
        /// </summary>
        /// <returns>Last, First (Setnb)</returns>
        public override string ToString()
        {
            return String.Format("{0}, {1} ({2})", Last, First, Setnb);
        }
    }

}
