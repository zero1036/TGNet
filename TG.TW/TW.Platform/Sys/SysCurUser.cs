using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EG.WeChat.Utility.Tools;
using TW.Platform.Model;
/*****************************************************
* 目的：当前用户For session
* 创建人：林子聪
* 创建时间：20150427
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace TW.Platform.Sys
{
    public class SysCurUser
    {
        /// <summary>
        /// 获取当前用户ID
        /// </summary>
        /// <returns></returns>
        public static string GetCurUserID()
        {
            var pUserID = SessionHelper.Get(ConstStr.SESSION_CURRENT_USERID);
            if (pUserID != null)
                return pUserID.ToString();

            return string.Empty;
        }
        /// <summary>
        /// 设置当前用户ID
        /// </summary>
        /// <param name="pUserID"></param>
        public static void SetCurUserID(string pUserID)
        {
            var pu = GetCurUserID();
            if (string.IsNullOrEmpty(pu))
            {
                SessionHelper.Add(ConstStr.SESSION_CURRENT_USERID, pUserID);
            }
            else if (pu != pUserID)
            {
                SessionHelper.Clear(ConstStr.SESSION_CURRENT_USERID);
                SessionHelper.Add(ConstStr.SESSION_CURRENT_USERID, pUserID);
            }
        }
        /// <summary>
        /// 获取当前用户
        /// </summary>
        /// <returns></returns>
        public static CurUserM GetCurUser()
        {
            var pUser = SessionHelper.Get(ConstStr.SESSION_CURRENT_USER);
            if (pUser != null && pUser is CurUserM)
                return pUser as CurUserM;
            return null;
        }
        /// <summary>
        /// 设置当前用户
        /// </summary>
        /// <returns></returns>
        public static void SetCurUser(CurUserM pT)
        {
            var pu = GetCurUser();
            if (pu == null)
            {
                SessionHelper.Add(ConstStr.SESSION_CURRENT_USER, pT);
                //设置当前用户ID
                SetCurUserID(pT.UserId);
            }
        }
    }

}
