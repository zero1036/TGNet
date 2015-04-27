CREATE TABLE `t_user_1` (
  `sysuserid` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL,
  `position` varchar(50) DEFAULT '',
  `mobile` varchar(50) DEFAULT '',
  `email` varchar(150) NOT NULL,
  `weixinid` varchar(80) DEFAULT '',
  `avatar` varchar(200) DEFAULT '',
  `status` tinyint(3) unsigned DEFAULT '4',
  `password` varchar(250) DEFAULT '123',
  PRIMARY KEY (`sysuserid`)
) ENGINE=InnoDB AUTO_INCREMENT=10002 DEFAULT CHARSET=utf8;
