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
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Data;

namespace Com.StellmanGreene.PubMed
{
    /// <summary>
    /// Class to read the people file and harvest publications
    /// </summary>
    public class Harvester
    {
        public Database DB;
        public string[] Languages = null;

        /// <summary>
        /// Initialize the harvester with a database
        /// </summary>
        /// <param name="DB">Database to use</param>
        public Harvester(Database DB)
        {
            this.DB = DB;
        }


        /// <summary>
        /// Create the tables in database, dropping them first if they exist
        /// </summary>
        /// <param name="DB">Database to add tables to</param>
        public void CreateTables()
        {
            DB.ExecuteNonQuery("DROP TABLE IF EXISTS People");
            DB.ExecuteNonQuery(@"CREATE TABLE People (
              Setnb char(8) NOT NULL,
              First varchar(20) default NULL,
              Middle varchar(20) default NULL,
              Last varchar(20) default NULL,
              Name1 varchar(36) NOT NULL,
              Name2 varchar(36) default NULL,
              Name3 varchar(36) default NULL,
              Name4 varchar(36) default NULL,
              MedlineSearch varchar(512) NOT NULL,
              Harvested bit(1) NOT NULL default '\0',
              Error bit(1) default NULL,
              ErrorMessage varchar(512) default NULL,
              PRIMARY KEY  (Setnb),
              KEY Setnb (Setnb)
            ) ENGINE=MyISAM DEFAULT CHARSET=utf8;
            ");

            DB.ExecuteNonQuery("DROP TABLE IF EXISTS PeoplePublications");
            DB.ExecuteNonQuery(@"CREATE TABLE PeoplePublications (
              Setnb char(8) NOT NULL,
              PMID int(11) NOT NULL,
              AuthorPosition int(11) NOT NULL,
              PositionType tinyint(4) NOT NULL,
              PRIMARY KEY  (Setnb,PMID),
              KEY index_setnb (Setnb),
              KEY index_pmid (PMID)
            ) ENGINE=MyISAM DEFAULT CHARSET=utf8;
            ");

            DB.ExecuteNonQuery("DROP TABLE IF EXISTS Publications");
            DB.ExecuteNonQuery(@"CREATE TABLE Publications (
              PMID int(11) NOT NULL,
              Journal varchar(128) default NULL,
              Year int(11) NOT NULL,
              Authors int(11) default NULL,
              Month varchar(32) default NULL,
              Day varchar(32) default NULL,
              Title varchar(244) default NULL,
              Volume varchar(32) default NULL,
              Issue varchar(32) default NULL,
              Pages varchar(50) default NULL,
              PubType varchar(50) NOT NULL,
              PubTypeCategoryID tinyint(4) NOT NULL,
              PRIMARY KEY  (PMID),
              KEY index_pmid (PMID)
            ) ENGINE=MyISAM DEFAULT CHARSET=utf8;
            ");

            DB.ExecuteNonQuery("DROP TABLE IF EXISTS PublicationAuthors");
            DB.ExecuteNonQuery(@"CREATE TABLE PublicationAuthors (
              PMID int(11) NOT NULL,
              Position int(11) NOT NULL,
              Author varchar(70) NOT NULL,
              First tinyint(4) NOT NULL,
              Last tinyint(4) NOT NULL,
              PRIMARY KEY  (PMID,Position),
              KEY index_pmid (PMID)
            ) ENGINE=MyISAM DEFAULT CHARSET=utf8;
            ");

            DB.ExecuteNonQuery("DROP TABLE IF EXISTS MeSHHeadings");
            DB.ExecuteNonQuery(@"CREATE TABLE MeSHHeadings (
              ID int(11) NOT NULL auto_increment,
              Heading varchar(255) NOT NULL,
              PRIMARY KEY  (ID),
              KEY index_heading (Heading)
            ) ENGINE=MyISAM DEFAULT CHARSET=utf8;
            ");

            DB.ExecuteNonQuery("DROP TABLE IF EXISTS PublicationMeSHHeadings");
            DB.ExecuteNonQuery(@"CREATE TABLE publicationmeshheadings (
              PMID int(11) NOT NULL,
              MeSHHEadingID int(11) NOT NULL,
              PRIMARY KEY  (PMID,MeSHHEadingID),
              KEY index_pmid (PMID)
            ) ENGINE=MyISAM DEFAULT CHARSET=utf8;
            ");

            DB.ExecuteNonQuery("DROP TABLE IF EXISTS PublicationGrants");
            DB.ExecuteNonQuery(@"CREATE TABLE PublicationGrants (
              PMID int(11) NOT NULL,
              GrantID varchar(50) NOT NULL,
              PRIMARY KEY  (PMID,GrantID)
            ) ENGINE=MyISAM DEFAULT CHARSET=utf8;
            ");

            DB.ExecuteNonQuery("DROP TABLE IF EXISTS PubTypeCategories");
            DB.ExecuteNonQuery(@"CREATE TABLE PubTypeCategories (
              PublicationType varchar(90) NOT NULL,
              PubTypeCategoryID tinyint(4) NOT NULL,
               OverrideFirstCategory  tinyint(1) NULL default 0,
              PRIMARY KEY  (PublicationType)
            ) ENGINE=MyISAM DEFAULT CHARSET=utf8;
            ");

        }




