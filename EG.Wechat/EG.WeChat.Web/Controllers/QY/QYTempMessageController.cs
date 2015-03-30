using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EG.WeChat.Web.Models;
using EG.WeChat.Platform.BL;
using EG.WeChat.Utility.Tools;

namespace EG.WeChat.Web.Controllers
{
    public class QYTempMessageController : AccessController
    {
        //
        // GET: /QYTempMessage/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateGsMessage(GsMessageVM gsMegVM)
        {
            WeChatMessageService pService = new WeChatMessageService("QY");
            pService.CreateGsMessage(UserID, gsMegVM.MediaId,1,gsMegVM.ToTarget,gsMegVM.Content,gsMegVM.MsgType, 2,gsMegVM.AgentId,gsMegVM.Safe);
            EGExceptionResult pResult = pService.GetActionResult();

            if (pResult != null)
            {
                return Json(pResult);
            }

            //if (gsMegVM.FuncID != 0 && gsMegVM.FuncType != 0)
            //{
            //    gsMegVM.FuncID = 1;
            //    gsMegVM.FuncType = 1;
            //    FuncStatistic funcStatistic = new FuncStatistic();
            //    funcStatistic.AddStatisticList(gsMegVM.ToTarget, gsMegVM.FuncID.ToString(), gsMegVM.FuncType.ToString(), "86", UserID);

            //}
            pResult = new EGExceptionResult(true, "", "");
            return Json(pResult);
        }

    }
}
