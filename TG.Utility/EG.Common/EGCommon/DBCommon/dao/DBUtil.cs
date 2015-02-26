using EG.Business.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq.Mapping;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EG.Utility.DBCommon.dao
{
    public class DBUtil
    {

        public static char GetDBParamFlag(short dbType)
        {
            if (ADOTemplate.DB_TYPE_ORACLE == dbType)
                return ':';
            return '@';
        }

        /// <summary>
        /// 把Table中的数据，通过反射，转为指定类型的对象数组
        /// </summary>
        /// <param name="DataTable">数据表</param>
        /// <generic name="T">返回列表中，元素的对象类型</generic>
        /// <returns>objectType指定类型的数组</returns>
        public static T[] Table2Array<T>(DataTable table) where T : new()
        {
            var list = Table2List<T>(table);
            if (list == null)
                return new T[0];
            return list.ToArray();
        }

        /// <summary>
        /// 把Table中的数据，通过反射，转为指定类型的对象列表
        /// </summary>
        /// <param name="DataTable">数据表</param>
        /// <generic name="T">返回列表中，元素的对象类型</generic>
        /// <returns>objectType指定类型的列表</returns>
        public static IList<T> Table2List<T>(DataTable table) where T : new()
        {
            if (table == null || table.Rows.Count <= 0)
                return new List<T>();

            IList<T> result = new List<T>(table.Rows.Count);

            IList<Object[]> proNameList = getEntityNames<T>(table);//Object[]:Object[]{PropertyInfo,string}

            foreach (DataRow row in table.Rows)
            {
                T t = Row2Object<T>(proNameList, row);
                result.Add(t);
            }
            return result;
        }

        /// <summary>
        /// 把Row类型数据，通过反射，转为指定类型的对象，
        /// 转换时，
        /// 1）创建objectType的实例object，遍历objectType中的所有属性
        /// 2）每一属性，先看有没有Column Attribute定义，
        ///       a)如果有，取出column's name，
        ///       b)若没有，则属性名
        ///    作为数据行字段查询名字（名字统一细写）
        /// 3）从数据行查出来的值，放于实例object中
        /// </summary>
        /// <param name="row">数据行</param>
        /// <generic name="T">返回的对象类型</generic>
        /// <returns>objectType指定类型的实例</returns>
        public static T Row2Object<T>(DataRow row) where T : new()
        {
            if (row == null)
                return default(T);
            IList<Object[]> proNameList = getEntityNames<T>(row.Table);
            return Row2Object<T>(proNameList, row);
        }

        /// <summary>
        /// 把Row类型数据，通过反射，转为指定类型的对象，
        /// 转换时，
        /// 1）创建objectType的实例object，遍历objectType中的所有属性
        /// 2）每一属性，先看有没有Column Attribute定义，
        ///       a)如果有，取出column's name，
        ///       b)若没有，则属性名
        ///    作为数据行字段查询名字（名字统一细写）
        /// 3）从数据行查出来的值，放于实例object中
        /// </summary>
        /// <param name="row">数据行</param>
        /// <generic name="T">返回的对象类型</generic>
        /// <returns>objectType指定类型的实例</returns>
        private static T Row2Object<T>(IList<Object[]> proNameList, DataRow row) where T : new()
        {
            T t = new T();  //t is temp
            for (int i = 0; i < proNameList.Count; i++)
            {
                PropertyInfo prop = proNameList[i][0] as PropertyInfo;
                string strColumnName = proNameList[i][1] as string;
                var value = row[strColumnName];
                if (value != null && value != DBNull.Value)
                    prop.SetValue(t, value, null);
            }
            return t;
        }

        private static IList<Object[]> getEntityNames<T>(DataTable dt)
        {
            PropertyInfo[] props = typeof(T).GetProperties();
            IList<Object[]> result = new List<Object[]>();

            foreach (PropertyInfo prop in props)
            {
                string proName = prop.Name;
                object[] colAtt = prop.GetCustomAttributes(typeof(ColumnAttribute), true);
                if (colAtt.Length > 0)//if true,it has ColumnAttribute
                {
                    ColumnAttribute colum = colAtt[0] as ColumnAttribute;
                    if (colum != null && colum.Name != null)
                    {
                        proName = colum.Name;
                    }
                }

                if (dt != null)
                {
                    if (dt.Columns.Contains(proName))
                    {
                        Object[] objTemp = new Object[] { prop, proName };
                        result.Add(objTemp);
                    }
                }
                else
                {
                    Object[] objTemp = new Object[] { prop, proName };
                    result.Add(objTemp);
                }
            }
            return result;
        }

        #region old logic 
        ///// <summary>
        ///// 把Row类型数据，通过反射，转为指定类型的对象，
        ///// 转换时，
        ///// 1）创建objectType的实例object，遍历objectType中的所有属性
        ///// 2）每一属性，先看有没有Column Attribute定义，
        /////       a)如果有，取出column's name，
        /////       b)若没有，则属性名
        /////    作为数据行字段查询名字（名字统一细写）
        ///// 3）从数据行查出来的值，放于实例object中
        ///// </summary>
        ///// <param name="row">数据行</param>
        ///// <generic name="T">返回的对象类型</generic>
        ///// <returns>objectType指定类型的实例</returns>
        ///// author=Edgar Ng
        //public static T Row2Object<T>(DataRow row) where T : new()
        //{
        //    T obj = new T();
        //    PropertyInfo[] props = obj.GetType().GetProperties();
        //    foreach (PropertyInfo prop in props)
        //    {
        //        string proName = prop.Name;
        //        //if it has ColumAttribute,then get ColumnAttribute name ;
        //        object[] attrs1 = prop.GetCustomAttributes(typeof(ColumnAttribute), true);
        //        if (attrs1.Length > 0)//if true,it has ColumnAttribute
        //        {
        //            ColumnAttribute colum = attrs1[0] as ColumnAttribute;
        //            if (colum != null && colum.Name != null)
        //            {
        //                proName = colum.Name;
        //            }
        //        }
        //        if (row.Table.Columns.Contains(proName))
        //        {
        //            object val = row[proName];
        //            if (val == System.DBNull.Value || val == null)
        //            {
        //                continue;
        //            }
        //            //prop.SetValue(obj, val, null);
        //            SetPropertyValue(obj, prop, val);
        //        }
        //    }
        //    return obj;
        //}

        //private static void SetPropertyValue(object obj, PropertyInfo prop, object val) 
        //{
        //    Type propType = prop.PropertyType;

        //    /*if (prop.PropertyType.Equals(val.GetType()))
        //    {
        //        prop.SetValue(obj, val, null);
        //    }
        //    else*/
        //    if (typeof(double).Equals(propType))
        //    {
        //        prop.SetValue(obj, Convert.ToDouble(val), null);
        //    }
        //    else 
        //    {
        //        prop.SetValue(obj, val, null);
        //    }
        //}

        ///// <summary>
        ///// 把Table中的数据，通过反射，转为指定类型的对象列表
        ///// </summary>
        ///// author=Edgar Ng
        ///// <param name="DataTable">数据表</param>
        ///// <generic name="T">返回列表中，元素的对象类型</generic>
        ///// <returns>objectType指定类型的列表</returns>
        //public static IList<T> Table2List<T>(DataTable table) where T : new()
        //{
        //    IList<T> result = new List<T>(table.Rows.Count);
        //    T obj = new T();
        //    PropertyInfo[] props = obj.GetType().GetProperties();
        //    IList<Object[]> proNameList = getEntityName<T>(props, table);//Object[]:Object[]{PropertyInfo,string}
        //    foreach (DataRow row in table.Rows)
        //    {
        //        T t = new T();  //t is temp
        //        for (int i = 0; i < proNameList.Count; i++)
        //        {
        //            PropertyInfo prop = proNameList[i][0] as PropertyInfo;
        //            string strColumnName = proNameList[i][1] as string;
        //            prop.SetValue(t, row[strColumnName], null);
        //        }
        //        result.Add(t);
        //    }
        //    return result;
        //}

        ///// <summary>
        ///// to support the function Table2List(): --by edgar
        ///// </summary>
        //private static IList<Object[]> getEntityName<T>(PropertyInfo[] props, DataTable dt)
        //{
        //    IList<Object[]> result = new List<Object[]>();
        //    DataRow drTemp = dt.Rows[0];
        //    foreach (PropertyInfo prop in props)
        //    {
        //        string proName = prop.Name;
        //        object[] colAtt = prop.GetCustomAttributes(typeof(ColumnAttribute), true);
        //        if (colAtt.Length > 0)//if true,it has ColumnAttribute
        //        {
        //            ColumnAttribute colum = colAtt[0] as ColumnAttribute;
        //            if (colum != null)
        //            {
        //                proName = colum.Name;
        //            }
        //        }
        //        if (drTemp.Table.Columns.Contains(proName))
        //        {
        //            Object[] objTemp = new Object[] { prop, proName };
        //            result.Add(objTemp);
        //        }
        //    }
        //    return result;
        //}

        #endregion

        /// <summary>
        /// get connection Object by dbType & connect String
        /// </summary>
        /// <param name="dbType"></param>
        /// <param name="connectString"></param>
        /// <returns></returns>
        public static IDbConnection GetConnection(string dbName)
        {
            return GetConnection(ConfigCache.GetDBType(dbName), ConfigCache.GetDBConnectStr(dbName));
        }

        public static IDbConnection GetConnection(short dbType, string connectString)
        {
            
            if (dbType == ADOTemplate.DB_TYPE_ORACLE)
            {
                return OracleConnectionFactory.Get(connectString);
            }
            else if (dbType == ADOTemplate.DB_TYPE_SQLSERVER)
            {
                return new SqlConnection(connectString);
            }
            else
            {
                return MySqlConnectionFactory.Get(connectString);
            }
        }
        /*
            public static String getCountSql(final String sql) {
    	
        String tempSql = sql.toLowerCase().trim() ;
        
        int beginPos = 7; // start after select
        int nextFormPos = tempSql.indexOf("from ", beginPos);
        int nextSelectPos = tempSql.indexOf("select ", beginPos);
        
        while (nextSelectPos >0 && nextFormPos > nextSelectPos) {
        	beginPos = nextFormPos + 4 ;

            nextFormPos = tempSql.indexOf("from ", beginPos);
            nextSelectPos = tempSql.indexOf("select ", beginPos);
        }
        
    	
        int endPos = tempSql.indexOf("order by");
        
        if (endPos > 0) {
            return "SELECT COUNT(*) " + sql.trim().substring(nextFormPos, endPos -1);
        } else {
            return "SELECT COUNT(*) " + sql.trim().substring(nextFormPos);
        }
    }*/
    }

    /// <summary>
    /// 临时类，为了测试使用
    /// </summary>
    class OracleConnectionFactory
    {
        // just because unit test throw exception!!
        public static IDbConnection Get(string connectString)
        {
            return new Oracle.DataAccess.Client.OracleConnection(connectString);
        }
    }
    /// <summary>
    /// 临时类，为了测试使用
    /// </summary>
    class MySqlConnectionFactory
    {
        // just because unit test throw exception!!
        public static IDbConnection Get(string connectString)
        {
            return new MySql.Data.MySqlClient.MySqlConnection(connectString);
        }
    }
}
