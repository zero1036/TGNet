using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EG.WeChat.Utility.WeiXin.ResponseChain
{
    /// <summary>
    /// 自定义处理器的接口
    /// </summary>
    public interface ICustomHandler : IHandler
    {
        /* 目前没想到其他约束。但是 自定义处理器 的限制，要严格于  内部处理器，规范一些意外的结果。 */
    }


    public interface ICustomHandlerConfigable { }

    /// <summary>
    /// 标记该自定义处理器，是可以配置的
    /// </summary>
    public interface ICustomHandlerConfigable<TConfigClass> : ICustomHandlerConfigable
                                        where TConfigClass : IConfigClassOfCustomHandler
    {
        /// <summary>
        /// 参数配置的数据成员
        /// </summary>
        TConfigClass ConfigData { get; set; }
    }


    /// <summary>
    /// 自定义处理器的参数配置类的接口<para />
    /// ( 配置单位为单个节点 )
    /// </summary>
    public interface IConfigClassOfCustomHandler : IConfig
    {
        /* 目前没想到其他约束。但是 自定义处理器的参数配置 的限制，要尽量严格，规范一些意外的结果、以及统一操作。 */
    }
}