using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EG.WeChat.Model;
using EG.WeChat.Platform.BL;
using EG.WeChat.Web.Models;

namespace EG.WeChat.Web.Controllers
{
    public class HomeController : BaseController
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


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Introduce()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View(new LoginVM());
        }


        [HttpPost]
        public ActionResult Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    LoginInfo rm = UserBL.Login(model.TemData);

                    if (rm != null)
                    {

                        UserID = rm.UserID;
                        UserName = rm.UserName;
                        AccessRightList = rm.AccessRight;

                        if (!StaticLibrary.ActiveSession.ContainsKey(HttpSession.SessionID))
                        {
                            StaticLibrary.ActiveSession.Add(HttpSession.SessionID, HttpSession);
                        }
                        var pMode = model.BMode ? 4 : 2;
                        return RedirectToAction("Index", "Home", new { id = pMode });
                        //return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("ErrorMessage", "User or password is not correct!");
                    }
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("ErrorMessage", e.Message);
                }
            }
            return View(model);
        }


        public ActionResult Logout()
        {
            HttpSession.RemoveAll();
            HttpSession.Abandon();
            return RedirectToAction("Login");
        }


        public ActionResult LoginTimeout()
        {
            ViewBag.Message = "抱歉！登录超时，请重新登录！";
            return View("../Shared/Message");
        }


        public ActionResult NoPermission()
        {
            ViewBag.Message = "抱歉！您没有该模块权限，请与管理员联系！";
            return View("../Shared/Message");
        }


        public ActionResult Sorry()
        {
            ViewBag.Message = "抱歉！系统出现故障，请稍后再试！";
            return View("../Shared/Message");
        }






    }
}
