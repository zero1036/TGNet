DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `pr_User_SingleAdd3`(
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
in ptbname varchar(30))
begin
-- 插入用户总表
insert into `sys_User` (userid,tid) values (puserid,ptid);

if (select row_count()=1) then
  -- set @p2=concat("insert into ",ptbname," (sysuserid,userid,name,position,mobile,email,weixinid,avatar,status,password) 
  -- values ('",last_insert_id(),"','",puserid,"','",pname,"','",pposition,"','",pmobile,"','",pemail,"','",pweixinid,"','",pavatar,"',",pstatus,",'",ppassword,"')");
   set @p2=concat("insert into ",ptbname," (sysuserid,name,position,mobile,email,weixinid,avatar,status,password) 
   values (?,?,?,?,?,?,?,?,?)");
   
   set @lastid=last_insert_id();
   set @pname=pname;
   set @pposition=pposition;
   set @pmobile=pmobile;
   set @pemail=pemail;
   set @pweixinid=pweixinid;
   set @pavatar=pavatar;
   set @pstatus=pstatus;
   set @ppassword=ppassword;

   prepare prod2 from @p2;
   -- execute prod2;
   execute prod2 using @lastid,@pname,@pposition,@pmobile,@pemail,@pweixinid,@pavatar,@pstatus,@ppassword;
   
   if (select row_count()=1) then
     commit;
     SELECT '1', '';
   else
     -- 异常回滚
     rollback;
     SELECT '0', '10001';
   end if;

   DEALLOCATE PREPARE prod2;
   else
    -- 异常回滚
   rollback;
   SELECT '0', '10001';
end if;

end$$
DELIMITER ;
