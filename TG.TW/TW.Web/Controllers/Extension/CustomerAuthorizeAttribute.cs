using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace TW.Web.Controllers
{
    public class CustomAuthorizeAttribute : System.Web.Http.AuthorizeAttribute
    {
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            //判断用户是否登录
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
                HandleUnauthorizedRequest(actionContext);
        }
        protected override void HandleUnauthorizedRequest(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            var challengeMessage = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            challengeMessage.Headers.Add("WWW-Authenticate", "Basic");
            throw new System.Web.Http.HttpResponseException(challengeMessage);

        }

    }

    //public class CustomAuthorizeAttribute : System.Web.Mvc.AuthorizeAttribute
    //{
    //    public new string[] Roles { get; set; }
    //    protected override bool AuthorizeCore(System.Web.Mvc.HttpContextBase httpContext)
    //    {
    //        if (httpContext == null)
    //        {
    //            throw new ArgumentNullException("HttpContext");
    //        }
    //        if (!httpContext.User.Identity.IsAuthenticated)
    //        {
    //            return false;
    //        }
    //        if (Roles == null)
    //        {
    //            return true;
    //        }
    //        if (Roles.Length == 0)
    //        {
    //            return true;
    //        }
    //        if (Roles.Any(httpContext.User.IsInRole))
    //        {
    //            return true;
    //        }
    //        return false;
    //    }

    //    public override void OnAuthorization(System.Web.Mvc.AuthorizationContext filterContext)
    //    {
    //        string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
    //        string actionName = filterContext.ActionDescriptor.ActionName;
    //        string roles = GetRoles.GetActionRoles(actionName, controllerName);
    //        if (!string.IsNullOrWhiteSpace(roles))
    //        {
    //            this.Roles = roles.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
    //        }
    //        base.OnAuthorization(filterContext);
    //    }
    //}

    public class ActionAttribute : System.Web.Mvc.ActionFilterAttribute
    {

        /// <summary>
        /// 重写OnActionExecuting，判断是用户是否登录超时或拥有该模块权限。
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
        {
             base.OnActionExecuting(filterContext);
        }




    }
}