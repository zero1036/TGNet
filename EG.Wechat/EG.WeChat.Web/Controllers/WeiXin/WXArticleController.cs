using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EG.WeChat.Platform.BL;
using EG.WeChat.Platform.Model;
using EG.WeChat.Utility;
using Senparc.Weixin.MP.Entities;
/*****************************************************
* 目的：图文段落外部预览Controller
* 创建人：林子聪
* 创建时间：20150121
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Web
{
    public class WXArticleController : Controller
    {
        /// <summary>
        /// action view
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="idx"></param>
        /// <returns></returns>
        public ActionResult Index(string mid, int idx)
        {
            return View();
        }
        /// <summary>
        /// action view
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="idx"></param>
        /// <returns></returns>
        public ActionResult LoadModel(string mid, int idx)
        {
            WeChatArticleService pService = new WeChatArticleService("QY");
            WXArticleResultJson pArticle = pService.LoadResourcesSingle(mid);
            var pResult = pService.GetActionResult();
            if (pResult != null || pArticle == null || pArticle.ListNews == null || pArticle.ListNews.Count <= idx)
                return Content("没有找到对应图文段落！");

            return Json(pArticle.ListNews[idx]);
        }
    }
}
