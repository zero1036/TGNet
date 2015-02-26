using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace EG.WeChat.Utility.WeiXin.ResponseChain.CustomHandlers
{
    /// <summary>
    /// 所有CustomHandler类型的Config类
    /// </summary>
    /// <typeparam name="T_IHandler"></typeparam>
    public class CustomHandlerConfig : IHandlerConfig<ICustomHandler>
    {        

        #region 自身关注的属性

        /// <summary>
        /// 选用的自定义Handler的类型(Type.FullName)
        /// </summary>
        public string HandlerTypeName;

        #endregion

        #region IConfig接口

        /// <summary>
        /// 从配置信息中创建出具体的ICustomHandler
        /// </summary>
        /// <returns></returns>
        public ICustomHandler CreateInstanceFromConfig()
        {
            //获取并检查类型
            Type targetType = GetICustomHandlerTypeFromCurrentDomain(HandlerTypeName);
            if (targetType == null)
                throw new Exception("找不到匹配的 自定义Handler。");

            //根据类型产品实例
            ICustomHandler resultHandler = Activator.CreateInstance(targetType) as ICustomHandler;

            //处理状态成员(不处理，由CustomHandler自己决定)

            //结果
            return resultHandler;
        }

        /// <summary>
        /// 将配置信息保存到XML
        /// </summary>
        /// <param name="CurrentNode">当前XML节点</param>
        public void SerializeConfigToXmlNode(System.Xml.Linq.XElement CurrentNode)
        {
            //参数检查
            Exception ex;
            if (IsTypeMatch(this.HandlerTypeName, out ex) == false)
            {
                throw ex;
            }

            //保存
            CurrentNode.Add(new XElement("HandlerTypeName", this.HandlerTypeName));
        }

        /// <summary>
        /// 从XLM中还原配置信息
        /// </summary>
        /// <param name="CurrentNode">当前XML节点</param>
        public void DeserializeConfigFromXmlNode(System.Xml.Linq.XElement CurrentNode)
        {
            XElement xeDataType         = CurrentNode.Element("HandlerTypeName");
            string strHandlerTypeName   = xeDataType.Value;

            //参数检查
            Exception ex;
            if (IsTypeMatch(strHandlerTypeName, out ex) == false)
            {
                throw ex;
            }

            //反序列化到配置
            this.HandlerTypeName = strHandlerTypeName;
        }

        /// <summary>
        /// 检查指定的 类名称，是否符合预期要求<para />
        /// (找得到此类型 + Class类型 + 继承自ICustomHandler)
        /// </summary>
        /// <param name="typeName">类的名称</param>
        /// <param name="exception">检查过程，返回的异常</param>
        /// <returns>正确与否</returns>
        private static bool IsTypeMatch(string typeName,out Exception exception)
        {
            //##找得到此类型
            if (String.IsNullOrEmpty(typeName))
            {
                exception = new Exception("未正确指定 自定义Hanlder 的类型名称。");
                return false;
            }

            Type targetType = GetICustomHandlerTypeFromCurrentDomain(typeName);
            if (targetType == null)
            {
                exception = new Exception("找不到指定的类型，或非继承于ICustomHandler的具体类。");
                return false;
            }

            //##通过所有检查
            exception = null;
            return true;
        }

        #endregion

        #region 状态成员

        /// <summary>
        /// ReadyMessage的配置
        /// </summary>
        public IResponseMessageConfig ReadyMessageConfig { get; set; }

        /// <summary>
        /// SuccessResult的配置
        /// </summary>
        public IResponseMessageConfig SuccessResultConfig { get; set; }

        /// <summary>
        /// FailResult的配置
        /// </summary>
        public IResponseMessageConfig FailResultConfig { get; set; }

        #endregion


        #region 辅助方法

        /// <summary>
        /// 从当前应用程序域，根据指定的FullName，查找符合ICustomHandler具体类的类型
        /// </summary>
        /// <param name="typeFullName"></param>
        /// <returns></returns>
        public static Type GetICustomHandlerTypeFromCurrentDomain(string typeFullName)
        {
            //Class类型 + 继承ICustomHandler接口 + FullName匹配
            var ret = AppDomain.CurrentDomain.GetAssemblies().Where(ass => ass.IsDynamic == false)
                                                .SelectMany((System.Reflection.Assembly assembly) =>
                                                {
                                                    try
                                                    {
                                                        return assembly.GetExportedTypes();
                                                    }
                                                    catch (Exception)         //过滤掉 GetTypes失败的程序集
                                                    {
                                                        return new Type[0];
                                                    }
                                                })
                                              .Where(type => typeof(ICustomHandler).IsAssignableFrom(type) && type.IsClass && type.FullName == typeFullName)
                                              .Distinct().FirstOrDefault();
            return ret;
        }

        #endregion

    }
}