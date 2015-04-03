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
* 目的：ISdkETS
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
    public interface ISdkETS
    {
        #region Base
        /// <summary>
        /// WXType
        /// </summary>
        int WXType { get; }
        /// <summary>
        /// Host
        /// </summary>
        string RHost { get; set; }
        /// <summary>
        /// GetAToken
        /// </summary>
        Func<string> GetAToken { get; }
        #endregion

        #region Media
        Func<string, string, string, int, object> MediaUpload { get; }
        //object GetUploadMediaFileType();
        Func<NewsModelX, object> CNews2SendArticle(string msgtype);
        /// <summary>
        /// 图文消息实体转换委托——NewsModel转换被动回复Article
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Func<NewsModelX, object> CNews2RspArticle(string type);
        Func<object, string, object> GetResConvert(string msgtype);
        #endregion
    }
    /// <summary>
    /// 
    /// </summary>
    public interface ISdkETS_MP : ISdkETS
    {
    }
    /// <summary>
    /// 
    /// </summary>
    public interface ISdkETS_QY : ISdkETS
    {
        /// <summary>
        /// 获取素材转换方法——用于转换被动回复消息
        /// 暂时企业号独占
        /// </summary>
        /// <param name="msgType"></param>
        /// <returns></returns>
        Func<int, string, Senparc.Weixin.QY.Entities.IResponseMessageBase> GetResConvertForResponse(string msgType);
        /// <summary>
        /// SDK 发送素材消息方法
        /// </summary>
        /// <returns></returns>
        Func<string, string, string, string, string, string, int, int, Senparc.Weixin.QY.AdvancedAPIs.Mass.MassResult> GetMediaSendFunc(string msgtype);
    }

}
