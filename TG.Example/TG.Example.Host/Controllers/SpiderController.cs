using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TG.Example.Host.Controllers
{
    public class SpiderController : Controller
    {
        //
        // GET: /Spider/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ContentResult Phone()
        {
            return Content("123");
        }

        public ContentResult Address()
        {
            return Content("Address");
        }

	}
}