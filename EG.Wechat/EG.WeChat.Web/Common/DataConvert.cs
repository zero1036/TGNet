using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace EG.WeChat.Web.Common
{
    public class DataConvert
    {
        /// <summary>  
        /// dataTable转换成Json格式  
        /// </summary>  
        /// <param name="dt"></param>  
        /// <returns></returns> 
        public static string DataTableToJson(DataTable dt)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("[");

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow i in dt.Rows)
                {
                    jsonBuilder.Append("{");
                    foreach (DataColumn j in dt.Columns)
                    {
                        jsonBuilder.AppendFormat(" \"{0}\" : \"{1}\" ,", j.ColumnName, i[j.ColumnName]);
                    }
                    jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                    jsonBuilder.Append("},");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            }
            else
            {   //当没有数据的时候，显示列名
                jsonBuilder.Append("{");
                foreach (DataColumn j in dt.Columns)
                {
                    jsonBuilder.AppendFormat(" \"{0}\" : \"{1}\" ,", j.ColumnName, string.Empty);
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("}");
            }
            jsonBuilder.Append("]");
            return jsonBuilder.ToString();
        }


    }


}