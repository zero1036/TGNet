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


        public ActionResult Tb(string path)
        {
            var request = this.HttpContext.Request;
            //当前请求Url
            string currentRequestUrl = string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Host, request.Url.PathAndQuery);
            //验证请求是否带SSOTICKET，没有则重定向到SSO登录地址
            string ssoTicket = request.QueryString["SSOTICKET"];


            Uri url = request.Url;
            //Uri pb = new Uri("name=1&sxy=2");
            Uri resu;
            Uri.TryCreate(url, path, out resu);
            return Content(resu.ToString());
        }
    }
}