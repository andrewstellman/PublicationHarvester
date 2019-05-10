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
using System.Collections;
using System.Text;
using System.Data;
using System.Data.Odbc;
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
        /// <param name="folder">Folder where the People file is located</param>
        /// <param name="filename">Name of the People file</param>
        public People(string folder, string filename)
        {
            DataTable results;
            if (filename.ToLower().EndsWith(".csv"))
            {
                results = CSVReader.CSVReader.ReadCSVFile(folder + "\\" + filename, true);
            }
            else
            {
                results = NpoiHelper.ReadExcelFileToDataTable(folder, filename);
            }
            CreatePersonsFromDataTable(results);
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
