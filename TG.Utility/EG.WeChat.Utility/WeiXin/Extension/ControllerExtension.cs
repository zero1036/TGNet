using Senparc.Weixin.MP.AdvancedAPIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EG.WeChat.Service.WeiXin;
using EG.WeChat.Utility.WeiXin;

namespace System.Web.Mvc
{
    /// <summary>
    /// Controller的扩展方法支持
    /// </summary>
    public static class ControllerExtension
    {

        #region 获取OpenId

        /* 首次获取时，将OpenId存储进来；
         * 如果后续访问，Session未超时/丢失 之前，则直接从Session中获得。
         */

        /// <summary>
        /// 根据CODE，获取OPENID
        /// </summary>
        /// <param name="code">Code凭证</param>
        /// <returns>OpenId</returns>
        public static string GetOpenId(this Controller controller,string code)
        {
            //参数检查
            if (string.IsNullOrEmpty(code))
            {
                return String.Empty;
            }

            const string OPENID = "OpenId";

            //判断Session是否有存储，有则直接返回
            string openIdInSession = controller.Session[OPENID] as string;
            if (String.IsNullOrEmpty(openIdInSession) == false)
            {
                return openIdInSession;
            }

            //目标：只获取OpenID
            OAuthAccessTokenResult result = null;
            try
            {
                result = Senparc.Weixin.MP.AdvancedAPIs.OAuth.GetAccessToken(WeiXinConfiguration.appID,
                                                                             WeiXinConfiguration.appsecret,
                                                                             code);
            }
            catch (Exception ex)
            {
                //如果获取不到，返回NULL，外部进行错误处理。
                return null;
            }

            if (result != null)
            {
                //存储到Session
                controller.Session[OPENID] = result.openid;

                //返回结果
                return result.openid;
            }
            else
            {
                return String.Empty;
            }
        }

        #endregion

    }
}