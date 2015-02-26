using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EG.WeChat.Service.WeiXin;
using Senparc.Weixin.MP.Entities;
/*****************************************************
* 目的：发送客服消息中间服务
* 创建人：林子聪
* 创建时间：20150123
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Platform.BL
{
    public class WXCSMessageService : IServiceX
    {
        /// <summary>
        /// 发送客服消息
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="strType"></param>
        /// <param name="pHost"></param>
        /// <param name="lcId"></param>
        /// <param name="mediaId"></param>
        /// <param name="pTextContent"></param>
        public EG.WeChat.Utility.Tools.EGExceptionResult SendCSMessage(string openId, string strType, string pHost, int? lcId, string mediaId, string pTextContent)
        {
            this.ExecuteTryCatch(() =>
            {
                if (strType == "text")
                {
                    SendCSMessageText(openId, pTextContent);
                }
                else if (strType == "image")
                {
                    SendCSMessageImage(openId, mediaId);
                }
                else if (strType == "voice")
                {
                    SendCSMessageVoice(openId, mediaId);
                }
                else if (strType == "video")
                {
                    SendCSMessageVideo(openId, mediaId);
                }
                else if (strType == "mpnews")
                {
                    SendCSMessageMpnews(openId, pHost, lcId);
                }
            });
            //加載異常信息
            EG.WeChat.Utility.Tools.EGExceptionResult pResult = this.GetActionResult();
            return pResult;
            //if (pResult != null)
            //{
            //    //if (pResult.ExCode == "45015")
            //    //{
            //    return "當前微信用戶不在對話狀態，無法發送客服消息！";
            //    //}
            //    //else
            //    //{
            //    //    return pResult.Message;
            //    //}
            //}
            //return string.Empty;
        }
        /// <summary>
        /// 发送文本消息
        /// </summary>
        private void SendCSMessageText(string openId, string pText)
        {
            WeixinMessageSender.SendText(openId, pText);
        }
        /// <summary>
        /// 发送图片消息
        /// </summary>
        private void SendCSMessageImage(string openId, string mediaId)
        {
            WeixinMessageSender.SendImage(openId, mediaId);
        }
        /// <summary>
        /// 发送音频消息
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="mediaId"></param>
        private void SendCSMessageVoice(string openId, string mediaId)
        {
            WeixinMessageSender.SendVoice(openId, mediaId);

        }
        /// <summary>
        /// 发送视频消息
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="mediaId"></param>
        private void SendCSMessageVideo(string openId, string mediaId)
        {
            WeixinMessageSender.SendVideo(openId, mediaId, string.Empty, string.Empty);
        }
        /// <summary>
        /// 发送图文消息
        /// </summary>
        private void SendCSMessageMpnews(string openId, string pHost, int? lcId)
        {
            if (lcId == null)
                return;

            WeChatArticleService pService = new WeChatArticleService();
            List<Article> pArticles = pService.LoadResources2LocalArticles(pHost, lcId.Value);
            var pResult = pService.GetActionResult();
            if (pResult != null || pArticles == null || pArticles.Count == 0)
                return;

            EG.WeChat.Service.WeiXin.WeixinMessageSender pSender = new WeixinMessageSender();
            pSender.SendArticle(openId, pArticles);
        }
    }
}
