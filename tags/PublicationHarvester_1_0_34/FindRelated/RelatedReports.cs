using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using Com.StellmanGreene.PubMed;

namespace Com.StellmanGreene.FindRelated
{
    class RelatedReports
    {
        private readonly Database db;
        private readonly string folder;

        public RelatedReports(Database db, string folder)
        {
            if (!folder.EndsWith("\\")) folder += "\\";

            if (!Directory.Exists(folder))
            {
                throw new ArgumentException("Folder does not exist: " + folder, "folder");
            }

            this.db = db;
            this.folder = folder;
        }

        /// <summary>
        /// Execute a report
        /// </summary>
        /// <param name="sql">SQL query for the report</param>
        /// <param name="filename">File to write (will be overwritten)</param>
        /// <param name="columnNames">Column names</param>
        /// <returns></returns>
        int ExecuteReport(string sql, string filename, IEnumerable<string> columnNames)
        {
            // Temporary filename to generate the report into (without column name header)
            string tempFile = Path.GetTempFileName();
            if (File.Exists(tempFile)) File.Delete(tempFile);

            // Query that generates the report into a temporary file
            // (note replacing \ with / in the filename for MySQL)
            string reportSql = sql + @"
-- create a CSV file with the results
INTO OUTFILE '" + tempFile.Replace('\\', '/') + @"'
FIELDS TERMINATED BY ',' OPTIONALLY ENCLOSED BY '""'
ESCAPED BY '\\'
LINES TERMINATED BY '\r\n'";

            // Execute the query, let MySQL do the export
            int linesWritten;
            try
            {
                linesWritten = db.ExecuteNonQuery(reportSql);
            }
            catch (Exception ex)
            {
                string error = "An error occurred while writing " + filename + Environment.NewLine + ex.Message;
                Trace.WriteLine(DateTime.Now + " - " + error);
                MessageBox.Show(error, "Unable to write report", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }

            string outputFile = folder + filename;
            if (File.Exists(outputFile))
                Trace.WriteLine(DateTime.Now + " - overwriting report file " + filename);

            // Copy the report from temporary into the final filename, adding column name header
            try
            {
                using (FileStream inputStream = File.OpenRead(tempFile))
                using (FileStream outputStream = File.Open(outputFile, FileMode.Create)) // Replace any existing file
                using (StreamReader reader = new StreamReader(inputStream))
                using (StreamWriter writer = new StreamWriter(outputStream))
                {
                    writer.WriteLine(String.Join(",", columnNames));
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        writer.WriteLine(line);
                    }
                }
            }
            catch (IOException ex)
            {
                string error = "An error occurred while writing " + filename + Environment.NewLine + ex.Message;
                Trace.WriteLine(DateTime.Now + " - " + error);
                MessageBox.Show(error, "Unable to write report", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }

            Trace.WriteLine(DateTime.Now + " - " + linesWritten + " lines written to " + filename);
            return linesWritten;
        }

        /// <summary>
        /// Generate the Linking report
        /// </summary>
        /// <param name="relatedPublicationsTableName">Related publications table name</param>
        /// <param name="filename">Filename to generate</param>
        public void Linking(string relatedPublicationsTableName, string filename)
        {
            Trace.WriteLine(DateTime.Now + " writing Linking report");

            string sql = @"-- Linking Report
SELECT PMID AS source_pmid, RelatedPMID AS related_pmid,
Rank AS link_ranking, Score AS link_score
FROM " + relatedPublicationsTableName;

            ExecuteReport(sql, filename, 
                new string[] { "source_pmid", "related_pmid", "link_ranking", "link_score" });
        }

        /// <summary>
        /// Generate the RelatedPMID report
        /// </summary>
        /// <param name="relatedPublicationsTableName">Related publications table name</param>
        /// <param name="filename">Filename to generate</param>
        public void RelatedPMID(string relatedPublicationsTableName, string filename)
        {
            Trace.WriteLine(DateTime.Now + " writing RelatedPMID report");

            string sql = @"-- Related PMID report
SELECT rp.RelatedPMID AS related_pmid, p.*
FROM " + relatedPublicationsTableName + @" rp, publications p
WHERE rp.RelatedPMID = p.PMID";

            ExecuteReport(sql, filename,
                new string[] { "related_pmid", "pmid", "journal", "year", "authors", "month", "day", 
                    "title", "volume", "issue", "pages", "pubtype", "pubtypecategoryid" });
        }

        /// <summary>
        /// Generate the RelatedMeSH report
        /// </summary>
        /// <param name="relatedPublicationsTableName">Related publications table name</param>
        /// <param name="filename">Filename to generate</param>
        public void RelatedMeSH(string relatedPublicationsTableName, string filename)
        {
            Trace.WriteLine(DateTime.Now + " writing RelatedMeSH report");

            string sql = @"-- Related MeSH report
SELECT DISTINCT rp.RelatedPMID AS related_pmid, mh.Heading AS related_mesh
FROM " + relatedPublicationsTableName + @" RP, publicationmeshheadings pmh, meshheadings mh
WHERE RP.RelatedPMID = pmh.PMID
AND pmh.MeSHHeadingID = mh.ID";

            ExecuteReport(sql, filename,
                new string[] { "related_pmid", "related_mesh" });
        }


        /// <summary>
        /// Generate the IdeaPeer report
        /// </summary>
        /// <param name="relatedPublicationsTableName">Related publications table name</param>
        /// <param name="filename">Filename to generate</param>
        public void IdeaPeer(string relatedPublicationsTableName, string filename)
        {
            Trace.WriteLine(DateTime.Now + " writing IdeaPeer report");

            string sql = @"-- Idea peer report, with author position and position type for the colleagues based on the related publication
SELECT sc.StarSetnb AS star_setnb, sc.setnb,
rp.PMID AS source_pmid, rp.RelatedPMID AS related_pmid,
cp.AuthorPosition as author_position, cp.PositionType as position_type
FROM starcolleagues sc, peoplepublications pp,
   " + relatedPublicationsTableName + @" rp LEFT JOIN colleaguepublications cp ON (cp.PMID = rp.RelatedPMID)
WHERE sc.StarSetnb = pp.Setnb
AND pp.PMID = rp.PMID
AND cp.Setnb = sc.Setnb";

            ExecuteReport(sql, filename,
                new string[] { "star_setnb", "setnb", "source_pmid", "related_pmid",  "author_position", "position_type" });
        }

        /// <summary>
        /// Generate the Most Relevant report
        /// </summary>
        /// <param name="relatedPublicationsTableName">Related publications table name</param>
        /// <param name="filename">Filename to generate</param>
        public void MostRelevant(string relatedPublicationsTableName, string filename)
        {
            Trace.WriteLine(DateTime.Now + " writing Most Relevant report");

            string sql = @"-- Most Relevant report
SELECT PMID as source_pmid, RelatedPMID as related_pmid, Score as score
FROM " + relatedPublicationsTableName + "_mostrelevant";

            ExecuteReport(sql, filename,
                new string[] { "source_pmid", "related_pmid", "score" });
        }
    }
}
