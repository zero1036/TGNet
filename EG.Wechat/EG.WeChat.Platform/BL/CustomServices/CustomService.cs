using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using EG.WeChat.Utility.Tools;
using System.Web.Caching;
using EG.WeChat.Platform.DA;
using System.Data;
/*****************************************************
* 目的：定制服务
* 创建人：林子聪
* 创建时间：20141216
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Platform.BL
{
    /// <summary>
    /// 定制服务
    /// </summary>
    public class CustomService : CustomBaseService
    {
        #region 私有成员
        private CServiceCacheConfig m_CacheConfig = new CServiceCacheConfig();
        /// <summary>
        /// 数据访问DAL
        /// </summary>
        private TServiceConfigDA m_DA;
        /// <summary>
        /// 定制服务列表
        /// </summary>
        private List<TServiceConfigDA> m_pListTSer;
        #endregion

        #region 公有成员
        /// <summary>
        /// 构造函数
        /// </summary>
        public CustomService()
        {
            //实例化DA
            m_DA = CastleAOPUtil.NewPxyByClass<TServiceConfigDA>(new DataWritingInterceptor(this.RemoveCache, this.m_CacheConfig));
            //构造时，获取定制服务集合
            m_pListTSer = GetCServiceCache();
        }
        /// <summary>
        /// 是否具有服务资源
        /// </summary>
        public bool IsReady
        {
            get
            {
                return m_pListTSer != null && m_pListTSer.Count > 0;
            }
        }
        /// <summary>
        /// 启动定制服务
        /// </summary>
        /// <param name="SourceString"></param>
        /// <param name="hsParam"></param>
        /// <returns></returns>
        public string ExcuteCService(string SourceString, Hashtable hsParam)
        {
            if (m_pListTSer == null || m_pListTSer.Count < 1)
                return SourceString;

            //服务返回结果
            object objResult = null;
            string TargetString = string.Empty;

            StringBuilder pBuildSourceString = new StringBuilder();
            pBuildSourceString.Append(SourceString);
            //
            foreach (TServiceConfigDA pSC in m_pListTSer)
            {
                if (!SourceString.Contains(pSC.CKey))
                    continue;

                //CType等于1：本地服务
                if (pSC.CType == 1)
                    objResult = this.RegisterAssemblyWithCache(pSC.CDLL, pSC.CNamespace, pSC.CClass, pSC.CMethod, hsParam);
                //CType等于2：外部Web服务——暂时未实现
                //else
                //    objResult=

                if (objResult == null)
                    continue;

                pBuildSourceString.Replace(pSC.CKey, objResult.ToString());
            }
            return pBuildSourceString.ToString();
        }
        #endregion

        #region 定制服务数据表读写
        /// <summary>
        /// 从数据库中加载定制服务数据
        /// </summary>
        /// <returns></returns>
        private List<TServiceConfigDA> LoadCServiceFromData()
        {
            //从数据库中加载定制服务表
            DataTable dt = m_DA.GetCServices();
            if (dt == null || dt.Rows.Count == 0)
                return null;
            //转换为实体列表
            List<TServiceConfigDA> pList = CommonFunction.GetEntitiesFromDataTable<TServiceConfigDA>(dt);
            return pList;
        }
        #endregion

        #region 定制服务缓存数据读写
        /// <summary>
        /// 从缓存中，读取会员卡内容模板数据
        /// </summary>
        /// <param name="pListMediaID"></param>
        /// <returns></returns>
        public List<TServiceConfigDA> GetCServiceCache()
        {
            List<TServiceConfigDA> pList = this.GetCacheList<TServiceConfigDA>(LoadCServiceFromData, m_CacheConfig, this.CacheRemovedCallback);
            if (pList == null || pList.Count == 0)
            {
                return null;
            }
            //根据条件过滤
            return pList;
        }
        #endregion

        #region 回调事件
        /// <summary>
        /// 当前缓存滑动清空后，自动重新加载并写入缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="vvalue"></param>
        /// <param name="r"></param>
        private void CacheRemovedCallback(String key, Object vvalue, CacheItemRemovedReason r)
        {
            if (r == CacheItemRemovedReason.Expired)
            {
                //GetWXGroupsFromDB();
            }
        }
        #endregion
    }
}
