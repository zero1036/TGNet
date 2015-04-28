using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EG.WeChat.Utility.Tools;
using EG.WeChat.Utility.WeiXin;
using TW.Platform.Sys;
using Senparc.Weixin.MP.CommonAPIs;
/*****************************************************
* 目的：系统/微信用户逻辑处理
* 创建人：林子聪
* 创建时间：20140427
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace TW.Platform.BL
{
    public class UserBL
    {
        /// <summary>
        /// 通过回调OAuth地址所得code及agentid参数，获取请求的userid
        /// </summary>
        /// <returns></returns>
        public string GetQYUserIDBySession()
        {
            var pUserID = SessionHelper.Get(ConstStr.SESSION_CURRENT_USERID);
            if (pUserID != null)
                return pUserID.ToString();

            return string.Empty;
        }
        /// <summary>
        /// 通过回调OAuth地址所得code及agentid参数，获取请求的userid
        /// </summary>
        /// <param name="code"></param>
        /// <param name="agentid"></param>
        public string GetQYUserIDByAPI(string code, int agentid)
        {
            //参数检查
            if (string.IsNullOrEmpty(code) || agentid < 1)
                return string.Empty;

            //目标：只获取OpenID
            Senparc.Weixin.QY.AdvancedAPIs.OAuth2.GetUserIdResult result;
            try
            {
                result = Senparc.Weixin.QY.AdvancedAPIs.OAuth2.OAuth2Api.GetUserId(WeiXinSDKExtension.GetCurrentAccessTokenQY(), code, agentid);
            }
            catch (Exception ex)
            {
                //如果获取不到，返回NULL，外部进行错误处理。
                return string.Empty;
            }

            if (result != null)
            {
                SessionHelper.Add(ConstStr.SESSION_CURRENT_USERID, result.UserId);
                //返回结果
                return result.UserId;
            }
            else
            {
                return string.Empty;
            }

            //SessionHelper.Add(ConstStr.SESSION_CURRENT_USERID, code + agentid);
            //return code + agentid;
        }
    }
}
