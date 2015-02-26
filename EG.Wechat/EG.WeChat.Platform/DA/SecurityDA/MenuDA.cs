using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EG.Utility.DBCommon.dao;
using EG.WeChat.Model.SecurityM;

namespace EG.WeChat.Platform.DA
{
    public class MenuDA : DataBase
    {

        public List<T_Menu> GetMenu(Hashtable model)
        {
            string sql = @"select Code,Name,FatherCode,Sort,Controller,Description,Href from T_Menu where State = [State] order by Sort";
            var table = template.Query(sql, model);
            return TableToEntity<T_Menu>(table);
        }


    }   
}
