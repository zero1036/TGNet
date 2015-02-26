using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Web;
using System.Xml.Serialization;

namespace EG.WeChat.Utility.WeiXin.ResponseChain
{
    /// <summary>
    /// 内置提供的默认的处理器
    /// </summary>
    [InputDataTypeLimit(DataTypes.InputAll)]
    [DealingHandlerAdditionalBehavior(DealingHandlerAdditionalBehaviorType.IgnoreReadyMessage)]
    [Description("處理階段默認處理器")]
    public class DefaultDealingHandler : IHandler
    {

        //-----------Members-----------

        #region 状态成员

        public IResponseMessage ReadyMessage { get; set; }

        public IResponseMessage SuccessResponseResult { get; set; }

        public ResponseTextMessage FailResponseResult { get; set; }

        private IResponseMessage DealingMessage {get;set;}

        #endregion


        //-----------Control-----------

        #region 构造函数

        /// <summary>
        /// 隐藏
        /// </summary>
        private DefaultDealingHandler()
        { }

        #endregion

        #region 处理

        HandlerResult IHandler.HandlerData(string openId, DataTypes intputType, object rawData)
        {
            SuccessResponseResult = DealingMessage;

            //始终返回成功
            return HandlerResult.Success;
        }

        #endregion


        //-----------Static------------

        #region 创建实例

        private static DefaultDealingHandler InternalCreateInstance(IResponseMessage DealingMessage)
        {
            DefaultDealingHandler result = new DefaultDealingHandler
            {
                DealingMessage = DealingMessage,
            };

            return result;
        }

        /// <summary>
        /// 创建文本类型的默认处理
        /// </summary>
        /// <param name="tipText"></param>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        public static DefaultDealingHandler CreateInstance_Text(string tipText)
        {
            //string tip = String.Format("{0}\r\n\r\n输入任意内容，返回主菜单。", tipText);
            string tip = String.Format("{0}", tipText);
            ResponseTextMessage message = new ResponseTextMessage(tip);

            return InternalCreateInstance(message);
        }

        /// <summary>
        /// 创建单一图文类型的默认处理
        /// </summary>
        /// <param name="tipText"></param>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        public static DefaultDealingHandler CreateInstance_SingleNews(ArticleCan article)
        {
            ResponseNewsResult message = new ResponseNewsResult(article);

            return InternalCreateInstance(message);
        }

        #endregion
    }
}