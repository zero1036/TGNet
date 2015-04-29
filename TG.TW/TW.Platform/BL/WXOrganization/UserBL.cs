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
        #region 私有变量
        private UserDA _da = new UserDA();
        #endregion

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
            LogSwHelper.Sing.Info("WA验证，从Session中获取userid：" + pUserID);
            if (string.IsNullOrEmpty(pUserID))
            {
                LogSwHelper.Sing.Info("获取code:" + code);
                LogSwHelper.Sing.Info("获取agentid:" + agentid);
                int iagentid = 0;
                if (int.TryParse(agentid, out iagentid))
                {
                    var pWeixinid = GetWeixinidByAPI(code, iagentid);
                    if (string.IsNullOrEmpty(pWeixinid))
                        throw new Exception();
                    LogSwHelper.Sing.Info("从API中获取微信号：" + pWeixinid);
                    //适用于登陆验证，通过微信号获取当前用户
                    UserTM pUser = GetUserByID(pWeixinid, _da.GetUserByWeixinid);
                    //WA端验证的是微信号
                    if (pUser == null || pUser.WeixinId != pWeixinid)
                        throw new Exception();

                    LogSwHelper.Sing.Info("从API中获取userid：" + pUser.UserId);
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
            //先通过session，查出用户ID
            string pUserID = SysCurUser.GetCurUserID();
            LogSwHelper.Sing.Info("BC验证，从Session中获取userid：" + pUserID);
            if (!string.IsNullOrEmpty(pUserID) && pUserID == userId)
                return true;
            //适用于登陆验证，通过用户ID获取当前用户
            UserTM pUser = GetUserByID(userId, _da.GetUserByUserID);
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
        private CurUserM GetUserByID(string pID, Func<string, DataTable> pFunc)
        {
            //通过UserID获取用户 或 通过微信号获取用户，根据传入Func而定
            DataTable dt = pFunc.Invoke(pID);
            if (!VerificationHelper.VDTableNull(dt)) return null;
            var pUsers = CommonFunction.GetEntitiesFromDataTable<CurUserM>(dt);
            var pUser = pUsers[0];
            //CurUserM pCurUser = pUser as CurUserM;
            //获取用户所在部门
            var dtDeparts = _da.GetDepartmentBySysUserID(pUser.Tid, pUser.SysUserId);
            if (VerificationHelper.VDTableNull(dtDeparts))
            {
                var pDepartments = CommonFunction.GetEntitiesFromDataTable<DepartmentTM>(dtDeparts);
                pUser.Departments = pDepartments;
                //获取用户及部门所属标签
                var dtTags = _da.GetTagsBySysUserID(pUser.Tid, pUser.SysUserId, pDepartments.Select(p => p.SysDepartmentId).ToArray());

                if (VerificationHelper.VDTableNull(dtTags))
                {
                    var pTags = CommonFunction.GetEntitiesFromDataTable<TagTM>(dtTags);
                    pUser.Tags = pTags;
                }
            }
            //设置当前用户
            SysCurUser.SetCurUser<CurUserM>(pUser);
            return pUser;

        }
        /// <summary>
        /// 通过回调OAuth地址所得code及agentid参数，获取请求的weixinid
        /// </summary>
        /// <param name="code"></param>
        /// <param name="agentid"></param>
        private string GetWeixinidByAPI(string code, int agentid)
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
