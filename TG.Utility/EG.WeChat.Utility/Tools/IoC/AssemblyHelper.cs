using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;
/*****************************************************
* 目的：依赖注入——反射辅助
* 创建人：林子聪
* 创建时间：20130606
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Utility.Tools
{
    public class AssemblyHelper
    {
        //创建指定程序集中指定类名的程序类
        public static object LoadObjectFromAssembly(string dllPath, string className)
        {
            object obj = null;

            try
            {
                if (System.IO.File.Exists(dllPath))
                {
                    Assembly pObjAssembly = Assembly.LoadFrom(dllPath);
                    if (pObjAssembly != null)
                    {
                        Type objType = pObjAssembly.GetType(className, false);
                        if (objType != null)
                        {
                            obj = Activator.CreateInstance(objType);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("dllPath:" + dllPath + ",  className:" + className);
            }
            if (obj == null)
            {
                throw new Exception("dllPath:" + dllPath + ",  className:" + className);
            }

            return obj;
        }

        public static Type GetTypeFromAssembly(string dllPath, string className)
        {
            Type objType = null;

            try
            {
                if (System.IO.File.Exists(dllPath))
                {
                    Assembly pObjAssembly = Assembly.LoadFrom(dllPath);
                    if (pObjAssembly != null)
                    {
                        objType = pObjAssembly.GetType(className, false);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("dllPath:" + dllPath + ",  className:" + className);
            }

            return objType;
        }

        public static object InvokeMethod(object curObject, MethodInfo mi, Hashtable args)
        {
            object result = null;

            if (mi != null)
            {
                if (args != null)
                {
                    ParameterInfo[] parmInfos = mi.GetParameters();
                    object[] parms = new object[parmInfos.Length];
                    for (int i = 0; i < parmInfos.Length; i++)
                    {
                        if (args.Contains(parmInfos[i].Name))
                        {
                            parms[i] = args[parmInfos[i].Name];
                        }
                    }
                    result = mi.Invoke(curObject, parms);
                }
                else
                {
                    result = mi.Invoke(curObject, null);
                }
            }

            return result;
        }

        public static object InvokeMethod(object curObject, string method, Hashtable args)
        {
            object result = null;

            MethodInfo mi = curObject.GetType().GetMethod(method, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance);
            if (mi != null)
            {
                if (args != null)
                {
                    ParameterInfo[] parmInfos = mi.GetParameters();
                    object[] parms = new object[parmInfos.Length];
                    for (int i = 0; i < parmInfos.Length; i++)
                    {
                        if (args.Contains(parmInfos[i].Name))
                        {
                            parms[i] = args[parmInfos[i].Name];
                        }
                    }
                    result = mi.Invoke(curObject, parms);
                }
                else
                {
                    result = mi.Invoke(curObject, null);
                }
            }

            return result;
        }

        public static object InvokeProperty(object curObject, PropertyInfo propertyInfo)
        {
            object result = null;

            if (propertyInfo != null)
            {
                result = propertyInfo.GetValue(curObject, null);
            }

            return result;
        }

        public static object InvokeProperty(object curObject, string propertyName)
        {
            object result = null;

            PropertyInfo propertyInfo = curObject.GetType().GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance);
            if (propertyInfo != null)
            {
                result = propertyInfo.GetValue(curObject, null);
            }

            return result;
        }
    }
}
