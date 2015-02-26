using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace EG.WeChat.Utility.WeiXin.ResponseChain
{

    #region ConfigHelper

    /// <summary>
    /// 配置辅助类
    /// </summary>
    public sealed class ConfigHelper
    {
        /// <summary>
        /// 将对象序列化为字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">支持序列化条件的对象</param>
        /// <returns>xml字符串</returns>
        public static string SerializeToString<T>(T data)
        {
            /* 补充tryCatch。不符合序列化条件的类型，会报错；此时返回NULL。 */

            //##创建对象
            XmlSerializer xmlser        = new XmlSerializer(typeof(T));
            StringBuilder result        = new StringBuilder();

            //##XML设置(去除无关的格式)
            XmlWriterSettings setting   = new XmlWriterSettings();
            setting.CheckCharacters     = false;
            setting.Indent              = false;                        //不缩进
            setting.NewLineHandling     = NewLineHandling.None;         //不换行
            setting.OmitXmlDeclaration  = true;                         //无需头部声明

            //##去除默认的命名空间声明
            XmlSerializerNamespaces names = new XmlSerializerNamespaces();
            names.Add("", "");

            using (XmlWriter writter    = XmlWriter.Create(result, setting))
            {
                xmlser.Serialize(writter, data, names);
                return result.ToString();    
            }
        }

        /// <summary>
        /// 将对象序列化为XElement
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">支持序列化条件的对象</param>
        /// <returns>XElement对象</returns>
        public static System.Xml.Linq.XElement SerializeToXE<T>(T data)
        {
            return System.Xml.Linq.XElement.Parse(SerializeToString<T>(data));
        }

        /// <summary>
        /// 从字符串反序列化为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">xml文本</param>
        /// <returns>对象</returns>
        public static T DeserializeFromString<T>(string strData)
        {
            /* 补充tryCatch。不符合序列化条件的类型，会报错；此时返回NULL。 */
            //##创建对象
            XmlSerializer xmlser = new XmlSerializer(typeof(T));

            //##XML设置(去除无关的格式)
            XmlReaderSettings setting   = new XmlReaderSettings();
            setting.CheckCharacters     = false;
            setting.DtdProcessing       = DtdProcessing.Ignore;
            setting.IgnoreComments      = true;
            setting.IgnoreProcessingInstructions = true;
            setting.IgnoreWhitespace    = true;
            setting.ConformanceLevel    = ConformanceLevel.Fragment;

            using (TextReader reader = new StringReader(strData))
            {
                return (T)xmlser.Deserialize(reader);
            }
        }
        

        /// <summary>
        /// 辅助函数——从XML中获取Handler类型的配置
        /// </summary>
        /// <param name="xe"></param>
        /// <returns></returns>
        public static IConfig GetIConfigFromXElement(XElement xe)
        {
            //xHanlder不能为空
            if (xe == null)
                return null;

            //Type不能为空
            if (xe.Attribute("Type") == null)
                return null;

            //获取IConfig类型
            Type targetType = Type.GetType(xe.Attribute("Type").Value);
            if (targetType == null)
                return null;

            //根据Type，创建对应的IConfig
            IConfig config = Activator.CreateInstance(targetType) as IConfig;
            if (config == null)
                return null;

            //解析并返回结果
            config.DeserializeConfigFromXmlNode(xe);
            return config;
        }
    }

    #endregion

    #region IHandlerConfig扩展支持

    /// <summary>
    /// IHandlerConfig扩展支持
    /// </summary>
    internal static class IHandlerConfigExtension
    {
        /// <summary>
        /// 序列化 状态成员
        /// </summary>
        /// <typeparam name="T_IHandler">IHandler类型</typeparam>
        /// <param name="hanlderConfig">IHandlerConfig</param>
        /// <param name="readyConfig">ReadyMessage结果</param>
        /// <param name="successConfig">SuccessMessage结果</param>
        /// <param name="failConfig">FailMessage结果</param>
        public static void SerializeStatusMember<T_IHandler>(this IHandlerConfig<T_IHandler> hanlderConfig,
                                                             out XElement readyConfig,out XElement successConfig,out XElement failConfig)
                                           where T_IHandler : IHandler
        {
            //初始化
            readyConfig     = null;
            successConfig   = null;
            failConfig      = null;

            //根据赋值情况进行处理
            if (hanlderConfig.ReadyMessageConfig != null)
            {
                readyConfig     = new XElement("ReadyMessageConfig");
                hanlderConfig.ReadyMessageConfig.SerializeConfigToXmlNode(readyConfig);
                readyConfig.SetAttributeValue("Type", hanlderConfig.ReadyMessageConfig.GetType().FullName);
            }
            if (hanlderConfig.SuccessResultConfig != null)
            {
                successConfig   = new XElement("SuccessResultConfig");
                hanlderConfig.SuccessResultConfig.SerializeConfigToXmlNode(successConfig);
                successConfig.SetAttributeValue("Type", hanlderConfig.SuccessResultConfig.GetType().FullName);
            }
            if (hanlderConfig.FailResultConfig != null)
            {
                failConfig      = new XElement("FailResultConfig");
                hanlderConfig.FailResultConfig.SerializeConfigToXmlNode(failConfig);
                failConfig.SetAttributeValue("Type", hanlderConfig.FailResultConfig.GetType().FullName);
            }
        }

        /// <summary>
        /// 反序列化 状态成员
        /// </summary>
        /// <typeparam name="T_IHandler">IHandler类型</typeparam>
        /// <param name="hanlderConfig">IHandlerConfig</param>
        /// <param name="currentNode">当前节点元素</param>
        public static void DeserializeStatusMember<T_IHandler>(this IHandlerConfig<T_IHandler> hanlderConfig,
                                                               XElement currentNode)
                                             where T_IHandler : IHandler
        {
            //根据变量情况进行赋值
            if (currentNode.Element("ReadyMessageConfig") != null)
            {
                hanlderConfig.ReadyMessageConfig = ConfigHelper.GetIConfigFromXElement(currentNode.Element("ReadyMessageConfig")) as IResponseMessageConfig;
                hanlderConfig.ReadyMessageConfig.DeserializeConfigFromXmlNode(currentNode.Element("ReadyMessageConfig"));
            }
            if (currentNode.Element("SuccessResultConfig") != null)
            {
                hanlderConfig.SuccessResultConfig = ConfigHelper.GetIConfigFromXElement(currentNode.Element("SuccessResultConfig")) as IResponseMessageConfig;
                hanlderConfig.SuccessResultConfig.DeserializeConfigFromXmlNode(currentNode.Element("SuccessResultConfig"));
            }
            if (currentNode.Element("FailResultConfig") != null)
            {
                hanlderConfig.FailResultConfig = ConfigHelper.GetIConfigFromXElement(currentNode.Element("FailResultConfig")) as IResponseMessageConfig;
                hanlderConfig.FailResultConfig.DeserializeConfigFromXmlNode(currentNode.Element("FailResultConfig"));
            }
        }

        /// <summary>
        /// 创建实例 状态成员
        /// </summary>
        /// <typeparam name="T_IHandler">IHandler类型</typeparam>
        /// <param name="hanlderConfig">IHandlerConfig</param>
        public static void CreateInstanceStatusMember<T_IHandler>(this IHandlerConfig<T_IHandler> hanlderConfig,
                                                                  out IResponseMessage readyMessage,out IResponseMessage successResult,out IResponseMessage failResult)
                                                where T_IHandler : IHandler
        {
            //初始化
            readyMessage    = null;
            successResult   = null;
            failResult      = null;

            //根据赋值情况进行创建
            if (hanlderConfig.ReadyMessageConfig != null)
            {
                dynamic config  = hanlderConfig.ReadyMessageConfig;
                readyMessage    = config.CreateInstanceFromConfig();
            }
            if (hanlderConfig.SuccessResultConfig != null)
            {
                dynamic config  = hanlderConfig.SuccessResultConfig;
                readyMessage    = config.CreateInstanceFromConfig();
            }
            if (hanlderConfig.FailResultConfig != null)
            {
                dynamic config  = hanlderConfig.FailResultConfig;
                readyMessage    = config.CreateInstanceFromConfig();
            }
        }
    }                              

    #endregion

}