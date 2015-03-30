using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using EG.Utility.DBCommon.dao;
/*****************************************************
* 目的：微信群发消息表DA
* 创建人：林子聪
* 创建时间：20141229
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Platform.DA
{
    public class WXGsMessageDA
    {
        #region 数据库结构
        public static readonly string TABLE_NAME_MP = "V_WC_GSMESSAGE";
        public static readonly string TABLE_NAME_QY = "V_WC_QYMESSAGE";
        public static readonly string FIELD_NAME_ID = "id";
        public static readonly string FIELD_NAME_USERID = "userid";
        public static readonly string FIELD_NAME_MTIME = "mtime";
        public static readonly string FIELD_NAME_STIME = "stime";
        public static readonly string FIELD_NAME_STYPE = "stype";
        public static readonly string FIELD_NAME_STARGET = "starget";
        public static readonly string FIELD_NAME_CONTENTTYPE = "contenttype";
        public static readonly string FIELD_NAME_SCONTENT = "scontent";
        public static readonly string FIELD_NAME_SSTATE = "sstate";
        public static readonly string FIELD_NAME_SLOG = "slog";
        public static readonly string FIELD_NAME_STARGETNAME = "stargetname";
        public static readonly string FIELD_NAME_STARGETCOUNT = "stargetcount";
        public static readonly string PRO_NAME_UPDATE = "PRO_WC_GSMESSAGE_UPDATE";
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
        /// 用户编号
        /// </summary>
        public string UserID
        {
            get;
            set;
        }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime MTime
        {
            get;
            set;
        }
        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime STime
        {
            get;
            set;
        }
        /// <summary>
        /// 发送类型
        /// </summary>
        public int SType
        {
            get;
            set;
        }
        /// <summary>
        /// 发送目标
        /// </summary>
        public string STarget
        {
            get;
            set;
        }
        /// <summary>
        /// 内容类型
        /// 文本：text、图片：image、音频：voice、视频：video、图文：mpnews
        /// </summary>
        public string ContentType
        {
            get;
            set;
        }
        /// <summary>
        /// 内容
        /// 文本消息：文本内容；其他消息：mediaId
        /// </summary>
        public string SContent
        {
            get;
            set;
        }
        /// <summary>
        /// 发送状态
        /// 待审核：1；审核通过：2；已发送：3；审核未通过：4
        /// </summary>
        public int SState
        {
            get;
            set;
        }
        /// <summary>
        /// 异常信息        
        /// </summary>
        public string SLog
        {
            get;
            set;
        }
        /// <summary>
        /// 微信类型：1公众  2企业
        /// </summary>
        public int wx_type
        {
            get;
            set;
        }
        /// <summary>
        /// 应用ID
        /// </summary>
        public int agentid
        {
            get;
            set;
        }
        /// <summary>
        /// 保密消息
        /// </summary>
        public int safe
        {
            get;
            set;
        }
        /// <summary>
        /// 发送目标群組名稱
        /// </summary>
        public string STargetName
        {
            get;
            set;
        }
        /// <summary>
        /// 发送目标群組用戶數量
        /// </summary>
        public int STargetCount
        {
            get;
            set;
        }
        #endregion

        #region 操作
        /// <summary>
        /// 数据访问接口
        /// </summary>
        private ADOTemplateX template = new ADOTemplateX();
        /// <summary>
        /// 获取所有群发消息
        /// </summary>
        /// <returns></returns>
        public DataTable GetGsMessage(string pFilter, int wxType)
        {
            string strSql = string.Empty;
            var tableN = wxType == 1 ? TABLE_NAME_MP : TABLE_NAME_QY;

            if (string.IsNullOrEmpty(pFilter))
                strSql = string.Format("select * from {0} order by {1} desc", tableN, FIELD_NAME_MTIME);
            else
                strSql = string.Format("select * from {0} {1} order by {2} desc", tableN, pFilter, FIELD_NAME_MTIME);
            return template.Query(strSql, null, null, null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pDic"></param>
        /// <returns></returns>
        public DataTable GetGsMessage(IDictionary<string, object> pDic, int wxType)
        {
            if (pDic == null || pDic.Count == 0)
            {
                return GetGsMessage(string.Empty, wxType);
            }
            var tableN = wxType == 1 ? TABLE_NAME_MP : TABLE_NAME_QY;
            //生成where条件字句
            Dictionary2Where pOper = new Dictionary2Where();
            pOper.parse(pDic);
            string strFilter = pOper.AsSql();
            string[] pPara = pOper.ParameterNames.Select(p => "@" + p).ToArray();
            object[] pValue = pOper.ParameterValues.ToArray();
            //查询SQL
            string strSql = string.Format("select * from {0} {1} order by {2} desc", tableN, strFilter, FIELD_NAME_MTIME);
            return template.Query(strSql, pPara, pValue, null);
        }
        /// <summary>
        /// 插入信息表
        /// </summary>
        /// <param name="pUserID">操作人员编号</param>
        /// <param name="pCreateTime">创建时间</param>
        /// <param name="pContentType">内容类型</param>
        /// <param name="pContent">内容</param>
        /// <param name="pState">状态</param>
        /// <param name="pLog">异常信息</param>
        /// <returns></returns>
        public bool InsertGsMessage(string pUserID, DateTime pCreateTime, int pSendType, string pSendTarget, string pContentType, object pContent, int pState, int wx_type, int agendId, int safe)
        {
            int result = template.Execute(PRO_NAME_UPDATE, new string[] { "@Pid", "@Puserid", "@Pmtime", "@Pstime", "@Pstype", "@Pstarget", "@Pcontenttype", "@Pscontent", "@Psstate", "@Pslog", "@Pwx_type", "@Pagentid", "@Psafe" },
                new object[] { null, pUserID, pCreateTime, null, pSendType, pSendTarget, pContentType, pContent, pState, null, wx_type, agendId, safe }, null, CommandType.StoredProcedure);
            return result > 0;
        }
        /// <summary>
        /// 更新信息表
        /// </summary>
        /// <param name="pId">消息编号</param>
        /// <param name="pUserID">操作人员编号</param>
        /// <param name="pSendTime">发送时间</param>
        /// <param name="pSendType">发送类型</param>
        /// <param name="pSendTarget">发送目标对象</param>
        /// <param name="pState">状态</param>
        /// <param name="pLog">异常信息</param>
        /// <returns></returns>
        public bool UpdateGsMessage(int pId, string pUserID, DateTime pSendTime, int pSendType, string pSendTarget, int pState, string pLog)
        {
            int result = template.Execute(PRO_NAME_UPDATE, new string[] { "@Pid", "@Puserid", "@Pmtime", "@Pstime", "@Pstype", "@Pstarget", "@Pcontenttype", "@Pscontent", "@Psstate", "@Pslog", "@Pwx_type", "@Pagentid", "@Psafe" }, new object[] { pId, pUserID, null, pSendTime, pSendType, pSendTarget, null, null, pState, pLog, null, null, null }, null, CommandType.StoredProcedure);
            return result > 0;
        }
        #endregion

        #region 辅助
        /// <summary>
        /// 生成查询项
        /// </summary>
        /// <param name="pQueryItems"></param>
        /// <returns></returns>
        public static IDictionary<string, object> CreateDicItems(List<object> pQueryItems)
        {
            //生成where条件字句
            IDictionary<string, object> pDic = new Dictionary<string, object>();
            if (pQueryItems != null && pQueryItems.Count > 0 && pQueryItems[0] != null)
                pDic.Add(WXGsMessageDA.FIELD_NAME_ID, pQueryItems[0]);
            if (pQueryItems != null && pQueryItems.Count > 1 && pQueryItems[1] != null)
                pDic.Add(WXGsMessageDA.FIELD_NAME_USERID, pQueryItems[1]);
            if (pQueryItems != null && pQueryItems.Count > 2 && pQueryItems[2] != null)
                pDic.Add(WXGsMessageDA.FIELD_NAME_MTIME, pQueryItems[2]);
            if (pQueryItems != null && pQueryItems.Count > 3 && pQueryItems[3] != null)
                pDic.Add(WXGsMessageDA.FIELD_NAME_STIME, pQueryItems[3]);
            if (pQueryItems != null && pQueryItems.Count > 4 && pQueryItems[4] != null)
                pDic.Add(WXGsMessageDA.FIELD_NAME_STYPE, pQueryItems[4]);
            if (pQueryItems != null && pQueryItems.Count > 5 && pQueryItems[5] != null)
                pDic.Add(WXGsMessageDA.FIELD_NAME_STARGET, pQueryItems[5]);
            if (pQueryItems != null && pQueryItems.Count > 6 && pQueryItems[6] != null)
                pDic.Add(WXGsMessageDA.FIELD_NAME_CONTENTTYPE, pQueryItems[6]);
            if (pQueryItems != null && pQueryItems.Count > 7 && pQueryItems[7] != null)
                pDic.Add(WXGsMessageDA.FIELD_NAME_SCONTENT, pQueryItems[7]);
            if (pQueryItems != null && pQueryItems.Count > 8 && pQueryItems[8] != null)
                pDic.Add(WXGsMessageDA.FIELD_NAME_SSTATE, pQueryItems[8]);
            if (pQueryItems != null && pQueryItems.Count > 9 && pQueryItems[9] != null)
                pDic.Add(WXGsMessageDA.FIELD_NAME_SLOG, pQueryItems[9]);

            return pDic;
        }
        #endregion
    }
}
