using EWJ.EOrdering.BL.Sys;
using EWJ.EOrdering.Common;
using EWJ.EOrdering.ViewModel.Sys;
using EWJ.EOrdering.WebAPI.AppCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EWJ.EOrdering.WebAPI.Controllers.Sys
{
    public class UserController : BaseCtrl
    {
        [HttpPost]
        public HttpResponseMessage GetList()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            UserResultVM result = new UserResultVM();

            try
            {
                UserBL scope = new UserBL();

                result.UserList = scope.GetList();
                response = Request.CreateResponse<UserResultVM>(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                LogHelper.Write(this.GetType(), ex);
                response = Request.CreateResponse<string>(HttpStatusCode.InternalServerError, ex.Message);
            }

            return response;
        }

        [HttpPost]
        public HttpResponseMessage GetCurrentUser()
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                UserBL scope = new UserBL();

                SiteIdentity currentUser = SysCommon.CurrentUser;
                if (currentUser != null)
                {
                    response = Request.CreateResponse<SiteIdentity>(HttpStatusCode.OK, currentUser);
                }
                else
                {
                    response = Request.CreateResponse(HttpStatusCode.Forbidden);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(this.GetType(), ex);
                response = Request.CreateResponse<string>(HttpStatusCode.InternalServerError, ex.Message);
            }

            return response;
        }
    }
}
