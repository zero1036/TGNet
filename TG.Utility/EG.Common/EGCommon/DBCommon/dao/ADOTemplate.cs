using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
//using System.Data.OracleClient;
using EG.Utility.DBCommon;
using System.Reflection;
using System.Data.Linq.Mapping;
using EG.Common.Entity;
using EG.Business.Common;

namespace EG.Utility.DBCommon.dao
{
    public class ADOTemplate
    {
        public const short DB_TYPE_MYSQL = 5;
        public const short DB_TYPE_SQLSERVER = 1;
        public const short DB_TYPE_ORACLE = 0;
        public const short DB_TYPE_XML = 9;

        public IDbConnection Connection { get; set; }
        public IDbTransaction Transaction { get; set; }
        public short dbType { get; set; }
        public String dbName { get; set; }

        private Object2Insert obj2Insert;
        private Object2Update obj2Update;
        private Object2Delete obj2Delete;
        private Object2Get obj2Get;
        private Object2Find obj2Find;

        private Object2Insert getObject2Insert()
        {
            if (this.obj2Insert == null)
            {
                this.obj2Insert = new Object2Insert();
                this.obj2Insert.adoTemplate = this;
            }
            return this.obj2Insert;
        }

        private Object2Update getObject2Update()
        {
            if (this.obj2Update == null)
            {
                this.obj2Update = new Object2Update();
            }

            return this.obj2Update;
        }

        private Object2Delete getObject2Delete()
        {
            if (this.obj2Delete == null)
            {
                this.obj2Delete = new Object2Delete();
            }

            return this.obj2Delete;
        }

        private Object2Get getObject2Get()
        {
            if (this.obj2Get == null)
            {
                this.obj2Get = new Object2Get();
            }

            return this.obj2Get;
        }

        private Object2Find getObject2Find()
        {
            if (this.obj2Find == null)
            {
                this.obj2Find = new Object2Find();
            }

            return this.obj2Find;
        }

        /// <summary>
        /// 保存一个对象到数据库中，保存前，根据对象的Maping信息，生成对应的insert sql.
        /// </summary>
        /// <param name="tableObject">含Mapping信息的对象. 自增长列需要加 IsDbGenerated = true, 只支持SqlServer</param>
        /// <returns>自增长列的值，没有自增长列时返回0</returns>
        public int Insert(Object tableObject)
        {
            Object2Insert obj2Insert = getObject2Insert();
            obj2Insert.parse(tableObject);

            int result = 0;
            // SqlServer中包括Identity列时，需要返回Identity的值
            if (obj2Insert.IsSqlServerIdentityTable)
            {
                var table = this.Query(obj2Insert.AsSql4ServerIdentityTable(), obj2Insert.GetSqlParameterNames(), obj2Insert.GetSqlParameterValues());
                result = (int)table.Rows[0][0];
                var id = table.Rows[0][1];
                obj2Insert.OutputValues.Add(id);
            }
            else
            {
                result = this.Execute(obj2Insert.AsSql(), obj2Insert.GetSqlParameterNames(), obj2Insert.GetSqlParameterValues());
            }

            obj2Insert.SetOutputValues(tableObject);

            return result;
        }


        /// <summary>
        /// 更新一个对象到数据库中，更新前，根据对象的Maping信息，生成对应的update sql.
        /// 当前，只支持全属性更新操作
        /// </summary>
        /// <param name="tableObject">含Mapping信息的对象</param>
        /// <returns>已执行处理的记录数</returns>
        public int Update(Object tableObject)
        {
            Object2Update obj2Update = getObject2Update();
            obj2Update.parse(tableObject);

            return this.Execute(obj2Update.AsSql(), obj2Update.GetSqlParameterNames(), obj2Update.GetSqlParameterValues());
        }

        /// <summary>
        /// 根据对象的主键，删除数据库记录
        /// </summary>
        /// <param name="tableObject">含Mapping信息的对象</param>
        /// <returns>已执行处理的记录数</returns>
        public int Delete(Object tableObject)
        {
            Object2Delete obj2Delete = getObject2Delete();
            obj2Delete.parse(tableObject);

            return this.Execute(obj2Delete.AsSql(), obj2Delete.GetSqlParameterNames(), obj2Delete.GetSqlParameterValues());
        }

