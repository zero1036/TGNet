using Senparc.Weixin.MP.CommonAPIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EG.WeChat.Service.WeiXin;
using System.Web.Services;
using EG.WeChat.Service.WeiXin;
using EG.WeChat.Utility.WeiXin;

namespace EG.WeChat.Web.Service.WebServices
{
    /// <summary>
    /// WebService for WeChatWS
    /// </summary>
    [WebService(Namespace = "http://webchat.cloudapp.net/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class WeChatWS : System.Web.Services.WebService
    {
        /// <summary>
        /// 获取当前可用的AccessToken
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string GetCurrentAccessToken()
        {
            //获取
            string accessToken = AccessTokenContainer.GetToken(WeiXinConfiguration.appID);
            
            //安全性的处理
            EG.Utility.AppCommon.Security Securityer = new EG.Utility.AppCommon.Security();
            return Securityer.Encrypt(accessToken, @"EGWECHAT");
        }
    }
}
