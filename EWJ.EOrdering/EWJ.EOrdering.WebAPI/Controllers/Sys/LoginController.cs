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
    public class LoginController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage Login([FromBody]UserPostParamVM param)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                UserBL scope = new UserBL();
                UserVM model = scope.GetByAccountPwd(param.Account, param.Password);
                if (model != null && model.Id.HasValue)
                {
                    SiteIdentity si = new SiteIdentity(model.Id.Value, model.StaffCode, model.Account
                                                        , model.CHName, model.Localization, model.StoreId);
                    SysCommon.CurrentUser = si;
                    response = Request.CreateResponse(HttpStatusCode.OK);
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

        [HttpPost]
        public HttpResponseMessage Logout()
        {
            HttpResponseMessage response = new HttpResponseMessage();

            SysCommon.CurrentUser = null;
            response = Request.CreateResponse(HttpStatusCode.OK);

            return response;
        }
    }
}
