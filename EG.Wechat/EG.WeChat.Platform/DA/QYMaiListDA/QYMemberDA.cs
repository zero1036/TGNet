using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
namespace EG.WeChat.Platform.DA.QYMaiListDA
{
    public class QYMemberDA:DataBase
    {
        private ADOTemplateX template = new ADOTemplateX();
        public bool AddMember(string Userid, string Name, string Position, string Mobile, string Email, string Weixinid, string Avatar, string Status, string CreateBy, string DepartmentPKId)
        {
            try
            {
                template.Execute("exec PRO_WC_QY_AddMember @Userid, @Name, @Position, @Mobile, @Email, @Weixinid, @Avatar, @Status, @CreateBy,@DepartmentPKId",
                   new string[] { "@Userid","@Name","@Position","@Mobile","@Email","@Weixinid","@Avatar","@Status","@CreateBy","@DepartmentPKId" },
                   new string[] { Userid, Name, Position, Mobile, Email, Weixinid, Avatar, Status, CreateBy, DepartmentPKId }, null);
                return true;
            }
            catch (Exception e)
            {
                Logger.Log4Net.Error("addMember database error:", e);
                return false;
            }
        }

        public bool UpdateMember(string ID,string Userid, string Name, string Position, string Mobile, string Email, string Weixinid, string Avatar, string Status, string CreateBy, string DepartmentPKId)
        {
            try
            {
                template.Execute(@"exec PRO_WC_QY_UpdateMember @Userid, @Name, @Position, @Mobile, @Email, @Weixinid, @Avatar, @Status, @CreateBy,@DepartmentPKId,@ID",
                   new string[] { "@Userid", "@Name", "@Position", "@Mobile", "@Email", "@Weixinid", "@Avatar", "@Status", "@CreateBy", "@DepartmentPKId","@ID" },
                   new string[] { Userid, Name, Position, Mobile, Email, Weixinid, Avatar, Status, CreateBy, DepartmentPKId,ID }, null);
                return true;
            }
            catch (Exception e)
            {
                Logger.Log4Net.Error("UpdateMember database error:", e);
                return false;
            }
        }

        public bool DeleteMember(string ID, string DeleteBy)
        {
            try
            {
                template.Execute(@"exec PRO_WC_QY_DeleteMember @ID,@DeleteBy",
                   new string[] { "@ID", "@DeleteBy" },
                   new string[] { ID,DeleteBy }, null);
                return true;
            }
            catch (Exception e)
            {
                Logger.Log4Net.Error("DeleteMember database error:", e);
                return false;
            }
        }

        public DataTable GetMemberByDepPKID(string DepartmentPKId)
        {
            return template.Query("select * from T_QY_Member where DepartmentPKId=@DepartmentPKId and DeleteDate is null",
                new string[] { "@DepartmentPKId" },
                new string[] { DepartmentPKId }, null);
        }

        public DataTable GetMemberByDepPKIDs(List<string> DepartmentPKIds)
        {
            string sql = @"select m.*,d.DepartmentID DepartmentWXId from T_QY_Member m inner join T_QY_Department d 
                             on m.DepartmentPKId=d.ID
                           where m.DepartmentPKId in (";
            List<string> par = new List<string>();

            for (int i = 0; i < DepartmentPKIds.Count; i++)
            {
                if (i == 0)
                {
                    sql += "@" + i.ToString();
                }
                else
                {
                    sql += ",@" + i.ToString();
                }
                par.Add("@" + i.ToString());
            }
            sql += ") and m.DeleteDate is null";
            return template.Query(sql,
                par.ToArray(),
                DepartmentPKIds.ToArray(), null);
        }

        public DataTable GetMemberByPKID(string ID)
        {
            return template.Query("select * from T_QY_Member where ID=@ID and DeleteDate is null",
                new string[] { "@ID" },
                new string[] { ID }, null);
        }


    }
}
