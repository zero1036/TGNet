using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using EG.WeChat.Service.WeiXin;


namespace EG.WeChat.Web.Controllers
{
    using Senparc.Weixin.MP.MessageHandlers;
    using Senparc.Weixin.MP.Entities;
    using Senparc.Weixin.MP.Helpers;
    using Senparc.Weixin.MP.MvcExtension;
    //using Senparc.Weixin.MP.Sample.Service;
    //using Senparc.Weixin.MP.Sample.CustomerMessageHandler;
    using EG.WeChat.Utility.WeiXin.ResponseChain;
    using EG.WeChat.Utility.WeiXin.ResponseChain.CustomHandlers;
    using Senparc.Weixin.MP;
    using EG.WeChat.Utility.WeiXin;
    using EG.WeChat.Business.BL.WeiXin;
    using Senparc.Weixin.MP.Entities.Request;
    using Senparc.Weixin.MP.Helpers;

    public partial class WeixinController : Controller
    {
        public static readonly string Token = WeiXinConfiguration.Token;//与微信公众账号后台的Token设置保持一致，区分大小写。

        public WeixinController()
        {

        }

        /// <summary>
        /// 微信后台验证地址（使用Get），微信后台的“接口配置信息”的Url填写如：http://weixin.senparc.com/weixin
        /// </summary>
        [HttpGet]
        [ActionName("Index")]
        public ActionResult Get(string signature, string timestamp, string nonce, string echostr)
        {
            if (CheckSignature.Check(signature, timestamp, nonce, Token))
            {
                return Content(echostr); //返回随机字符串则表示验证通过
            }
            else
            {
                return Content("failed:" + signature + "," + CheckSignature.GetSignature(timestamp, nonce, Token) + "。"+
                    "如果你在浏览器中看到这句话，说明此地址可以被作为微信公众账号后台的Url，请注意保持Token一致。");
            }
        }

        /// <summary>
        /// 用户发送消息后，微信平台自动Post一个请求到这里，并等待响应XML。
        /// PS：此方法为简化方法，效果与OldPost一致。
        /// v0.8之后的版本可以结合Senparc.Weixin.MP.MvcExtension扩展包，使用WeixinResult，见MiniPost方法。
        /// </summary>
        [HttpPost]
        [ActionName("Index")]
        public ActionResult Post(PostModel postModel)
        {
            if (!CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, Token))
            {
                return Content("参数错误！");
            }

            //填充信息给MessageHandler用(微信设置为 加密模式时 会用到)
            postModel.Token             = Token;
            postModel.EncodingAESKey    = WeiXinConfiguration.EncodingAESKey;
            postModel.AppId             = WeiXinConfiguration.appID;

            //v4.2.2之后的版本，可以设置每个人上下文消息储存的最大数量，防止内存占用过多，如果该参数小于等于0，则不限制
            var maxRecordCount = 10;

            //自定义MessageHandler，对微信请求的详细判断操作都在这里面。
            var messageHandler = new WeiXinMessageHandler(Request.InputStream, postModel, maxRecordCount);

            try
            {
#if DEBUG
                //测试时可开启此记录，帮助跟踪数据，使用前请确保App_Data文件夹存在，且有读写权限。
                messageHandler.RequestDocument.Save(Server.MapPath("~/App_Data/" + DateTime.Now.Ticks + "_Request_" + messageHandler.RequestMessage.FromUserName + ".txt"));
                if (messageHandler.UsingEcryptMessage)
                {
                    messageHandler.EcryptRequestDocument.Save(Server.MapPath("~/App_Data/" + DateTime.Now.Ticks + "_Request_Ecrypt_" + messageHandler.RequestMessage.FromUserName + ".txt"));
                }
#endif

                /* 如果需要添加消息去重功能，只需打开OmitRepeatedMessage功能，SDK会自动处理。
                 * 收到重复消息通常是因为微信服务器没有及时收到响应，会持续发送2-5条不等的相同内容的RequestMessage*/
                messageHandler.OmitRepeatedMessage = true;

                //执行微信处理过程
                messageHandler.Execute();

#if DEBUG
                //测试时可开启，帮助跟踪数据
                messageHandler.ResponseDocument.Save(Server.MapPath("~/App_Data/" + DateTime.Now.Ticks + "_Response_" + messageHandler.ResponseMessage.ToUserName + ".txt"));
                if (messageHandler.UsingEcryptMessage)
                {
                    //记录加密后的响应信息
                    messageHandler.FinalResponseDocument.Save(Server.MapPath("~/App_Data/" + DateTime.Now.Ticks + "_Response_Final_" + messageHandler.ResponseMessage.ToUserName + ".txt"));
                }
#endif

                //return Content(messageHandler.ResponseDocument.ToString());//v0.7-
                return new FixWeixinBugWeixinResult(messageHandler);//为了解决官方微信5.0软件换行bug暂时添加的方法，平时用下面一个方法即可
                //return new WeixinResult(messageHandler);//v0.8+
            }
            catch (Exception ex)
            {
                Logger.Log4Net.Error("WeixinController Post Error.", ex);
                return Content("");
            }
        }


        /// <summary>
        /// 最简化的处理流程
        /// </summary>
        [HttpPost]
        [ActionName("MiniPost")]
        public ActionResult MiniPost(string signature, string timestamp, string nonce, string echostr)
        {
            if (!CheckSignature.Check(signature, timestamp, nonce, Token))
            {
                //return Content("参数错误！");//v0.7-
                return new WeixinResult("参数错误！");//v0.8+
            }

            var messageHandler = new CustomMessageHandler(Request.InputStream,null,10);

            messageHandler.Execute();//执行微信处理过程

            //return Content(messageHandler.ResponseDocument.ToString());//v0.7-
            return new FixWeixinBugWeixinResult(messageHandler);//v0.8+
            return new WeixinResult(messageHandler);//v0.8+
        }

        /*
         * v0.3.0之前的原始Post方法见：WeixinController_OldPost.cs
         * 
         * 注意：虽然这里提倡使用CustomerMessageHandler的方法，但是MessageHandler基类最终还是基于OldPost的判断逻辑，
         * 因此如果需要深入了解Senparc.Weixin.MP内部处理消息的机制，可以查看WeixinController_OldPost.cs中的OldPost方法。
         * 目前为止OldPost依然有效，依然可用于生产。
         */



        /// <summary>
        /// 将 请求/响应 的XML文档，记录到文件（只用于调试目的）
        /// </summary>
        /// <param name="document">XDocument</param>
        /// <param name="filePath">路径</param>
        private void RecordXmlDocumentToFile(XDocument document,string filePath)
        {
            //检验
            if (document == null)
                return;

            //写入准备
            StringBuilder testingDatas  = new StringBuilder();
            StringWriter sw             = new StringWriter(testingDatas);
            document.Save(sw);
            testingDatas.Insert(0, DateTime.Now.ToString("\r\n------------ yyyy-MM-dd HH:mm:ss------------\r\n"));

            //写入文件
            System.IO.File.AppendAllText(filePath, testingDatas.ToString());
        }
    }
}
