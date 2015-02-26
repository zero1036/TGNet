using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EG.WeChat.Model;

namespace EG.WeChat.Platform.DA
{
    public class GroupDA : DataBase
    {

        public DataTable GetGroupList(Hashtable model)
        {
            string sql = @"select GroupID,GroupName,Description,State from T_Group where GroupID like [GroupID] and GroupName like [GroupName] ";

            var result = template.Query(sql, model);

            return result;

        }

        public PagingM GetPageList(Hashtable model, int pageIndex)
        {
            string sql = @"select GroupID,GroupName,Description,State from T_Group where GroupID like [GroupID] and GroupName like [GroupName] and IsDeleted= 0 ";

            return QueryByPage(sql, model, pageIndex, "GroupID");
        }


        public int Add(T_Group model)
        {
            string sql = @"INSERT INTO T_Group(GroupName,Description,CreatedBy,LastModifiedBy) VALUES([GroupName],[Description],[CreatedBy],[LastModifiedBy])";
            return template.Execute(sql, model);
        }

    }
}