        /// <summary>
        /// 通过物理主键，从数据库中，查找记录，并根据Mapping信息，把数据封装成对象返回
        /// </summary>
        /// <param name="tableObject">含Mapping信息的对象</param>
        /// <returns>具体对象</returns>
        public T Get<T>(T tableObject) where T : new()
        {
            Object2Get obj2Get = getObject2Get();

            obj2Get.parse(tableObject);
            DataRow row = this.Get(obj2Get.AsSql(), obj2Get.GetSqlParameterNames(), obj2Get.GetSqlParameterValues());

            T result = default(T);
            if (row == null)
            {
                return result;
            }
            else
            {
                return DBUtil.Row2Object<T>(row);
            }

        }

        /// <summary>
        /// 通过逻辑主键，从数据库中，查找记录，并根据Mapping信息，把数据封装成对象返回
        /// </summary>
        /// <param name="tableObject">含Mapping信息的对象</param>
        /// <returns>具体对象</returns>
        public T Find<T>(T tableObject) where T : new()
        {
            Object2Find Obj2Find = getObject2Find();

            obj2Find.parse(tableObject);
            DataRow row = this.Find(Obj2Find.AsSql(), Obj2Find.GetSqlParameterNames(), Obj2Find.GetSqlParameterValues());



            T result = default(T);
            if (row == null)
            {
                return result;
            }
            else
            {
                return DBUtil.Row2Object<T>(row);
            }
        }

        public int GetInt(String sql)
        {
            return GetInt(sql, null, null);
        }

        public int GetInt(String sql, String[] paramNames, Object[] paramValues)
        {
            return GetInt(sql, paramNames, paramValues, 0);
        }

        public int GetInt(String sql, String[] paramNames, Object[] paramValues, int defaultValue)
        {
            object obj = GetObject(sql, paramNames, paramValues);

            if (obj == null)
            {
                return defaultValue;
            }
            else
            {
                return (int)obj;
            }
        }

        public long GetLong(String sql)
        {
            return GetLong(sql, null, null);
        }

        public long GetLong(String sql, String[] paramNames, Object[] paramValues)
        {
            return GetLong(sql, paramNames, paramValues, 0);
        }

        public long GetLong(String sql, String[] paramNames, Object[] paramValues, long defaultValue)
        {
            object obj = GetObject(sql, paramNames, paramValues);

            if (obj == null)
            {
                return defaultValue;
            }
            else //if (obj is decimal)
            {
                return Convert.ToInt64(obj);
            }
        }

        public double GetDouble(String sql)
        {
            return GetDouble(sql, null, null);
        }

        public double GetDouble(String sql, String[] paramNames, Object[] paramValues)
        {
            return GetDouble(sql, paramNames, paramValues, 0);
        }

        public double GetDouble(String sql, String[] paramNames, Object[] paramValues, double defaultValue)
        {
            object obj = GetObject(sql, paramNames, paramValues);

            if (obj == null)
            {
                return defaultValue;
            }
            else
            {
                return Convert.ToDouble(obj);
            }
        }

        public object GetObject(String sql, String[] paramNames, Object[] paramValues)
        {
            DataTable _table = Query(sql, paramNames, paramValues);

            if (_table.Rows.Count == 0)
            {
                return null;
            }

            return _table.Rows[0][0];
        }


        public DataRow Get(string sql, String[] paramNames, Object[] paramValues)
        {
            DataTable table = this.Query(sql, paramNames, paramValues);

            if (table.Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return table.Rows[0];
            }

        }

        /// <summary>
        /// SQL配置于XML中，执行SQL查询，返回一个对象
        /// </summary>
        /// <param name="TemplateModule">SQL配置在XML的Module名</param>
        /// <param name="TemplateName">SQL配置在XML中的名</param>
        /// <param name="module">具体的参数对象</param>
        /// <returns>DataRow</returns>
        public DataRow GetName(string TemplateModule, string TemplateName, object module)
        {
            string sql = SqlProvider.Get(TemplateModule, TemplateName);//这个需要先初始化sqlprovider...请参考测试用例
            return Get(sql, module);
        }

        public DataRow Get(string sql, object module)
        {
            SQLParser parser = new SQLParser();
            parser.Parse(sql, module);

            return Get(parser.AsSql(), parser.GetSqlParameterNames(), parser.GetSqlParameterValues());
        }

        public DataRow Find(string sql, string[] paramNames, object[] paramValues)
        {
            DataTable table = this.Query(sql, paramNames, paramValues);

            if (table.Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return table.Rows[0];
            }
        }

