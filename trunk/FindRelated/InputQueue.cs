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
         * Add error reporting
         * Add queue clear/restart
         * Add UI
         */ 

        public string CurrentSetnb { get; private set; }

        public IEnumerable<int> CurrentPmids { get; private set; }

        public int Count { get; private set; }

        private readonly Dictionary<string, List<int>> _peopleIds = new Dictionary<string, List<int>>();

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
                        }
                        else
                            ids = _peopleIds[setnb];
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

        public bool Next()
        {
            return false;
        }
    }
}
