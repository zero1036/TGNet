using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net;
using System.Net.Http;

namespace TW.Web.Controllers
{
    public class HomeController : ApiControllerBase
    {
        [HttpGet]
        public HttpResponseMessage Index()
        {
            return Request.CreateResponse(HttpStatusCode.OK, new { Name = "tg" });
        }
    }
}
