using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDS.RDF;
using VDS.RDF.Ontology;

namespace ExportRdf
{
    abstract class OntologyMember
    {
        public string Uri { get; private set; }
        public string Label { get; private set; }
        public string Comment { get; private set; }

        public OntologyMember(string uri, string label, string comment)
        {
            Uri = uri;
            Label = label;
            Comment = comment;
        }

        public INode CreateNode(IGraph g)
        {
            return g.CreateUriNode(Uri);
        }

        public virtual void AssertOntologyTriples(IGraph g)
        {
            g.Assert(new Triple(g.CreateUriNode(Uri), g.CreateUriNode(new Uri(OntologyHelper.PropertyLabel)), g.CreateLiteralNode(Label), Ontology.ContextUri));
            g.Assert(new Triple(g.CreateUriNode(Uri), g.CreateUriNode(new Uri(OntologyHelper.PropertyComment)), g.CreateLiteralNode(Comment), Ontology.ContextUri));
        }
    }
}
