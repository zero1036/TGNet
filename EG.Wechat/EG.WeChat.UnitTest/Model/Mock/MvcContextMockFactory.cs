using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EG.WeChat.Web.Controllers;
using System.Web.Routing;
using Moq;
using System.Web;
using EG.WeChat.Web.Models;
/*****************************************************
* 目的：ControllerContext生成器
* 创建人：林子聪
* 创建时间：20141104
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.UnitTest.Model
{
    /// <summary>
    /// ControllerContext生成器
    /// 
    /// </summary>
    public static class MvcContextMockFactory
    {
        private static ControllerContext controllerContext = null;

        #region 公有接口
        /// <summary>
        /// 
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public static ControllerContext CreateControllerContext(Controller controller)
        {
            controllerContext = new ControllerContext(CreateHttpContext(), new RouteData(), controller);
            return controllerContext;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="contextBase"></param>
        /// <returns></returns>
        public static ControllerContext CreateControllerContext(Controller controller, HttpContextBase contextBase)
        {
            controllerContext = new ControllerContext(contextBase, new RouteData(), controller);
            return controllerContext;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="url"></param>
        /// <param name="httpMethod"></param>
        /// <param name="name"></param>
        /// <param name="pattern"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static ControllerContext CreateControllerContext(Controller controller, string url, string httpMethod, string name, string pattern, string obj)
        {
            controllerContext = new ControllerContext(CreateHttpContext(), GetRouteData(url, httpMethod, name, pattern, obj), controller);
            return controllerContext;
        }
        #endregion

        #region 创建httpcontext
        /// <summary> 
        /// 创建HttpContextBase 
        /// </summary> 
        /// <returns></returns> 
        public static HttpContextBase CreateHttpContext()
        {
            var context = new Mock<HttpContextBase>();
            var request = new Mock<HttpRequestBase>();
            var response = new Mock<HttpResponseBase>();
            var session = new Mock<HttpSessionStateBase>();
            var server = new Mock<HttpServerUtilityBase>();
            response.Setup(rsp => rsp.ApplyAppPathModifier(Moq.It.IsAny<string>())).Returns((string s) => s);

            context.Setup(ctx => ctx.Request).Returns(request.Object);
            context.Setup(ctx => ctx.Response).Returns(response.Object);
            context.Setup(ctx => ctx.Session).Returns(session.Object);
            context.Setup(ctx => ctx.Server).Returns(server.Object);

            return context.Object;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="httpMethod"></param>
        /// <returns></returns>
        private static HttpContextBase CreateHttpContext(string url, string httpMethod)
        {
            var context = new Mock<HttpContextBase>();
            var request = new Mock<HttpRequestBase>();
            var response = new Mock<HttpResponseBase>();
            var session = new Mock<HttpSessionStateBase>();
            var server = new Mock<HttpServerUtilityBase>();
            response.Setup(rsp => rsp.ApplyAppPathModifier(Moq.It.IsAny<string>())).Returns((string s) => s);
            request.Setup(req => req.AppRelativeCurrentExecutionFilePath).Returns(url);
            request.Setup(req => req.HttpMethod).Returns(httpMethod);
            context.Setup(ctx => ctx.Request).Returns(request.Object);
            context.Setup(ctx => ctx.Response).Returns(response.Object);
            context.Setup(ctx => ctx.Session).Returns(session.Object);
            context.Setup(ctx => ctx.Server).Returns(server.Object);

            return context.Object;
        }
        #endregion

        #region 路由处理
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="httpMethod"></param>
        /// <param name="name"></param>
        /// <param name="pattern"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static RouteData GetRouteData(string url, string httpMethod,
string name, string pattern, string obj)
        {
            HttpContextBase pContextBase = CreateHttpContext(url, httpMethod);
            var routeData = RouteTable.Routes.GetRouteData(pContextBase);
            if (routeData == null)
            {
                RouteTable.Routes.MapRoute(name, pattern, obj);
                routeData = RouteTable.Routes.GetRouteData(pContextBase);
            }
            return routeData;
        }
        #endregion

    }
}
