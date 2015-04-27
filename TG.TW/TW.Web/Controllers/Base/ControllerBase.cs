using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TW.Web.Controllers
{
    public class ControllerBase : ApiController
    {
        //#region Method
        ///// <summary>
        ///// 执行方法并捕获错误
        ///// </summary>
        ///// <param name="pInterface"></param>
        ///// <param name="action"></param>
        //protected HttpResponseMessage ExecuteTryCatch<VModel>(Func<VModel> action)
        //{
        //    CActionResult pActionResult;
        //    try
        //    {
        //        var vm = action.Invoke();
        //        pActionResult = new CActionResult() { Success = true, Message = "", Data = vm };
        //        return Request.CreateResponse(HttpStatusCode.OK, pActionResult);
        //    }
        //    catch (Exception ex)
        //    {
        //        //异常日志
        //        EWJ.EOrdering.Common.LogHelper.Write(this.GetType(), ex);
        //        pActionResult = new CActionResult() { Success = false, Message = ex.Message, Data = null };
        //        //消息不为空时，确保异常消息返回前端
        //        if (string.IsNullOrEmpty(pActionResult.Message))
        //            return Request.CreateResponse(HttpStatusCode.InternalServerError, pActionResult);
        //        else
        //            return Request.CreateResponse(HttpStatusCode.OK, pActionResult);
        //    }
        //}
        //#endregion
    }
}
