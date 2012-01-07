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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;

namespace Com.StellmanGreene.PubMed
{
    /// <summary>
    /// The Publications class takes the results from a Medline
    /// query (returned by the NCBI class) and creates an array of Publication objects.
    /// </summary>
    public class Publications
    {
        /// <summary>
        /// Table that joins people to publications
        /// </summary>
        public string PeoplePublicationsTable = "PeoplePublications";

        /// <summary>
        /// PublicationList read from the Medline data
        /// </summary>
        public Publication[] PublicationList;


        /// <summary>
        /// Read a set of publications from the database for a specific person
        /// </summary>
        /// <param name="DB">Database to read from</param>
        /// <param name="Author">Author whose publications should be read</param>
        /// <param name="PeoplePublicationsTable">Value for the PeoplePublications table</param>
        /// <param name="SkipMeSHHeadingsAndGrants">Set SkipMeSHHeadingsAndGrants to true to skip reading MeSH headings and grants when reading publications</param>
        public Publications(Database DB, Person Author, string PeoplePublicationsTable, bool SkipMeSHHeadingsAndGrants)
        {
            this.PeoplePublicationsTable = PeoplePublicationsTable;
            InitializePublicationsObject(DB, Author, SkipMeSHHeadingsAndGrants);
        }

        /// <summary>
        /// Read a set of publications from the database for a specific person
        /// </summary>
        /// <param name="DB">Database to read from</param>
        /// <param name="Author">Author whose publications should be read</param>
        /// <param name="SkipMeSHHeadingsAndGrants">Set SkipMeSHHeadingsAndGrants to true to skip reading MeSH headings and grants when reading publications</param>
        public Publications(Database DB, Person Author, bool SkipMeSHHeadingsAndGrants)
        {
            InitializePublicationsObject(DB, Author, SkipMeSHHeadingsAndGrants);
        }


        /// <summary>
        /// Two different Publications() constructors need to call the same function
        /// </summary>
        /// <param name="DB">Database to read from</param>
        /// <param name="Author">Author whose publications should be read</param>
        /// <param name="SkipMeSHHeadingsAndGrants">Set SkipMeSHHeadingsAndGrants to true to skip reading MeSH headings and grants when reading publications</param>
        private void InitializePublicationsObject(Database DB, Person Author, bool SkipMeSHHeadingsAndGrants)
        {
            // Query the database for the publications
            ArrayList Parameters = new ArrayList();
            Parameters.Add(Database.Parameter(Author.Setnb));
            DataTable Pubs = DB.ExecuteQuery
                ("SELECT PMID FROM " + PeoplePublicationsTable + " WHERE Setnb = ?", Parameters);
            if (Pubs.Rows.Count == 0)
            {
                throw new Exception("No publications found for setnb '" + Author.Setnb + "'");
            }
            else
            {
                List<Publication> tempList = new List<Publication>();
                // Read each publication
                foreach (DataRow PMIDRow in Pubs.Rows)
                {
                    int PMID = (int)PMIDRow["PMID"];
                    Publication publication;
                    if (Publications.GetPublication(DB, PMID, out publication, SkipMeSHHeadingsAndGrants) == false)
                    {
                        throw new Exception("No publication found in the database for PMID " + PMID.ToString());
                    }
                    tempList.Add(publication);
                }
                PublicationList = tempList.Count > 0 ? tempList.ToArray() : null;
            }
        }

        /// <summary>
        /// Read a string with data full of Medline citations and extract all of 
        /// the publications
        /// </summary>
        /// <param name="MedlineData">String with data full of Medline citations</param>
        public Publications(string MedlineData, PublicationTypes pubTypes)
        {
            // Get each publication from MedlineData and add it to PublicationList[]
            Publication publication;
            List<Publication> tempList = new List<Publication>();
            if (PublicationList != null) tempList.AddRange(PublicationList);
            while (GetNextPublication(ref MedlineData, out publication, pubTypes))
            {
                // Add the publication to the end of PublicationList[]
                tempList.Add(publication);
            }
            PublicationList = tempList.Count > 0 ? tempList.ToArray() : null;
        }


