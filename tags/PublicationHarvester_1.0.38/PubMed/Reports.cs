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
using System.Collections;
using System.Data;
using System.IO;

namespace Com.StellmanGreene.PubMed
{
    public class Reports
    {
        /// <summary>
        /// Table that contains the people to read (usually "People")
        /// </summary>
        public string PeopleTable = "People";

        /// <summary>
        /// Table contains the publications for the people
        /// </summary>
        public string PeoplePublicationsTable = "PeoplePublications";


        /// <summary>
        /// The contents of the journal weights file
        /// </summary>
        public Hashtable Weights;

        /// <summary>
        /// The PublicationTypes object passed in the constructor
        /// </summary>
        public PublicationTypes PubTypes;

        /// <summary>
        /// The Database object passed in the constructor
        /// </summary>
        public Database DB;

        /// <summary>
        /// Initialize the journal weights file
        /// </summary>
        /// <param name="JournalWeightsFilename">Filename of the journal weights file</param>
        public Reports(Database DB, string JournalWeightsFilename)
        {
            PeopleReportSections = DefaultPeopleReportSections();

            string[] Columns = { "JOURNAL TITLE", "JIF" };
            DataTable Results = NpoiHelper.ReadExcelFileToDataTable(
                Path.GetDirectoryName(JournalWeightsFilename), 
                Path.GetFileName(JournalWeightsFilename));

            // Populate the Weights hash table (which was declared to be case-insensitive)
            Weights = new Hashtable(StringComparer.CurrentCultureIgnoreCase);
            foreach (DataRow Row in Results.Rows)
            {
                if (!Weights.ContainsKey(Row["JOURNAL TITLE"].ToString())) 
                    Weights.Add(Row["JOURNAL TITLE"].ToString(), Convert.ToSingle(Row["JIF"]));
            }

            this.PubTypes = new PublicationTypes(DB);
            this.DB = DB;
        }




        /// <summary>
        /// Class to contain the counts for an author
        /// </summary>
        private class Counts
        {
            public int First;
            public float FirstWeighted;
            public int Last;
            public float LastWeighted;
            public int Middle;
            public float MiddleWeighted;
            public int NextToLast;
            public float NextToLastWeighted;
            public int Second;
            public float SecondWeighted;

            /// <summary>
            /// Retrieve counts from a publication list
            /// </summary>
            /// <param name="PublicationList">Publication list to retrieve counts from,
            /// sorted by year, publication type and author position</param>
            /// <param name="Index">Offset in the publication list of the first publication
            /// matching the year and publication type</param>
            /// <param name="Year">Year to match for</param>
            /// <param name="PublicationType">Publication type to match for</param>
            public Counts(Publication[] PublicationList, ref int Index, int Year, int PublicationType, 
                PublicationTypes PubTypes, Database DB, Person person, Hashtable Weights, string PeoplePublicationsTable)
            {
                // Return zero counts if the publication list is empty
                if (PublicationList.Length == 0)
                    return;

                // Return zero counts if the index is out of bounds
                if ((Index < 0) || (Index >= PublicationList.Length))
                    return;

                // Return zero counts if the index doesn't point to a match -- that means
                // there are no matches
                Publication pub = PublicationList[Index];
                int PubType = PubTypes.GetCategoryNumber(pub.PubType);
                if ((pub.Year != Year) || (PubType != PublicationType))
                    return;

                // If we get this far, we have a match. Move forward through the publication
                // list, adding to the counts, until we find a non-matching publication or
                // the list runs out.
                do
                {
                    // Get the weight for the journal
                    float Weight = 0;
                    if (pub.Journal != null && Weights.ContainsKey(pub.Journal))
                    {
                        Weight += (float)Weights[pub.Journal];
                    }

                    // Get the position type, and increment the correct counter
                    Harvester.AuthorPositions PositionType;
                    Publications.GetAuthorPosition(DB, pub.PMID, person, out PositionType, PeoplePublicationsTable);
                    switch (PositionType)
                    {
                        case Harvester.AuthorPositions.First:
                            First++;
                            FirstWeighted += Weight;
                            break;
                        case Harvester.AuthorPositions.Last:
                            Last++;
                            LastWeighted += Weight;
                            break;
                        case Harvester.AuthorPositions.Second:
                            Second++;
                            SecondWeighted += Weight;
                            break;
                        case Harvester.AuthorPositions.NextToLast:
                            NextToLast++;
                            NextToLastWeighted += Weight;
                            break;
                        case Harvester.AuthorPositions.Middle:
                        case Harvester.AuthorPositions.None:
                            Middle++;
                            MiddleWeighted += Weight;
                            break;
                    }
                    Index++;
                    if (Index < PublicationList.Length)
                    {
                        pub = PublicationList[Index];
                        PubType = PubTypes.GetCategoryNumber(pub.PubType);
                    }
                } while ((Index < PublicationList.Length)
                    && (PublicationList[Index].Year == Year)
                    && (PubType == PublicationType));
            }

