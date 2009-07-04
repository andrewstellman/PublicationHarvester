-- This file was generated with MySQL dump, and then modified by hand
-- to hard-code the CGUnitTestCopyPublications table.

DROP DATABASE IF EXISTS CGUnitTestCopyPublications;
CREATE DATABASE CGUnitTestCopyPublications;

-- ------------------------------------------------------


-- MySQL dump 10.10
--
-- Host: localhost    Database: publicationharvester
-- ------------------------------------------------------
-- Server version	5.0.18-nt

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
-- Table structure for table `meshheadings`
--

DROP TABLE IF EXISTS `CGUnitTestCopyPublications`.`meshheadings`;
CREATE TABLE `CGUnitTestCopyPublications`.`meshheadings` (
  `ID` int(11) NOT NULL auto_increment,
  `Heading` varchar(255) NOT NULL,
  PRIMARY KEY  (`ID`),
  KEY `index_heading` (`Heading`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Dumping data for table `meshheadings`
--


/*!40000 ALTER TABLE `CGUnitTestCopyPublications`.`meshheadings` DISABLE KEYS */;
LOCK TABLES `CGUnitTestCopyPublications`.`meshheadings` WRITE;
INSERT INTO `CGUnitTestCopyPublications`.`meshheadings` VALUES (1,'Abnormalities/*epidemiology'),(2,'*Birth Certificates'),(3,'Female'),(4,'Hawaii'),(5,'Humans'),(6,'Infant, Newborn'),(7,'Male'),(8,'*Medical Records'),(9,'Pilot Projects'),(10,'Pregnancy'),(11,'Child'),(12,'*Child Health Services'),(13,'Infant'),(14,'*Regional Medical Programs'),(15,'Respiratory Tract Diseases/*therapy'),(16,'Epidermal Necrolysis, Toxic/*drug therapy'),(17,'Lincomycin/*therapeutic use'),(18,'Prednisolone/*therapeutic use'),(19,'*Child'),(20,'*Diagnosis'),(21,'*Infant'),(22,'*Klippel-Feil Syndrome'),(23,'*Mental Retardation'),(24,'*Gastric Juice'),(25,'*Glucagon'),(26,'*Histamine'),(27,'*Pharmacology'),(28,'*Phenoxybenzamine'),(29,'*Pyloric Stenosis'),(30,'*Rats'),(31,'*Vagotomy');
UNLOCK TABLES;
/*!40000 ALTER TABLE `CGUnitTestCopyPublications`.`meshheadings` ENABLE KEYS */;

--
-- Table structure for table `people`
--

DROP TABLE IF EXISTS `CGUnitTestCopyPublications`.`people`;
CREATE TABLE `CGUnitTestCopyPublications`.`people` (
  `Setnb` char(8) NOT NULL,
  `First` varchar(20) default NULL,
  `Middle` varchar(20) default NULL,
  `Last` varchar(20) default NULL,
  `Name1` varchar(36) NOT NULL,
  `Name2` varchar(36) default NULL,
  `Name3` varchar(36) default NULL,
  `Name4` varchar(36) default NULL,
  `MedlineSearch` varchar(512) NOT NULL,
  `Harvested` bit(1) NOT NULL default '\0',
  `Error` bit(1) default NULL,
  `ErrorMessage` varchar(512) default NULL,
  PRIMARY KEY  (`Setnb`),
  KEY `Setnb` (`Setnb`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Dumping data for table `people`
--


/*!40000 ALTER TABLE `CGUnitTestCopyPublications`.`people` DISABLE KEYS */;
LOCK TABLES `CGUnitTestCopyPublications`.`people` WRITE;
INSERT INTO `CGUnitTestCopyPublications`.`people` VALUES ('A4800524','PAUL','A.','BUNN','bunn p jr','bunn pa jr','bunn pa','bunn p','((\"bunn pa jr\"[au] or \"bunn p jr\"[au]) or ((\"bunn p\"[au] or \"bunn pa\"[au]) and (lymphoma or cancer)) and 1970:2005[dp])','\0','\0',''),
                            ('A2700156','SHARON','J','BINTLIFF','bintliff sj',NULL,NULL,NULL,'\"bintliff sj\"[au]','','\0',''),
                            ('A1234567','FAKE','Q','FAKERSON','fakerson fq',NULL,NULL,NULL,'\"fakerson fq\"[au]','','\0','');
UNLOCK TABLES;
/*!40000 ALTER TABLE `CGUnitTestCopyPublications`.`people` ENABLE KEYS */;

--
-- Table structure for table `peoplepublications`
--

DROP TABLE IF EXISTS `CGUnitTestCopyPublications`.`peoplepublications`;
CREATE TABLE `CGUnitTestCopyPublications`.`peoplepublications` (
  `Setnb` char(8) NOT NULL,
  `PMID` int(11) NOT NULL,
  `AuthorPosition` int(11) NOT NULL,
  `PositionType` tinyint(4) NOT NULL,
  PRIMARY KEY  (`Setnb`,`PMID`),
  KEY `index_setnb` (`Setnb`),
  KEY `index_pmid` (`PMID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Dumping data for table `peoplepublications`
--


/*!40000 ALTER TABLE `CGUnitTestCopyPublications`.`peoplepublications` DISABLE KEYS */;
LOCK TABLES `CGUnitTestCopyPublications`.`peoplepublications` WRITE;
INSERT INTO `CGUnitTestCopyPublications`.`peoplepublications` VALUES ('A4800524',669939,1,1),('A4800524',5416017,2,5),('A2700156',5941109,1,1),('A2700156',14296906,1,1),('A2700156',14052000,1,1),('A1234567',98765,1,1);
UNLOCK TABLES;
/*!40000 ALTER TABLE `CGUnitTestCopyPublications`.`peoplepublications` ENABLE KEYS */;

--
-- Table structure for table `publicationauthors`
--

DROP TABLE IF EXISTS `CGUnitTestCopyPublications`.`publicationauthors`;
CREATE TABLE `CGUnitTestCopyPublications`.`publicationauthors` (
  `PMID` int(11) NOT NULL,
  `Position` int(11) NOT NULL,
  `Author` varchar(70) NOT NULL,
  `First` tinyint(4) NOT NULL,
  `Last` tinyint(4) NOT NULL,
  PRIMARY KEY  (`PMID`,`Position`),
  KEY `index_pmid` (`PMID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Dumping data for table `publicationauthors`
--


/*!40000 ALTER TABLE `CGUnitTestCopyPublications`.`publicationauthors` DISABLE KEYS */;
LOCK TABLES `CGUnitTestCopyPublications`.`publicationauthors` WRITE;
INSERT INTO `CGUnitTestCopyPublications`.`publicationauthors` VALUES (669939,1,'Bunn PA',1,0),(669939,2,'Hernandez DB',0,1),(5416017,1,'Shim WK',1,0),(5416017,2,'Bunn PA',0,0),(5416017,3,'Shirkey HC',0,1),(5941109,1,'Bintliff SJ',1,1),(14296906,1,'BINTLIFF SJ',1,1),(14052000,1,'BINTLIFF SJ',1,0),(14052000,2,'CONDON RE',0,0),(14052000,3,'HARKINS HN',0,1),(98765,1,'FAKERSON F',1,0),(98765,2,'MADEUPGUY J',0,1);
UNLOCK TABLES;
/*!40000 ALTER TABLE `CGUnitTestCopyPublications`.`publicationauthors` ENABLE KEYS */;

--
-- Table structure for table `publicationgrants`
--

DROP TABLE IF EXISTS `CGUnitTestCopyPublications`.`publicationgrants`;
CREATE TABLE `CGUnitTestCopyPublications`.`publicationgrants` (
  `PMID` int(11) NOT NULL,
  `GrantID` varchar(50) NOT NULL,
  PRIMARY KEY  (`PMID`,`GrantID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Dumping data for table `publicationgrants`
--


/*!40000 ALTER TABLE `CGUnitTestCopyPublications`.`publicationgrants` DISABLE KEYS */;
LOCK TABLES `CGUnitTestCopyPublications`.`publicationgrants` WRITE;
UNLOCK TABLES;
/*!40000 ALTER TABLE `CGUnitTestCopyPublications`.`publicationgrants` ENABLE KEYS */;

--
-- Table structure for table `publicationmeshheadings`
--

DROP TABLE IF EXISTS `CGUnitTestCopyPublications`.`publicationmeshheadings`;
CREATE TABLE `CGUnitTestCopyPublications`.`publicationmeshheadings` (
  `PMID` int(11) NOT NULL,
  `MeSHHEadingID` int(11) NOT NULL,
  PRIMARY KEY  (`PMID`,`MeSHHEadingID`),
  KEY `index_pmid` (`PMID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Dumping data for table `publicationmeshheadings`
--


/*!40000 ALTER TABLE `CGUnitTestCopyPublications`.`publicationmeshheadings` DISABLE KEYS */;
LOCK TABLES `CGUnitTestCopyPublications`.`publicationmeshheadings` WRITE;
INSERT INTO `CGUnitTestCopyPublications`.`publicationmeshheadings` VALUES (669939,1),(669939,2),(669939,3),(669939,4),(669939,5),(669939,6),(669939,7),(669939,8),(669939,9),(669939,10),(5416017,4),(5416017,5),(5416017,11),(5416017,12),(5416017,13),(5416017,14),(5416017,15),(5941109,3),(5941109,5),(5941109,13),(5941109,16),(5941109,17),(5941109,18),(14052000,24),(14052000,25),(14052000,26),(14052000,27),(14052000,28),(14052000,29),(14052000,30),(14052000,31),(14296906,19),(14296906,20),(14296906,21),(14296906,22),(14296906,23),(98765,2),(98765,5);
UNLOCK TABLES;
/*!40000 ALTER TABLE `CGUnitTestCopyPublications`.`publicationmeshheadings` ENABLE KEYS */;

--
-- Table structure for table `publications`
--

DROP TABLE IF EXISTS `CGUnitTestCopyPublications`.`publications`;
CREATE TABLE `CGUnitTestCopyPublications`.`publications` (
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
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Dumping data for table `publications`
--


/*!40000 ALTER TABLE `CGUnitTestCopyPublications`.`publications` DISABLE KEYS */;
LOCK TABLES `CGUnitTestCopyPublications`.`publications` WRITE;
INSERT INTO `CGUnitTestCopyPublications`.`publications` VALUES (669939,'New York Med J',1978,2,'Jun',NULL,'Under-reporting of birth defects in New York: a pilot study.','37','6','173-5','Journal Article',3),(5416017,'New York Med J',1970,3,'Jan-Feb',NULL,'The Mount Sinai Pediatric Pulmonary Center and Regional Pediatric Pulmonary Program.','29','3','203-4','Journal Article',3),(5941109,'Hawaii Med J',1966,1,'Jul-Aug',NULL,'Toxic epidermal necrolysis. A case report.','25','6','468-9','Case Reports',1),(14296906,'J Am Med Womens Assoc',1965,1,'Jun',NULL,'KLIPPEL-FEIL SYNDROME.','20',NULL,'547-50','Journal Article',3),(14052000,'J Surg Res',1963,3,'Aug',NULL,'EFFECT OF GLUCAGON ON GASTRIC SECRETION IN THE RAT WITH PYLORIC OCCLUSION.','56',NULL,'318-20','Journal Article',3),(98765,'J Fake Stuff',1991,13,'Aug',NULL,'Fake article.','56',NULL,'18-20','Journal Article',3);
UNLOCK TABLES;
/*!40000 ALTER TABLE `CGUnitTestCopyPublications`.`publications` ENABLE KEYS */;

--
-- Table structure for table `pubtypecategories`
--

DROP TABLE IF EXISTS `CGUnitTestCopyPublications`.`pubtypecategories`;
CREATE TABLE `CGUnitTestCopyPublications`.`pubtypecategories` (
  `PublicationType` varchar(90) NOT NULL,
  `PubTypeCategoryID` tinyint(4) NOT NULL,
  `OverrideFirstCategory` tinyint(1) default '0',
  PRIMARY KEY  (`PublicationType`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Dumping data for table `pubtypecategories`
--


/*!40000 ALTER TABLE `CGUnitTestCopyPublications`.`pubtypecategories` DISABLE KEYS */;
LOCK TABLES `CGUnitTestCopyPublications`.`pubtypecategories` WRITE;
INSERT INTO `CGUnitTestCopyPublications`.`pubtypecategories` VALUES ('Consensus Development Conference, NIH',1,0),('Technical Report',3,0),('Scientific Integrity Review',0,0),('Lectures',0,0),('Biography',0,0),('Newspaper Article',0,0),('Clinical Conference',1,0),('hrift',0,0),('Retracted Publication',0,0),('News',0,0),('Legal Cases',0,0),('Meta-Analysis',1,0),('Case Reports',1,0),('Retraction of Publication',0,0),('Clinical Trial, Phase IV',1,0),('Practice Guideline',1,0),('Government Publications',0,0),('Clinical Trial, Phase II',1,0),('Guideline',1,0),('Corrected and Republished Article',0,0),('Review, Tutorial',2,1),('Consensus Development Conference',1,0),('Comment',4,0),('Overall',0,0),('Festsc',0,0),('Dictionary',0,0),('Review of Reported Cases',2,1),('Letter',4,0),('Patient Education Handout',0,0),('Periodical Index',0,0),('Duplicate Publication',3,0),('Bibliography',0,0),('Review, Multicase',2,1),('Directory',0,0),('Addresses',0,0),('Evaluation Studies',3,0),('Multicenter Study',1,0),('Clinical Trial, Phase I',1,0),('Controlled Clinical Trial',1,0),('Clinical Trial',1,0),('Published Erratum',0,0),('Validation Studies',3,0),('Journal Article',3,0),('Editorial',4,0),('Twin Study',1,0),('Randomized Controlled Trial',1,0),('Interview',0,0),('Clinical Trial, Phase III',1,0),('Classical Article',3,0),('Review',2,1),('Historical Article',0,0),('Legislation',0,0);
UNLOCK TABLES;
/*!40000 ALTER TABLE `CGUnitTestCopyPublications`.`pubtypecategories` ENABLE KEYS */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;


-- EOF
