using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace EG.WeChat.Utility.WeiXin.ResponseChain
{
    /// <summary>
    /// 节点的配置
    /// </summary>
    public class ResponseNodeConfig 
    {
        //---------Members---------

        #region 数据成员

        /// <summary>
        /// 节点标示(唯一)
        /// </summary>
        public readonly string NodeID;
        
        /// <summary>
        /// DealingHandler的配置
        /// </summary>
        public IHandlerConfig DealingHandlerConfig;

        /// <summary>
        /// DoneHandler的配置
        /// </summary>
        public IHandlerConfig DoneHandlerConfig;

        #endregion


        //---------Control---------

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ID"></param>
        public ResponseNodeConfig(string ID)
        {
            this.NodeID = ID;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ID"></param>
        public ResponseNodeConfig(string ID, IHandlerConfig dealingHandlerConfig, IHandlerConfig doneHandlerConfig)
        {
            this.NodeID                 = ID;
            this.DealingHandlerConfig   = dealingHandlerConfig;
            this.DoneHandlerConfig      = doneHandlerConfig;
        }

        #endregion

        #region 扩展-获取配置的概要信息

        /// <summary>
        /// 扩展-获取配置的概要信息
        /// </summary>
        public string GetSummary()
        {
            StringBuilder result = new StringBuilder();

            //根节点的判断
            if (String.Equals(ConstString.ROOT_NODE_ID, this.NodeID))
                result.Append("[根節點];　");

            //DealingHandler判断
            if (this.DealingHandlerConfig != null)
            {
                string description = this.DealingHandlerConfig.GetSummary();
                if (String.IsNullOrEmpty(description) == false)
                {
                    result.Append("[主要階段]:");
                    result.Append(description);
                    result.Append(";　");
                }
            }

            //DoneHandler判断
            if (this.DoneHandlerConfig != null)
            {
                string description = this.DoneHandlerConfig.GetSummary();
                if (String.IsNullOrEmpty(description) == false)
                {
                    result.Append("[完畢階段]:");
                    result.Append(description);
                    result.Append("; ");
                }
            }

            return result.ToString();
        }

        #endregion

    }
}