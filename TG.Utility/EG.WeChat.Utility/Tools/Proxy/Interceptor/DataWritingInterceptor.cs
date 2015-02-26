using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
//using Castle.Core.Interceptor;
using Castle.DynamicProxy;
using System.Reflection;
using EG.WeChat.Utility.Tools;
/*****************************************************
* 目的：数据写入拦截器，主要用于拦截数据写入更新的操作，当数据库有写入时，清空其数据表对应缓存，以便后续功能重新读取
* 创建人：林子聪
* 创建时间：20141204
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Utility.Tools
{
    public class DataWritingInterceptor : IInterceptor
    {
        private RemoveCache m_RemoveCache;
        private ICacheConfig m_CacheConfig;
        /// <summary>
        /// 构造函数逻辑代码
        /// </summary>
        public DataWritingInterceptor(RemoveCache pRemoveCache, ICacheConfig pCacheConfig)
        {
            //TODO:构造函数逻辑代码
            m_RemoveCache = pRemoveCache;
            m_CacheConfig = pCacheConfig;
        }
        /// <summary>
        /// 拦截
        /// </summary>
        /// <param name="invocation"></param>
        public void Intercept(IInvocation invocation)
        {
            //MethodInfo concreteMethod = invocation.GetConcreteMethod();
            //if (!invocation.MethodInvocationTarget.IsAbstract)
            //{
            //    System.Diagnostics.Debug.WriteLine(concreteMethod.Name);
            //    //System.Diagnostics.Debug.WriteLine(concreteMethod.);
            //    //concreteMethod
            //    bool bb = concreteMethod.IsSpecialName;
            //}

            PreProceed(invocation);
            invocation.Proceed();
            PostProceed(invocation);
        }
        /// <summary>
        /// 执行前
        /// </summary>
        /// <param name="invocation"></param>
        public void PreProceed(IInvocation invocation)
        {
            Console.WriteLine("方法执行前");
        }
        /// <summary>
        /// 执行后
        /// </summary>
        /// <param name="invocation"></param>
        public void PostProceed(IInvocation invocation)
        {
            m_RemoveCache.Invoke(m_CacheConfig);
            Console.WriteLine("方法执行后");
        }
    }
}
