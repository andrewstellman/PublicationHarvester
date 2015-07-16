using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDS.RDF;
using VDS.RDF.Ontology;

namespace ExportRdf
{
    class OntologyClass : OntologyMember
    {
        public string SubClassOf { get; private set; }

        public OntologyClass(string uri, string label, string comment)
            : base(uri, label, comment)
        {
            SubClassOf = null;
        }

        public OntologyClass(string uri, string label, string comment, string subClassOf)
            : base(uri, label, comment)
        {
            SubClassOf = subClassOf;
        }

        public override void AssertOntologyTriples(IGraph g)
        {
            g.Assert(new Triple(g.CreateUriNode(Uri), g.CreateUriNode(new Uri(OntologyHelper.PropertyType)), g.CreateUriNode(new Uri(OntologyHelper.OwlClass)), Ontology.ContextUri));
            if (!string.IsNullOrWhiteSpace(SubClassOf))
            {
                g.Assert(new Triple(g.CreateUriNode(Uri), g.CreateUriNode(new Uri(OntologyHelper.PropertySubClassOf)), g.CreateUriNode(SubClassOf), Ontology.ContextUri));
            }
            base.AssertOntologyTriples(g);
        }
    }
}
