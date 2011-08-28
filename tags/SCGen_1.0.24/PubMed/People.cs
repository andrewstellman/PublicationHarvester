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
using System.Data.OleDb;
using Com.StellmanGreene.CSVReader;

namespace Com.StellmanGreene.PubMed
{
    /// <summary>
    /// Read and write lists of people
    /// </summary>
    public class People
    {
        /// <summary>
        /// Table that contains the people to read (usually "People")
        /// </summary>
        public string PeopleTable = "People";


        /// <summary>
        /// Array of Person objects
        /// </summary>
        public List<Person> PersonList;

        /// <summary>
        /// Read a list of people from the database
        /// </summary>
        /// <param name="DB">Database to read from</param>
        public People(Database DB)
        {
            DataTable Results = DB.ExecuteQuery(
                "SELECT " + Database.PEOPLE_COLUMNS + " FROM " + PeopleTable
            );
            CreatePersonsFromDataTable(Results);
        }

        /// <summary>
        /// Read a list of people from the database
        /// </summary>
        /// <param name="DB">Database to read from</param>
        public People(Database DB, string PeopleTable)
        {
            this.PeopleTable = PeopleTable;
            DataTable Results = DB.ExecuteQuery(
                "SELECT " + Database.PEOPLE_COLUMNS + " FROM " + PeopleTable
            );
            CreatePersonsFromDataTable(Results);
        }

        /// <summary>
        /// Read a list of people from the database given a SQL WHERE clause
        /// </summary>
        /// <param name="DB">Database to read from</param>
        /// <param name="NonHarvestedOnly">Indicates whether or not to retrieve only non-narvested people</param>
        public People(Database DB, bool NonHarvestedOnly)
        {
            ArrayList Parameters = new ArrayList();
            Parameters.Add(Database.Parameter(NonHarvestedOnly));
            DataTable Results = DB.ExecuteQuery(
                "SELECT " + Database.PEOPLE_COLUMNS + " WHERE Harvested = ?", Parameters
            );
            CreatePersonsFromDataTable(Results);
        }


        /// <summary>
        /// Read a list of people froman Excel file
        /// </summary>
        /// <param name="Folder">Folder where the People file is located</param>
        /// <param name="Filename">Name of the People file</param>
        public People(string Folder, string Filename)
        {
            string[] Columns = { "setnb", "first", "middle", "last", "name1", "name2", "name3", "name4", "medline_search1" };
            DataTable Results;
            if (Filename.ToLower().EndsWith(".csv"))
            {
                Results = ReadCSVFile(Folder, Filename, Columns);
            }
            else
            {
                Results = ReadExcelFile(Folder, Filename, Columns);
            }
            CreatePersonsFromDataTable(Results);
        }


        /// <summary>
        /// Read the contents of a CSV file into a DataTable
        /// </summary>
        /// <param name="Folder">Folder that contains the CSV file</param>
        /// <param name="Filename">Filename of the CSV file</param>
        /// <param name="Columns">Columns to read</param>
        /// <returns>A DataTable object that contains the contents of the file</returns>
        public static DataTable ReadCSVFile(string Folder, string Filename, string[] Columns)
        {
            // Get the publication types from the input file

//            We're replacing the ODBC CSV code with our own CSVReader
//
//            string ConnectionString =
//                "Driver={Microsoft Text Driver (*.txt; *.csv)};Dbq="
//                + Folder + ";";
//            OdbcConnection Connection = new OdbcConnection(ConnectionString);
//
//            // Turn the column names into a SQL statement
//            string ColumnSQL = "[" + String.Join("], [", Columns) + "]";
//
//            // Read the data from the file and return it
//            OdbcDataAdapter DataAdapter = new OdbcDataAdapter(
//                "SELECT " + ColumnSQL + "FROM [" + Filename + "]",
//                Connection
//            );
//            DataTable Results = new DataTable();
//            DataAdapter.Fill(Results);

            DataTable Results = CSVReader.CSVReader.ReadCSVFile(Folder + "\\" + Filename, true);
            return Results;
        }

        /// <summary>
        /// Read the contents of the first worksheet of an Excel file into a DataTable
        /// </summary>
        /// <param name="Folder">Folder that contains the Excel file</param>
        /// <param name="Filename">Filename of the Excel file</param>
        /// <param name="Columns">Columns to read</param>
        /// <returns>A DataTable object that contains the contents of the file</returns>
        public static DataTable ReadExcelFile(string Folder, string Filename, string[] Columns)
        {
            // Open the Excel file and read the name of the first worksheet from it
            string ConnectionString =
                "Provider=Microsoft.Jet.OLEDB.4.0;Data Source="
                + Folder + "\\" + Filename
                + ";Extended Properties=\"Excel 8.0;HDR=yes;IMEX=1\"";
            OleDbConnection Connection = new OleDbConnection(ConnectionString);
            Connection.Open();
            DataTable ExcelTableSchema = Connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string Worksheet = "";
            for (int Row = 0;
                (Worksheet.Length == 0) && (Row <= ExcelTableSchema.Rows.Count - 1)
                ; Row++)
            {
                string ThisWorksheet;
                ThisWorksheet = ExcelTableSchema.Rows[Row]["TABLE_NAME"].ToString();
                if ((ThisWorksheet.Length >= 2)
                    && (ThisWorksheet.StartsWith("'"))
                    && (ThisWorksheet.EndsWith("'")))
                {
                    ThisWorksheet =
                        ThisWorksheet.Substring(1, ThisWorksheet.Length - 2);
                }
                if ((ThisWorksheet.Length >= 1) && (ThisWorksheet.EndsWith("$")))
                {
                    Worksheet = ThisWorksheet;
                }
            }
            if (Worksheet.Length == 0)
                throw new Exception("Unable to find a worksheet in the spreadsheet '" + Filename + "'");

            // Turn the column names into a SQL statement
            string ColumnSQL = "[" + String.Join("], [", Columns) + "]";

            // Read the data from the worksheet
            OleDbDataAdapter DataAdapter = new OleDbDataAdapter(
                "SELECT " + ColumnSQL + "FROM [" + Worksheet + "]",
                Connection
            );
            DataTable Results = new DataTable();

            // Add each PersonToWrite to PersonList
            DataAdapter.Fill(Results);
            return Results;
        }

        /// <summary>
        /// Take a DataTable object that contains the results from a SQL query
        /// (either against Excel or the People table in the database) and 
        /// turn it into the PersonList array
        /// </summary>
        /// <param name="Report">DataTable that contains either Excel or database People table</param>
        private void CreatePersonsFromDataTable(DataTable Results)
        {
            for (int RowNum = 0; RowNum < Results.Rows.Count; RowNum++)
            {
                DataRow Row = Results.Rows[RowNum];
                Person ThisPerson = new Person(Row, Results.Columns);
                if (PersonList == null)
                    PersonList = new List<Person>();
                PersonList.Add(ThisPerson);
            }
        }



    }
}
