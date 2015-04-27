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
* 目的：组织DA——微信用户分组
* 创建人：林子聪
* 创建时间：20141107
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace TW.Platform.DA
{
    /// <summary>
    /// 执行类
    /// </summary>
    public class WeChatGroupDA : WeChatOrgAOP
    {
        #region 私有成员
        /// <summary>
        /// 数据访问接口
        /// </summary>
        private ADOTemplateX template = new ADOTemplateX();

        #region 保存微信用户
        private string m_tableName_WCGROUP = "WC_GROUP";
        private string m_proceName_SaveGroup_ByTable = "PRO_WC_GROUP_UPDATE";
        private string m_paraName_SaveGroup_ByTable = "@tb";
        private string m_proceName_GetGroup = "select * from WC_GROUP";
        private string m_paraName_GetGroup = string.Empty;
        private string m_proceName_ReloadGroup = "PRO_WC_GROUP_RELOADX";
        //private string m_paraName_ReloadGroup = "@pgroupid";
        #endregion
        #endregion

        #region 公有成员
        /// <summary>
        /// 保存微信分组信息至数据库
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public override bool SaveGroup(DataTable dt)
        {
            int result = template.Execute(m_proceName_SaveGroup_ByTable, new string[] { m_paraName_SaveGroup_ByTable }, new object[] { dt }, null, CommandType.StoredProcedure);
            return result > 0;
        }
        /// <summary>
        /// 从数据库中获取微信分组信息
        /// </summary>
        /// <param name="openID">指定openid，或设为空查询全部</param>
        /// <returns></returns>
        public DataTable GetGroup(string openID = "")
        {
            //return template.Query(m_proceName_GetUser, new string[] { m_paraName_GetUser }, new object[] { openID },null);

            return template.Query(m_proceName_GetGroup, null, null, null);
        }
        /// <summary>
        /// 同步目标分组ID在分组表与用户表的数量数据
        /// </summary>
        /// <returns></returns>
        public override bool ReloadGroup()
        {
            int result = template.Execute(m_proceName_ReloadGroup, null, null, null, CommandType.StoredProcedure);
            return result > 0;
        }
        #endregion
    }
}
