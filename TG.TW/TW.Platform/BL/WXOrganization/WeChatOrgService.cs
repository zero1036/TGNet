using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using System.Data;
//using EG.WeChat.Business.Model;
//using EG.WeChat.Business.Interface;
using Senparc.Weixin.MP.AdvancedAPIs;
using TW.Platform.DA;
using Senparc.Weixin.Exceptions;
using Senparc.Weixin.Entities;
using System.Web.Caching;
using EG.WeChat.Service;
using System.Linq.Expressions;
using System.Diagnostics;
using EG.WeChat.Utility.Tools;
using TW.Platform.Model;
using Senparc.Weixin.MP.AdvancedAPIs.User;
/*****************************************************
* 目的：微信组织（用户、分组）服务
* 创建人：林子聪
* 创建时间：20141107
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace TW.Platform.BL
{
    /// <summary>
    /// WeChatOrgService
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    public class WeChatOrgService : IServiceX
    {
        #region 用户处理
        /// <summary>
        /// 同步微信用户数据
        /// </summary>
        public void ReLoadAllWeChatUser()
        {
            List<UserInfoJson> pList = new List<UserInfoJson>();
            this.ExecuteTryCatch(() =>
             {
                 //通过sdk获取accessToken
                 string strAccessToken = Senparc.Weixin.MP.CommonAPIs.WeiXinSDKExtension.GetCurrentAccessToken();
                 //初始化服務
                 WCUserServiceWeChat<UserInfoJson> pServiceWechat = new WCUserServiceWeChat<UserInfoJson>();
                 WCUserServiceLocal<UserInfoJson> pServiceLocal = new WCUserServiceLocal<UserInfoJson>();

                 //首先从微信API加载所有用于信息
                 pList = pServiceWechat.GetWCUserList(strAccessToken);
                 if (pList == null || pList.Count == 0)
                     EGExceptionOperator.ThrowX<Exception>("公众号缺少微信關注用戶", EGActionCode.缺少目标数据);

                 //再更新至数据库——由于可能无更新，所以受影响行数为0
                 pServiceLocal.UpdateWCUserList<WeChatUser, UserInfoJson>(pList);
             });
        }
        /// <summary>
        /// 同步微信用户数据，并且重新读取本地数据
        /// </summary>
        /// <param name="returnResult"></param>
        /// <returns></returns>
        public List<WeChatUser> ReLoadAllWeChatUser(bool returnResult)
        {
            List<UserInfoJson> pList = new List<UserInfoJson>();
            List<WeChatUser> pListResult = new List<WeChatUser>();
            this.ExecuteTryCatch(() =>
            {
                //同步微信用户数据
                ReLoadAllWeChatUser();

                WCUserServiceLocal<WeChatUser> pServiceLocal2 = new WCUserServiceLocal<WeChatUser>();
                //由于有备注名称信息，需要重新从数据库读取，以后升级至缓存机制
                pListResult = pServiceLocal2.GetWXUsersFromDB();
            });
            return pListResult;
        }
        ///// <summary>
        ///// 获取微信用户本地数据，来源自缓存
        ///// </summary>
        ///// <param name="iGroupID"></param>
        ///// <returns></returns>
        //public List<WeChatUser> GetWCUserList_Cache(int iGroupID)
        //{
        //    List<WeChatUser> pList = new List<WeChatUser>();
        //    this.ExecuteTryCatch(() =>
        //   {
        //       //从缓存中获取所有微信用户数据集合
        //       WCUserServiceLocal<WeChatUser> pUserService = new WCUserServiceLocal<WeChatUser>();
        //       pList = pUserService.GetWXUsersCache();

        //       //当GroupID为-1时，即加载全部用户
        //       if (iGroupID != -1)
        //       {
        //           //生成查询参数队列
        //           Queue<QueryEntity> paramQue = new Queue<QueryEntity>();
        //           paramQue.Enqueue(new QueryEntity("groupid", iGroupID, true, false));

        //           //查询目标分组下的用户数据
        //           IQueryable<WeChatUser> pQueryable = CommonFunction.QueryEnumerable<WeChatUser>(pList, paramQue);
        //           pList = pQueryable.ToList<WeChatUser>();
        //       }

        //   });
        //    return pList;
        //}
        /// <summary>
        /// 查询微信用户本地数据，来源自缓存，并适合前端分页
        /// </summary>
        /// <param name="iPageIndex"></param>
        /// <param name="iRowCountInPage"></param>
        /// <param name="filterString"></param>
        /// <returns></returns>
        public List<WeChatUser> QueryWCUserList_Cache(int iPageIndex, int iRowCountInPage, string filterString, ref int iRecordCount)
        {
            List<WeChatUser> pList = new List<WeChatUser>();
            int iRcCount = -1;
            this.ExecuteTryCatch(() =>
            {
                if (!string.IsNullOrEmpty(filterString))
                {
                    //转换前端查询目标
                    List<object> pQueryItems = CommonFunction.FromJsonTo<List<object>>(filterString);
                    //创建查询参数队列
                    Queue<QueryEntity> pQue = WeChatUser.Query2(pQueryItems);
                    //查询微信用户本地数据，来源自缓存
                    pList = QueryWCUserList_Cache(pQue);
                }
                else
                {
                    //查询微信用户本地数据，来源自缓存
                    pList = QueryWCUserList_Cache(null);
                }

                if (pList == null || pList.Count == 0)
                {
                    EGExceptionOperator.ThrowX<Exception>("缺少微信用戶數據", EGActionCode.缺少目标数据);
                }
                //查询目标总记录数
                iRcCount = pList.Count;

                ////受限总共显示记录数，由服务端控制，暂时写死2000条，现行最大记录可以去到1万条json记录，扩展jsonresult可以去到50万条
                //pList = CommonFunction.SubListForTable<WeChatUser>(pList, 2000);
                //筛选目标页记录数
                pList = CommonFunction.SubListForTable<WeChatUser>(pList, iPageIndex, iRowCountInPage);
            });
            iRecordCount = iRcCount;
            return pList;
        }
        /// <summary>
        /// 查询微信用户本地数据，来源自缓存
        /// </summary>
        /// <param name="paramQue"></param>
        /// <returns></returns>
        public List<WeChatUser> QueryWCUserList_Cache(Queue<QueryEntity> paramQue)
        {
            //从缓存中获取所有微信用户数据集合
            WCUserServiceLocal<WeChatUser> pUserService = new WCUserServiceLocal<WeChatUser>();
            List<WeChatUser> pList = pUserService.GetWXUsersCache();
            if (paramQue == null || paramQue.Count == 0)
                return pList;

            //查询目标分组下的用户数据
            IQueryable<WeChatUser> pQueryable = CommonFunction.QueryEnumerable<WeChatUser>(pList, paramQue);
            pList = pQueryable.ToList<WeChatUser>();
            return pList;
        }
        #endregion

        #region 分组处理
        /// <summary>
        /// 重新加载微信分组信息并保存
        /// </summary>
        public List<GroupsJson_Group> ReLoadAllWeChatGroup()
        {
            List<GroupsJson_Group> pList = new List<GroupsJson_Group>();
            this.ExecuteTryCatch(() =>
            {
                //通过sdk获取accessToken
                string strAccessToken = Senparc.Weixin.MP.CommonAPIs.WeiXinSDKExtension.GetCurrentAccessToken();
                WXGroupServiceWeChat pService = new WXGroupServiceWeChat();
                //首先从微信API加载所有分组信息
                pList = pService.GetWCGroupList(strAccessToken);
                if (pList == null || pList.Count == 0)
                    EGExceptionOperator.ThrowX<Exception>("没有微信端用户分组信息", EGActionCode.缺少目标数据);

                WXGroupServiceLocal pServiceLocal = new WXGroupServiceLocal();
                //再更新至数据库并清空缓存
                if (!pServiceLocal.UpdateWCGroupList(pList))
                    throw new Senparc.Weixin.Exceptions.WeixinException("微信限制接口");

                //最后从缓存中读取
                pList = this.GetWCGroupList_Cache();
            });
            return pList;
        }
        /// <summary>
        /// 获取微信分组本地数据，来源自缓存
        /// </summary>
        /// <returns></returns>
        public List<GroupsJson_Group> GetWCGroupList_Cache()
        {
            List<GroupsJson_Group> pList = new List<GroupsJson_Group>();
            this.ExecuteTryCatch(() =>
            {
                WXGroupServiceLocal pServiceLocal = new WXGroupServiceLocal();
                //从缓存读取分组信息集合
                pList = pServiceLocal.GetWXGroupsCache();
            });
            return pList;
        }
        /// <summary>
        /// 创建微信分组并同步
        /// </summary>
        /// <param name="strGroupName"></param>
        /// <returns></returns>
        public List<GroupsJson_Group> CreateWXGroupAndLoad(string strGroupName)
        {
            List<GroupsJson_Group> pList = new List<GroupsJson_Group>();
            this.ExecuteTryCatch(() =>
            {
                //通过sdk获取accessToken
                string strAccessToken = Senparc.Weixin.MP.CommonAPIs.WeiXinSDKExtension.GetCurrentAccessToken();

                WXGroupServiceWeChat pService = new WXGroupServiceWeChat();
                //首先创建分组
                pService.CreateWXGroup(strAccessToken, strGroupName);
                //再从微信API加载所有分组信息
                pList = pService.GetWCGroupList(strAccessToken);
                if (pList == null || pList.Count == 0)
                    EGExceptionOperator.ThrowX<Exception>("没有微信端用户分组信息", EGActionCode.缺少目标数据);

                WXGroupServiceLocal pServiceLocal = new WXGroupServiceLocal();
                //再更新至数据库并清空缓存
                if (!pServiceLocal.UpdateWCGroupList(pList))
                    throw new Senparc.Weixin.Exceptions.WeixinException("微信限制接口");
            });
            return pList;
        }
        /// <summary>
        /// 移动用户分组
        /// </summary>
        /// <param name="strListOpenID"></param>
        /// <param name="strTargetGroupID"></param>
        public void ChangeWXGroup(string strListOpenID, string strTargetGroupID)
        {
            this.ExecuteTryCatch(() =>
           {
               if (string.IsNullOrEmpty(strListOpenID))
                   EGExceptionOperator.ThrowX<Exception>("请選擇移動目標用戶", EGActionCode.缺少必要参数);
               if (string.IsNullOrEmpty(strTargetGroupID))
                   EGExceptionOperator.ThrowX<Exception>("请選擇移動目標分組", EGActionCode.缺少必要参数);

               int iGroupID = Convert.ToInt16(strTargetGroupID);
               //通过sdk获取accessToken
               string strAccessToken = Senparc.Weixin.MP.CommonAPIs.WeiXinSDKExtension.GetCurrentAccessToken();
               //初始化服务
               WXGroupServiceWeChat pGroupServiceWX = new WXGroupServiceWeChat();
               WXGroupServiceLocal pGroupServiceLocal = new WXGroupServiceLocal();
               WCUserServiceLocal<WeChatUser> pUserService = new WCUserServiceLocal<WeChatUser>();

               //将前端生成json串转为openID集合
               List<string> pListOpenID = CommonFunction.FromJsonTo<List<string>>(strListOpenID);
               //遍历更新微信端移动用户分组
               foreach (string openID in pListOpenID)
               {
                   Senparc.Weixin.MP.Entities.WxJsonResult pResult = pGroupServiceWX.MemberUpdate(strAccessToken, openID, iGroupID);
                   if (pResult.errcode != Senparc.Weixin.ReturnCode.请求成功)
                       EGExceptionOperator.ThrowX<Exception>(pResult.errmsg, EGActionCode.未知错误);
               }
               //更新用户新分组信息至数据库，并清空缓存
               pUserService.UpdateWCUserForGroupID(pListOpenID, iGroupID);
               //根据用户新分组信息，同步分组表信息，并清空缓存
               pGroupServiceLocal.ReloadWCGroupFromUser();

           });
        }
        #endregion

    }
}