        public DataRow Find(string sql, object module)
        {
            SQLParser parser = new SQLParser();
            parser.Parse(sql, module);

            return Find(parser.AsSql(), parser.GetSqlParameterNames(), parser.GetSqlParameterValues());
        }

        /// <summary>
        /// SQL配置于XML中，执行SQL查询，返回一个对象列表
        /// </summary>
        /// <param name="TemplateModule">SQL配置在XML的Module名</param>
        /// <param name="TemplateName">SQL配置在XML中的名</param>
        /// <param name="module">具体的参数对象</param>
        /// <returns>DataTable</returns>
        public DataTable QueryName(string TemplateModule, string TemplateName, object model)
        {
            string sql = SqlProvider.Get(TemplateModule, TemplateName);//这个需要先初始化sqlprovider...请参考测试用例

            return Query(sql, model);
        }

        public DataTable Query(string sql, String[] paramNames, Object[] paramValues)
        {
            if (this.Connection == null)
            {
                TransactionContext tranContext = TransactionContext.get();
                this.Connection = tranContext.connection;
                this.Transaction = tranContext.transaction;
                this.dbType = tranContext.dbType;
            }

            if (this.dbType == ADOTemplate.DB_TYPE_SQLSERVER)
            {

                SqlParameter[] parm = convert4Sqlserver(paramNames, paramValues);

                if (this.Transaction == null)
                {
                    return SQLHelper.ExecuteDataset(
                        this.Connection as SqlConnection, CommandType.Text, sql, parm
                        ).Tables[0];
                }
                else
                {
                    return SQLHelper.ExecuteDataset(
                        this.Transaction, CommandType.Text, sql, parm
                        ).Tables[0];
                }
            }
            else if (this.dbType == ADOTemplate.DB_TYPE_MYSQL)
            {

                IDbDataParameter[] parm = convert4MySqlserver(paramNames, paramValues);

                return null;// MySQLHelper.ExecuteDataset(this.Connection, CommandType.Text, sql, parm).Tables[0];
            }
            else
            {
                IDbDataParameter[] parm = convert4Oracle(paramNames, paramValues);

                return OracleHelper.ExecuteDataset(this.Connection, CommandType.Text, sql, parm).Tables[0];
            }

        }

        public DataTable Query(string sql, object module)
        {
            SQLParser parser = new SQLParser();
            parser.Parse(sql, module);

            string limitSql = parser.AsSql();
            string[] paramNames = parser.GetSqlParameterNames();
            object[] paramValues = parser.GetSqlParameterValues();

            BaseSO so = module as BaseSO;

            if (so != null && so.PageIndex != BaseSO.PAGE_INDEX_NO_PAGE)
            {
                TransactionContext tranContext = TransactionContext.get();
                this.dbType = tranContext.dbType;

                Dialect dialect = Dialect.GetDialect(dbType);

                if (so.PageIndex != BaseSO.PAGE_INDEX_NO_TOTAL)
                {
                    string countSql = dialect.GetCountSql(limitSql);

                    so.Total = GetLong(countSql, paramNames, paramValues);
                }

                limitSql = dialect.GetLimitSql(limitSql, so.PageIndex, so.PageSize);

                string[] limitParamNames = dialect.GetLimitParamNames(so.PageIndex, so.PageSize);
                object[] limitParamValues = dialect.GetLimitParamValues(so.PageIndex, so.PageSize);

                string[] tempParamNames = new string[limitParamNames.Length + paramNames.Length];
                object[] tempParamValues = new object[tempParamNames.Length];

                paramNames.CopyTo(tempParamNames, 0);
                limitParamNames.CopyTo(tempParamNames, paramNames.Length);
                paramNames = tempParamNames;

                paramValues.CopyTo(tempParamValues, 0);
                limitParamValues.CopyTo(tempParamValues, paramValues.Length);
                paramValues = tempParamValues;
            }

            return Query(limitSql, paramNames, paramValues);
        }


