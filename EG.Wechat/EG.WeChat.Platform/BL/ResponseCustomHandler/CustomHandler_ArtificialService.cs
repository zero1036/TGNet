using EG.WeChat.Utility.WeiXin.ResponseChain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EG.WeChat.Platform.BL
{
    /// <summary>
    /// 应答链模式 自定义Handler 人工客服
    /// </summary>
    [InputDataTypeLimit(DataTypes.InputAll)]
    [Description("微信會話進入【人工客服】狀態")]
    [DealingHandlerAdditionalBehavior(DealingHandlerAdditionalBehaviorType.IgnoreReadyMessage)]
    public class CustomHandler_ArtificialService : ICustomHandler,
                                                   ICustomHandlerConfigable<CustomHandler_ArtificialService_ConfigClass>                
    {
        //---------Members--------

        #region 状态成员

        public IResponseMessage ReadyMessage
        {
            get;
            set;
        }

        public IResponseMessage SuccessResponseResult
        {
            get;
            set;
        }

        public ResponseTextMessage FailResponseResult
        {
            get;
            set;
        }
        
        #endregion

        #region 配置成员

        /// <summary>
        /// 参数配置的数据成员
        /// </summary>
        public CustomHandler_ArtificialService_ConfigClass ConfigData { get; set; }

        #endregion


        //---------Control--------

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public CustomHandler_ArtificialService()
        {

        }
        #endregion

        #region 处理数据

        /// <summary>
        /// 处理数据
        /// </summary>
        public HandlerResult HandlerData(string openId, DataTypes intputType, object rawData)
        {
            //微信的人工客服
            if (ConfigData != null && ConfigData.TargetAccountList != null && ConfigData.TargetAccountList.Count() > 0)
                this.SuccessResponseResult = new ResponseArtificialCustomerResultMessage(ConfigData.TargetAccountList.ToArray());
            else
                this.SuccessResponseResult = new ResponseArtificialCustomerResultMessage();

            //返回成功
            return HandlerResult.Success;
        }
        
        #endregion

    }


    /// <summary>
    /// 人工客服 的配置类
    /// </summary>
    public class CustomHandler_ArtificialService_ConfigClass : IConfigClassOfCustomHandler
    {
        #region 多客服指定的账号

        /// <summary>
        /// 多客服指定的账号<para />
        /// (数量为0时，表示不明确指定；微信会按照默认形式，通知所有目前可以接入的客服账号)
        /// </summary>
        public readonly List<string> TargetAccountList = new List<string>();

        #endregion

        #region 序列化进Xml节点

        void IConfig.SerializeConfigToXmlNode(System.Xml.Linq.XElement CurrentNode)
        {
            CurrentNode.RemoveNodes();
            CurrentNode.Add(new System.Xml.Linq.XElement("TargetAccountList", ConfigHelper.SerializeToXE(this.TargetAccountList)));
        }

        #endregion

        #region 从Xml节点反序列化

        void IConfig.DeserializeConfigFromXmlNode(System.Xml.Linq.XElement CurrentNode)
        {
            //获取目标节点
            System.Xml.Linq.XElement xeTargetAccountList = CurrentNode.Element("TargetAccountList");
            if (xeTargetAccountList == null)
                return;

            //读取数据
            List<string> lstData = ConfigHelper.DeserializeFromString<List<string>>(xeTargetAccountList.FirstNode.ToString());
            this.TargetAccountList.Clear();
            this.TargetAccountList.AddRange(lstData);
        }

        #endregion
    }

}
