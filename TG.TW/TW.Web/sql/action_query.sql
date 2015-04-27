use test;


set @p='3';
call pr_Sys_ExtendTable(2,'t_User_',@p);
call pr_User_SingleAdd2(1,'TG','Mark','PM','13802410241','','zero1036','',1,'504',3);

call pr_User_SingleAdd(1,'MarkLin','Mark','PM','13802410241','','zero1036','',1,'504',3);
call pr_User_SingleAdd(2,'SarahLi','Sarah','PM','13802410241','','zero1036','',1,'504',3);
call pr_User_SingleAdd(4,'MoLI','Mo','PM','13802410241','','zero1036','',1,'504',3);

set @ptbname='';
set @pcurtrid=0;
call pr_Sys_GetTrid('sys_User','t_User_',2,200,@ptbname,@pcurtrid);
select @ptbname,@pcurtrid;

set auto_increment=0;


set @ptbname='t_User_1';
set @p2=concat("insert into ",@ptbname," (userid,name,position,mobile,email,weixinid,avatar,status,password) 
   values (?,?,?,?,?,?,?,?,?)");
      
   set @puserid='gg';
   set @pname='gg';
   set @pposition='gg';
   set @pmobile='gg';
   set @pemail='gg';
   set @pweixinid='gg';
   set @pavatar='gg';
   set @pstatus=1;
   set @ppassword='gg';
   prepare prod2 from @p2;
   execute prod2 using @puserid,@pname,@pposition,@pmobile,@pemail,@pweixinid,@pavatar,@pstatus,@ppassword;
    DEALLOCATE PREPARE prod2;


delete from t_user_1 where sysuserid>0;
delete from sys_user where sysuserid>0;
ALTER TABLE t_user_1 AUTO_INCREMENT=1;
ALTER TABLE sys_user AUTO_INCREMENT=1;


delete from sys_tag where systagid>0;
ALTER TABLE sys_tag AUTO_INCREMENT=1;

SELECT * FROM test.t_user_1 order by sysuserid desc;
select * from test.sys_user order by sysuserid desc;

select count(*) from test.t_user_1;
select count(*) from test.sys_user;

select count(*) from test.t_user_1 group by sysuserid;



rollback;
commit;

set autocommit = 0;

show variables like 'autocommit';

show index from t_User_1;

select * from sys_tag order by systagid desc;
select * from sys_tag where tagid= 99 and tid=1 order by systagid desc;

select * from sys_tag where tid=1 and sysuserid=0 order by systagid desc;
select * from sys_tag where tagid>0 and tid=1 and sysuserid=0 order by systagid desc;

select * from sys_tag where tagid=99 and tid=1 and sysuserid=0 order by systagid desc;

alter table sys_tag add key(tagid,tid);
alter table sys_tag drop index tagid;

172000

