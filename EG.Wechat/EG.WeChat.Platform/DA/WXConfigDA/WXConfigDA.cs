using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using EG.WeChat.Platform.Model;
using EG.Utility.DBCommon.dao;
using EG.WeChat.Utility.Tools;
/*****************************************************
* 目的：微信基础配置表DA
* 创建人：林子聪
* 创建时间：20150313
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Platform.DA
{
    public class WXConfigDA
    {
        #region 数据库结构
        public static readonly string TABLE_NAME = "WC_CONFIG";
        public static readonly string PRO_UPDATE_NAME = "PRO_WC_CONFIG_UPDATE";
        public static readonly string FIELD_NAME_ID = "id";
        public static readonly string FIELD_NAME_ACTYPE = "actype";
        public static readonly string FIELD_NAME_ACID = "acid";
        public static readonly string FIELD_NAME_ACSECRET = "acsecret";
        public static readonly string FIELD_NAME_TOKEN = "token";
        public static readonly string FIELD_NAME_AESKEY = "aeskey";
        public static readonly string FIELD_NAME_AID = "aid";
        #endregion

        #region 操作
        /// <summary>
        /// 数据访问接口
        /// </summary>
        private ADOTemplateX template = new ADOTemplateX();
        /// <summary>
        /// 获取微信配置
        /// </summary>
        /// <returns></returns>
        public DataTable GetWXConfig()
        {
            string strSql = string.Format("select * from {0}", TABLE_NAME);
            return template.Query(strSql, null, null, null);
        }
        /// <summary>
        /// 更新配置
        /// </summary>
        /// <param name="dt"></param>
        public bool SetWXConfig(DataTable dt)
        {
            DataSet ds = template.ExecuteX(PRO_UPDATE_NAME, new string[] { "@tb" }, new object[] { dt }, null, CommandType.StoredProcedure);
            if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0)
                return true;
            return false;
        }

        #endregion

    }
}
