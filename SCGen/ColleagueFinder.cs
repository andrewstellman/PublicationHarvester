using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using Com.StellmanGreene.PubMed;

namespace SCGen
{
    /// <summary>
    /// Object to find the colleagues for a star
    /// </summary>
    public class ColleagueFinder
    {
        // PubMed objects passed as constructor parameters
        private Database DB;
        private Harvester harvester;
        private PublicationTypes pubTypes;
        private NCBI ncbi;

        // The AAMC roster object
        private Roster roster;

        /// <summary>
        /// An alternate PeoplePublications table name, null if not using alternate table name
        /// </summary>
        public string AlternateTableName { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="roster">AAMC roster object</param>
        /// <param name="alternateTableName">Alternate PeoplePublications table name, null if not using it</param>
        public ColleagueFinder(Database DB, Roster roster, NCBI ncbi, string alternateTableName)
        {
            this.DB = DB;
            this.roster = roster;
            this.ncbi = ncbi;
            this.harvester = new Harvester(DB);
            this.pubTypes = new PublicationTypes(DB);
            AlternateTableName = alternateTableName;
        }


        /// <summary>
        /// Find the potential colleagues for a star and write them to the database.
        /// (They need to be stored in the database in case the NCBI web search
        /// fails due to an error -- that way the publications can be re-retrieved.)
        /// </summary>
        /// <param name="Star">Star to search for</param>
        /// <param name="Colleagues">Colleagues to return</param>
        /// <returns>An array of colleagues, or null if none were found</returns>
        public Person[] FindPotentialColleagues(Person Star) {
            // Keep an array of found Setnbs, so each colleague is only found once
            ArrayList FoundSetnbs = new ArrayList();

            // Create the new set of colleagues
            Person[] Colleagues = null;

            // Get all of the star's publications and look through the author lists for matches
            // This will throw an error if there are no publications for the person
            Publications pubs;
            try
            {
                if (String.IsNullOrEmpty(AlternateTableName))
                    pubs = new Publications(DB, Star, true);
                else
                    pubs = new Publications(DB, Star, AlternateTableName, true);
            }
            catch 
            {
                pubs = null;
            }
            if (pubs != null && pubs.PublicationList != null)
            {
                foreach (Publication pub in pubs.PublicationList)
                {
                    foreach (string name in pub.Authors)
                    {
                        // TODO: This only retunrs one person. What if there are two people
                        // who match that name? It should return multiple people, and
                        // add each of them as a potential colleague.
                        Person[] colleagues = roster.FindPerson(name);

                        // If FindPerson returned colleagues (that's not the star), 
                        // we have a match
                        if (colleagues != null)
                        {
                            // FindPerson could return several colleagues whose names match.
                            foreach (Person colleague in colleagues)
                            {
                                // Each of the matches needs to be added as a colleague.
                                if (colleague.Setnb != Star.Setnb)
                                {

                                    // Always add it to ColleagueMatches
                                    ArrayList Parameters = new ArrayList();
                                    Parameters.Add(Database.Parameter(colleague.Setnb));
                                    Parameters.Add(Database.Parameter(Star.Setnb));
                                    Parameters.Add(Database.Parameter(pub.PMID));
                                    Parameters.Add(Database.Parameter(name));
                                    DB.ExecuteNonQuery(@"INSERT INTO ColleagueMatches 
                                  (Setnb, StarSetnb, PMID, MatchName)
                                  VALUES ( ? , ? , ? , ? )", Parameters);

                                    // If the colleague hasn't already been added, add it to the array 
                                    // and write it to the database
                                    if (!FoundSetnbs.Contains(colleague.Setnb))
                                    {
                                        FoundSetnbs.Add(colleague.Setnb);
                                        if (Colleagues == null)
                                            Colleagues = new Person[1];
                                        else
                                            Array.Resize(ref Colleagues, Colleagues.Length + 1);
                                        Colleagues[Colleagues.GetUpperBound(0)] = colleague;
                                        WriteColleagueToDB(DB, Star, colleague);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return Colleagues;
        }



        /// <summary>
        /// Retrieve all of the publications a set of colleagues
        /// </summary>
        /// <param name="Star">Star whose colleagues are being retrieved</param>
        /// <param name="Colleagues">List of colleagues whose publications should be retrieved</param>
        public void GetColleaguePublications(Person[] Colleagues, string[] Languages, List<int> AllowedPubTypeCategories)
        {
            // Keep a list of written Setnbs, just to make sure we don't write the
            // same colleage twice
            ArrayList WrittenSetnbs = new ArrayList();

            // Process each colleague
            foreach (Person Colleague in Colleagues)
            {
                // Only process a colleague that hasn't yet been touched
                if (!WrittenSetnbs.Contains(Colleague.Setnb))
                {
                    WrittenSetnbs.Add(Colleague.Setnb);

                    // Get the colleague's publications
                    // Search NCBI -- if an error is thrown, write that error to the database
                    string results;
                    try
                    {
                        results = ncbi.Search(Colleague.MedlineSearch);
                    }
                    catch (Exception ex)
                    {
                        string Message = "Error reading publications for "
                            + Colleague.Last + " (" + Colleague.Setnb + ex.Message;
                        ColleagueFinder.WriteErrorToDB(Message, DB, Colleague);
                        throw new Exception(Message);
                    }

                    // Turn the results into a set of publications for the colleague
                    if ((results != null) && (results.Trim() != ""))
                    {
                        Publications ColleaguePublications = new Publications(results, pubTypes);

                        // Write the publications to the database -- but only if they
                        // actually belong to the colleague.
                        if (ColleaguePublications.PublicationList != null)
                            foreach (Publication pub in ColleaguePublications.PublicationList)
                            {
                                // If the publication has no authors, it's clearly not actually
                                // a publication that belongs to this colleague. 
                                // Also, since the publication harvester only harvests 
                                // English publications, we exclude any non-English ones as well.
                                if ((pub.Authors != null) && (pub.Language == "eng")
                                    && (AllowedPubTypeCategories.Contains(pubTypes.GetCategoryNumber(pub.PubType))))
                                {
                                    // Add a row to the ColleaguePublications table -- this will
                                    // return False if the publication doesn't actually belong
                                    // to the colleague
                                    bool PubBelongsToColleague = WriteColleaguePublicationsToDB(DB, Colleague, pub, pubTypes, Languages);
                                    if (PubBelongsToColleague)
                                    {
                                        // Make sure the publication doesn't already exist, then write
                                        // it to the database.
                                        if (DB.GetIntValue("SELECT Count(*) FROM Publications WHERE PMID = " + pub.PMID.ToString()) == 0)
                                            Publications.WriteToDB(pub, DB, pubTypes, Languages);
                                    }

                                }
                            }

                        // Update the Harvested column in the Colleagues table
                        ArrayList Parameters = new ArrayList();
                        Parameters.Add(Database.Parameter(Colleague.Setnb));
                        DB.ExecuteNonQuery("UPDATE Colleagues SET Harvested = 1 WHERE Setnb = ?", Parameters);
                    }
                }
            }
        }

        /// <summary>
        /// Remove any false "colleagues" from StarColleagues which don't share 
        /// publications with the star
        /// </summary>
        /// <param name="DB"></param>
        public static void RemoveFalseColleagues(Database DB, Form1 ParentForm, string PeoplePublicationsTable)
        {
            // First load all of the People in the database -- these are the stars
            People Stars = new People(DB);
            int Count = 0;
            foreach (Person Star in Stars.PersonList)
            {
                Count++;
                if (ParentForm != null)
                {
                    ParentForm.SetProgressBar(0, Stars.PersonList.Count, Count);
                    ParentForm.AddLogEntry("Checking " + Star.Last + " (" + Star.Setnb + "), " + Count.ToString() + " of " + Stars.PersonList.Count.ToString());
                }

                // Retrieve the star's publications
                // This will throw an error if no publications were found
                Publications StarPublications;
                try
                {
                    StarPublications = new Publications(DB, Star, PeoplePublicationsTable, true);
                }
                catch (Exception ex)
                {
                    if (ParentForm != null)
                        ParentForm.AddLogEntry(ex.Message);
                    StarPublications = null;
                }

                if (StarPublications != null)
                {
                    // Find each of the star's colleagues
                    ArrayList Parameters = new ArrayList();
                    Parameters.Add(Database.Parameter(Star.Setnb));
                    DataTable Colleagues = DB.ExecuteQuery(
                        "SELECT Setnb FROM StarColleagues WHERE StarSetnb = ? ORDER BY Setnb", Parameters
                    );

                    foreach (DataRow Row in Colleagues.Rows)
                    {
                        string ColleagueSetnb = Row["Setnb"].ToString();

                        int PublicationsInCommon = 0;

                        // Get the list of the colleague's publications, store the PMIDs in
                        // an ArrayList
                        Parameters = new ArrayList();
                        Parameters.Add(Database.Parameter(ColleagueSetnb));
                        DataTable ColleaguePublications = DB.ExecuteQuery(
                            "SELECT PMID FROM ColleaguePublications WHERE Setnb = ?", Parameters
                            );
                        ArrayList PMIDs = new ArrayList();
                        foreach (DataRow PubRow in ColleaguePublications.Rows)
                        {
                            if (Publications.IsNumeric(PubRow["PMID"]))
                                PMIDs.Add(Convert.ToInt32(PubRow["PMID"]));
                        }

                        // Check if the star and colleague have publications in common
                        if (StarPublications.PublicationList != null)
                            foreach (Publication pub in StarPublications.PublicationList)
                            {
                                if (PMIDs.Contains(pub.PMID))
                                    PublicationsInCommon++;
                            }

                        // Eliminate any false colleagues
                        if (PublicationsInCommon == 0)
                        {
                            Parameters = new ArrayList();
                            Parameters.Add(Database.Parameter(Star.Setnb));
                            Parameters.Add(Database.Parameter(ColleagueSetnb));
                            DB.ExecuteNonQuery(
                                "DELETE FROM StarColleagues WHERE StarSetnb = ? AND Setnb = ?", Parameters
                            );
                            if (ParentForm != null)
                                ParentForm.AddLogEntry("Removed false colleague " + ColleagueSetnb);
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// Create the Colleague tables
        /// </summary>
        /// <param name="DB">Database to create tables in</param>
        public static void CreateTables(Database DB) {
            DB.ExecuteNonQuery("DROP TABLE IF EXISTS StarColleagues");
            DB.ExecuteNonQuery(@"CREATE TABLE StarColleagues (
              StarSetnb char(8) NOT NULL,
              Setnb char(8) NOT NULL,
              PRIMARY KEY  (StarSetnb, Setnb),
              KEY Setnb (Setnb)
            ) ENGINE=MyISAM DEFAULT CHARSET=utf8;
            ");

            DB.ExecuteNonQuery("DROP TABLE IF EXISTS Colleagues");
            DB.ExecuteNonQuery(@"CREATE TABLE Colleagues (
              Setnb char(8) NOT NULL,
              First varchar(20) default NULL,
              Middle varchar(20) default NULL,
              Last varchar(20) default NULL,
              Name1 varchar(36) NOT NULL,
              Name2 varchar(36) default NULL,
              Name3 varchar(36) default NULL,
              Name4 varchar(36) default NULL,
              Name5 varchar(36) default NULL,
              Name6 varchar(36) default NULL,
              MedlineSearch varchar(10000) NOT NULL,
              Harvested bit(1) NOT NULL default false,
              Error bit(1) default NULL,
              ErrorMessage varchar(512) default NULL,
              PRIMARY KEY  (Setnb)
            ) ENGINE=MyISAM DEFAULT CHARSET=utf8;
            ");

            DB.ExecuteNonQuery("DROP TABLE IF EXISTS ColleaguePublications");
            DB.ExecuteNonQuery(@"CREATE TABLE ColleaguePublications (
              Setnb char(8) NOT NULL,
              PMID int(11) NOT NULL,
              AuthorPosition int(11) NOT NULL,
              PositionType tinyint(4) NOT NULL,
              PRIMARY KEY  (Setnb,PMID),
              KEY index_setnb (Setnb),
              KEY index_pmid (PMID)
            ) ENGINE=MyISAM DEFAULT CHARSET=utf8;
            ");

            DB.ExecuteNonQuery("DROP TABLE IF EXISTS ColleagueMatches");
            DB.ExecuteNonQuery(@"CREATE TABLE ColleagueMatches (
              Setnb char(8) NOT NULL,
              StarSetnb char(8) NOT NULL,
              PMID int(11) NOT NULL,
              MatchName varchar(40) NOT NULL,
              KEY index_setnb (Setnb),
              KEY index_starsetnb (StarSetnb),
              KEY index_pmid (PMID),
              KEY index_matchname (MatchName)
            ) ENGINE=MyISAM DEFAULT CHARSET=utf8;
            ");
        }



        /// <summary>
        /// Add a row to ColleaguePublications for a person and publication
        /// </summary>
        /// <param name="DB">Database to write to</param>
        /// <param name="person">Colleague who is the author of the publication</param>
        /// <param name="publication">Publication to write</param>
        /// <returns>True if the publication really matches the colleague, False otherwise</returns>
        public static bool WriteColleaguePublicationsToDB(Database DB, Person Colleague, Publication publication, PublicationTypes pubTypes, string[] Languages)
        {
            ArrayList Parameters;

            // Write the publication to the database
            Publications.WriteToDB(publication, DB, pubTypes, Languages);

            if (publication.Authors == null)
                return false;

            int AuthorPosition = 0;
            for (int i = 1; (AuthorPosition == 0) && (i <= publication.Authors.Length); i++)
                foreach (string name in Colleague.Names)
                    if (StringComparer.CurrentCultureIgnoreCase.Equals(
                        publication.Authors[i - 1], name.ToUpper()
                        ))
                        AuthorPosition = i;

            // If AuthorPosition is still 0, the colleague wasn't actually found in the 
            // publication, so return false without writing the publication to the database.
            if (AuthorPosition == 0)
                return false;

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
            else
                PositionType = Harvester.AuthorPositions.Middle;

            // Write the row to ColleaguePublications
            Parameters = new ArrayList();
            Parameters.Add(Database.Parameter(Colleague.Setnb));
            Parameters.Add(Database.Parameter(publication.PMID));
            Parameters.Add(Database.Parameter(AuthorPosition));
            Parameters.Add(Database.Parameter((int)PositionType));
            DB.ExecuteNonQuery(@"INSERT INTO ColleaguePublications
                                                (Setnb, PMID, AuthorPosition, PositionType)
                                         VALUES ( ? , ? , ? , ? )", Parameters);
            return true;
        }

        /// <summary>
        /// Write a colleague to the database
        /// </summary>
        /// <param name="Colleague">Colleague to write</param>
        /// <returns>False if the colleague already exists, True if the colleague was written</returns>
        public static bool WriteColleagueToDB(Database DB, Person Star, Person Colleague)
        {
            try
            {
                // Check if the PersonToWrite is already in the database
                ArrayList Parameters = new ArrayList();
                Parameters.Add(Database.Parameter(Star.Setnb));
                Parameters.Add(Database.Parameter(Colleague.Setnb));
                int Count = DB.GetIntValue("SELECT Count(*) FROM Colleagues c, StarColleagues s WHERE c.Setnb = s.Setnb AND s.StarSetnb = ? AND c.Setnb = ?", Parameters);

                if (Count > 0)
                {
                    return false;
                }
                else
                {
                    // The colleague doesn't exist yet -- first add him to Colleagues if
                    // he's not already there
                    Parameters = new ArrayList();
                    Parameters.Add(Database.Parameter(Database.Left(Colleague.Setnb, 8)));
                    if (DB.GetIntValue("SELECT Count(*) FROM Colleagues WHERE Setnb = ?", Parameters) == 0)
                    {
                        Parameters = new ArrayList();
                        Parameters.Add(Database.Parameter(Database.Left(Colleague.Setnb, 8)));
                        Parameters.Add(Database.Parameter(Database.Left(Colleague.First, 20)));
                        Parameters.Add(Database.Parameter(Database.Left(Colleague.Middle, 20)));
                        Parameters.Add(Database.Parameter(Database.Left(Colleague.Last, 20)));
                        Parameters.Add(Database.Parameter(Database.Left(Colleague.Names[0], 36)));
                        Parameters.Add(Database.Parameter(Colleague.Names.Length >= 2 ? Database.Left(Colleague.Names[1], 36) : null));
                        Parameters.Add(Database.Parameter(Colleague.Names.Length >= 3 ? Database.Left(Colleague.Names[2], 36) : null));
                        Parameters.Add(Database.Parameter(Colleague.Names.Length >= 4 ? Database.Left(Colleague.Names[3], 36) : null));
                        Parameters.Add(Database.Parameter(Colleague.Names.Length >= 5 ? Database.Left(Colleague.Names[4], 36) : null));
                        Parameters.Add(Database.Parameter(Colleague.Names.Length >= 6 ? Database.Left(Colleague.Names[5], 36) : null));
                        Parameters.Add(Database.Parameter(Database.Left(Colleague.MedlineSearch, 10000)));
                        DB.ExecuteNonQuery(
                            @"INSERT INTO Colleagues 
                                 (Setnb, First, Middle, Last, Name1, Name2, Name3, Name4, Name5, Name6, MedlineSearch)
                          VALUES ( ? , ? , ? , ? , ? , ? , ? , ? , ? , ? , ? )", Parameters);
                    }

                    // The colleague isn't really a colleague of the star until it's
                    // added to StarColleagues
                    Parameters = new ArrayList();
                    Parameters.Add(Database.Parameter(Database.Left(Star.Setnb, 8)));
                    Parameters.Add(Database.Parameter(Database.Left(Colleague.Setnb, 8)));
                    DB.ExecuteNonQuery(
                        @"INSERT INTO StarColleagues 
                                 (StarSetnb, Setnb)
                          VALUES ( ? , ? )", Parameters);

                    return true;
                }

            }
            catch (Exception ex)
            {
                try
                {
                    // Attempt to write the error to the database
                    ArrayList Parameters = new ArrayList();
                    Parameters.Add(Database.Parameter(1));
                    Parameters.Add(Database.Parameter("Unable to update Colleague row (star " + Star.Setnb + "): " + ex.Message));
                    Parameters.Add(Database.Parameter(Colleague.Setnb));
                    DB.ExecuteQuery(
                        @"UPDATE Colleagues 
                             SET Error = ?, ErrorMessage = ?
                           WHERE Setnb = ?", Parameters);
                }
                catch
                {
                    // Do nothing if the attempt to write Error and ErrorMessage fails
                }
                throw new Exception("Error writing colleague " + Colleague.Setnb + "/" + Star.Setnb + ": " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Write an error to the Colleagues table
        /// </summary>
        public static void WriteErrorToDB(string Message, Database DB, Person Colleague)
        {
            try
            {
                // Attempt to write the error to the database
                ArrayList Parameters = new ArrayList();
                Parameters.Add(Database.Parameter(1));
                Parameters.Add(Database.Parameter(Message));
                Parameters.Add(Database.Parameter(Colleague.Setnb));
                DB.ExecuteQuery(
                    @"UPDATE Colleagues 
                             SET Error = ?, ErrorMessage = ?
                           WHERE Setnb = ?", Parameters);
            }
            catch
            {
                // Do nothing if the attempt to write Error and ErrorMessage fails
            }
        }


        /// <summary>
        /// Get the status of the database
        /// </summary>
        /// <param name="DB">Database to check</param>
        /// <param name="FoundColleagues">Number of colleagues that have been found</param>
        /// <returns>True if the database is populated and ready for colleague finding</returns>
        public static bool GetDBStatus(Database DB, out int ColleaguesFound, out int DiadsFound, out int ColleaguesHarvested, out int ColleaguePublications, out int StarsWithColleagues)
        {
            ColleaguesFound = 0;
            DiadsFound = 0;
            ColleaguesHarvested = 0;
            ColleaguePublications = 0;
            StarsWithColleagues = 0;

            // Make sure that the PublicationHarvester status is good
            bool TablesCreated;
            int numPeople;
            int numPublications;
            int numErrors;
            int numHarvestedPeople;
            DB.GetStatus(out TablesCreated, out numPeople, out numHarvestedPeople, out numPublications, out numErrors);

            // Determine whether all of the tables have been created
            if (!TablesCreated)
                return false;
            DataTable Results = DB.ExecuteQuery("SHOW TABLES");
            if (Results.Rows.Count < 2)
            {
                return false;
            }
            ArrayList Tables = new ArrayList();
            foreach (DataRow Row in Results.Rows)
            {
                Tables.Add(Row[0].ToString().ToLower());
            }
            foreach (string Table in new string[] { "colleagues", "colleaguepublications" })
            {
                if (Tables.Contains(Table) == false)
                {
                    return false;
                }
            }

            DiadsFound = DB.GetIntValue("SELECT Count(*) FROM StarColleagues");
            ColleaguesFound = DB.GetIntValue("SELECT Count(DISTINCT Setnb) FROM StarColleagues");
            ColleaguesHarvested = DB.GetIntValue("SELECT Count(*) FROM Colleagues WHERE Harvested > 0");
            ColleaguePublications = DB.GetIntValue("SELECT Count(*) FROM ColleaguePublications");
            StarsWithColleagues = DB.GetIntValue("SELECT Count(DISTINCT StarSetnb) FROM StarColleagues");

            return true;
        }
    }
}
