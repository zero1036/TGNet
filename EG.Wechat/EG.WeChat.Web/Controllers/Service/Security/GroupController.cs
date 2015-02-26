using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EG.WeChat.Model;
using EG.WeChat.Platform.BL;
using EG.WeChat.Web.Common;
using EG.WeChat.Web.Models;

namespace EG.WeChat.Web.Controllers
{
    public class GroupController : AccessController
    {

        private GroupBL _groupBL;
        protected GroupBL GroupBL
        {
            get
            {
                if (_groupBL == null)
                {
                    _groupBL = TransactionProxy.New<GroupBL>();
                }
                return _groupBL;
            }
        }

        #region 組

        public ActionResult List()
        {
            return View();
        }


        [HttpPost]
        public ActionResult List(string GroupID, string GroupName, int pageIndex)
        {
            var modle = GroupBL.List(GroupID, GroupName, pageIndex);

            PagingVM result = new PagingVM();
            result.PageIndex = modle.PageIndex;
            result.PageSize = modle.PageSize;
            result.TotalCount = modle.TotalCount;
            result.TotalPages = modle.TotalPages;
            result.JsonData = DataConvert.DataTableToJson(modle.DataTable);

            return Json(result);
        }


        public ActionResult New()
        {
            return View(new GroupVM());
        }


        [HttpPost]
        public ActionResult New(GroupVM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ResultM result = GroupBL.New(model.TemData, UserID);

                    if (result.IsSuccess)
                    {
                        return RedirectToAction("List", "Group");
                    }
                    else
                    {
                        ModelState.AddModelError("ErrorMessage", result.Message);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("ErrorMessage", ex.Message);
                }
            }
            return View(model);
        }


        public ActionResult Edit(int GroupID)
        {
            var model = new GroupVM() { TemData = GroupBL.GetGroup(GroupID) };
            return View(model);
        }


        [HttpPost]
        public ActionResult Edit(GroupVM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ResultM result = GroupBL.Edit(model.TemData, UserID);

                    if (result.IsSuccess)
                    {
                        return RedirectToAction("List", "Group");
                    }
                    else
                    {
                        ModelState.AddModelError("ErrorMessage", result.Message);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("ErrorMessage", ex.Message);
                }
            }
            return View(model);
        }


        public ActionResult Detail(int GroupID)
        {
            var model = new GroupVM() { TemData = GroupBL.GetGroup(GroupID) };
            return View(model);
        }


        [HttpPost]
        public ActionResult Delete(int GroupID)
        {
            ResultM result = GroupBL.Delete(GroupID, UserID);
            return Json(result);
        }


        #endregion


        #region 用户組


 



        #endregion



    }
}
