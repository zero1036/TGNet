DELIMITER $$
drop procedure if exists pr_Sys_GetTrid$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `pr_Sys_GetTrid`(
in psystbname varchar(30),
in ptbheader varchar(20),
in ptid int,
in plitmitcount int,
out ptbname varchar(30),
out pcurtrid MEDIUMINT
)
begin
-- SELECT max(trid) INTO @ctrid FROM `sys_User` WHERE tid = 1;
-- select @ctrid;

set @p1=concat("SELECT max(trid) INTO @ctrid FROM ",psystbname," WHERE tid = ",ptid,";");
prepare prod1 from @p1;
execute prod1;
DEALLOCATE PREPARE prod1;

-- 如果路由编号不为空，赋值当前路由编号 ，反之，获取最大路由编号+1

if @ctrid is not null then
   set pcurtrid=@ctrid;
else
   -- select max(trid) into @ctrid from `sys_User`;
   set @p2=concat("select max(trid) into @ctrid from ",psystbname);
   prepare prod2 from @p2;
   execute prod2;
   DEALLOCATE PREPARE prod2;
   -- 根据路由编号，获取是否存在对应用户表
   set @p3=concat('select count(*) into @pcount1 from ',ptbheader,@ctrid);
   prepare prod3 from @p3;
   execute prod3;
   -- 当分支表数量少于阈值时，无需新增分支表，反之要+1
   if @pcount1 < plitmitcount then
       set pcurtrid=@ctrid;
   else
      set pcurtrid=@ctrid+1;
   end if;
   DEALLOCATE PREPARE prod3;
end if;

set ptbname=concat(ptbheader,pcurtrid);
end$$
DELIMITER ;

