DELIMITER $$
drop procedure if exists pr_User_SingleAdd4$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `pr_User_SingleAdd4`(
in ptid MEDIUMINT,
in pcurtrid MEDIUMINT,
in ptbname varchar(30))
begin
-- 插入用户总表
insert into `sys_User` (trid,tid) values (pcurtrid,ptid);

-- if (select row_count()=1) then
  -- set @p2=concat("insert into ",ptbname," (sysuserid,userid,name,position,mobile,email,weixinid,avatar,status,password) 
  -- values ('",last_insert_id(),"','",puserid,"','",pname,"','",pposition,"','",pmobile,"','",pemail,"','",pweixinid,"','",pavatar,"',",pstatus,",'",ppassword,"')");
   set @p2=concat("insert into ",ptbname," (sysuserid,userid,name,position,mobile,email,weixinid,avatar,status,password) 
   values (?,?,?,?,?,?,?,?,?,?)");
   
 --   set @lastid=last_insert_id();
--    set @puserid=puserid;
--    set @pname=pname;
--    set @pposition=pposition;
--    set @pmobile=pmobile;
--    set @pemail=pemail;
--    set @pweixinid=pweixinid;
--    set @pavatar=pavatar;
--    set @pstatus=pstatus;
--    set @ppassword=ppassword;

set @lastid=last_insert_id();
SET @puserid='userid';
SET @pname='name';
SET @pposition='position';
SET @pmobilep4='mobile';
SET @pemail='email';
SET @pweixinid='weixinid';
SET @pavatar='avatar';
SET @pstatus=1;
SET @ppassword='password';
   prepare prod2 from @p2;
  -- execute prod2;
    execute prod2 using @lastid,@puserid,@pname,@pposition,@pmobile,@pemail,@pweixinid,@pavatar,@pstatus,@ppassword;
     commit;
  /* if (select row_count()=1) then
     commit;
    -- SELECT '1', '';
   else
     -- 异常回滚
     rollback;
    -- SELECT '0', '10001';
   end if;*/

   DEALLOCATE PREPARE prod2;
-- else
--    -- 异常回滚
--    rollback;
--    SELECT '0', '10001';
-- end if;

end$$
DELIMITER ;


