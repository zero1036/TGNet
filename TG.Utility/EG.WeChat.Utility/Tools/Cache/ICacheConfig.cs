using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
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
    /// 缓存执行项实体接口
    /// </summary>
    public interface ICacheConfig
    {
        /// <summary>
        /// 缓存类型
        /// </summary>
        CacheType CacheType { get; }
        /// <summary>
        /// 缓存类型
        /// </summary>
        string CacheTypeName { get; }
        /// <summary>
        /// 缓存内容
        /// </summary>
        object CacheContent { get; set; }
        /// <summary>
        /// 缓存过期绝对时间
        /// </summary>
        DateTime AbsoluteTime { get; set; }
        /// <summary>
        /// 缓存依赖项
        /// </summary>
        CacheDependency Dependency { get; set; }
        /// <summary>
        /// 缓存过期平滑时间
        /// </summary>
        TimeSpan SlidingTime { get; set; }
        /// <summary>
        /// 级别
        /// </summary>
        /// <remarks></remarks>
        /// AboveNormal	在服务器释放系统内存时，具有该优先级级别的缓存项被删除的可能性比分配了 Normal 优先级的项要小。 
        /// BelowNormal	在服务器释放系统内存时，具有该优先级级别的缓存项比分配了 Normal 优先级的项更有可能被从缓存删除。 
        /// Default	缓存项优先级的默认值为 Normal。 
        /// High	在服务器释放系统内存时，具有该优先级级别的缓存项最不可能被从缓存删除。 
        /// Low	在服务器释放系统内存时，具有该优先级级别的缓存项最有可能被从缓存删除。 
        /// Normal	在服务器释放系统内存时，具有该优先级级别的缓存项很有可能被从缓存删除，其被删除的可能性仅次于具有 Low 或 BelowNormal 优先级的那些项。这是默认选项。
        /// NotRemovable	在服务器释放系统内存时，具有该优先级级别的缓存项将不会被自动从缓存删除。但是，具有该优先级级别的项会根据项的绝对到期时间或可调整到期时间与其他项一起被移除。 
        CacheItemPriority Priority { get; set; }
    }
    /// <summary>
    /// 缓存类型枚举
    /// </summary>
    public enum CacheType
    {
        /// <summary>
        /// 微信用户缓存
        /// </summary>
        /// <remarks>启用</remarks>
        WCUser,
        /// <summary>
        /// 微信用户分组缓存
        /// </summary>
        WCGroup,
        /// <summary>
        /// EG账户缓存
        /// </summary>
        EGAccout,
        /// <summary>
        /// 会员卡内容
        /// </summary>
        CardContent,
        /// <summary>
        /// 定制
        /// </summary>
        CustomService,
        /// <summary>
        /// 微信资源
        /// </summary>
        WXResource,
        /// <summary>
        /// 其他
        /// </summary>
        Others
    }
}
