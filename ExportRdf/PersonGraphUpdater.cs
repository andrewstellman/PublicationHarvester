using Com.StellmanGreene.PubMed;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDS.RDF;
using VDS.RDF.Ontology;
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

            g.Assert(new Triple(personNode, g.CreateUriNode(new Uri(OntologyHelper.PropertyType)), g.GetClassNode("person:Person")));
            g.Assert(new Triple(personNode, g.GetPropertyNode("person:first"), g.CreateLiteralNode(person.Last)));
            g.Assert(new Triple(personNode, g.GetPropertyNode("person:last"), g.CreateLiteralNode(person.First)));
            g.Assert(new Triple(personNode, g.GetPropertyNode("person:setnb"), g.CreateLiteralNode(person.Setnb)));

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
                g.Assert(new Triple(g.CreateUriNode(starUri), g.GetPropertyNode("person:colleagueOf"), g.CreateUriNode(colleagueUri)));
                g.Assert(new Triple(g.CreateUriNode(colleagueUri), g.GetPropertyNode("person:colleagueOf"), g.CreateUriNode(starUri)));
                g.Assert(new Triple(g.CreateUriNode(starUri), g.GetPropertyNode("person:isStar"), g.CreateLiteralNode("true", new Uri(XmlSpecsHelper.XmlSchemaDataTypeBoolean))));
            }
        }

        private void AddPublicationAssertions(IGraph g, Publication pub)
        {
            if (!PreviouslyAddedChecker.CheckPublication(pub.PMID))
            {
                PreviouslyAddedChecker.AddPublication(pub.PMID);
                IUriNode publicationNode = g.CreateUriNode(new Uri("http://www.stellman-greene.com/publication/" + pub.PMID));
                g.Assert(new Triple(publicationNode, g.CreateUriNode(new Uri(OntologyHelper.PropertyType)), g.GetClassNode("publication:Publication")));
                g.Assert(new Triple(publicationNode, g.GetPropertyNode("publication:year"), g.CreateLiteralNode(pub.Year.ToString(), new Uri(XmlSpecsHelper.XmlSchemaDataTypeInteger))));
                if (pub.Journal != null)
                    g.Assert(new Triple(publicationNode, g.GetPropertyNode("publication:journal"), g.CreateLiteralNode(pub.Journal)));
                if (pub.Issue != null)
                    g.Assert(new Triple(publicationNode, g.GetPropertyNode("publication:issue"), g.CreateLiteralNode(pub.Issue)));
                if (pub.Title != null)
                    g.Assert(new Triple(publicationNode, g.GetPropertyNode("publication:title"), g.CreateLiteralNode(pub.Title)));
                if (pub.PubType != null)
                    g.Assert(new Triple(publicationNode, g.GetPropertyNode("publication:publicationType"), g.CreateLiteralNode(pub.PubType)));
                if (pub.Authors != null)
                    g.Assert(new Triple(publicationNode, g.GetPropertyNode("publication:authorCount"), g.CreateLiteralNode(pub.Authors.Length.ToString(), new Uri(XmlSpecsHelper.XmlSchemaDataTypeInteger))));

                var data = _db.ExecuteQuery("SELECT Setnb, AuthorPosition, PositionType FROM PeoplePublications WHERE PMID = " + pub.PMID);
                foreach (DataRow row in data.Rows)
                {
                    var authorNode = g.CreateUriNode(new Uri("http://www.stellman-greene.com/person/" + row.Field<String>("Setnb")));
                    g.Assert(authorNode, g.GetPropertyNode("person:authorOf"), publicationNode);

                    var publicationAuthorNode = g.CreateBlankNode();
                    g.Assert(publicationAuthorNode, g.CreateUriNode(new Uri(OntologyHelper.PropertyType)), g.GetClassNode("publication:PublicationAuthor"));
                    g.Assert(publicationNode, g.GetPropertyNode("publication:hasAuthor"), publicationAuthorNode);
                    g.Assert(publicationAuthorNode, g.GetPropertyNode("publication:author"), authorNode);

                    var authorPosition = row["AuthorPosition"];
                    if (authorPosition != null && authorPosition is System.Int32)
                        g.Assert(publicationAuthorNode, g.GetPropertyNode("publication:authorPosition"), g.CreateLiteralNode(authorPosition.ToString(), new Uri(XmlSpecsHelper.XmlSchemaDataTypeInteger)));

                    var positionType = row["PositionType"];
                    if (positionType != null && positionType is System.Int16 && Enum.IsDefined(typeof(Harvester.AuthorPositions), Convert.ToInt32(positionType)))
                        g.Assert(publicationAuthorNode, g.GetPropertyNode("publication:authorPositionType"), g.GetAuthorPositionEntityNode((Harvester.AuthorPositions)Convert.ToInt32(positionType)));

                }
            }
        }
    }
}