            /// <summary>
            /// Create a Counts() object that's a sum of other counts
            /// </summary>
            /// <param name="CountsToSum">Array of Counts() objects to sum</param>
            public Counts(Counts[] CountsToSum)
            {
                foreach (Counts counts in CountsToSum)
                {
                    First += counts.First;
                    FirstWeighted += counts.FirstWeighted;

                    Last += counts.Last;
                    LastWeighted += counts.LastWeighted;

                    Middle += counts.Middle;
                    MiddleWeighted += counts.MiddleWeighted;

                    NextToLast += counts.NextToLast;
                    NextToLastWeighted += counts.NextToLastWeighted;

                    Second += counts.Second;
                    SecondWeighted += counts.SecondWeighted;
                }
            }

            /// <summary>
            /// Write the six counts to the report row
            /// </summary>
            /// <param name="writer">Writer to write the counts to</param>
            public void WriteCounts(StringWriter writer)
            {
                // pubcount -- Total nb. of pubs
                int Total = First + Last + Second + NextToLast + Middle;
                writer.Write("," + Total.ToString());

                // wghtd_pubcount -- Weighted total nb. of pubs
                float TotalWeighted = FirstWeighted + LastWeighted + SecondWeighted +
                    NextToLastWeighted + MiddleWeighted;
                writer.Write("," + TotalWeighted.ToString());

                // pubcount_pos1 -- Total nb. of pubs, 1st author
                writer.Write("," + First);

                // wghtd_pubcount_pos1 -- Weighted total nb. of pubs, 1st author
                writer.Write("," + FirstWeighted);

                // pubcount_posN -- Total nb. of pubs, last author
                writer.Write("," + Last);

                // wghtd_pubcount_posN -- Weighted total nb. of pubs, last author
                writer.Write("," + LastWeighted);

                // pubcount_posM -- Total nb. of pubs, middle author
                writer.Write("," + Middle);

                // wghtd_pubcount_posM -- Weighted total nb. of pubs, middle author
                writer.Write("," + MiddleWeighted);

                // pubcount_posNTL -- Total nb. of pubs, next-to-last author
                writer.Write("," + NextToLast);

                // wghtd_pubcount_posNTL -- Weighted total nb. of pubs, next-to-last author
                writer.Write("," + NextToLastWeighted);

                // pubcount_pos2 -- Total nb. of pubs, 2nd author
                writer.Write("," + Second);

                // wghtd_pubcount_pos2 -- Weighted total nb. of pubs, 2nd author
                writer.Write("," + SecondWeighted);

            }
        }


