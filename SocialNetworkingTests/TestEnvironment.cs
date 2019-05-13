/*
 *                           Publication Harvester
 *                          Social Networking Report
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
using System.Collections;
using Microsoft.Win32;
using System.Data;
using NUnit.Framework;
using Com.StellmanGreene.PubMed;


namespace Com.StellmanGreene.SocialNetworking
{
    /// <summary>
    /// Make sure that the unit testing environment is set up
    /// </summary>
    [TestFixture]
    public class TestEnvironment
    {
        /// <summary>
        /// There must be an ODBC DSN called "Social Networking Unit Test" that points to a MySQL 5.0 server 
        /// </summary>
        [Test]
        public void CheckDSN()
        {
            ArrayList DSNs = new ArrayList();
            string str;
            RegistryKey rootKey;
            RegistryKey subKey;
            string[] dsnList;
            rootKey = Registry.LocalMachine;
            str = "SOFTWARE\\\\ODBC\\\\ODBC.INI\\\\ODBC Data Sources";
            subKey = rootKey.OpenSubKey(str);
            if (subKey != null)
            {
                dsnList = subKey.GetValueNames();

                foreach (string dsnName in dsnList)
                {
                    DSNs.Add(dsnName);
                }
                subKey.Close();
            }
            rootKey.Close();
            rootKey = Registry.CurrentUser;
            str = "SOFTWARE\\\\ODBC\\\\ODBC.INI\\\\ODBC Data Sources";
            subKey = rootKey.OpenSubKey(str);
            dsnList = subKey.GetValueNames();
            if (subKey != null)
            {
                foreach (string dsnName in dsnList)
                {
                    DSNs.Add(dsnName);
                }
                subKey.Close();
            }
            rootKey.Close();

            Assert.IsTrue(DSNs.Contains("Social Networking Unit Test"), "The unit tests require an ODBC DSN called 'Social Networking Unit Test' that points to a MySQL 5.7 database");
        }


        /// <summary>
        /// Verify that the "Social Networking Unit Test" DSN points to a MySQL 5.7 database
        /// </summary>
        [Test]
        public void CheckDatabaseVersion()
        {
            Database DB = new Database("Social Networking Unit Test");
            DataTable Results = DB.ExecuteQuery("SHOW VARIABLES WHERE Variable_name = 'version'");
            Assert.IsTrue(Results.Rows[0]["value"].ToString().StartsWith("5.7"), "The unit tests require an ODBC DSN called 'Publication Harvester Unit Test' that points to a MySQL 5.7 database");
        }
    }
}