        /// <summary>
        /// Read the people and write each of them to the database
        /// 
        /// </summary>
        /// <param name="PeopleFile"></param>
        /// <param name="DB"></param>
        public void ImportPeople(string PeopleFile)
        {
            try
            {
                People peopleFile = new People(Path.GetDirectoryName(PeopleFile), Path.GetFileName(PeopleFile));
                foreach (Person person in peopleFile.PersonList)
                {
                    person.WriteToDB(DB);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Unable to read people file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        /// <summary>
        /// Callback function for GetPublications to return its status
        /// </summary>
        /// <param name="number">Number of publications processed</param>
        /// <param name="total">Total publications for this person</param>
        /// <param name="averageTime">Average time in milliseconds of the call to WriteToDB()</param>
        public delegate void GetPublicationsStatus(int number, int total, int averageTime);

        /// <summary>
        /// Callback function for GetPublications to send a message
        /// </summary>
        /// <param name="Message">Message to send</param>
        public delegate void GetPublicationsMessage(string Message, bool StatusBarOnly);


        /// <summary>
        /// Callback function to check to see if the harvesting process has been interrupted
        /// </summary>
        /// <returns>True if the harvesting process has been interrupted, false otherwise</returns>
        public delegate bool CheckForInterrupt();

        // From the SRS -- definition of position type: 
        // •	1 if the person is the first author
        // •	2 if the person is the last author
        // •	3 if the person is the second author
        // •	4 if the person is the next-to-last author
        // •	5 if the person is in the middle (and there are five or more authors for the publication)
        public enum AuthorPositions
        {
            First = 1,
            Last = 2,
            Second = 3,
            NextToLast = 4,
            Middle = 5,
            None = 6
        }

        /// <summary>
        /// Retrieve the publications for a person and write them to the database
        /// </summary>
        /// <param name="ncbi">NCBI web query object</param>
        /// <param name="pubTypes">PublicationTypes object</param>
        /// <param name="person">Person to retrieve publications for</param>
        /// <param name="StatusCallback">Callback function to return status</param>
        /// <param name="MessageCallback">Callback function to send messages</param>
        /// <param name="AverageMilliseconds">Average time (in milliseconds) of each publication write</param>
        /// <returns>Number of publications written</returns>
        public int GetPublications(NCBI ncbi, PublicationTypes pubTypes, Person person,
            GetPublicationsStatus StatusCallback, GetPublicationsMessage MessageCallback,
            CheckForInterrupt InterruptCallback, out double AverageMilliseconds)
        {
            ArrayList Parameters;

            DateTime StartTime;
            DateTime EndTime;
            double TotalMilliseconds = 0;
            AverageMilliseconds = 0;
            int numberFound = 0;
            int numberWritten = 0;

            // Double-check that the person is really unharvested. If we try to 
            // write publications for a person who already has publications, it will
            // cause an error -- and that could happen if this person was already
            // written from a duplicate person earlier.
            Parameters = new ArrayList();
            Parameters.Add(Database.Parameter(person.Setnb));
            int HarvestedCount = DB.GetIntValue("SELECT Count(*) FROM People WHERE Setnb = ? AND Harvested = 1", Parameters);
            if (HarvestedCount > 0)
            {
                MessageCallback("Already harvested publications for " + person.Last + " (" + person.Setnb + ")", false);
                Parameters = new ArrayList();
                Parameters.Add(Database.Parameter(person.Setnb));
                return DB.GetIntValue("SELECT Count(*) FROM PeoplePublications WHERE Setnb = ?", Parameters);
            }

            
            MessageCallback("Retrieving data from NCBI", true);


            // Find any other people with the same names and search criteria.
            // Any publications found for this person should also be found
            // for them, so when we write the rows to PeoplePublications later
            // we'll also write them for the other people as well.

            // Look in the database for any other people with the same
            // values for name1, name2, name3, name4 and MedlineSearch.
            // Write their PeoplePublications as well.
            string NamesClause = "";
            Parameters = new ArrayList();
            for (int i = 0; i < 4; i++)
            {
                if (i < person.Names.Length)
                {
                    Parameters.Add(Database.Parameter(person.Names[i]));
                    NamesClause += " Name" + ((int)(i+1)).ToString() + " = ? AND ";
                }
                else {
                    NamesClause += " Name" + ((int)(i+1)).ToString() + " IS NULL AND ";
                }
            }
            Parameters.Add(Database.Parameter(person.MedlineSearch));
            Parameters.Add(Database.Parameter(person.Setnb));
            DataTable Results = DB.ExecuteQuery("SELECT " + Database.PEOPLE_COLUMNS + 
                                                 @"FROM People
                                                  WHERE Harvested = 0 AND "
                                                    + NamesClause +
                                                   @" MedlineSearch = ?
                                                    AND Setnb <> ?", Parameters
            );
            ArrayList DuplicatePeople = new ArrayList();
            foreach (DataRow Row in Results.Rows)
            {
                Person dupe = new Person(Row, Results.Columns);
                DuplicatePeople.Add(dupe);
                MessageCallback("Also writing publications for " + dupe.Last + " (" + dupe.Setnb + ") with same names and search criteria", false);
            }


            
            // Search NCBI -- if an error is thrown, write that error to the database
            string results;
            try
            {
                results = ncbi.Search(person.MedlineSearch);
                if (results.Substring(0, 100).Contains("Error occurred"))
                {
                    // NCBI returns an HTML error page in the results
                    //
                    // <html>
                    // <body>
                    // <br/><h2>Error occurred: Unable to obtain query #1</h2><br/> 
                    // ...
                    //
                    // If NCBI returns an empty result set with no publications, it will give the error:
                    // Error occurred: Empty result - nothing todo
                    //
                    // That error should generate a warning and mark the person as harvested in the database.
                    // Any other error should be written to the database as an error.
                    string Error = results.Substring(results.IndexOf("Error occurred"));
                    if (results.Contains("<"))
                        Error = Error.Substring(0, Error.IndexOf("<"));
                    string Message;
                    if (Error.ToLower().Contains("empty result"))
                    {
                        Message = "Warning for "
                            + person.Last + " (" + person.Setnb + "): no publications found (NCBI returned empty results)";
                        person.Harvested = true;
                        person.WriteToDB(DB);
                    }
                    else
                    {
                        Message = "Error reading publications for "
                            + person.Last + " (" + person.Setnb + "): NCBI returned '" + Error + "'";
                        person.WriteErrorToDB(DB, Message);
                    }
                    MessageCallback(Message, false);
                    return 0;
                }
            }
            catch (Exception ex)
            {
                string Message = "Error reading publications for " 
                    + person.Last + "(" + person.Setnb + "): " + ex.Message;
                person.WriteErrorToDB(DB, Message);
                MessageCallback(Message, false);
                return 0;
            }

            Publications mpr = new Publications(results, pubTypes);
            foreach (Publication publication in mpr.PublicationList)
            {
                numberFound++;

                // Exit immediately if the user interrupted the harvest
                if (InterruptCallback())
                    return numberWritten;

                try
                {
                    // Calculate the average time, to return in the callback status function
                    StartTime = DateTime.Now;

                    // Add the publication to PeoplePublications
                    // First find the author position and calculate the position type
                    int AuthorPosition = 0;
                    for (int i = 1; (publication.Authors != null) && (AuthorPosition == 0) && (i <= publication.Authors.Length); i++)
                        foreach (string name in person.Names)
                        {
                            if (StringComparer.CurrentCultureIgnoreCase.Equals(
                                publication.Authors[i - 1], name //.ToUpper()
                                ))
                            {
                                AuthorPosition = i;
                            }
                            else if (name == "*")
                            {
                                AuthorPosition = -1;
                            }
                        }

                    // If for some reason the author doesn't exist in the publication, send a message back
                    // This should never happen
                    if (AuthorPosition == 0)
                        MessageCallback("Publication " + publication.PMID + " does not contain author " + person.Setnb, false);
                    else
                    {
                        // Write the publication to the database
                        if (Publications.WriteToDB(publication, DB, pubTypes, Languages))
                        {
                            // Exit immediately if the user interrupted the harvest
                            if (InterruptCallback())
                                return numberWritten;

                            // Only increment the publication count if the publication
                            // is actually written or already in the database
                            numberWritten++;

                            // Only add the row to PeoplePublications if the publication
                            // was written, or was already in the database. (For example, 
                            // if the publication is not in English, it won't be written.)

                            Publications.WritePeoplePublicationsToDB(DB, person, publication);

                            // Write the publication for each of the other people
                            foreach (Person dupe in DuplicatePeople)
                            {
                                Publications.WritePeoplePublicationsToDB(DB, dupe, publication);
                            }

                            // Calculate the average time per publication in milliseconds
                            EndTime = DateTime.Now;
                            TimeSpan Difference = EndTime - StartTime;
                            TotalMilliseconds += Difference.TotalMilliseconds;
                            AverageMilliseconds = TotalMilliseconds / numberWritten;
                        }
                    }
                }
                catch (Exception ex)
                {
                    person.WriteErrorToDB(DB, ex.Message);
                    throw new Exception("Error writing publication " + publication.PMID.ToString() + ": " + ex.Message, ex);
                }
                StatusCallback(numberFound, mpr.PublicationList.Length, (int)AverageMilliseconds);
            }

            // Make sure each of the people with the same names and search query
            // are marked as harvested and have their errors cleared
            foreach (Person dupe in DuplicatePeople)
            {
                Parameters = new ArrayList();
                Parameters.Add(Database.Parameter(dupe.Setnb));
                DB.ExecuteNonQuery(
                    @"UPDATE People
                         SET Harvested = 1, Error = NULL, ErrorMessage = NULL
                       WHERE Setnb = ?", Parameters);
            }
            
            // Once the publications are all read, updated People.Harvested, as part of
            // the fault-tolerance scheme -- PeoplePublications rows are only "final" when
            // this value is updated for the person. Any others can be cleared using 
            // ClearDataAfterInterruption().
            Parameters = new ArrayList();
            Parameters.Add(Database.Parameter(person.Setnb));
            DB.ExecuteNonQuery(@"UPDATE People
                                    SET Harvested = 1
                                  WHERE Setnb = ?", Parameters); 

            return numberWritten;
        }


        /// <summary>
        /// Clear any left-over data after a database operation was interrupted
        /// </summary>
        /// <param name="DB">Database to clear data from</param>
        public void ClearDataAfterInterruption()
        {
            try
            {
                // Remove any grants, authors and MeSH headings not associated with a publication
                // (The publication is added last, so if the program is interrupted during
                // the publication write, an incomplete publication will have orphan
                // authors and MeSH headings). Note that the headings and publication types
                // can stay in MeSHHeadings and PubTypeCategories.
                DB.ExecuteNonQuery(
                    @"DELETE PublicationMeSHHeadings.* 
                        FROM PublicationMeSHHeadings 
                   LEFT JOIN Publications 
                          ON Publications.PMID = PublicationMeSHHeadings.PMID
                       WHERE Publications.PMID IS NULL"
                    );

                DB.ExecuteNonQuery(
                    @"DELETE PublicationAuthors.* 
                        FROM PublicationAuthors 
                   LEFT JOIN Publications 
                          ON Publications.PMID = PublicationAuthors.PMID
                       WHERE Publications.PMID IS NULL"
                    );

                DB.ExecuteNonQuery(
                    @"DELETE PublicationGrants.* 
                        FROM PublicationGrants 
                   LEFT JOIN Publications 
                          ON Publications.PMID = PublicationGrants.PMID
                       WHERE Publications.PMID IS NULL"
                    );

                // Remove any publications from PeoplePublications associated with 
                // any people without the Harvested flag set
                DB.ExecuteNonQuery(
                    @"DELETE PeoplePublications
                        FROM PeoplePublications, People
                       WHERE PeoplePublications.Setnb = People.Setnb
                         AND People.Harvested = 0"
                    );

                // Clear any errors from People
                DB.ExecuteNonQuery(
                    @"UPDATE People
                         SET Error = 0, ErrorMessage = ''");
            }
            catch (Exception ex)
            {
                throw new Exception("Error clearing data after interruption: " + ex.Message, ex);
            }
        }


        /// <summary>
        /// Check whether data exists from a previous GetPublications() interruption
        /// </summary>
        /// <returns>True if interrupted data exists</returns>
        public bool InterruptedDataExists()
        {
            // If InterruptedCount ends up greater than zero, there is evidence
            // of interruption.
            int InterruptedCount = 0;

            // Interrupted data exists if there are MeSH headings without publications
            InterruptedCount += DB.GetIntValue(
                    @"SELECT Count(*)
                        FROM PublicationMeSHHeadings 
                   LEFT JOIN Publications 
                          ON Publications.PMID = PublicationMeSHHeadings.PMID
                       WHERE Publications.PMID IS NULL"
                    );

            // Ditto for authors
            InterruptedCount += DB.GetIntValue(
                    @"SELECT Count(*)
                        FROM PublicationAuthors 
                   LEFT JOIN Publications 
                          ON Publications.PMID = PublicationAuthors.PMID
                       WHERE Publications.PMID IS NULL"
                    );

            // And if there are any publications associated with unharvested people
            InterruptedCount += DB.GetIntValue(
                    @"SELECT Count(*)
                        FROM PeoplePublications, People
                       WHERE PeoplePublications.Setnb = People.Setnb
                         AND People.Harvested = 0"
                    );

            if (InterruptedCount > 0)
                return true;
            else
                return false;
        }


        /// <summary>
        /// Check whether any unharvested people exist
        /// </summary>
        /// <returns>True if unharvested people exist</returns>
        public bool UnharvestedPeopleExist()
        {
            int UnharvestedCount = DB.GetIntValue(
                    @"SELECT Count(*)
                        FROM People
                       WHERE Harvested = 0"
                    );

            if (UnharvestedCount > 0)
                return true;
            else
                return false;
        }

    }
}
