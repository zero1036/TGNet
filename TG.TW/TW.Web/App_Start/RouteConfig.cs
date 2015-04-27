using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TW.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            //忽略
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //Controller默认指向Index的Action
            routes.MapRoute(
                name: "ControllerDefault",
                url: "{controller}/{action}/{id}",
                defaults: new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}