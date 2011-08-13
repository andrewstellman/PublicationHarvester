using System;
using System.Collections.Generic;
using System.Text;
using Com.StellmanGreene.PubMed;

namespace SCGen
{
    public static class CopyPublications
    {
        /// <summary>
        /// Check the database to see if any unharvested colleagues have
        /// publications in another database and copy their publications
        /// </summary>
        /// <param name="DB">Database containing the unharvested colleagues</param>
        /// <param name="SourceDatabaseName">Source database to copy the publications from</param>
        /// <param name="colleaguePublicationsTable">Name of the colleague publications table</param>
        public static void DoCopy(Database DB, string SourceDatabaseName, string PublicationTypes, string colleaguePublicationsTable)
        {
            // Copy authors from the source database's PublicationAuthors
            DB.ExecuteNonQuery(@"/* copy authors */
                                INSERT IGNORE INTO publicationauthors
                                (PMID, Position, Author, First, Last)
                                SELECT pa.PMID, pa.Position, pa.Author, pa.First, pa.Last
                                FROM colleagues c,
                                     " + SourceDatabaseName + @".publications p,
                                     " + SourceDatabaseName + @".peoplepublications pp,
                                     " + SourceDatabaseName + @".publicationauthors pa
                                WHERE c.Setnb = pp.Setnb
                                AND c.Harvested = 0
                                AND pp.PMID = pa.PMID
                                AND pp.PMID = p.PMID
                                AND p.PubTypeCategoryId IN (" + PublicationTypes + ")");

            // Copy grants from the PublicationGrants table
            DB.ExecuteNonQuery(@"/* copy grants */
                                INSERT IGNORE INTO publicationgrants
                                (PMID, GrantID)
                                SELECT pg.PMID, pg.GrantID
                                FROM colleagues c,
                                     " + SourceDatabaseName + @".publications p,
                                     " + SourceDatabaseName + @".peoplepublications pp,
                                     " + SourceDatabaseName + @".publicationgrants pg
                                WHERE c.Setnb = pp.Setnb
                                AND c.Harvested = 0
                                AND pp.PMID = pg.PMID
                                AND pp.PMID = p.PMID
                                AND p.PubTypeCategoryId IN (" + PublicationTypes + ")");

            // Note: we need to insert new MeSH headings manually because
            // their MeSHHeadingID values will not match up between the
            // two databases.

            /*
             * MeSH Headings:
             * 1. Use a "find not matched" query to find any MeSH headings
             *    attached to a source article that does not exist in the
             *    destination database's MeSHHeadings table
             * 2. Add each of the missing MeSH headings to MeSHHeadings
             * 3. When inserting the MeSH headings, do a join on the 
             *    actual heading to insert the proper heading ID
             */

            DB.ExecuteNonQuery(@"/*
                                  * find unmatched headings and insert them
                                  * (the SELECT finds any records in source that don't appear in dest)
                                  */
                                INSERT INTO meshheadings (Heading)
                                SELECT DISTINCT source.Heading
                                  FROM " + SourceDatabaseName + @".meshheadings source
                                  LEFT JOIN meshheadings dest
                                         ON source.Heading = dest.Heading
                                 WHERE dest.Heading IS NULL");

            DB.ExecuteNonQuery(@"/* copy headings
                                  * this is predicated on first finding the unmatched headings and
                                  * inserting them into meshheadings
                                  */
                                INSERT IGNORE INTO publicationmeshheadings
                                (PMID, MeSHHeadingID)
                                SELECT pp.PMID, mhdest.ID AS MeSHHeadnigID
                                FROM colleagues c,
                                     " + SourceDatabaseName + @".publications p,
                                     " + SourceDatabaseName + @".peoplepublications pp,
                                     " + SourceDatabaseName + @".publicationmeshheadings pmsource,
                                     " + SourceDatabaseName + @".meshheadings mhsource,
                                     meshheadings mhdest
                                WHERE c.Setnb = pp.Setnb
                                AND c.Harvested = 0
                                AND pp.PMID = pmsource.PMID
                                AND pmsource.MeSHHeadingID = mhsource.ID /* this is the original MeSH
                                Heading ID */
                                AND mhsource.Heading = mhdest.Heading
                                AND pp.PMID = p.PMID
                                AND p.PubTypeCategoryId IN (" + PublicationTypes + ")");

            // Copy the publications from the Publications table
            DB.ExecuteNonQuery(@"/*
                                  * copy publications
                                  */
                                INSERT IGNORE INTO publications
                                (PMID, Journal, Year, Authors, Month, Day, Title, Volume, Issue, Pages,
                                PubType, PubTypeCategoryID)
                                SELECT p.PMID, p.Journal, p.Year, p.Authors, p.Month, p.Day, p.Title,
                                p.Volume, p.Issue, p.Pages, p.PubType, p.PubTypeCategoryID
                                FROM colleagues c,
                                     " + SourceDatabaseName + @".peoplepublications pp,
                                     " + SourceDatabaseName + @".publications p
                                WHERE c.Setnb = pp.Setnb
                                AND c.Harvested = 0
                                AND pp.PMID = p.PMID
                                AND p.PubTypeCategoryID IN (" + PublicationTypes + ")");

            // Copy the publications from the Publications table
            DB.ExecuteNonQuery(@"/*
                                  * copy colleaguepublications
                                  */
                                INSERT IGNORE INTO " + colleaguePublicationsTable + @"
                                (Setnb, PMID, AuthorPosition, PositionType)
                                SELECT pp.Setnb, pp.PMID, pp.AuthorPosition, pp.PositionType
                                FROM colleagues c,
                                     " + SourceDatabaseName + @".publications p,
                                     " + SourceDatabaseName + @".peoplepublications pp
                                WHERE c.Setnb = pp.Setnb
                                AND c.Harvested = 0
                                AND pp.PMID = p.PMID
                                AND p.PubTypeCategoryId IN (" + PublicationTypes + ")");

            // Mark the copied people as "harvested"
            DB.ExecuteNonQuery(@"/*
                                  * mark copied people as harvested in the destniation database
                                  */
                                UPDATE colleagues c, " + SourceDatabaseName + @".peoplepublications pp,
                                     " + SourceDatabaseName + @".publications p
                                SET c.Harvested = 1
                                WHERE c.Setnb = pp.Setnb
                                AND c.Harvested = 0
                                AND pp.PMID = p.PMID
                                AND p.PubTypeCategoryId IN (" + PublicationTypes + ")");
        }
    }
}
