using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using Senparc.Weixin.Exceptions;
using Senparc.Weixin.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Utilities;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Senparc.Weixin.MP.AdvancedAPIs;
using System.Xml;
using EG.WeChat.Service;
using EG.WeChat.Service.WeiXin;
using EG.WeChat.Utility.Tools;
using TW.Platform.Model;
using TW.Platform.DA;
using Senparc.Weixin.MP.AdvancedAPIs.GroupMessage;
/*****************************************************
* 目的：微信自动信息服务
* 创建人：林子聪
* 创建时间：20141118
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace TW.Platform.BL
{
    public class WXMessageBL : WXResourcesBL
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sdkType"></param>
        public WXMessageBL(string sdkType)
            : base(sdkType)
        { }

        #region 私有成员
        private string m_strPath = "/App_Data/config/WechatConfig.xml";
        private string m_strTargetType = "TemplateMessage";
        private string m_strRootName = "ConfigService";
        #endregion

        #region 外接方法
        #region 企业号消息
        /// <summary>
        /// 发送企业消息
        /// </summary>
        /// <param name="lcId"></param>
        /// <param name="messageType"></param>
        /// <param name="toUser"></param>
        /// <param name="toParty"></param>
        /// <param name="toTag"></param>
        /// <param name="agentId"></param>
        public void SendQYMessage(string messageid, string userId, string keyW, string messageType, string toUser, string toParty, string toTag, string agentId, int safe = 0)
        {
            this.ExecuteTryCatch(() =>
            {
                #region
                //messageid = "11";
                //userId = "22";
                //keyW = "1dI_2UQ-kLaMBJ4boKzGopBnisZW2-yP4Z9UG3O00nYBrN9J_zyBX1pxMj9OLqtIo";
                //messageType = "video";
                //toUser = "@all";
                //toParty = "@all";
                //toTag = "@all";
                //agentId = "47";
                #endregion

                int lcId;
                object objKeyW = null;
                if (messageType == "news" || messageType == "mpnews")
                {
                    if (int.TryParse(keyW, out lcId)) objKeyW = lcId;
                }
                else
                    objKeyW = keyW;
                if (objKeyW == null) EGExceptionOperator.ThrowX<Exception>("素材已失效", EGActionCode.未知错误);

                ISdkETS_QY pSdk = QYSdkETS.Singleon;
                //获取素材数据方法并转换
                var resConvertFunc = pSdk.GetResConvert(messageType);
                var content = resConvertFunc(objKeyW, messageType);
                //
                var sendMsFunc = pSdk.GetMediaSendFunc(messageType);
                //获取AToken
                var atoken = pSdk.GetAToken();
                //发送消息
                var pQYMessageApi = new WeixinMessageSenderQY();
                var pResult = pQYMessageApi.SendQYMS(content, sendMsFunc, messageType, atoken, toUser, toParty, toTag, agentId, safe);
                if (!string.IsNullOrEmpty(pResult.errmsg) && pResult.errmsg != "send job submission success" && pResult.errmsg != "ok")
                {
                    EGExceptionOperator.ThrowX<Exception>(pResult.errmsg, pResult.errcode.ToString());
                }
                //发送完成后更新
                UpdateGsMessageAfterSend(messageid, userId, 1, string.Empty, pResult.errmsg);
            });
        }
        #endregion
        #endregion

        #region 私有方法
        /// <summary>
        /// 群发消息发送完成后，更新数据库D:\EG_Project_Simgle\QYWechat\trunk\Standard\trunk\EG.WeChat.Standard\EG.WeChat.Web\Controllers\QY\QYConfigController.cs
        /// </summary>
        /// <param name="messageid"></param>
        /// <param name="userId"></param>
        /// <param name="pSendType"></param>
        /// <param name="targetUsers"></param>
        /// <returns></returns>
        private bool UpdateGsMessageAfterSend(string messageid, string userId, int pSendType, string targetUsers, string pLog)
        {
            int imessageid = -1;
            if (!int.TryParse(messageid, out imessageid))
                return false;
            if (imessageid < 1)
                return false;

            WXGsMessageDA pDa = new WXGsMessageDA();
            return pDa.UpdateGsMessage(imessageid, userId, DateTime.Now, pSendType, targetUsers, 3, pLog);
        }
        /// <summary>
        /// 检查ODS账户信息
        /// </summary>
        /// <param name="strAccount"></param>
        /// <param name="strTemplateID"></param>
        /// <param name="strURL"></param>
        /// <param name="strTemData"></param>
        /// <param name="pDlg"></param>
        /// <returns></returns>
        private string CheckAccountInfo(string strAccount, string strTemplateID, string strURL, string strTemData, DlgCommonString pDlgGetOpenIDByAccount)
        {
            if (string.IsNullOrEmpty(strAccount))
                EGExceptionOperator.ThrowX<Exception>("缺少ODS账户", EGActionCode.缺少必要参数);

            if (string.IsNullOrEmpty(strTemplateID))
                EGExceptionOperator.ThrowX<Exception>("缺少模板消息ID", EGActionCode.缺少必要参数);

            if (string.IsNullOrEmpty(strURL))
                EGExceptionOperator.ThrowX<Exception>("缺少模板消息回调地址ID", EGActionCode.缺少必要参数);

            //EG.WeChat.Business.JR.BL.AccountBL pBL = new EG.WeChat.Business.JR.BL.AccountBL();
            //string strOpenID = pBL.GetOpenIDByAccountName(strAccount);
            string strOpenID = pDlgGetOpenIDByAccount.Invoke(strAccount);

            if (string.IsNullOrEmpty(strOpenID))
                EGExceptionOperator.ThrowX<Exception>("缺少账户对应微信用户信息", EGActionCode.缺少目标数据);

            return strOpenID;
        }
        /// <summary>
        /// 检查OpenID信息
        /// </summary>
        /// <param name="strOpenID"></param>
        /// <param name="strTemplateID"></param>
        /// <param name="strURL"></param>
        private void CheckOpenIDInfo(string strOpenID, string strTemplateID, string strURL)
        {
            if (string.IsNullOrEmpty(strOpenID))
                EGExceptionOperator.ThrowX<Exception>("缺少微信用户公开编号", EGActionCode.缺少必要参数);

            if (string.IsNullOrEmpty(strTemplateID))
                EGExceptionOperator.ThrowX<Exception>("缺少模板消息ID", EGActionCode.缺少必要参数);

            //if (string.IsNullOrEmpty(strURL))
            //    EGExceptionOperator.ThrowX<Exception>("缺少模板消息回调地址ID", EGActionCode.缺少必要参数);
        }
        /// <summary>
        /// 获取字段日期范围过滤
        /// </summary>
        /// <param name="strFieldName"></param>
        /// <param name="iMonths"></param>
        /// <returns></returns>
        private string GetTargetDateFilter(string strFieldName, int iMonths)
        {
            string strNext = CommonFunction.GetNextMonth(iMonths);
            string strLast = CommonFunction.GetLastMonth(iMonths);

            string strFilter = string.Format(" where {0}>='{1}' and {0}<='{2}'", strFieldName, strLast, strNext);
            return strFilter;
        }
        #endregion

    }


}
