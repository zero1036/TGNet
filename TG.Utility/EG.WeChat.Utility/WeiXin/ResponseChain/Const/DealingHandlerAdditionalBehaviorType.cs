using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EG.WeChat.Utility.WeiXin.ResponseChain
{
    /// <summary>
    /// ResponseNode附加行为类型(扩展功能)
    /// </summary>
    [Flags]
    public enum DealingHandlerAdditionalBehaviorType : ushort
    {
        /// <summary>
        /// 无附加行为(默认)
        /// </summary>
        None = 0,

        /// <summary>
        /// 忽略ReadyMessage的呈现(直接处理HandlerData函数)
        /// </summary>
        IgnoreReadyMessage = 1,
    }
}