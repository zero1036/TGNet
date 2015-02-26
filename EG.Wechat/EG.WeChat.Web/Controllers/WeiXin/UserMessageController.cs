using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EG.WeChat.Platform.BL;
using EG.WeChat.Web.Common;
using EG.WeChat.Web.Models;

namespace EG.WeChat.Web.Controllers.WeiXin
{
    public class UserMessageController : Controller
    {

        private MediaBL _mediaBL;
        protected MediaBL MediaBL
        {
            get
            {
                if (_mediaBL == null)
                {
                    _mediaBL = TransactionProxy.New<MediaBL>();
                }
                return _mediaBL;
            }
        }

        private TextBL _textBL;
        protected TextBL TextBL
        {
            get
            {
                if (_textBL == null)
                {
                    _textBL = TransactionProxy.New<TextBL>();
                }
                return _textBL;
            }
        }


        public ActionResult List()
        {
            /*TypeList是根據SDK的 enum UploadMediaFileType 但是SDK跟微信接口也有出入，
             *並且加入自己的類型，因此不直接使用UploadMediaFileType */
            ViewBag.TypeList = new SelectListItem[]{
                        new SelectListItem(){ Text = "文本" ,Value = "-1"},
                        new SelectListItem(){ Text = "圖片",Value = "0"},
                        new SelectListItem(){ Text = "語音",Value = "1"},
                        new SelectListItem(){ Text = "視頻",Value = "2"},
                        //new SelectListItem(){ Text = "thumb",Value = "3"}
                        };

            return View();
        }

        [HttpPost]
        public ActionResult List(string OpenID, string Date, int Type, int pageIndex)
        {
            DateTime? dTime = null;
            DateTime parsedTime ;
            if (DateTime.TryParse(Date, out parsedTime))
            {
                dTime = parsedTime.Date;
            };

            EG.WeChat.Model. PagingM modle;
            if (Type == -1)
            {
                modle = TextBL.List(OpenID, dTime, pageIndex);
            }
            else {
                modle = MediaBL.List(OpenID, dTime, Type, pageIndex);
            }

            PagingVM result = new PagingVM();
            result.PageIndex = modle.PageIndex;
            result.PageSize = modle.PageSize;
            result.TotalCount = modle.TotalCount;
            result.TotalPages = modle.TotalPages;
            result.JsonData = DataConvert.DataTableToJson(modle.DataTable);

            return Json(result);
        }

        [HttpPost]
        public ActionResult SaveMedia(string openId, string MediaID, Senparc.Weixin.MP.UploadMediaFileType type)
        {
            /* 思路备忘：
             * 对于“图片”、“语音”、“视频”这些【由微信服务器存储，我们只能通过MediaID交互】的资源，
             * 1.数据库将会从WC_Media表，将资源下载，然后记录到WCR_Media_Resource表；
             * 2.将资源下载后，由我们后台的“资源管理”模块接管。
             */

            ActionResult result = null;

            try
            {
                var ret = MediaBL.SaveMediaResource(openId, MediaID, type);
                result = Json(new { IsSuccess = ret.IsSuccess, Message = ret.Message });
            }
            catch (Exception ex)
            {
                result = Json(new {IsSuccess = false,Message = ex.Message});
            }

            return result;
        }


    }
}
