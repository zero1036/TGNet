using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EWJ.EOrdering.ViewModel.Diamond;
using EWJ.EOrdering.BL.Diamond;
using EWJ.EOrdering.WebAPI.AppCode;

namespace EWJ.EOrdering.WebAPI.Controllers
{
    public class DiamondController :BaseCtrl
    {
        /// <summary>
        /// 查询获取diamond列表
        /// </summary>
        /// <param name="queryItem"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage DiamondsQuery(DiamondQueryVM diamondQuery)
        {
            try
            {
                if (diamondQuery == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "沒有可用的參數");
                }
                if (diamondQuery.ScendingList == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "必須要有排序的參數");
                }

                DiamondListResultVM diamondResult = new DiamondListResultVM();
                //string localization=SysCommon.CurrentUser.Localization;
                string localization = "014002";
                diamondResult.DiamondList = DiamondBL.DiamondsQuery(diamondQuery, localization);
                return Request.CreateResponse<DiamondListResultVM>(HttpStatusCode.OK,diamondResult);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError,ex.Message);
            }
 
        }


        /// <summary>
        /// 调配diamond
        /// </summary>
        /// <param name="assignList"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage DiamondsAssign(DiamondAssignVM assignList)
        {
            try
            {
                DiamondBL diamondBl = new DiamondBL();
                AssignResultVM assignResult = new AssignResultVM();
                assignResult.AssignResultList = diamondBl.DiamondsAssign(assignList);
                return Request.CreateResponse<AssignResultVM>(HttpStatusCode.OK,assignResult);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}