using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.AdvancedAPIs;
using System.Data;
using TW.Platform.DA;
//using EG.WeChat.Business.Model;
//using EG.WeChat.Business.Interface;
using EG.WeChat.Service;
using System.Web.Caching;
using EG.WeChat.Utility.Tools;
/*****************************************************
* 目的：微信分组服务实现接口
* 创建人：林子聪
* 创建时间：20141126
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace TW.Platform.BL
{
    /// <summary>
    /// 微信端微信分组服务
    /// </summary>
    public class WXGroupServiceWeChat
    {
        /// <summary>
        /// 获取所有微信分组信息的实体列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strAccessToken"></param>
        /// <returns></returns>
        public List<GroupsJson_Group> GetWCGroupList(string strAccessToken)
        {
            GroupsJson pGroup = Senparc.Weixin.MP.AdvancedAPIs.GroupsApi.Get(strAccessToken);
            if (pGroup == null || pGroup.groups == null)
                return null;
            return pGroup.groups;
        }
        /// <summary>
        /// 创建微信分组
        /// </summary>
        /// <param name="strAccessToken"></param>
        /// <param name="strGroupName"></param>
        /// <returns></returns>
        public CreateGroupResult CreateWXGroup(string strAccessToken, string strGroupName)
        {
            CreateGroupResult pResult = Senparc.Weixin.MP.AdvancedAPIs.GroupsApi.Create(strAccessToken, strGroupName);
            return pResult;
        }
        /// <summary>
        /// 移动分组用户
        /// </summary>
        /// <param name="strAccessToken"></param>
        /// <param name="openId"></param>
        /// <param name="toGroupId"></param>
        /// <returns></returns>
        public WxJsonResult MemberUpdate(string strAccessToken, string openId, int toGroupId)
        {
            WxJsonResult pResult = Senparc.Weixin.MP.AdvancedAPIs.GroupsApi.MemberUpdate(strAccessToken, openId, toGroupId);
            return pResult;
        }
    }
    /// <summary>
    /// 本地微信分组服务
    /// </summary>
    public class WXGroupServiceLocal : IServiceXCache
    {
        #region 全局变量及构造函数
        /// <summary>
        /// 微信分组缓存项
        /// </summary>
        public WXGroupCacheConfig m_WCGroupCacheConfig = new WXGroupCacheConfig();
        /// <summary>
        /// 数据访问DAL
        /// </summary>
        private WeChatGroupDA m_DA;
        /// <summary>
        /// 构造函数——开启AOP
        /// </summary>
        public WXGroupServiceLocal()
        {
            m_DA = CastleAOPUtil.NewPxyByClass<WeChatGroupDA>(new DataWritingInterceptor(this.RemoveCache, this.m_WCGroupCacheConfig));
        }
        #endregion

        #region 分组数据读写
        /// <summary>
        /// 获取所有微信分组信息的实体列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strAccessToken"></param>
        /// <returns></returns>
        public List<GroupsJson_Group> GetWXGroupsFromDB()
        {
            //从数据库中加载微信用户表
            DataTable dt = m_DA.GetGroup();
            if (dt == null || dt.Rows.Count == 0)
                throw new Senparc.Weixin.Exceptions.WeixinException("");
            //转换为实体列表
            List<GroupsJson_Group> pList = CommonFunction.GetEntitiesFromDataTable<GroupsJson_Group>(dt);
            //写入缓存
            InsertGroupCache(pList);
            return pList;
        }
        /// <summary>
        /// 更新所有微信分组信息的实体列表
        /// </summary>
        /// <typeparam name="TUser"></typeparam>
        /// <param name="pList"></param>
        /// <returns></returns>
        public bool UpdateWCGroupList(List<GroupsJson_Group> pList)
        {
            //转换为DataTable，利用存储过程，自定义表参数写入（考虑到数据量会大）
            DataTable dt = CommonFunction.GetDataTableFromEntities<GroupsJson_Group>(pList);
            bool bOK = m_DA.SaveGroup(dt);
            return bOK;
        }
        /// <summary>
        /// 同步目标分组ID在分组表与用户表的数量数据
        /// </summary>
        /// <returns></returns>
        public bool ReloadWCGroupFromUser()
        {
            bool bOK = m_DA.ReloadGroup();
            return bOK;
        }
        #endregion

        #region 分组缓存读写
        /// <summary>
        /// 从缓存中获取微信分组数据
        /// </summary>
        /// <returns></returns>
        public List<GroupsJson_Group> GetWXGroupsCache()
        {
            List<GroupsJson_Group> pList = this.GetCacheList<GroupsJson_Group>(this.GetWXGroupsFromDB, m_WCGroupCacheConfig);
            if (pList == null || pList.Count == 0)
            {
                EGExceptionOperator.ThrowX<Exception>("缺少本地微信用户分组数据，请同步微信端用户分组信息", EGActionCode.缺少目标数据);
            }
            return pList;
        }
        /// <summary>
        /// 将目标分组集合插入更新缓存
        /// </summary>
        /// <param name="pList"></param>
        public void InsertGroupCache(List<GroupsJson_Group> pList)
        {
            //将缓存内容写入执行项中
            m_WCGroupCacheConfig.CacheContent = pList;
            //每次从数据库中获取数据都添加到缓存中
            this.InsertCache(m_WCGroupCacheConfig, GroupCacheRemovedCallback);
        }
        #endregion

        #region 回调事件
        /// <summary>
        /// 当前微信分组信息缓存滑动清空后，自动重新加载并写入缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="vvalue"></param>
        /// <param name="r"></param>
        private void GroupCacheRemovedCallback(String key, Object vvalue, CacheItemRemovedReason r)
        {
            if (r == CacheItemRemovedReason.Expired)
            {
                //GetWXGroupsFromDB();
            }
        }
        #endregion
    }
}
