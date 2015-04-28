using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using System.Web.Caching;

namespace TW.Web.Controllers
{
    public class HomeController : ApiControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage Index()
        {
            HttpRuntime.Cache.Insert("name", "tg");
            //HttpContext.Current.Cache.Add("ni", 123,CacheDependency.);
            return Request.CreateResponse(HttpStatusCode.OK, new { Name = "tg" });
        }


    }

    public class dbController : ApiController
    {
         //[AllowAnonymous]
        [WXOAuth]
        [HttpGet]
        public HttpResponseMessage db(string code ,string agentid)
        {
               return Request.CreateResponse(HttpStatusCode.OK, new { Name = HttpRuntime.Cache.Get("name") });
        }
    }
}
