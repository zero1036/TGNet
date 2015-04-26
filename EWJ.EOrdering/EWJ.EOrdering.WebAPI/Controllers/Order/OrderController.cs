using EWJ.EOrdering.BL.Order;
using EWJ.EOrdering.Common;
using EWJ.EOrdering.ViewModel.Order;
using EWJ.EOrdering.WebAPI.AppCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EWJ.EOrdering.WebAPI.Controllers.Order
{
    public class OrderController : BaseCtrl
    {
        [HttpPost]
        //[HttpGet]
        public HttpResponseMessage GetCycles()
        {
            return this.ExecuteTryCatch(() =>
            {
                var po = new OrderCycleBL(GetCurUSerLocalization());
                return po.GetOrderCycles<OrderCycleVM>();
            });
        }

        [HttpPost]
        public HttpResponseMessage GetCycleNo()
        {
            return this.ExecuteTryCatch(() =>
            {
                var po = new OrderCycleBL(GetCurUSerLocalization());
                return po.GetOrderCycleConfig();
            });
        }

        [HttpPost]
        public HttpResponseMessage UpdateCycles([FromBody]OrderCycleTM pcycle)
        {
            return this.ExecuteTryCatch(() =>
            {
                var po = new OrderCycleBL(GetCurUSerLocalization());
                pcycle.StartDate = pcycle.StartDate.ToLocalTime();
                pcycle.EndDate = pcycle.EndDate.ToLocalTime();
                if (!po.UpdateOrderCycle<OrderCycleTM>(pcycle))
                {
                    throw new Exception("保存错误");
                }
                return true;
            });
        }

    }
}