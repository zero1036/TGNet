CREATE TABLE `sys_tenantroute` (
  `tenid` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `tbname` varchar(20) NOT NULL,
  `tid` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `trid` tinyint(3) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`tenid`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
