using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

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

        public InputQueue(FileInfo inputFile)
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
    }
}
