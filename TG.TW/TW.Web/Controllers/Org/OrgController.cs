using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TW.Platform.BL;
using TW.Platform.Model;
using TW.Platform.Sys;

namespace TW.Web.Controllers.Org
{
    public class OrgController : ApiControllerBase
    {
        /// <summary>
        /// 获取用户，以部门分组形式展示
        /// </summary>
        /// <returns></returns>
        [HttpGet]
#if Publishes
        [WXOAuth]
#endif
        public HttpResponseMessage GetUsersGroup()
        {
            return this.ExecuteTryCatch(() =>
            {
                var org = new OrgBL();
                var deps = org.GetDepsFromOrgM();
                return deps;
            });
        }
    }
}