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
    public class OrgController : ApiControllerBase
    {
        /// <summary>
        /// 获取部门
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        //#if Publishes
        //        [WXOAuth]
        //#endif
        public HttpResponseMessage GetDeps()
        {
            return this.ExecuteTryCatch(() =>
            {
                var org = new OrgBL();
                var deps = org.GetDepsFromOrgM();
                return deps;
            });
        }
        /// <summary>
        /// 获取用户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        //#if Publishes
        //        [WXOAuth]
        //#endif
        public HttpResponseMessage GetUsers(int did)
        {
            return this.ExecuteTryCatch(() =>
            {
                var org = new OrgBL();
                var deps = org.GetUsersByDepId(did);
                return deps;
            });
        }
        /// <summary>
        /// 获取用户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //#if Publishes
        //        [WXOAuth]
        //#endif
        public HttpResponseMessage GetUsersX(int did)
        {
            return this.ExecuteTryCatch(() =>
            {
                var org = new OrgBL();
                var deps = org.GetUsersByDepId(did);
                return deps;
            });
        }
    }

    public class OrgXController : ApiController
    {
        /// <summary>
        /// 获取部门
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        //#if Publishes
        //        [WXOAuth]
        //#endif
        public HttpResponseMessage GetDeps()
        {
            var Departments = new List<DepartmentVM>();

            Departments.Add(new DepartmentVM()
            {
                SysDepartmentId = 1,
                Did = 1,
                Name = "OK",
                Level = 1
            });

            //pActionResult = new CActionResult() { ok = true, message = "", data = vm };
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, message = "", data = Departments }); 
        }
    }
}