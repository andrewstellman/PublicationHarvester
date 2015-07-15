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

        private readonly Database DB;
        private readonly IStorageProvider storage;
        private readonly PersonGraphCreator personGraphCreator;
        private readonly PersonGraphWriter personGraphWriter = new PersonGraphWriter();
        private readonly int peoplePerWrite;

        /// <summary>
        /// Query the Publication Harvester database and export RDF
        /// </summary>
        /// <param name="DB">Database to query for people</param>
        /// <param name="storage">Optional storage to query for people and publications to skip</param>
        public RdfExporter(Database DB, int peoplePerWrite, IQueryableStorage storage = null)
        {
            this.DB = DB;
            this.storage = storage;
            this.personGraphCreator = new PersonGraphCreator(DB);
            this.peoplePerWrite = peoplePerWrite;
        }

        int publicationsSkipped = 0;

        int peopleSkipped = 0;

        int peopleAddedThisRun = 0;

        /// <summary>
        /// Read the Publication Harvester database and export RDF
        /// </summary>
        public void ExportRdf()
        {
            if (storage != null)
                SkipAddedPeopleAndPublications();
            
            logger.Info("Started exporting RDF");
            DateTime startTime = DateTime.Now;

            IGraph g = GraphHelper.GetNewGraph();

            People people = new People(DB);
            int total = people.PersonList.Count - peopleSkipped;
            foreach (Person person in people.PersonList)
            {
                if (!personGraphCreator.peopleAdded.ContainsKey(person.Setnb))
                {
                    personGraphCreator.peopleAdded[person.Setnb] = true;
                    peopleAddedThisRun++;

                    logger.Info(String.Format("Processing #{0} of {1}: {2}", peopleAddedThisRun, total, person.ToString()));

                    personGraphCreator.AssertTriplesForPerson(g, person);
                    if (peopleAddedThisRun % peoplePerWrite == 0)
                    {
                        personGraphWriter.Write(g);
                        g.Dispose();
                        g = GraphHelper.GetNewGraph();                        
                    }

                    if ((personGraphCreator.peopleAdded.Count - peopleSkipped) % 10 == 0)
                        LogTiming(startTime, total);
                }
            }

            g.Dispose();

            logger.Info("Finished exporting RDF");
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
            var timeLeft = perPerson * (totalPeopleExportedThisRun - personGraphCreator.peopleAdded.Count);
            logger.Info(String.Format("Elapsed time {0:0.00}sec ({1:0.00}sec/person), estimated time left {2:0.00}min", totalSeconds, perPerson, timeLeft / 60));
        }

        /// <summary>
        /// Query the storage to skip the people and publications that have already been added
        /// </summary>
        private void SkipAddedPeopleAndPublications()
        {
            IQueryableStorage queryableStorage = storage as IQueryableStorage;

            var peopleQuery = @"BASE <http://www.stellman-greene.com>
PREFIX person: <person#>
SELECT * {
  ?person a person:Person .
}";
            SparqlResultSet peopleResults = queryableStorage.Query(peopleQuery) as SparqlResultSet;
            foreach (var result in peopleResults.Results)
            {
                IUriNode personUri = result["person"] as IUriNode;
                string setnb = personUri.Uri.LocalPath;
                personGraphCreator.peopleAdded[setnb] = true;
            }
            peopleSkipped = personGraphCreator.peopleAdded.Count;
            if (peopleSkipped > 0)
                logger.Info("Skipped " + peopleSkipped + " " + (peopleSkipped == 1 ? "person" : "people") + " previously processed");

            var publicationQuery = @"BASE <http://www.stellman-greene.com>
PREFIX publication: <publication#>
SELECT * {
  ?publication a publication:Publication .
}";
            SparqlResultSet publicationResults = queryableStorage.Query(publicationQuery) as SparqlResultSet;
            foreach (var result in publicationResults.Results)
            {
                IUriNode publicationUri = result["publication"] as IUriNode;
                string pmidString = publicationUri.Uri.LocalPath;
                int pmid;
                if (int.TryParse(pmidString, out pmid))
                    personGraphCreator.publicationsAdded[pmid] = true;
            }
            publicationsSkipped = personGraphCreator.publicationsAdded.Count;
            if (publicationsSkipped > 0)
                logger.Info("Skipped " + publicationsSkipped + " publication" + (publicationsSkipped == 1 ? "" : "s") + " previously processed");
        }

    }
}
