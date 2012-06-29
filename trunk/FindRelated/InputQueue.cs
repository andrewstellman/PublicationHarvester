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
        /* TODO:
         * Make Next() work
         * Make the properties work
         * Read/write queue from the database
         * Add error reporting
         * Add queue interrupt/clear/restart
         * Add UI
         */ 

        public string CurrentSetnb { get; private set; }

        public IEnumerable<int> CurrentPmids { get; private set; }

        private readonly Database _db;

        private readonly string _queueTableName;

        public int Count 
        {
            get
            {
                if (_setnbs == null)
                    return 0;
                return _setnbs.Count;
            }
        }

        private readonly List<string> _setnbs = new List<string>();
        private readonly Dictionary<string, List<int>> _peopleIds = new Dictionary<string, List<int>>();
        private int _currentIndex = -1;

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
            DataTable queue = _db.ExecuteQuery("SELECT Setnb, PMID FROM " + _queueTableName + " WHERE Processed = 0 OR Error = 1");
            foreach (DataRow row in queue.Rows)
            {
                if (row["Setnb"] != null && row["PMID"] != null)
                {
                    AddPairToQueue(row["Setnb"].ToString(), (int)row["PMID"]);
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
                        string[] split = line.Split(',');

                        // Check for the correct header
                        if (lineCount == 0)
                        {
                            if ((split.Length != 2)
                                || (split[0].Trim().ToLower() != "setnb")
                                || (split[1].Trim().ToLower() != "pmid"))
                            {
                                Trace.WriteLine(DateTime.Now + " ERROR - Input file must have header row 'setnb,pmid'");
                                return;
                            }
                            continue;
                        }

                        int pmid;
                        if (split.Length != 2 || !int.TryParse(split[1], out pmid))
                        {
                            Trace.WriteLine(DateTime.Now + " WARNING - line " + lineCount + ": invalid format: " + (String.IsNullOrEmpty(line) ? "(empty)" : line));
                            continue;
                        }
                        string setnb = split[0];
                        if (setnb.StartsWith("\"") && setnb.EndsWith("\""))
                            setnb = setnb.Substring(1, setnb.Length - 2);

                        AddPairToQueue(setnb, pmid);
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
        /// Add a Setnb/PMID pair to the queue
        /// </summary>
        private void AddPairToQueue(string setnb, int pmid)
        {
            List<int> ids;
            if (!_peopleIds.ContainsKey(setnb))
            {
                ids = new List<int>();
                _peopleIds[setnb] = ids;
                _setnbs.Add(setnb);
            }
            else
            {
                ids = _peopleIds[setnb];
            }
            ids.Add(pmid);
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

                foreach (string setnb in _setnbs)
                {
                    if (_peopleIds.ContainsKey(setnb))
                    {
                        IEnumerable<int> pmids = _peopleIds[setnb];
                        if (pmids == null) break;
                        foreach (int pmid in pmids)
                        {
                            _db.ExecuteNonQuery(
                                "INSERT INTO " + _queueTableName + " (Setnb, PMID) VALUES (?, ?)",
                                new System.Collections.ArrayList() { 
                                            Database.Parameter(setnb), 
                                            Database.Parameter(pmid),
                                        });
                            count++;
                        }
                    }
                }

                Trace.WriteLine(DateTime.Now + " Wrote " + count + " rows to queue table " + _queueTableName);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(DateTime.Now + " Database error after writing " + count + " rows to queue table " + _queueTableName + ": " + ex.Message);
            }
        }

        /// <summary>
        /// Get the next items from the queue
        /// </summary>
        /// <returns>True if a queue item is available, false otherwise</returns>
        public bool Next()
        {
            _currentIndex++;

            if (_setnbs == null || _peopleIds == null)
                return false;

            if (_currentIndex >= _setnbs.Count)
                return false;
            
            CurrentSetnb = _setnbs[_currentIndex];
            if (String.IsNullOrEmpty(CurrentSetnb))
                return false;

            if (!_peopleIds.ContainsKey(CurrentSetnb))
                return false;

            CurrentPmids = _peopleIds[CurrentSetnb];
            if (CurrentPmids == null)
                return false;

            return true;
        }

        /// <summary>
        /// Mark a current setnb/pmid pair as processed in the queue table
        /// </summary>
        public void MarkProcessed(int pmid)
        {
            try
            {
                _db.ExecuteNonQuery(
                    "UPDATE " + _queueTableName + " SET Processed = 1, Error = 0 WHERE Setnb = ? AND PMID = ?",
                    new System.Collections.ArrayList() { 
                    Database.Parameter(CurrentSetnb), 
                    Database.Parameter(pmid),
                });
            }
            catch (Exception ex)
            {
                Trace.WriteLine(DateTime.Now + " Database error marking " + CurrentSetnb + "/" + pmid +" as processed in " + _queueTableName + ": " + ex.Message);
                MarkError(pmid);
            }
        }

        /// <summary>
        /// Mark a current setnb/pmid pair as error in the queue table
        /// </summary>
        public void MarkError(int pmid)
        {
            try
            {
                _db.ExecuteNonQuery(
                    "UPDATE " + _queueTableName + " SET Processed = 0, Error = 1 WHERE Setnb = ? AND PMID = ?",
                    new System.Collections.ArrayList() { 
                    Database.Parameter(CurrentSetnb), 
                    Database.Parameter(pmid),
                });
            }
            catch (Exception ex)
            {
                Trace.WriteLine(DateTime.Now + " Database error marking " + CurrentSetnb + "/" + pmid +" as error in " + _queueTableName + ": " + ex.Message);
            }
        }
    }
}
