using System;
using System.Collections.Generic;
using System.Text;
using Com.StellmanGreene.SocialNetworking;
using System.Collections;
using System.Data;
using System.IO;
using NUnit.Framework;
using Com.StellmanGreene.PubMed;


namespace Com.StellmanGreene.SocialNetworking.Unit_Tests
{
    /// <summary>
    /// Test the CommonPublications class
    /// </summary>
    [TestFixture]
    public class TestCommonPublications
    {
        private Database DB;

        [OneTimeSetUp]
        public void TestCommonPublicationsSetUp()
        {
            UnitTestData.Create();
            DB = UnitTestData.GetDB();
        }

        [Test]
        public void TestAliceCarol()
        {
            // Alice and Carol coauthored the following publications:
            //   19910001, 19930001, 19950003, 19940002
            // So the hash table should have these values:
            // 1991=>1
            // 1992=>0
            // 1993=>1
            // 1994=>5
            // 1995=>1

            // Get the common publications
            Hashtable Results = CommonPublications.Find(
                "Alice", "social_unit_test_firstdegree", false,
                "Carol", "social_unit_test_seconddegree", true, DB);
            Assert.AreEqual(CommonPublications.EarliestYear(Results), 1991);
            Assert.AreEqual(CommonPublications.LatestYear(Results), 1995);
            Assert.AreEqual(Results.Count, 5);
            Assert.AreEqual(Results[1991], 1);
            Assert.AreEqual(Results[1992], 0);
            Assert.AreEqual(Results[1993], 1);
            Assert.AreEqual(Results[1994], 5);
            Assert.AreEqual(Results[1995], 1);
        }



        [Test]
        public void TestJustinJohn()
        {
            // Justin and John coauthored the following publications:
            //   19960002, 19990002, 20050002, 20030002, 20050003
            // So the hash table should have these values:
            // 1996=>1
            // 1997=>0
            // 1998=>0
            // 1999=>1
            // 2000=>0
            // 2001=>0
            // 2002=>0
            // 2003=>1
            // 2004=>0
            // 2005=>2

            // Get the common publications
            Hashtable Results = CommonPublications.Find(
                "Justin", "social_unit_test_seconddegree", true,
                "John", "social_unit_test_seconddegree", false, DB);
            Assert.AreEqual(CommonPublications.EarliestYear(Results), 1996);
            Assert.AreEqual(CommonPublications.LatestYear(Results), 2005);
            Assert.AreEqual(Results.Count, 10);
            Assert.AreEqual(Results[1996], 1);
            Assert.AreEqual(Results[1997], 0);
            Assert.AreEqual(Results[1998], 0);
            Assert.AreEqual(Results[1999], 1);
            Assert.AreEqual(Results[2000], 0);
            Assert.AreEqual(Results[2001], 0);
            Assert.AreEqual(Results[2002], 0);
            Assert.AreEqual(Results[2003], 1);
            Assert.AreEqual(Results[2004], 0);
            Assert.AreEqual(Results[2005], 2);
        }


        [Test]
        public void TestJohnCarol()
        {
            // John and Carol did not author any publications together

            // Get the common publications
            Hashtable Results = CommonPublications.Find(
                "John", "social_unit_test_seconddegree", false,
                "Carol", "social_unit_test_seconddegree", true, DB);
            Assert.AreEqual(CommonPublications.EarliestYear(Results), 0);
            Assert.AreEqual(CommonPublications.LatestYear(Results), 0);
            Assert.AreEqual(Results.Count, 0);
        }

        [Test]
        public void TestJustinConstance()
        {
            // Justin and Constance coauthored the following publications:
            //   19980001, 20000001, 20050001, 20050003
            // So the hash table should have these values:
            // 1998=>1
            // 1999=>0
            // 2000=>1
            // 2001=>0
            // 2002=>0
            // 2003=>0
            // 2004=>0
            // 2005=>2

            // Get the common publications
            Hashtable Results = CommonPublications.Find(
                "Justin", "social_unit_test_seconddegree", true,
                "Constanc", "social_unit_test_seconddegree", false, DB);
            Assert.AreEqual(CommonPublications.EarliestYear(Results), 1998);
            Assert.AreEqual(CommonPublications.LatestYear(Results), 2005);
            Assert.AreEqual(Results.Count, 8);
            Assert.AreEqual(Results[1998], 1);
            Assert.AreEqual(Results[1999], 0);
            Assert.AreEqual(Results[2000], 1);
            Assert.AreEqual(Results[2001], 0);
            Assert.AreEqual(Results[2002], 0);
            Assert.AreEqual(Results[2003], 0);
            Assert.AreEqual(Results[2004], 0);
            Assert.AreEqual(Results[2005], 2);
        }



        [Test]
        public void TestJustinRoger()
        {
            // Justin and Roger coauthored the following publications:
            //   20010001, 20020001, 20030001, 20010002, 20010003
            // So the hash table should have these values:
            // 2001=>3
            // 2002=>1
            // 2003=>1

            // Get the common publications
            Hashtable Results = CommonPublications.Find(
                "Justin", "social_unit_test_seconddegree", true,
                "Roger", "social_unit_test_seconddegree", false, DB);
            Assert.AreEqual(CommonPublications.EarliestYear(Results), 2001);
            Assert.AreEqual(CommonPublications.LatestYear(Results), 2003);
            Assert.AreEqual(Results.Count, 3);
            Assert.AreEqual(Results[2001], 3);
            Assert.AreEqual(Results[2002], 1);
            Assert.AreEqual(Results[2003], 1);

            // Make sure it still works if we reverse the parameters to Find()
            Results = CommonPublications.Find(
                "Roger", "social_unit_test_seconddegree", false,
                "Justin", "social_unit_test_seconddegree", true, DB);
            Assert.AreEqual(CommonPublications.EarliestYear(Results), 2001);
            Assert.AreEqual(CommonPublications.LatestYear(Results), 2003);
            Assert.AreEqual(Results.Count, 3);
            Assert.AreEqual(Results[2001], 3);
            Assert.AreEqual(Results[2002], 1);
            Assert.AreEqual(Results[2003], 1);

        }


        [Test]
        public void TestCarolLisa()
        {
            // Carol and Lisa coauthored the following publications:
            //   19960001, 19970001, 19990001, 19990003, 19990004
            // So the hash table should have these values:
            // 1996=>1
            // 1997=>1
            // 1998=>0
            // 1999=>3

            // Get the common publications
            Hashtable Results = CommonPublications.Find(
                "Carol", "social_unit_test_seconddegree", true,
                "Lisa", "social_unit_test_seconddegree", false, DB);
            Assert.AreEqual(CommonPublications.EarliestYear(Results), 1996);
            Assert.AreEqual(CommonPublications.LatestYear(Results), 1999);
            Assert.AreEqual(Results.Count, 4);
            Assert.AreEqual(Results[1996], 1);
            Assert.AreEqual(Results[1997], 1);
            Assert.AreEqual(Results[1998], 0);
            Assert.AreEqual(Results[1999], 3);
        }

    }
}
