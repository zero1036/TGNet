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
    public class AccessRightDA : DataBase
    {

        public List<TR_Group_Right> GetRightList(Hashtable model)
        {
            string sql = @"select t1.Controller,t4.Name as 'ControllerName',t4.Description as 'ControllerD',
                            t1.Action,t3.Name as 'ActionName',t3.Description as 'ActionD',
                            t2.GroupID  from TR_Action_Controller t1 
                            left join TR_Group_Right t2 on t1.Action = t2.Action and t1.Controller= t2.Controller and t2.GroupID = [GroupID] 
                            inner join T_Action t3 on t1.Action = t3.Code 
                            inner join T_Controller t4 on t1.Controller = t4.Code 
                            order by t4.Sort ,t3.Sort ";
            var table = template.Query(sql, model);

            return TableToEntity<TR_Group_Right>(table);
        }



        public int Del(int GroupID)
        {
            string sql = @"DELETE FROM TR_Group_Right WHERE  GroupID= @GroupID";
            return template.Execute(sql, new string[] { "@GroupID" }, new object[] { GroupID });
        }


        public int Add(TR_Group_Right model)
        {
            string sql = @"INSERT INTO TR_Group_Right(GroupID,Controller,Action) VALUES([GroupID],[Controller],[Action])";
            return template.Execute(sql,model);
        }


    }



}
