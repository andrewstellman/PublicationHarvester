using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDS.RDF;
using VDS.RDF.Ontology;

namespace ExportRdf
{
    class OntologyPrimitive : OntologyMember
    {
        public string PrimitiveType { get; private set; }

        public OntologyPrimitive(string uri, string primitiveType, string label, string comment) : base(uri, label, comment) 
        {
            PrimitiveType = primitiveType;
        }

        public override void AssertOntologyTriples(IGraph g)
        {
            var uriNode = g.CreateUriNode(Uri);
            g.Assert(new Triple(uriNode, g.CreateUriNode(new Uri(OntologyHelper.PropertyType)), g.CreateUriNode(PrimitiveType), Ontology.ContextUri));
            base.AssertOntologyTriples(g);
        }
    }
}
