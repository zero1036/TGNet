using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using EG.WeChat.Business.Model;
//using EG.WeChat.Business.Definition;
using System.Web;
using System.Web.Caching;
//using EG.WeChat.Business.Model;
//using EG.WeChat.Business.Definition;
using EG.WeChat.Service;
/*****************************************************
* 目的：缓存执行项实体接口
* 创建人：林子聪
* 创建时间：20141107
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Utility.Tools
{
    /// <summary>
    /// 微信用户缓存配置
    /// </summary>
    public class WXUserCacheConfig : ICacheConfig
    {
        #region 私有成员
        //没有绝对过期时间
        private DateTime m_AbsoluteTime = System.Web.Caching.Cache.NoAbsoluteExpiration;
        //暂时没有缓存依赖
        private CacheDependency m_Dependency = null;
        //private TimeSpan m_SlidingTime = Cache.NoSlidingExpiration;
        //90分钟内无访问就过期
        private TimeSpan m_SlidingTime = TimeSpan.FromSeconds(60 * 120);
        //级别为high
        private CacheItemPriority m_Priority = CacheItemPriority.High;
        #endregion

        #region 共有成员
        /// <summary>
        /// 缓存类型
        /// </summary>
        public CacheType CacheType
        {
            get { return CacheType.WCUser; }
        }
        /// <summary>
        /// 缓存类型
        /// </summary>
        public string CacheTypeName
        {
            get
            {
                return CacheType.WCUser.ToString();
            }
        }
        /// <summary>
        /// 缓存内容
        /// </summary>
        public object CacheContent
        {
            get;
            set;
        }
        /// <summary>
        /// 缓存过期绝对时间
        /// </summary>
        public DateTime AbsoluteTime
        {
            get { return m_AbsoluteTime; }
            set { m_AbsoluteTime = value; }
        }
        /// <summary>
        /// 缓存依赖项
        /// </summary>
        public CacheDependency Dependency
        {
            get { return m_Dependency; }
            set { m_Dependency = value; }
        }
        /// <summary>
        /// 缓存过期平滑时间
        /// </summary>
        public TimeSpan SlidingTime
        {
            get { return m_SlidingTime; }
            set { m_SlidingTime = value; }
        }
        /// <summary>
        /// 级别
        /// </summary>
        public CacheItemPriority Priority
        {
            get { return m_Priority; }
            set { m_Priority = value; }
        }
        #endregion
    }
    /// <summary>
    /// 微信分组缓存配置
    /// </summary>
    public class WXGroupCacheConfig : ICacheConfig
    {
        #region 私有成员
        //没有绝对过期时间
        private DateTime m_AbsoluteTime = System.Web.Caching.Cache.NoAbsoluteExpiration;
        //暂时没有缓存依赖
        private CacheDependency m_Dependency = null;
        //private TimeSpan m_SlidingTime = Cache.NoSlidingExpiration;
        //90分钟内无访问就过期——单位分钟
        private TimeSpan m_SlidingTime = TimeSpan.FromSeconds(60 * 120);
        //级别为high
        private CacheItemPriority m_Priority = CacheItemPriority.High;
        #endregion

        #region 共有成员
        /// <summary>
        /// 缓存类型
        /// </summary>
        public CacheType CacheType
        {
            get { return CacheType.WCGroup; }
        }
        /// <summary>
        /// 缓存类型
        /// </summary>
        public string CacheTypeName
        {
            get
            {
                return CacheType.WCGroup.ToString();
            }
        }
        /// <summary>
        /// 缓存内容
        /// </summary>
        public object CacheContent
        {
            get;
            set;
        }
        /// <summary>
        /// 缓存过期绝对时间
        /// </summary>
        public DateTime AbsoluteTime
        {
            get { return m_AbsoluteTime; }
            set { m_AbsoluteTime = value; }
        }
        /// <summary>
        /// 缓存依赖项
        /// </summary>
        public CacheDependency Dependency
        {
            get { return m_Dependency; }
            set { m_Dependency = value; }
        }
        /// <summary>
        /// 缓存过期平滑时间
        /// </summary>
        public TimeSpan SlidingTime
        {
            get { return m_SlidingTime; }
            set { m_SlidingTime = value; }
        }
        /// <summary>
        /// 级别
        /// </summary>
        public CacheItemPriority Priority
        {
            get { return m_Priority; }
            set { m_Priority = value; }
        }
        #endregion
    }
    /// <summary>
    /// 会员卡内容模板缓存配置
    /// </summary>
    public class CardContentCacheConfig : ICacheConfig
    {
        #region 私有成员
        //没有绝对过期时间
        private DateTime m_AbsoluteTime = System.Web.Caching.Cache.NoAbsoluteExpiration;
        //暂时没有缓存依赖
        private CacheDependency m_Dependency = null;
        //private TimeSpan m_SlidingTime = Cache.NoSlidingExpiration;
        //90分钟内无访问就过期——单位分钟
        private TimeSpan m_SlidingTime = TimeSpan.FromSeconds(60 * 120);
        //级别为high
        private CacheItemPriority m_Priority = CacheItemPriority.High;
        #endregion

        #region 共有成员
        /// <summary>
        /// 缓存类型
        /// </summary>
        public CacheType CacheType
        {
            get { return CacheType.CardContent; }
        }
        /// <summary>
        /// 缓存类型
        /// </summary>
        public string CacheTypeName
        {
            get
            {
                return CacheType.CardContent.ToString();
            }
        }
        /// <summary>
        /// 缓存内容
        /// </summary>
        public object CacheContent
        {
            get;
            set;
        }
        /// <summary>
        /// 缓存过期绝对时间
        /// </summary>
        public DateTime AbsoluteTime
        {
            get { return m_AbsoluteTime; }
            set { m_AbsoluteTime = value; }
        }
        /// <summary>
        /// 缓存依赖项
        /// </summary>
        public CacheDependency Dependency
        {
            get { return m_Dependency; }
            set { m_Dependency = value; }
        }
        /// <summary>
        /// 缓存过期平滑时间
        /// </summary>
        public TimeSpan SlidingTime
        {
            get { return m_SlidingTime; }
            set { m_SlidingTime = value; }
        }
        /// <summary>
        /// 级别
        /// </summary>
        public CacheItemPriority Priority
        {
            get { return m_Priority; }
            set { m_Priority = value; }
        }
        #endregion
    }
    /// <summary>
    /// 定制服务实例缓存配置
    /// </summary>
    public class IoCClassCacheConfig : ICacheConfig
    {
        #region 私有成员
        //没有绝对过期时间
        private DateTime m_AbsoluteTime = System.Web.Caching.Cache.NoAbsoluteExpiration;
        //暂时没有缓存依赖
        private CacheDependency m_Dependency = null;
        //private TimeSpan m_SlidingTime = Cache.NoSlidingExpiration;
        //90分钟内无访问就过期——单位分钟
        private TimeSpan m_SlidingTime = TimeSpan.FromSeconds(60 * 120);
        //级别为high
        private CacheItemPriority m_Priority = CacheItemPriority.High;
        //
        private string m_CacheTypeName = string.Empty;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name"></param>
        public IoCClassCacheConfig(string name)
        {
            m_CacheTypeName = string.Format("IOC.{0}", name);
        }
        #endregion

        #region 共有成员
        /// <summary>
        /// 缓存类型
        /// </summary>
        public CacheType CacheType
        {
            get { return CacheType.Others; }
        }
        /// <summary>
        /// 缓存类型
        /// </summary>
        public string CacheTypeName
        {
            get
            {
                return m_CacheTypeName;
            }
        }
        /// <summary>
        /// 缓存内容
        /// </summary>
        public object CacheContent
        {
            get;
            set;
        }
        /// <summary>
        /// 缓存过期绝对时间
        /// </summary>
        public DateTime AbsoluteTime
        {
            get { return m_AbsoluteTime; }
            set { m_AbsoluteTime = value; }
        }
        /// <summary>
        /// 缓存依赖项
        /// </summary>
        public CacheDependency Dependency
        {
            get { return m_Dependency; }
            set { m_Dependency = value; }
        }
        /// <summary>
        /// 缓存过期平滑时间
        /// </summary>
        public TimeSpan SlidingTime
        {
            get { return m_SlidingTime; }
            set { m_SlidingTime = value; }
        }
        /// <summary>
        /// 级别
        /// </summary>
        public CacheItemPriority Priority
        {
            get { return m_Priority; }
            set { m_Priority = value; }
        }
        #endregion
    }
    /// <summary>
    /// 定制服务数据缓存配置
    /// </summary>
    public class CServiceCacheConfig : ICacheConfig
    {
        #region 私有成员
        //没有绝对过期时间
        private DateTime m_AbsoluteTime = System.Web.Caching.Cache.NoAbsoluteExpiration;
        //暂时没有缓存依赖
        private CacheDependency m_Dependency = null;
        //private TimeSpan m_SlidingTime = Cache.NoSlidingExpiration;
        //90分钟内无访问就过期——单位分钟
        private TimeSpan m_SlidingTime = TimeSpan.FromSeconds(60 * 120);
        //级别为high
        private CacheItemPriority m_Priority = CacheItemPriority.High;
        //
        private string m_CacheTypeName = string.Empty;
        #endregion

        #region 共有成员
        /// <summary>
        /// 缓存类型
        /// </summary>
        public CacheType CacheType
        {
            get { return CacheType.CustomService; }
        }
        /// <summary>
        /// 缓存类型
        /// </summary>
        public string CacheTypeName
        {
            get
            {
                return m_CacheTypeName;
            }
        }
        /// <summary>
        /// 缓存内容
        /// </summary>
        public object CacheContent
        {
            get;
            set;
        }
        /// <summary>
        /// 缓存过期绝对时间
        /// </summary>
        public DateTime AbsoluteTime
        {
            get { return m_AbsoluteTime; }
            set { m_AbsoluteTime = value; }
        }
        /// <summary>
        /// 缓存依赖项
        /// </summary>
        public CacheDependency Dependency
        {
            get { return m_Dependency; }
            set { m_Dependency = value; }
        }
        /// <summary>
        /// 缓存过期平滑时间
        /// </summary>
        public TimeSpan SlidingTime
        {
            get { return m_SlidingTime; }
            set { m_SlidingTime = value; }
        }
        /// <summary>
        /// 级别
        /// </summary>
        public CacheItemPriority Priority
        {
            get { return m_Priority; }
            set { m_Priority = value; }
        }
        #endregion
    }
    /// <summary>
    /// 微信资源缓存配置
    /// </summary>
    public class WXResourceCacheConfig : ICacheConfig
    {
        #region 私有成员
        //没有绝对过期时间
        private DateTime m_AbsoluteTime = System.Web.Caching.Cache.NoAbsoluteExpiration;
        //暂时没有缓存依赖
        private CacheDependency m_Dependency = null;
        //private TimeSpan m_SlidingTime = Cache.NoSlidingExpiration;
        //90分钟内无访问就过期——单位分钟
        private TimeSpan m_SlidingTime = TimeSpan.FromSeconds(60 * 120);
        //级别为high
        private CacheItemPriority m_Priority = CacheItemPriority.High;
        //
        private string m_CacheTypeName = string.Empty;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name"></param>
        public WXResourceCacheConfig(string name)
        {
            m_CacheTypeName = string.Format("WXRES.{0}", name);
        }
        #endregion

        #region 共有成员
        /// <summary>
        /// 缓存类型
        /// </summary>
        public CacheType CacheType
        {
            get { return CacheType.WXResource; }
        }
        /// <summary>
        /// 缓存类型
        /// </summary>
        public string CacheTypeName
        {
            get
            {
                return m_CacheTypeName;
            }
        }
        /// <summary>
        /// 缓存内容
        /// </summary>
        public object CacheContent
        {
            get;
            set;
        }
        /// <summary>
        /// 缓存过期绝对时间
        /// </summary>
        public DateTime AbsoluteTime
        {
            get { return m_AbsoluteTime; }
            set { m_AbsoluteTime = value; }
        }
        /// <summary>
        /// 缓存依赖项
        /// </summary>
        public CacheDependency Dependency
        {
            get { return m_Dependency; }
            set { m_Dependency = value; }
        }
        /// <summary>
        /// 缓存过期平滑时间
        /// </summary>
        public TimeSpan SlidingTime
        {
            get { return m_SlidingTime; }
            set { m_SlidingTime = value; }
        }
        /// <summary>
        /// 级别
        /// </summary>
        public CacheItemPriority Priority
        {
            get { return m_Priority; }
            set { m_Priority = value; }
        }
        #endregion
    }
}
