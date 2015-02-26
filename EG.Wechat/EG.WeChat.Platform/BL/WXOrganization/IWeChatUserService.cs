using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Senparc.Weixin.MP.AdvancedAPIs;
using EG.WeChat.Platform.DA;
using Senparc.Weixin.Entities;
using System.Web;
using System.Web.Caching;
using EG.WeChat.Service;
using EG.WeChat.Utility.Tools;
/*****************************************************
* 目的：微信用户综合服务接口
* 创建人：林子聪
* 创建时间：20141106
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Platform.BL
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    /// <param name="strAccessToken"></param>
    /// <returns></returns>
    public delegate List<TUser> GetWCUserList<TUser>(string strAccessToken = "");
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public delegate List<T> RecallDataFromLocal<T>();
    ///// <summary>
    ///// 
    ///// </summary>
    ///// <typeparam name="T"></typeparam>
    ///// <returns></returns>
    //public delegate List<T> RecallDataFromLocal1<T, TParm>(List<TParm> pList);
    /// <summary>
    /// 服务基接口，通过Extension实现
    /// </summary>
    public interface IServiceX
    { }
    /// <summary>
    /// 缓存服务基接口，通过Extension实现
    /// </summary>
    public interface IServiceXCache
    { }
    public static class ServiceXExtension
    {
        #region 操作日志
        /// <summary>
        /// 执行方法并捕获错误
        /// </summary>
        /// <param name="pInterface"></param>
        /// <param name="action"></param>
        public static void ExecuteTryCatch(this IServiceX pInterface, Action action)
        {
            try
            {
                action.Invoke();
                ActionResult = null;
            }
            catch (Exception ex)
            {
                ActionResult = EGExceptionOperator.ConvertException(ex);
            }
        }
        /// <summary>
        /// 获取异常输出json实体
        /// </summary>
        /// <param name="pInterface"></param>
        /// <returns></returns>
        public static EGExceptionResult GetActionResult(this IServiceX pInterface)
        {
            return ActionResult;
        }
        /// <summary>
        /// 异常结果
        /// </summary>
        public static EGExceptionResult ActionResult
        {
            get;
            set;
        }
        #endregion

        #region 缓存--引用Asp.net Cache
        /// <summary>
        /// 插入缓存
        /// </summary>
        /// <param name="pCacheConfig"></param>
        /// <param name="onRemove"></param>
        public static void AddCache(this IServiceXCache pInterface, ICacheConfig pCacheConfig, CacheItemRemovedCallback onRemove)
        {
            HttpRuntime.Cache.Add(pCacheConfig.CacheTypeName, pCacheConfig.CacheContent, pCacheConfig.Dependency, pCacheConfig.AbsoluteTime, pCacheConfig.SlidingTime, pCacheConfig.Priority, onRemove);
        }
        /// <summary>
        /// 插入缓存，覆盖旧有缓存
        /// </summary>
        /// <param name="pInterface"></param>
        /// <param name="pCacheConfig"></param>
        /// <param name="onRemove"></param>
        public static void InsertCache(this IServiceXCache pInterface, ICacheConfig pCacheConfig, CacheItemRemovedCallback onRemove)
        {
            HttpRuntime.Cache.Insert(pCacheConfig.CacheTypeName, pCacheConfig.CacheContent, pCacheConfig.Dependency, pCacheConfig.AbsoluteTime, pCacheConfig.SlidingTime, pCacheConfig.Priority, onRemove);
        }
        /// <summary>
        /// 清空缓存
        /// </summary>
        /// <param name="pInterface"></param>
        /// <param name="pCacheConfig"></param>
        public static void RemoveCache(this IServiceXCache pInterface, ICacheConfig pCacheConfig)
        {
            HttpRuntime.Cache.Remove(pCacheConfig.CacheTypeName);
        }
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="pInterface"></param>
        /// <param name="pCacheConfig"></param>
        /// <returns></returns>
        public static object GetCache(this IServiceXCache pInterface, ICacheConfig pCacheConfig)
        {
            return HttpRuntime.Cache.Get(pCacheConfig.CacheTypeName);
        }
        /// <summary>
        /// 获取缓存中的本地数据，如果缓存中缺少数据，则回调RecallDataFromLocal事件的数据
        /// </summary>
        /// <returns></returns>
        public static List<T> GetCacheList<T>(this IServiceXCache pInterface, RecallDataFromLocal<T> pdelegate, ICacheConfig pCacheConfig)
        {
            object objValue = GetCache(pInterface, pCacheConfig);
            if (objValue == null || objValue.GetType() != typeof(List<T>))
                return pdelegate();
            return objValue as List<T>;
        }
        /// <summary>
        /// 获取缓存中的本地数据，如果缓存中缺少数据，则回调RecallDataFromLocal事件的数据
        /// </summary>
        /// <returns></returns>
        public static List<T> GetCacheList<T>(this IServiceXCache pInterface, RecallDataFromLocal<T> pdelegate, ICacheConfig pCacheConfig, CacheItemRemovedCallback RemovedCallback)
        {
            object objValue = GetCache(pInterface, pCacheConfig);
            if (objValue == null || objValue.GetType() != typeof(List<T>))
            {
                var pValue = pdelegate();
                if (pValue != null)
                {
                    if (pValue is IEnumerable<T>)
                    {
                        if ((pValue as IEnumerable<T>).Count() > 0)
                        {
                            //将缓存内容写入执行项中
                            pCacheConfig.CacheContent = pValue;
                            //每次从数据库中获取数据都添加到缓存中
                            pInterface.InsertCache(pCacheConfig, RemovedCallback);
                        }
                    }
                    else
                    {
                        //将缓存内容写入执行项中
                        pCacheConfig.CacheContent = pValue;
                        //每次从数据库中获取数据都添加到缓存中
                        pInterface.InsertCache(pCacheConfig, RemovedCallback);
                    }
                }
                return pValue;
            }
            return objValue as List<T>;
        }
        #endregion

    }


}
