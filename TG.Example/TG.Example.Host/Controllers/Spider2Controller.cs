using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TG.Example.Host.Controllers
{
    public class Spider2Controller : Controller
    {
        //
        // GET: /Spider2/
        public ActionResult Index()
        {
            return Content("Index2");
        }


        public ActionResult Me()
        {
            return View();
        }
	}
}