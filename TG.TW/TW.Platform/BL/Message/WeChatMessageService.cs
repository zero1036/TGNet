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
    public class WeChatMessageService : WeChatResourcesService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sdkType"></param>
        public WeChatMessageService(string sdkType)
            : base(sdkType)
        { }

        #region 私有成员
        private string m_strPath = "/App_Data/config/WechatConfig.xml";
        private string m_strTargetType = "TemplateMessage";
        private string m_strRootName = "ConfigService";
        #endregion

        #region 外接方法

        #region 公众号
        #region 消息起稿
        /// <summary>
        /// 创建群发消息，并加入到待审核队列
        /// </summary>
        /// <param name="pUserID">用戶編號</param>
        /// <param name="mediaId">媒體ID</param>
        /// <param name="sendType">發送對象類型</param>
        /// <param name="sendTarget">發送目標對象</param>
        /// <param name="content">內容</param>
        /// <param name="messageType">內容類型</param>
        public void CreateGsMessage(string pUserID, string mediaId, int sendType, string sendTarget, string content, string messageType, int wx_type, int agendid, int safe)
        {
            this.ExecuteTryCatch(() =>
            {
                bool iResult = false;
                WXGsMessageDA pDa = new WXGsMessageDA();
                if (messageType == "text")
                    iResult = pDa.InsertGsMessage(pUserID, DateTime.Now, sendType, sendTarget, messageType, content, 1, wx_type, agendid, safe);
                else
                {
                    iResult = pDa.InsertGsMessage(pUserID, DateTime.Now, sendType, sendTarget, messageType, mediaId, 1, wx_type, agendid, safe);
                }

                if (!iResult)
                    EGExceptionOperator.ThrowX<Exception>("群发消息加入队伍错误", EGActionCode.数据库表保存错误);
            });
        }
        #endregion

        #region 消息发送
        /// <summary>
        /// 获取所有群发消息
        /// </summary>
        public List<T> GetAllGsMessage<T>()
        {
            List<T> pList = new List<T>();
            this.ExecuteTryCatch(() =>
            {
                WXGsMessageDA pDa = new WXGsMessageDA();
                //获取从服务器当天时间算起，的两个月时间内的群发消息，例如：当天为10月，即是获得9、10月的消息记录
                string strDateFilter = GetTargetDateFilter(WXGsMessageDA.FIELD_NAME_MTIME, 1);
                DataTable dt = pDa.GetGsMessage(strDateFilter, m_sdk.WXType);
                if (dt == null || dt.Rows.Count == 0)
                    EGExceptionOperator.ThrowX<Exception>("缺少群发消息数据", EGActionCode.缺少目标数据);

                pList = CommonFunction.GetEntitiesFromDataTable<T>(dt);
            });
            return pList;
        }
        /// <summary>
        /// 获取所有群发消息
        /// </summary>
        public List<T> GetGsMessage<T>(string pFilter)
        {
            List<T> pList = new List<T>();
            this.ExecuteTryCatch(() =>
            {
                WXGsMessageDA pDa = new WXGsMessageDA();
                //转换前端查询目标
                List<object> pQueryItems = CommonFunction.FromJsonTo<List<object>>(pFilter);
                //转换查询字典
                IDictionary<string, object> pDic = WXGsMessageDA.CreateDicItems(pQueryItems);
                //查询获取dATATABLE
                DataTable dt = pDa.GetGsMessage(pDic, m_sdk.WXType);
                if (dt == null || dt.Rows.Count == 0)
                    EGExceptionOperator.ThrowX<Exception>("缺少群发消息数据", EGActionCode.缺少目标数据);

                pList = CommonFunction.GetEntitiesFromDataTable<T>(dt);
            });
            return pList;
        }
        /// <summary>
        /// 发送模板消息——已知OpenID参数
        /// </summary>
        /// <param name="strOpenID">目标微信用户OpenID</param>
        /// <param name="strTemplateID"></param>
        /// <param name="strURL"></param>
        /// <param name="pTemAction"></param>
        public void SendTemplateMessage(string strOpenID, string strTemplateID, string strURL, WCTemplateAction pTemAction)
        {
            this.ExecuteTryCatch(() =>
            {
                //检查输入信息
                CheckOpenIDInfo(strOpenID, strTemplateID, strURL);
                ////发送模板消息
                //EG.WeChat.Service.WeiXin.WeixinMessageSender.SendTemplateMessage<WCTemplateAction>(strOpenID, strTemplateID, System.Drawing.Color.AliceBlue, strURL, pTemAction);
            });
        }
        /// <summary>
        /// 发送模板消息_已知ODS账户，需要获取ODS对应微信用户OpenID
        /// 需要转换消息内容json文本为WCTemplateAction
        /// </summary>
        /// <param name="strAccount">目标ODS账户</param>
        /// <param name="strTemplateID"></param>
        /// <param name="strURL"></param>
        /// <param name="strTemData"></param>
        /// <param name="pDlgGetOpenIDByAccount">通过ODSAccount获取OPenID委托</param>
        public void SendTemplateMessage(string strAccount, string strTemplateID, string strURL, string strTemData, DlgCommonString pDlgGetOpenIDByAccount)
        {
            this.ExecuteTryCatch(() =>
            {
                //检查服务输入信息，并通过ods账户获取微信用户openid
                //由于，模板消息不能大量发送，因此并发性较低，因此，采用直连数据库获取信息
                string strOpenID = CheckAccountInfo(strAccount, strTemplateID, strURL, strTemData, pDlgGetOpenIDByAccount);
                //Json转换为WCTemplateAction实体
                WCTemplateAction pEntity = FromJsonToMatch(strTemData);
                //发送模板消息
                //EG.WeChat.Service.WeiXin.WeixinMessageSender.SendTemplateMessage<WCTemplateAction>(strOpenID, strTemplateID, System.Drawing.Color.AliceBlue, strURL, pEntity);
            });
        }
        /// <summary>
        /// 根据分组进行群发消息
        /// </summary>
        /// <param name="messageid"></param>
        /// <param name="groupId"></param>
        /// <param name="mediaId"></param>
        /// <param name="content"></param>
        /// <param name="messageType"></param>
        public void SendGroupMessageByGroupID(string messageid, string userId, string groupId, string mediaId, string content, string messageType)
        {
            this.ExecuteTryCatch(() =>
           {
               if (string.IsNullOrEmpty(groupId))
                   EGExceptionOperator.ThrowX<Exception>("請選中目標發送微信用戶組", EGActionCode.缺少必要参数);
               if (string.IsNullOrEmpty(mediaId) && string.IsNullOrEmpty(content))
                   EGExceptionOperator.ThrowX<Exception>("請輸入發送內容", EGActionCode.缺少必要参数);
               //
               SendResult pResult = WeixinMessageSender.SendGroupMessageByGroupId(groupId, mediaId, content, messageType);
               if (!string.IsNullOrEmpty(pResult.errmsg) && pResult.errmsg != "send job submission success")
               {
                   EGExceptionOperator.ThrowX<Exception>(pResult.errmsg, pResult.errcode.ToString());
               }

               //发送完成后更新
               UpdateGsMessageAfterSend(messageid, userId, 2, groupId, pResult.errmsg);
           });
        }
        /// <summary>
        /// 根据用户OpenID进行群发消息
        /// </summary>
        /// <param name="messageid"></param>
        /// <param name="groupId"></param>
        /// <param name="mediaId"></param>
        /// <param name="content"></param>
        /// <param name="messageType"></param>
        public void SendGroupMessageByOpenID(string messageid, string userId, string mediaId, string content, string messageType, string sex)
        {
            this.ExecuteTryCatch(() =>
            {
                if (string.IsNullOrEmpty(mediaId) && string.IsNullOrEmpty(content))
                    EGExceptionOperator.ThrowX<Exception>("請輸入發送內容", EGActionCode.缺少必要参数);
                //从缓存中读取微信用户缓存
                WCUserServiceLocal<WeChatUser> pUserService = new WCUserServiceLocal<WeChatUser>();
                List<WeChatUser> pList = pUserService.GetWXUsersCache();
                List<string> pListOpenID = new List<string>();
                //筛选条件——以后扩展为动态条件，暂时只支持性别
                if (sex == "1" || sex == "2")
                {
                    int iSex = Convert.ToInt16(sex);
                    pList = pList.Where(pEn => pEn.sex == iSex).ToList();
                }
                //
                foreach (WeChatUser pUser in pList)
                {
                    if (!pListOpenID.Contains(pUser.openid))
                        pListOpenID.Add(pUser.openid);
                }
                //转换为数组
                string[] pArray = pListOpenID.ToArray();
                //发送群发消息
                SendResult pResult = WeixinMessageSender.SendGroupMessageByOpenId(mediaId, content, messageType, pArray);
                if (!string.IsNullOrEmpty(pResult.errmsg) && pResult.errmsg != "send job submission success")
                {
                    EGExceptionOperator.ThrowX<Exception>(pResult.errmsg, pResult.errcode.ToString());
                }

                //发送完成后更新
                UpdateGsMessageAfterSend(messageid, userId, 1, string.Empty, pResult.errmsg);
            });
        }
        #endregion

        #region 模板从资源表中读写
        ///// <summary>
        ///// 获取模板消息列表，转换为Json串
        ///// </summary>
        ///// <returns></returns>
        //public string GetTemplateMessageListJson<T>()
        //    where T : new()
        //{
        //    string strJsonResult = string.Empty;
        //    this.ExecuteTryCatch(() =>
        //    {
        //        //获取配置，并匹配实体集合
        //        List<T> pList = CommonFunction.MatchConfigList<T>(m_strPath, m_strRootName, m_strTargetType);
        //        //转换为Json
        //        strJsonResult = CommonFunction.ConvertToJson<List<T>>(pList);
        //    });
        //    return strJsonResult;
        //}
        /// <summary>
        /// 获取模板消息列表，转换为实体集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> GetTemplateMessageList<T>()
            where T : new()
        {
            List<T> pList = new List<T>();
            this.ExecuteTryCatch(() =>
            {
                ////获取配置，并匹配实体集合
                //pList = CommonFunction.MatchConfigList<T>(m_strPath, m_strRootName, m_strTargetType);
                pList = base.LoadResources<T>(m_strTargetType);
            });
            return pList;
        }
        /// <summary>
        /// 获取指定模板消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetTemplateMessageSingle<T>(int pIndex)
            where T : new()
        {
            T pEn = new T();
            this.ExecuteTryCatch(() =>
            {
                //获取配置，并匹配实体集合
                //List<T> pList = CommonFunction.MatchConfigList<T>(m_strPath, m_strRootName, m_strTargetType);
                List<T> pList = base.LoadResources<T>(m_strTargetType);
                if (pIndex <= pList.Count - 1)
                {
                    pEn = pList[pIndex];
                }

            });
            return pEn;
        }
        /// <summary>
        /// 更新模板消息配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pT"></param>
        /// <param name="pMediaID"></param>
        public void UpdateTemplateMessageList<T>(T pT, string pMediaID)
        {
            this.ExecuteTryCatch(() =>
            {
                ////首先将界面填写值model转换成json
                //string strValue = CommonFunction.ConvertToJson<T>(pT);
                ////然后再更新至xml配置，如果配置已有对应ID则，更新，没有则插入新记录
                //CommonFunction.UpdateXMLConfig(m_strPath, m_strRootName, m_strTargetType, strValue, pMediaID);
                base.UpdateResources<T>(m_strTargetType, pMediaID, pT);
            });
        }
        #endregion
        #endregion

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
        /// 
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        private WCTemplateAction FromJsonToMatch(string jsonString)
        {
            JObject jo = (JObject)JsonConvert.DeserializeObject(jsonString);
            WCTemplateAction entity = new WCTemplateAction();
            string strValue = string.Empty;
            //使用反射根据对象获取属性集
            //Type typeEntity = entity.GetType();
            Type typeEntity = typeof(WCTemplateAction);
            PropertyInfo[] propertyInfos = typeEntity.GetProperties();
            //为每个属性设置数据行中的相应值
            foreach (PropertyInfo info in propertyInfos)
            {
                if (jo.Property(info.Name) == null || jo.Property(info.Name).ToString() == "")
                    continue;
                strValue = jo[info.Name].ToString();
                //info.SetValue(entity, propertyValue, null);
                info.SetValue(entity, new Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage.TemplateDataItem(strValue), null);
            }
            return entity;
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
