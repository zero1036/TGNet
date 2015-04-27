DELIMITER $$
drop procedure if exists pr_User_SingleAdd$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `pr_User_SingleAdd`(
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
declare ptbname varchar(30);
declare pcurtrid MEDIUMINT;
/*
set pcurtrid=1;
-- 关闭事务自动提交
-- set autocommit=0;
-- 根据租户ID查询路由编号
SELECT max(trid) INTO @ctrid FROM `sys_User` WHERE tid = ptid;
-- SELECT trid INTO @ctrid From `sys_User` where trid=(SELECT trid FROM `sys_User` WHERE tid = ptid) order by tid desc limit 1;
-- 如果路由编号不为空，赋值当前路由编号 ，反之，获取最大路由编号+1
if @ctrid is not null then
   set pcurtrid=@ctrid;
else
   select max(trid) into @ctrid from `sys_User`;
   -- 根据路由编号，获取是否存在对应用户表
   set @p1=concat('select count(*) into @pcount1 from t_User_',@ctrid);
   prepare prod1 from @p1;
   execute prod1;
   -- 当分支表数量少于阈值时，无需新增分支表，反之要+1
   if @pcount1 < plitmitcount then
       set pcurtrid=@ctrid;
   else
      set pcurtrid=@ctrid+1;
   end if;
   DEALLOCATE PREPARE prod1;
end if;
*/
-- 根据租户ID、受限记录数获取对应用户分支表名称及分支路由ID
call pr_Sys_GetTrid('sys_User','t_User_',ptid,plitmitcount,ptbname,pcurtrid);

-- 根据路由编号，获取分支表或生成分支表
call pr_Sys_ExtendTable(pcurtrid,'t_User_',ptbname);

-- 插入用户总表
insert into `sys_User` (trid,tid) values (pcurtrid,ptid);

if (select row_count()=1) then
   set @p2=concat("insert into ",ptbname," (sysuserid,userid,name,position,mobile,email,weixinid,avatar,status,password) 
values ('",last_insert_id(),"','",puserid,"','",pname,"','",pposition,"','",pmobile,"','",pemail,"','",pweixinid,"','",pavatar,"',",pstatus,",'",ppassword,"')");
   prepare prod2 from @p2;
   execute prod2;
   
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


