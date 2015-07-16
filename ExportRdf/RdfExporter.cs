using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using System.Linq;
using Com.StellmanGreene.PubMed;
using VDS.RDF;
using VDS.RDF.Writing;
using VDS.RDF.Parsing;
using NLog;
using VDS.RDF.Storage;
using VDS.RDF.Query;

namespace ExportRdf
{
    /// <summary>
    /// Object to find the colleagues for a star
    /// </summary>
    class RdfExporter
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private readonly Database _db;
        private readonly IQueryableStorage _storage;
        private readonly PersonGraphUpdater _personGraphCreator;
        private readonly int _peoplePerWrite;

        public RdfExporter(Database DB, int peoplePerWrite, IQueryableStorage storage)
        {
            _db = DB;
            _personGraphCreator = new PersonGraphUpdater(DB);
            _peoplePerWrite = peoplePerWrite;
            _storage = storage;
        }

        public RdfExporter(Database DB, int peoplePerWrite)
        {
            _db = DB;
            _personGraphCreator = new PersonGraphUpdater(DB);
            _peoplePerWrite = peoplePerWrite;
            _storage = null;
        }

        int peopleAddedThisRun = 0;

        /// <summary>
        /// Read the Publication Harvester database and export RDF
        /// </summary>
        public void ExportRdf()
        {
            if (_storage != null)
            {
                PreviouslyAddedChecker.SkipAddedPeopleAndPublications(_storage);
            }
            
            DateTime startTime = DateTime.Now;

            IGraph g = GraphHelper.GetNewGraph();

            People people = new People(_db);
            int total = people.PersonList.Count - PreviouslyAddedChecker._PeopleSkipped;
            foreach (Person person in people.PersonList)
            {
                if (!PreviouslyAddedChecker.CheckPerson(person.Setnb))
                {
                    PreviouslyAddedChecker.AddPerson(person.Setnb);
                    peopleAddedThisRun++;

                    logger.Info(String.Format("Processing #{0} of {1}: {2}", peopleAddedThisRun, total, person.ToString()));

                    _personGraphCreator.AddPersonToGraph(g, person);
                    if (peopleAddedThisRun % _peoplePerWrite == 0)
                    {
                        PersonGraphWriter.Write(g);
                        g.Dispose();
                        g = GraphHelper.GetNewGraph();                        
                    }

                    if ((PreviouslyAddedChecker.PeopleAdded - PreviouslyAddedChecker._PeopleSkipped) % 10 == 0)
                        LogTiming(startTime, total);
                }
            }

            g.Dispose();

            logger.Info("Finished writing RDF to {0}", PersonGraphWriter.Filename);
            LogTiming(startTime, total);
        }

        /// <summary>
        /// Log the timing status of the current run
        /// </summary>
        /// <param name="startTime">When the run started</param>
        /// <param name="totalPeopleExportedThisRun">Total number of people exported this run</param>
        private void LogTiming(DateTime startTime, int totalPeopleExportedThisRun)
        {
            var totalSeconds = (DateTime.Now - startTime).TotalSeconds;
            var perPerson = totalSeconds / peopleAddedThisRun;
            var timeLeft = perPerson * (totalPeopleExportedThisRun - PreviouslyAddedChecker.PeopleAdded);
            logger.Info(String.Format("Elapsed time {0:0.00}sec ({1:0.00}sec/person), estimated time left {2:0.00}min", totalSeconds, perPerson, timeLeft / 60));
        }



    }
}
