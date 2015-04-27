using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TW.Platform.DA;
using TW.Platform.BL;
/*****************************************************
* 目的：微信群发消息前端页面绑定模型
* 创建人：林子聪
* 创建时间：20141118
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace TW.Platform.Model
{
    public class WXGsMessageVM : TW.Platform.DA.WXGsMessageDA
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
                else if (strBase == "news")
                {
                    return "链接圖文";
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
        public object SQYApp
        {
            get
            {
                int agentid = base.agentid;
                if (agentid < 1)
                    return null;
                EG.WeChat.Utility.WeiXin.IWXCorpInfo pc = EG.WeChat.Utility.WeiXin.WeiXinConfiguration.corpInfos.Single(p => p.aid == agentid);
                return new
                {
                    round_logo_url = pc.round_logo_url,
                    name = pc.aname,
                    aid = pc.aid
                };
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public object STargetQY
        {
            get
            {
                string qyuser = base.STarget;
                object oj = EG.WeChat.Utility.Tools.CommonFunction.Json_DeserializeObject(qyuser);
                if (oj is Dictionary<string, object>)
                {
                    var dic = (Dictionary<string, object>)(oj);
                    var op = new { touser = dic["touser"], toparty = dic["toparty"], totag = dic["totag"] };
                    return op;
                }
                else
                {
                    return null;
                }
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
                    WXPictureBL pSer = new WXPictureBL("QY");
                    _ResultJson = pSer.LoadResourcesSingle(base.SContent);
                }
                else if (base.ContentType == "voice")
                {
                    WXVoiceBL pSer = new WXVoiceBL("QY");
                    _ResultJson = pSer.LoadResourcesSingle(base.SContent);
                }
                else if (base.ContentType == "video")
                {
                    WXVideoBL pSer = new WXVideoBL("QY");
                    _ResultJson = pSer.LoadResourcesSingle(base.SContent);
                }
                else if (base.ContentType == "news")
                {
                    WXArticleBL pSer = new WXArticleBL("QY", "news");
                    var lcid = Convert.ToInt32(base.SContent);
                    _ResultJson = pSer.LoadResourcesSingleBylcID(lcid);
                }
                else if (base.ContentType == "mpnews")
                {
                    WXArticleBL pSer = new WXArticleBL("QY", "mpnews");
                    var lcid = Convert.ToInt32(base.SContent);
                    _ResultJson = pSer.LoadResourcesSingleBylcID(lcid);
                }
                return _ResultJson;
            }
        }
    }
}
