using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using System.Reflection;
/*****************************************************
* 目的：根据EGCommon应用情况，AOP模块引入Castle方法，未来根据业务复杂程度再考虑Spring PostSharp
* 创建人：林子聪
* 创建时间：20141204
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Utility.Tools
{
    /// <summary>
    /// AOP操作
    /// </summary>
    /// <remarks></remarks>
    public class CastleAOPUtil
    {
        /// <summary>
        /// AOP生成拦截器
        /// 拦截条件：业务类执行Function需要为抽象虚方法
        /// </summary>
        /// <typeparam name="TClass"></typeparam>
        /// <param name="pRemoveCache"></param>
        /// <param name="pCacheConfig"></param>
        /// <returns></returns>
        public static TBase NewPxyByClass<TBase>(IInterceptor pInterceptor)
            where TBase : class
        {
            //DataWritingInterceptor pInterceptor = new DataWritingInterceptor(pRemoveCache, pCacheConfig);
            ProxyGenerator Generator = new ProxyGenerator();
            return Generator.CreateClassProxy<TBase>(pInterceptor);
        }
        /// <summary>
        /// AOP生成拦截器
        /// 拦截条件：业务类执行Function需要实现{基类}的抽象虚方法
        /// </summary>
        /// <typeparam name="TClass"></typeparam>
        /// <param name="pRemoveCache"></param>
        /// <param name="pCacheConfig"></param>
        /// <returns>返回基类</returns>
        public static TBase NewPxyByClass<TBase, TDderived>(IInterceptor pInterceptor)
            where TBase : class
            where TDderived : TBase, new()
        {
            //DataWritingInterceptor pInterceptor = new DataWritingInterceptor(pRemoveCache, pCacheConfig);
            ProxyGenerator Generator = new ProxyGenerator();
            return Generator.CreateClassProxyWithTarget<TBase>(new TDderived(), pInterceptor);
        }
        /// <summary>
        /// AOP生成拦截器
        /// 拦截条件：业务类执行Function需要实现{基类}的抽象虚方法
        /// </summary>
        /// <typeparam name="TClass"></typeparam>
        /// <param name="pRemoveCache"></param>
        /// <param name="pCacheConfig"></param>
        /// <returns>返回派生类</returns>
        public static TDderived NewPxyByClass2<TBase, TDderived>(IInterceptor pInterceptor)
            where TBase : class
            where TDderived : TBase, new()
        {
            ProxyGenerator Generator = new ProxyGenerator();
            TBase pBase = Generator.CreateClassProxyWithTarget<TBase>(new TDderived(), pInterceptor);
            return (TDderived)pBase;
        }
        /// <summary>
        /// AOP生成拦截器
        /// 拦截条件：业务类执行Function需要实现{接口}的抽象虚方法
        /// </summary>
        /// <typeparam name="TClass"></typeparam>
        /// <param name="pRemoveCache"></param>
        /// <param name="pCacheConfig"></param>
        /// <returns>返回接口</returns>
        public static TBase NewPxyByInterface<TBase, TDderived>(IInterceptor pInterceptor)
            where TBase : class
            where TDderived : TBase, new()
        {
            //DataWritingInterceptor pInterceptor = new DataWritingInterceptor(pRemoveCache, pCacheConfig);
            ProxyGenerator Generator = new ProxyGenerator();
            return Generator.CreateInterfaceProxyWithTarget<TBase>(new TDderived(), pInterceptor);
        }
        ///// <summary>
        ///// AOP生成拦截器
        ///// 拦截条件：业务类执行Function需要实现{接口}的抽象虚方法
        ///// </summary>
        ///// <typeparam name="TClass"></typeparam>
        ///// <param name="pRemoveCache"></param>
        ///// <param name="pCacheConfig"></param>
        ///// <returns>返回派生类</returns>
        //public static TDderived NewPxyByInterface2<TBase, TDderived>(IInterceptor pInterceptor)
        //    where TBase : class
        //    where TDderived : TBase, new()
        //{
        //    ProxyGenerator Generator = new ProxyGenerator();
        //    TBase pBase = new TDderived();
        //    pBase = Generator.CreateInterfaceProxyWithTarget<TBase>(new TDderived(), pInterceptor);

        //    return (TDderived)pBase;
        //}
    }
}
