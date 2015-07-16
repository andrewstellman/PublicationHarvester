using Com.StellmanGreene.PubMed;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDS.RDF;
using VDS.RDF.Parsing;

namespace ExportRdf
{
    /// <summary>
    /// Class to look up a person (including publications and colleagues) in the database and add triples to a graph
    /// </summary>
    class PersonGraphUpdater
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private readonly Database _db;

        public PersonGraphUpdater(Database DB)
        {
            _db = DB;
        }

        public IGraph AddPersonToGraph(IGraph g, Person person)
        {
            IUriNode personNode;
            personNode = g.CreateUriNode(new Uri("http://www.stellman-greene.com/person/" + person.Setnb));

            g.Assert(new Triple(personNode, GraphHelper.RdfType, GraphHelper.PersonClass));
            g.Assert(new Triple(personNode, GraphHelper.Last, g.CreateLiteralNode(person.Last)));
            g.Assert(new Triple(personNode, GraphHelper.First, g.CreateLiteralNode(person.First)));
            g.Assert(new Triple(personNode, GraphHelper.Setnb, g.CreateLiteralNode(person.Setnb)));

            Publications publications = new Publications(_db, person, false);
            foreach (Publication pub in publications.PublicationList)
            {
                AddPublicationAssertions(g, pub);
            }

            AddColleagues(g, person);

            return g;
        }

        private void AddColleagues(IGraph g, Person person)
        {
            DataTable colleagues = _db.ExecuteQuery(String.Format("SELECT StarSetnb, Setnb FROM StarColleagues WHERE Setnb = '{0}' OR StarSetnb = '{0}'", person.Setnb));
            foreach (DataRow dataRow in colleagues.Rows)
            {
                var starUri = new Uri("http://www.stellman-greene.com/person/" + dataRow["Setnb"]);
                var colleagueUri = new Uri("http://www.stellman-greene.com/person/" + dataRow["StarSetnb"]);
                g.Assert(new Triple(g.CreateUriNode(starUri), GraphHelper.ColleagueOf, g.CreateUriNode(colleagueUri)));
                g.Assert(new Triple(g.CreateUriNode(colleagueUri), GraphHelper.ColleagueOf, g.CreateUriNode(starUri)));
                g.Assert(new Triple(g.CreateUriNode(starUri), GraphHelper.IsStar, g.CreateLiteralNode("true", new Uri(XmlSpecsHelper.XmlSchemaDataTypeBoolean))));
            }
        }

        private void AddPublicationAssertions(IGraph g, Publication pub)
        {
            if (!PreviouslyAddedChecker.CheckPublication(pub.PMID))
            {
                PreviouslyAddedChecker.AddPublication(pub.PMID);
                IUriNode publicationNode = g.CreateUriNode(new Uri("http://www.stellman-greene.com/publication/" + pub.PMID));
                g.Assert(new Triple(publicationNode, GraphHelper.RdfType, GraphHelper.PublicationClass));
                g.Assert(new Triple(publicationNode, GraphHelper.Year, g.CreateLiteralNode(pub.Year.ToString(), new Uri(XmlSpecsHelper.XmlSchemaDataTypeInteger))));
                if (pub.Journal != null)
                    g.Assert(new Triple(publicationNode, GraphHelper.Journal, g.CreateLiteralNode(pub.Journal)));
                if (pub.Issue != null)
                    g.Assert(new Triple(publicationNode, GraphHelper.Issue, g.CreateLiteralNode(pub.Issue)));
                if (pub.Title != null)
                    g.Assert(new Triple(publicationNode, GraphHelper.Title, g.CreateLiteralNode(pub.Title)));
                if (pub.PubType != null)
                    g.Assert(new Triple(publicationNode, GraphHelper.PublicationType, g.CreateLiteralNode(pub.PubType)));
                if (pub.Authors != null)
                    g.Assert(new Triple(publicationNode, GraphHelper.AuthorCount, g.CreateLiteralNode(pub.Authors.Length.ToString(), new Uri(XmlSpecsHelper.XmlSchemaDataTypeInteger))));

                var data = _db.ExecuteQuery("SELECT Setnb, AuthorPosition, PositionType FROM PeoplePublications WHERE PMID = " + pub.PMID);
                foreach (DataRow row in data.Rows)
                {
                    var authorNode = g.CreateUriNode(new Uri("http://www.stellman-greene.com/person/" + row.Field<String>("Setnb")));
                    g.Assert(authorNode, GraphHelper.AuthorOf, publicationNode);

                    var publicationAuthorNode = g.CreateBlankNode();
                    g.Assert(publicationAuthorNode, GraphHelper.RdfType, GraphHelper.PublicationAuthorClass);
                    g.Assert(publicationNode, GraphHelper.HasAuthor, publicationAuthorNode);
                    g.Assert(publicationAuthorNode, GraphHelper.Author, authorNode);

                    var authorPosition = row["AuthorPosition"];
                    if (authorPosition != null && authorPosition is System.Int32)
                        g.Assert(publicationAuthorNode, GraphHelper.AuthorPosition, g.CreateLiteralNode(authorPosition.ToString(), new Uri(XmlSpecsHelper.XmlSchemaDataTypeInteger)));

                    var positionType = row["PositionType"];
                    if (positionType != null && positionType is System.Int16 && Enum.IsDefined(typeof(Harvester.AuthorPositions), Convert.ToInt32(positionType)))
                        g.Assert(publicationAuthorNode, GraphHelper.AuthorPosition, GraphHelper.GetAuthorPositionNode(g, (Harvester.AuthorPositions)Convert.ToInt32(positionType)));

                }
            }
        }
    }
}
