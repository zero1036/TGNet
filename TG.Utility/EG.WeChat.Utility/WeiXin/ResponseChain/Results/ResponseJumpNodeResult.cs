using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EG.WeChat.Utility.WeiXin.ResponseChain
{
    /// <summary>
    /// 跳转节点
    /// </summary>
    public class ResponseJumpNode : IResponseMessage
    {
        /// <summary>
        /// 节点ID
        /// </summary>
        public string NodeId = "";

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nodeId">节点ID</param>
        public ResponseJumpNode(string nodeId)
        {
            //参数检查
            if (NodeIdValidator.IsValid(nodeId) == false)
                throw new ArgumentException("节点ID格式不正确", "nodeId");

            this.NodeId = nodeId;
        }
    }
}