using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web.Configuration;
using Senparc.Weixin.MP.Agent;
using Senparc.Weixin.Context;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.MessageHandlers;
using Senparc.Weixin.MP.Helpers;
using EG.WeChat.Service.WeiXin;
using System.Drawing;
using RC = EG.WeChat.Utility.WeiXin.ResponseChain;
using Senparc.Weixin.MP.Entities.Request;

namespace EG.WeChat.Utility.WeiXin
{
    /* EG.WeChat.Utility的CustomMessageHandler，提供通用的消息处理；
     * 需要加入定制化的逻辑时，由各自业务系统的Business项目，处理(可以参考标准库Business项目的WeiXinMessageHandler)：
     * 1.处理完之后，需要继续交由通用逻辑时，重载之后使用Base.方法名；
     * 2.处理完之后，不需要交由通用逻辑时，直接在重载结束即可。
     */


    /// <summary>
    /// 自定义MessageHandler
    /// 把MessageHandler作为基类，重写对应请求的处理方法
    /// </summary>
    public partial class CustomMessageHandler : MessageHandler<CustomMessageContext>
    {
        /*
         * 重要提示：v1.5起，MessageHandler提供了一个DefaultResponseMessage的抽象方法，
         * DefaultResponseMessage必须在子类中重写，用于返回没有处理过的消息类型（也可以用于默认消息，如帮助信息等）；
         * 其中所有原OnXX的抽象方法已经都改为虚方法，可以不必每个都重写。若不重写，默认返回DefaultResponseMessage方法中的结果。
         */

        public CustomMessageHandler(Stream inputStream,PostModel postModel, int maxRecordCount = 0)
            : base(inputStream, postModel,maxRecordCount)
        {
            //这里设置仅用于测试，实际开发可以在外部更全局的地方设置，
            //比如MessageHandler<MessageContext>.GlobalWeixinContext.ExpireMinutes = 3。

            //设置为5分钟吧
            WeixinContext.ExpireMinutes = 5;
        }

        public override void OnExecuting()
        {
            //测试MessageContext.StorageData
            if (CurrentMessageContext.StorageData == null)
            {
                CurrentMessageContext.StorageData = 0;
            }
            base.OnExecuting();
        }

        public override void OnExecuted()
        {
            base.OnExecuted();
            CurrentMessageContext.StorageData = ((int)CurrentMessageContext.StorageData) + 1;
        }

        /// <summary>
        /// 处理文字请求
        /// </summary>
        /// <returns></returns>
        public override IResponseMessageBase OnTextRequest(RequestMessageText requestMessage)
        {
            #region 来自SDK的建议注释

            //TODO:这里的逻辑可以交给Service处理具体信息，参考OnLocationRequest方法或/Service/LocationSercice.cs

            //方法一（v0.1），此方法调用太过繁琐，已过时（但仍是所有方法的核心基础），建议使用方法二到四
            //var responseMessage =
            //    ResponseMessageBase.CreateFromRequestMessage(RequestMessage, ResponseMsgType.Text) as
            //    ResponseMessageText;

            //方法二（v0.4）
            //var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(RequestMessage);

            //方法三（v0.4），扩展方法，需要using Senparc.Weixin.MP.Helpers;
            //var responseMessage = RequestMessage.CreateResponseMessage<ResponseMessageText>();

            //方法四（v0.6+），仅适合在HandlerMessage内部使用，本质上是对方法三的封装
            //注意：下面泛型ResponseMessageText即返回给客户端的类型，可以根据自己的需要填写ResponseMessageNews等不同类型。

            #endregion

            #region 不同的应答链模块
            /* 使用不同的模块去处理。具体使用的时候，请只使用其中一种。 */

            //## 统一模块进行查询的新应答链模块,By MarkLin
            //return XXXX到时的函数名称(requestMessage);

            //## 带[当前节点级别]的应答链模块,By Doraemon
            return ResponseChainRequest_LevelForNode(RC.DataTypes.Text, requestMessage);

            #endregion
        }