        /// <summary>
        /// Strip the next publication off of the top of the string.
        /// </summary>
        /// <param name="MedlineData">A string containing the Medline data, passed 
        /// by reference so that the first publication can be stripped off</param>
        /// <param name="publication">A publication that will contain the next publication in the Medline stream</param>
        /// <returns>True if a publication was read, false otherwise</param>
        private static bool GetNextPublication(ref string MedlineData, out Publication publication, PublicationTypes pubTypes) 
        {
            Publication PublicationToWrite = new Publication();

            string line;
            StringReader reader = new StringReader(MedlineData);

            // Skip past any blank lines at the top of the publication
            // Return null if there are no more publications
            line = reader.ReadLine();
            if (line == null)
            {
                // There are no more publications
                publication = new Publication();
                return false;
            }

            else if (line.Trim().Length == 0)
            {
                // There are blank lines to skip. Read each line, and if it's blank
                // advance MedlineData past it.
                while (line.Trim().Length == 0)
                {
                    MedlineData = reader.ReadToEnd();
                    reader = new StringReader(MedlineData);
                    line = reader.ReadLine();
                    if (line == null)
                    {
                        publication = new Publication();
                        return false;
                    }
                }
                // MedlineData is now set to the first line after the blanks
            }
            // Never mind, set reader back to the beginning of MedlineData
            reader = new StringReader(MedlineData);

            // Read the next line, and keep reading until it hits a blank line
            // or the end of the file
            while (((line = reader.ReadLine()) != null) && (line.Trim().Length != 0))
            {
                // Take each following line that starts with a space and add them
                // to the end of the current line
                while (reader.Peek() == ' ')
                    line = line + " " + reader.ReadLine().Trim();
                Publications.ProcessMedlineTag(ref PublicationToWrite, line, pubTypes);
            }

            MedlineData = reader.ReadToEnd();

            publication = PublicationToWrite;
            return true;
        }


