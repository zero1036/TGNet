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
    public class UserController : AccessController
    {

        private UserBL _userBL;
        protected UserBL UserBL
        {
            get
            {
                if (_userBL == null)
                {
                    _userBL = TransactionProxy.New<UserBL>();
                }
                return _userBL;
            }
        }


        #region 用户

        public ActionResult List()
        {
            return View();
        }


        [HttpPost]
        public ActionResult List(string userID, string userName, int pageIndex)
        {
            var  modle = UserBL.List(userID, userName, pageIndex);

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
            return View(new UserVM());
        }


        [HttpPost]
        public ActionResult New(UserVM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ResultM result = UserBL.New(model.TemData, UserID);

                    if (result.IsSuccess )
                    {
                        return RedirectToAction("List", "User");
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


        public ActionResult Edit(string UserID)
        {
            var model = new UserVM() { TemData = UserBL.GetUser(UserID) };
            return View(model);
        }


        [HttpPost]
        public ActionResult Edit(UserVM model)
        {
            if (string.IsNullOrEmpty(model.Password) && string.IsNullOrEmpty(model.ConfirmPassword))
            {
                ModelState.Remove("Password");
                ModelState.Remove("ConfirmPassword");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    ResultM result = UserBL.Edit(model.TemData,UserID);

                    if (result.IsSuccess)
                    {
                        return RedirectToAction("List", "User");
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


        public ActionResult Detail(string UserID)
        {
            var model = new UserVM() { TemData = UserBL.GetUser(UserID) };
            return View(model);
        }


        [HttpPost]
        public ActionResult Delete(string userID)
        {
            ResultM result = UserBL.Delete(userID, UserID);
            return Json(result);
        }


        #endregion


        #region 用戶組


        public ActionResult UserGroup(int GroupID)
        {
            ViewData["GroupID"] = GroupID;
            return View();
        }


        [HttpPost]
        public ActionResult AddUserGroup(int GroupID, string userUIDs)
        {
            ResultM result = new ResultM();
            try
            {
                result = UserBL.AddUserGroup(GroupID, userUIDs);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return Json(result);
        }


        [HttpPost]
        public ActionResult DelUserGroup(int GroupID, string userUIDs)
        {
            ResultM result = new ResultM();
            try
            {
                result = UserBL.DelUserGroup(GroupID, userUIDs);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return Json(result);
        }


        /// <summary>
        /// 获取已选用户
        /// </summary>
        /// <param name="GroupId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetSelectedUser(int GroupID,string SelectedUser)
        {
            ResultM<T_User> result = new ResultM<T_User>();
            try
            {
                result.EntityList = UserBL.GetSelectedUser(GroupID, SelectedUser);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
            }
            return Json(result);
        }


        /// <summary>
        /// 获取可选用户
        /// </summary>
        /// <param name="GroupId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetSelectUser(int GroupID, string SelectUser)
        {
            ResultM<T_User> result = new ResultM<T_User>();
            try
            {
                result.EntityList = UserBL.GetSelectUser(GroupID, SelectUser);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
            }
            return Json(result);
        }


        #endregion


    }
}