        public DataTable Query(string sql, String[] paramNames, Object[] paramValues, string dbName)
        {
            using (Connection = DBUtil.GetConnection(dbName))
            {
                try
                {
                    Connection.Open();
                    this.dbType = ConfigCache.GetDBType(dbName);
                    return Query(sql, paramNames, paramValues);
                }
                finally
                {
                    if (Connection != null && ConnectionState.Open == Connection.State)
                    {
                        Connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// SQL配置于XML中，执行SQL，完成一次Insert/Update/Delete操作
        /// </summary>
        /// <param name="TemplateModule">SQL配置在XML的Module名</param>
        /// <param name="TemplateName">SQL配置在XML中的名</param>
        /// <param name="module">具体的参数对象</param>
        /// <returns>受影响的记录数</returns>
        public int ExecuteName(string TemplateModule, string TemplateName, object model)
        {
            string sql = SqlProvider.Get(TemplateModule, TemplateName);//这个需要先初始化sqlprovider...请参考测试用例
            return Execute(sql, model);
        }

        public int Execute(string sql, String[] paramNames, Object[] paramValues)
        {
            if (this.Transaction == null)
            {
                TransactionContext tranContext = TransactionContext.get();
                this.dbType = tranContext.dbType;
                this.Transaction = tranContext.transaction;
            }

            if (this.dbType == ADOTemplate.DB_TYPE_SQLSERVER)
            {

                SqlParameter[] parm = convert4Sqlserver(paramNames, paramValues);

                return SQLHelper.ExecuteNonQuery(
                    this.Transaction as SqlTransaction, CommandType.Text, sql, parm
                    );
            }
            else if (this.dbType == ADOTemplate.DB_TYPE_MYSQL)
            {

                IDbDataParameter[] parm = convert4MySqlserver(paramNames, paramValues);

                return 1;/* MySQLHelper.ExecuteNonQuery(
                     this.Transaction, CommandType.Text, sql, parm
                     );*/
            }
            else
            {
                IDbDataParameter[] parm = convert4Oracle(paramNames, paramValues);

                return OracleHelper.ExecuteNonQuery(
                    this.Transaction, CommandType.Text, sql, parm
                    );

            }

        }

        public int Execute(string sql, object module)
        {
            SQLParser parser = new SQLParser();
            parser.Parse(sql, module);

            return Execute(parser.AsSql(), parser.GetSqlParameterNames(), parser.GetSqlParameterValues());
        }

        public int Execute(string sql, String[] paramNames, Object[] paramValues, string dbName)
        {
            using (Connection = DBUtil.GetConnection(dbName))
            {
                try
                {
                    Connection.Open();
                    this.dbType = ConfigCache.GetDBType(dbName);
                    this.Transaction = this.Connection.BeginTransaction();
                    int result = Execute(sql, paramNames, paramValues);
                    this.Transaction.Commit();

                    return result;
                }
                catch (Exception e)
                {
                    this.Transaction.Rollback();
                    throw e;
                }
                finally
                {
                    if (Connection != null && ConnectionState.Open == Connection.State)
                    {
                        Connection.Close();
                    }
                }
            }
        }

        private SqlParameter[] convert4Sqlserver(String[] paramNames, Object[] paramValues)
        {
            if (paramNames == null) return null;

            var size = paramNames.Length;

            if (size == 0) return null;

            SqlParameter[] result = new SqlParameter[size];

            for (int i = 0; i < size; i++)
            {
                string name = paramNames[i];
                if (!name.StartsWith("@"))
                {
                    name = "@" + name;
                }

                result[i] = new SqlParameter(name, paramValues[i]);
            }

            return result;
        }

        private IDbDataParameter[] convert4MySqlserver(String[] paramNames, Object[] paramValues)
        {
            if (paramNames == null) return null;

            var size = paramNames.Length;

            if (size == 0) return null;

            IDbDataParameter[] result = new IDbDataParameter[size];

            for (int i = 0; i < size; i++)
            {
                string name = paramNames[i];
                if (!name.StartsWith("@"))
                {
                    name = "@" + name;
                }

                result[i] = MysqlParameterFactory.Get(name, paramValues[i]);
            }

            return result;
        }

        private IDbDataParameter[] convert4Oracle(String[] paramNames, Object[] paramValues)
        {
            if (paramNames == null) return null;

            var size = paramNames.Length;

            if (size == 0) return null;

            IDbDataParameter[] result = new IDbDataParameter[size];

            for (int i = 0; i < size; i++)
            {
                string name = paramNames[i];
                if (!name.StartsWith(":"))
                {
                    name = ":" + name;
                }

                result[i] = OracleParameterFactory.Get(name, paramValues[i]);
            }

            return result;
        }

    }


    public class OracleParameterFactory
    {
        public static IDbDataParameter Get(string name, object value)
        {
            return new Oracle.DataAccess.Client.OracleParameter(name, value);
        }
    }


    public class MysqlParameterFactory
    {
        public static IDbDataParameter Get(string name, object value)
        {
            return new MySql.Data.MySqlClient.MySqlParameter(name, value);
        }
    }


}
