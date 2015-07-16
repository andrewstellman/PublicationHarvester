using Com.StellmanGreene.PubMed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDS.RDF;
using VDS.RDF.Ontology;
using VDS.RDF.Parsing;
using VDS.RDF.Writing;

namespace ExportRdf
{
    static class Ontology
    {
        public static readonly Uri ContextUri = new Uri("http://www.stellman-greene.com/PublicationHarvester/Ontology");

        static Ontology()
        {
            AddPrimitives();
        }

        private static readonly List<OntologyMember> _classes =
            new List<OntologyMember>() { 
                new OntologyClass("person:Person", "Person", "Class that represents a person"),
                new OntologyClass("publication:Publication", "Publication", "Class that represents a publication"),
                new OntologyClass("publication:PublicationAuthor", "Publication Author", "Class for a node that holds the author of a publication"),
                new OntologyClass("publication:AuthorPosition", "Author Position", "Class that represents an author position", "ph:Primitive"),
                new OntologyClass("ph:Primitive", "Primitive", "Class that represents a primitive type"),
            };

        private static readonly List<OntologyMember> _properties =
            new List<OntologyMember>() {
                new OntologyProperty("person:last", false, "person:Person", "xsd:string", "Last Name", "Property that holds the last name of a person"),
                new OntologyProperty("person:first", false, "person:Person", "xsd:string", "First Name", "Property that holds the first name of a person"),
                new OntologyProperty("person:setnb", false, "person:Person", "xsd:string", "Setnb", "Property that holds the setnb (ID) of a person"),
                new OntologyProperty("person:authorOf", true, "person:Person", "publication:Publication", "Author Of", "Property that links to a publication authored by a person"),
                new OntologyProperty("person:colleagueOf", true, "person:Person", "publication:Publication", "Colleague Of", "Property that links to a colleague of a person"),
                new OntologyProperty("person:isStar", false, "person:Person", "xsd:boolean", "Is Star", "Property that indicates whether or not a person is a star"),
                new OntologyProperty("publication:journal", false, "publication:Publication", "xsd:string", "Journal", "Property holds the journal of a publication"),
                new OntologyProperty("publication:issue", false, "publication:Publication", "xsd:string", "Issue", "Property holds the issue of a publication"),
                new OntologyProperty("publication:year", false, "publication:Publication", "xsd:integer", "Issue", "Property holds the year of a publication"),
                new OntologyProperty("publication:title", false, "publication:Publication", "xsd:string", "Issue", "Property holds the title of a publication"),
                new OntologyProperty("publication:publicationType", false, "publication:Publication", "publication:PublicationType", "Publication Type", "Property links to the type of a publication"),
                new OntologyProperty("publication:meshHeading", false, "publication:Publication", "xsd:string", "MeSH Heading", "Property holds a MeSH heading for a publication"),
                new OntologyProperty("publication:hasAuthor", false, "publication:Publication", "publication:PublicationAuthor", "Has Author", "Property links to a publication author node"),
                new OntologyProperty("publication:author", false, "publication:PublicationAuthor", "person:Person", "Author", "Property that links to the author of a publication"),
                new OntologyProperty("publication:authorCount", false, "publication:PublicationAuthor", "xsd:integer", "Author Count", "Property that holds the author count"),
                new OntologyProperty("publication:authorPositionType", true, "publication:PublicationAuthor", "publication:AuthorPosition", "Author position", "Property holds the author's position in the author list"),
                new OntologyProperty("publication:authorPosition", true, "publication:PublicationAuthor", "xsd:integer", "Author position type", "Property that links to the author position type"),
            };

        private static readonly List<OntologyMember> _primitives = new List<OntologyMember>();

        /// <summary>
        /// Add the primitives to the ontology
        /// </summary>
        private static void AddPrimitives()
        {
            foreach (Harvester.AuthorPositions authorPosition in Enum.GetValues(typeof(Harvester.AuthorPositions)))
            {
                string authorPositionUri = GetAuthorPositionUri(authorPosition);
                _primitives.Add(new OntologyPrimitive(authorPositionUri, "publication:AuthorPosition", authorPosition.ToString(), "Class that represents the " + authorPosition.ToString() + " author position"));
            }

        }

