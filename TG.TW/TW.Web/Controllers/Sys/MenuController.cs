using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TW.Platform.Model;
using TW.Platform.Sys;

namespace TW.Web.Controllers.Sys
{
    public class MenuController : ApiControllerBase
    {
        [HttpGet]
        public HttpResponseMessage GetMenu()
        {
            return this.ExecuteTryCatch(() =>
            {
                MenuVM pvm = new MenuVM();
                pvm.TemData = new MenuTM()
                {
                    Code = "M100",
                    Type = 1,
                    Name = "测试菜单",
                    FatherCode = string.Empty,
                    Sort = 1,
                    State = 0,
                    Description = "detail",
                    Href = "/orderCycleList"
                };
                List<MenuVM> pli = new List<MenuVM>();
                pli.Add(pvm);
                return pli;
            });
        }
    }
}