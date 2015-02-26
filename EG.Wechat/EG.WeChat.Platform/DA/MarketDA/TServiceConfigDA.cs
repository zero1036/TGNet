using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
/*****************************************************
* 目的：定制服务表DA
* 创建人：林子聪
* 创建时间：20141216
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Platform.DA
{
    public class TServiceConfigDA
    {
        #region 数据库结构
        public static readonly string TABLE_NAME = "T_SERVICECONFIG";
        public static readonly string FIELD_NAME_ID = "id";
        public static readonly string FIELD_NAME_CKEY = "ckey";
        public static readonly string FIELD_NAME_CTYPE = "ctype";
        public static readonly string FIELD_NAME_CDLL = "cdll";
        public static readonly string FIELD_NAME_CNAMESPACE = "cnamespace";
        public static readonly string FIELD_NAME_CCLASS = "cclass";
        public static readonly string FIELD_NAME_CMETHOD = "cmethod";
        public static readonly string FIELD_NAME_CURL = "curl";
        public static readonly string FIELD_NAME_CTIME = "ctime";
        #endregion

        #region 单例模式
        //public static TServiceConfigDA Singleton = new TServiceConfigDA();
        /// <summary>
        /// 数据访问接口
        /// </summary>
        private ADOTemplateX template = new ADOTemplateX();
        #endregion

        #region 属性
        /// <summary>
        /// ID
        /// </summary>
        public int ID
        {
            get;
            set;
        }
        /// <summary>
        /// 全局标识
        /// </summary>
        public string CKey
        {
            get;
            set;
        }
        /// <summary>
        /// 服务类型
        /// [ctype] [int] NOT NULL check(ctype=1 or ctype=2),
        /// </summary>
        public int CType
        {
            get;
            set;
        }
        /// <summary>
        /// DLL
        /// </summary>
        public string CDLL
        {
            get;
            set;
        }
        /// <summary>
        /// 服务命名空间
        /// </summary>
        public string CNamespace
        {
            get;
            set;
        }
        /// <summary>
        /// 类
        /// </summary>
        public string CClass
        {
            get;
            set;
        }
        /// <summary>
        /// 执行方法
        /// </summary>
        public string CMethod
        {
            get;
            set;
        }
        /// <summary>
        /// 服务地址
        /// </summary>
        public string CUrl
        {
            get;
            set;
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public int CTime
        {
            get;
            set;
        }
        #endregion

        #region 操作
        /// <summary>
        /// 从数据库中所有定制服务
        /// </summary>
        /// <returns></returns>
        public DataTable GetCServices()
        {
            string strSql;
            strSql = string.Format("select * from {0}", TABLE_NAME);
            return template.Query(strSql, null, null, null);
        }
        #endregion


    }
}
