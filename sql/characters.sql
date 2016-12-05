/*
MySQL Data Transfer
Source Host: localhost
Source Database: ffxiv_server
Target Host: localhost
Target Database: ffxiv_server
Date: 12/3/2016 5:39:10 PM
*/

SET FOREIGN_KEY_CHECKS=0;
-- ----------------------------
-- Table structure for characters
-- ----------------------------
CREATE TABLE `characters` (
  `id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `userId` int(11) unsigned NOT NULL,
  `slot` smallint(6) unsigned NOT NULL,
  `serverId` int(11) NOT NULL,
  `name` varchar(32) NOT NULL,
  `state` smallint(5) unsigned NOT NULL DEFAULT '0',
  `creationDate` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `isLegacy` smallint(1) unsigned DEFAULT '0',
  `doRename` smallint(1) unsigned DEFAULT '0',
  `playTime` int(10) unsigned NOT NULL DEFAULT '0',
  `positionX` float NOT NULL DEFAULT '0',
  `positionY` float NOT NULL DEFAULT '0',
  `positionZ` float NOT NULL DEFAULT '0',
  `rotation` float NOT NULL DEFAULT '0',
  `actorState` smallint(5) unsigned DEFAULT '0',
  `currentZoneId` smallint(5) unsigned DEFAULT '0',
  `destinationZoneId` smallint(5) unsigned DEFAULT '0',
  `destinationSpawnType` tinyint(3) unsigned DEFAULT '0',
  `guardian` tinyint(3) unsigned NOT NULL,
  `birthDay` tinyint(3) unsigned NOT NULL,
  `birthMonth` tinyint(3) unsigned NOT NULL,
  `initialTown` tinyint(3) unsigned NOT NULL,
  `tribe` tinyint(3) unsigned NOT NULL,
  `gcCurrent` tinyint(3) unsigned DEFAULT '0',
  `gcLimsaRank` tinyint(3) unsigned DEFAULT '127',
  `gcGridaniaRank` tinyint(3) unsigned DEFAULT '127',
  `gcUldahRank` tinyint(3) unsigned DEFAULT '127',
  `currentTitle` int(10) unsigned DEFAULT '0',
  `restBonus` int(10) DEFAULT '0',
  `achievementPoints` int(10) unsigned DEFAULT '0',
  `currentPartyGroupId` bigint(20) unsigned DEFAULT '0',
  `currentRetainerGroupId` bigint(20) unsigned DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;