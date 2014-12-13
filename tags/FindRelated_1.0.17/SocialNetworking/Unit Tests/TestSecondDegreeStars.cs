using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Collections;
using Com.StellmanGreene.PubMed;

namespace Com.StellmanGreene.SocialNetworking.Unit_Tests
{
    /// <summary>
    /// Test the SecondDegreeStars class
    /// </summary>
    [TestFixture]
    public class TestSecondDegreeStars
    {
        private Database DB;

        [TestFixtureSetUp]
        public void TestSecondDegreeStarsSetUp()
        {
            UnitTestData.Create();
            DB = UnitTestData.GetDB();
        }


        /// <summary>
        /// Find second degree stars for Alice / Justin
        /// </summary>
        [Test]
        public void TestAliceJustin()
        {
            // The stars should be Effie, Constance, Roger John
            SecondDegreeStars Stars = new SecondDegreeStars(
                DB, "Alice", "social_unit_test_firstdegree", "Justin", "social_unit_test_seconddegree");

            Assert.AreEqual(Stars.ColleagueSetnb, "Alice");
            Assert.AreEqual(Stars.FirstDegreeStarSetnb, "Justin");
            Assert.AreEqual(Stars.Setnbs.Count, 4);
            ArrayList UniqueStars = new ArrayList();
            foreach (string Setnb in Stars.Setnbs)
            {
                if ((Setnb == "Effie" || Setnb == "Constanc" || Setnb == "Roger" || Setnb == "John")
                    && (!UniqueStars.Contains(Setnb)))
                    UniqueStars.Add(Setnb);
            }
            Assert.AreEqual(UniqueStars.Count, 4);
        }


        /// <summary>
        /// Find second degree stars for Alice / Carol
        /// </summary>
        [Test]
        public void TestAliceCarol()
        {
            // The stars should be Mallory, Joe, Lisa, Effie
            SecondDegreeStars Stars = new SecondDegreeStars(
                DB, "Alice", "social_unit_test_firstdegree", "Carol", "social_unit_test_seconddegree");

            Assert.AreEqual(Stars.ColleagueSetnb, "Alice");
            Assert.AreEqual(Stars.FirstDegreeStarSetnb, "Carol");
            Assert.AreEqual(Stars.Setnbs.Count, 4);
            ArrayList UniqueStars = new ArrayList();
            foreach (string Setnb in Stars.Setnbs)
            {
                if ((Setnb == "Mallory" || Setnb == "Joe" || Setnb == "Lisa" || Setnb == "Effie")
                    && (!UniqueStars.Contains(Setnb)))
                    UniqueStars.Add(Setnb);
            }
            Assert.AreEqual(UniqueStars.Count, 4);
        }


        /// <summary>
        /// Make sure no second degree stars are returned for Carol and Boffo
        /// </summary>
        [Test]
        public void TestMismatch()
        {
            SecondDegreeStars Stars = new SecondDegreeStars(
                DB, "Alice", "social_unit_test_firstdegree", "Boffo", "social_unit_test_seconddegree");

            Assert.AreEqual(Stars.ColleagueSetnb, "Alice");
            Assert.AreEqual(Stars.FirstDegreeStarSetnb, "Boffo");
            Assert.AreEqual(Stars.Setnbs.Count, 0);
        }


    }
}
