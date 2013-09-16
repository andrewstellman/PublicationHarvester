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
using System.Data;
using System.Data.Odbc;
using System.Collections;

namespace Com.StellmanGreene.PubMed
{
    public class Database
    {
        OdbcConnection Connection;
        public string DSN;

        /// <summary>
        /// Establish a connection with an ODBC data source
        /// </summary>
        /// <param name="DSN">HandGeneratedData source to connect to</param>
        public Database(string DSN)
        {
            Connection = new OdbcConnection("DSN=" + DSN + ";");
            Connection.Open();
            this.DSN = DSN;
        }

        /// <summary>
        /// Retrieve the status of the data in the database
        /// </summary>
        /// <param name="TablesCreated">True if the Harvested.CreateTables() has been called</param>
        /// <param name="numPeople">Number of people that have been added to the database</param>
        /// <param name="numHarvestedPeople">Number of the people whose publications have been harvested</param>
        /// <param name="numPublications">Number of publications in the database</param>
        public void GetStatus(out bool TablesCreated, out int numPeople,
            out int numHarvestedPeople, out int numPublications, out int numErrors)
        {
            // Determine whether all seven of the tables have been created
            DataTable Results = ExecuteQuery("SHOW TABLES");
            if (Results.Rows.Count < 7)
            {
                TablesCreated = false;
                numPeople = 0;
                numHarvestedPeople = 0;
                numPublications = 0;
                numErrors = 0;
                return;
            }
            ArrayList Tables = new ArrayList();
            foreach (DataRow Row in Results.Rows)
            {
                Tables.Add(Row[0].ToString().ToLower());
            }
            foreach (string Table in new string[] {
                "meshheadings", "people", "peoplepublications", "publicationauthors",
                "publicationmeshheadings", "publications", "pubtypecategories", "publicationgrants"
            }) {
                if (Tables.Contains(Table) == false)
                {
                    TablesCreated = false;
                    numPeople = 0;
                    numHarvestedPeople = 0;
                    numPublications = 0;
                    numErrors = 0;
                    return;
                }
            }
            TablesCreated = true;

            // Get the other values
            numPeople = GetIntValue("SELECT Count(*) FROM People");
            numHarvestedPeople = GetIntValue("SELECT Count(*) FROM People WHERE Harvested = 1");
            numPublications = GetIntValue("SELECT Count(*) FROM Publications");
            numErrors = GetIntValue("SELECT Count(*) FROM People WHERE Error = 1");
        }

        /// <summary>
        ///  Execute a query that does not return a table
        /// </summary>
        /// <param name="SQL">SQL query to execute</param>
        /// <returns>Number of rows affected</returns>
        public virtual int ExecuteNonQuery(string SQL)
        {
            int results;
            OdbcCommand Command;
            using (Command = new OdbcCommand(SQL, Connection))
            {
                results = Command.ExecuteNonQuery();
            }
            return results;
        }

        /// <summary>
        ///  Execute a query that does not return a table
        /// </summary>
        /// <param name="SQL">SQL query to execute</param>
        /// <param name="Parameters">ArrayList of OdbcParameter objects</param>
        /// <returns>Number of rows affected</returns>
        public virtual int ExecuteNonQuery(string SQL, ArrayList Parameters)
        {
            int results;
            OdbcCommand Command;
            using (Command = new OdbcCommand(SQL, Connection))
            {
                for (int i = 0; i < Parameters.Count; i++)
                    Command.Parameters.Add(Parameters[i]);
                results = Command.ExecuteNonQuery();
            }
            return results;
        }

        /// <summary>
        /// Execute a query that returns a table
        /// </summary>
        /// <param name="SQL">SQL query to execute</param>
        /// <returns>Returns a DataTable containing the results of the query</returns>
        public virtual DataTable ExecuteQuery(string SQL)
        {
            DataTable Table = new DataTable();
            using (OdbcDataAdapter Query = new OdbcDataAdapter(SQL, Connection))
            {
                Query.Fill(Table);
            }
            return Table;
        }


        /// <summary>
        /// Execute a query that returns a table
        /// </summary>
        /// <param name="SQL">SQL query to execute</param>
        /// <param name="Parameters">ArrayList of OdbcParameter objects</param>
        /// <returns>Returns a DataTable containing the results of the query</returns>
        public virtual DataTable ExecuteQuery(string SQL, ArrayList Parameters)
        {
            using (OdbcCommand Command = new OdbcCommand(SQL, Connection))
            {
                for (int i = 0; i < Parameters.Count; i++)
                    Command.Parameters.Add(Parameters[i]);
                OdbcDataAdapter Query = new OdbcDataAdapter(Command);
                DataTable Table = new DataTable();
                Query.Fill(Table);
                return Table;
            }
        }