        /// <summary>
        /// Create a row in the People report
        /// </summary>
        /// <param name="person">Person to write</param>
        /// <param name="Pubs">Publications to use as input</param>
        /// <param name="Year">Year to write</param>
        /// <returns>The row in CSV format</returns>
        public string ReportRow(Person person, Publications Pubs, int Year)
        {
            // This function has been optimized so that the software only loops through the list of publications 
            // once. To do this, the list is first sorted in order of year, publication type "bin", author
            // position type and PMID. (PMID is only there so that the ordering of the list is easily predictable.)
            //
            // The function builds one row in the report by constructing an array of values, and then joining
            // that array using commas. (There are no strings with commas, so this will be a valid CSV row.)
            // The row is divided into sections: a set of columns for each bin, one column per author position.

            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);

            // Write the keys
            // setnb -- Person identifier
            // year -- Year of transition
            writer.Write(person.Setnb + ",");
            writer.Write(Year.ToString());


            // The array has been sorted, so we an search for the year. Note that the 
            // binary search may not return the first matching index, so we need to rewind.
            PublicationYearFinder YearFinder = new PublicationYearFinder();
            int Index = Array.BinarySearch(Pubs.PublicationList, Year, YearFinder);
            while ((Index > 0) && (Pubs.PublicationList[Index - 1].Year == Year))
                Index--;

            // Get the counts for each publication type "bin"
            // The bins are defined in the PeopleReportSections array, which
            // contains either "all", "i+j+k+..+y+z" or "n"

            // Query the PublicationTypes table to find all of the pub types,
            // and use them to build a Hashtable, indexed by publication type
            // category, that contains a Counts() object for that type
            Hashtable CategoryCounts = new Hashtable();
            DataTable CategoryTable = DB.ExecuteQuery(
                @"SELECT DISTINCT PubTypeCategoryID FROM PubTypeCategories
                         ORDER BY PubTypeCategoryID;");  // Order by Category ID so it doesn't break the optimization
            int NumCategories = CategoryTable.Rows.Count;
            int[] Categories = new int[NumCategories];
            for (int RowNum = 0; RowNum < CategoryTable.Rows.Count; RowNum++)
            {
                int Category = Convert.ToInt32(CategoryTable.Rows[RowNum]["PubTypeCategoryID"]);
                Categories[RowNum] = Category;
                CategoryCounts[Category] = new Counts(
                    Pubs.PublicationList, ref Index, Year, Category,
                    PubTypes, DB, person, Weights, PeoplePublicationsTable);
            }

            // For each section in PeopleReportSections, write the appropriate section,
            // using the Counts() object that was just calculated and stuck into 
            // the CategoryCounts hashtable
            for (int SectionNum = 0; SectionNum < PeopleReportSections.Length; SectionNum++)
            {
                string Section = PeopleReportSections[SectionNum];
                if (Section == "all")
                {
                    // The section is "all" -- generate a count of all values
                    Counts[] AllCountObjects = new Counts[NumCategories];
                    for (int i = 0; i < NumCategories; i++) {
                        AllCountObjects[i] = (Counts)CategoryCounts[Categories[i]];
                    }
                    Counts AllCounts = new Counts(AllCountObjects);
                    AllCounts.WriteCounts(writer);
                }
                else if (Section.Contains("+"))
                {
                    // The section contains a list of categories separated with +'s
                    // This is a sum of categories (like "1+2+3")
                    string[] SectionSplit = Section.Split('+');
                    Counts[] SumCountObjects = new Counts[SectionSplit.Length];
                    for (int i = 0; i < SectionSplit.Length; i++)
                    {
                        string OneSection = SectionSplit[i];
                        if (!Publications.IsNumeric(OneSection))
                            throw new Exception("ReportSections contains invalid section '" + Section + "'");
                        int SectionValue = Convert.ToInt32(OneSection);
                        if (CategoryCounts.ContainsKey(SectionValue))
                        {
                            Counts OneBinCounts = (Counts) CategoryCounts[SectionValue];
                            SumCountObjects[i] = OneBinCounts;
                        }
                        else
                        {
                            throw new Exception("ReportSections contains invalid section '" + Section + "'");
                        }
                    }
                    Counts SumCounts = new Counts(SumCountObjects);
                    SumCounts.WriteCounts(writer);
                }
                else
                {
                    // The section contains a single bin -- generate a Counts object
                    // and write it out. (Make sure it's a real category!)
                    if (!Publications.IsNumeric(Section)) 
                        throw new Exception("ReportSections contains invalid section '" + Section + "'");
                    int SectionValue = Convert.ToInt32(Section);
                    if (CategoryCounts.ContainsKey(SectionValue))
                    {
                        Counts SingleBinCounts = (Counts)CategoryCounts[Categories[SectionValue]];
                        SingleBinCounts.WriteCounts(writer);
                    }
                    else
                    {
                        throw new Exception("ReportSections contains invalid section '" + Section + "'");
                    }
                }
            }

            return sb.ToString();
        }


