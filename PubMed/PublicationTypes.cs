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
using System.Collections;
using Com.StellmanGreene.CSVReader;

namespace Com.StellmanGreene.PubMed
{
    /// <summary>
    /// This class reads the PublicationTypeCategories file
    /// </summary>
    public class PublicationTypes
    {
        /// <summary>
        /// Hash table that contains each of the categories (with case-insensitive comparer)
        /// </summary>
        internal Hashtable Categories;

        /// <summary>
        /// HashTable that contains each of the categories flagged to override the first category (with case-insensitive comparer)
        /// </summary>
        internal Hashtable OverrideFirstCategory;

        /// <summary>
        /// This constructor reads publication types from a database
        /// </summary>
        /// <param name="DB">The database to read publication types from</param>
        public PublicationTypes(Database DB)
        {
            DataTable Results = DB.ExecuteQuery(
                @"SELECT PublicationType, PubTypeCategoryID, OverrideFirstCategory
                    FROM PubTypeCategories"
            );

            // Read the categories -- note the case insensitive string comparer to ensure that the
            // matches are case insensitive
            Categories = new Hashtable(StringComparer.CurrentCultureIgnoreCase);
            OverrideFirstCategory = new Hashtable(StringComparer.CurrentCultureIgnoreCase);
            for (int row = 0; row < Results.Rows.Count; row++)
            {
                string Key = Results.Rows[row][0].ToString();
                Categories.Add(Key, (int) Convert.ToInt32(Results.Rows[row][1]));
                if ((int)Convert.ToInt32(Results.Rows[row][2]) == 1)
                    OverrideFirstCategory[Key] = true;
            }

        }

        /// <summary>
        /// This constructor reads publication types from a file
        /// </summary>
        /// <param name="Folder">Folder the file is located in</param>
        /// <param name="Filename">Filename that contains the publication types</param>
        public PublicationTypes(string Folder, string Filename) {
            // Get the publication types from the input file


//            We're replacing the ODBC CSV code with our own CSVReader
//
//            string ConnectionString =
//                "Driver={Microsoft Text Driver (*.txt; *.csv)};Dbq="
//                + Folder + ";";
//            OdbcConnection Connection = new OdbcConnection(ConnectionString);
//            OdbcDataAdapter DataAdapter = new OdbcDataAdapter
//                ("SELECT PublicationType, PubTypeCategoryID  FROM [" + Filename + "]", Connection);
//            DataTable Results = new DataTable() ;
//            int Rows = DataAdapter.Fill(Results);
            DataTable Results = CSVReader.CSVReader.ReadCSVFile(Folder + "\\" + Filename, true);
            int Rows = Results.Rows.Count;

            // Create the instance variables
            Categories = new Hashtable(StringComparer.CurrentCultureIgnoreCase);
            OverrideFirstCategory = new Hashtable(StringComparer.CurrentCultureIgnoreCase);

            // Read the categories -- throw an exception if there's a duplicate
            for (int row = 0; row < Results.Rows.Count; row++)
            {
                string Key = Results.Rows[row][0].ToString();
                if (Categories.ContainsKey(Key))
                {
                    throw new Exception("Publication type file '" + Filename + 
                        "' contains duplicate publication type '" + 
                        Key + "'");
                }

                // Throw an exception if the value is not an integer
                string StringValue = Results.Rows[row][1].ToString();
                if ((!IsNumeric(StringValue)) || (StringValue.Contains(".")))
                {
                    throw new Exception("Publication type file '" + Filename + 
                        "' contains invalid publication category number '" + 
                        Results.Rows[row][1].ToString() + "' for type '" + 
                        Results.Rows[row][0].ToString() + "'");
                }

                // Negative values indicate OverrideFirstCategory
                int Value = Convert.ToInt32(StringValue);
                if (Value < 0)
                {
                    Value = -Value;
                    OverrideFirstCategory[Key] = true;
                }

                // Add the category
                Categories.Add(Key, Value);
            }
        }


        /// <summary>
        /// Write the publication types to the database
        /// </summary>
        /// <param name="DB">DB to write to</param>
        public void WriteToDB(Database DB)
        {
            // Remove the entries from the table
            DB.ExecuteNonQuery("TRUNCATE TABLE PubTypeCategories");

            // Write the publication types
            foreach ( DictionaryEntry Entry in Categories ) {
                string PubType = Entry.Key.ToString();
                int OverrideFirstCategory = 0;
                if (this.OverrideFirstCategory.ContainsKey(PubType))
                {
                    OverrideFirstCategory = 1;
                }
                int PubTypeCategory;
                try {
                    PubTypeCategory = Convert.ToInt32(Entry.Value);
                } catch {
                    throw new Exception("Invalid Publication Type: " + Entry.Key.ToString() + ", " + PubType);
                }

                ArrayList Parameters = new ArrayList();
                Parameters.Add(Database.Parameter(PubType));
                Parameters.Add(Database.Parameter(PubTypeCategory));
                Parameters.Add(Database.Parameter(OverrideFirstCategory));
                DB.ExecuteNonQuery(
                    @"INSERT INTO PubTypeCategories 
                           (PublicationType, PubTypeCategoryID, OverrideFirstCategory)
                      VALUES ( ? , ? , ? )", Parameters
                );
            }
        }

        /// <summary>
        /// Find the category number for a given publication type
        /// </summary>
        /// <param name="PublicationType">The publication type to search for</param>
        /// <returns>The category number of the publication type</returns>
        public int GetCategoryNumber(string PublicationType)
        {
            if (Categories.Contains(PublicationType))
            {
                return (int) Categories[PublicationType];
            }
            else
            {
                return 0;
            }
        }


        /// <summary>
        /// Implements the equivalent of VB's IsNumeric() function
        /// </summary>
        /// <param name="Expression">Expression to evaluate</param>
        /// <returns>Returns true if the expression is numeric, false otherwise</returns>
        public static bool IsNumeric(object Expression)
        {
            bool isNum;
            double retNum;
            isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }
    }
}
