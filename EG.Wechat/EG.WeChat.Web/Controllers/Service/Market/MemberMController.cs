using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EG.WeChat.Platform.BL;
using EG.WeChat.Web.Common;
using EG.WeChat.Web.Models;

namespace EG.WeChat.Web.Controllers
{
    public class MemberMController : AccessController
    {
        private MemberBL _memberBL;
        protected MemberBL MemberBL
        {
            get
            {
                if (_memberBL == null)
                {
                    _memberBL = TransactionProxy.New<MemberBL>();
                }
                return _memberBL;
            }
        }


        public ActionResult List()
        {
            return View();
        }

        [HttpPost]
        public ActionResult List(string Name, int pageIndex)
        {
            var modle = MemberBL.List(Name, pageIndex);

            PagingVM result = new PagingVM();
            result.PageIndex = modle.PageIndex;
            result.PageSize = modle.PageSize;
            result.TotalCount = modle.TotalCount;
            result.TotalPages = modle.TotalPages;
            result.JsonData = DataConvert.DataTableToJson(modle.DataTable);

            return Json(result);
        }

    }
}