        /// <summary>
        /// 处理位置请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnLocationRequest(RequestMessageLocation requestMessage)
        {
            return null;

            #region SDK范例

            //return ResponseChainRequest(RC.DataTypes.Location, requestMessage);
            //var locationService = new LocationService();
            //var responseMessage = locationService.GetResponseMessage(requestMessage as RequestMessageLocation);
            //return responseMessage;

            //我们的地理信息测试代码
            //Logger.Log4Net.Info(String.Format("地理位置B：維度：{0}，經度：{1}，比例尺：{2}", 
            //                                  requestMessage.Location_X,
            //                                  requestMessage.Location_Y,
            //                                  requestMessage.Scale));


            #endregion
        }

        /// <summary>
        /// 处理图片请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnImageRequest(RequestMessageImage requestMessage)
        {
            return ResponseChainRequest_LevelForNode(RC.DataTypes.Image, requestMessage);

            #region SDK范例
            //var responseMessage = CreateResponseMessage<ResponseMessageNews>();
            //responseMessage.Articles.Add(new Article()
            //{
            //    Title = "您刚才发送了图片信息",
            //    Description = "您发送的图片将会显示在边上",
            //    PicUrl = requestMessage.PicUrl,
            //    Url = "http://weixin.senparc.com"
            //});
            //responseMessage.Articles.Add(new Article()
            //{
            //    Title = "第二条",
            //    Description = "第二条带连接的内容",
            //    PicUrl = requestMessage.PicUrl,
            //    Url = "http://weixin.senparc.com"
            //});
            //return responseMessage;
            #endregion
        }

        /// <summary>
        /// 处理语音请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnVoiceRequest(RequestMessageVoice requestMessage)
        {
            return ResponseChainRequest_LevelForNode(RC.DataTypes.Voice, requestMessage);

            #region SDK范例
            //var responseMessage = CreateResponseMessage<ResponseMessageMusic>();
            //responseMessage.Music.MusicUrl = "http://weixin.senparc.com/Content/music1.mp3";
            //responseMessage.Music.Title = "这里是一条音乐消息";
            //responseMessage.Music.Description = "来自Jeffrey Su的美妙歌声~~";
            //responseMessage.Music.ThumbMediaId = "mediaid";
            //return responseMessage;
            #endregion
        }

        /// <summary>
        /// 处理视频请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnVideoRequest(RequestMessageVideo requestMessage)
        {
            return ResponseChainRequest_LevelForNode(RC.DataTypes.Video, requestMessage);

            #region SDK范例
            //return ResponseChainRequest(RC.DataTypes.Video, requestMessage);
            #endregion
        }

        /// <summary>
        /// 处理链接消息请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnLinkRequest(RequestMessageLink requestMessage)
        {
            return null;
        }

        /// <summary>
        /// 处理事件请求（这个方法一般不用重写，这里仅作为示例出现。除非需要在判断具体Event类型以外对Event信息进行统一操作
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEventRequest(IRequestMessageEventBase requestMessage)
        {
            var eventResponseMessage = base.OnEventRequest(requestMessage);//对于Event下属分类的重写方法，见：CustomerMessageHandler_Events.cs
            //TODO: 对Event信息进行统一操作
            return eventResponseMessage;
        }

        public override IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage)
        {
            /* 所有没有被处理的消息会默认返回这里的结果，
             * 因此，如果想把整个微信请求委托出去（例如需要使用分布式或从其他服务器获取请求），
             * 只需要在这里统一发出委托请求，如：
             * var responseMessage = MessageAgent.RequestResponseMessage(agentUrl, agentToken, RequestDocument.ToString());
             * return responseMessage;
             */

            var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "这条消息来自DefaultResponseMessage。";
            return responseMessage;
        }


        #region 带[当前节点级别]的应答链模块,By Doraemon