        /// <summary>
        /// Initialize the publication from the database
        /// </summary>
        /// <param name="DB">Database to read from</param>
        /// <param name="PMID">PMID of the publication</param>
        /// <param name="publication">The publication to write</param>
        /// <param name="SkipMeSHHeadingsAndGrants">Set SkipMeSHHeadingsAndGrants to true to skip reading MeSH headings and grants</param>
        /// <returns>True if the publication was found, false otherwise</returns>
        public static bool GetPublication(Database DB, int PMID, out Publication publication, bool SkipMeSHHeadingsAndGrants)
        {
            publication = new Publication();
            publication.PMID = PMID;

            // Read the publication information
            ArrayList Parameters = new ArrayList();
            Parameters.Add(Database.Parameter(PMID));
            DataTable Results = DB.ExecuteQuery(
                @"SELECT Journal, Year, Authors, Month, Day, Title, Volume, 
                                 Issue, Pages, PubType
                            FROM Publications
                           WHERE PMID = ?", Parameters);    
            if (Results.Rows.Count == 0)
            {
                throw new Exception("Article " + PMID.ToString() + " not found");
            }
            DataRow Row = Results.Rows[0];

            publication.Year = (int)Row["Year"];
            publication.Journal = Row["Journal"].Equals(DBNull.Value) ? null : Row["Journal"].ToString();
            publication.Month = Row["Month"].Equals(DBNull.Value) ? null : Row["Month"].ToString();
            publication.Day = Row["Day"].Equals(DBNull.Value) ? null : Row["Day"].ToString();
            publication.Title = Row["Title"].Equals(DBNull.Value) ? null : Row["Title"].ToString();
            publication.Volume = Row["Volume"].Equals(DBNull.Value) ? null : Row["Volume"].ToString();
            publication.Issue = Row["Issue"].Equals(DBNull.Value) ? null : Row["Issue"].ToString();
            publication.Pages = Row["Pages"].Equals(DBNull.Value) ? null : Row["Pages"].ToString();
            publication.PubType = Row["PubType"].Equals(DBNull.Value) ? null : Row["PubType"].ToString();


            // Read the authors
            Parameters = new ArrayList();
            Parameters.Add(Database.Parameter(PMID));
            Results = DB.ExecuteQuery(
                @"SELECT Author 
                    FROM PublicationAuthors
                   WHERE PMID = ? 
                ORDER BY Position", Parameters);
            if (Results.Rows.Count == 0)
            {
                publication.Authors = new string[] { };
            }
            else
            {
                publication.Authors = new string[Results.Rows.Count];
                for (int i = 0; i < Results.Rows.Count; i++)
                {
                    publication.Authors[i] = Results.Rows[i]["Author"].ToString();
                }
            }

            if (SkipMeSHHeadingsAndGrants == false)
            {

                // Read the MeSH headings
                Parameters = new ArrayList();
                Parameters.Add(Database.Parameter(PMID));
                Results = DB.ExecuteQuery(
                    @"SELECT mh.Heading 
                    FROM MeSHHEadings mh, PublicationMeSHHeadings pm
                   WHERE pm.PMID = ? 
                     AND mh.ID = pm.MeSHHeadingID", Parameters);
                // MeSHHeadings was initialized to null, so only set it if there are rows returned
                if (Results.Rows.Count != 0)
                {
                    publication.MeSHHeadings = new ArrayList();
                    for (int i = 0; i < Results.Rows.Count; i++)
                    {
                        publication.MeSHHeadings.Add(Results.Rows[i]["Heading"].ToString());
                    }
                }

                // Read the grants
                Parameters = new ArrayList();
                Parameters.Add(Database.Parameter(PMID));
                Results = DB.ExecuteQuery(
                    @"SELECT GrantID
                    FROM PublicationGrants
                   WHERE PMID = ?", Parameters);
                // Grants was initialized to null, so only set it if there are rows returned
                if (Results.Rows.Count != 0)
                {
                    publication.Grants = new ArrayList();
                    for (int i = 0; i < Results.Rows.Count; i++)
                    {
                        publication.Grants.Add(Results.Rows[i]["GrantID"].ToString());
                    }
                }

            }

            return true;
        }


        /// <summary>
        /// Read a string containing a Medline tag and its data and populate the
        /// appropriate property
        /// </summary>
        /// <param name="MedlineTag">A Medline tag and its data (with newlines stripped out)</param>
        public static void ProcessMedlineTag(ref Publication publication, string MedlineTag, PublicationTypes pubTypes)
        {
            // Verify that the string is really a tag -- it must start with four characters
            // followed by a dash and a space. If not, don't process it.
            if ((MedlineTag.Length < 6) || (MedlineTag.Substring(4, 2) != "- "))
                return;

            string data = MedlineTag.Substring(5).Trim();

            // Process the tags
            switch (MedlineTag.Substring(0, 4).Trim())
            {

                case "PMID":
                    // "PMID- 16319490"
                    if (IsNumeric(data))
                        publication.PMID = Convert.ToInt32(data);
                    else
                        publication.PMID = 0;
                    break;

                case "DP":
                    // "DP  - 2005 Nov 24"
                    if (IsNumeric(data.Substring(0, 4)))
                    {
                        // Grab the year first, then the rest is month and day.
                        publication.Year = Convert.ToInt32(data.Substring(0, 4));
                        data = data.Substring(4).Trim();
                        if (data.Contains(" "))
                        {
                            publication.Month = data.Substring(0, data.IndexOf(" "));
                            publication.Day = data.Substring(data.IndexOf(" ") + 1);
                        }
                        else
                        {
                            if (data.Length != 0)
                                publication.Month = data;
                        }
                    }
                    break;

                case "TI":
                    // "TI  - Title..."
                    publication.Title = data;
                    break;

                case "TA":
                    // "TA  - Hum Hered"
                    publication.Journal = data;
                    break;

                case "GR":
                    // "GR  - GM 28356/GM/NIGMS"
                    // GrantID should contain a comma-delimit list of grant IDs
                    if (publication.Grants == null)
                        publication.Grants = new ArrayList();
                        publication.Grants.Add(data);
                    break;

                case "PT":
                    // "PT  - Clinical Trial"
                    // Only copy PubType if it's the first PT tag encountered in this 
                    // publication or if the publication type is flagged as an 
                    // "override first category" publication type
                    if (publication.PubType == null)
                        publication.PubType = data;
                    else if (pubTypes.OverrideFirstCategory.ContainsKey(data))
                        publication.PubType = data;
                    break;

                case "IP":
                    // "IP  - 3"
                    publication.Issue = data;
                    break;

                case "PG":
                    // "PG  - 134-142"
                    publication.Pages = data;
                    break;

                case "LA":
                    // "LA  - eng"
                    publication.Language = data.ToLower();
                    break;

                case "VI":
                    // "VI  - 60"
                    publication.Volume = data;
                    break;

                case "AU":
                    // "AU  - Wang T"
                    // If this is the first author, create the Authors array
                    if (publication.Authors == null)
                    {
                        publication.Authors = new string[1];
                        publication.Authors[0] = data;
                    }
                    else
                    {
                        // Otherwise, copy the author to the end of the Authors array
                        string[] temp = new string[publication.Authors.Length + 1];
                        publication.Authors.CopyTo(temp, 0);
                        publication.Authors = temp;
                        publication.Authors[publication.Authors.GetUpperBound(0)] = data;
                    }
                    break;

                case "MH":
                    // "AU  - Wang T"
                    // If this is the first MeSH heading, create the new ArrayList
                    if (publication.MeSHHeadings == null)
                    {
                        publication.MeSHHeadings = new ArrayList();
                    }
                    publication.MeSHHeadings.Add(data.ToString());
                    break;

            }

        }


        /// <summary>
        /// Add the current publication to the database
        /// </summary>
        /// <param name="publication">Publication to write</param>
        /// <param name="DB">Database to add to</param>
        /// <param name="PubTypes">PublicationTypes object</param>
        /// <param name="Languages">The publication must match one of these languages or it will be rejected</param>
        /// <returns>True if the publication was written or is in the database already, false otherwise</returns>
        public static bool WriteToDB(Publication publication, Database DB, PublicationTypes PubTypes,string[] Languages)
        {
            ArrayList Parameters;

            // If the object already exists in the database, do nothing
            Parameters = new ArrayList();
            Parameters.Add(Database.Parameter(publication.PMID));
            int Count = DB.GetIntValue(
                @"SELECT Count(*) FROM Publications p
                   WHERE p.PMID = ?", Parameters
            );
            if (Count > 0)
                return true; // Return true because the publication is in the database

            // Only write a publication if the language matches one of the languages
            // passed in the Languages parameter -- if Languages is null, accept
            // all values
            if (Languages != null)
            {
                bool Found = false;
                foreach (string Language in Languages)
                {
                    if (publication.Language == Language)
                        Found = true;
                }
                if (!Found)
                    return false;  // Return false because the publication wasn't written
            }

            // Add the authors
            // First delete any authors that are there, in case the add was
            // interrupted partway through
            for (int Position = 0; (publication.Authors != null) && (Position <= publication.Authors.GetUpperBound(0)); Position++)
            {
                int First = (Position == 0) ? 1 : 0;
                int Last = (Position == publication.Authors.GetUpperBound(0)) ? 1 : 0;
                Parameters = new ArrayList();
                Parameters.Add(Database.Parameter(publication.PMID));
                Parameters.Add(Database.Parameter(Position + 1)); // The first author position is 1, not 0
                Parameters.Add(Database.Parameter(Database.Left(publication.Authors[Position],70)));
                Parameters.Add(Database.Parameter(First));
                Parameters.Add(Database.Parameter(Last));
                DB.ExecuteNonQuery(
                    @"INSERT INTO PublicationAuthors 
                        (PMID, Position, Author, First, Last)
                      VALUES (? , ? , ? , ? , ?)"
                    , Parameters);
            }


            // Add the MeSH headings
            for (int Heading = 0; (publication.MeSHHeadings != null) && (Heading < publication.MeSHHeadings.Count); Heading++)
            {
                // If the MeSH heading already exists in the MeSHHeadings table, reuse it,
                // otherwise add it
                int ID;
                string MeSHHeading = Database.Left((string)publication.MeSHHeadings[Heading], 255);
                Parameters = new ArrayList();
                Parameters.Add(Database.Parameter(MeSHHeading));
                DataTable Results = DB.ExecuteQuery("SELECT ID FROM MeSHHeadings WHERE Heading = ?", Parameters);
                if (Results.Rows.Count > 0)
                {
                    ID = Convert.ToInt32(Results.Rows[0][0]);
                }
                else
                {
                    Parameters = new ArrayList();
                    Parameters.Add(Database.Parameter(MeSHHeading));
                    DB.ExecuteNonQuery("INSERT INTO MeSHHeadings (Heading) VALUES (?)", Parameters);
                    ID = DB.GetIntValue("SELECT LAST_INSERT_ID()");
                }

                Parameters = new ArrayList();
                Parameters.Add(Database.Parameter(publication.PMID));
                Parameters.Add(Database.Parameter(ID));
                DB.ExecuteNonQuery(
                    @"INSERT INTO PublicationMeSHHeadings (PMID, MeSHHeadingID)
                        VALUES ( ? , ? )", Parameters);
            }

            // Add the grants
            for (int Grant = 0; (publication.Grants != null) && (Grant < publication.Grants.Count); Grant++)
            {
                // Some publications may have duplicate grants, so only add non-duplicates to avoid
                // primary key problems
                string GrantID = Database.Left((string)publication.Grants[Grant], 50);
                Parameters = new ArrayList();
                Parameters.Add(Database.Parameter(publication.PMID));
                Parameters.Add(Database.Parameter(GrantID));
                if (DB.GetIntValue(
                    @"SELECT Count(*) FROM PublicationGrants
                       WHERE PMID = ? AND GrantID = ?", Parameters) == 0)
                {
                    Parameters = new ArrayList();
                    Parameters.Add(Database.Parameter(publication.PMID));
                    Parameters.Add(Database.Parameter(GrantID));
                    DB.ExecuteNonQuery(
                        @"INSERT INTO PublicationGrants (PMID, GrantID)
                        VALUES ( ? , ? )", Parameters);
                }
            }

            Parameters = new ArrayList();
            Parameters.Add(Database.Parameter(publication.PMID));
            Parameters.Add(Database.Parameter(Database.Left(publication.Journal, 128)));
            Parameters.Add(Database.Parameter(publication.Year));
            Parameters.Add(Database.Parameter(publication.Authors == null ? 0 : publication.Authors.Length));
            Parameters.Add(Database.Parameter(Database.Left(publication.Month, 32)));
            Parameters.Add(Database.Parameter(Database.Left(publication.Day, 32)));
            Parameters.Add(Database.Parameter(Database.Left(publication.Title,244)));
            Parameters.Add(Database.Parameter(Database.Left(publication.Volume, 32)));
            Parameters.Add(Database.Parameter(Database.Left(publication.Issue, 32)));
            Parameters.Add(Database.Parameter(Database.Left(publication.Pages, 50)));

            // Finally, add the publication. This is part of the publication fault
            // tolerance system -- the headings and authors are not "final" until
            // the publication is written, and can be cleared from the database using 
            // Harvester.ClearDataAfterInterruption().

            // Publication type processing -- read the publication type file,
            // create the publication type table, add the types, file types into bins
            Parameters.Add(Database.Parameter(publication.PubType));
            Parameters.Add(Database.Parameter(PubTypes.GetCategoryNumber(publication.PubType)));
            DB.ExecuteNonQuery(
                @"INSERT INTO Publications
                       (PMID, Journal, Year, Authors, Month, Day, Title,
                        Volume, Issue, Pages, PubType, PubTypeCategoryID)
                  VALUES
                       (? , ? , ? , ? , ? , ? , ? , ? , ? , ? , ? , ?  )", Parameters);

            return true;
        }


        /// <summary>
        /// Add a row to PeoplePublications for a person and publication
        /// </summary>
        /// <param name="DB">Database to write to</param>
        /// <param name="person">Person who is the author of the publication</param>
        /// <param name="publication">Publication to write</param>
        public static void WritePeoplePublicationsToDB(Database DB, Person person, Publication publication)
        {
            ArrayList Parameters;

            int AuthorPosition = 0;
            for (int i = 1; (AuthorPosition == 0) && (i <= publication.Authors.Length); i++)
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

            // From the SRS -- definition of position type: 
            // •	1 if the person is the first author
            // •	2 if the person is the last author
            // •	3 if the person is the second author -- only if # authors >= 5
            // •	4 if the person is the next-to-last author -- only if # authors >= 5
            // •	5 if the person is in the middle (and there are five or more authors for the publication)
            Harvester.AuthorPositions PositionType;
            if (AuthorPosition == 1)
                PositionType = Harvester.AuthorPositions.First;
            else if (AuthorPosition == publication.Authors.Length)
                PositionType = Harvester.AuthorPositions.Last;
            else if ((AuthorPosition == 2) && (publication.Authors.Length >= 5))
                PositionType = Harvester.AuthorPositions.Second;
            else if ((AuthorPosition == publication.Authors.Length - 1) && (publication.Authors.Length >= 5))
                PositionType = Harvester.AuthorPositions.NextToLast;
            else if (AuthorPosition == -1)
                PositionType = Harvester.AuthorPositions.None;
            else
                PositionType = Harvester.AuthorPositions.Middle;

            // Write the row to PeoplePublications
            Parameters = new ArrayList();
            Parameters.Add(Database.Parameter(person.Setnb));
            Parameters.Add(Database.Parameter(publication.PMID));
            Parameters.Add(Database.Parameter(AuthorPosition));
            Parameters.Add(Database.Parameter((int)PositionType));
            DB.ExecuteNonQuery(@"INSERT INTO PeoplePublications
                                                (Setnb, PMID, AuthorPosition, PositionType)
                                         VALUES ( ? , ? , ? , ? )", Parameters);
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

        /// <summary>
        /// Get the author position from the PeoplePublications table
        /// </summary>
        /// <param name="publication">The Person to search for</param>
        /// <param name="PositionType">The value in PeoplePublications.PositionType</param>
        /// <returns>The author position (or 0 if not found)</returns>
        public static int GetAuthorPosition(Database DB, int PMID, Person person, out Harvester.AuthorPositions PositionType, string PeoplePublicationsTable)
        {
            ArrayList Parameters = new ArrayList();
            Parameters.Add(Database.Parameter(PMID));
            Parameters.Add(Database.Parameter(person.Setnb));
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


    }


}
