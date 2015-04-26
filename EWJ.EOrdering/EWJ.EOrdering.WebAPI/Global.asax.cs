using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using System.Web.SessionState;

namespace EWJ.EOrdering.WebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        public override void Init()
        {
            this.PostAuthenticateRequest += (sender, e) => HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
            base.Init();

            ////缓存设置
            //this.PreRequestHandlerExecute -= WebApiApplication_PreRequestHandlerExecute;
            //this.PreRequestHandlerExecute += WebApiApplication_PreRequestHandlerExecute;
        }

        void WebApiApplication_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            ///* 2014-12-24 Team:不使用此逻辑。由具体的Controller去标记Action。 */
            if (HttpContext.Current != null &&
                HttpContext.Current.Response != null &&
                HttpContext.Current.Response.Cache != null)
            {
                HttpContext.Current.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
                HttpContext.Current.Response.Cache.SetValidUntilExpires(false);
                HttpContext.Current.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                HttpContext.Current.Response.Cache.SetNoStore();
            }
        }
    }
}
