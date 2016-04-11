using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TG.Example;

namespace TG.Example.Host.Controllers
{
    public class HomeController : Controller
    {
        private UserService _service;
        public HomeController()
        {
            _service = new UserService();
        }
        //
        // GET: /Home/
        public ActionResult Index()
        {
            //var res = this._service.InsertOne();
            return Content("test");
        }

        public ActionResult Set()
        {
            var res = this._service.InsertOne();
            return Content(res.ToString());
        }

        public ActionResult Get()
        {
            Random rand = new Random();
            int tid = rand.Next(1, 20);
            User pu = this._service.GetUser(tid);
            return Content(pu != null ? "3" : "4");
        }
    }
}