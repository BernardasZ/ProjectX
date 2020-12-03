-- MySQL dump 10.13  Distrib 8.0.22, for Win64 (x86_64)
--
-- Host: localhost    Database: ProjectX
-- ------------------------------------------------------
-- Server version	8.0.22

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `permission_action`
--

DROP TABLE IF EXISTS `permission_action`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `permission_action` (
  `id` int NOT NULL AUTO_INCREMENT,
  `controller_id` int NOT NULL,
  `action_name` varchar(255) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `permission_action__permission_controller_idx` (`controller_id`),
  CONSTRAINT `permission_action__permission_controller` FOREIGN KEY (`controller_id`) REFERENCES `permission_controller` (`id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `permission_action`
--

LOCK TABLES `permission_action` WRITE;
/*!40000 ALTER TABLE `permission_action` DISABLE KEYS */;
INSERT INTO `permission_action` VALUES (1,1,'ReadTask'),(2,1,'CreateTask'),(3,1,'UpdateTask'),(4,1,'DeleteTask'),(6,1,'Tasks'),(7,2,'All'),(8,1,'All');
/*!40000 ALTER TABLE `permission_action` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `permission_controller`
--

DROP TABLE IF EXISTS `permission_controller`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `permission_controller` (
  `id` int NOT NULL AUTO_INCREMENT,
  `controller_name` varchar(255) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `permission_controller`
--

LOCK TABLES `permission_controller` WRITE;
/*!40000 ALTER TABLE `permission_controller` DISABLE KEYS */;
INSERT INTO `permission_controller` VALUES (1,'Task'),(2,'User');
/*!40000 ALTER TABLE `permission_controller` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `permission_mapping`
--

DROP TABLE IF EXISTS `permission_mapping`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `permission_mapping` (
  `id` int NOT NULL AUTO_INCREMENT,
  `role_id` int NOT NULL,
  `controller_id` int NOT NULL,
  `action_id` int NOT NULL,
  `allow_all_actions` bit(1) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `permission_mapping__role_idx` (`role_id`),
  KEY `permission_mapping__permission_controller_idx` (`controller_id`),
  KEY `permission_mapping__permission_action_idx` (`action_id`),
  CONSTRAINT `permission_mapping__permission_action` FOREIGN KEY (`action_id`) REFERENCES `permission_action` (`id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `permission_mapping__permission_controller` FOREIGN KEY (`controller_id`) REFERENCES `permission_controller` (`id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `permission_mapping__role` FOREIGN KEY (`role_id`) REFERENCES `role` (`id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=34 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `permission_mapping`
--

LOCK TABLES `permission_mapping` WRITE;
/*!40000 ALTER TABLE `permission_mapping` DISABLE KEYS */;
INSERT INTO `permission_mapping` VALUES (18,1,1,1,_binary '\0'),(22,1,1,6,_binary '\0'),(24,1,2,7,_binary ''),(25,2,2,7,_binary ''),(26,2,1,1,_binary '\0'),(27,2,1,2,_binary '\0'),(28,2,1,3,_binary '\0'),(29,2,1,4,_binary '\0'),(30,2,1,6,_binary '\0'),(32,1,1,4,_binary '\0');
/*!40000 ALTER TABLE `permission_mapping` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Temporary view structure for view `permission_view`
--

DROP TABLE IF EXISTS `permission_view`;
/*!50001 DROP VIEW IF EXISTS `permission_view`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `permission_view` AS SELECT 
 1 AS `role_name`,
 1 AS `controller_name`,
 1 AS `action_name`,
 1 AS `allow_all_actions`*/;
SET character_set_client = @saved_cs_client;

--
-- Table structure for table `role`
--

DROP TABLE IF EXISTS `role`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `role` (
  `id` int NOT NULL AUTO_INCREMENT,
  `role_name` varchar(50) NOT NULL,
  `role_value` tinyint unsigned NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `role`
--

LOCK TABLES `role` WRITE;
/*!40000 ALTER TABLE `role` DISABLE KEYS */;
INSERT INTO `role` VALUES (1,'Admin',0),(2,'User',1),(3,'AllRoles',255);
/*!40000 ALTER TABLE `role` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `task`
--

DROP TABLE IF EXISTS `task`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `task` (
  `id` int NOT NULL AUTO_INCREMENT,
  `user_id` int NOT NULL,
  `task_name` varchar(1000) NOT NULL,
  `task_status` tinyint unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `task`
--

LOCK TABLES `task` WRITE;
/*!40000 ALTER TABLE `task` DISABLE KEYS */;
INSERT INTO `task` VALUES (2,1,'Mano superinis task',0);
/*!40000 ALTER TABLE `task` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user_data`
--

DROP TABLE IF EXISTS `user_data`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user_data` (
  `id` int NOT NULL AUTO_INCREMENT,
  `role_id` int NOT NULL DEFAULT '0',
  `user_name` varchar(255) DEFAULT NULL,
  `user_email` varchar(255) DEFAULT NULL,
  `pass_hash` varchar(255) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `UserName_UNIQUE` (`user_name`),
  UNIQUE KEY `Email_UNIQUE` (`user_email`),
  KEY `user_data__role_idx` (`role_id`),
  CONSTRAINT `user_data__role` FOREIGN KEY (`role_id`) REFERENCES `role` (`id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user_data`
--

LOCK TABLES `user_data` WRITE;
/*!40000 ALTER TABLE `user_data` DISABLE KEYS */;
INSERT INTO `user_data` VALUES (2,1,'bernardas','bzxmixrox@gmail.com','b681a50c44242e2a4524ce1ec95c33b990d1afa6b7f47fd6382d4c6be765d8cd');
/*!40000 ALTER TABLE `user_data` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user_session`
--

DROP TABLE IF EXISTS `user_session`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user_session` (
  `id` int NOT NULL AUTO_INCREMENT,
  `session_identifier` varchar(255) NOT NULL,
  `create_dt` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `ip` varchar(15) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user_session`
--

LOCK TABLES `user_session` WRITE;
/*!40000 ALTER TABLE `user_session` DISABLE KEYS */;
/*!40000 ALTER TABLE `user_session` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Final view structure for view `permission_view`
--

/*!50001 DROP VIEW IF EXISTS `permission_view`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `permission_view` AS select `R`.`role_name` AS `role_name`,`PC`.`controller_name` AS `controller_name`,`PA`.`action_name` AS `action_name`,`PM`.`allow_all_actions` AS `allow_all_actions` from (((`permission_mapping` `PM` join `role` `R` on((`PM`.`role_id` = `R`.`id`))) join `permission_controller` `PC` on((`PM`.`controller_id` = `PC`.`id`))) join `permission_action` `PA` on((`PM`.`action_id` = `PA`.`id`))) order by `R`.`role_name` */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2020-12-03 23:45:58