        /// <summary>
        /// Execute a query that returns a scalar
        /// </summary>
        /// <param name="SQL">SQL query to execute</param>
        /// <returns>Returns an object containing the results of the query</returns>
        public virtual object ExecuteScalar(string SQL)
        {
            object result;
            using (OdbcCommand Command = new OdbcCommand(SQL, Connection))
            {
                result = Command.ExecuteScalar();
            }
            return result;
        }


        /// <summary>
        /// Get a string from the database
        /// </summary>
        /// <param name="SQL">SQL query to execute</param>
        /// <returns>Returns a string containing the results of the query</returns>
        public virtual string GetStringValue(string SQL)
        {
            string result;
            using (OdbcCommand Command = new OdbcCommand(SQL, Connection))
            {
                result = Command.ExecuteScalar().ToString();
            }
            return result;
        }


        /// <summary>
        /// Get an int from the database
        /// </summary>
        /// <param name="SQL">SQL query to execute</param>
        /// <returns>Returns an int containing the results of the query</returns>
        public virtual int GetIntValue(string SQL)
        {
            int result;
            using (OdbcCommand Command = new OdbcCommand(SQL, Connection)) {
                result = Convert.ToInt32(Command.ExecuteScalar().ToString());
            }
            return result;
        }


        /// <summary>
        /// Get an int from the database
        /// </summary>
        /// <param name="SQL">SQL query to execute</param>
        /// <param name="Parameters">ArrayList of OdbcParameter objects</param>
        /// <returns>Returns an int containing the results of the query</returns>
        public virtual int GetIntValue(string SQL, ArrayList Parameters)
        {
            int result;
            OdbcCommand Command;
            using (Command = new OdbcCommand(SQL, Connection))
            {
                for (int i = 0; i < Parameters.Count; i++)
                    Command.Parameters.Add(Parameters[i]);
                result = Convert.ToInt32(Command.ExecuteScalar().ToString());
            }
            return result;
        }


        /// <summary>
        /// Create an OdbcParameter
        /// </summary>
        /// <param name="Object">Object to pass to a query</param>
        /// <returns>An OdbcParameter object that has the value Objct</returns>
        public static OdbcParameter Parameter(object Object) {
            if (Object == null) 
                return new OdbcParameter("", DBNull.Value);
            else
                return new OdbcParameter("", Object);
        }


        /// <summary>
        /// Trim a string to a given length, but only if it's greater than that length
        /// </summary>
        /// <param name="Input">String to trim</param>
        /// <param name="Length">Maximum length of the string</param>
        /// <returns>Trimmed string</returns>
        public static string Left(string Input, int Length)
        {
            if (Input == null)
                return null;
            else if (Input.Length > Length)
                return Input.Substring(0, Length);
            else
                return Input;
        }


        /// <summary>
        /// The columns from the People table for use in a SQL query
        /// </summary>
        public const string PEOPLE_COLUMNS = " Setnb, First, Middle, Last, Name1, Name2, Name3, Name4, MedlineSearch, CAST(Harvested AS unsigned integer) AS Harvested, CAST(Error AS unsigned integer) AS Error, ErrorMessage ";

        /// <summary>
        /// Get a boolean value from an object
        /// </summary>
        /// <param name="value">Value to parse</param>
        /// <param name="boolValue">Output of value if one is found</param>
        /// <returns>True if a boolean value is found, false otherwise</returns>
        public static bool GetBoolValue(object value, out bool result)
        {
            /*
             * Note: This is needed as part of a workaround for a bug in MySQL 5.1 and
             * MySQL ODBC Connector 5.1.5, where bit(1) columns always read as true
             * no matter what their actual contents are. If we cast the column as an
             * unsigned integer, it does return the actual value as 1 or 0:
             * 
             * SELECT CAST(Harvested AS unsigned integer) AS Harvested
             * FROM Colleagues
             * 
             * This method is used to parse the results, in as flexible a manner as possible.
             */

            if (value.Equals(1) || value.Equals(true))
            {
                result = true;
                return true;
            }

            if (value.Equals(0) || value.Equals(false))
            {
                result = false;
                return true;
            }

            if (value.Equals(System.DBNull.Value))
            {
                result = false;
                return true;
            }


            int intValue;
            bool boolValue;
            if (int.TryParse(value.ToString(), out intValue))
            {
                if (intValue == 1)
                    result = true;
                else
                    result = false;
                return true;
            }
            else if (bool.TryParse(value.ToString(), out boolValue))
            {
                result = boolValue;
                return true;
            }
            result = true;
            return false;
        }


    }
}
