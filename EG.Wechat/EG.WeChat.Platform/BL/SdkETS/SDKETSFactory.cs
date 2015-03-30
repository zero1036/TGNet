using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
* 目的：SDKETSFactory
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
    public static class SDKETSFactory
    {
        /// <summary>
        /// 获取单例
        /// </summary>
        /// <param name="sdkType"></param>
        /// <returns></returns>
        public static ISdkETS GetSDK(string sdkType)
        {
            if (sdkType == "MP")
            {
                return MPSdkETS.Singleon;
            }
            else
            {
                return QYSdkETS.Singleon;
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class CommonSdkETS
    {
        /// <summary>
        /// 
        /// </summary>
        protected Dictionary<string, Func<NewsModelX, object>> _DicNewsModel;
        protected Dictionary<string, Func<NewsModelX, object>> GetDicNewsModel()
        {
            if (_DicNewsModel == null || _DicNewsModel.Count == 0)
            {
                _DicNewsModel = new Dictionary<string, Func<NewsModelX, object>>();
                _DicNewsModel.Add("news", (z) => new
                {
                    title = z.title,
                    description = z.digest,
                    url = z.content_source_url,
                    picurl = z.RPath
                });
                _DicNewsModel.Add("mpnews", (z) => new
                {
                    title = z.title,
                    thumb_media_id = z.thumb_media_id,
                    author = z.author,
                    content_source_url = z.content_source_url,
                    content = z.content,
                    digest = z.digest,
                    show_cover_pic = z.show_cover_pic
                });
            }
            return _DicNewsModel;
        }
        /// <summary>
        /// lcid,media_id,content,msgtype
        /// </summary>
        protected Dictionary<string, Func<object, string, object>> _DicResourceConvert;
        protected Func<object, string, object> GetDicResourceConvert(int wxtype, string msgType, Func<NewsModelX, object> pFunc)
        {
            if (_DicResourceConvert == null || _DicResourceConvert.Count == 0)
            {
                var ps = wxtype == 1 ? "MP" : "QY";

                var pArticleSer = new WeChatArticleService(ps);
                pArticleSer.ArticleConvertFunc = pFunc;

                _DicResourceConvert = new Dictionary<string, Func<object, string, object>>();
                _DicResourceConvert.Add("text", (object content, string mType) => { return content; });
                _DicResourceConvert.Add("image", (object media_id, string mType) => { return media_id; });
                _DicResourceConvert.Add("voice", (object media_id, string mType) => { return media_id; });
                _DicResourceConvert.Add("video", (object media_id, string mType) => { return media_id; });
                _DicResourceConvert.Add("file", (object media_id, string mType) => { return media_id; });
                _DicResourceConvert.Add("news", pArticleSer.LoadResources2News);
                _DicResourceConvert.Add("mpnews", pArticleSer.LoadResources2News);
                //pdicContent.Add("mpnews", .LoadResources2Mpnews);
            }

            if (!_DicResourceConvert.ContainsKey(msgType))
                return null;
            return _DicResourceConvert[msgType];
        }
    }
}
