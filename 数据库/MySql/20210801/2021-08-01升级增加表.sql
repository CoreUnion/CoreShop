/*
SQLyog Ultimate v13.1.1 (64 bit)
MySQL - 5.7.33 : Database - CoreShop
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
CREATE DATABASE /*!32312 IF NOT EXISTS*/`CoreShop` /*!40100 DEFAULT CHARACTER SET latin1 */;

USE `CoreShop`;

/*Table structure for table `WeChatAccessToken` */

DROP TABLE IF EXISTS `WeChatAccessToken`;

CREATE TABLE `WeChatAccessToken` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `appType` int(10) NOT NULL,
  `appId` varchar(50) NOT NULL,
  `accessToken` varchar(250) NOT NULL,
  `expireTimestamp` bigint(19) NOT NULL,
  `updateTimestamp` bigint(19) NOT NULL,
  `createTimestamp` bigint(19) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `PK_WeChatAccessToken` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `WeChatAccessToken` */

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
