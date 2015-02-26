using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EG.Utility.DBCommon.dao;
using EG.WeChat.Model;

namespace EG.WeChat.Platform.DA
{
    public class UserDA : DataBase
    {
        #region 用户

        public T_User Login(T_User model)
        {
            //string sql = @"select UserID,UserName,State from T_User where UserID=[UserID] and Password=[Password] and State=[State] and IsDeleted=[IsDeleted] ";
            //return template.Get( model);

            string sql = @"select UserID,UserName,State from T_User where UserID=[UserID] and Password=[Password] and State=0 and IsDeleted=0 ";
            var row = template.Get(sql, model);
            return row == null ? null : DBUtil.Row2Object<T_User>(row);
        }


        public DataTable GetUserAccessRight(string userID)
        {
            string sql = @"select UserID,GroupID,Controller,Action from V_User_AccessRight where UserID=@UserID ";

            var result = template.Query(sql, new string[] { "@UserID" }, new string[] { userID });

            return result;
        }


        public DataTable GetUserList(Hashtable model)
        {
            string sql = @"select UserID,UserName,State from T_User where UserID like [UserID] and UserName like [UserName]  and IsDeleted= 0 and State=0 ";

            var result = template.Query(sql, model);

            return result;

        }


        public PagingM GetPageList(Hashtable model, int pageIndex)
        {
            string sql = @"select UserID,UserName,State from T_User where UserID like [UserID] and UserName like [UserName] and IsDeleted= 0 and State=0 ";

            return QueryByPage(sql, model, pageIndex, "UserID");
        }


        #endregion


        #region 用户组

        public DataTable GetSelectUser(Hashtable model)
        {
            string sql = @"select UserID,UserName from T_User 
                            where UserID not in (
                            select UserID from TR_User_Group where GroupID = [GroupID] ) and UserName like [UserName]  ";
            return template.Query(sql, model); ;
        }


        public DataTable GetSelectedUser(Hashtable model)
        {
            string sql = @"select t2.UserID ,t2.UserName from TR_User_Group t1
                            inner join T_User t2 on t1.UserID = t2.UserID
                            where t1.GroupID= [GroupID] and t2.IsDeleted =0 and t2.UserName like [UserName]   ";
            return template.Query(sql, model); ;
        }


        public int AddUserGroup(TR_User_Group model)
        {
            string sql = @"INSERT INTO TR_User_Group(GroupID,UserID)VALUES([GroupID],[UserID])";
            return template.Execute(sql, model); ;
        }


        #endregion

    }
}
