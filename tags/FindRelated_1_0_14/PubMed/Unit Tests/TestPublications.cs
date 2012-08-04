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
using NUnit.Framework;

namespace Com.StellmanGreene.PubMed.Unit_Tests
{

    /// <summary>
    /// Test the Publications() class
    /// Note: The constructor Publications(DB, person) is tested in TestHarvester
    /// </summary>
    [TestFixture]
    public class TestPublications
    {
        /// <summary>
        /// Read three normal publications which were retrieved from NCBI
        /// </summary>
        [Test]
        public void ReadThreeNormalPublications()
        {
            string MedlineData;
            #region Assign MedlineData variable to contain test data
            MedlineData = @"PMID- 15904469
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
SO  - Cancer Sci 2005 May;96(5):283-7.

PMID- 15856074
OWN - NLM
STAT- In-Data-Review
DA  - 20051118
PUBM- Print
IS  - 1053-4245
VI  - 15
IP  - 6
DP  - 2005 Nov
TI  - Residential environmental exposures and other characteristics associated
      with detectable PAH-DNA adducts in peripheral mononuclear cells in a
      population-based sample of adult females.
PG  - 482-90
AB  - The detection of polycyclic aromatic hydrocarbon (PAH)-DNA adducts in
      human lymphocytes may be useful as a surrogate end point for individual
      cancer risk prediction. In this study, we examined the relationship
      between environmental sources of residential PAH, as well as other
      potential factors that may confound their association with cancer risk,
      and the detection of PAH-DNA adducts in a large population-based sample of
      adult women. Adult female residents of Long Island, New York, aged at
      least 20 years were identified from the general population between August
      1996 and July 1997. Among 1556 women who completed a structured
      questionnaire, 941 donated sufficient blood (25+ ml) to allow use of a
      competitive ELISA for measurement of PAH-DNA adducts in peripheral blood
      mononuclear cells. Ambient PAH exposure at the current residence was
      estimated using geographic modeling (n=796). Environmental home samples of
      dust (n=356) and soil (n=360) were collected on a random subset of
      long-term residents (15+ years). Multivariable regression was conducted to
      obtain the best-fitting predictive models. Three separate models were
      constructed based on data from : (A) the questionnaire, including a
      dietary history; (B) environmental home samples; and (C) geographic
      modeling. Women who donated blood in summer and fall had increased odds of
      detectable PAH-DNA adducts (OR=2.65, 95% confidence interval (CI)=1.69,
      4.17; OR=1.59, 95% CI=1.08, 2.32, respectively), as did current and past
      smokers (OR=1.50, 95% CI=1.00, 2.24; OR=1.46, 95% CI=1.05, 2.02,
      respectively). There were inconsistent associations between detectable
      PAH-DNA adducts and other known sources of residential PAH, such as
      grilled and smoked foods, or a summary measure of total dietary
      benzo-[a]-pyrene (BaP) intake during the year prior to the interview.
      Detectable PAH-DNA adducts were inversely associated with increased BaP
      levels in dust in the home, but positively associated with BaP levels in
      soil outside of the home, although CIs were wide. Ambient BaP estimates
      from the geographic model were not associated with detectable PAH-DNA
      adducts. These data suggest that PAH-DNA adducts detected in a
      population-based sample of adult women with ambient exposure levels
      reflect some key residential PAH exposure sources assessed in this study,
      such as cigarette smoking.Journal of Exposure Analysis and Environmental
      Epidemiology (2005) 15, 482-490. doi:10.1038/sj.jea.7500426; published
      online 27 April 2005.
AD  - aDepartment of Epidemiology, CB#7435 McGavran-Greenberg Hall, University
      of North Carolina School of Public Health, Chapel Hill, North Carolina
      27599-7435, USA.
FAU - Shantakumar, Sumitra
AU  - Shantakumar S
FAU - Gammon, Marilie D
AU  - Gammon MD
FAU - Eng, Sybil M
AU  - Eng SM
FAU - Sagiv, Sharon K
AU  - Sagiv SK
FAU - Gaudet, Mia M
AU  - Gaudet MM
FAU - Teitelbaum, Susan L
AU  - Teitelbaum SL
FAU - Britton, Julie A
AU  - Britton JA
FAU - Terry, Mary Beth
AU  - Terry MB
FAU - Paykin, Andrea
AU  - Paykin A
FAU - Young, Tie Lan
AU  - Young TL
FAU - Wang, Lian Wen
AU  - Wang LW
FAU - Wang, Qiao
AU  - Wang Q
FAU - Stellman, Steven D
AU  - Stellman SD
FAU - Beyea, Jan
AU  - Beyea J
FAU - Hatch, Maureen
AU  - Hatch M
FAU - Camann, David
AU  - Camann D
FAU - Prokopczyk, Bogdan
AU  - Prokopczyk B
FAU - Kabat, Geoffrey C
AU  - Kabat GC
FAU - Levin, Bruce
AU  - Levin B
FAU - Neugut, Alfred I
AU  - Neugut AI
FAU - Santella, Regina M
AU  - Santella RM
LA  - eng
PT  - Journal Article
PL  - England
TA  - J Expo Anal Environ Epidemiol
JID - 9111438
SB  - IM
EDAT- 2005/04/28 09:00
MHDA- 2005/04/28 09:00
AID - 7500426 [pii]
AID - 10.1038/sj.jea.7500426 [doi]
PST - ppublish
SO  - J Expo Anal Environ Epidemiol 2005 Nov;15(6):482-90.

PMID- 15767332
OWN - NLM
STAT- MEDLINE
DA  - 20050315
DCOM- 20050725
PUBM- Print
IS  - 1055-9965
VI  - 14
IP  - 3
DP  - 2005 Mar
TI  - Influence of type of cigarette on peripheral versus central lung cancer.
PG  - 576-81
AB  - OBJECTIVES: Adenocarcinoma has replaced squamous cell carcinoma as the
      most common cell type of lung cancer in the United States. It has been
      proposed that this shift is due to the increased use of filter and
      lower-tar cigarettes, resulting in increased delivery of smoke to
      peripheral regions of the lungs, where adenocarcinoma usually occurs. We
      reviewed radiologic data to evaluate the hypothesis that tumors in smokers
      of cigarettes with lower-tar yield are more likely to occur peripherally
      than tumors in smokers of higher-yield cigarettes. METHODS: At two urban
      academic medical centers, we reviewed computed tomographic scans, chest
      radiographs, and medical records to assign tumor location (peripheral or
      central) for 330 smokers diagnosed with carcinoma of the lung between 1993
      and 1999. We compared the proportion of tumors in a peripheral versus
      central location by lifetime filter use and average lifetime tar rating (<
      21 and > or = 21 mg). RESULTS: Tumor location (69% peripheral and 31%
      central) was unrelated to cigarette filter use. Smokers of cigarettes with
      lower-tar ratings were more likely than those with higher ratings to have
      peripheral rather than central tumors (odds ratio, 1.76; 95% confidence
      interval, 0.89-3.47). When restricted to subjects with adenocarcinoma or
      squamous cell carcinoma, the odds ratio (95% confidence interval) was 2.31
      (1.05-5.08). CONCLUSIONS: Among cigarette smokers with lung cancer, use of
      cigarettes with lower-tar yield was associated with preferential
      occurrence of tumors in peripheral sites. Our findings support the
      hypothesis that changes in smoking associated with lower-tar cigarettes
      have led to a shift in the location of smoking-related lung cancer.
AD  - Department of Epidemiology, Boston University School of Public Health,
      Boston, MA 02118, USA. danbrook@bu.edu
FAU - Brooks, Daniel R
AU  - Brooks DR
FAU - Austin, John H M
AU  - Austin JH
FAU - Heelan, Robert T
AU  - Heelan RT
FAU - Ginsberg, Michelle S
AU  - Ginsberg MS
FAU - Shin, Victor
AU  - Shin V
FAU - Olson, Sara H
AU  - Olson SH
FAU - Muscat, Joshua E
AU  - Muscat JE
FAU - Stellman, Steven D
AU  - Stellman SD
LA  - eng
GR  - CA-17613/CA/NCI
GR  - CA-68384/CA/NCI
PT  - Journal Article
PL  - United States
TA  - Cancer Epidemiol Biomarkers Prev
JID - 9200608
RN  - 0 (Tars)
SB  - IM
MH  - Aged
MH  - Case-Control Studies
MH  - Female
MH  - Humans
MH  - Lung Neoplasms/*etiology/*pathology
MH  - Male
MH  - Middle Aged
MH  - Odds Ratio
MH  - Research Support, N.I.H., Extramural
MH  - Research Support, U.S. Gov't, P.H.S.
MH  - Smoking/*adverse effects
MH  - Tars/*adverse effects/classification
MH  - Tomography, X-Ray Computed
EDAT- 2005/03/16 09:00
MHDA- 2005/07/26 09:00
AID - 14/3/576 [pii]
AID - 10.1158/1055-9965.EPI-04-0468 [doi]
PST - ppublish
SO  - Cancer Epidemiol Biomarkers Prev 2005 Mar;14(3):576-81.

";
            #endregion

            PublicationTypes ptc = new PublicationTypes(
                AppDomain.CurrentDomain.BaseDirectory + "\\Unit Tests\\TestPublicationTypes",
                "PublicationTypes.csv"
               );

            Publications mpr = new Publications(MedlineData, ptc);
            Assert.IsTrue(mpr.PublicationList.Length == 3);

            Publication p = mpr.PublicationList[0];
            Assert.AreEqual(p.PMID, 15904469);
            Assert.AreEqual(p.Year, 2005);
            Assert.IsTrue(p.Month == "May");
            Assert.IsTrue(p.Day == null);
            Assert.IsTrue(p.Title == "Charcoal cigarette filters and lung cancer risk in Aichi Prefecture, Japan.");
            Assert.IsTrue(p.Pages == "283-7");
            Assert.IsTrue(p.Journal == "Cancer Sci");
            Assert.IsTrue(p.Volume == "96");
            Assert.IsTrue(p.Issue == "5");
            Assert.IsTrue(p.Grants.Count == 2);
            Assert.IsTrue(p.Grants.Contains("CA-17613/CA/NCI"));
            Assert.IsTrue(p.Grants.Contains("CA-68387/CA/NCI"));
            Assert.IsTrue(p.PubType == "Clinical Trial");
            Assert.IsTrue(p.MeSHHeadings.Count == 17);
            Assert.IsTrue(p.MeSHHeadings.Contains("Adult"));
            Assert.IsTrue(p.MeSHHeadings.Contains("Smoking/*adverse effects"));
            Assert.IsTrue(p.Authors.Length == 4);
            Assert.IsTrue(p.Authors[0] == "Muscat JE");
            Assert.IsTrue(p.Authors[3] == "Stellman SD");

            p = mpr.PublicationList[2];
            Assert.AreEqual(p.PMID, 15767332);
            Assert.AreEqual(p.Year, 2005);
            Assert.IsTrue(p.Month == "Mar");
            Assert.IsTrue(p.Day == null);
            Assert.IsTrue(p.Title == "Influence of type of cigarette on peripheral versus central lung cancer.");
            Assert.IsTrue(p.Pages == "576-81");
            Assert.IsTrue(p.Journal == "Cancer Epidemiol Biomarkers Prev");
            Assert.IsTrue(p.Volume == "14");
            Assert.IsTrue(p.Issue == "3");
            Assert.IsTrue(p.Grants.Count == 2);
            Assert.IsTrue(p.Grants.Contains("CA-17613/CA/NCI"));
            Assert.IsTrue(p.Grants.Contains("CA-68384/CA/NCI"));
            Assert.IsTrue(p.PubType == "Journal Article");
            Assert.IsTrue(p.MeSHHeadings.Count == 13);
            Assert.IsTrue(p.MeSHHeadings.Contains("Aged"));
            Assert.IsTrue(p.MeSHHeadings.Contains("Tomography, X-Ray Computed"));
            Assert.IsTrue(p.Authors.Length == 8);
            Assert.IsTrue(p.Authors[0] == "Brooks DR");
            Assert.IsTrue(p.Authors[7] == "Stellman SD");
        }

        [Test]
        public void NullMedlineString()
        {
            PublicationTypes ptc = new PublicationTypes(
                AppDomain.CurrentDomain.BaseDirectory + "\\Unit Tests\\TestPublicationTypes",
                "PublicationTypes.csv"
               );
            Publications mpr = new Publications("", ptc);
            Assert.IsNull(mpr.PublicationList);
        }

        [Test]
        public void LeadingBlankLines()
        {
            #region Test HandGeneratedData
            string MedlineData = @"











PMID- 15856074
OWN - NLM
STAT- In-Data-Review
DA  - 20051118
PUBM- Print
IS  - 1053-4245
VI  - 15
IP  - 6
DP  - 2005 Nov
TI  - Residential environmental exposures and other characteristics associated
      with detectable PAH-DNA adducts in peripheral mononuclear cells in a
      population-based sample of adult females.
PG  - 482-90
AB  - The detection of polycyclic aromatic hydrocarbon (PAH)-DNA adducts in
      human lymphocytes may be useful as a surrogate end point for individual
      cancer risk prediction. In this study, we examined the relationship
      between environmental sources of residential PAH, as well as other
      potential factors that may confound their association with cancer risk,
      and the detection of PAH-DNA adducts in a large population-based sample of
      adult women. Adult female residents of Long Island, New York, aged at
      least 20 years were identified from the general population between August
      1996 and July 1997. Among 1556 women who completed a structured
      questionnaire, 941 donated sufficient blood (25+ ml) to allow use of a
      competitive ELISA for measurement of PAH-DNA adducts in peripheral blood
      mononuclear cells. Ambient PAH exposure at the current residence was
      estimated using geographic modeling (n=796). Environmental home samples of
      dust (n=356) and soil (n=360) were collected on a random subset of
      long-term residents (15+ years). Multivariable regression was conducted to
      obtain the best-fitting predictive models. Three separate models were
      constructed based on data from : (A) the questionnaire, including a
      dietary history; (B) environmental home samples; and (C) geographic
      modeling. Women who donated blood in summer and fall had increased odds of
      detectable PAH-DNA adducts (OR=2.65, 95% confidence interval (CI)=1.69,
      4.17; OR=1.59, 95% CI=1.08, 2.32, respectively), as did current and past
      smokers (OR=1.50, 95% CI=1.00, 2.24; OR=1.46, 95% CI=1.05, 2.02,
      respectively). There were inconsistent associations between detectable
      PAH-DNA adducts and other known sources of residential PAH, such as
      grilled and smoked foods, or a summary measure of total dietary
      benzo-[a]-pyrene (BaP) intake during the year prior to the interview.
      Detectable PAH-DNA adducts were inversely associated with increased BaP
      levels in dust in the home, but positively associated with BaP levels in
      soil outside of the home, although CIs were wide. Ambient BaP estimates
      from the geographic model were not associated with detectable PAH-DNA
      adducts. These data suggest that PAH-DNA adducts detected in a
      population-based sample of adult women with ambient exposure levels
      reflect some key residential PAH exposure sources assessed in this study,
      such as cigarette smoking.Journal of Exposure Analysis and Environmental
      Epidemiology (2005) 15, 482-490. doi:10.1038/sj.jea.7500426; published
      online 27 April 2005.
AD  - aDepartment of Epidemiology, CB#7435 McGavran-Greenberg Hall, University
      of North Carolina School of Public Health, Chapel Hill, North Carolina
      27599-7435, USA.
FAU - Shantakumar, Sumitra
AU  - Shantakumar S
FAU - Gammon, Marilie D
AU  - Gammon MD
FAU - Eng, Sybil M
AU  - Eng SM
FAU - Sagiv, Sharon K
AU  - Sagiv SK
FAU - Gaudet, Mia M
AU  - Gaudet MM
FAU - Teitelbaum, Susan L
AU  - Teitelbaum SL
FAU - Britton, Julie A
AU  - Britton JA
FAU - Terry, Mary Beth
AU  - Terry MB
FAU - Paykin, Andrea
AU  - Paykin A
FAU - Young, Tie Lan
AU  - Young TL
FAU - Wang, Lian Wen
AU  - Wang LW
FAU - Wang, Qiao
AU  - Wang Q
FAU - Stellman, Steven D
AU  - Stellman SD
FAU - Beyea, Jan
AU  - Beyea J
FAU - Hatch, Maureen
AU  - Hatch M
FAU - Camann, David
AU  - Camann D
FAU - Prokopczyk, Bogdan
AU  - Prokopczyk B
FAU - Kabat, Geoffrey C
AU  - Kabat GC
FAU - Levin, Bruce
AU  - Levin B
FAU - Neugut, Alfred I
AU  - Neugut AI
FAU - Santella, Regina M
AU  - Santella RM
LA  - eng
PT  - Journal Article
PL  - England
TA  - J Expo Anal Environ Epidemiol
JID - 9111438
SB  - IM
EDAT- 2005/04/28 09:00
MHDA- 2005/04/28 09:00
AID - 7500426 [pii]
AID - 10.1038/sj.jea.7500426 [doi]
PST - ppublish
SO  - J Expo Anal Environ Epidemiol 2005 Nov;15(6):482-90.";
            #endregion

            PublicationTypes ptc = new PublicationTypes(
                AppDomain.CurrentDomain.BaseDirectory + "\\Unit Tests\\TestPublicationTypes",
                "PublicationTypes.csv"
               );
            Publications mpr = new Publications(MedlineData, ptc);
            Assert.IsTrue(mpr.PublicationList.Length == 1);
            Publication p = mpr.PublicationList[0];
            Assert.AreEqual(p.PMID, 15856074);
            Assert.AreEqual(p.Year, 2005);
            Assert.IsTrue(p.Month == "Nov");
            Assert.IsNull(p.Day);
            Assert.IsTrue(p.Title == "Residential environmental exposures and other characteristics associated with detectable PAH-DNA adducts in peripheral mononuclear cells in a population-based sample of adult females.");
            Assert.IsTrue(p.Pages == "482-90");
            Assert.IsTrue(p.Journal == "J Expo Anal Environ Epidemiol");
            Assert.IsTrue(p.Volume == "15");
            Assert.IsTrue(p.Issue == "6");
            Assert.IsNull(p.Grants);
            Assert.IsTrue(p.PubType == "Journal Article");
            Assert.IsNull(p.MeSHHeadings);
            Assert.IsTrue(p.Authors.Length == 21);
            Assert.IsTrue(p.Authors[0] == "Shantakumar S");
            Assert.IsTrue(p.Authors[20] == "Santella RM");

        }

    }


}
