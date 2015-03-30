using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EG.WeChat.Platform.BL;
using EG.WeChat.Utility.Tools;
using EG.WeChat.Utility.WeiXin;
using EG.WeChat.Platform.Model;
using EG.WeChat.Web.Models;
/*****************************************************
* 目的：企业号消息Controller
* 创建人：林子聪
* 创建时间：20150325
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Web.Controllers
{
    public class QYMessageController : AccessController
    {
        /// <summary>
        /// 创建企业消息
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View("QYMessageSend");
        }
        /// <summary>
        /// 企业消息审核
        /// </summary>
        /// <returns></returns>
        public ActionResult QYMsReview()
        {
            return View();
        }

        public ActionResult QYFuncMsg()
        {
            return View("QYFuncMsgSend");
        }

        /// <summary>
        /// 发送企业消息
        /// </summary>
        /// <param name="messageid"></param>
        /// <param name="mediaid"></param>
        /// <param name="lcId"></param>
        /// <param name="msgtype"></param>
        /// <param name="toUser"></param>
        /// <param name="toParty"></param>
        /// <param name="toTag"></param>
        /// <param name="agentId"></param>
        /// <returns></returns>
        public ActionResult QYMsSend(string messageid, string keyW, string msgtype, string agentId, string toUser = "@all", string toParty = "@all", string toTag = "@all", int safe = 0)
        {
            var pService = new WeChatMessageService("QY");

            pService.SendQYMessage(messageid, UserID, keyW, msgtype, toUser, toParty, toTag, agentId, safe);
            EGExceptionResult pResult = pService.GetActionResult();
            //
            if (pResult != null)
            {
                return Json(pResult);
            }
            pResult = new EGExceptionResult(true, "", "");
            return Json(pResult);
        }
        /// <summary>
        /// 查询获取所有群发消息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetQYMs()
        {
            WeChatMessageService pService = new WeChatMessageService("QY");
            List<WXGsMessageVM> pList = pService.GetAllGsMessage<WXGsMessageVM>();
            EGExceptionResult pResult = pService.GetActionResult();
            //
            if (pResult != null)
            {
                return new EmptyResult();
            }
            return Json(pList);
        }
        /// <summary>
        /// 查询获取目标条件群发消息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetQYMsByFilter(string filterString)
        {
            WeChatMessageService pService = new WeChatMessageService("QY");
            List<WXGsMessageVM> pList = pService.GetGsMessage<WXGsMessageVM>(filterString);
            EGExceptionResult pResult = pService.GetActionResult();
            //
            if (pResult != null)
            {
                return Json(pResult);
            }
            return Json(pList);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult TestSend()
        {
            var pService = new WeChatMessageService("QY");
            pService.SendQYMessage("", "", "1", "", "", "", "", "");
            EGExceptionResult pResult = pService.GetActionResult();
            //
            if (pResult != null)
            {
                return Json(pResult);
            }
            pResult = new EGExceptionResult(true, "", "");
            return Json(pResult);
        }



    }
}
