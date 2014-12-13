using System;
using System.Collections.Generic;
using System.Text;
using Com.StellmanGreene.PubMed;
using System.Collections;
using System.Data;

namespace Com.StellmanGreene.SocialNetworking
{
    /// <summary>
    /// Find the number of common publictions between two people
    /// </summary>
    public static class CommonPublications
    {
        /// <summary>
        /// Look up the common publications between two people and populate PublicationCountPerYear
        /// </summary>
        /// <param name="Setnb1">Setnb of the first person</param>
        /// <param name="Database1">Database that contains the first person</param>
        /// <param name="IsStar1">True if the first person is a star (not a colleague), false if a star</param>
        /// <param name="Setnb2">Setnb of the second person</param>
        /// <param name="Database2">Database that contains the second person</param>
        /// <param name="IsStar2">True if the second person is a star (not a colleague), false if a star</param>
        /// <param name="DB">Database object to use for querying</param>
        /// <returns>Hashtable that maps a year to the number of common publications</returns>
        public static Hashtable Find(string Setnb1, string Database1, bool IsStar1, string Setnb2, string Database2, bool IsStar2, Database DB)
        {
            string TableName;
            ArrayList Parameters;

            // Retrieve the publications for the first person
            DB.ExecuteNonQuery("use " + Database1 + ";");
            if (IsStar1)
                TableName = "PeoplePublications";
            else
                TableName = "ColleaguePublications";
            Parameters = new ArrayList();
            Parameters.Add(Database.Parameter(Setnb1));
            DataTable FirstPublications = DB.ExecuteQuery(
                @"SELECT p.PMID, p.Year
                    FROM Publications p, " + TableName + @" t
                   WHERE t.PMID = p.PMID
                     AND t.Setnb = ?
                ORDER BY p.PMID", Parameters);

            // Retrieve the publications for the second person
            DB.ExecuteNonQuery("use " + Database2 + ";");
            if (IsStar2)
                TableName = "PeoplePublications";
            else
                TableName = "ColleaguePublications";
            Parameters = new ArrayList();
            Parameters.Add(Database.Parameter(Setnb2));
            DataTable SecondPublications = DB.ExecuteQuery(
                @"SELECT p.PMID, p.Year
                    FROM Publications p, " + TableName + @" t
                   WHERE t.PMID = p.PMID
                     AND t.Setnb = ?
                ORDER BY p.PMID", Parameters);

            // Build the hashtable to return
            Hashtable Results = new Hashtable();

            // Now we have two result sets that contain lists of PMIDs and years
            // for the two people whose publications we're comparing. They're both
            // sorted by year, so we can just loop through and compare their contents.
            int Row1 = 0;
            int Row2 = 0;
            while (Row1 < FirstPublications.Rows.Count && Row2 < SecondPublications.Rows.Count)
            {
                // Compare the current rows. If they're equal, we've got a common 
                // publication. If not, then increment the row with the lower PMID.
                if ((int)FirstPublications.Rows[Row1]["PMID"] == (int)SecondPublications.Rows[Row2]["PMID"])
                {
                    // We have a match -- create or increment the hashtable row
                    int Year = (int)FirstPublications.Rows[Row1]["Year"];
                    if (Results.ContainsKey(Year))
                        Results[Year] = (int)Results[Year] + 1;
                    else
                        Results[Year] = 1;
                    Row1++;
                    Row2++;
                }
                else if ((int)FirstPublications.Rows[Row1]["PMID"] < (int)SecondPublications.Rows[Row2]["PMID"])
                {
                    // The first publication has a lower PMID
                    Row1++;
                }
                else
                {
                    // The second publication has a lower PMID
                    Row2++;
                }
            }

            for (int Year = EarliestYear(Results); Year > 0 && Year <= LatestYear(Results); Year++)
            {
                if (!Results.ContainsKey(Year))
                    Results[Year] = 0;
            }

            return Results;
        }


        /// <summary>
        /// Find the earliest year in a Hashtable returned by CommonPublications.Find()
        /// </summary>
        /// <param name="CommonPublications">Hashtable returned by CommonPublications.Find()</param>
        /// <returns>Earliest year in the Hashtable, or 0 if no publications are in the hash</returns>
        public static int EarliestYear(Hashtable Results)
        {
            if (Results.Count == 0)
                return 0;
            int[] Keys = new int[Results.Keys.Count];
            Results.Keys.CopyTo(Keys, 0);
            Array.Sort(Keys);
            return Keys[0];
        }


        /// <summary>
        /// Find the latest year in a Hashtable returned by CommonPublications.Find()
        /// </summary>
        /// <param name="CommonPublications">Hashtable returned by CommonPublications.Find()</param>
        /// <returns>Latest year in the Hashtable, or 0 if no publications are in the hash</returns>
        public static int LatestYear(Hashtable Results)
        {
            if (Results.Count == 0)
                return 0;
            int[] Keys = new int[Results.Keys.Count];
            Results.Keys.CopyTo(Keys, 0);
            Array.Sort(Keys);
            return Keys[Keys.GetUpperBound(0)];
        }
    
    }
}
