using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Senparc.Weixin.MP.AdvancedAPIs.GroupMessage;
using Senparc.Weixin.MP.AdvancedAPIs.Custom;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
/*****************************************************
* 目的：
* 创建人：
* 创建时间：
* 修改目的：添加发送群发信息通用api
* 修改人：林子聪
* 修改时间：20141125
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Service.WeiXin
{
    /// <summary>
    /// 微信消息工具类
    /// <para />
    /// (单方向发送消息给指定用户) 
    /// <para />
    /// [目前使用“客服消息接口”进行处理；模板消息请使用本类的SendTemplateMessage方法。]
    /// </summary>
    public sealed class WeixinMessageSender
    {
        //---Public:

        #region 发送纯文本信息

        /// <summary>
        /// 发送纯文本
        /// </summary>
        /// <param name="openId">OPENID</param>
        /// <param name="text">纯文本信息</param>
        public static void SendText(string openId, string text)
        {
            if (string.IsNullOrEmpty(openId))
                return;

            var accessToken = GetAccessToken();
            var result = CustomApi.SendText(accessToken, openId, text);
            LogErrorInfo(result, openId, "SendText");
        }

        #endregion

        #region 发送图文消息

        /* 参数范例：
         articles.Add(new Article()
            {
                Title       = "标题信息",
                Description = "描述信息",
                Url         = "http://webchat.cloudapp.net/",
                PicUrl      = "http://webchat.cloudapp.net/Images/qrcode.jpg"
            });
         */

        /// <summary>
        /// 发送文章
        /// </summary>
        /// <param name="openId">OPENID</param>
        /// <param name="articles">Article集合。内容可以参考<seealso cref="Article"/>类。</param>
        public void SendArticle(string openId, List<Article> articles)
        {
            var accessToken = GetAccessToken();
            var result = CustomApi.SendNews(accessToken, openId, articles);
            LogErrorInfo(result, openId, "SendArticle");
        }

        #endregion

        #region 发送纯图片信息

        /// <summary>
        /// 发送纯图片
        /// </summary>
        /// <param name="openId">OPENID</param>
        /// <param name="mediaId">上传在公众平台的资源ID</param>
        public static void SendImage(string openId, string mediaId)
        {
            var accessToken = GetAccessToken();
            var result = CustomApi.SendImage(accessToken, openId, mediaId);
            LogErrorInfo(result, openId, "SendImage");
        }

        #endregion

        #region 发送语音信息

        /// <summary>
        /// 发送语音
        /// </summary>
        /// <param name="openId">OPENID</param>
        /// <param name="mediaId">上传在公众平台的资源ID</param>
        public static void SendVoice(string openId, string mediaId)
        {
            var accessToken = GetAccessToken();

            var result = CustomApi.SendVoice(accessToken, openId, mediaId);
            LogErrorInfo(result, openId, "SendVoice");
        }

        #endregion

        #region 发送视频消息
        /// <summary>
        /// 发送视频
        /// </summary>
        /// <param name="openId">OPENID</param>
        /// <param name="mediaId">上传在公众平台的资源ID</param>
        /// <param name="title">标题</param>
        /// <param name="description">说明</param>
        public static void SendVideo(string openId, string mediaId, string title, string description)
        {
            var accessToken = GetAccessToken();

            var result = CustomApi.SendVideo(accessToken, openId, mediaId, title, description);
            LogErrorInfo(result, openId, "SendVideo");
        }
        #endregion

        #region 发送模板消息

        /* 参数范例：

         public class TemplateData_SensitiveAction
         {
            public TemplateDataItem first { get; set; }
            public TemplateDataItem time { get; set; }
            public TemplateDataItem sec_type { get; set; }
            public TemplateDataItem remark { get; set; }
         }
         * 这里的字段名，需要与 模板的内容对应，比如：{{first.DATA}}，则这里对应为 first。
          
         var data = new TemplateData_SensitiveAction()
         {
            first       = new TemplateDataItem("您好，您的英皇账户，有一条敏感操作确认消息。"),
            time        = new TemplateDataItem(DateTime.Now.ToString()),
            sec_type    = new TemplateDataItem("资金转移超过每日上限。"),
            remark      = new TemplateDataItem("更详细信息，请点击进行查看！")
         };
          
         */

        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <typeparam name="TData">匹配模板的Class的类型，成员需为TemplateDataItem。</typeparam>
        /// <param name="openId">OPENID</param>
        /// <param name="templateId">模板ID</param>
        /// <param name="topcolor">顶部颜色</param>
        /// <param name="url">点击后要打开的网址</param>
        /// <param name="data">配模板的Class</param>
        public static void SendTemplateMessage<TData>(string openId, string templateId, Color topcolor, string url, TData data)
        {
            ExecuteTryCatch(() =>
            {
                var accessToken = GetAccessToken();
                var strColor = WebColorConvertor.ConvertToString(topcolor);

                var result = TemplateApi.SendTemplateMessage<TData>(accessToken, openId, templateId, strColor, url, data);
                LogErrorInfo(result, openId, "SendTemplateMessage");
            });
        }

        #endregion

        #region 发送群发消息
        /// <summary>
        /// 根据分组进行群发
        /// 
        /// 请注意：
        /// 1、该接口暂时仅提供给已微信认证的服务号
        /// 2、虽然开发者使用高级群发接口的每日调用限制为100次，但是用户每月只能接收4条，请小心测试
        /// 3、无论在公众平台网站上，还是使用接口群发，用户每月只能接收4条群发消息，多于4条的群发将对该用户发送失败。
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="groupId">群发到的分组的group_id</param>
        /// <param name="mediaId">用于群发的消息的media_id</param>
        /// <returns></returns>
        public static SendResult SendGroupMessageByGroupId(string groupId, string mediaId, string content, string messageType)
        {
            //获取accessToken
            string accessToken = GetAccessToken();
            const string urlFormat = "https://api.weixin.qq.com/cgi-bin/message/mass/sendall?access_token={0}";
            //获取群发消息json模板对象
            var data = GetGroupMessageJsonTemplate(groupId, mediaId, content, messageType);
            if (data == null)
                return null;
            return CommonJsonSend.Send<SendResult>(accessToken, urlFormat, data);
        }
        /// <summary>
        /// 根据OpenId进行群发
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="mediaId">用于群发的消息的media_id</param>
        /// <param name="openIds">openId字符串数组</param>
        /// <returns></returns>
        public static SendResult SendGroupMessageByOpenId(string mediaId, string content, string messageType, params string[] openIds)
        {
            //获取accessToken
            string accessToken = GetAccessToken();
            const string urlFormat = "https://api.weixin.qq.com/cgi-bin/message/mass/send?access_token={0}";
            //获取群发消息json模板对象
            var data = GetGroupMessageJsonTemplate(mediaId, content, messageType, openIds);
            if (data == null)
                return null;
            return CommonJsonSend.Send<SendResult>(accessToken, urlFormat, data);
        }
        /// <summary>
        /// 删除群发消息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="mediaId">发送出去的消息ID</param>
        /// <returns></returns>
        public static WxJsonResult DeleteSendMessage(string mediaId)
        {
            //获取accessToken
            string accessToken = GetAccessToken();
            return GroupMessageApi.DeleteSendMessage(accessToken, mediaId);
        }
        #endregion

        //---Private:

        #region 获取AccessToken

        /// <summary>
        /// 获取 当前公众号对应的AccessToken
        /// </summary>
        /// <returns></returns>
        private static string GetAccessToken()
        {
            return WeiXinSDKExtension.GetCurrentAccessToken();
        }

        #endregion

        #region 记录日志

        /// <summary>
        /// 记录错误信息
        /// </summary>
        /// <param name="result">返回的Json结果</param>
        /// <param name="openId">OPENID</param>
        /// <param name="actionName">方法名</param>
        private static void LogErrorInfo(WxJsonResult result, string openId, string actionName)
        {
            if (result.errcode != Senparc.Weixin.ReturnCode.请求成功)
            {
                //拼凑错误信息
                string errorRecord = String.Format("{0} throw an exception,openId={1},errorcode={2},message={3}",
                                                   actionName,
                                                   openId,
                                                   result.errcode,
                                                   result.errmsg);

                Logger.Log4Net.Error(errorRecord);
            }
        }

        #endregion

        #region 颜色转换器

        /// <summary>
        /// 颜色转换器<para/>
        /// (System.Drawing.Color => #000000 )
        /// </summary>
        private static WebColorConverter WebColorConvertor = new WebColorConverter();

        #endregion

        #region 执行方法并捕获错误
        /// <summary>
        /// 执行方法并捕获错误
        /// </summary>
        /// <param name="action">方法</param>
        private static void ExecuteTryCatch(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception ex)
            {
                Logger.Log4Net.Error("WeixinMessageSenderError", ex);
            }
        }


        #endregion

        #region 模板转换
        /// <summary>
        /// 获取群发消息json模板对象
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="mediaId"></param>
        /// <param name="content"></param>
        /// <param name="messageType"></param>
        /// <returns></returns>
        /// <remarks>暂时写死，未来改为配置</remarks>
        private static object GetGroupMessageJsonTemplate(string groupId, string mediaId, string content, string messageType)
        {
            switch (messageType)
            {
                case "text":
                    var data = new
                    {
                        filter = new
                        {
                            group_id = groupId
                        },
                        text = new
                        {
                            content = content
                        },
                        msgtype = messageType
                    };
                    return data;
                case "image":
                    var data2 = new
                    {
                        filter = new
                        {
                            group_id = groupId
                        },
                        image = new
                        {
                            media_id = mediaId
                        },
                        msgtype = messageType
                    };
                    return data2;
                case "voice":
                    var data3 = new
                    {
                        filter = new
                        {
                            group_id = groupId
                        },
                        voice = new
                        {
                            media_id = mediaId
                        },
                        msgtype = messageType
                    };
                    return data3;
                case "video":
                    var data4 = new
                    {
                        filter = new
                        {
                            group_id = groupId
                        },
                        video = new
                        {
                            media_id = mediaId
                        },
                        msgtype = messageType
                    };
                    return data4;
                case "mpnews":
                    var data5 = new
                    {
                        filter = new
                        {
                            group_id = groupId
                        },
                        mpnews = new
                        {
                            media_id = mediaId
                        },
                        msgtype = messageType
                    };
                    return data5;
            }
            return null;
        }
        /// <summary>
        /// 获取群发消息json模板对象
        /// </summary>
        /// <param name="mediaId"></param>
        /// <param name="content"></param>
        /// <param name="messageType"></param>
        /// <param name="openIds"></param>
        /// <returns></returns>
        private static object GetGroupMessageJsonTemplate(string mediaId, string content, string messageType, params string[] openIds)
        {
            switch (messageType)
            {
                case "text":
                    var data = new
                    {
                        touser = openIds,
                        text = new
                        {
                            content = content
                        },
                        msgtype = messageType
                    };
                    return data;
                case "image":
                    var data2 = new
                    {
                        touser = openIds,
                        image = new
                        {
                            media_id = mediaId
                        },
                        msgtype = messageType
                    };
                    return data2;
                case "voice":
                    var data3 = new
                    {
                        touser = openIds,
                        voice = new
                        {
                            media_id = mediaId
                        },
                        msgtype = messageType
                    };
                    return data3;
                case "video":
                    var data4 = new
                    {
                        touser = openIds,
                        video = new
                        {
                            media_id = mediaId
                        },
                        msgtype = messageType
                    };
                    return data4;
                case "mpnews":
                    var data5 = new
                    {
                        touser = openIds,
                        mpnews = new
                        {
                            media_id = mediaId
                        },
                        msgtype = messageType
                    };
                    return data5;
            }
            return null;
        }
        #endregion
    }

}