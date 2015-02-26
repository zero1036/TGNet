using EG.WeChat.Utility.Tools;
using Senparc.Weixin.MP.AdvancedAPIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

/* 这里保留这个命名空间，因为“扩展方法”对命名空间匹配。 */
namespace Senparc.Weixin.MP.CommonAPIs
{
    /// <summary>
    /// 微信SDK的扩展方法支持和其他支持
    /// </summary>
    public static class WeiXinSDKExtension
    {

        #region 获取当前的accessToken
        /// <summary>
        /// 根据EG.WeChat.Web当前的配置，获取我们账号的有效的AccessToken
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        public static string GetCurrentAccessToken()
        {
            /* 2014-11-12:
             * 1.每次正式发布的时候，使用"Publishes"配置，此时直接调用微信API去获取；
             * 2.调试的时候，使用"Debug"配置，此时跟目标IIS去获取。
             * 
             * 背景：
             * “微信获取AccessToken”的API，获取之后，最后一次调用得到的AccessToken才有效；
             * 而之前的AccessToken，即使未到过期的时间，依然会立即变成无效。
             * 因此，希望统一获取的方式：API的调用，只由IIS去获取，
             * 然后Team的开发，每次都跟IIS的外部接口去获取。
             */

#if DEBUG
            //调试模式下

            //##获取(加密状态的数据)
            object ret =
            DynamicInvokerService.InvokeWebService("http://webchat.cloudapp.net/WebServices/WeChatWS.asmx",
                                                                        "GetCurrentAccessToken",
                                                                        null);
            string rawAccessToken = ret.ToString();
                                                                         

            //##解密并返回数据
            EG.Utility.AppCommon.Security Securityer = new EG.Utility.AppCommon.Security();
            return Securityer.Decrypt(rawAccessToken, @"EGWECHAT");
#else
            //正式发布模式下
            return AccessTokenContainer.GetToken(EG.WeChat.Service.WeiXin.WeiXinConfiguration.appID);
#endif

        }

        #endregion

    }
}