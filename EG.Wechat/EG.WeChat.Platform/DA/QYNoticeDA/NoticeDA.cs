using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
namespace EG.WeChat.Platform.DA.QYNoticeDA
{
    public class NoticeDA:DataBase
    {
        public bool AddNotice(string Title, string Context,  string CreateBy)
        {
            try
            {
                string sql = @"	insert into T_QY_Notice(Title,Context,CreateBy)
		                            values(@Title,@Context,@CreateBy)";
                template.Execute(sql, new string[] { "@Title", "@Context", "@CreateBy" },
                                     new string[] { Title, Context, CreateBy }, null);
                return true;
            }
            catch (Exception e)
            {
                Logger.Log4Net.Error("QY insert T_QY_Department error:", e);
            }
            return false;
        }

        public bool DeleteNotice(string id,string userid)
        {
            try
            {
                string sql = @"	update T_QY_Notice set DeleteDate=GETDATE(),DeleteBy=@userid where id=@id";
                template.Execute(sql, new string[] { "@id","@userid" },
                                     new string[] { id,userid }, null);
                return true;
            }
            catch (Exception e)
            {
                Logger.Log4Net.Error("QY insert T_QY_Department error:", e);
            }
            return false;
        }

        public bool UpdateNotice(string id, string userid,string Title,string Context,string status)
        {
            try
            {
                string sql = @"	update T_QY_Notice set updatedate=GETDATE(),UpdateBy=@userid,Title=@Title,Context=@context,Status=@status where id=@id";
                template.Execute(sql, new string[] { "@id", "@userid", "@Title", "@context", "@status" },
                                     new string[] { id, userid,Title,Context,status }, null);
                return true;
            }
            catch (Exception e)
            {
                Logger.Log4Net.Error("QY insert T_QY_Department error:", e);
            }
            return false;
        }


        public DataTable GetNotices(string id = null)
        {
            string sql;
            if (id != null)
            {
                sql = @"	select n.*,u.UserName CreateUserName from T_QY_Notice n inner join T_User u on n.CreateBy=u.UserID
	                             where n.DeleteDate is null and n.id=@id  order by id desc";
                return template.Query(sql, new string[] { "@id" },
                                    new object[] { id }, null);
            }
            else
            {
                sql = @"		select n.*,u.UserName CreateUserName from T_QY_Notice n inner join T_User u on n.CreateBy=u.UserID
	                                 where n.DeleteDate is null  order by id desc  ";
                return template.Query(sql, new string[] { },
                                    new object[] { }, null);
            }
        }

        public DataTable GetNoticeMember(string id )
        {
            string sql;

            sql = @"select m.ID,m.Avatar,m.Userid,m.Name,m.Position,m.Mobile,m.Email,m.Weixinid,t.CreateDate,str(t.IsChecked) Status 
                            from T_QY_Statistic t inner join T_QY_Member m 
                             on t.MemberID=m.ID where t.functype=2 and t.FuncID=@id and t.DeleteDate is null
                            order by t.IsChecked";
                return template.Query(sql, new string[] {"@id" },
                                    new object[] { id }, null);
            }
        }

    }
