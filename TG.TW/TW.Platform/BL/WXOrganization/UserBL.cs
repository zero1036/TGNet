using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using EG.WeChat.Utility.Tools;
using EG.WeChat.Utility.WeiXin;
using TW.Platform.Sys;
using Senparc.Weixin.MP.CommonAPIs;
using TW.Platform.Model;
using TW.Platform.DA;
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
        #region Public
        /// <summary>
        /// 验证WA端登陆用户信息
        /// </summary>
        /// <param name="code"></param>
        /// <param name="agentid"></param>
        public void VerifyWALoginUser(string code, string agentid)
        {
            //先通过session，查出用户ID
            string pUserID = SysCurUser.GetCurUserID();
            LogSwHelper.Sing.Info("从Session中获取userid：" + pUserID);
            if (string.IsNullOrEmpty(pUserID))
            {
                LogSwHelper.Sing.Info("获取code:" + code);
                LogSwHelper.Sing.Info("获取agentid:" + agentid);
                int iagentid = 0;
                if (int.TryParse(agentid, out iagentid))
                {
                    pUserID = GetQYUserIDByAPI(code, iagentid);
                    if (string.IsNullOrEmpty(pUserID))
                        throw new Exception();

                    UserTM pUser = GetUserByUserID(pUserID);
                    if (pUser == null || pUser.UserId != pUserID)
                        throw new Exception();

                    LogSwHelper.Sing.Info("从API中获取userid：" + pUserID);
                }
                else
                {
                    throw new Exception();
                }
            }
        }
        /// <summary>
        /// 验证BC端登陆用户信息
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        public bool VerifyBCLoginUser(string userId, string passWord)
        {
            //适用于登陆验证，通过用户ID获取当前用户
            UserTM pUser = GetUserByUserID(userId);
            if (pUser != null && pUser.UserId == userId)
            {
                var pwdCode = Emperor.UtilityLib.CyberUtils.Encrypt("Aes", 256, passWord, "TW" + userId);
                if (pUser.Password == pwdCode)
                    return true;
            }
            return false;
        }
        #endregion

        /// <summary>
        /// 适用于登陆验证，通过用户ID获取当前用户
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        private UserTM GetUserByUserID(string UserID)
        {
            UserDA pDa = new UserDA();
            DataTable dt = pDa.GetUserByUserID(UserID);
            if (VerificationHelper.VDTableNull(dt))
            {
                var pUsers = CommonFunction.GetEntitiesFromDataTable<UserTM>(dt);
                var pUser = pUsers[0];
                //设置当前用户
                SysCurUser.SetCurUser<UserTM>(pUser);
                return pUser;
            }
            return null;
        }
        /// <summary>
        /// 通过回调OAuth地址所得code及agentid参数，获取请求的userid
        /// </summary>
        /// <param name="code"></param>
        /// <param name="agentid"></param>
        private string GetQYUserIDByAPI(string code, int agentid)
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
                //SessionHelper.Add(ConstStr.SESSION_CURRENT_USERID, result.UserId);
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
