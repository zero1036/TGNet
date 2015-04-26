using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using EWJ.EOrdering.WebAPI.AppCode;

namespace EWJ.EOrdering.WebAPI.Controllers
{
    [CustomerAuthorizeAttribute]
    public class BaseCtrl : ApiController
    {
        #region Public Member
        /// <summary>
        /// ActionResult
        /// </summary>
        public class CActionResult
        {
            public bool Success { get; set; }
            public string Message { get; set; }
            public object Data { get; set; }
        }
        #endregion

        #region Method
        /// <summary>
        /// 执行方法并捕获错误
        /// </summary>
        /// <param name="pInterface"></param>
        /// <param name="action"></param>
        protected HttpResponseMessage ExecuteTryCatch<VModel>(Func<VModel> action)
        {
            CActionResult pActionResult;
            try
            {
                var vm = action.Invoke();
                pActionResult = new CActionResult() { Success = true, Message = "", Data = vm };
                return Request.CreateResponse(HttpStatusCode.OK, pActionResult);
            }
            catch (Exception ex)
            {
                //异常日志
                EWJ.EOrdering.Common.LogHelper.Write(this.GetType(), ex);
                pActionResult = new CActionResult() { Success = false, Message = ex.Message, Data = null };
                return Request.CreateResponse(HttpStatusCode.InternalServerError, pActionResult);
            }
        }
        /// <summary>
        /// 获取当前用户国际化
        /// </summary>
        /// <returns></returns>
        protected string GetCurUSerLocalization()
        {
            return EWJ.EOrdering.WebAPI.AppCode.SysCommon.CurrentUser == null ? "014003" : EWJ.EOrdering.WebAPI.AppCode.SysCommon.CurrentUser.Localization;
        }
        #endregion

    }
}