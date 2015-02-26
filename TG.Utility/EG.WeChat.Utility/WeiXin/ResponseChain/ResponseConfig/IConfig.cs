using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EG.WeChat.Utility.WeiXin.ResponseChain
{
    /* 希望支持“配置”的配置类，请集成IConfig接口。
     * 用于实现基础的 序列化和反序列化。
     */

    #region 配置类的基础接口

    /// <summary>
    /// 配置类的基础接口
    /// </summary>
    public interface IConfig
    {
        /// <summary>
        /// 将配置序列化为Xml节点<para />
        /// (附加子节点到CurrentNode，或者直接写入CurrentNode.InnerXml)
        /// </summary>
        void SerializeConfigToXmlNode(System.Xml.Linq.XElement CurrentNode);

        /// <summary>
        /// 从Xml节点反序列化为配置<para />
        /// (从CurrentNode读取子节点，或者直接读取CurrentNode.InnerXml)
        /// </summary>
        void DeserializeConfigFromXmlNode(System.Xml.Linq.XElement CurrentNode);
    }

    #endregion


    #region 配置IHandler

    /// <summary>
    /// 配置IHandler(基础接口)
    /// </summary>
    public interface IHandlerConfig : IConfig
    { }

    /// <summary>
    /// 配置IHandler
    /// </summary>
    /// <typeparam name="T_IHandler"></typeparam>
    internal interface IHandlerConfig<T_IHandler> : IConfig, IHandlerConfig
                                where T_IHandler  : IHandler
    {
        #region 基础成员

        /// <summary>
        /// 根据配置，创建IHander实例
        /// </summary>
        /// <returns></returns>
        T_IHandler CreateInstanceFromConfig();

        #endregion

        #region 状态成员
        /* 出于功能性的考虑，要求IHandlerConfig的继承者们，必须完成一些状态成员的定义。 */

        /// <summary>
        /// ReadyMessage的配置
        /// </summary>
        IResponseMessageConfig ReadyMessageConfig { get; set; }

        /// <summary>
        /// SuccessResult的配置
        /// </summary>
        IResponseMessageConfig SuccessResultConfig { get; set; }

        /// <summary>
        /// FailResult的配置
        /// </summary>
        IResponseMessageConfig FailResultConfig { get; set; }

        #endregion
    }

    #endregion

    #region 配置ResponseMessage

    /// <summary>
    /// 配置ResponseMessage(基础接口)
    /// </summary>
    public interface IResponseMessageConfig : IConfig
    { }

    /// <summary>
    /// 配置ResponseMessage
    /// </summary>
    internal interface IResponseMessageConfig<T_IResponseMessage> : IConfig, IResponseMessageConfig
                                        where T_IResponseMessage  : IResponseMessage
    {
        /// <summary>
        /// 根据配置，创建IResponseMessage实例
        /// </summary>
        /// <returns></returns>
        T_IResponseMessage CreateInstanceFromConfig();
    }
    
    #endregion
}
