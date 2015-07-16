using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDS.RDF;
using VDS.RDF.Query;
using VDS.RDF.Storage;

namespace ExportRdf
{
    static class PreviouslyAddedChecker
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private static readonly IDictionary<string, bool> _peopleAdded = new Dictionary<string, bool>();
        private static readonly IDictionary<int, bool> _publicationsAdded = new Dictionary<int, bool>();

        /// <summary>
        /// Add a publication
        /// </summary>
        /// <param name="pmid">PMID of the publication to add</param>
        public static void AddPublication(int pmid)
        {
            _publicationsAdded[pmid] = true;
        }

        /// <summary>
        /// Check if a publication has been added
        /// </summary>
        /// <param name="pmid">PMID of the publication to check</param>
        /// <returns>True if the publication has been added</returns>
        public static bool CheckPublication(int pmid)
        {
            return _publicationsAdded.ContainsKey(pmid);
        }

        /// <summary>
        /// Check if a person has been addded
        /// </summary>
        /// <param name="setnb">Setnb of the person to check</param>
        /// <returns>True if person has been added, false otherwise</returns>
        public static bool CheckPerson(string setnb)
        {
            return _peopleAdded.ContainsKey(setnb);
        }

        /// <summary>
        /// Add a person
        /// </summary>
        /// <param name="setnb">Setnb of the person to add</param>
        public static void AddPerson(string setnb)
        {
            _peopleAdded[setnb] = true;
        }


        private static int _publicationsSkipped = 0;

        /// <summary>
        /// Number of people skipped so far (including people read from storage when the program started)
        /// </summary>
        public static int _PeopleSkipped { get { return _peopleSkipped; } }
        private static int _peopleSkipped = 0;

        /// <summary>
        /// Number of people added so far (including people read from storage when the program started)
        /// </summary>
        public static int PeopleAdded { get { return _peopleAdded.Count; } }

        /// <summary>
        /// Query for people and publications who have already been added to the storage
        /// </summary>
        /// <param name="storage">Storage to query</param>
        public static void SkipAddedPeopleAndPublications(IQueryableStorage storage)
        {
            SkipAddedPeople(storage);
            SkipAddedPublications(storage);
        }

        /// <summary>
        /// Query the storage to skip the people and publications that have already been added
        /// </summary>
        private static void SkipAddedPeople(IQueryableStorage storage)
        {
            var peopleQuery = @"BASE <http://www.stellman-greene.com>
PREFIX person: <person#>
SELECT * {
  ?person a person:Person .
}";
            SparqlResultSet peopleResults = storage.Query(peopleQuery) as SparqlResultSet;
            foreach (var result in peopleResults.Results)
            {
                IUriNode personUri = result["person"] as IUriNode;
                string setnb = personUri.Uri.LocalPath.Replace("/person/", "");
                _peopleAdded[setnb] = true;
            }
            _peopleSkipped = _peopleAdded.Count;
            if (_peopleSkipped > 0)
                logger.Info("Skipped " + _peopleSkipped + " " + (_peopleSkipped == 1 ? "person" : "people") + " previously processed");
        }

        private static void SkipAddedPublications(IQueryableStorage storage)
        {
            var publicationQuery = @"BASE <http://www.stellman-greene.com>
PREFIX publication: <publication#>
SELECT * {
  ?publication a publication:Publication .
}";
            SparqlResultSet publicationResults = storage.Query(publicationQuery) as SparqlResultSet;
            foreach (var result in publicationResults.Results)
            {
                IUriNode publicationUri = result["publication"] as IUriNode;
                string pmidString = publicationUri.Uri.LocalPath.Replace("/publication/", "");
                int pmid;
                if (int.TryParse(pmidString, out pmid))
                    _publicationsAdded[pmid] = true;
            }
            _publicationsSkipped = _publicationsAdded.Count;
            if (_publicationsSkipped > 0)
                logger.Info("Skipped " + _publicationsSkipped + " publication" + (_publicationsSkipped == 1 ? "" : "s") + " previously processed");
        }
    }
}
