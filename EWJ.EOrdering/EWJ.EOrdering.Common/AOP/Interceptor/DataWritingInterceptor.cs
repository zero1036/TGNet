using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;
/*****************************************************
* 目的：数据写入拦截器，主要用于拦截数据写入更新的操作，当数据库有写入时，清空其数据表对应缓存，以便后续功能重新读取
* 创建人：林子聪
* 创建时间：20141204
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EWJ.EOrdering.Common
{
    public class DataWritingInterceptor : IInterceptor
    {
        private Action _ActionBefore;
        private Action _pActionAfter;
        /// <summary>
        /// 构造函数逻辑代码
        /// </summary>
        public DataWritingInterceptor(Action pActionBefore, Action pActionAfter)
        {
            //TODO:构造函数逻辑代码
            _ActionBefore = pActionBefore;
            _pActionAfter = pActionAfter;
        }
        /// <summary>
        /// 拦截
        /// </summary>
        /// <param name="invocation"></param>
        public void Intercept(IInvocation invocation)
        {
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
            _ActionBefore.Invoke();
            Console.WriteLine("方法执行前");
        }
        /// <summary>
        /// 执行后
        /// </summary>
        /// <param name="invocation"></param>
        public void PostProceed(IInvocation invocation)
        {
            _pActionAfter.Invoke();
            Console.WriteLine("方法执行后");
        }
    }
}
