using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.CommonAPIs;
using EG.WeChat.Service;
using EG.WeChat.Utility.Tools;
using EG.WeChat.Platform.Model;
using EG.WeChat.Platform.BL;
using System.IO;
/*****************************************************
* 目的：微信资源管理Controller
* 创建人：林子聪
* 创建时间：20141124
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Web.Controllers
{
    public class WXResourceController : Controller
    {
        #region 图片
        /// <summary>
        /// 启动图片资源配置页面
        /// </summary>
        /// <param name="ColCount"></param>
        /// <returns></returns>
        public ActionResult WXPictureConfig(string ColCount)
        {
            //返回视图
            return View("WXPictureConfig2");
        }
        /// <summary>
        /// 启动图片资源页面后，再单独执行action获取图片资源
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LoadWXPictures(int PageIndex = 1, int RowCountInPage = 8, string filterString = "")
        {
            int iRecCount = -1;
            //加载图片资源集合
            WeChatPictureService pService = new WeChatPictureService("QY");
            List<WXPictureResultJson> plist = pService.LoadResources(PageIndex, RowCountInPage, filterString, out iRecCount);
            EGExceptionResult pResult = pService.GetActionResult();
            if (pResult != null)
            {
                return Json(pResult);
            }

            var pModel = new
            {
                IsSuccess = true,
                ListJson = plist,
                RecordsCount = iRecCount
            };
            return Json(pModel);
        }
        /// <summary>
        /// 批量保存Dropzone上传图片——旧有方法
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveUploadedFile()
        {
            bool isSavedSuccessfully = true;
            string fName = "";
            string fPath = "";
            try
            {
                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];
                    //Save file content goes here
                    fName = file.FileName;
                    if (file != null && file.ContentLength > 0)
                    {

                        var originalDirectory = new DirectoryInfo(string.Format("{0}Images", Server.MapPath(@"\")));

                        string pathString = System.IO.Path.Combine(originalDirectory.ToString(), "WXResources");

                        var fileName1 = Path.GetFileName(file.FileName);

                        bool isExists = System.IO.Directory.Exists(pathString);

                        if (!isExists)
                            System.IO.Directory.CreateDirectory(pathString);

                        var path = string.Format("{0}\\{1}", pathString, file.FileName);

                        file.SaveAs(path);

                        fPath = path;
                        fPath = fPath.Replace('\\', '/');
                    }

                }

            }
            catch (Exception)
            {
                isSavedSuccessfully = false;
            }

            if (isSavedSuccessfully)
            {
                return Json(new { Message = fName, FileName = fName, FilePath = fPath });
            }
            else
            {
                return Json(new { Message = "Error in saving file", FileName = fName, FilePath = fPath });
            }
        }
        /// <summary>
        /// 图片资源上传action
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ImageUpLoad()
        {
            WeChatPictureService pService = new WeChatPictureService("QY");
            //首先上传图片资源到微信服务器，并写入本地服务器配置文件
            List<WXPictureResultJson> plist = pService.UpdateResources(Request, "flUpload");
            EGExceptionResult pResult = pService.GetActionResult();
            if (pResult != null)
            {
                return Json(pResult);
            }

            //初始化绑定model
            var pModel = new
            {
                IsSuccess = true,
                ListJson = plist
            };
            return Json(pModel);
        }
        #endregion

        #region 图文
        /// <summary>
        /// 启动图文资源列表配置页面
        /// </summary>
        /// <param name="ColCount"></param>
        /// <returns></returns>
        //[OutputCache(Duration = 1200, VaryByParam = "*", Location = OutputCacheLocation.Client, NoStore = false)]
        //[CacheFilter(Duration = 60, Order = 2)]
        public ActionResult WXArticlesConfig()
        {
            return View("WXArticlesConfig2");
        }
        /// <summary>
        /// 启动图文资源页面后，再单独执行action获取图文资源
        /// </summary>
        /// <returns></returns>
        public ActionResult LoadWXArticles(int PageIndex = 1, int RowCountInPage = 8, string filterString = "")
        {
            int iRecCount = -1;
            //加载图片资源集合
            WeChatArticleService pService = new WeChatArticleService("QY");
            List<WXArticleResultJson> plist = pService.LoadResources(PageIndex, RowCountInPage, filterString, out iRecCount);
            EGExceptionResult pResult = pService.GetActionResult();
            if (pResult != null)
            {
                return Json(pResult);
            }
            var pModel = new
            {
                IsSuccess = true,
                ListJson = plist,
                RecordsCount = iRecCount
            };
            return Json(pModel);
        }
        /// <summary>
        /// 图文资源上传
        /// </summary>
        /// <param name="ListNews"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ArticleUpLoad(int? id, string name, string cla, string ListNews, bool bByLink)
        {
            var nw = bByLink ? "news" : "mpnews";
            WeChatArticleService pService = new WeChatArticleService("QY", nw);
            pService.UpdateResources(id, name, cla, ListNews, bByLink);
            EGExceptionResult pResult = pService.GetActionResult();
            if (pResult != null)
            {
                return Json(pResult);
            }
            pResult = new EGExceptionResult(true, "保存成功", EGActionCode.执行成功.ToString());
            return Json(pResult);
        }
        #endregion

        #region 视频
        /// <summary>
        /// 啟動視頻資源列表頁面
        /// </summary>
        /// <returns></returns>
        public ActionResult WXVideosConfig()
        {
            return View();
        }
        /// <summary>
        /// 獲取微信端視頻資源
        /// </summary>
        /// <returns></returns>
        public ActionResult LoadWXVideos(int PageIndex = 1, int RowCountInPage = 8, string filterString = "")
        {
            int iRecCount = -1;
            WeChatVideoService pService = new WeChatVideoService("QY");
            List<WXVideoResultJson> plist = pService.LoadResources(PageIndex, RowCountInPage, filterString, out iRecCount);
            EGExceptionResult pResult = pService.GetActionResult();
            if (pResult != null)
            {
                return Json(pResult);
            }

            var pModel = new
            {
                IsSuccess = true,
                ListJson = plist,
                RecordsCount = iRecCount
            };
            return Json(pModel);
        }
        /// <summary>
        /// 視頻资源上传action
        /// </summary>
        /// <param name="lcname"></param>
        /// <param name="lcclassify"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult VideoUpLoad(string lcname, string lcclassify)
        {
            WeChatVideoService pService = new WeChatVideoService("QY");
            //首先上传視頻资源到微信服务器，并写入本地服务器配置文件
            List<WXVideoResultJson> plist = pService.UpdateResources(Request, lcname, lcclassify);
            EGExceptionResult pResult = pService.GetActionResult();
            if (pResult != null)
            {
                return Json(pResult);
            }

            //初始化绑定model
            var pModel = new
            {
                IsSuccess = true,
                ListJson = plist
            };
            return Json(pModel);
        }
        #endregion

        #region 音頻
        /// <summary>
        /// 啟動音頻資源列表頁面
        /// </summary>
        /// <returns></returns>
        public ActionResult WXVoicesConfig()
        {
            return View();
        }
        /// <summary>
        /// 獲取微信端音頻資源
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <param name="RowCountInPage"></param>
        /// <param name="filterString"></param>
        /// <returns></returns>
        public ActionResult LoadWXVoices(int PageIndex = 1, int RowCountInPage = 8, string filterString = "")
        {
            int iRecCount = -1;
            WeChatVoiceService pService = new WeChatVoiceService("QY");
            List<WXVoiceResultJson> plist = pService.LoadResources(PageIndex, RowCountInPage, filterString, out iRecCount);
            EGExceptionResult pResult = pService.GetActionResult();
            if (pResult != null)
            {
                return Json(pResult);
            }

            var pModel = new
            {
                IsSuccess = true,
                ListJson = plist,
                RecordsCount = iRecCount
            };
            return Json(pModel);
        }
        /// <summary>
        /// 音頻资源上传action
        /// </summary>
        /// <param name="lcname"></param>
        /// <param name="lcclassify"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult VoiceUpLoad(string lcname, string lcclassify)
        {
            WeChatVoiceService pService = new WeChatVoiceService("QY");
            //首先上传资源到微信服务器，并写入本地服务器配置文件
            List<WXVoiceResultJson> plist = pService.UpdateResources(Request, lcname, lcclassify);
            EGExceptionResult pResult = pService.GetActionResult();
            if (pResult != null)
            {
                return Json(pResult);
            }

            //初始化绑定model
            var pModel = new
            {
                IsSuccess = true,
                ListJson = plist
            };
            return Json(pModel);
        }
        #endregion

        #region 关键字
        /// <summary>
        /// 关键字配置
        /// </summary>
        /// <returns></returns>
        public ActionResult KWConfig()
        {
            return View();
        }
        #endregion

        #region 通用
        /// <summary>
        /// 刪除資源
        /// </summary>
        /// <param name="ptype"></param>
        /// <param name="lcid"></param>
        /// <returns></returns>
        public ActionResult DeleteResource(int lcid, string ptype)
        {
            WeChatResourcesService pService = new WeChatResourcesService();
            pService.DeleteResource(ptype, lcid);
            EGExceptionResult pResult = pService.GetActionResult();
            if (pResult != null)
            {
                return Json(pResult);
            }

            var pModel = new
            {
                IsSuccess = true
            };
            return Json(pModel);
        }
        #endregion
    }
}
