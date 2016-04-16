-- MySQL dump 10.10
--
-- Host: localhost    Database: social_unit_test_firstdegree
-- ------------------------------------------------------
-- Server version	5.0.22-community-nt

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `colleaguematches`
--

DROP TABLE IF EXISTS `colleaguematches`;
CREATE TABLE `colleaguematches` (
  `Setnb` char(8) NOT NULL,
  `StarSetnb` char(8) NOT NULL,
  `PMID` int(11) NOT NULL,
  `MatchName` varchar(40) NOT NULL,
  KEY `index_setnb` (`Setnb`),
  KEY `index_starsetnb` (`StarSetnb`),
  KEY `index_pmid` (`PMID`),
  KEY `index_matchname` (`MatchName`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

--
-- Dumping data for table `colleaguematches`
--


/*!40000 ALTER TABLE `colleaguematches` DISABLE KEYS */;
LOCK TABLES `colleaguematches` WRITE;
UNLOCK TABLES;
/*!40000 ALTER TABLE `colleaguematches` ENABLE KEYS */;

--
-- Table structure for table `colleaguepublications`
--

DROP TABLE IF EXISTS `colleaguepublications`;
CREATE TABLE `colleaguepublications` (
  `Setnb` char(8) NOT NULL,
  `PMID` int(11) NOT NULL,
  `AuthorPosition` int(11) NOT NULL,
  `PositionType` tinyint(4) NOT NULL,
  PRIMARY KEY  (`Setnb`,`PMID`),
  KEY `index_setnb` (`Setnb`),
  KEY `index_pmid` (`PMID`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

--
-- Dumping data for table `colleaguepublications`
--


/*!40000 ALTER TABLE `colleaguepublications` DISABLE KEYS */;
LOCK TABLES `colleaguepublications` WRITE;
INSERT INTO `colleaguepublications` VALUES ('Alice',19910001,2,2),('Alice',19930001,1,1),('Alice',19950002,2,2),('Alice',20020002,1,1),('Alice',20030002,1,1),('Alice',19950003,3,2),('Alice',19940002,1,1),('Alice',19940003,1,1),('Alice',19940004,2,2),('Alice',19940005,3,2),('Alice',19940006,3,2),('Alice',19910002,1,1),('Alice',19910003,1,1),('Alice',19930002,1,1),('Alice',19930003,1,1),('Alice',19930004,1,1);
UNLOCK TABLES;
/*!40000 ALTER TABLE `colleaguepublications` ENABLE KEYS */;

--
-- Table structure for table `colleagues`
--

DROP TABLE IF EXISTS `colleagues`;
CREATE TABLE `colleagues` (
  `Setnb` char(8) NOT NULL,
  `First` varchar(20) default NULL,
  `Middle` varchar(20) default NULL,
  `Last` varchar(20) default NULL,
  `Name1` varchar(36) NOT NULL,
  `Name2` varchar(36) default NULL,
  `Name3` varchar(36) default NULL,
  `Name4` varchar(36) default NULL,
  `MedlineSearch` varchar(10000) NOT NULL,
  `Harvested` bit(1) NOT NULL default '\0',
  `Error` bit(1) default NULL,
  `ErrorMessage` varchar(512) default NULL,
  PRIMARY KEY  (`Setnb`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

--
-- Dumping data for table `colleagues`
--


/*!40000 ALTER TABLE `colleagues` DISABLE KEYS */;
LOCK TABLES `colleagues` WRITE;
INSERT INTO `colleagues` VALUES ('Alice','Alice','','Anderson','alice a',NULL,NULL,NULL,'alice a[au]','','\0','\r');
UNLOCK TABLES;
/*!40000 ALTER TABLE `colleagues` ENABLE KEYS */;

--
-- Table structure for table `meshheadings`
--

DROP TABLE IF EXISTS `meshheadings`;
CREATE TABLE `meshheadings` (
  `ID` int(11) NOT NULL auto_increment,
  `Heading` varchar(255) NOT NULL,
  PRIMARY KEY  (`ID`),
  KEY `index_heading` (`Heading`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

--
-- Dumping data for table `meshheadings`
--


/*!40000 ALTER TABLE `meshheadings` DISABLE KEYS */;
LOCK TABLES `meshheadings` WRITE;
UNLOCK TABLES;
/*!40000 ALTER TABLE `meshheadings` ENABLE KEYS */;

--
-- Table structure for table `people`
--

DROP TABLE IF EXISTS `people`;
CREATE TABLE `people` (
  `Setnb` char(8) NOT NULL,
  `First` varchar(20) default NULL,
  `Middle` varchar(20) default NULL,
  `Last` varchar(20) default NULL,
  `Name1` varchar(36) NOT NULL,
  `Name2` varchar(36) default NULL,
  `Name3` varchar(36) default NULL,
  `Name4` varchar(36) default NULL,
  `MedlineSearch` varchar(10000) NOT NULL,
  `Harvested` bit(1) NOT NULL default '\0',
  `Error` bit(1) default NULL,
  `ErrorMessage` varchar(512) default NULL,
  PRIMARY KEY  (`Setnb`),
  KEY `Setnb` (`Setnb`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

--
-- Dumping data for table `people`
--


/*!40000 ALTER TABLE `people` DISABLE KEYS */;
LOCK TABLES `people` WRITE;
INSERT INTO `people` VALUES ('Carol','Carol','','Test','Carol T',NULL,NULL,NULL,'\"carol t\"[au]','',NULL,'N\r'),('Justin','Justin','First','Degree','Justin FD','Justin F',NULL,NULL,'\"justin f\"[au] OR \"justin fd\"[au]','',NULL,'N\r');
UNLOCK TABLES;
/*!40000 ALTER TABLE `people` ENABLE KEYS */;

--
-- Table structure for table `peoplepublications`
--

DROP TABLE IF EXISTS `peoplepublications`;
CREATE TABLE `peoplepublications` (
  `Setnb` char(8) NOT NULL,
  `PMID` int(11) NOT NULL,
  `AuthorPosition` int(11) NOT NULL,
  `PositionType` tinyint(4) NOT NULL,
  PRIMARY KEY  (`Setnb`,`PMID`),
  KEY `index_setnb` (`Setnb`),
  KEY `index_pmid` (`PMID`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

--
-- Dumping data for table `peoplepublications`
--


/*!40000 ALTER TABLE `peoplepublications` DISABLE KEYS */;
LOCK TABLES `peoplepublications` WRITE;
INSERT INTO `peoplepublications` VALUES ('Carol',19920001,1,1),('Carol',19890001,1,1),('Carol',19960001,1,1),('Carol',19970001,1,1),('Carol',19990001,2,2),('Carol',19850001,2,2),('Carol',19950001,1,1),('Carol',19910001,1,1),('Carol',19930001,3,2),('Carol',19950003,1,1),('Carol',19940002,2,3),('Justin',19950002,1,1),('Justin',19940001,3,2),('Justin',19980001,2,2),('Justin',20000001,3,2),('Justin',20050001,1,1),('Justin',20010001,2,2),('Justin',20020001,1,1),('Justin',20030001,1,1),('Justin',19960002,2,2),('Justin',19990002,3,2),('Justin',20050002,1,1),('Justin',20030002,2,3),('Justin',19940002,3,2),('Carol',19940003,2,2),('Carol',19940004,1,1),('Carol',19940005,1,1),('Carol',19990003,2,2),('Carol',19990004,1,1),('Justin',20010002,1,1),('Justin',20010003,2,2),('Justin',19940006,1,1),('Carol',19940006,2,3),('Justin',20050003,1,1),('Justin',20010004,1,1),('Justin',20010005,1,1),('Carol',19950004,1,1),('Carol',19950005,1,1),('Justin',20010006,1,1);
UNLOCK TABLES;
/*!40000 ALTER TABLE `peoplepublications` ENABLE KEYS */;

--
-- Table structure for table `publicationauthors`
--

DROP TABLE IF EXISTS `publicationauthors`;
CREATE TABLE `publicationauthors` (
  `PMID` int(11) NOT NULL,
  `Position` int(11) NOT NULL,
  `Author` varchar(70) NOT NULL,
  `First` tinyint(4) NOT NULL,
  `Last` tinyint(4) NOT NULL,
  PRIMARY KEY  (`PMID`,`Position`),
  KEY `index_pmid` (`PMID`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

--
-- Dumping data for table `publicationauthors`
--


/*!40000 ALTER TABLE `publicationauthors` DISABLE KEYS */;
LOCK TABLES `publicationauthors` WRITE;
INSERT INTO `publicationauthors` VALUES (19920001,1,'Carol T',1,0),(19920001,2,'Mallory M',0,0),(19920001,3,'Joe J',0,1),(19890001,1,'Carol T',1,0),(19890001,2,'Joe J',0,1),(19960001,1,'Carol T',1,0),(19960001,2,'Lisa L',0,1),(19970001,1,'Carol T',1,0),(19970001,2,'Other O',0,0),(19970001,3,'Lisa L',0,1),(19990001,1,'Lisa L',1,0),(19990001,2,'Carol T',0,1),(19850001,1,'Effie E',1,0),(19850001,2,'Carol T',0,1),(19950001,1,'Carol T',1,0),(19950001,2,'Other O',0,0),(19950001,3,'Effie E',0,1),(19910001,1,'Carol T',1,0),(19910001,2,'Alice A',0,1),(19930001,1,'Alice A',1,0),(19930001,2,'Other O',0,0),(19930001,3,'Carol T',0,1),(19950002,1,'Justin J',1,0),(19950002,2,'Alice A',0,1),(19940001,1,'Effie E',1,0),(19940001,2,'Other O',0,0),(19940001,3,'Justin J',0,1),(19980001,1,'Constance C',1,0),(19980001,2,'Justin J',0,1),(20000001,1,'Constance C',1,0),(20000001,2,'Other O',0,0),(20000001,3,'Justin J',0,1),(20050001,1,'Justin J',1,0),(20050001,2,'Constance C',0,1),(20010001,1,'Roger R',1,0),(20010001,2,'Justin J',0,1),(20020001,1,'Justin J',1,0),(20020001,2,'Other O',0,0),(20020001,3,'Roger R',0,1),(20030001,1,'Justin J',1,0),(20030001,2,'Roger R',0,1),(19960002,1,'John J',1,0),(19960002,2,'Justin J',0,1),(19990002,1,'John J',1,0),(19990002,2,'Other O',0,0),(19990002,3,'Justin J',0,1),(20050002,1,'Justin J',1,0),(20050002,2,'John J',0,1),(20020002,1,'Alice A',1,0),(20020002,2,'John J',0,1),(20030002,1,'Alice A',1,0),(20030002,2,'Justin J',0,0),(20030002,3,'John J',0,1),(19950003,1,'Carol T',1,0),(19950003,2,'Joe J',0,0),(19950003,3,'Alice A',0,1),(19940002,1,'Alice A',1,0),(19940002,2,'Carol T',0,0),(19940002,3,'Justin J',0,1),(19940003,1,'Alice A',1,0),(19940003,2,'Carol T',0,1),(19940004,1,'Carol T',1,0),(19940004,2,'Alice A',0,1),(19940005,1,'Carol T',1,0),(19940005,2,'Other O',0,0),(19940005,3,'Alice A',0,1),(19990003,1,'Lisa L',1,0),(19990003,2,'Carol T',0,1),(19990004,1,'Carol T',1,0),(19990004,2,'Other O',0,0),(19990004,3,'Lisa L',0,1),(20010002,1,'Justin J',1,0),(20010002,2,'Roger R',0,1),(20010003,1,'Roger R',1,0),(20010003,2,'Justin J',0,1),(19940006,1,'Justin J',1,0),(19940006,2,'Carol T',0,0),(19940006,3,'Alice A',0,1),(20050003,1,'Justin J',1,0),(20050003,2,'John J',0,0),(20050003,3,'Constance C',0,1),(19910002,1,'Alice A',1,1),(19910003,1,'Alice A',1,1),(19930002,1,'Alice A',1,1),(19930003,1,'Alice A',1,1),(19930004,1,'Alice A',1,1),(20010004,1,'Justin J',1,1),(20010005,1,'Justin J',1,1),(19950004,1,'Carol T',1,1),(19950005,1,'Carol T',1,1),(20010006,1,'Justin J',1,1);
UNLOCK TABLES;
/*!40000 ALTER TABLE `publicationauthors` ENABLE KEYS */;

--
-- Table structure for table `publicationgrants`
--

DROP TABLE IF EXISTS `publicationgrants`;
CREATE TABLE `publicationgrants` (
  `PMID` int(11) NOT NULL,
  `GrantID` varchar(50) NOT NULL,
  PRIMARY KEY  (`PMID`,`GrantID`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

--
-- Dumping data for table `publicationgrants`
--


/*!40000 ALTER TABLE `publicationgrants` DISABLE KEYS */;
LOCK TABLES `publicationgrants` WRITE;
UNLOCK TABLES;
/*!40000 ALTER TABLE `publicationgrants` ENABLE KEYS */;

--
-- Table structure for table `publicationmeshheadings`
--

DROP TABLE IF EXISTS `publicationmeshheadings`;
CREATE TABLE `publicationmeshheadings` (
  `PMID` int(11) NOT NULL,
  `MeSHHEadingID` int(11) NOT NULL,
  PRIMARY KEY  (`PMID`,`MeSHHEadingID`),
  KEY `index_pmid` (`PMID`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

--
-- Dumping data for table `publicationmeshheadings`
--


/*!40000 ALTER TABLE `publicationmeshheadings` DISABLE KEYS */;
LOCK TABLES `publicationmeshheadings` WRITE;
UNLOCK TABLES;
/*!40000 ALTER TABLE `publicationmeshheadings` ENABLE KEYS */;

--
-- Table structure for table `publications`
--

DROP TABLE IF EXISTS `publications`;
CREATE TABLE `publications` (
  `PMID` int(11) NOT NULL,
  `Journal` varchar(128) default NULL,
  `Year` int(11) NOT NULL,
  `Authors` int(11) default NULL,
  `Month` varchar(32) default NULL,
  `Day` varchar(32) default NULL,
  `Title` varchar(244) default NULL,
  `Volume` varchar(32) default NULL,
  `Issue` varchar(32) default NULL,
  `Pages` varchar(50) default NULL,
  `PubType` varchar(50) NOT NULL,
  `PubTypeCategoryID` tinyint(4) NOT NULL,
  PRIMARY KEY  (`PMID`),
  KEY `index_pmid` (`PMID`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

--
-- Dumping data for table `publications`
--


/*!40000 ALTER TABLE `publications` DISABLE KEYS */;
LOCK TABLES `publications` WRITE;
INSERT INTO `publications` VALUES (19920001,'J Comput Assist Tomogr',1992,3,'Aug','','NMR imaging of intracranial hemorrhage.','8','4','599-607','Journal Article',3),(19890001,'Radiology',1989,2,'Apr','','Potential hazards and artifacts of ferromagnetic and nonferromagnetic surgical and dental materials and devices in nuclear magnetic resonance imaging.','147','1','139-48','Journal Article',3),(19960001,'J Comput Assist Tomogr',1996,2,'Feb','','Nuclear magnetic resonance (NMR) imaging of Arnold-Chiari type I malformation with hydromyelia.','7','1','126-9','Case Reports',1),(19970001,'AJNR Am J Neuroradiol',1997,3,'Mar-Apr','','MR imaging of pituitary adenomas using a prototype resistive magnet: preliminary assessment.','5','2','131-7','Journal Article',3),(19990001,'Phys Med Biol',1999,2,'Jun','','Measurement of spin-lattice relaxation times in nuclear magnetic resonance imaging.','28','6','723-9','Journal Article',3),(19850001,'Arch Neurol',1985,2,'Jul','','\"Diffusion-weighted magnetic resonance imaging identifies the \"\"clinically relevant\"\" small-penetrator infarcts.\"','57','7','1009-14','Clinical Trial',1),(19950001,'Brain Cogn',1995,3,'Jul','','Positron brain imaging--normal patterns and asymmetries.','1','3','286-93','Journal Article',3),(19910001,'Trans Am Neurol Assoc',1991,2,'','','Positron imaging of the normal brain--regional patterns of cerebral blood flow and metabolism.','105','','10-Aug','Journal Article',3),(19930001,'AJR Am J Roentgenol',1993,3,'Jul','','Evaluation of multiplanar reconstruction in CT recognition of lumbar disk disease.','143','1','169-76','Journal Article',3),(19950002,'Acta Radiol Diagn (Stockh)',1995,2,'May','','INFLUENCE OF VENTRICULAR SIZE ON MORTALITY AND MORBIDITY FOLLOWING  VENTRICULOGRAPHY.','11','','602-8','Journal Article',3),(19940001,'AJNR Am J Neuroradiol',1994,3,'Aug','','Hypoattenuation on CT angiographic source images predicts risk of intracerebral hemorrhage and outcome after intra-arterial reperfusion therapy.','26','7','1798-803','Journal Article',3),(19980001,'Crit Care Med',1998,2,'Jul','','Relationship between hyperglycemia and symptomatic vasospasm after subarachnoid hemorrhage.','33','7','1603-9 quiz 1623','Journal Article',3),(20000001,'Stroke',2000,3,'Apr','','A pilot study of normobaric oxygen therapy in acute ischemic stroke.','36','4','797-802','Clinical Trial',1),(20050001,'J Neuroimaging',2005,2,'Oct','','Contrast M-mode power Doppler ultrasound in the detection of right-to-left shunts: utility of submandibular internal carotid artery recording.','13','4','315-23','Journal Article',3),(20010001,'J Neuroimaging',2001,2,'Jul','','Importance of jugular valve incompetence in contrast transcranial Doppler ultrasonography for the diagnosis of patent foramen ovale.','13','3','272-5','Case Reports',1),(20020001,'J Neurosurg',2002,3,'Jun','','Subarachnoid hemorrhage without evident cause on initial angiography studies: diagnostic yield of subsequent angiography and other neuroimaging tests.','98','6','1235-40','Journal Article',3),(20030001,'Radiology',2003,2,'Jun','','Whole-brain CT perfusion measurement of perfused cerebral blood volume in acute ischemic stroke: probability curve for regional infarction.','227','3','725-30','Journal Article',3),(19960002,'J Neurol Neurosurg Psychiatry',1996,2,'Apr','','Middle cerebral artery territory infarction sparing the precentral gyrus: report of three cases.','74','4','510-2','Case Reports',1),(19990002,'J Neuroimaging',1999,3,'Jan','','Diffusion-weighted magnetic resonance imaging abnormalities in Bartonella encephalopathy.','13','1','79-82','Case Reports',1),(20050002,'Cerebrovasc Dis',2005,2,'','','\'Footprints\' of transient ischemic attacks: a diffusion-weighted MRI study.','14','4-Mar','177-86','Journal Article',3),(20020002,'Stroke',2002,2,'May','','Acute ischemic stroke patterns in infective and nonbacterial thrombotic endocarditis: a diffusion-weighted magnetic resonance imaging study.','33','5','1267-73','Journal Article',3),(20030002,'Curr Opin Neurol',2003,3,'Apr','','Arterial ischemic stroke in childhood: the role of plasma-phase risk factors.','15','2','139-44','Review',2),(19950003,'J Neuroimaging',1995,3,'Oct','','Cerebral infarction in conjunction with patent foramen ovale and May-Thurner syndrome.','11','4','432-4','Case Reports',1),(19940002,'Stroke',1994,3,'Sep','','Utility of perfusion-weighted CT imaging in acute middle cerebral artery stroke treated with intra-arterial thrombolysis: prediction of final infarct volume and clinical outcome.','32','9','2021-8','Journal Article',3),(19940003,'Prev Med',1994,2,'Oct',NULL,'Ernst Wynder: a remembrance.','43','4','239-45','Journal Article',3),(19940004,'Prev Med',1994,2,'Oct',NULL,'Ernst Wynder: citation analysis.','43','4','268-70','Case Reports',1),(19940005,'Environ Health Perspect',1994,3,'Jul',NULL,'Validation and calibration of a model used to reconstruct historical exposure to polycyclic aromatic hydrocarbons for use in epidemiologic studies.','114','7','1053-8','Journal Article',3),(19990003,'Arch Environ Health',1999,2,'Dec',NULL,'Polycyclic aromatic hydrocarbon-DNA adducts and breast cancer: a pooled analysis.','59','12','640-9','Journal Article',3),(19990004,'Cancer Sci',1999,3,'May',NULL,'\"Charcoal cigarette filters and lung cancer risk in Aichi Prefecture, Japan.\"','96','5','283-7','Journal Article',3),(20010002,'J Expo Anal Environ Epidemiol',2001,2,'Nov',NULL,'Residential environmental exposures and other characteristics associated with detectable PAH-DNA adducts in peripheral mononuclear cells in a population-based sample of adult females.','15','6','482-90','Journal Article',3),(20010003,'Cancer Epidemiol Biomarkers Prev',2001,3,'Mar',NULL,'Influence of type of cigarette on peripheral versus central lung cancer.','14','3','576-81','Journal Article',3),(19940006,'Tob Control',1994,6,'Feb',NULL,'An extremely compensatible cigarette by design: documentary evidence on industry awareness and reactions to the Barclay filter design cheating the tar testing system.','14','1','64-70','Journal Article',3),(20050003,'Cancer',2005,3,'Apr','1','Racial differences in exposure and glucuronidation of the tobacco-specific carcinogen 4-(methylnitrosamino)-1-(3-pyridyl)-1-butanone (NNK).','103','7','1420-6','Journal Article',3),(19910002,'J Expo Anal Environ Epidemiol',1991,1,'Jul',NULL,'\"Exposure opportunity models for Agent Orange, dioxin, and other military herbicides used in Vietnam, 1961-1971.\"','14','4','354-62','Journal Article',3),(19910003,'Cancer Epidemiol Biomarkers Prev',1991,1,'Dec',NULL,'\"Adipose concentrations of organochlorine compounds and breast cancer recurrence in Long Island, New York.\"','12','12','1474-8','Journal Article',3),(19930002,'J Consult Clin Psychol',1993,1,'Dec',NULL,'Risk factors for course of posttraumatic stress disorder among Vietnam veterans: a 14-year follow-up of American Legionnaires.','71','6','980-6','Journal Article',3),(19930003,'Ann Epidemiol',1993,1,'Apr',NULL,'Lung cancer risk in white and black Americans.','13','4','294-302','Journal Article',3),(19930004,'Cancer',1993,1,'Apr','1','Risk of lung carcinoma among users of nonsteroidal antiinflammatory drugs.','97','7','1732-6','Journal Article',3),(12611661,'Environ Health Perspect',2003,6,'Mar',NULL,'A geographic information system for characterizing exposure to Agent Orange and other herbicides in Vietnam.','111','3','321-8','Journal Article',3),(12432163,'Tob Control',2002,3,'Dec',NULL,'Mentholated cigarettes and smoking habits in whites and blacks.','11','4','368-71','Journal Article',3),(12206514,'Breast Cancer Res Treat',2002,29,'Jun',NULL,'The Long Island Breast Cancer Study Project: description of a multi-institutional collaboration to identify environmental risk factors for breast cancer.','74','3','235-54','Journal Article',3),(12163320,'Cancer Epidemiol Biomarkers Prev',2002,26,'Aug',NULL,'Environmental toxins and breast cancer on Long Island. II. Organochlorine compound levels in blood.','11','8','686-97','Case Reports',1),(12163319,'Cancer Epidemiol Biomarkers Prev',2002,27,'Aug',NULL,'Environmental toxins and breast cancer on Long Island. I. Polycyclic aromatic hydrocarbon DNA adducts.','11','8','677-85','Journal Article',3),(11971109,'Neurology',2002,7,'Apr','23','Handheld cellular telephones and risk of acoustic neuroma.','58','8','1304-6','Journal Article',3),(11700268,'Cancer Epidemiol Biomarkers Prev',2001,13,'Nov',NULL,'Smoking and lung cancer risk in American and Japanese men: an international case-control study.','10','11','1193-9','Journal Article',3),(10750675,'Cancer Epidemiol Biomarkers Prev',2000,6,'Mar',NULL,'Role of polymorphisms in codons 143 and 160 of the O6-alkylguanine DNA alkyltransferase gene in lung cancer risk.','9','3','339-42','Case Reports',1),(10639511,'J Natl Cancer Inst',2000,3,'Jan','19','Doses of nicotine and lung carcinogens delivered to cigarette smokers.','92','2','106-11','Journal Article',3),(9698992,'Am J Ind Med',1998,4,'Sep',NULL,'Nonmalignant respiratory disease mortality among woodworkers participating in the American Cancer Society Cancer Prevention Study-II (CPS-II).','34','3','238-43','Journal Article',3);
UNLOCK TABLES;
/*!40000 ALTER TABLE `publications` ENABLE KEYS */;

--
-- Table structure for table `pubtypecategories`
--

DROP TABLE IF EXISTS `pubtypecategories`;
CREATE TABLE `pubtypecategories` (
  `PublicationType` varchar(90) NOT NULL,
  `PubTypeCategoryID` tinyint(4) NOT NULL,
  `OverrideFirstCategory` tinyint(1) default '0',
  PRIMARY KEY  (`PublicationType`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

--
-- Dumping data for table `pubtypecategories`
--


/*!40000 ALTER TABLE `pubtypecategories` DISABLE KEYS */;
LOCK TABLES `pubtypecategories` WRITE;
INSERT INTO `pubtypecategories` VALUES ('Case Reports',1,0),('Clinical Trial',1,0),('Review',2,0),('Journal Article',3,0);
UNLOCK TABLES;
/*!40000 ALTER TABLE `pubtypecategories` ENABLE KEYS */;

--
-- Table structure for table `starcolleagues`
--

DROP TABLE IF EXISTS `starcolleagues`;
CREATE TABLE `starcolleagues` (
  `StarSetnb` char(8) NOT NULL,
  `Setnb` char(8) NOT NULL,
  PRIMARY KEY  (`StarSetnb`,`Setnb`),
  KEY `Setnb` (`Setnb`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

--
-- Dumping data for table `starcolleagues`
--


/*!40000 ALTER TABLE `starcolleagues` DISABLE KEYS */;
LOCK TABLES `starcolleagues` WRITE;
INSERT INTO `starcolleagues` VALUES ('Carol','Alice'),('Justin','Alice');
UNLOCK TABLES;
/*!40000 ALTER TABLE `starcolleagues` ENABLE KEYS */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

