using System;
using System.Collections.Generic;
using System.Text;
using Com.StellmanGreene.PubMed;
using NUnit.Framework;

namespace SCGen.Unit_Tests
{
    [TestFixture]
    public class TestRoster
    {
        Roster roster;

        /// <summary>
        /// Set up the test fixture by loading the roster
        /// </summary>
        [OneTimeSetUp]
        public void TestRosterSetUp()
        {
            roster = new Roster(AppDomain.CurrentDomain.BaseDirectory + "\\Test Data\\TestRoster\\testroster.csv");
        }

        /// <summary>
        /// Verify that roster.RosterData contains the correct number of rows.
        /// </summary>
        [Test]
        public void IsRosterLoaded()
        {
            Assert.IsTrue(roster.RosterData.Rows.Count == 17);
        }


        /// <summary>
        /// Verify that roster.FindPerson() returns the correct information for a Jerry Gilles
        /// </summary>
        [Test]
        public void TestGillesJM()
        {
            Person[] people = roster.FindPerson("gilles jm");
            Assert.AreEqual(people.Length, 1);
            Person person = people[0];
            Assert.AreEqual(person.Setnb, "A0100564");
            Assert.AreEqual(person.First, "JERRY");
            Assert.AreEqual(person.Middle, "M");
            Assert.AreEqual(person.Last, "GILLES");
            Assert.AreEqual(person.MedlineSearch, "\"gilles jm\"[au]");
            Assert.IsTrue(person.Names.Length == 1);
            Assert.AreEqual(person.Names[0], "gilles jm");
        }

        /// <summary>
        /// Verify that roster.FindPerson() returns the correct information for a Malcom De Camp
        /// </summary>
        [Test]
        public void TestDeCampMM()
        {
            Person[] people = roster.FindPerson("de camp mm");
            Assert.AreEqual(people.Length, 1);
            Person person = people[0];
            Assert.AreEqual(person.Setnb, "A0100539");
            Assert.AreEqual(person.First, "MALCOLM");
            Assert.AreEqual(person.Middle, "MCAVOY");
            Assert.AreEqual(person.Last, "DE CAMP");
            Assert.AreEqual(person.MedlineSearch, "\"de camp mm jr\"[au]");
            Assert.IsTrue(person.Names.Length == 3);
            Assert.AreEqual(person.Names[0], "de camp mm jr");
            Assert.AreEqual(person.Names[1], "de camp mm");
            Assert.AreEqual(person.Names[2], "de camp m");
        }

        /// <summary>
        /// Verify that roster.FindPerson() returns the correct information for a Emil Frei
        /// </summary>
        [Test]
        public void TestFreiEF()
        {
            Person[] people = roster.FindPerson("frei e 3rd");
            Assert.AreEqual(people.Length, 1);
            Person person = people[0];
            Assert.AreEqual(person.Setnb, "A4900732");
            Assert.AreEqual(person.First, "EMIL");
            Assert.AreEqual(person.Middle, "");
            Assert.AreEqual(person.Last, "FREI");
            Assert.AreEqual(person.MedlineSearch, "(\"frei e 3rd\"[au] or (\"frei e\"[au] and cancer not (\"frei e 2nd\"[au] or germany[ad] or rats) and 1950:1993[dp]))");
            Assert.IsTrue(person.Names.Length == 4);
            Assert.AreEqual(person.Names[0], "frei e 3rd");
            Assert.AreEqual(person.Names[1], "frei e");
            Assert.AreEqual(person.Names[2], "frei ef 3rd");
            Assert.AreEqual(person.Names[3], "frei e iii");
        }

