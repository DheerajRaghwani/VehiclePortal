-- MySQL dump 10.13  Distrib 8.0.17, for Win64 (x86_64)
--
-- Host: localhost    Database: vehicle
-- ------------------------------------------------------
-- Server version	8.0.17

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `block`
--

DROP TABLE IF EXISTS `block`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `block` (
  `BlockId` int(11) NOT NULL,
  `Blockname` varchar(50) DEFAULT NULL,
  `DistrictId` int(11) DEFAULT NULL,
  PRIMARY KEY (`BlockId`),
  KEY `DistrictId` (`DistrictId`),
  CONSTRAINT `block_ibfk_1` FOREIGN KEY (`DistrictId`) REFERENCES `district` (`DistrictId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `block`
--

LOCK TABLES `block` WRITE;
/*!40000 ALTER TABLE `block` DISABLE KEYS */;
INSERT INTO `block` VALUES (1,'Arang',1),(2,'Tilda',1),(3,'Abhanpur',1),(4,'Dharsiwa',1),(5,'Durg',3),(6,'Dhamdha',3),(7,'Patan',3),(8,'Dhamatari',2),(9,'Kurud',2),(10,'Magarlod',2),(11,'Nagari',2),(12,'Raipur',1);
/*!40000 ALTER TABLE `block` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `checkpost`
--

DROP TABLE IF EXISTS `checkpost`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `checkpost` (
  `Id` char(36) NOT NULL,
  `CheckpostId` int(11) NOT NULL,
  `VehicleNo` varchar(20) DEFAULT NULL,
  `DistrictId` int(11) DEFAULT NULL,
  `BlockId` int(11) DEFAULT NULL,
  `Pass` tinyint(1) DEFAULT NULL,
  `TotalPeople` int(11) DEFAULT NULL,
  `CurrentDate` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `VehicleNo_UNIQUE` (`VehicleNo`),
  KEY `CheckpostId_idx` (`CheckpostId`),
  KEY `DistrictId_idx` (`DistrictId`),
  KEY `BlockId_idx` (`BlockId`),
  CONSTRAINT `CheckpostId` FOREIGN KEY (`CheckpostId`) REFERENCES `checkpostname` (`CheckpostId`),
  CONSTRAINT `VehicleNo` FOREIGN KEY (`VehicleNo`) REFERENCES `vehicleregistration` (`VehicleNo`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `checkpost`
--

LOCK TABLES `checkpost` WRITE;
/*!40000 ALTER TABLE `checkpost` DISABLE KEYS */;
INSERT INTO `checkpost` VALUES ('32202cf9-64b6-4c7d-b1d0-8155d7d9781f',2,'CG04PE9875',NULL,NULL,1,40,'2025-10-19 20:39:22'),('4a2d767f-138b-4478-a608-caac45e9e982',1,'CG042272',NULL,NULL,1,20,'2025-10-19 10:09:02'),('bf183998-5a0e-426c-91e2-c9ff1aef0a57',1,'CG02YX6636',NULL,NULL,1,13,'2025-10-19 15:34:26'),('c6e21e95-0cd7-4b78-881c-a1d54830922b',3,'CG07JS4848',NULL,NULL,1,25,'2025-10-19 20:14:19');
/*!40000 ALTER TABLE `checkpost` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `checkpostname`
--

DROP TABLE IF EXISTS `checkpostname`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `checkpostname` (
  `CheckpostId` int(11) NOT NULL,
  `Checkpostname` varchar(75) DEFAULT NULL,
  PRIMARY KEY (`CheckpostId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `checkpostname`
--

LOCK TABLES `checkpostname` WRITE;
/*!40000 ALTER TABLE `checkpostname` DISABLE KEYS */;
INSERT INTO `checkpostname` VALUES (1,'Gadi Chowk'),(2,'Civil Line'),(3,'Police Line'),(4,'Shankar Nagar'),(5,'Lodi para'),(6,'Railway Station'),(7,'Hawa Mahal'),(8,'Kamal Vihar'),(9,'Main Chowk'),(10,'GolChowk'),(11,NULL),(12,NULL),(13,'Ram Nagar');
/*!40000 ALTER TABLE `checkpostname` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `clusternodalregistration`
--

DROP TABLE IF EXISTS `clusternodalregistration`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `clusternodalregistration` (
  `Id` char(36) NOT NULL,
  `NodalName` varchar(45) DEFAULT NULL,
  `MobileNo` varchar(15) DEFAULT NULL,
  `Block` varchar(45) DEFAULT NULL,
  `GPName` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `clusternodalregistration`
--

LOCK TABLES `clusternodalregistration` WRITE;
/*!40000 ALTER TABLE `clusternodalregistration` DISABLE KEYS */;
/*!40000 ALTER TABLE `clusternodalregistration` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `district`
--

DROP TABLE IF EXISTS `district`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `district` (
  `DistrictId` int(11) NOT NULL,
  `DistrictName` varchar(30) DEFAULT NULL,
  PRIMARY KEY (`DistrictId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `district`
--

LOCK TABLES `district` WRITE;
/*!40000 ALTER TABLE `district` DISABLE KEYS */;
INSERT INTO `district` VALUES (1,'Raipur'),(2,'Dhamtari'),(3,'Durg'),(4,'Mahasamund'),(5,'Gariyaband'),(6,'Balodabazar'),(7,'Bemetera'),(8,'Mungeli'),(9,'Rajnandgaon'),(10,'Khairagarh'),(11,'Balod');
/*!40000 ALTER TABLE `district` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `nodalregistration`
--

DROP TABLE IF EXISTS `nodalregistration`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `nodalregistration` (
  `Id` char(36) NOT NULL,
  `District` varchar(30) DEFAULT NULL,
  `NodalName` varchar(45) DEFAULT NULL,
  `NodalMobileNo` varchar(15) DEFAULT NULL,
  `AssitantNodalName` varchar(45) DEFAULT NULL,
  `AssitantNodalMobileNo` varchar(15) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='		';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `nodalregistration`
--

LOCK TABLES `nodalregistration` WRITE;
/*!40000 ALTER TABLE `nodalregistration` DISABLE KEYS */;
/*!40000 ALTER TABLE `nodalregistration` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sources`
--

DROP TABLE IF EXISTS `sources`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `sources` (
  `Id` char(36) NOT NULL,
  `VehicleNo` varchar(20) DEFAULT NULL,
  `DistrictId` int(11) DEFAULT NULL,
  `BlockId` int(11) DEFAULT NULL,
  `Pass` tinyint(1) DEFAULT NULL,
  `TotalPeople` int(11) DEFAULT NULL,
  `CurrentDate` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `VehicleNo` (`VehicleNo`),
  KEY `DistrictId` (`DistrictId`),
  KEY `BlockId` (`BlockId`),
  CONSTRAINT `sources_ibfk_1` FOREIGN KEY (`VehicleNo`) REFERENCES `vehicleregistration` (`VehicleNo`),
  CONSTRAINT `sources_ibfk_2` FOREIGN KEY (`DistrictId`) REFERENCES `vehicleregistration` (`DistrictId`),
  CONSTRAINT `sources_ibfk_3` FOREIGN KEY (`BlockId`) REFERENCES `vehicleregistration` (`BlockId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sources`
--

LOCK TABLES `sources` WRITE;
/*!40000 ALTER TABLE `sources` DISABLE KEYS */;
INSERT INTO `sources` VALUES ('59360c9d-0fff-49bf-a0cf-e0586fb25f2d','CG042272',NULL,NULL,1,35,'2025-10-19 23:44:01'),('6a574145-9982-4125-9c65-aba3239599b0','CG02YX6636',NULL,NULL,1,10,'2025-10-19 19:21:01');
/*!40000 ALTER TABLE `sources` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `userlogin`
--

DROP TABLE IF EXISTS `userlogin`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `userlogin` (
  `Id` int(11) NOT NULL,
  `UserName` varchar(45) DEFAULT NULL,
  `Password` varchar(255) DEFAULT NULL,
  `LoginRole` varchar(45) DEFAULT NULL,
  `DistrictId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `DistrictId` (`DistrictId`),
  CONSTRAINT `userlogin_ibfk_1` FOREIGN KEY (`DistrictId`) REFERENCES `district` (`DistrictId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `userlogin`
--

LOCK TABLES `userlogin` WRITE;
/*!40000 ALTER TABLE `userlogin` DISABLE KEYS */;
INSERT INTO `userlogin` VALUES (1,'admin','admin123','admin',NULL),(2,'checkpost','checkpost123','checkpost',NULL),(3,'source','source123','source',NULL),(4,'raipur','raipur123','user',1),(5,'dhamtari','dhamtari123','user',2),(6,'durg','durg123','user',3),(7,'mahasamund','mahasamund123','user',4),(8,'gariyaband','gariyaband123','user',5),(9,'balodabazar','balodabazar123','user',6),(10,'bemetera','bemetera123','user',7),(11,'mungeli','mungeli123','user',8),(12,'rajnandgaon','rajnandgaon123','user',9),(13,'khairagarh','khairagarh123','user',10),(14,'balod','balod123','user',11);
/*!40000 ALTER TABLE `userlogin` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `vehicleregistration`
--

DROP TABLE IF EXISTS `vehicleregistration`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `vehicleregistration` (
  `VehicleNo` varchar(20) NOT NULL,
  `VehicleType` varchar(30) NOT NULL,
  `SeatCapacity` int(11) NOT NULL,
  `DriverName` varchar(45) NOT NULL,
  `DriverMobileNo` varchar(15) NOT NULL,
  `VehicleNodalName` varchar(45) NOT NULL,
  `NodalMobileNo` varchar(15) NOT NULL,
  `DistrictId` int(11) NOT NULL,
  `BlockId` int(11) NOT NULL,
  `GPName` varchar(45) DEFAULT NULL,
  `Remark` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`VehicleNo`),
  UNIQUE KEY `VehicleNo_UNIQUE` (`VehicleNo`),
  KEY `BlockId_idx` (`BlockId`),
  KEY `DistrictId_idx` (`DistrictId`),
  CONSTRAINT `BlockId` FOREIGN KEY (`BlockId`) REFERENCES `block` (`BlockId`),
  CONSTRAINT `DistrictId` FOREIGN KEY (`DistrictId`) REFERENCES `district` (`DistrictId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `vehicleregistration`
--

LOCK TABLES `vehicleregistration` WRITE;
/*!40000 ALTER TABLE `vehicleregistration` DISABLE KEYS */;
INSERT INTO `vehicleregistration` VALUES ('CG02YX6636','Small Vehicle',15,'Test','7878545747','test','7896541230',1,4,'string','string'),('CG042272','BUS',40,'TEST','7878787878','TEST','7878787878',1,12,'NONE','NONE'),('CG04PE9875','BUS',50,'TEST2','9996969696','TEST2','8785458512',1,2,'NONE','NONE'),('CG07JS4848','BUS',40,'TEST2','7854542151','TEST2','4561654894',3,6,'-','-');
/*!40000 ALTER TABLE `vehicleregistration` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-10-20 17:03:08
