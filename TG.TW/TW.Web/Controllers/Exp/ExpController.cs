using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TW.Platform.BL;
using TW.Platform.Model;
using TW.Platform.Sys;

namespace TW.Web.Controllers
{
    public class ExpController : ApiControllerBase
    {
        /// <summary>
        /// 获取货品
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        //#if Publishes
        //        [WXOAuth]
        //#endif
        public HttpResponseMessage GetModels()
        {
            return this.ExecuteTryCatch(() =>
            {
                var org = new OrgBL();
                var deps = org.GetDepsFromOrgM();
                return deps;
            });
        }

    }

    //public class ExpHandler
    //{
    //    public List<ModelM> Models
    //    {

    //    }
    //}

    //public class ModelM{

    //}
}