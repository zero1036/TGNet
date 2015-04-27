
set @userid='MarkLin';
set @tbs='';
set @tid=0;
select concat(tbname , '_', trid),tid into @tbs,@tid from sys_tenantroute where tid =( select tid from sys_user where userid=@userid);
set @p1=concat("SELECT * FROM ", @tbs," t left outer join wxtest.sys_user s on t.sysuserid = s.sysuserid where userid='",@userid,"' and tid=",@tid,"; ");
prepare prod2 from @p1;
execute prod2 ;   
DEALLOCATE PREPARE prod2;
    


SELECT * FROM t_user_1 t left outer join wxtest.sys_user s on t.sysuserid = s.sysuserid  where userid='MarkLin' or s.sysuserid>500;

