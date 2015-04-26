using EWJ.EOrdering.BL.Sys;
using EWJ.EOrdering.Common;
using EWJ.EOrdering.ViewModel.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EWJ.EOrdering.WebAPI.Controllers.Sys
{
    public class StoreController : BaseCtrl
    {
        [HttpPost]
        public HttpResponseMessage GetStores()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                //result.MenuList = new PermissionBL().GetMenu(SysCommon.CurrentUser.Id, SysCommon.CurrentUser.Localization);
                IList<StoreVM> list = new StoreBL().GetStoreList(Guid.NewGuid());
                response = Request.CreateResponse<IList<StoreVM>>(HttpStatusCode.OK, list);
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