        /// <summary>
        /// Callback function for the reports to return status
        /// </summary>
        /// <param name="number">Number of publications processed</param>
        /// <param name="total">Total publications for this person</param>
        public delegate void ReportStatus(int number, int total, Person person, bool ProgressBarOnly);

        /// <summary>
        /// Callback function for reports to send a message
        /// </summary>
        /// <param name="Message">Message to send</param>
        public delegate void ReportMessage(string Message);



        /// <summary>
        /// This array defines the columns that are generated as part of the People report
        /// Each array element either contains "all" (for all bins), "i+j+k+...+y+z" (for
        /// a sum of many bins), or "n" (for bin #n)
        /// </summary>
        public string[] PeopleReportSections;
        
        /// <summary>
        /// This static function returns the default report sections, so the GUI
        /// can populate its list properly
        /// </summary>
        /// <returns>An array of the default People report sections</returns>
        public static string[] DefaultPeopleReportSections() {
            string[] returnValue = { "all", "1+2+3", "1", "2", "3", "4" };
            return returnValue;
        }



        /// <summary>
        /// Add rows to the People report
        /// </summary>
        /// <param name="writer">Writer to write the CSV rows to</param>
        public void PeopleReport(ArrayList SetnbsToSkip, StreamWriter writer, ReportStatus StatusCallback, ReportMessage MessageCallback)
        {
            // Write the header row -- this must be generated dynamically
            // based on the values in PeopleReportSections

            // write the keys
            writer.Write("setnb,year");

            // write a set of column names for each element in PeopleReportSections
            for (int i = 0; i < PeopleReportSections.Length; i++)
            {
                string values = PeopleReportSections[i].ToLower().Trim();
                string[] BaseColumnNames = {
                    "pubcount", "wghtd_pubcount", "pubcount_pos1", 
                    "wghtd_pubcount_pos1", "pubcount_posN", "wghtd_pubcount_posN", 
                    "pubcount_posM", "wghtd_pubcount_posM", "pubcount_posNTL", 
                    "wghtd_pubcount_posNTL", "pubcount_pos2", "wghtd_pubcount_pos2"
                };
                if (values == "all")
                {
                    // all bins -- use the base column names as-is
                    writer.Write("," + String.Join(",", BaseColumnNames));
                } else 
                {
                    // string any +'s from the value type, so "1+2+3" turns into "123"
                    values = values.Replace("+", "");

                    // replace pubcount_posM with 123pubcount_posM
                    // replace wghtd_pubcount_pos1 with wghtd_123pubcount_pos1
                    for (int j = 0; j < BaseColumnNames.Length; j++)
                    {
                        string Column;
                        if (BaseColumnNames[j].Contains("wghtd_"))
                            Column = BaseColumnNames[j].Replace("wghtd_", "wghtd_" + values);
                        else
                            Column = values + BaseColumnNames[j];
                    writer.Write("," + Column);
                    }
                }
            }
            
            writer.WriteLine();

            // Write the row for each person
            People people = new People(DB, PeopleTable);
            int Total = people.PersonList.Count;
            int Number = 0;
            foreach (Person person in people.PersonList)
            {
                Number++;
                StatusCallback(Number, Total, person, false);

                // Skip the person if the Setnb is in SetnbsToSkip
                if ((SetnbsToSkip == null) || (!SetnbsToSkip.Contains(person.Setnb)))
                {

                    // Get the person's publications. If there are no publications for
                    // the person, this will throw an error.
                    Publications pubs;
                    try
                    {
                        pubs = new Publications(DB, person, PeoplePublicationsTable, false);
                    }
                    catch (Exception ex)
                    {
                        MessageCallback(ex.Message);
                        pubs = null;
                    }

                    // Sort the list of publications
                    if (pubs != null)
                    {
                        PublicationComparer Comparer = new PublicationComparer();
                        Comparer.DB = DB;
                        Comparer.person = person;
                        Comparer.publicationTypes = PubTypes;
                        Array.Sort(pubs.PublicationList, Comparer);

                        // Find the minimum and maximum years
                        int YearMinimum = pubs.PublicationList[0].Year;
                        int YearMaximum = pubs.PublicationList[0].Year;
                        if (pubs.PublicationList != null)
                            foreach (Publication pub in pubs.PublicationList)
                            {
                                if (pub.Year < YearMinimum)
                                    YearMinimum = pub.Year;
                                if (pub.Year > YearMaximum)
                                    YearMaximum = pub.Year;
                            }

                        // Write each row
                        for (int Year = YearMinimum; Year <= YearMaximum; Year++)
                        {
                            StatusCallback(Year - YearMinimum, YearMaximum - YearMinimum, person, true);
                            writer.WriteLine(ReportRow(person, pubs, Year));
                        }
                    }
                }
                else
                {
                    MessageCallback("Skipping " + person.Last + " (" + person.Setnb + ")");
                }
            }
        }


