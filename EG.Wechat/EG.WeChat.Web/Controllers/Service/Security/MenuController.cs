using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EG.WeChat.Model;
using EG.WeChat.Platform.BL;
using EG.WeChat.Web.Models.Security;

namespace EG.WeChat.Web.Controllers
{
    [ActionAttribute(level = Level.TwoLevel)]
    public class MenuController : BaseController
    {
        private MenuBL _menuBL;
        protected MenuBL MenuBL
        {
            get
            {
                if (_menuBL == null)
                {
                    _menuBL = TransactionProxy.New<MenuBL>();
                }
                return _menuBL;
            }
        }


        [HttpPost]
        public ActionResult GetMenu()
        {
            List<MenuVM> model = new List<MenuVM>();

            foreach (var item in MenuBL.GetMenu(AccessRightList))
            {
                model.Add(new MenuVM() { TemData = item });
            }

            return Json(model);
        }

    }
}
