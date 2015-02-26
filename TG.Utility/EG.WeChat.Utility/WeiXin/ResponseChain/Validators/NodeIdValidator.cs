using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace EG.WeChat.Utility.WeiXin.ResponseChain
{

    /// <summary>
    /// 节点ID的验证器
    /// </summary>
    public sealed class NodeIdValidator
    {
        static Regex regex_NodeId = new Regex(@"^\d+([\.]\d+)*$", 
                                              RegexOptions.Compiled);

        /// <summary>
        /// 验证是否合法
        /// </summary>
        /// <param name="nodeId">节点ID</param>
        public static bool IsValid(string nodeId)
        {
            /* 除了RootNode之外，其他的节点ID，必须满足以下格式：
             * "x"  Or  "xxx.yyy"  Or "xxx.yyy.zzz" 以此类推。
             */

            //##根节点
            if (ConstString.ROOT_NODE_ID.Equals(nodeId, StringComparison.Ordinal))
                return true;

            //##其他节点
            return regex_NodeId.IsMatch(nodeId);
        }
    }
}