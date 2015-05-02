using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Collections;
using System.Web.Caching;
/*****************************************************
* 目的：CacheHelper
* 创建人：林子聪
* 创建时间：20141107
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Utility.Tools
{
    public class CacheHelper
    {
        /// <summary>
        /// 获取数据缓存
        /// </summary>
        /// <param name="CacheKey">键</param>
        public static object Get(string category, string CacheKey)
        {
            CacheKey = GetCode(category, CacheKey);
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            return objCache[CacheKey];
        }

        /// <summary>
        /// 设置数据缓存
        /// </summary>
        public static void Insert(string category, string CacheKey, object objObject, double pSeconds, CacheItemPriority pCacheItemPriority, CacheItemRemovedCallback pCacheItemRemovedCallback)
        {
            CacheKey = GetCode(category, CacheKey);
            Cache objCache = HttpRuntime.Cache;
            objCache.Insert(CacheKey, objObject, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(pSeconds), pCacheItemPriority, pCacheItemRemovedCallback);
        }

        /// <summary>
        /// 移除指定数据缓存
        /// </summary>
        public static void RemoveAll(string category, string CacheKey)
        {
            CacheKey = GetCode(category, CacheKey);
            System.Web.Caching.Cache _cache = HttpRuntime.Cache;
            _cache.Remove(CacheKey);
        }

        /// <summary>
        /// 移除全部缓存
        /// </summary>
        public static void RemoveAll()
        {
            System.Web.Caching.Cache _cache = HttpRuntime.Cache;
            IDictionaryEnumerator CacheEnum = _cache.GetEnumerator();
            while (CacheEnum.MoveNext())
            {
                _cache.Remove(CacheEnum.Key.ToString());
            }
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="category"></param>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        private static string GetCode(string category, string cacheKey)
        {
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            //md5.
            return string.Format("{0}.{1}", category, cacheKey);
        }
    }
}
