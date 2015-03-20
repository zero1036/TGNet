using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EG.Business.Common;
using EG.Utility.DBCommon;
using EG.Utility.DBCommon.dao;
using System.Data;
using System.Data.SqlClient;
/*****************************************************
* 目的：组织DA——微信用户
* 创建人：林子聪
* 创建时间：20141107
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Platform.DA
{
    public class WeChatUserDA : WeChatOrgAOP
    {
        #region 私有成员
        /// <summary>
        /// 数据访问接口
        /// </summary>
        private ADOTemplateX template = new ADOTemplateX();

        #region 保存微信用户
        private string m_tableName_WCUSer = "WC_USER";
        private string m_proceName_SaveUser_ByTable = "PRO_WC_USER_UPDATE";
        private string m_paraName_SaveUser_ByTable = "@tb";
        private string m_proceName_GetUser = "select * from WC_USER";
        private string m_paraName_GetUser = string.Empty;
        #endregion
        #endregion

        #region 公有成员
        /// <summary>
        /// 保存微信用户至数据库
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public override bool SaveUser(DataTable dt)
        {
            int result = template.Execute(m_proceName_SaveUser_ByTable, new string[] { m_paraName_SaveUser_ByTable }, new object[] { dt }, null, CommandType.StoredProcedure);
            return result > 0;
        }
        /// <summary>
        /// 从数据库中获取微信用户
        /// </summary>
        /// <param name="openID">指定openid，或设为空查询全部</param>
        /// <returns></returns>
        public DataTable GetUser(string openID = "")
        {
            //return template.Query(m_proceName_GetUser, new string[] { m_paraName_GetUser }, new object[] { openID },null);

            return template.Query(m_proceName_GetUser, null, null, null);
        }
        /// <summary>
        /// 通过OpenID更新GroupID
        /// </summary>
        /// <param name="GroupID"></param>
        /// <param name="OpenID"></param>
        /// <returns></returns>
        public override int UpdateGroupIDByOpenID(int GroupID, string OpenID)
        {
            string sql = string.Format(@"UPDATE [dbo].[{0}] SET [groupid]=@GroupID  WHERE [openid]=@OpenID ", m_tableName_WCUSer);

            int result = template.Execute(sql, new string[] { "@GroupID", "@OpenID" }, new object[] { GroupID, OpenID }, null, CommandType.Text);

            return result;
        }
        #endregion

    }
    public class ADOTemplateX : ADOTemplate
    {
        /// <summary>
        /// 
        /// </summary>
        protected CommandType m_CommandType = CommandType.StoredProcedure;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paramNames"></param>
        /// <param name="paramValues"></param>
        /// <returns></returns>
        public new int Execute(string sql, String[] paramNames, Object[] paramValues)
        {
            //if (this.Transaction == null)
            //{
            //    TransactionContext tranContext = TransactionContext.get();
            //    this.dbType = tranContext.dbType;
            //    this.Transaction = tranContext.transaction;
            //}

            if (this.dbType == ADOTemplate.DB_TYPE_SQLSERVER)
            {

                SqlParameter[] parm = convert4Sqlserver(paramNames, paramValues);

                return SQLHelper.ExecuteNonQuery(
                    this.Transaction as SqlTransaction, m_CommandType, sql, parm
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

                return 1; /*return OracleHelper.ExecuteNonQuery(
                    this.Transaction, m_CommandType, sql, parm
                    );*/

            }

        }

        public   int ExecuteEX(string sql, String[] paramNames, Object[] paramValues)
        {
            //if (this.Transaction == null)
            //{
            //    TransactionContext tranContext = TransactionContext.get();
            //    this.dbType = tranContext.dbType;
            //    this.Transaction = tranContext.transaction;
            //}

            if (this.dbType == ADOTemplate.DB_TYPE_SQLSERVER)
            {

                SqlParameter[] parm = convert4Sqlserver(paramNames, paramValues);

                SqlConnection con =   (SqlConnection)(DBUtil.GetConnection("EGWeChat"));
                return SQLHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, sql, parm);
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

                return 1; /*return OracleHelper.ExecuteNonQuery(
                    this.Transaction, m_CommandType, sql, parm
                    );*/

            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paramNames"></param>
        /// <param name="paramValues"></param>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public new int Execute(string sql, String[] paramNames, Object[] paramValues, string dbName, CommandType pCommandType)
        {
            using (Connection = DBUtil.GetConnection(dbName))
            {
                try
                {
                    Connection.Open();
                    this.dbType = ConfigCache.GetDBType(dbName);
                    this.Transaction = this.Connection.BeginTransaction();
                    m_CommandType = pCommandType;
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paramNames"></param>
        /// <param name="paramValues"></param>
        /// <returns></returns>
        public new DataSet ExecuteX(string sql, String[] paramNames, Object[] paramValues)
        {
            //if (this.Transaction == null)
            //{
            //    TransactionContext tranContext = TransactionContext.get();
            //    this.dbType = tranContext.dbType;
            //    this.Transaction = tranContext.transaction;
            //}

            if (this.dbType == ADOTemplate.DB_TYPE_SQLSERVER)
            {

                SqlParameter[] parm = convert4Sqlserver(paramNames, paramValues);

                return SQLHelper.ExecuteDataset(
                    this.Transaction as SqlTransaction, m_CommandType, sql, parm
                    );
            }
            else if (this.dbType == ADOTemplate.DB_TYPE_MYSQL)
            {

                IDbDataParameter[] parm = convert4MySqlserver(paramNames, paramValues);

                return null;/* MySQLHelper.ExecuteNonQuery(
                     this.Transaction, CommandType.Text, sql, parm
                     );*/
            }
            else
            {
                IDbDataParameter[] parm = convert4Oracle(paramNames, paramValues);

                return null; /*return OracleHelper.ExecuteNonQuery(
                    this.Transaction, m_CommandType, sql, parm
                    );*/

            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paramNames"></param>
        /// <param name="paramValues"></param>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public new DataSet ExecuteX(string sql, String[] paramNames, Object[] paramValues, string dbName, CommandType pCommandType)
        {
            using (Connection = DBUtil.GetConnection(dbName))
            {
                try
                {
                    Connection.Open();
                    this.dbType = ConfigCache.GetDBType(dbName);
                    this.Transaction = this.Connection.BeginTransaction();
                    m_CommandType = pCommandType;
                    DataSet result = ExecuteX(sql, paramNames, paramValues);
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramNames"></param>
        /// <param name="paramValues"></param>
        /// <returns></returns>
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

                Type t = paramValues[i].GetType();
              
                    result[i] = new SqlParameter(name, paramValues[i]);
              

                
            }

            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramNames"></param>
        /// <param name="paramValues"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramNames"></param>
        /// <param name="paramValues"></param>
        /// <returns></returns>
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
}
