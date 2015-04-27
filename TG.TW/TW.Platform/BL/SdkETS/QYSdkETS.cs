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
using TW.Platform.Model;
/*****************************************************
* 目的：QYSdkETS
* 创建人：林子聪
* 创建时间：20150129
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace TW.Platform.BL
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
        /// RHost
        /// </summary>
        public string RHost { get; set; }
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
                    PicUrl = string.Format("http://{0}{1}", RHost, z.RPath)
                });
                _DicNewsConvertToArticleFunc.Add("mpnews", (z) => new Senparc.Weixin.QY.Entities.Article
                {
                    Title = z.title,
                    Description = z.digest,
                    Url = z.content_source_url,
                    PicUrl = string.Format("http://{0}{1}", RHost, z.RPath)
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


                _DicRCForResponse.Add("text", (int lcId, string mType) =>
                {
                    var responseMessage = new Senparc.Weixin.QY.Entities.ResponseMessageText();
                    responseMessage.Content = "测试";
                    return responseMessage;
                });
                _DicRCForResponse.Add("image", (int lcId, string mType) =>
                {
                    var pAr = new WXPictureBL("QY");
                    var pImg = pAr.LoadResourcesSingleBylcId(lcId);

                    var responseMessage = new Senparc.Weixin.QY.Entities.ResponseMessageImage();
                    if (pImg != null) responseMessage.Image.MediaId = pImg.media_id;
                    return responseMessage;
                });
                _DicRCForResponse.Add("voice", (int lcId, string mType) =>
                {
                    var pAr = new WXVoiceBL("QY");
                    var pVoice = pAr.LoadResourcesSingleBylcId(lcId);

                    var responseMessage = new Senparc.Weixin.QY.Entities.ResponseMessageVoice();
                    if (pVoice != null) responseMessage.Voice.MediaId = pVoice.media_id;
                    return responseMessage;
                });
                _DicRCForResponse.Add("video", (int lcId, string mType) =>
                {
                    var pAr = new WXVideoBL("QY");
                    var pVideo = pAr.LoadResourcesSingleBylcId(lcId);

                    var responseMessage = new Senparc.Weixin.QY.Entities.ResponseMessageVideo();
                    if (pVideo != null)
                    {
                        responseMessage.Video.Title = pVideo.lcName;
                        responseMessage.Video.MediaId = pVideo.media_id;
                    }
                    return responseMessage;
                });
                _DicRCForResponse.Add("news", (int lcId, string mType) =>
                {
                    var pAr = new WXArticleBL("QY");
                    pAr.ArticleConvertFunc = this.CNews2RspArticle(msgType);
                    List<object> pos = pAr.LoadResources2News(lcId, mType);
                    if (pos == null || pos.Count == 0)
                        return null;

                    var responseMessage = new Senparc.Weixin.QY.Entities.ResponseMessageNews();
                    responseMessage.Articles = pos.Select(p => (p as Senparc.Weixin.QY.Entities.Article)).ToList();
                    return responseMessage;
                });
                _DicRCForResponse.Add("mpnews", (int lcId, string mType) =>
                {
                    var pAr = new WXArticleBL("QY");
                    pAr.ArticleConvertFunc = this.CNews2RspArticle(msgType);
                    List<object> pos = pAr.LoadResources2News(lcId, mType);

                    var responseMessage = new Senparc.Weixin.QY.Entities.ResponseMessageNews();
                    responseMessage.Articles = pos.Select(p => (p as Senparc.Weixin.QY.Entities.Article)).ToList();
                    int idx = 0;
                    foreach (var a in responseMessage.Articles)
                    {
                        a.Url = string.Format("http://{0}/WXArticle/Index?lcid={1}&idx={2}", RHost, lcId, idx);
                        idx += 1;
                    }
                    return responseMessage;
                });
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