        /// <summary>
        /// Write one row of the publications report
        /// </summary>
        /// <param name="pub">Publication to write</param>
        /// <returns></returns>
        public string PubsReportRow(Person person, Publication pub)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);

            // setnb -- unique identifier
            writer.Write(person.Setnb);

            // pmid -- Unique article identifier
            WriteCSV(pub.PMID.ToString(), writer);

            // journal_name -- Name of journal
            WriteCSV(pub.Journal, writer);

            // year -- Year of publication
            WriteCSV(pub.Year.ToString(), writer);

            // Month -- Month of publication
            WriteCSV(pub.Month, writer);

            // day -- Day of publication
            WriteCSV(pub.Day, writer);

            // title -- Article title
            WriteCSV(pub.Title, writer);

            // Volume -- volume number of the journal in which the article was published
            WriteCSV(pub.Volume, writer);

            // issue -- Issue in which the article was published
            WriteCSV(pub.Issue, writer);

            // position -- Position in authorship list
            Harvester.AuthorPositions positionType;
            int authorPosition = person.GetAuthorPosition(DB, pub, out positionType);
            WriteCSV(authorPosition.ToString(), writer);

            // nbauthors -- Number of coauthors (including person)
            WriteCSV(pub.Authors.Length.ToString(), writer);

            // Bin -- From I to IV, based on the PubTypeCategoryID value
            int bin = PubTypes.GetCategoryNumber(pub.PubType);
            WriteCSV(bin.ToString(), writer);

            // Pages -- Page numbers
            WriteCSV(pub.Pages, writer);

            // Publication_type -- Publication Type from Medline
            WriteCSV(pub.PubType, writer);