        private IResponseMessageBase ResponseChainRequest_LevelForNode(RC.DataTypes inputType, IRequestMessageBase baseRequestMessage)
        {
            //获取关注的参数
            string openId = baseRequestMessage.FromUserName;
            object rawObject = null;
            switch (inputType)
            {
                case RC.DataTypes.Text:
                    rawObject = ((RequestMessageText)baseRequestMessage).Content;
                    break;

                case RC.DataTypes.Image:
                    {
                        var inputData = baseRequestMessage as RequestMessageImage;
                        if (inputData != null)
                        {
                            rawObject = new RC.ImageCan(inputData.PicUrl, inputData.MediaId);
                        }
                    }
                    break;

                case RC.DataTypes.Voice:
                    {
                        var inputData = baseRequestMessage as RequestMessageVoice;
                        if (inputData != null)
                        {
                            rawObject = new RC.VoiceCan(inputData.Format, inputData.Recognition, inputData.MediaId);
                        }
                    }
                    break;

                case RC.DataTypes.Video:
                    {
                        var inputData = baseRequestMessage as RequestMessageVideo;
                        if (inputData != null)
                        {
                            rawObject = new RC.VideoCan(inputData.ThumbMediaId, inputData.MediaId);
                        }
                    }
                    break;

                #region 后面才去支持
                //case DataTypes.Location:
                //    break;

                //case DataTypes.Link:
                //    break;                
                #endregion

                default:
                    return null;
            }

            //用应答链去处理
            RC.ResponseService service = RC.ResponseService_ConfigurationSupport.CreateOrGetService(this.CurrentMessageContext);     //[使用配置文件方式]
            RC.IResponseMessage ret = null;
            try
            {
                ret = service.HanderData(openId, inputType, rawObject);
            }
            catch (Exception ex)
            {
                Logger.Log4Net.Error("应答模块捕获异常。", ex);
            }

            //返回结果
            return ResponseChainRequest_LevelForNode_ResultHandler(ret);
        }
        
        /// <summary>
        /// 处理应答链的结果，转为SDK的结果
        /// </summary>
        private IResponseMessageBase ResponseChainRequest_LevelForNode_ResultHandler(RC.IResponseMessage ret)
        {
            //根据应答链的处理结果，构造微信SDK的结果
            IResponseMessageBase result = null;
            if (ret is RC.ResponseTextMessage)
            {
                ResponseMessageText result_text = base.CreateResponseMessage<ResponseMessageText>();
                result_text.Content = ((RC.ResponseTextMessage)ret).Context;
                result = result_text;
            }
            else if (ret is RC.ResponseNewsResult)
            {
                ResponseMessageNews result_text = base.CreateResponseMessage<ResponseMessageNews>();
                result_text.Articles = new System.Collections.Generic.List<Article>();
                foreach (RC.ArticleCan item in ((RC.ResponseNewsResult)ret).ArticleList)
                {
                    result_text.Articles.Add(new Article
                    {
                        Description = item.Description,
                        PicUrl = item.PicUrl,
                        Title = item.Title,
                        Url = item.Url
                    });
                }
                result = result_text;
            }
            else if (ret is RC.ResponseArtificialCustomerResultMessage)
            {
                string[] accountList = ((RC.ResponseArtificialCustomerResultMessage)ret).AccountList;
                return GetArtificialCustomerService(accountList);
            }
            else if (ret is RC.ResponseRawResult)
            {
                result = this.ResponseChainRequest_LevelForNode_RawResultHandler((RC.ResponseRawResult)ret);
            }

            //返回结果
            return result;
        }

        /// <summary>
        /// 允许处理应答链中，外部自定义“Raw”数据的处理
        /// </summary>
        /// <param name="rawData"></param>
        /// <returns></returns>
        internal virtual IResponseMessageBase ResponseChainRequest_LevelForNode_RawResultHandler(RC.ResponseRawResult rawData)
        {
            if (rawData.RawData is RC.FunctionActionType)
            {
                //FunctionActionType类型
                switch ((RC.FunctionActionType)rawData.RawData)
                {
                    case RC.FunctionActionType.ArtificialService:
                        return GetArtificialCustomerService();
                }

            }

            //没有匹配的处理
            return null;
        }

        #endregion

        #region 统一模块进行查询的新应答链模块,By MarkLin

        //IResponseMessageBase XXXX到时的函数名称(IRequestMessageBase requestMessage){ }

        #endregion

    }
}
