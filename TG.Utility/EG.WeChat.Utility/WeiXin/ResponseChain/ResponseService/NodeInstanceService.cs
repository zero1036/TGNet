using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EG.WeChat.Utility.WeiXin.ResponseChain
{
    /// <summary>
    /// 节点实例服务类
    /// </summary>
    public class NodeInstanceService
    {

        //------------Members----------

        #region 当前状态

        /// <summary>
        /// 节点状态
        /// </summary>
        internal NodeStatusTypes NodeStatus { get; set; }

        #endregion

        #region 应答链处理器

        /// <summary>
        /// Dealing状态的处理器。
        /// </summary>
        public IHandler DealingHandler;

        /// <summary>
        /// Doned状态的处理器。
        /// </summary>
        public IHandler DonedHandler;
        
        #endregion

        #region 标记

        /* 方便日后 跟踪和调试数据 */

        /// <summary>
        /// 从哪个Config创建而成(只读)
        /// </summary>
        public string RefrenceConfigID{get;private  set;}

        #endregion


        //------------Control----------

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="refrenceConfigID">节点ID</param>
        /// <param name="showReadyMessage">是否呈现Ready消息，同时等待输入；如果为False，则立即进行Dealing状态。</param>
        internal NodeInstanceService(string refrenceConfigID)
        {   
            this.RefrenceConfigID   = refrenceConfigID;
            this.NodeStatus = NodeStatusTypes.Created;
        }

        #endregion

        #region 节点状态的处理
        /// <summary>
        /// 当前节点状态 处理成功  后的处理
        /// </summary>
        internal void MarkSuccess_ThisNodeStatus()
        {
            //##进入下一节点的状态  Created => Dealing => Doned => Finalized
            switch (NodeStatus)
            {
                case NodeStatusTypes.Created:
                    this.NodeStatus = NodeStatusTypes.Dealing;
                    break;
                case NodeStatusTypes.Dealing:
                    this.NodeStatus = NodeStatusTypes.Doned;
                    break;

                case NodeStatusTypes.Doned:
                    this.NodeStatus = NodeStatusTypes.Finalized;
                    break;
            }
        }

        #endregion

    }
}