using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VDS.RDF;
using ExportRdf;
using VDS.RDF.Query;
using System.Collections.Generic;
using System.Linq;

namespace ExportRdfTests
{
    [TestClass]
    public class OntologyTest
    {
        [TestMethod]
        public void TestOntologyTripleAssertion()
        {
            using (IGraph g = new Graph())
            {
                Ontology.AssertOntologyTriples(g);

                Assert.IsTrue(((SparqlResultSet)g.ExecuteQuery("SELECT * { ?s ?p ?o }")).Results.Count > 100);

                var personValues = ((SparqlResultSet)g.ExecuteQuery(
                    "SELECT * { <http://www.stellman-greene.com/person#Person> ?p ?o } ORDER BY STR(?p)"
                    )).Results
                    .Select(r => new string[] { r["p"].ToString(), r["o"].ToString() });
                Assert.AreEqual(personValues.Count(), 3);
                Assert.IsTrue(personValues.ElementAt(0).SequenceEqual(new string[] { "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", "http://www.w3.org/2002/07/owl#Class" }));
                Assert.IsTrue(personValues.ElementAt(1).SequenceEqual(new string[] { "http://www.w3.org/2000/01/rdf-schema#comment", "Class that represents a person" }));
                Assert.IsTrue(personValues.ElementAt(2).SequenceEqual(new string[] { "http://www.w3.org/2000/01/rdf-schema#label", "Person" }));

                var isStarValues = ((SparqlResultSet)g.ExecuteQuery(
                    "SELECT * { <http://www.stellman-greene.com/person#isStar> ?p ?o } ORDER BY STR(?p)"
                    )).Results
                    .Select(r => new string[] { r["p"].ToString(), r["o"].ToString() });
                Assert.AreEqual(isStarValues.Count(), 5);
                Assert.IsTrue(isStarValues.ElementAt(0).SequenceEqual(new string[] { "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", "http://www.w3.org/2002/07/owl#DatatypeProperty" }));
                Assert.IsTrue(isStarValues.ElementAt(1).SequenceEqual(new string[] { "http://www.w3.org/2000/01/rdf-schema#comment", "Property that indicates whether or not a person is a star" }));
                Assert.IsTrue(isStarValues.ElementAt(2).SequenceEqual(new string[] { "http://www.w3.org/2000/01/rdf-schema#domain", "http://www.stellman-greene.com/person#Person" }));
                Assert.IsTrue(isStarValues.ElementAt(3).SequenceEqual(new string[] { "http://www.w3.org/2000/01/rdf-schema#label", "Is Star" }));
                Assert.IsTrue(isStarValues.ElementAt(4).SequenceEqual(new string[] { "http://www.w3.org/2000/01/rdf-schema#range", "http://www.w3.org/2001/XMLSchema#boolean" }));
            }
        }
    }
}
