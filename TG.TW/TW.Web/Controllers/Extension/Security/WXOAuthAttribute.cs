using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using TW.Platform.Sys;
using TW.Platform.BL;
using EG.WeChat.Utility.Tools;
using EG.WeChat.Utility.WeiXin;
/*****************************************************
* 目的：微信请求拦截，验证用户身份
* 创建人：林子聪
* 创建时间：20150427
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace TW.Web.Controllers
{
    public class WXOAuthAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 检查微信用户是否系统用户
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            try
            {
                //判断是否匿名action
                if (!GetIsAllowAnonymous(actionContext))
                {
                    var userBL = new UserBL();
                    //先通过session，查出用户ID
                    string pUserID = userBL.GetQYUserIDBySession();
                    Logger.Log4Net.Info("从Session中获取userid：" + pUserID);
                    if (string.IsNullOrEmpty(pUserID))
                    {
                        //获取请求及code与agentid参数
                        var req = GetHttpRequestBase(actionContext);
                        var code = req.QueryString["code"] != null ? req.QueryString["code"].ToString() : string.Empty;
                        var agentid = req.QueryString["agentid"] != null ? req.QueryString["agentid"].ToString() : string.Empty;

                        Logger.Log4Net.Info("获取code" + code);
                        Logger.Log4Net.Info("获取agentid" + agentid);
                        int iagentid = 0;
                        if (int.TryParse(agentid, out iagentid))
                        {
                            pUserID = userBL.GetQYUserIDByAPI(code, iagentid);

                            if (string.IsNullOrEmpty(pUserID))
                            {
                                throw new Exception();
                            }
                            Logger.Log4Net.Info("从API中获取userid：" + pUserID);
                        }
                        else
                        {
                            throw new Exception();
                        }
                    }
                }
                base.OnActionExecuting(actionContext);
            }
            catch
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }
        }
        /// <summary>
        /// 获取是否匿名action，匿名action无需验证
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        private bool GetIsAllowAnonymous(HttpActionContext actionContext)
        {
            //如果请求Header不包含ticket，则判断是否是匿名调用
            var attr = actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().OfType<AllowAnonymousAttribute>();
            bool isAnonymous = attr.Any(a => a is AllowAnonymousAttribute);
            return isAnonymous;
        }
        /// <summary>
        /// 由于webapi，获取传统HttpRequestBase
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        private HttpRequestBase GetHttpRequestBase(HttpActionContext actionContext)
        {
            var req = actionContext.Request;
            HttpContextBase context = (HttpContextBase)req.Properties["MS_HttpContext"];//获取传统context
            HttpRequestBase request = context.Request;//定义传统request对象
            return request;
        }


    }



}