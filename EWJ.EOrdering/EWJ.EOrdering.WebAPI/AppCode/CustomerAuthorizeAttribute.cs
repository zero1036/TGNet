using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace EWJ.EOrdering.WebAPI.AppCode
{
    public class CustomerAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (SysCommon.CurrentUser != null)
            {
                IsAuthorized(actionContext);
            }
            else
            {
                HandleUnauthorizedRequest(actionContext);
            }
        }
    }
}