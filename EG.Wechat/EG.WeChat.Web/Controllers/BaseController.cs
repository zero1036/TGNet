using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using EG.WeChat.Model;
using EG.WeChat.Web;
using EG.WeChat.Web.Common;

namespace EG.WeChat.Web.Controllers
{
    public class BaseController : Controller
    {

        private HttpSessionState _httpSession;

        protected HttpSessionState HttpSession
        {
            get
            {
                if (_httpSession == null)
                {
                    _httpSession = System.Web.HttpContext.Current.Session;
                }
                return _httpSession;
            }
        }



        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserID
        {
            set { HttpSession[ConstString.S_UserID] = value; }
            get
            {
                if (HttpSession[ConstString.S_UserID] != null)
                {
                    return HttpSession[ConstString.S_UserID].ToString();
                }
                return string.Empty;
            }
        }


        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName
        {
            set { HttpSession[ConstString.S_UserName] = value; }
            get
            {
                if (HttpSession[ConstString.S_UserName] != null)
                {
                    return HttpSession[ConstString.S_UserName].ToString();
                }
                return string.Empty;
            }
        }


        /// <summary>
        /// 用戶访问权限
        /// </summary>
        public List<AccessRight> AccessRightList
        {
            set { HttpSession[ConstString.S_AccessRight] = value; }
            get
            {
                if (HttpSession[ConstString.S_AccessRight] != null && HttpSession[ConstString.S_AccessRight] is List<AccessRight>)
                {
                    return HttpSession[ConstString.S_AccessRight] as List<AccessRight>;
                }
                return new List<AccessRight>();
            }
        }


        //protected override void OnResultExecuting(ResultExecutingContext filterContext)
        //{
        //    var mpFileVersionInfo = FileVersionInfo.GetVersionInfo(Server.MapPath("~/bin/Senparc.Weixin.MP.dll"));
        //    var extensionFileVersionInfo = FileVersionInfo.GetVersionInfo(Server.MapPath("~/bin/Senparc.Weixin.MP.MvcExtension.dll"));
        //    TempData["MpVersion"] = string.Format("{0}.{1}", mpFileVersionInfo.FileMajorPart, mpFileVersionInfo.FileMinorPart); //Regex.Match(fileVersionInfo.FileVersion, @"\d+\.\d+");
        //    TempData["ExtensionVersion"] = string.Format("{0}.{1}", extensionFileVersionInfo.FileMajorPart, extensionFileVersionInfo.FileMinorPart); //Regex.Match(fileVersionInfo.FileVersion, @"\d+\.\d+");

        //    base.OnResultExecuting(filterContext);
        //}

    }
}
