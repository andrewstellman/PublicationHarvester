using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDS.RDF;
using VDS.RDF.Ontology;

namespace ExportRdf
{
    class OntologyProperty : OntologyMember
    {
        public bool IsObjectProperty { get; private set; }
        public string DomainUri { get; private set; }
        public string RangeUri { get; private set; }

        public OntologyProperty(string uri, bool isObjectProperty, string domainUri, string rangeUri, string label, string comment)
            : base(uri, label, comment)
        {
            IsObjectProperty = isObjectProperty;
            DomainUri = domainUri;
            RangeUri = rangeUri;
        }

        public override void AssertOntologyTriples(IGraph g)
        {
            if (IsObjectProperty)
                g.Assert(new Triple(g.CreateUriNode(Uri), g.CreateUriNode(new Uri(OntologyHelper.PropertyType)), g.CreateUriNode(new Uri(OntologyHelper.OwlObjectProperty)), Ontology.ContextUri));
            else
                g.Assert(new Triple(g.CreateUriNode(Uri), g.CreateUriNode(new Uri(OntologyHelper.PropertyType)), g.CreateUriNode(new Uri(OntologyHelper.OwlDatatypeProperty)), Ontology.ContextUri));
            g.Assert(new Triple(g.CreateUriNode(Uri), g.CreateUriNode(new Uri(OntologyHelper.PropertyDomain)), g.CreateUriNode(DomainUri), Ontology.ContextUri));
            g.Assert(new Triple(g.CreateUriNode(Uri), g.CreateUriNode(new Uri(OntologyHelper.PropertyRange)), g.CreateUriNode(RangeUri), Ontology.ContextUri));
            base.AssertOntologyTriples(g);
        }
    }
}
