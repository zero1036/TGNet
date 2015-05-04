using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using EG.WeChat.Utility.Tools;

namespace TW.Web.Controllers
{
    /// <summary>
    /// Controller的基类，用于实现适合业务场景的基础功能
    /// </summary>
    /// <typeparam name="T"></typeparam>
    //[BasicAuthentication]
    public abstract class ApiControllerBase : ApiController
    {
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
#if DEBUG
                //调试模式下，自动注册
                var pUserBL = new TW.Platform.BL.UserBL();
                pUserBL.VerifyBCLoginUser("mark", "504");
#endif

                var vm = action.Invoke();
                pActionResult = new CActionResult() { ok = true, message = "", data = vm };
                return Request.CreateResponse(HttpStatusCode.OK, pActionResult);
            }
            catch (Exception ex)
            {
                //转换异常
                var pEGResult = EGExceptionOperator.ConvertException(ex);
                //转换Action结果
                pActionResult = new CActionResult() { ok = false, message = pEGResult.Message, data = null };
                //写入日志
                if (pEGResult.IsLog)
                    Logger.Log4Net.Info(string.Format("异常代码：{0};异常信息：{1}", pEGResult.ExCode, pEGResult.Message));

                //消息不为空时，确保异常消息返回前端
                if (pEGResult.StCode == null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, pActionResult);
                }
                else
                {
                    return Request.CreateResponse((HttpStatusCode)pEGResult.StCode.Value, pActionResult);
                }
            }
        }
    }
    /// <summary>
    /// UserAuthModel
    /// </summary>
    public class UserAuthModel
    {
        public string Controller
        {
            get;
            set;
        }

        public string Actions
        {
            get;
            set;
        }
    }

}
