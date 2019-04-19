/*
 *                           Publication Harvester
 *              Copyright (c) 2003-2006 Stellman & Greene Consulting
 *      Developed for Joshua Zivin and Pierre Azoulay, Columbia University
 *            http://www.stellman-greene.com/PublicationHarvester
 *
 * This program is free software; you can redistribute it and/or modify it under
 * the terms of the GNU General Public License as published by the Free Software 
 * Foundation; either version 2 of the License, or (at your option) any later 
 * version.
 *
 * This program is distributed in the hope that it will be useful, but WITHOUT 
 * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS 
 * FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License along with 
 * this program (GPL.txt); if not, write to the Free Software Foundation, Inc., 51 
 * Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using NUnit.Framework;

namespace Com.StellmanGreene.PubMed.Unit_Tests
{
    /// <summary>
    /// Test the Publication() class
    /// Note: GetAuthorPosition is verified in TestHarvester
    /// </summary>
    [TestFixture]
    public class TestPublication
    {
        private PublicationTypes pubTypes;
        [OneTimeSetUp]
        public void TestPublicationSetUp()
        {
            pubTypes = new PublicationTypes(
                AppDomain.CurrentDomain.BaseDirectory + "\\Unit Tests\\TestPublicationTypes",
                "PublicationTypes.csv"
               );
        }

        [Test]
        public void TestPMID()
        {
            Publication p = new Publication();
            Publications.ProcessMedlineTag(ref p, "PMID- 16319490", pubTypes);
            Assert.IsTrue(p.PMID == 16319490);
        }

        [Test]
        public void TestPublicationDate()
        {
            Publication p = new Publication();
            // Year will always be set, so don't test for null
            Assert.IsNull(p.Month);
            Assert.IsNull(p.Day);
            Publications.ProcessMedlineTag(ref p, "DP  - 2005 Nov 24", pubTypes);
            Assert.IsTrue(p.Year == 2005);
            Assert.IsTrue(p.Month == "Nov");
            Assert.IsTrue(p.Day == "24");
        }

        [Test]
        public void TestTitle()
        {
            Publication p = new Publication();
            Assert.IsNull(p.Title);
            Publications.ProcessMedlineTag(ref p, "TI  - The Bias Introduced by Population Stratification in IBD Based Linkage Analysis.", pubTypes);
            Assert.IsTrue(p.Title == "The Bias Introduced by Population Stratification in IBD Based Linkage Analysis.");
        }

        [Test]
        public void TestJournal()
        {
            Publication p = new Publication();
            Assert.IsNull(p.Journal);
            Publications.ProcessMedlineTag(ref p, "TA  - Hum Hered", pubTypes);
            Assert.IsTrue(p.Journal == "Hum Hered");
        }

        [Test]
        public void TestAuthors()
        {
            Publication p = new Publication();
            Assert.IsNull(p.Authors);
            Publications.ProcessMedlineTag(ref p, "AU  - Wang T", pubTypes);
            Assert.IsTrue(p.Authors.Length == 1);
            Assert.IsTrue(p.Authors[0] == "Wang T");
            Publications.ProcessMedlineTag(ref p, "AU  - Elston RC", pubTypes);
            Assert.IsTrue(p.Authors.Length == 2);
            Assert.IsTrue(p.Authors[1] == "Elston RC");
        }

        [Test]
        public void TestVolume()
        {
            Publication p = new Publication();
            Assert.IsNull(p.Volume);
            Publications.ProcessMedlineTag(ref p, "VI  - 60", pubTypes);
            Assert.IsTrue(p.Volume == "60");
        }

        [Test]
        public void TestIssue()
        {
            Publication p = new Publication();
            Assert.IsNull(p.Issue);
            Publications.ProcessMedlineTag(ref p, "IP  - 3", pubTypes);
            Assert.IsTrue(p.Issue == "3");
        }

        [Test]
        public void TestPages()
        {
            Publication p = new Publication();
            Assert.IsNull(p.Pages);
            Publications.ProcessMedlineTag(ref p, "PG  - 134-142", pubTypes);
            Assert.IsTrue(p.Pages == "134-142");
        }

        [Test]
        public void TestGrantID()
        {
            Publication p = new Publication();
            Assert.IsNull(p.Grants);
            Publications.ProcessMedlineTag(ref p, "GR  - GM 28356/GM/NIGMS", pubTypes);
            Assert.IsTrue(p.Grants.Contains("GM 28356/GM/NIGMS"));
        }

        [Test]
        public void TestLongGrantID()
        {
            Publication p = new Publication();
            Assert.IsNull(p.Grants);
            Publications.ProcessMedlineTag(ref p, "GR  - GM 28356/GM/NIGMS", pubTypes);
            Publications.ProcessMedlineTag(ref p, "GR  - AM18811/AM/NIADDK", pubTypes);
            Publications.ProcessMedlineTag(ref p, "GR  - HD09690/HD/NICHD", pubTypes);
            Publications.ProcessMedlineTag(ref p, "GR  - AG11624/AG/NIA", pubTypes);
            Publications.ProcessMedlineTag(ref p, "GR  - AG20557/AG/NIA", pubTypes);
            Assert.IsTrue(p.Grants.Count == 5);
            foreach (string Grant in new string[] { "GM 28356/GM/NIGMS", "AM18811/AM/NIADDK", 
                "HD09690/HD/NICHD", "AG11624/AG/NIA", "AG20557/AG/NIA" } )
            {
                Assert.IsTrue(p.Grants.Contains(Grant));
            }
        }

        [Test]
        public void TestPubType()
        {
            Publication p = new Publication();
            Assert.IsNull(p.PubType);
            Publications.ProcessMedlineTag(ref p, "PT  - Clinical Trial", pubTypes);
            Assert.IsTrue(p.PubType == "Clinical Trial");
            Publications.ProcessMedlineTag(ref p, "PT  - Journal Article", pubTypes);
            Assert.IsTrue(p.PubType == "Clinical Trial");
        }

        [Test]
        public void TestMeSHHeadings()
        {
            Publication p = new Publication();
            Assert.IsNull(p.MeSHHeadings);
            Publications.ProcessMedlineTag(ref p, "MH  - Case-Control Studies", pubTypes);
            Assert.IsTrue(p.MeSHHeadings.Count == 1);
            Assert.IsTrue(p.MeSHHeadings.Contains("Case-Control Studies"));
            Publications.ProcessMedlineTag(ref p, "MH  - Charcoal/*adverse effects", pubTypes);
            Assert.IsTrue(p.MeSHHeadings.Count == 2);
            Assert.IsTrue(p.MeSHHeadings.Contains("Charcoal/*adverse effects"));
        }

        private Publication CreateTestPublication()
        {
            string MedlineData = @"PMID- 15904469
OWN - NLM
STAT- MEDLINE
DA  - 20050520
DCOM- 20050728
PUBM- Print
IS  - 1347-9032
VI  - 96
IP  - 5
DP  - 2005 May
TI  - Charcoal cigarette filters and lung cancer risk in Aichi Prefecture,
      Japan.
PG  - 283-7
AB  - The lung cancer mortality rate has been lower in Japan than in the United
      States for several decades. We hypothesized that this difference is due to
      the Japanese preference for cigarettes with charcoal-containing filters,
      which efficiently absorb selected gas phase components of mainstream smoke
      including the carcinogen 4-(methylnitrosamino)-1-(3-pyridyl)-1-butanone.
      We analyzed a subset of smokers (396 cases and 545 controls) from a
      case-control study of lung cancer conducted in Aichi Prefecture, Japan.
      The risk associated with charcoal filters (73% of all subjects) was
      evaluated after adjusting for age, sex, education and smoking dose. The
      odds ratio (OR) associated with charcoal compared with 'plain' cigarette
      filters was 1.2 (95% confidence intervals [CI] 0.9, 1.6). The
      histologic-specific risks were similar (e.g. OR = 1.3, 95% CI 0.9, 2.1 for
      adenocarcinoma). The OR was 1.7 (95% CI 1.1, 2.9) in smokers who switched
      from 'plain' to charcoal brands. The mean daily number of cigarettes
      smoked in subjects who switched from 'plain' to charcoal brands was 22.5
      and 23.0, respectively. The findings from this study did not indicate that
      charcoal filters were associated with an attenuated risk of lung cancer.
      As the detection of a modest benefit or risk (e.g. 10-20%) that can have
      significant public health impact requires large samples, the findings
      should be confirmed or refuted in larger studies.
AD  - Department of Health Evaluation Sciences, Penn State Cancer Institute,
      Division of Population Sciences, Penn State College of Medicine, Hershey,
      PA 17033, USA.
FAU - Muscat, Joshua E
AU  - Muscat JE
FAU - Takezaki, Toshiro
AU  - Takezaki T
FAU - Tajima, Kazuo
AU  - Tajima K
FAU - Stellman, Steven D
AU  - Stellman SD
LA  - eng
GR  - CA-17613/CA/NCI
GR  - CA-68387/CA/NCI
PT  - Clinical Trial
PT  - Journal Article
PL  - England
TA  - Cancer Sci
JID - 101168776
RN  - 16291-96-6 (Charcoal)
SB  - IM
MH  - Adult
MH  - Aged
MH  - Aged, 80 and over
MH  - Case-Control Studies
MH  - Charcoal/*adverse effects
MH  - Female
MH  - Filtration/*utilization
MH  - Humans
MH  - Japan
MH  - Lung Neoplasms/*etiology/pathology
MH  - Male
MH  - Middle Aged
MH  - Research Support, N.I.H., Extramural
MH  - Research Support, U.S. Gov't, Non-P.H.S.
MH  - Research Support, U.S. Gov't, P.H.S.
MH  - Risk Factors
MH  - Smoking/*adverse effects
EDAT- 2005/05/21 09:00
MHDA- 2005/07/29 09:00
AID - CAS045 [pii]
AID - 10.1111/j.1349-7006.2005.00045.x [doi]
PST - ppublish
SO  - Cancer Sci 2005 May;96(5):283-7.";

            PublicationTypes ptc = new PublicationTypes(
                AppDomain.CurrentDomain.BaseDirectory + "\\Unit Tests\\TestPublicationTypes",
                "PublicationTypes.csv"
               );

            Publications Reader = new Publications(MedlineData, ptc);
            Assert.IsTrue(Reader.PublicationList.Length == 1);
            return Reader.PublicationList[0];
        }

        /// <summary>
        /// Test that a person contains the publication created with CreateTestPublication
        /// </summary>
        /// <param name="p">Person to test</param>
        private void TestCreatedPublication(Publication p)
        {
            Assert.IsTrue(p.PMID == 15904469);
            Assert.IsTrue(p.Year == 2005);
            Assert.IsTrue(p.Month == "May");
            Assert.IsNull(p.Day);
            Assert.IsTrue(p.Volume == "96");
            Assert.IsTrue(p.Issue == "5");
            Assert.IsTrue(p.Title == "Charcoal cigarette filters and lung cancer risk in Aichi Prefecture, Japan.");
            Assert.IsTrue(p.Pages == "283-7");
            Assert.IsTrue(p.Authors.Length == 4);
            Assert.IsTrue(p.Authors[0] == "Muscat JE");
            Assert.IsTrue(p.Authors[3] == "Stellman SD");
            Assert.IsTrue(p.Grants.Count == 2);
            Assert.IsTrue(p.Grants.Contains("CA-17613/CA/NCI"));
            Assert.IsTrue(p.Grants.Contains("CA-68387/CA/NCI"));
            Assert.IsTrue(p.Journal == "Cancer Sci");
            Assert.IsTrue(p.MeSHHeadings.Count == 17);
            Assert.IsTrue(p.MeSHHeadings.Contains("Adult"));
            Assert.IsTrue(p.MeSHHeadings.Contains("Smoking/*adverse effects"));
        }




        [Test]
        public void TestWholePublication()
        {
            Publication p = CreateTestPublication();
            TestCreatedPublication(p);
        }

        /// <summary>
        /// Verify that a publication is written to the database properly
        /// </summary>
        [Test]
        public void TestWriteToDB()
        {
            // Create a new database and write the publication to it
            Database DB = new Database("Publication Harvester Unit Test");
            Harvester h = new Harvester(DB);
            h.CreateTables();
            Publication p = CreateTestPublication();
            PublicationTypes ptc = new PublicationTypes(
                AppDomain.CurrentDomain.BaseDirectory + "\\Unit Tests\\TestPublicationTypes",
                "PublicationTypes.csv"
               );
            Publications.WriteToDB(p, DB, ptc, new string[] {"eng"});

            // Verify the publication has been added
            DataTable Results = DB.ExecuteQuery(
                @"SELECT Journal, Year, Authors, Month, Day, Title, Volume,
                         Issue, Pages, PubType, PubTypeCategoryID
                    FROM Publications
                   WHERE PMID = 15904469"
            );
            Assert.IsTrue(Results.Rows[0]["Year"].Equals(2005));
            Assert.IsTrue(Results.Rows[0]["Authors"].Equals(4));
            Assert.IsTrue(Results.Rows[0]["Year"].Equals(2005));
            Assert.IsTrue(Results.Rows[0]["Month"].Equals("May"));
            Assert.IsTrue(Results.Rows[0]["Day"].Equals(DBNull.Value));
            Assert.IsTrue(Results.Rows[0]["Title"].Equals("Charcoal cigarette filters and lung cancer risk in Aichi Prefecture, Japan."));
            Assert.IsTrue(Results.Rows[0]["Volume"].Equals("96"));
            Assert.IsTrue(Results.Rows[0]["Issue"].Equals("5"));
            Assert.IsTrue(Results.Rows[0]["Pages"].Equals("283-7"));
            Assert.IsTrue(Results.Rows[0]["PubType"].Equals("Clinical Trial"));
            Assert.IsTrue(Results.Rows[0]["PubTypeCategoryID"].Equals((short) 1));
                         

            // Verify the authors
            Assert.IsTrue(DB.GetIntValue("SELECT Count(*) FROM PublicationAuthors") == 4);
            Results = DB.ExecuteQuery("SELECT PMID, Position, Author, First, Last FROM PublicationAuthors ORDER BY Position");
            Assert.IsTrue(Results.Rows[0]["PMID"].Equals(15904469));
            Assert.IsTrue(Results.Rows[1]["PMID"].Equals(15904469));
            Assert.IsTrue(Results.Rows[2]["PMID"].Equals(15904469));
            Assert.IsTrue(Results.Rows[3]["PMID"].Equals(15904469));
            Assert.IsTrue(Results.Rows[0]["Position"].Equals(1));
            Assert.IsTrue(Results.Rows[1]["Position"].Equals(2));
            Assert.IsTrue(Results.Rows[2]["Position"].Equals(3));
            Assert.IsTrue(Results.Rows[3]["Position"].Equals(4));
            Assert.IsTrue(Results.Rows[0]["Author"].ToString() == "Muscat JE");
            Assert.IsTrue(Results.Rows[1]["Author"].ToString() == "Takezaki T");
            Assert.IsTrue(Results.Rows[2]["Author"].ToString() == "Tajima K");
            Assert.IsTrue(Results.Rows[3]["Author"].ToString() == "Stellman SD");
            Assert.IsTrue(Results.Rows[0]["First"].Equals((short) 1));
            Assert.IsTrue(Results.Rows[1]["First"].Equals((short) 0));
            Assert.IsTrue(Results.Rows[2]["First"].Equals((short) 0));
            Assert.IsTrue(Results.Rows[3]["First"].Equals((short) 0));
            Assert.IsTrue(Results.Rows[0]["Last"].Equals((short) 0));
            Assert.IsTrue(Results.Rows[1]["Last"].Equals((short) 0));
            Assert.IsTrue(Results.Rows[2]["Last"].Equals((short) 0));
            Assert.IsTrue(Results.Rows[3]["Last"].Equals((short) 1));

            // Verifying the MeSH headings
            // Note the join to PublicationMeSHHeadings to make sure it was populated properly
            Assert.IsTrue(DB.GetIntValue("SELECT Count(*) FROM PublicationMeSHHeadings WHERE PMID = " + p.PMID.ToString()) == 17);
            Results = DB.ExecuteQuery(
                @"SELECT m.Heading 
                    FROM MeSHHeadings m, PublicationMeSHHeadings p
                   WHERE p.MeSHHeadingID = m.ID
                     AND p.PMID = " + p.PMID.ToString()
            );
            Assert.IsTrue(Results.Rows.Count == 17);
            Assert.IsTrue(Results.Rows[0]["Heading"].ToString() == "Adult");
            Assert.IsTrue(Results.Rows[5]["Heading"].ToString() == "Female");
            Assert.IsTrue(Results.Rows[9]["Heading"].ToString() == "Lung Neoplasms/*etiology/pathology");
            Assert.IsTrue(Results.Rows[16]["Heading"].ToString() == "Smoking/*adverse effects");

            // Verify the grants
            Assert.IsTrue(DB.GetIntValue("SELECT Count(*) FROM PublicationGrants WHERE PMID = " + p.PMID.ToString()) == 2);
            Results = DB.ExecuteQuery(
                @"SELECT GrantID
                    FROM PublicationGrants
                   WHERE PMID = " + p.PMID.ToString() + " ORDER BY GrantID ASC"
            );
            Assert.IsTrue(Results.Rows[0]["GrantID"].ToString() == "CA-17613/CA/NCI");
            Assert.IsTrue(Results.Rows[1]["GrantID"].ToString() == "CA-68387/CA/NCI");

        }

        /// <summary>
        /// Test the constructor that takes a database and a PMID and reads
        /// the publications for that person
        /// </summary>
        [Test]
        public void TestCreateFromDatabase()
        {
            // Add a publication to the database
            Database DB = new Database("Publication Harvester Unit Test");
            Harvester harvester = new Harvester(DB);
            harvester.CreateTables();
            Publication write = CreateTestPublication();
            PublicationTypes ptc = new PublicationTypes(
                AppDomain.CurrentDomain.BaseDirectory + "\\Unit Tests\\TestPublicationTypes",
                "PublicationTypes.csv"
               );
            Publications.WriteToDB(write, DB, ptc, new string[] { "eng" });

            // Read it back
            Publication read;
            Assert.IsTrue(Publications.GetPublication(DB, 15904469, out read, false));
            TestCreatedPublication(read);
        }

        /// <summary>
        /// For PMID 14332364, there are 3 authors. MATHIES is the 2nd author. The software 
        /// should count him as a "middle author" pub, not a "second author" pub, because 
        /// there are only 3 authors.
        /// </summary>
        [Test]
        public void TestMiddleAuthor()
        {
            string MedlineData = @"PMID- 14332364
OWN - NLM
STAT- OLDMEDLINE
DA  - 19651101
DCOM- 19961201
LR  - 20051116
PUBM- Print
IS  - 0002-922X (Print)
VI  - 110
DP  - 1965 Sep
TI  - CONGENITAL SCALP DEFECTS IN TWIN SISTERS.
PG  - 293-5
FAU - HODGMAN, J E
AU  - HODGMAN JE
FAU - MATHIES, A W Jr
AU  - MATHIES AW Jr
FAU - LEVAN, N E
AU  - LEVAN NE
LA  - eng
PT  - Journal Article
PL  - UNITED STATES
TA  - Am J Dis Child
JT  - American journal of diseases of children (1960)
JID - 0370471
SB  - OM
MH  - *Abnormalities
MH  - *Diseases in Twins
MH  - *Infant, Newborn
MH  - *Scalp
OTO - NLM
OT  - *ABNORMALITIES
OT  - *DISEASES IN TWINS
OT  - *INFANT, NEWBORN
OT  - *SCALP
EDAT- 1965/09/01
MHDA- 1965/09/01 00:01
PST - ppublish
SO  - Am J Dis Child. 1965 Sep;110:293-5.";
            
            // Add a person to the database
            Database DB = new Database("Publication Harvester Unit Test");
            Person Mathies = new Person("A5703161", "Allan", "W", "MATHIES", false,
                new String[] { "MATHIES AW Jr" }, "MATHIES AW Jr[au]");

            // Create the publication
            PublicationTypes ptc = new PublicationTypes(
                AppDomain.CurrentDomain.BaseDirectory + "\\Unit Tests\\TestPublicationTypes",
                "PublicationTypes.csv"
               );
            Publications Pubs = new Publications(MedlineData, ptc);
            Assert.IsTrue(Pubs.PublicationList.Length == 1);
            Publication Pub = Pubs.PublicationList[0];
            Assert.IsTrue(Pub.Authors[1] == "MATHIES AW Jr");

            // Clear the database
            Harvester harvester = new Harvester(DB);
            harvester.CreateTables();

            // Write the person and publication to the database
            // NOTE: The author position is determined in WritePeoplePublicationsToDB()
            PublicationTypes PubTypes = new PublicationTypes(
                AppDomain.CurrentDomain.BaseDirectory + "\\Unit Tests\\TestPublicationTypes",
                "PublicationTypes.csv"
               );
            Mathies.WriteToDB(DB);
            Publications.WriteToDB(Pub, DB, PubTypes, new string[] { "eng" });
            Publications.WritePeoplePublicationsToDB(DB, Mathies, Pub);

            // Verify that Mathies is the middle author, not the second author
            Publications PubsFromDB = new Publications(DB, Mathies, false);
            Assert.IsTrue(PubsFromDB.PublicationList.Length == 1);
            Assert.IsTrue(PubsFromDB.PublicationList[0].PMID == 14332364);
            Harvester.AuthorPositions PositionType;
            Assert.IsTrue(Publications.GetAuthorPosition(DB, 14332364, Mathies, out PositionType, "PeoplePublications") == 2);
            Assert.IsTrue(PositionType == Harvester.AuthorPositions.Middle);

        }

    }
}
