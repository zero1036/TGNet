using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using System.Web.Caching;
using TW.Platform.BL;

namespace TW.Web.Controllers
{
    public class abController : ApiController
    {
        [WXOAuth]
        [HttpGet]
        public HttpResponseMessage db()
        {
          
            //var userBL = new UserBL();
            string pUserID = TW.Platform.Sys.SysCurUser.GetCurUserID();
            return Request.CreateResponse(HttpStatusCode.OK, new { Name = pUserID });
        }

    }
}
