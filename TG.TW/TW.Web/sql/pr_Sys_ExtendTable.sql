DELIMITER $$
drop procedure if exists pr_Sys_ExtendTable$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `pr_Sys_ExtendTable`(
in ptrid MEDIUMINT,
in ptbheader varchar(20),
out ptbname varchar(30)
)
begin

-- 根据路由编号，获取是否存在对应分支表
set @ptarget=concat(ptbheader,ptrid);
set @psource=concat(ptbheader,"1");

if not exists(select 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'test' AND TABLE_NAME = @ptarget) then
   -- SELECT TABLE_NAME into @tbname FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'test' AND TABLE_NAME = @p3;
   -- select @p3,@tbname;
   -- 如果分支表不存在，则新建
   -- if @tbname is not null then
   set @p2=concat("create table ",@ptarget," like ",@psource,";");
   prepare prod2 from @p2;
   execute prod2;
   DEALLOCATE PREPARE prod2;
   
end if;

set ptbname =  @ptarget;
end$$
DELIMITER ;




