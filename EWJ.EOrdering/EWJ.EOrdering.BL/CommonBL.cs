using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace EWJ.EOrdering.BL
{
    public static class CommonBL
    {
        public static IList<T> ToList<T>(this DataTable dt)
        {
            if (dt == null)
                return null;

            var list = new List<T>();
            Type t = typeof(T);
            var plist = new List<PropertyInfo>(typeof(T).GetProperties());

            foreach (DataRow item in dt.Rows)
            {
                T s = Activator.CreateInstance<T>();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    PropertyInfo info = plist.Find(p => p.Name == dt.Columns[i].ColumnName);
                    if (info != null)
                    {
                        if (!Convert.IsDBNull(item[i]))
                        {
                            info.SetValue(s, item[i], null);
                        }
                    }
                }
                list.Add(s);
            }
            return list;
        }

        public static IList<T> ToList<T>(this DataSet ds)
        {
            if (ds == null || ds.Tables.Count == 0)
                return null;

            return ds.Tables[0].ToList<T>();
        }

        /// <summary>    
        /// 转化一个DataTable
        /// 自动创建表结构并填充值
        /// </summary>    
        /// <typeparam name="T"></typeparam>    
        /// <param name="list"></param>    
        /// <returns></returns>    
        public static DataTable GetDataTableFromEntities<T>(IEnumerable<T> list)
        {
            //创建属性的集合    
            List<PropertyInfo> pList = new List<PropertyInfo>();
            //获得反射的入口    

            Type type = typeof(T);
            DataTable dt = new DataTable();
            //把所有的public属性加入到集合 并添加DataTable的列    
            Array.ForEach<PropertyInfo>(type.GetProperties(), p => { pList.Add(p); dt.Columns.Add(p.Name, p.PropertyType); });
            foreach (var item in list)
            {
                //创建一个DataRow实例    
                DataRow row = dt.NewRow();
                //给row 赋值    
                pList.ForEach(p => row[p.Name] = p.GetValue(item, null));
                //加入到DataTable    
                dt.Rows.Add(row);
            }
            return dt;
        }
    }
}
