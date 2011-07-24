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
using System.Collections;
using System.Data.Odbc;

namespace Com.StellmanGreene.PubMed
{
    /// <summary>
    /// The Publication struct contains information about a publication. That 
    /// information is populated by calling the ProcessMedlineTag() function
    /// repeatedly. ProcessMedlineTag() reads a single tag from the output 
    /// of a Medline query and updates the properties accordingly. The calling
    /// object is responsible for sending all of the tags for the publication
    /// through that function.
    /// </summary>
    public struct Publication
    {

        /// <summary>
        /// PubMed ID for the publication (Medline PMID)
        /// </summary>
        public int PMID;

        /// <summary>
        /// Journal name (medline TA)
        /// </summary>
        public string Journal;

        /// <summary>
        /// Publication year (medline DP, year portion of "2005 May 17")
        /// </summary>
        public int Year;

        /// <summary>
        /// Month of publication (or null, if no month is specified)  
        /// (medline DP, optional month portion of "2005 May 17")
        /// </summary>
        public string Month;

        /// <summary>
        /// Day of publication (or null, if no day is specified)
        /// (medline DP, optional day portion of "2005 May 17")
        /// </summary>
        public string Day;

        /// <summary>
        /// Title of publication (or null, if no title is specified) (medline TI)
        /// </summary>
        public string Title;

        /// <summary>
        /// List of authors (medline AU)
        /// </summary>
        public string[] Authors;

        /// <summary>
        /// Volume of publication (or null, if no volume is specified) (medline VI)
        /// </summary>
        public string Volume;

        /// <summary>
        /// Issue of publication (or null, if no issue is specified) (medline IP)
        /// </summary>
        public string Issue;

        /// <summary>
        /// Page numbers of publication (or null, if no pages are specified) (medline PG)
        /// </summary>
        public string Pages;

        /// <summary>
        /// Language of publication (or null, if no language is specified) (medline LA)
        /// </summary>
        public string Language;

        /// <summary>
        /// Listof grant IDs associated with the publication (or null, if no grant ID is specified)
        /// (Medline GR, first occurrence)
        /// </summary>
        public ArrayList Grants;

        /// <summary>
        /// First publication type listed for the publication (or null, if no 
        /// publication type is specified) (medline PT, first occurrence)
        /// </summary>
        public string PubType;


        /// <summary>
        /// List of MeSH headings for the publication (medline MH)
        /// </summary>
        public ArrayList MeSHHeadings;

        public override string ToString()
        {
            return PMID + " " + (Title ?? "[no title]");
        }

    }



    /// <summary>
    /// Comparer class to compare two publications.
    /// </summary>
    public class PublicationComparer : IComparer   
    {
        // Classes to get bin and author position
        public PublicationTypes publicationTypes;
        public Person person;
        public Database DB;

        /// <summary>
        /// Sort publications in order of year, publication type "bin", author position and PMID
        /// </summary>
        /// <param name="pFirstObject"></param>
        /// <param name="pObjectToCompare"></param>
        /// <returns></returns>
        public Int32 Compare(Object pFirstObject, Object pObjectToCompare)
        {
            if ((pFirstObject is Publication) && (pObjectToCompare is Publication))
            {
                Publication First = (Publication)pFirstObject;
                Publication Compare = (Publication)pObjectToCompare;
                if (First.Year != Compare.Year)
                    return Comparer.DefaultInvariant.Compare(First.Year, Compare.Year);
                else
                {
                    int FirstPubType = publicationTypes.GetCategoryNumber(First.PubType);
                    int ComparePubType = publicationTypes.GetCategoryNumber(Compare.PubType);
                    if (FirstPubType != ComparePubType)
                        return Comparer.DefaultInvariant.Compare(FirstPubType, ComparePubType);
                    else
                    {
                        Harvester.AuthorPositions FirstAuthorPosition;
                        person.GetAuthorPosition(DB, First, out FirstAuthorPosition);
                        Harvester.AuthorPositions CompareAuthorPosition;
                        person.GetAuthorPosition(DB, Compare, out CompareAuthorPosition);
                        if (FirstAuthorPosition != CompareAuthorPosition)
                            return Comparer.DefaultInvariant.Compare(FirstAuthorPosition, CompareAuthorPosition);
                        else
                            return Comparer.DefaultInvariant.Compare(First.PMID, Compare.PMID);
                    }
                }
            }
            else
            {
                return 0;
            }
        }
    }


    /// <summary>
    /// IComparer for finding a specific year in an array of publications
    /// </summary>
    public class PublicationYearFinder : IComparer
    {
        public Int32 Compare(Object pFirstObject, Object pObjectToCompare)
        {
            if ((pFirstObject is Publication) && (pObjectToCompare is int))
                return Comparer.DefaultInvariant.Compare(((Publication)pFirstObject).Year, (int)pObjectToCompare);
            else
                return 0;
        }
    }

}
