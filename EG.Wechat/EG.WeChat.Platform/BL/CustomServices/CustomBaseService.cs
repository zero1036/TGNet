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
* 目的：定制服务基础服务
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
    /// 定制服务基础服务
    /// </summary>
    public class CustomBaseService : IServiceXCache
    {
        private IoCClassCacheConfig m_CacheConfig;
        /// <summary>
        /// 反射目标对象，并执行目标方法，不带缓存
        /// </summary>
        /// <param name="DLLName"></param>
        /// <param name="NameSpaceName"></param>
        /// <param name="ClassName"></param>
        /// <param name="MethodName"></param>
        /// <param name="hsParam"></param>
        protected object RegisterAssembly(string DLLName, string NameSpaceName, string ClassName, string MethodName, Hashtable hsParam)
        {
            //返回结果
            object objResult = null;
            //string SendSet = "EG.WeChat.Platform.BL.SerCla2";
            string FullClassName = string.Format("{0}.{1}", NameSpaceName, ClassName);
            string m_strPath = string.Format("/bin/{0}", DLLName);
            string dllPath = System.Web.HttpContext.Current.Server.MapPath(m_strPath);
            //反射目标对象
            object target = EG.WeChat.Utility.Tools.AssemblyHelper.LoadObjectFromAssembly(dllPath, FullClassName);
            //执行对象加载函数
            Type typeEntity = target.GetType();
            //获取函数
            MethodInfo pMethodInfo = typeEntity.GetMethod(MethodName);
            objResult = AssemblyHelper.InvokeMethod(target, pMethodInfo, hsParam);
            return objResult;
        }
        /// <summary>
        /// 反射目标对象，并执行目标方法，带缓存
        /// </summary>
        /// <param name="DLLName"></param>
        /// <param name="NameSpaceName"></param>
        /// <param name="ClassName"></param>
        /// <param name="MethodName"></param>
        /// <param name="hsParam"></param>
        protected object RegisterAssemblyWithCache(string DLLName, string NameSpaceName, string ClassName, string MethodName, Hashtable hsParam)
        {
            //返回结果
            object objResult = null;
            //string SendSet = "EG.WeChat.Platform.BL.SerCla2";
            string CacheName = string.Format("{0}.{1}.{2}", NameSpaceName, ClassName, MethodName);
            string FullClassName = string.Format("{0}.{1}", NameSpaceName, ClassName);
            m_CacheConfig = new IoCClassCacheConfig(CacheName);
            var pEn = this.GetCache(m_CacheConfig);
            if (pEn == null)
            {
                string m_strPath = string.Format("/bin/{0}", DLLName);
                string dllPath = System.Web.HttpContext.Current.Server.MapPath(m_strPath);
                //加载对象
                //string dllPath = System.Windows.Forms.Application.StartupPath + "\\SouthGIS.TrcmsAlteration.dll";
                //string dllPath = Server.MapPath + "\\bin\\" + DLLName;
                //string className = "SouthGIS.TrcmsAlteration.UI." + UCName;

                //反射目标对象
                object target = EG.WeChat.Utility.Tools.AssemblyHelper.LoadObjectFromAssembly(dllPath, FullClassName);
                //执行对象加载函数
                Type typeEntity = target.GetType();
                //获取函数
                MethodInfo pMethodInfo = typeEntity.GetMethod(MethodName);
                objResult = AssemblyHelper.InvokeMethod(target, pMethodInfo, hsParam);
                //写入缓存
                m_CacheConfig.CacheContent = new AssemblyEn(target, pMethodInfo);
                this.InsertCache(m_CacheConfig, CacheRemovedCallback);
            }
            else
            {
                AssemblyEn ppp = pEn as AssemblyEn;
                objResult = AssemblyHelper.InvokeMethod(ppp.target, ppp.methodInfo, hsParam);
            }
            return objResult;
        }
        /// <summary>
        /// 回调事件
        /// </summary>
        /// <param name="key"></param>
        /// <param name="vvalue"></param>
        /// <param name="r"></param>
        private void CacheRemovedCallback(String key, Object vvalue, CacheItemRemovedReason r)
        { }
    }
    /// <summary>
    /// 测试服务
    /// </summary>
    public class TestCustomService
    {
        public string TestFunction(string OPENID)
        {
            return string.Format("{0}.测试", OPENID);
        }
    }
}
