using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EG.WeChat.Platform.DA;
using EG.WeChat.Platform.BL;
/*****************************************************
* 目的：微信群发消息前端页面绑定模型
* 创建人：林子聪
* 创建时间：20141118
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Platform.Model
{
    public class WXGsMessageVM : EG.WeChat.Platform.DA.WXGsMessageDA
    {
        private object _ResultJson;
        /// <summary>
        /// 修改时间
        /// </summary>
        public string MTimeX
        {
            get
            {
                return base.MTime.ToString();
            }
        }
        /// <summary>
        /// 发送时间
        /// </summary>
        public string STimeX
        {
            get
            {
                return base.STime.ToString();
            }
        }
        /// <summary>
        /// 内容类型
        /// </summary>
        public string ContentTypeX
        {
            get
            {
                string strBase = base.ContentType.ToString();
                if (strBase == "text")
                {
                    return "文本";
                }
                else if (strBase == "image")
                {
                    return "圖片";
                }
                else if (strBase == "voice")
                {
                    return "音頻";
                }
                else if (strBase == "video")
                {
                    return "視頻";
                }
                else if (strBase == "mpnews")
                {
                    return "圖文";
                }
                return string.Empty;
            }
        }
        /// <summary>
        /// 发送状态
        /// </summary>
        public string SStateX
        {
            get
            {
                int strBase = base.SState;
                if (strBase == 1)
                {
                    return "待審核";
                }
                else if (strBase == 2)
                {
                    return "審核通過";
                }
                else if (strBase == 3)
                {
                    return "已發送";
                }
                else if (strBase == 4)
                {
                    return "審核未通過";
                }
                return string.Empty;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public object ResultJson
        {
            get
            {
                if (_ResultJson != null)
                    return _ResultJson;
                if (base.ContentType == "text")
                {
                    _ResultJson = base.SContent;
                }
                else if (base.ContentType == "image")
                {
                    WeChatPictureService pSer = new WeChatPictureService();
                    _ResultJson = pSer.LoadResourcesSingle(base.SContent);
                }
                else if (base.ContentType == "voice")
                {
                    WeChatVoiceService pSer = new WeChatVoiceService();
                    _ResultJson = pSer.LoadResourcesSingle(base.SContent);
                }
                else if (base.ContentType == "video")
                {
                    WeChatVideoService pSer = new WeChatVideoService();
                    _ResultJson = pSer.LoadResourcesSingle(base.SContent);
                }
                else if (base.ContentType == "mpnews")
                {
                    WeChatArticleService pSer = new WeChatArticleService();
                    _ResultJson = pSer.LoadResourcesSingle(base.SContent);
                }
                return _ResultJson;
            }
        }
    }
}
