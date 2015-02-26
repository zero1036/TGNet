using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using EG.Common.Entity;
using EG.Utility.DBCommon;
using EG.Utility.DBCommon.dao;
using EG.WeChat.Model;

namespace EG.WeChat.Platform
{
    public class DataBase
    {
        private ADOTemplate _template;
        public ADOTemplate template
        {
            get
            {
                if (_template == null)
                {
                    _template = new ADOTemplate();
                }
                return _template;
            }
        }


        public int Add<T>(T tableObject) where T : new()
        {
            return template.Insert(tableObject);
        }


        public int Edit<T>(T tableObject) where T : new()
        {
            return template.Update(tableObject);
        }


        public int Del<T>(T tableObject) where T : new()
        {
            return template.Delete(tableObject);
        }


        public T GetByPKey<T>(T tableObject) where T : new()
        {
            return template.Get(tableObject);
        }


        public List<T> TableToEntity<T>(DataTable table) where T : new()
        {
            List<T> result = new List<T>();
            foreach (DataRow row in table.Rows)
            {
                T obj = DBUtil.Row2Object<T>(row);
                result.Add(obj);
            }
            return result;
        }


        public int GetInt(string sql, object module)
        {
            SQLParser parser = new SQLParser();
            parser.Parse(sql, module);

            return template.GetInt(sql, parser.GetSqlParameterNames(), parser.GetSqlParameterValues(), 0);
        }


        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="model"></param>
        /// <param name="pageSize">每页行数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        protected PagingM QueryByPage(string Sql, object model, int pageIndex, string orderby)
        {
            /*
             * SqlParameter[] 封装在 template 中，无法使用 ParameterDirection.Output 
             * 调用不了存储过程，只能在SQL中分页，不能在存储过程中。
             */
            PagingM result = new PagingM();

            string CountSQL = string.Format("SELECT COUNT(1) as TotalCount FROM ({0}) t1 ", Sql);

            //result.TotalCount = GetInt(CountSQL, model);          //GetInt好似无法识别like查询
            var c = template.Query(CountSQL, model);
            if (c.Rows.Count > 0)
            {
                result.TotalCount = Convert.ToInt32(c.Rows[0][0]);
            }

            result.PageIndex = pageIndex > result.TotalPages ? result.TotalPages : pageIndex;

            string select = "select ";

            int insertBit = Sql.ToLower().IndexOf(select);

            if (insertBit == -1)
            {
                throw new FormatException("SQL 语句不正确！");
            }

            string addNumberSQL = string.Format(" TOP {0} row_number() OVER (ORDER BY {1}) as RowNumber, ", result.PageSize * (result.PageIndex - 1) + result.PageSize, orderby);
            string addwhereSQL = string.Format(" where t1.RowNumber > {0} ", result.PageSize * (result.PageIndex - 1));
            string headSQL = Sql.Substring(0, insertBit + select.Length);
            string footSQL = Sql.Substring(insertBit + select.Length);

            string newSQL = "select * from ( " + headSQL + addNumberSQL + footSQL + ") t1 " + addwhereSQL;

            result.DataTable = template.Query(newSQL, model);

            result.DataTable.Columns.Remove("RowNumber");

            return result;
        }



      


    }






}
