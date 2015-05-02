using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;
using EG.WeChat.Utility.Tools;

/*****************************************************
* 目的：CustomCacheHelper
* 创建人：林子聪
* 创建时间：20141107
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace TW.Platform.Sys
{
    public class CustomCacheHelper
    {
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pKey"></param>
        /// <param name="pDataCall"></param>
        /// <param name="pSlidingSecond"></param>
        /// <returns></returns>
        public T Get<T>(string pKey, Func<T> pDataCall, double pSlidingSecond = 600)
            where T : class
        {
            var pUser = SysCurUser.GetCurUser();
            if (pUser == null || pUser.Tid < 0)
                return default(T);
            var obj = CacheHelper.Get(pUser.Tid.ToString(), pKey);
            if (obj == null || obj.GetType() != typeof(T))
            {
                //回调数据
                var pValue = pDataCall();
                if (pValue != null)
                {
                    if (pValue is IList)
                    {
                        if ((pValue as IList).Count > 0)
                        {
                            CacheHelper.Insert(pUser.Tid.ToString(), pKey, pValue, pSlidingSecond, CacheItemPriority.High, null);
                        }
                    }
                    else
                    {
                        CacheHelper.Insert(pUser.Tid.ToString(), pKey, pValue, pSlidingSecond, CacheItemPriority.High, null);
                    }
                }
                return pValue;
            }
            else
            {
                return obj as T;
            }
        }


    }
}
