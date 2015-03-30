using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.Media;
using Senparc.Weixin.MP.CommonAPIs;
using System.Net;
using System.Web;
using EG.WeChat.Service;
using EG.WeChat.Utility.Tools;
using EG.WeChat.Utility.WeiXin;
using EG.WeChat.Platform.Model;
/*****************************************************
* 目的：QYSdkETS
* 创建人：林子聪
* 创建时间：20150129
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Platform.BL
{
    /// <summary>
    /// 
    /// </summary>
    public class QYSdkETS : CommonSdkETS, ISdkETS_QY, ISdkETS
    {
        #region Singleon
        public static QYSdkETS Singleon = new QYSdkETS();
        #endregion

        #region Base
        /// <summary>
        /// 微信账号类型：1：公众号；2：企业号
        /// </summary>
        public int WXType { get { return 2; } }
        /// <summary>
        /// 微信账号类型：1：公众号；2：企业号
        /// </summary>
        public Func<string> GetAToken
        {
            get { return WeiXinSDKExtension.GetCurrentAccessTokenQY; }
        }
        #endregion

        #region Media
        /// <summary>
        /// 媒体上传方法
        /// </summary>
        public Func<string, string, string, int, object> MediaUpload
        {
            get { return Senparc.Weixin.QY.AdvancedAPIs.Media.MediaApiX.Upload; }
        }
        /// <summary>
        /// 图文消息实体转换委托——NewsModel转换主动发送Object
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Func<NewsModelX, object> CNews2SendArticle(string type)
        {
            var dic = GetDicNewsModel();
            if (dic.ContainsKey(type))
            {
                return dic[type];
            }
            return null;
        }
        private Dictionary<string, Func<NewsModelX, object>> _DicNewsConvertToArticleFunc;
        /// <summary>
        /// 图文消息实体转换委托——NewsModel转换被动回复Article
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Func<NewsModelX, object> CNews2RspArticle(string type)
        {
            if (_DicNewsConvertToArticleFunc == null || _DicNewsConvertToArticleFunc.Count == 0)
            {
                _DicNewsConvertToArticleFunc = new Dictionary<string, Func<NewsModelX, object>>();
                _DicNewsConvertToArticleFunc.Add("news", (z) => new Senparc.Weixin.QY.Entities.Article
                {
                    Title = z.title,
                    Description = z.digest,
                    Url = z.content_source_url,
                    PicUrl = z.RPath
                });
                _DicNewsConvertToArticleFunc.Add("mpnews", (z) => new Senparc.Weixin.QY.Entities.Article
                {
                    Title = z.title,
                    Description = z.digest,
                    Url = z.content_source_url,
                    PicUrl = z.RPath
                });
            }
            return _DicNewsConvertToArticleFunc[type];
        }
        /// <summary>
        /// 获取素材转换方法
        /// </summary>
        /// <param name="msgType"></param>
        /// <returns></returns>
        public Func<object, string, object> GetResConvert(string msgType)
        {
            return base.GetDicResourceConvert(this.WXType, msgType, this.CNews2SendArticle(msgType));
        }


        #endregion

        #region 企业独占方法
        #region Response Function
        private Dictionary<string, Func<int, string, Senparc.Weixin.QY.Entities.IResponseMessageBase>> _DicRCForResponse;
        /// <summary>
        /// 获取素材转换方法——用于转换被动回复消息
        /// 暂时企业号独占
        /// </summary>
        /// <param name="msgType"></param>
        /// <returns></returns>
        public Func<int, string, Senparc.Weixin.QY.Entities.IResponseMessageBase> GetResConvertForResponse(string msgType)
        {
            if (_DicRCForResponse == null || _DicRCForResponse.Count == 0)
            {
                _DicRCForResponse = new Dictionary<string, Func<int, string, Senparc.Weixin.QY.Entities.IResponseMessageBase>>();


                //    _DicRCForResponse.Add("text", (int lcId, string mType) =>
                //    {
                //Senparc.Weixin.QY.Entities.ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(
                //        return new TObject
                //        {
                //            MsgType = Senparc.Weixin.QY.ResponseMsgType.Text,
                //            Content = content
                //        };
                //    });
                //_DicRCForResponse.Add("image", (int lcId, string mType) =>
                //{
                //    return new
                //    {
                //        MsgType = Senparc.Weixin.QY.ResponseMsgType.Image,
                //        Image = media_id
                //    };
                //});
                //_DicRCForResponse.Add("voice", (object media_id, string mType) =>
                //{
                //    return new
                //    {
                //        MsgType = Senparc.Weixin.QY.ResponseMsgType.Voice,
                //        Voice = media_id
                //    };
                //});
                //_DicRCForResponse.Add("video", (object media_id, string mType) =>
                //{
                //    return new
                //    {
                //        MsgType = Senparc.Weixin.QY.ResponseMsgType.Video,
                //        Video = new
                //        {
                //            MediaId = media_id,
                //            Title = "",
                //            Description = ""
                //        }
                //    };
                //});
                _DicRCForResponse.Add("news", (int lcId, string mType) =>
                {
                    var pAr = new WeChatArticleService("QY");
                    pAr.ArticleConvertFunc = this.CNews2RspArticle(msgType);
                    List<object> pos = pAr.LoadResources2News(lcId, mType);

                    var responseMessage = new Senparc.Weixin.QY.Entities.ResponseMessageNews();
                    responseMessage.Articles = pos.Select(p => (p as Senparc.Weixin.QY.Entities.Article)).ToList();
                    return responseMessage;
                });
                //_DicRCForResponse.Add("mpnews", new WeChatArticleService("QY").LoadResources2News);
            }

            if (!_DicRCForResponse.ContainsKey(msgType))
                return null;
            return _DicRCForResponse[msgType];
        }
        #endregion

        #region SDK Function
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, Func<string, string, string, string, string, string, int, int, Senparc.Weixin.QY.AdvancedAPIs.Mass.MassResult>> _DicMediaSendFunc;
        /// <summary>
        /// SDK 发送素材消息方法
        /// </summary>
        /// <returns></returns>
        public Func<string, string, string, string, string, string, int, int, Senparc.Weixin.QY.AdvancedAPIs.Mass.MassResult> GetMediaSendFunc(string msgtype)
        {
            if (_DicMediaSendFunc == null || _DicMediaSendFunc.Count == 0)
            {
                _DicMediaSendFunc = new Dictionary<string, Func<string, string, string, string, string, string, int, int, Senparc.Weixin.QY.AdvancedAPIs.Mass.MassResult>>();

                _DicMediaSendFunc.Add("file", Senparc.Weixin.QY.AdvancedAPIs.Mass.MassApi.SendFile);
                _DicMediaSendFunc.Add("text", Senparc.Weixin.QY.AdvancedAPIs.Mass.MassApi.SendText);
                _DicMediaSendFunc.Add("image", Senparc.Weixin.QY.AdvancedAPIs.Mass.MassApi.SendImage);
                _DicMediaSendFunc.Add("voice", Senparc.Weixin.QY.AdvancedAPIs.Mass.MassApi.SendVoice);
                _DicMediaSendFunc.Add("video", Senparc.Weixin.QY.AdvancedAPIs.Mass.MassApiX.SendVideo);
                _DicMediaSendFunc.Add("mpnews", Senparc.Weixin.QY.AdvancedAPIs.Mass.MassApiX.SendMpNews);
                _DicMediaSendFunc.Add("news", Senparc.Weixin.QY.AdvancedAPIs.Mass.MassApiX.SendNews);
            }

            if (!_DicMediaSendFunc.ContainsKey(msgtype))
                return null;
            return _DicMediaSendFunc[msgtype];
        }
        #endregion
        #endregion
    }

}
