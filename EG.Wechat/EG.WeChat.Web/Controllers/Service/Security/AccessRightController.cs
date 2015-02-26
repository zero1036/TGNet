using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EG.WeChat.Model;
using EG.WeChat.Platform.BL;
using EG.WeChat.Web.Models.Security;

namespace EG.WeChat.Web.Controllers.Service.Security
{
    public class AccessRightController :  BaseController
    {

        private AccessRightBL _accessRightBL;
        protected AccessRightBL AccessRightBL
        {
            get
            {
                if (_accessRightBL == null)
                {
                    _accessRightBL = TransactionProxy.New<AccessRightBL>();
                }
                return _accessRightBL;
            }
        }


        public ActionResult List(int GroupID)
        {
            var model = AccessRightBL.List(GroupID);
            return View(model);
        }



        [HttpPost]
        public ActionResult SaveList(int GroupID, string Data)
        {
            ResultM result = new ResultM();
            try
            {
                result = AccessRightBL.Save(GroupID, Data);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return Json(result);
        }




    }
}
