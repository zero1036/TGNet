using EWJ.EOrdering.BL.ModelBL;
using EWJ.EOrdering.Common;
using EWJ.EOrdering.ViewModel.ModelVM;
using EWJ.EOrdering.WebAPI.AppCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EWJ.EOrdering.WebAPI.Controllers.Model
{
    public class SCModelController :BaseCtrl
    {
        [HttpPost]
        public HttpResponseMessage GetModels(ModelSearchVM search)
        {
            return this.ExecuteTryCatch(() =>
            {
                SCModel m = new SCModel();
                search.Localization = GetCurUSerLocalization();
                return m.Getlist(search);
            });
        }
    }
}