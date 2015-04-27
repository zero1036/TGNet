using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using EG.WeChat.Business.Interface;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.User;
using TW.Platform.DA;
using System.Data;
using Senparc.Weixin.MP.CommonAPIs;
using EG.WeChat.Service;
using System.Web.Caching;
using EG.WeChat.Utility.Tools;
/*****************************************************
* 目的：微信用户综合服务实现接口
* 创建人：林子聪
* 创建时间：20141106
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace TW.Platform.BL
{
    /// <summary>
    /// 微信端微信用户服务
    /// </summary>
    public class WCUserServiceWeChat<TUser>
        where TUser : UserInfoJson, new()
    {
        /// <summary>
        /// 获取所有微信关注用户的OpenID
        /// </summary>
        /// <param name="strAccessToken"></param>
        /// <returns></returns>
        public List<String> GetWCUserOpenID(string strAccessToken)
        {
            //汇总结果结合
            List<string> pListAllResultOpenID = new List<string>();
            //调用SDK API
            OpenIdResultJson result = Senparc.Weixin.MP.AdvancedAPIs.User.UserApi.Get(strAccessToken, string.Empty);
            pListAllResultOpenID.AddRange(result.data.openid);

            while (!String.IsNullOrEmpty(result.next_openid))
            {
                result = Senparc.Weixin.MP.AdvancedAPIs.User.UserApi.Get(strAccessToken, result.next_openid);
                if (result.data != null)
                    pListAllResultOpenID.AddRange(result.data.openid);
            }
            return pListAllResultOpenID;
        }
        /// <summary>
        /// 获取所有微信关注用户的实体列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strAccessToken"></param>
        /// <returns></returns>
        public List<TUser> GetWCUserList(string strAccessToken)
        {
            if (typeof(TUser) != typeof(UserInfoJson))
                return null;

            //获取所有微信关注用户的OpenID，通过OpenID再获取用户信息
            List<string> pList = GetWCUserOpenID(strAccessToken);
            if (pList == null || pList.Count == 0)
                return null;

            UserInfoJson pWCUser;
            List<TUser> pListUser = new List<TUser>();
            foreach (string strOpenID in pList)
            {
                pWCUser = new TUser();
                //调用SDK，通过OpenID获取用户信息
                pWCUser = Senparc.Weixin.MP.AdvancedAPIs.User.UserApi.Info(strAccessToken, strOpenID);
                //pWCUser = (TUser)pWCUser;
                pListUser.Add((TUser)pWCUser);
            }
            return pListUser;
        }
    }
    /// <summary>
    /// 本地微信用户服务
    /// </summary>
    public class WCUserServiceLocal<TUser> : IServiceXCache
         where TUser : UserInfoJson, new()
    {
        #region 全局变量及构造函数
        /// <summary>
        /// 数据操作接口
        /// </summary>
        private WeChatUserDA m_WeChatUserDA;
        /// <summary>
        /// 微信用户缓存项
        /// </summary>
        protected WXUserCacheConfig m_WCUserCacheConfig = new WXUserCacheConfig();
        /// <summary>
        /// 构造函数——开启AOP
        /// </summary>
        public WCUserServiceLocal()
        {
            m_WeChatUserDA = CastleAOPUtil.NewPxyByClass<WeChatUserDA>(new DataWritingInterceptor(this.RemoveCache, this.m_WCUserCacheConfig));
        }
        #endregion

        #region 本地数据读写
        /// <summary>
        /// 获取所有微信关注用户的实体列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strAccessToken"></param>
        /// <returns></returns>
        public List<TUser> GetWXUsersFromDB()
        {
            //从数据库中加载微信用户表
            DataTable dt = m_WeChatUserDA.GetUser();
            if (dt == null || dt.Rows.Count == 0)
                throw new Senparc.Weixin.Exceptions.WeixinException("");
            //转换为实体列表
            List<TUser> pList = CommonFunction.GetEntitiesFromDataTable<TUser>(dt);
            //将目标用户集合插入更新缓存
            InsertUserCache(pList);
            return pList;
        }
        /// <summary>
        /// 更新所有微信关注用户的实体列表
        /// </summary>
        /// <typeparam name="TUser"></typeparam>
        /// <param name="pList"></param>
        /// <returns></returns>
        public bool UpdateWCUserList(List<TUser> pList)
        {
            //转换为DataTable，利用存储过程，自定义表参数写入（考虑到数据量会大）
            DataTable dt = CommonFunction.GetDataTableFromEntities<TUser>(pList);
            return m_WeChatUserDA.SaveUser(dt);
        }
        /// <summary>
        /// 更新所有微信关注用户的实体列表
        /// </summary>
        /// <typeparam name="TUser"></typeparam>
        /// <param name="pList"></param>
        /// <returns></returns>
        public bool UpdateWCUserList<TConstruct, TUser>(List<TUser> pList)
        {
            //转换为DataTable，利用存储过程，自定义表参数写入（考虑到数据量会大）
            DataTable dt = CommonFunction.GetDataTableFromEntities<TConstruct, TUser>(pList);
            return m_WeChatUserDA.SaveUser(dt);
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="pListOpenID"></param>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public bool UpdateWCUserForGroupID(List<string> pListOpenID, int GroupID)
        {
            bool iEff = true;
            //m_WeChatUserDA = new WeChatUserDA();
            foreach (string OpenID in pListOpenID)
            {
                if (m_WeChatUserDA.UpdateGroupIDByOpenID(GroupID, OpenID) != 1)
                    iEff = false;
            }
            return iEff;
        }
        #endregion

        #region 用户缓存读写
        /// <summary>
        /// 从缓存中获取微信用户数据
        /// </summary>
        /// <returns></returns>
        public List<TUser> GetWXUsersCache()
        {
            List<TUser> pList = this.GetCacheList<TUser>(this.GetWXUsersFromDB, m_WCUserCacheConfig);
            if (pList == null || pList.Count == 0)
            {
                EGExceptionOperator.ThrowX<Exception>("缺少本地微信用户数据，请同步微信端用户分组信息", EGActionCode.缺少目标数据);
            }
            return pList;
        }
        /// <summary>
        /// 将目标用户集合插入更新缓存
        /// </summary>
        /// <param name="pList"></param>
        public void InsertUserCache(List<TUser> pList)
        {
            //将缓存内容写入执行项中
            m_WCUserCacheConfig.CacheContent = pList;
            //每次从数据库中获取数据都添加到缓存中
            this.InsertCache(m_WCUserCacheConfig, UserCacheRemovedCallback);
        }
        #endregion

        #region 回调事件
        /// <summary>
        /// 当前微信用户缓存滑动清空后，自动重新加载并写入缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="vvalue"></param>
        /// <param name="r"></param>
        private void UserCacheRemovedCallback(String key, Object vvalue, CacheItemRemovedReason r)
        {
            if (r == CacheItemRemovedReason.Expired)
            {
                //GetWXUsersFromDB();
            }
        }
        #endregion

    }

}
