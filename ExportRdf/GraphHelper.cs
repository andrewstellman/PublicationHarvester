using Com.StellmanGreene.PubMed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDS.RDF;

namespace ExportRdf
{
    static class GraphHelper
    {
        public static IUriNode RdfType { get; private set; }

        public static IUriNode PersonClass { get; private set; }
        public static IUriNode Last { get; private set; }
        public static IUriNode First { get; private set; }
        public static IUriNode Setnb { get; private set; }
        public static IUriNode AuthorOf { get; private set; }
        public static IUriNode ColleagueOf { get; private set; }
        public static IUriNode IsStar { get; private set; }

        public static IUriNode PublicationClass { get; private set; }
        public static IUriNode PublicationAuthorClass { get; private set; }
        public static IUriNode Journal { get; private set; }
        public static IUriNode Issue { get; private set; }
        public static IUriNode Year { get; private set; }
        public static IUriNode Title { get; private set; }
        public static IUriNode PublicationType { get; private set; }
        public static IUriNode HasAuthor { get; private set; }
        public static IUriNode Author { get; private set; }
        public static IUriNode AuthorCount { get; private set; }
        public static IUriNode AuthorPosition { get; private set; }

        public static IGraph GetNewGraph()
        {
            IGraph g = new Graph();

            AddPrefixes(g);

            RdfType = g.CreateUriNode("rdf:type");

            PersonClass = g.CreateUriNode("person:Person");
            Last = g.CreateUriNode("person:last");
            First = g.CreateUriNode("person:first");
            Setnb = g.CreateUriNode("person:setnb");
            AuthorOf = g.CreateUriNode("person:authorOf");
            ColleagueOf = g.CreateUriNode("person:colleagueOf");
            IsStar = g.CreateUriNode("person:isStar");

            PublicationClass = g.CreateUriNode("publication:Publication");
            PublicationAuthorClass = g.CreateUriNode("publication:PublicationAuthor");
            Journal = g.CreateUriNode("publication:journal");
            Issue = g.CreateUriNode("publication:issue");
            Year = g.CreateUriNode("publication:year");
            Title = g.CreateUriNode("publication:title");
            PublicationType = g.CreateUriNode("publication:publicationType");
            HasAuthor = g.CreateUriNode("publication:hasAuthor");
            Author = g.CreateUriNode("publication:author");
            AuthorCount = g.CreateUriNode("publication:authorCount");
            AuthorPosition = g.CreateUriNode("publication:authorPosition");

            return g;
        }

        public static IUriNode GetAuthorPositionNode(IGraph g, Harvester.AuthorPositions authorPosition) {
            return g.CreateUriNode(new Uri("http://www.stellman-greene.com/author-positions/" + authorPosition.ToString().ToLower()));
        }

        private static void AddPrefixes(IGraph g)
        {
            String baseUri = "http://www.stellman-greene.com/";
            g.BaseUri = new Uri(baseUri);
            g.NamespaceMap.AddNamespace("person", new Uri(baseUri + "person#"));
            g.NamespaceMap.AddNamespace("publication", new Uri(baseUri + "publication#"));
            //g.NamespaceMap.AddNamespace("author-positions", new Uri(baseUri + "authorPositions/"));
            //g.NamespaceMap.AddNamespace("people", new Uri(baseUri + "person/"));
            //g.NamespaceMap.AddNamespace("publications", new Uri(baseUri + "publication/"));
        }
    }
}