            return sb.ToString();
        }


        /// <summary>
        /// Write the publications report
        /// </summary>
        /// <param name="writer">Writer to write the report to</param>
        public void PubsReport(ArrayList SetnbsToSkip, StreamWriter writer, ReportStatus StatusCallback, ReportMessage MessageCallback)
        {
            // Write the header row
            string[] columns = { "setnb", "pmid", "journal_name", "year", "Month", 
                "day", "title", "Volume", "issue", "position", "nbauthors", "Bin", 
                "Pages", "Publication_type" };
            writer.WriteLine(String.Join(",", columns));

            // Write the row for each person
            People people = new People(DB, PeopleTable);
            int Total = people.PersonList.Count;
            int Number = 0;
            foreach (Person person in people.PersonList)
            {
                Number++;
                StatusCallback(Number, Total, person, false);

                // Skip the person if the Setnb is in SetnbsToSkip

                if ((SetnbsToSkip == null) || (!SetnbsToSkip.Contains(person.Setnb)))
                {
                    // Get the person's publications -- this will throw an exception
                    // if there are no publications so catch it and use the message
                    // callback
                    Publications pubs = null;
                    try
                    {
                        pubs = new Publications(DB, person, PeoplePublicationsTable, false);
                    }
                    catch (Exception ex)
                    {
                        MessageCallback("Unable to retrive publications for " + person.Last + " (" + person.Setnb + "): " + ex.Message);
                    }

                    if (pubs != null && pubs.PublicationList != null)
                    {
                        foreach (Publication pub in pubs.PublicationList)
                        {
                            // Write each row
                            writer.WriteLine(PubsReportRow(person, pub));
                        }
                    }
                }
                else
                {
                    MessageCallback("Skipping " + person.Last + " (" + person.Setnb + ")");
                }
            }
        }

        /// <summary>
        /// Write a CSV-friendly string
        /// </summary>
        /// <param name="data">String to write</param>
        /// <param name="writer">Writer to write to</param>
        private static void WriteCSV(string data, StringWriter writer)
        {
            if (data == null)
                data = "";

            if (data.Contains(","))
            {
                data = "\"" + data.Replace("\"", "\"\"") + "\"";
            }
            writer.Write("," + data);
        }



        /// <summary>
        /// Create a backup of either a People or Publications report, copying
        /// rows for all but the last Setnb in the file. Return an array of
        /// the Setnbs in the new file. 
        /// </summary>
        /// <returns>ArrayList of Setnbs that were found in the previous file</returns>
        public static ArrayList BackupReportAndGetSetnbs(string Filename)
        {
            if (File.Exists(Filename + ".bak"))
                File.Delete(Filename + ".bak");
            File.Move(Filename, Filename + ".bak");

            // Read the setnbs from the file
            ArrayList Setnbs = new ArrayList();
            string LastSetnb = "";

            // First pass through the file: Get the setnbs
            StreamReader reader = new StreamReader(Filename + ".bak");
            string Line = reader.ReadLine(); // Get the header row
            // If the header row doesn't start with "setnb,", this file is invalid
            // and should be thrown out.
            if ((Line == null) || (!Line.ToLower().StartsWith("setnb,")))
                return null;

            while ((Line = reader.ReadLine()) != null) {
                // If there's a line that doesn't start with a setnb, the file is
                // invalid and should be thrown out.
                if (Line.Length < 10 || Line.Substring(8, 1) != ",")
                    return null;

                string Setnb = Line.Substring(0, 8);
                if ((LastSetnb != "") && (Setnb != LastSetnb))
                    Setnbs.Add(LastSetnb); // This will make sure the last Setnb isn't added
                LastSetnb = Setnb;
            }
            reader.Close();

            reader = new StreamReader(Filename + ".bak");
            StreamWriter writer = new StreamWriter(Filename);
            // Second pass through the file: Copy all but the last setnb
            // This is done so that if the file was interrupted, a partial person
            // isn't saved to the report.
            Line = reader.ReadLine(); // copy the header row
            writer.WriteLine(Line);
            while ((Line = reader.ReadLine()) != null)
            {
                string Setnb = Line.Substring(0, 8);
                if (Setnbs.Contains(Setnb))
                {
                    writer.WriteLine(Line);
                }
            }
            reader.Close();
            writer.Close();

            return Setnbs;
        }


        /// <summary>
        /// Write the MeSH Heading report
        /// </summary>
        /// <param name="writer">Writer to send the report to</param>
        public void MeSHHeadingReport(StreamWriter writer, ReportStatus StatusCallback, ReportMessage MessageCallback)
        {
            // Write the header
            writer.WriteLine("setnb,year,heading,count");

            // The MeSH Heading report has one row per person per year per heading
            People people = new People(DB, PeopleTable);
            int Total = people.PersonList.Count;
            int Count = 0;
            foreach (Person person in people.PersonList)
            {
                // Report status
                Count++;
                StatusCallback(Count, Total, person, false);

                // Catch any errors, report them, and continue
                try
                {

                    // Find the minimum and maximum year for the person
                    int MinYear = 0;
                    int MaxYear = 0;
                    Publications pubs = new Publications(DB, person, PeoplePublicationsTable, false);
                    Hashtable years = new Hashtable();
                    if (pubs.PublicationList != null)
                        foreach (Publication pub in pubs.PublicationList)
                        {
                            if (MinYear == 0 || MinYear > pub.Year)
                                MinYear = pub.Year;
                            if (MaxYear == 0 || MaxYear < pub.Year)
                                MaxYear = pub.Year;

                            // Go through each of the MeSH headings and count how many
                            // occurrences of each heading are in each year. Store each
                            // count in a hashtable keyed by heading, which in turn is 
                            // stored in a hashtable keyed by year.
                            if (!years.ContainsKey(pub.Year))
                                years[pub.Year] = new Hashtable();
                            Hashtable yearHeadings = (Hashtable)years[pub.Year];
                            if (pub.MeSHHeadings != null)
                            {
                                foreach (string Heading in pub.MeSHHeadings)
                                {
                                    if (!yearHeadings.ContainsKey(Heading))
                                        yearHeadings[Heading] = 0;
                                    yearHeadings[Heading] = ((int)yearHeadings[Heading]) + 1;
                                }
                            }
                        }

                    // Write the heading rows for each year
                    for (int Year = MinYear; Year <= MaxYear; Year++)
                    {
                        // Write the rows for that person's year to the writer
                        if (years.ContainsKey(Year))
                        {
                            Hashtable yearHeadings = (Hashtable)years[Year];
                            if (yearHeadings != null)
                            {
                                foreach (string Heading in yearHeadings.Keys)
                                {
                                    StringWriter swriter = new StringWriter();
                                    swriter.Write(person.Setnb);                // setnb
                                    Reports.WriteCSV(Year.ToString(), swriter);    // year
                                    Reports.WriteCSV(Heading, swriter);    // heading
                                    Reports.WriteCSV(yearHeadings[Heading].ToString(), swriter);    // count
                                    writer.WriteLine(swriter.ToString());
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageCallback(ex.Message);
                }
            }
        }

        /// <summary>
        /// Write the Grants report
        /// </summary>
        /// <param name="writer">Writer to send the report to</param>
        public void GrantsReport(StreamWriter writer)
        {
            // Write the header row
            writer.WriteLine("year,pmid,grant_id");

            // Get the data for the grant report
            DataTable Result = DB.ExecuteQuery(
                @"SELECT p.Year, p.PMID, pg.GrantID
                    FROM Publications p, PublicationGrants pg
                   WHERE p.PMID = pg.PMID
                     AND p.PubTypeCategoryID IN (1,2,3,4)
                ORDER BY p.Year, p.PMID"
                );

            // Write the report to the writer
            for (int i = 0; i < Result.Rows.Count; i++)
            {
                DataRow Row = Result.Rows[i];
                StringWriter swriter = new StringWriter();
                swriter.Write(Row[0].ToString());                // year
                Reports.WriteCSV(Row[1].ToString(), swriter);    // PMID
                Reports.WriteCSV(Row[2].ToString(), swriter);    // grant ID
                writer.WriteLine(swriter.ToString());
            }
        }
    
    }
}
