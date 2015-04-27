CREATE TABLE `sys_user` (
  `sysuserid` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `userid` varchar(64) NOT NULL,
  `tid` mediumint(8) unsigned NOT NULL DEFAULT '1',
  PRIMARY KEY (`sysuserid`),
  KEY `userid` (`userid`)
) ENGINE=InnoDB AUTO_INCREMENT=10002 DEFAULT CHARSET=utf8;
