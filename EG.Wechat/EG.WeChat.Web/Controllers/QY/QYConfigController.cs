using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EG.WeChat.Platform.BL;
using EG.WeChat.Utility.Tools;
/*****************************************************
* 目的：企业号基础设置Controller
* 创建人：林子聪
* 创建时间：20150316
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Web.Controllers
{
    public class QYConfigController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View("QYConfig");
        }
        /// <summary>
        /// 获取企业应用集合
        /// </summary>
        /// <returns></returns>
        public ActionResult Get()
        {
            WXCorpBaseService pService = new WXCorpBaseService();
            var pList = pService.GetCorpInfos();
            EGExceptionResult pResult = pService.GetActionResult();
            if (pResult != null)
                return Json(pResult);
            return Json(pList);
        }
        /// <summary>
        /// 获取企业应用菜单
        /// </summary>
        /// <returns></returns>
        public ActionResult GetQYAppMenu(int agentid)
        {
            WXCorpBaseService pService = new WXCorpBaseService();
            var pMenu = pService.GetQYAppMenu(agentid);
            EGExceptionResult pResult = pService.GetActionResult();
            if (pResult != null)
                return Json(pResult);
            return Json(pMenu);
        }
        /// <summary>
        /// 设置企业应用菜单
        /// </summary>
        /// <returns></returns>
        public ActionResult SetQYAppMenu(int agentid, string svl)
        {
            WXCorpBaseService pService = new WXCorpBaseService();
            pService.SetQYAppMenu(agentid, svl);
            EGExceptionResult pResult = pService.GetActionResult();
            if (pResult != null)
                return Json(pResult);
            return Json(new EGExceptionResult(true, "", ""));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="vl"></param>
        /// <returns></returns>
        public ActionResult Coding(string key, string vl)
        {
            var vll = Emperor.UtilityLib.CyberUtils.Encrypt("Aes", 256, vl, key);
            return Content(vll);
        }
    }
}