        /// <summary>
        /// The name "williams k" should match three different people in the roster.
        /// </summary>
        [Test]
        public void TestDuplicateRosterEntries()
        {
            // Let's also make sure the match is case-insensitive
            Person[] people = roster.FindPerson("WILLiams K");
            Assert.AreEqual(people.Length, 3);

            foreach (Person person in people)
            {
                switch (person.Setnb)
                {
                    case "Z7654321":
                        Assert.AreEqual(person.First, "JOHN");
                        Assert.AreEqual(person.Middle, "");
                        Assert.AreEqual(person.Last, "DOE");
                        Assert.AreEqual(person.MedlineSearch, "this is meant to be a duplicate of Williams");
                        Assert.AreEqual(person.Names.Length, 6);
                        Assert.AreEqual(person.Names[0], "williams k");
                        Assert.AreEqual(person.Names[1], "doe j");
                        Assert.AreEqual(person.Names[2], "another name");
                        Assert.AreEqual(person.Names[3], "and another");
                        Assert.AreEqual(person.Names[4], "fifth name");
                        Assert.AreEqual(person.Names[5], "sixth name");

                        // Also check that John Doe's other name matches
                        people = roster.FindPerson("doe j");
                        Assert.AreEqual(people.Length, 1);
                        Person secondPerson = people[0];
                        Assert.AreEqual(secondPerson.First, "JOHN");
                        Assert.AreEqual(secondPerson.Middle, "");
                        Assert.AreEqual(secondPerson.Last, "DOE");
                        Assert.AreEqual(secondPerson.MedlineSearch, "this is meant to be a duplicate of Williams");
                        Assert.AreEqual(secondPerson.Names.Length, 6);
                        Assert.AreEqual(secondPerson.Names[0], "williams k");
                        Assert.AreEqual(secondPerson.Names[1], "doe j");
                        Assert.AreEqual(secondPerson.Names[2], "another name");
                        Assert.AreEqual(secondPerson.Names[3], "and another");
                        Assert.AreEqual(secondPerson.Names[4], "fifth name");
                        Assert.AreEqual(secondPerson.Names[5], "sixth name");
                        break;

                    case "A0100733":
                        Assert.AreEqual(person.First, "KENNETH");
                        Assert.AreEqual(person.Middle, "");
                        Assert.AreEqual(person.Last, "WILLIAMS");
                        Assert.AreEqual(person.MedlineSearch, "\"williams k\"[au]");
                        Assert.AreEqual(person.Names.Length, 1);
                        Assert.AreEqual(person.Names[0], "williams k");
                        break;

                    case "Q1234567":
                        Assert.AreEqual(person.First, "FAKEY");
                        Assert.AreEqual(person.Middle, "Q");
                        Assert.AreEqual(person.Last, "FICTITIOUSGUY");
                        Assert.AreEqual(person.MedlineSearch, "this is meant to be another duplicate of Williams");
                        Assert.AreEqual(person.Names.Length, 2);
                        Assert.AreEqual(person.Names[0], "fictitiousguy fq");
                        Assert.AreEqual(person.Names[1], "williams k");

                        // Also check that John Doe's other name matches
                        people = roster.FindPerson("fictitiousguy fq");
                        Assert.AreEqual(people.Length, 1);
                        secondPerson = people[0];
                        Assert.AreEqual(secondPerson.First, "FAKEY");
                        Assert.AreEqual(secondPerson.Middle, "Q");
                        Assert.AreEqual(secondPerson.Last, "FICTITIOUSGUY");
                        Assert.AreEqual(secondPerson.MedlineSearch, "this is meant to be another duplicate of Williams");
                        Assert.IsTrue(secondPerson.Names.Length == 2);
                        Assert.AreEqual(secondPerson.Names[0], "fictitiousguy fq");
                        Assert.AreEqual(secondPerson.Names[1], "williams k");
                        break;

                    default:
                        Assert.Fail();
                        break;
                }
            }
        }
        /// <summary>
        /// Test some negative and boundary cases
        /// </summary>
        [Test]
        public void BoundaryCases() 
        {
            Assert.AreEqual(roster.FindPerson("this won't be found"), null);
            Assert.AreEqual(roster.FindPerson(""), null);
            Assert.AreEqual(roster.FindPerson(null), null);
        }
    }
}
