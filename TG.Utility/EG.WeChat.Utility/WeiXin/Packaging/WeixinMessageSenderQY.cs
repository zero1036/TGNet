using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*****************************************************
* 目的：企业号消息发送
* 创建人：林子聪
* 创建时间：20150326
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Service.WeiXin
{
    /// <summary>
    /// 
    /// </summary>
    public class WeixinMessageSenderQY
    {
        #region 企业号发送主动消息
        /// <summary>
        /// 企业号发送主动消息
        /// </summary>
        /// <param name="ctype"></param>
        /// <param name="accessToken"></param>
        /// <param name="toUser"></param>
        /// <param name="toParty"></param>
        /// <param name="toTag"></param>
        /// <param name="agentId"></param>
        /// <param name="lcId"></param>
        /// <param name="safe"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public Senparc.Weixin.QY.AdvancedAPIs.Mass.MassResult SendQYMS(object content, Func<string, string, string, string, string, string, int, int, Senparc.Weixin.QY.AdvancedAPIs.Mass.MassResult> sendFunc, string messageType, string accessToken, string toUser, string toParty, string toTag, string agentId, int safe = 0, int timeOut = 10000)
        {
            //再转换为可发送json格式及json串
            var data = GetQYMessageJsonTemp(messageType, toUser, toParty, toTag, agentId, content, safe);
            //发送消息
            return sendFunc(accessToken, toUser, toParty, toTag, agentId, data, safe, timeOut);
        }
        /// <summary>
        /// 获取企业消息转换
        /// </summary>
        /// <param name="lcId"></param>
        /// <param name="ctype"></param>
        /// <returns></returns>
        private string GetQYMessageJsonTemp(string messageType, string toUser, string toParty, string toTag, string agentId, object content, int safe)
        {
            switch (messageType)
            {
                case "video":
                    var data3 = new
                    {
                        touser = toUser,
                        toparty = toParty,
                        totag = toTag,
                        msgtype = "video",
                        agentid = agentId,
                        video = new
                        {
                            media_id = content.ToString(),
                            title = "",
                            description = ""
                        },
                        safe = safe
                    };
                    return EG.WeChat.Utility.Tools.CommonFunction.Json_Serialize(data3);
                case "news":
                    var data4 = new
                    {
                        touser = toUser,
                        toparty = toParty,
                        totag = toTag,
                        msgtype = "news",
                        agentid = agentId,
                        news = new
                        {
                            articles = content
                        },
                        safe = safe
                    };
                    return EG.WeChat.Utility.Tools.CommonFunction.Json_Serialize(data4);
                case "mpnews":
                    var data5 = new
                    {
                        touser = toUser,
                        toparty = toParty,
                        totag = toTag,
                        msgtype = "mpnews",
                        agentid = agentId,
                        mpnews = new
                        {
                            articles = content
                        },
                        safe = safe
                    };
                    return EG.WeChat.Utility.Tools.CommonFunction.Json_Serialize(data5);
            }
            return content.ToString();
        }
        #endregion
    }
}
