using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Data;
using Com.StellmanGreene.PubMed;

namespace Com.StellmanGreene.FindRelated
{
    class InputQueue
    {
        private readonly List<int> _pmids = new List<int>();

        /// <summary>
        /// Get the next PMIDs from the queue
        /// </summary>
        /// <param name="maxCount">The number of PMIDs to get</param>
        /// <returns>The number of IDs returned</returns>
        public int Next(int maxCount)
        {
            int count = Math.Min(maxCount, _pmids.Count);
            CurrentPmids = _pmids.Take(count);
            _pmids.RemoveRange(0, count);
            return count;
        }

        public IEnumerable<int> CurrentPmids { get; private set; }

        private readonly Database _db;

        private readonly string _queueTableName;

        /// <summary>
        /// The progress through the queue (the total number of PMIDs ever added minus the ones left to process)
        /// </summary>
        public int Progress { get { return _totalPmidsAdded - _pmids.Count; } }

        private int _totalPmidsAdded = 0;

        /// <summary>
        /// The total number of PMIDs that have been added the queue
        /// </summary>
        public int TotalPmidsAdded {  get { return _totalPmidsAdded; } }

        /// <summary>
        /// Read a new input queue from a file and write it to the database
        /// </summary>
        public InputQueue(FileInfo inputFile, Database db, string queueTableName)
        {
            _db = db;
            _queueTableName = queueTableName;

            ReadInputFile(inputFile);
            WriteQueueTable();
        }

        /// <summary>
        /// Read the existing input queue from the database to resume
        /// </summary>
        /// <param name="db"></param>
        /// <param name="queueTableName"></param>
        public InputQueue(Database db, string queueTableName)
        {
            _db = db;
            _queueTableName = queueTableName;

            ResumeInputQueue();
        }

        /// <summary>
        /// Resume an existing inpt queue
        /// </summary>
        private void ResumeInputQueue()
        {
            DataTable queue = _db.ExecuteQuery("SELECT PMID FROM " + _queueTableName + " WHERE Processed = 0 OR Error = 1");
            foreach (DataRow row in queue.Rows)
            {
                if (row["PMID"] != null)
                {
                    _pmids.Add((int)row["PMID"]);
                    _totalPmidsAdded++;
                }
            }
        }

        /// <summary>
        /// Read the input file into the fields
        /// </summary>
        private void ReadInputFile(FileInfo inputFile)
        {
            int lineCount = -1;

            // Read the input file into the peopleIds Dictionary
            try
            {
                using (StreamReader input = inputFile.OpenText())
                {
                    while (!input.EndOfStream)
                    {
                        lineCount++;
                        string line = input.ReadLine();

                        // Check for the correct header
                        if ((lineCount == 0) && (line.ToLower().Trim() != "pmid"))
                        {
                            Trace.WriteLine(DateTime.Now + " ERROR - Input file must have header row 'pmid'");
                            return;
                        }

                        if (lineCount > 0)
                        {
                            if (!int.TryParse(line, out int pmid))
                            {
                                Trace.WriteLine(DateTime.Now + " WARNING - line " + lineCount + ": invalid PMID: " + (String.IsNullOrEmpty(line) ? "(empty)" : line));
                                continue;
                            }

                            _pmids.Add(pmid);
                            _totalPmidsAdded++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(DateTime.Now + " - error occurred reading input file " + inputFile);
                Trace.WriteLine(ex.Message);
                throw ex;
            }

            Trace.WriteLine(DateTime.Now + " Read " + lineCount + " rows from the input file");
        }

        /// <summary>
        /// Write the queue data out to the table
        /// </summary>
        private void WriteQueueTable()
        {
            int count = 0;
            try
            {

                _db.ExecuteNonQuery("TRUNCATE TABLE " + _queueTableName);

                foreach (int pmid in _pmids)
                {
                    _db.ExecuteNonQuery(
                        "INSERT INTO " + _queueTableName + " (PMID) VALUES (?)",
                        new System.Collections.ArrayList() {
                                            Database.Parameter(pmid),
                                });
                    count++;
                }

                Trace.WriteLine(DateTime.Now + " Wrote " + count + " PMIDs to queue table " + _queueTableName);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(DateTime.Now + " Database error after writing " + count + " rows to queue table " + _queueTableName + ": " + ex.Message);
            }
        }

        /// <summary>
        /// Mark a pmid as processed in the queue table
        /// </summary>
        public void MarkProcessed(int pmid)
        {
            try
            {
                _db.ExecuteNonQuery(
                    "UPDATE " + _queueTableName + " SET Processed = 1, Error = 0 WHERE PMID = ?",
                    new System.Collections.ArrayList() { 
                    Database.Parameter(pmid),
                });
            }
            catch (Exception ex)
            {
                Trace.WriteLine(DateTime.Now + " Database error marking PMID " + pmid +" as processed in " + _queueTableName + ": " + ex.Message);
                MarkError(pmid);
            }
        }

        /// <summary>
        /// Mark a PMID as error in the queue table
        /// </summary>
        public void MarkError(int pmid)
        {
            try
            {
                _db.ExecuteNonQuery(
                    "UPDATE " + _queueTableName + " SET Processed = 0, Error = 1 WHERE PMID = ?",
                    new System.Collections.ArrayList() { 
                    Database.Parameter(pmid),
                });
            }
            catch (Exception ex)
            {
                Trace.WriteLine(DateTime.Now + " Database error marking PMID " + pmid +" as error in " + _queueTableName + ": " + ex.Message);
            }
        }
    }
}
