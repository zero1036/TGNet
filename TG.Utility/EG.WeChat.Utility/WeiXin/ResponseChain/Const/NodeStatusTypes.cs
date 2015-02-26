using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EG.WeChat.Utility.WeiXin.ResponseChain
{
    /// <summary>
    /// 应答节点状态枚举
    /// </summary>
    internal enum NodeStatusTypes
    {
        /// <summary>
        /// 刚创建
        /// </summary>
        Created,

        /// <summary>
        /// 处理中状态
        /// </summary>
        Dealing,

        /// <summary>
        /// 完成状态
        /// </summary>
        Doned,

        /// <summary>
        /// 终结状态(标记，不允许此节点继续使用)
        /// </summary>
        Finalized,

    }
}