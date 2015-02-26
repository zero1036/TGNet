using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Services.Description;

namespace EG.WeChat.Utility.Tools
{
    /* 摘录From：http://blog.csdn.net/ysq5202121/article/details/6942813 */

    /// <summary>
    /// 动态调用WS服务
    /// </summary>
    public sealed class DynamicInvokerService
    {
        /// <summary>
        /// 动态调用web服务(WS的描述，只存在单个对象类)
        /// </summary>
        /// <param name="url">WSDL服务地址</param>
        /// <param name="methodname">方法名</param> 
        /// <param name="args">参数</param> 
        /// <returns>结果</returns> 
        /// <exception cref="System.Exception">无法动态编译类</exception>
        public static object InvokeWebService(string url, string methodname, object[] args)
        {
            return InvokeWebService(url, null, methodname, args);
        }

        /// <summary>
        /// 动态调用web服务(WS的描述，只存在多个对象类)
        /// </summary>
        /// <param name="url">WSDL服务地址</param>
        /// <param name="classname">类名</param>
        /// <param name="methodname">方法名</param> 
        /// <param name="args">参数</param> 
        /// <returns>结果</returns> 
        /// <exception cref="System.Exception">无法动态编译类</exception>
        public static object InvokeWebService(string url, string classname, string methodname, object[] args)
        {
            //##随意固定一个命名空间
            const string @namespace = "DynamicInvokerServiceNS.DynamicWebCalling";

            //##获取类名
            if (String.IsNullOrEmpty(classname))
            {
                classname = GetWsClassName(url);
            }

            //##执行
            try
            {
                //##获取WSDL 
                WebClient wc    = new WebClient();
                Stream stream   = wc.OpenRead(url + "?WSDL");

                //##动态生成Code的准备
                ServiceDescription sd           = ServiceDescription.Read(stream);
                ServiceDescriptionImporter sdi  = new ServiceDescriptionImporter();
                sdi.AddServiceDescription(sd, "", "");
                CodeNamespace cn = new CodeNamespace(@namespace);
                CodeCompileUnit ccu = new CodeCompileUnit();
                ccu.Namespaces.Add(cn);
                sdi.Import(cn, ccu);
                CSharpCodeProvider icc = new CSharpCodeProvider();

                //设定编译参数
                CompilerParameters cplist   = new CompilerParameters();
                cplist.GenerateExecutable   = false;
                cplist.GenerateInMemory     = true;     //在内存中编译
                cplist.ReferencedAssemblies.Add("System.dll");
                cplist.ReferencedAssemblies.Add("System.XML.dll");
                cplist.ReferencedAssemblies.Add("System.Web.Services.dll");
                cplist.ReferencedAssemblies.Add("System.Data.dll");

                //##编译代理类 
                CompilerResults cr = icc.CompileAssemblyFromDom(cplist, ccu);
                if (cr.Errors.HasErrors)
                {//##存在错误的时候
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    foreach (System.CodeDom.Compiler.CompilerError ce in cr.Errors)
                    {
                        sb.Append(ce.ToString());
                        sb.Append(System.Environment.NewLine);
                    }
                    throw new Exception(sb.ToString());
                }

                //##生成代理实例，并调用方法  
                System.Reflection.Assembly assembly     = cr.CompiledAssembly;
                Type t      = assembly.GetType(@namespace + "." + classname, true, true);
                object obj  = Activator.CreateInstance(t);
                System.Reflection.MethodInfo mi = t.GetMethod(methodname);
                return mi.Invoke(obj, args);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message, new Exception(ex.InnerException.StackTrace));
            }
        }

        /// <summary>
        /// 根据WS的URL，获取类的名称
        /// </summary>
        /// <param name="wsUrl"></param>
        /// <returns></returns>
        private static string GetWsClassName(string wsUrl)
        {
            /* 抽取类名。
             * 格式为：
             * http://xxxxxx/WebServices/WeChatWS.asmx
             * 
             * 因此抽取最后一个“/”结尾的数据，WeChatWS.asmx，
             * 然后再抽取“.”之前的字符串。
             */

            string[] parts = wsUrl.Split('/');
            string[] pps = parts[parts.Length - 1].Split('.');
            return pps[0];
        }
    }
}
