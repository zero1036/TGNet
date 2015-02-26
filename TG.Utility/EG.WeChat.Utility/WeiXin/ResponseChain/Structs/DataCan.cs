using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EG.WeChat.Utility.WeiXin.ResponseChain
{
    /* 数据容器类。
     * 
     * 这些容器类，用于ResponseChain内部的数据交换和处理；
     * 然后去到外部，才由调用者转回WeixinSDK的类。
     * 
     * 解耦彼此的关联。
     * 虽然这些数据类大部分属性跟WeixinSDK的类相似，
     * 但是并不属于同个应用概念。
     * 同时允许此模块以下的内容，往后开源用于跟【其它微信的SDK】的接入。
     */

    #region 文章容器类

    /// <summary>
    /// 文章容器类
    /// </summary>
    public class ArticleCan : IDataCan
    {
        public ArticleCan(string title, string description, string picUrl, string url)
        {
            this.Title          = title;
            this.Description    = description;
            this.PicUrl         = picUrl;
            this.Url            = url;
        }

        /// <summary>
        /// 描述(出现在图片下方，灰色字体；如果有多个文章，则此内容会消失[微信这样处理])
        /// </summary>
        public readonly string Description;
        /// <summary>
        /// 图片Url
        /// </summary>
        public readonly string PicUrl;
        /// <summary>
        /// 文章的标题
        /// </summary>
        public readonly string Title;
        /// <summary>
        /// 点击之后，微信跳转的URL
        /// </summary>
        public readonly string Url;
    }

    #endregion

    #region 图片类容器

    /// <summary>
    /// 图片类容器
    /// </summary>
    public class ImageCan : IDataCan
    {
        public ImageCan(string picUrl)
        {
            this.PicUrl = picUrl;
        }
        public ImageCan(string picUrl, string mediaID)
        {
            this.PicUrl     = picUrl;
            this.MediaID    = mediaID;
        }

        /// <summary>
        /// 资源ID
        /// </summary>
        public readonly string MediaID;
        /// <summary>
        /// 图片Url
        /// </summary>
        public readonly string PicUrl;
    }

    #endregion

    #region 语音类容器

    /// <summary>
    /// 语音类容器
    /// </summary>
    public class VoiceCan : IDataCan
    {
        public VoiceCan(string mediaID)
        {
            this.MediaID = mediaID;
        }
        public VoiceCan(string format, string recognition, string mediaID)
        {
            this.MediaID    = mediaID;
            this.Format     = format;
            this.Recognition= recognition;
        }

        /// <summary>
        /// 资源ID
        /// </summary>
        public readonly string MediaID;
        /// <summary>
        /// 音频格式的名称
        /// </summary>
        public string Format;
        /// <summary>
        /// 语音识别结果
        /// </summary>
        public string Recognition;
    }

    #endregion

    #region 视频类容器

    /// <summary>
    /// 视频类容器
    /// </summary>
    public class VideoCan : IDataCan
    {
        public VideoCan(string thumbMediaId,string mediaID)
        {
            this.MediaID        = mediaID;
            this.ThumbMediaId   = thumbMediaId;
        }

        /// <summary>
        /// 预览/缩略图 ID
        /// </summary>
        public string ThumbMediaId;
        /// <summary>
        /// 资源ID
        /// </summary>
        public readonly string MediaID;
    }

    #endregion    

    //public class LocationCan: IDataCan

}