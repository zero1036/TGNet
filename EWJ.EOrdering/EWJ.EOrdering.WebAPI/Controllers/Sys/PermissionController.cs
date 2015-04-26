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
    public class PermissionController : BaseCtrl
    {
        [HttpPost]
        public HttpResponseMessage GetMenu()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                //result.MenuList = new PermissionBL().GetMenu(SysCommon.CurrentUser.Id, SysCommon.CurrentUser.Localization);
                IList<MenuVM> list = new PermissionBL().GetMenu(Guid.NewGuid(), string.Empty);
                response = Request.CreateResponse<IList<MenuVM>>(HttpStatusCode.OK, list);
            }
            catch (Exception ex)
            {
                LogHelper.Write(this.GetType(), ex);
                response = Request.CreateResponse<string>(HttpStatusCode.InternalServerError, ex.Message);
            }

            return response;

        }

        [HttpPost]
        public HttpResponseMessage GetPermission()
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                IList<PermissionVM> list = new PermissionBL().GetPermission(Guid.NewGuid());
                response = Request.CreateResponse<IList<PermissionVM>>(HttpStatusCode.OK, list);
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
