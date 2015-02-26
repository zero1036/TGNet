using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using EG.WeChat.Model;
using EG.WeChat.Platform.Model;

namespace EG.WeChat.Platform.DA
{
    public class MemberDA : DataBase
    {

        public List<T_Member> GetMemberList(Hashtable model)
        {
            string sql = @"select ID,OpenID,Name,Phone,Mail,Type,Integral,Deposit,State,LastModifiedTime from T_Member where OpenID=[OpenID] ";

            var result = template.Query(sql, model);

            return TableToEntity<T_Member>(result);
        }



        public PagingM GetPageList(Hashtable model, int pageIndex)
        {
            string sql = @"select Name,Phone,Mail,Type,Integral,Deposit from T_Member where Name like [Name] and state=0 ";

            return QueryByPage(sql, model, pageIndex, "Name");
        }




    }
}
