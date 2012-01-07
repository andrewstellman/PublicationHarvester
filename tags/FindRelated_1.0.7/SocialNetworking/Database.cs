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
using System.Data.OleDb;
using System.Data.Odbc;
using System.Collections;

namespace Com.StellmanGreene.SocialNetworking
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
            OdbcCommand Command;
            Command = new OdbcCommand(SQL, Connection);
            return Command.ExecuteNonQuery();
        }

        /// <summary>
        ///  Execute a query that does not return a table
        /// </summary>
        /// <param name="SQL">SQL query to execute</param>
        /// <param name="Parameters">ArrayList of OdbcParameter objects</param>
        /// <returns>Number of rows affected</returns>
        public virtual int ExecuteNonQuery(string SQL, ArrayList Parameters)
        {
            OdbcCommand Command;
            Command = new OdbcCommand(SQL, Connection);
            for (int i = 0; i < Parameters.Count; i++)
                Command.Parameters.Add(Parameters[i]);
            return Command.ExecuteNonQuery();
        }

        /// <summary>
        /// Execute a query that returns a table
        /// </summary>
        /// <param name="SQL">SQL query to execute</param>
        /// <returns>Returns a DataTable containing the results of the query</returns>
        public virtual DataTable ExecuteQuery(string SQL)
        {
            OdbcDataAdapter Query = new OdbcDataAdapter(SQL, Connection);
            DataTable Table = new DataTable();
            Query.Fill(Table);
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
            OdbcCommand Command = new OdbcCommand(SQL, Connection);
            for (int i = 0; i < Parameters.Count; i++)
                Command.Parameters.Add(Parameters[i]);
            OdbcDataAdapter Query = new OdbcDataAdapter(Command);
            DataTable Table = new DataTable();
            Query.Fill(Table);
            return Table;
        }


        /// <summary>
        /// Execute a query that returns a scalar
        /// </summary>
        /// <param name="SQL">SQL query to execute</param>
        /// <returns>Returns an object containing the results of the query</returns>
        public virtual object ExecuteScalar(string SQL)
        {
            OdbcCommand Command = new OdbcCommand(SQL, Connection);
            return Command.ExecuteScalar();
        }


        /// <summary>
        /// Get a string from the database
        /// </summary>
        /// <param name="SQL">SQL query to execute</param>
        /// <returns>Returns a string containing the results of the query</returns>
        public virtual string GetStringValue(string SQL)
        {
            OdbcCommand Command = new OdbcCommand(SQL, Connection);
            return Command.ExecuteScalar().ToString();
        }


        /// <summary>
        /// Get an int from the database
        /// </summary>
        /// <param name="SQL">SQL query to execute</param>
        /// <returns>Returns an int containing the results of the query</returns>
        public virtual int GetIntValue(string SQL)
        {
            OdbcCommand Command = new OdbcCommand(SQL, Connection);
            return Convert.ToInt32(Command.ExecuteScalar().ToString());
        }


        /// <summary>
        /// Get an int from the database
        /// </summary>
        /// <param name="SQL">SQL query to execute</param>
        /// <param name="Parameters">ArrayList of OdbcParameter objects</param>
        /// <returns>Returns an int containing the results of the query</returns>
        public virtual int GetIntValue(string SQL, ArrayList Parameters)
        {
            OdbcCommand Command;
            Command = new OdbcCommand(SQL, Connection);
            for (int i = 0; i < Parameters.Count; i++)
                Command.Parameters.Add(Parameters[i]);
            return Convert.ToInt32(Command.ExecuteScalar().ToString());
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
    }
}
