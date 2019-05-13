/*
 * The unit testing databases are built with test data
 * See http://svn.stellman-greene.com/SocialNetworkingTestData
 *
 * The colleague in the database has the setnb "Alice"
 * The stars are "Justin" and "Carol"
 * Justin's colleagues in the second-degree roster are Effie, Constance, Roger and John
 * Carol's are Mallory, Joe, Lisa and Effie
 * 
 * More details are here:
 * http://svn.stellman-greene.com/SocialNetworkingTestData/Social%20Publications.xls
 */

using System;
using System.Collections.Generic;
using System.Text;
using Com.StellmanGreene.SocialNetworking;
using System.Collections;
using System.Data;
using System.IO;
using Com.StellmanGreene.PubMed;

namespace Com.StellmanGreene.SocialNetworking.Unit_Tests
{
    /// <summary>
    /// Generate unit test data by rebuilding MySQL databases
    /// </summary>
    public static class UnitTestData
    {
        /// <summary>
        /// Create a database object
        /// </summary>
        /// <returns>The database object pointing at the right DSN</returns>
        public static Database GetDB()
        {
            Database DB = new Database("Social Networking Unit Test");
            return DB;
        }

        /// <summary>
        /// Drop and recreate the unit testing databases
        /// </summary>
        public static void Create()
        {
            RebuildDatabase("social_unit_test_firstdegree");
            RebuildDatabase("social_unit_test_seconddegree");
        }


        /// <summary>
        /// Rebuild a database using a SQL file (which must be named appropriately)
        /// </summary>
        /// <param name="DatabaseName">Name of the database to rebuild</param>
        private static void RebuildDatabase(string DatabaseName)
        {
            Database DB = GetDB();
            DB.ExecuteNonQuery("drop database if exists " + DatabaseName + ";");
            DB.ExecuteNonQuery("create database " + DatabaseName + ";");
            DB.ExecuteNonQuery("use " + DatabaseName + ";");
            StreamReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory
                + "\\Test Data\\" + DatabaseName + ".sql");
            string Contents = reader.ReadToEnd();
            int Statement = 0;
            foreach (string SQL in Contents.Split(';'))
            {
                Statement++;
                try
                {
                    if (SQL.Trim() != "")
                    {
                        DB.ExecuteNonQuery(SQL);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("SQL statement #" + Statement.ToString() + " failed: " + ex.Message, "TestCommonPublicationsSetUp");
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Add additional colleagues to the first-degree database
        /// </summary>
        public static void AddExtraColleagues()
        {
            Database DB = GetDB();
            DB.ExecuteNonQuery("insert into social_unit_test_firstdegree.colleagues (setnb, first, last, name1, medlinesearch, harvested) values (\"Jimmy\", \"Jimmy\", \"Johnson\", \"jimmy j\", \"\", 1);");
            DB.ExecuteNonQuery("insert into social_unit_test_firstdegree.starcolleagues (starsetnb, setnb) values (\"Carol\", \"Jimmy\");");

            DB.ExecuteNonQuery("insert into social_unit_test_firstdegree.colleagues (setnb, first, last, name1, medlinesearch, harvested) values (\"Bob\", \"Bob\", \"Bingo\", \"bob b\", \"\", 1);");
            DB.ExecuteNonQuery("insert into social_unit_test_firstdegree.starcolleagues (starsetnb, setnb) values (\"Carol\", \"Bob\");");

            DB.ExecuteNonQuery("insert into social_unit_test_firstdegree.colleagues (setnb, first, last, name1, medlinesearch, harvested) values (\"Frank\", \"Frank\", \"Flat\", \"frank \", \"\", 1);");
            DB.ExecuteNonQuery("insert into social_unit_test_firstdegree.starcolleagues (starsetnb, setnb) values (\"Justin\", \"Frank\");");
        }
    }
}
