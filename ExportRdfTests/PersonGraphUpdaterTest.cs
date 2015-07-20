using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Com.StellmanGreene.PubMed;
using VDS.RDF;
using ExportRdf;
using VDS.RDF.Query;
using System.Collections.Generic;
using System.Linq;

namespace ExportRdfTests
{
    [TestClass]
    public class PersonGraphUpdaterTest
    {
        static PersonGraphUpdater _updater = null;
        static Database _db = null;
        static People _people = null;
        static Person _keith = null;
        static List<string[]> _keithTriples = null;

        /// <summary>
        /// Initialize the test database with test data, get all triple values for person Keith Reemtsa into _keithTriples
        /// </summary>
        [ClassInitialize]
        public static void ClassSetup(TestContext testContext)
        {
            // This requires an ODBC data source named "Publication Harvester Unit Test" that can be cleared
            new Com.StellmanGreene.PubMed.Unit_Tests.TestPeopleMaintenance().ResetDatabase();
            _db = new Database("Publication Harvester Unit Test");
            SCGen.ColleagueFinder.CreateTables(_db);
            _db.ExecuteNonQuery(@"
INSERT INTO StarColleagues (StarSetnb, Setnb)
VALUES 
('A5501586', 'A5401532'),
('A5501586', 'X0000001'),
('A5501586', 'X0000002'),
('A7809652', 'X0000003');
");
            
            // Read the input file input1.xls
            People PeopleFromFile = new People(
                AppDomain.CurrentDomain.BaseDirectory + "\\Unit Tests\\TestPeople",
                "input1.xls");

            // Initialize the person to test
            _people = new People(_db);
            _keith = _people.PersonList.Find(p => p.Setnb == "A5501586");
            Assert.AreEqual("Reemtsma, Keith (A5501586)", _keith.ToString());

            // Get the graph for Keith Reemtsma
            _updater = new PersonGraphUpdater(_db);
            using (IGraph g = new Graph())
            {
                Ontology.AssertOntologyTriples(g);

                _updater.AddPersonToGraph(g, _keith);
                _keithTriples = ((SparqlResultSet)g.ExecuteQuery("SELECT * { ?s ?p ?o }")).Results
                    .Select(r => new string[] { r["s"].ToString(), r["p"].ToString(), r["o"].ToString() })
                    .ToList<string[]>();
            }
        }

        [TestMethod]
        public void TestPersonTriples()
        {
            string first = _keithTriples.Where(r => r[1] == "http://www.stellman-greene.com/person#first").First()[2];
            Assert.AreEqual("Keith", first);

            string last = _keithTriples.Where(r => r[1] == "http://www.stellman-greene.com/person#last").First()[2];
            Assert.AreEqual("Reemtsma", last);

            string setnb = _keithTriples.Where(r => r[1] == "http://www.stellman-greene.com/person#setnb").First()[2];
            Assert.AreEqual("A5501586", setnb);
        }

        [TestMethod]
        public void TestPublicationTriples()
        {
            Console.WriteLine(_keithTriples.Count());
            _keithTriples.ForEach(r => Console.WriteLine(r[0] + " ! " + r[1] + " ! " + r[2]));

            var publicationTriples = _keithTriples.Where(r => r[0] == "http://www.stellman-greene.com/publication/682412");

            Console.WriteLine(publicationTriples.Count());
            Console.WriteLine(publicationTriples.Where(r => r[1] == "http://www.w3.org/1999/02/22-rdf-syntax-ns#type").First()[2]);

            var nsType = publicationTriples.Where(r => r[1] == "http://www.w3.org/1999/02/22-rdf-syntax-ns#type").First()[2];
            Assert.AreEqual("http://www.stellman-greene.com/publication#Publication", nsType);

            var year = publicationTriples.Where(r => r[1] == "http://www.stellman-greene.com/publication#year").First()[2];
            Assert.AreEqual("1978^^http://www.w3.org/2001/XMLSchema#integer", year);

            var journal = publicationTriples.Where(r => r[1] == "http://www.stellman-greene.com/publication#journal").First()[2];
            Assert.AreEqual("Kardiologiia", journal);

            var issue = publicationTriples.Where(r => r[1] == "http://www.stellman-greene.com/publication#issue").First()[2];
            Assert.AreEqual("7", issue);

            var publicationType = publicationTriples.Where(r => r[1] == "http://www.stellman-greene.com/publication#publicationType").First()[2];
            Assert.AreEqual("Journal Article", publicationType);

            var title = publicationTriples.Where(r => r[1] == "http://www.stellman-greene.com/publication#title").First()[2];
            Assert.AreEqual("[Counterpulsation by means of a new device with a pulsatile blood flow in open heart operations (authors transl)]", title);

            List<string> meshHeadings = _keithTriples.Where(r => r[0] == "http://www.stellman-greene.com/publication/682412" && r[1] == "http://www.stellman-greene.com/publication#meshHeading").Select(r => r[2]).ToList();
            Assert.AreEqual(13, meshHeadings.Count);
            CollectionAssert.Contains(meshHeadings, "Animals");
            CollectionAssert.Contains(meshHeadings, "Intra-Aortic Balloon Pumping/adverse effects/*instrumentation");

            List<string> authorOf = _keithTriples.Where(r => r[1] == "http://www.stellman-greene.com/person#authorOf").Select(r => r[2]).ToList();
            CollectionAssert.Contains(authorOf, "http://www.stellman-greene.com/publication/682412");
            CollectionAssert.Contains(authorOf, "http://www.stellman-greene.com/publication/10961290");
            CollectionAssert.Contains(authorOf, "http://www.stellman-greene.com/publication/11528018");
            CollectionAssert.Contains(authorOf, "http://www.stellman-greene.com/publication/11661702");
        }

        /// <summary>
        /// Verify that colleague triples are generated
        /// </summary>
        [TestMethod]
        public void TestColleagueTriples()
        {
            List<string> colleagues = _keithTriples
                .Where(r => r[0] == "http://www.stellman-greene.com/person/A5501586" && r[1] == "http://www.stellman-greene.com/person#colleagueOf")
                .Select(r => r[2])
                .ToList();

            Assert.AreEqual(3, colleagues.Count());
            CollectionAssert.Contains(colleagues, "http://www.stellman-greene.com/person/A5401532");
            CollectionAssert.Contains(colleagues, "http://www.stellman-greene.com/person/X0000001");
            CollectionAssert.Contains(colleagues, "http://www.stellman-greene.com/person/X0000002");
            Console.WriteLine(_keithTriples.Count());

            List<string> incomingColleagues = _keithTriples
                .Where(r => r[1] == "http://www.stellman-greene.com/person#colleagueOf" && r[2] == "http://www.stellman-greene.com/person/A5501586")
                .Select(r => r[0])
                .ToList();

            Assert.AreEqual(3, incomingColleagues.Count());
            CollectionAssert.Contains(incomingColleagues, "http://www.stellman-greene.com/person/A5401532");
            CollectionAssert.Contains(incomingColleagues, "http://www.stellman-greene.com/person/X0000001");
            CollectionAssert.Contains(incomingColleagues, "http://www.stellman-greene.com/person/X0000002");

            string isStar = _keithTriples.Where(r => r[1] == "http://www.stellman-greene.com/person#isStar").First()[2];
            Assert.AreEqual("true^^http://www.w3.org/2001/XMLSchema#boolean", isStar);

            Console.WriteLine(_keithTriples.Count());
            _keithTriples.ForEach(r => Console.WriteLine(r[0] + " ! " + r[1] + " ! " + r[2]));
            Console.WriteLine(_keithTriples.Count());
        }

        /// <summary>
        /// Verify that a person without publications will still generate expected triples
        /// </summary>
        [TestMethod]
        public void TestPersonWithNoPublications()
        {
            Person sylvia = _people.PersonList.Find(p => p.Setnb == "A7809652");
            Assert.AreEqual("Wassertheil-Smoller, Sylvia (A7809652)", sylvia.ToString());
            using (IGraph g = new Graph())
            {
                Ontology.AssertOntologyTriples(g);
                _updater.AddPersonToGraph(g, sylvia);

                var sylviaTriples = ((SparqlResultSet)g.ExecuteQuery("SELECT * { ?s ?p ?o }")).Results
                    .Select(r => new string[] { r["s"].ToString(), r["p"].ToString(), r["o"].ToString() });

                string first = sylviaTriples.Where(r => r[1] == "http://www.stellman-greene.com/person#first").First()[2];
                Assert.AreEqual("Sylvia", first);

                string last = sylviaTriples.Where(r => r[1] == "http://www.stellman-greene.com/person#last").First()[2];
                Assert.AreEqual("Wassertheil-Smoller", last);

                string setnb = sylviaTriples.Where(r => r[1] == "http://www.stellman-greene.com/person#setnb").First()[2];
                Assert.AreEqual("A7809652", setnb);

                var sylviaColleagues = sylviaTriples
                    .Where(r => r[0] == "http://www.stellman-greene.com/person/A7809652" && r[1] == "http://www.stellman-greene.com/person#colleagueOf")
                    .Select(r => r[2])
                    .ToList<string>();

                Assert.AreEqual(1, sylviaColleagues.Count());
                CollectionAssert.Contains(sylviaColleagues, "http://www.stellman-greene.com/person/X0000003");
            }
        }
    }
}
