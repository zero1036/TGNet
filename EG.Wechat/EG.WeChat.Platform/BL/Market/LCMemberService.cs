using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EG.WeChat.Platform.DA;
using EG.WeChat.Platform.Model;
using EG.Utility.DBCommon.dao;
/*****************************************************
* 目的：本地会员服务
* 创建人：林子聪
* 创建时间：20141215
* 备注：服务暂未实现
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Platform.BL
{
    public class LCMemberServiceSingle
    {
        public static LCMemberServiceSingle SingleTon = new LCMemberServiceSingle();
        public List<T_Member> ListTMember
        { get; set; }

    }

    public class LCMemberService
    {
        /// <summary>
        /// 实时调用DB，获取微信用户拥有的会员卡
        /// </summary>
        /// <param name="OpenID"></param>
        /// <returns></returns>
        public List<T_Member> GetMembers(string OpenID)
        {
            MemberBL pMemberService = (MemberBL)TransactionAOP.newInstance(typeof(MemberBL));
            return pMemberService.GetMembers(OpenID);
        }
        /// <summary>
        /// 实时调用DB，获取微信用户拥有的会员卡类型
        /// </summary>
        /// <param name="OpenID"></param>
        /// <returns></returns>
        public List<string> GetMemberCardTypes(string OpenID)
        {
            //获取微信用户对应会员身份
            List<T_Member> pTMembers = GetMembers(OpenID);
            LCMemberServiceSingle.SingleTon.ListTMember = pTMembers;
            //获取会员身份拥有的卡类型
            List<string> pListCardType = pTMembers
                .Where(p => p != null)
                .Select<T_Member, string>(me => me.Type.ToString())
                .Distinct().ToList();
            return pListCardType;
        }
        /// <summary>
        /// 内部服务——获取积分
        /// </summary>
        /// <param name="OPENID"></param>
        /// <param name="CARDID"></param>
        /// <returns></returns>
        public string GetIntegralSX(string OPENID, string CARDID)
        {
            List<T_Member> pListTMember;
            if (LCMemberServiceSingle.SingleTon.ListTMember == null || LCMemberServiceSingle.SingleTon.ListTMember.Count == 0)
            {
                pListTMember = GetMembers(OPENID);
            }
            else
            {
                pListTMember = LCMemberServiceSingle.SingleTon.ListTMember;
            }
            return pListTMember.First(p => p.Type == Convert.ToInt32(CARDID)).Integral.ToString();
        }
        /// <summary>
        /// 内部服务——获取折扣
        /// </summary>
        /// <param name="OPENID"></param>
        /// <param name="CARDID"></param>
        /// <returns></returns>
        public string GetDepositSX(string OPENID, string CARDID)
        {
            List<T_Member> pListTMember;
            if (LCMemberServiceSingle.SingleTon.ListTMember == null || LCMemberServiceSingle.SingleTon.ListTMember.Count == 0)
            {
                pListTMember = GetMembers(OPENID);
            }
            else
            {
                pListTMember = LCMemberServiceSingle.SingleTon.ListTMember;
            }
            return pListTMember.First(p => p.Type == Convert.ToInt32(CARDID)).Deposit.ToString();
        }

    }
}