        /// <summary>
        /// Write a graph containing the ontology to a file
        /// </summary>
        /// <param name="filename">File to write the ontology to</param>
        public static void WriteOntology(string filename)
        {
            using (TripleStore store = new TripleStore())
            using (IGraph g = new Graph())
            {
                AssertOntologyTriples(g);
                g.BaseUri = Ontology.ContextUri;
                store.Add(g);
                TriGWriter writer = new TriGWriter();
                writer.PrettyPrintMode = true;
                writer.Save(store, filename);
            }
        }

        /// <summary>
        /// Get a class node (extension method)
        /// </summary>
        /// <param name="g">Graph to use for creating the node</param>
        /// <param name="uri">URI of the class to get</param>
        /// <returns>URI node for the class if it's known (throws ArgumentException if not found)</returns>
        public static INode GetClassNode(this IGraph g, string uri)
        {
            var ontologyClass = _classes.Find(c => c.Uri == uri);
            if (ontologyClass == null)
                throw new ArgumentException("uri");
            else
                return ontologyClass.CreateNode(g);
        }

        /// <summary>
        /// Get a property node (extension method)
        /// </summary>
        /// <param name="g">Graph to use for creating the node</param>
        /// <param name="uri">URI of the property to get</param>
        /// <returns>URI node for the class if it's known (throws ArgumentException if not found)</returns>
        public static INode GetPropertyNode(this IGraph g, string uri)
        {
            var propertyClass = _properties.Find(c => c.Uri == uri);
            if (propertyClass == null)
                throw new ArgumentException("uri");
            else
                return propertyClass.CreateNode(g);
        }

        /// <summary>
        /// Get a new graph with the prefixes needed for the ontology
        /// </summary>
        /// <returns></returns>
        public static IGraph GetNewGraph()
        {
            IGraph g = new Graph();
            AddPrefixes(g);
            return g;
        }

        /// <summary>
        /// Get the author position Uri for a Harvester.AuthorPositions value
        /// </summary>
        /// <param name="authorPosition">Harvester.AuthorPositions value</param>
        /// <returns>Prefixed URI for the author position</returns>
        public static string GetAuthorPositionUri(Harvester.AuthorPositions authorPosition) {
            return "author-position:" + authorPosition.ToString();
        }

        /// <summary>
        /// Get an author position entity node (extension method)
        /// </summary>
        /// <param name="g">Graph to use for creating the node</param>
        /// <param name="authorPosition">Author position to get</param>
        /// <returns>URI for the author position entity</returns>
        public static INode GetAuthorPositionEntityNode(this IGraph g, Harvester.AuthorPositions authorPosition)
        {
            return g.CreateUriNode(GetAuthorPositionUri(authorPosition));
        }

        /// <summary>
        /// Add the ontology prefixes to a graph
        /// </summary>
        /// <param name="g">Graph to add prefixes to</param>
        private static void AddPrefixes(IGraph g)
        {
            String baseUri = "http://www.stellman-greene.com/";
            g.NamespaceMap.AddNamespace("ph", new Uri(baseUri + "publicationHarvester#"));
            g.NamespaceMap.AddNamespace("person", new Uri(baseUri + "person#"));
            g.NamespaceMap.AddNamespace("publication", new Uri(baseUri + "publication#"));
            g.NamespaceMap.AddNamespace("author-position", new Uri(baseUri + "author-position/"));
            g.NamespaceMap.AddNamespace("xsd", new Uri("http://www.w3.org/2001/XMLSchema#"));
            g.NamespaceMap.AddNamespace("rdf", new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#"));
            g.NamespaceMap.AddNamespace("rdfs", new Uri("http://www.w3.org/2000/01/rdf-schema#"));
            g.NamespaceMap.AddNamespace("owl", new Uri("http://www.w3.org/2002/07/owl#"));
        }


        /// <summary>
        /// Assert the triples for the ontology
        /// </summary>
        /// <param name="g">Graph to assert the triples into</param>
        public static void AssertOntologyTriples(IGraph g)
        {
            AddPrefixes(g);
            var ontologyMembers = _classes.Concat(_properties).Concat(_primitives);
            foreach (var ontologyMember in ontologyMembers)
            {
                ontologyMember.AssertOntologyTriples(g);
            }
        }
    }
}
