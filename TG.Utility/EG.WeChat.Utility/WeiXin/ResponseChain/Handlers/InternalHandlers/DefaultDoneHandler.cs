using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EG.WeChat.Utility.WeiXin.ResponseChain
{
    /// <summary>
    /// 用于减少重复的Result组合--节点完成时<para />
    /// (提示 "輸入#，{0};\r\n輸入其它內容，返回主功能表。" )
    /// </summary>
    [InputDataTypeLimit(DataTypes.InputAll)]    
    [Description("完畢階段默認處理器")]
    public class DefaultDoneHandler : IHandler
    {

        //-----------Members-----------

        #region 状态成员

        public IResponseMessage ReadyMessage
        {
            get
            {
                string tip = String.Format("輸入#，{0};\r\n輸入其它內容，返回主功能表。", TipText);
                return new ResponseTextMessage(tip);
            }

            //忽略赋值
            set
            {
                ;
            }
        }

        public IResponseMessage SuccessResponseResult { get; set; }

        [Obsolete("不赋值此参数", true)]
        public ResponseTextMessage FailResponseResult { get; set; }

        #endregion

        #region 自身关注的数据

        /// <summary>
        /// 要跳转的节点ID(只读)
        /// </summary>
        public readonly string NodeId = "";

        /// <summary>
        /// 提示信息(只读)
        /// </summary>
        public readonly string TipText = "";

        #endregion


        //-----------Control-----------

        #region 构造函数
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tipText">提示信息</param>
        /// <param name="nodeId">要跳转的节点ID</param>
        public DefaultDoneHandler(string tipText, string nodeId)
        {
            //参数检查
            if (NodeIdValidator.IsValid(nodeId) == false)
                throw new ArgumentException("节点ID格式不正确", "nodeId");

            this.TipText = tipText;
            this.NodeId = nodeId;
        }

        #endregion

        #region 数据处理

        /// <summary>
        /// 数据处理
        /// </summary>
        HandlerResult IHandler.HandlerData(string openId, DataTypes intputType, object rawData)
        {
            //检查
            if (rawData != null &&
                rawData.ToString() == "#")
            {
                this.SuccessResponseResult = new ResponseJumpNode(this.NodeId);
            }
            else
            {
                this.SuccessResponseResult = new ResponseJumpNode(ConstString.ROOT_NODE_ID);
            }

            //始终返回成功(匹配的时候跳节点；不匹配的时候返回主菜单)
            return HandlerResult.Success;
        }

        #endregion

    }
}