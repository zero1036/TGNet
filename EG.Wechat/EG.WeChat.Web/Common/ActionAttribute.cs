using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EG.WeChat.Model;
using EG.WeChat.Web.Common;

namespace EG.WeChat.Web
{

    public class ActionAttribute : ActionFilterAttribute
    {
        private string _action;
        private string _controller;
        private string _requestType;
        private HttpSessionStateBase _session;
        private Level _level = Level.OneLevel;

        /// <summary>
        /// 权限应用等级，默认为一级。
        /// </summary>
        public Level level
        {
            get { return _level; }
            set { _level = value; }
        }


        /// <summary>
        /// 重写OnActionExecuting，判断是用户是否登录超时或拥有该模块权限。
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _session = filterContext.HttpContext.Session;
            _requestType = filterContext.HttpContext.Request.RequestType;
            _controller = filterContext.RouteData.Values["controller"].ToString().ToLower();
            _action = filterContext.RouteData.Values["action"].ToString().ToLower();

            if (_session[ConstString.S_UserID] == null)
            {
                TransferToPermissionDeniedPage(filterContext, 1);
                return;
            }

            //只控制Get请求的是否有权限
            if (_requestType.Equals("GET") && level == Level.OneLevel)
            {
                if (!CheckUserAllowAccess())
                {
                    TransferToPermissionDeniedPage(filterContext, 2);
                }
            }
        }


        /// <summary>
        /// 检查用户是否拥有该模块功能
        /// </summary>
        /// <returns></returns>
        private bool CheckUserAllowAccess()
        {
            List<AccessRight> accessRight = _session[ConstString.S_AccessRight] as List<AccessRight>;

            return accessRight.Any(z => z.Action.ToLower() == _action && z.Controller.ToLower() == _controller);
        }


        /// <summary>
        /// 权限错误，页面跳转处理
        /// </summary>
        /// <param name="filterContext"></param>
        /// <param name="type"></param>
        public void TransferToPermissionDeniedPage(ActionExecutingContext filterContext, int type = 1)
        {
            ContentResult result = new ContentResult();
            switch (type)
            {
                case 1: result.Content = "<script>window.location.href='/Home/LoginTimeout';</script>";
                    _session.RemoveAll();
                    _session.Abandon();
                    break;
                case 2: result.Content = "<script>window.location.href='/Home/NoPermission';</script>";
                    break;
                default: result.Content = "<script>window.location.href='/Home/Sorry';</script>";
                    _session.RemoveAll();
                    _session.Abandon();
                    break;
            }
            filterContext.Result = result;
        }

    }

    public class CacheFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Get or sets the cache duration in seconds . the default is 10 seconds
        /// </summary>
        /// <value> the cache duration in seconds </value>

        public int Duration
        {
            get;
            set;
        }
        public CacheFilterAttribute()
        {
            Duration = 10;
        }
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (Duration <= 0) return;

            HttpCachePolicyBase cache = filterContext.HttpContext.Response.Cache;//.HttpContext.Response.Cache;
            TimeSpan cacheDuration = TimeSpan.FromSeconds(Duration);

            cache.SetCacheability(System.Web.HttpCacheability.Public);
            cache.SetExpires(DateTime.Now.Add(cacheDuration));
            cache.SetMaxAge(cacheDuration);
            cache.AppendCacheExtension("must revalidate, proxy-revalidate");
        }
    }
}