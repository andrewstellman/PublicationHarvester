using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using Com.StellmanGreene.PubMed;

namespace Com.StellmanGreene.SocialNetworking
{
    /// <summary>
    /// Take a colleague and star and look up all second degree stars 
    /// </summary>
    public class SecondDegreeStars
    {
        /// <summary>
        /// Setnb of the colleague whose second degree stars are being found
        /// </summary>
        public readonly string ColleagueSetnb;

        /// <summary>
        /// Setnb of the first degree star
        /// </summary>
        public readonly string FirstDegreeStarSetnb;


        /// <summary>
        /// Setnbs of the second degree stars
        /// </summary>
        public readonly ArrayList Setnbs;

        /// <summary>
        /// Find the second degree stars and populate the object
        /// </summary>
        /// <param name="DB">Database object</param>
        /// <param name="ColleagueSetnb">Setnb of the colleague</param>
        /// <param name="FirstDegreeDB">Name of the first-degree database</param>
        /// <param name="FirstDegreeStarSetnb">Setnb of the 1st degree star</param>
        /// <param name="SecondDegreeDB">Name of the second-degree database</param>
        public SecondDegreeStars(Database DB, string ColleagueSetnb, string FirstDegreeDB, string FirstDegreeStarSetnb, string SecondDegreeDB)
        {
            this.ColleagueSetnb = ColleagueSetnb;
            this.FirstDegreeStarSetnb = FirstDegreeStarSetnb;
            Setnbs = new ArrayList();

            // Retrieve the second degree star Setnbs (sorted)
            ArrayList Parameters = new ArrayList();
            Parameters.Add(Database.Parameter(FirstDegreeStarSetnb));
            DataTable Results = DB.ExecuteQuery(
                @"SELECT DISTINCT Setnb
                    FROM " + SecondDegreeDB + @".StarColleagues
                   WHERE StarSetnb = ?
                ORDER BY Setnb ASC", Parameters);

            // Process the second degree stars
            for (int Row = 0; Row < Results.Rows.Count; Row++)
            {
                DataRow SecondDegreeRow = Results.Rows[Row];
                string SecondDegreeStarSetnb = SecondDegreeRow["Setnb"].ToString();
                if (SecondDegreeStarSetnb != ColleagueSetnb) 
                    Setnbs.Add(SecondDegreeStarSetnb);
            }

            // Make sure the setnbs are sorted
            Setnbs.Sort();
        }

    }
}
