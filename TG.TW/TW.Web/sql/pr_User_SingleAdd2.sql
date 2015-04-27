DELIMITER $$
drop procedure if exists pr_User_SingleAdd2$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `pr_User_SingleAdd2`(
in ptid MEDIUMINT,
in puserid varchar(50),
in pname varchar(50),
in pposition varchar(50),
in pmobile varchar(50),
in pemail varchar(150),
in pweixinid varchar(80),
in pavatar varchar(200),
in pstatus tinyint,
in ppassword varchar(250),
in plitmitcount int)
begin
-- 插入用户总表
insert into `sys_User` (trid,tid) values (1,ptid);

if (select row_count()=1) then
   insert into t_User_1 (sysuserid,userid,name,position,mobile,email,weixinid,avatar,status,password) 
   values (last_insert_id(),puserid,pname,pposition,pmobile,pemail,pweixinid,pavatar,pstatus,ppassword);
   
   if (select row_count()=1) then
     commit;
     SELECT '1', '';
   else
     -- 异常回滚
     rollback;
     SELECT '0', '10001';
   end if;

else
   -- 异常回滚
   rollback;
   SELECT '0', '10001';
end if;

end$$
DELIMITER ;


