using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EG.WeChat.Web.Models;
using Senparc.Weixin.MP.AdvancedAPIs;
using System.IO;
using EG.WeChat.Service;
//using EG.WeChat.Business.Interface;
using EG.WeChat.Utility.Tools;
using EG.WeChat.Platform.Model;
using EG.WeChat.Platform.BL;
using EG.WeChat.Platform.DA;
//using System.ServiceModel.Web;
using Senparc.Weixin.MP.Entities;
/*****************************************************
* 目的：模板消息通讯功能Controller
* 创建人：林子聪
* 创建时间：20141107
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Web.Controllers
{
    public class TemplateMessageController : WXOrganizationController
    {
        #region 私有成员
        //ViewData字典项
        private string m_RecordCount = "RecordCount";
        #endregion

        #region 控制器

        #region MessageConfig
        [HttpGet]
        /// <summary>
        /// 消息模板配置View加载
        /// </summary>
        /// <returns></returns>
        public ActionResult MessageConfig()
        {
            WeChatMessageService pService = new WeChatMessageService();
            List<TemplateMessageConfigM> pList = pService.GetTemplateMessageList<TemplateMessageConfigM>();
            EGExceptionResult pResult = pService.GetActionResult();
            if (pResult != null)
            {
                return Json(pResult);
            }
            return View("MessageConfig", pList);
        }
        /// <summary>
        /// 开启修改/新增模板View
        /// </summary>
        /// <param name="pIndex"></param>
        /// <returns></returns>
        public ActionResult TemplateEdit(string pIndex)
        {
            WeChatMessageService pService = new WeChatMessageService();
            TemplateMessageConfigM pEn = pService.GetTemplateMessageSingle<TemplateMessageConfigM>(Convert.ToInt32(pIndex));
            EGExceptionResult pResult = pService.GetActionResult();
            if (pResult != null)
            {
                return Json(pResult);
            }
            return View("TemplateEdit", pEn);
        }
        [HttpPost]
        /// <summary>
        /// 执行修改/新增模板action
        /// </summary>
        /// <param name="pModel"></param>
        /// <returns></returns>
        public ActionResult TemplateEdit(TemplateMessageConfigM pModel)
        {
            EGExceptionResult pResult;
            if (!ModelState.IsValid)
            {
                pResult = new EGExceptionResult(false, "请填写 * 项", ((int)EGActionCode.缺少必要参数).ToString());
                return Json(pResult);
            }

            WeChatMessageService pService = new WeChatMessageService();
            pService.UpdateTemplateMessageList<TemplateMessageConfigM>(pModel, pModel.ID);
            pResult = pService.GetActionResult();
            if (pResult != null)
            {
                return Json(pResult);
            }
            else
            {
                pResult = new EGExceptionResult(true, "保存成功", ((int)EGActionCode.执行成功).ToString());
                return Json(pResult);
            }
        }
        #endregion

        #region Message
        /// <summary>
        /// 模板消息页加载action
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Message()
        {
            return base.WXUserManager();
        }
        #endregion

        #region ServicesSend
        /// <summary>
        /// 发送客服消息
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="mid"></param>
        /// <returns></returns>
        public ActionResult CSMessageSend(string openId, string msgtype, int? lcId, string mediaId = "", string pContent = "")
        {
            WXCSMessageService pService = new WXCSMessageService();
            EGExceptionResult pResult = pService.SendCSMessage(openId, msgtype, this.Request.Url.Host, lcId, mediaId, pContent);
            if (pResult == null)
            {
                pResult = new EGExceptionResult(true, "", "");
            }
            else
            {
                pResult.Message = "當前微信用戶不在對話狀態，無法發送客服消息！";
            }
            return Json(pResult);
        }
        #endregion

        #region MessageSend
        [HttpPost]
        /// <summary>
        /// 发送模板消息执行action_通过平台界面发送
        /// </summary>
        /// <returns></returns>
        public ActionResult MessageSend(WCTemplateBindingA model)
        {
            if (!ModelState.IsValid)
                return Content("填写错误，请重新输入！");

            WeChatMessageService pService = new WeChatMessageService();
            //发送模板消息
            pService.SendTemplateMessage(model.OpenID, model.TemplateID, model.URL, model.TemData);
            EGExceptionResult pResult = pService.GetActionResult();
            if (pResult != null)
            {
                return Json(pResult);
            }
            pResult = new EGExceptionResult(true, "", "");
            return Json(pResult);
        }
        //[HttpPost]
        ///// <summary>
        ///// 发送模板消息执行action_通过服务接口调用发送
        ///// </summary>
        ///// <param name="Account"></param>
        ///// <param name="TemplateID"></param>
        ///// <param name="URL"></param>
        ///// <param name="TemData"></param>
        ///// <returns></returns>
        //public ActionResult TemplateMessageService(string Account, string TemplateID, string URL, string TemData)
        //{
        //    WeChatMessageService pService = new WeChatMessageService();
        //    //发送模板消息
        //    pService.SendTemplateMessage(Account, TemplateID, URL, TemData, new EG.WeChat.Business.JR.BL.AccountBL().GetOpenIDByAccountName);
        //    EGExceptionResult pResult = pService.GetActionResult();
        //    if (pResult != null)
        //    {
        //        return Json(pResult);
        //    }
        //    return Content("发送成功");
        //}
        //[HttpPost]
        ///// <summary>
        ///// 获取模板消息配置列表Json串——服务接口
        ///// </summary>
        ///// <param name="State"></param>
        ///// <returns></returns>
        //public ActionResult GetMessageConfigService(string State)
        //{
        //    WeChatMessageService pService = new WeChatMessageService();
        //    string strResult = pService.GetTemplateMessageListJson<TemplateMessageConfigM>();
        //    EG.WeChat.Service.EGExceptionResult pResult = pService.GetActionResult();
        //    if (pResult != null)
        //    {
        //        return Json(pResult);
        //    }
        //    return Content(strResult);
        //}
        #endregion

        #region GroupSend
        /// <summary>
        /// 加载发送群发消息页面
        /// </summary>
        /// <returns></returns>
        public ActionResult GroupSend()
        {
            return View();
        }
        /// <summary>
        /// 加载群发消息审核页面
        /// </summary>
        /// <returns></returns>
        public ActionResult GroupSendReview()
        {
            return View();
        }
        /// <summary>
        /// 创建群发消息，加入待审核队伍
        /// </summary>
        /// <param name="mediaid"></param>
        /// <param name="sendtype"></param>
        /// <param name="sendtarget"></param>
        /// <param name="textcontent"></param>
        /// <param name="msgtype"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateGsMessage(string mediaid, int sendtype, string sendtarget, string textcontent, string msgtype)
        {
            WeChatMessageService pService = new WeChatMessageService();
            pService.CreateGsMessage(UserID, mediaid, sendtype, sendtarget, textcontent, msgtype);
            EGExceptionResult pResult = pService.GetActionResult();
            //
            if (pResult != null)
            {
                return Json(pResult);
            }
            pResult = new EGExceptionResult(true, "", "");
            return Json(pResult);
        }
        /// <summary>
        /// 查询获取所有群发消息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetGsMessage()
        {
            WeChatMessageService pService = new WeChatMessageService();
            List<WXGsMessageVM> pList = pService.GetAllGsMessage<WXGsMessageVM>();
            EGExceptionResult pResult = pService.GetActionResult();
            //
            if (pResult != null)
            {
                return new EmptyResult();
            }
            return Json(pList);
        }
        /// <summary>
        /// 查询获取目标条件群发消息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetGsMessageByFilter(string filterString)
        {
            WeChatMessageService pService = new WeChatMessageService();
            List<WXGsMessageVM> pList = pService.GetGsMessage<WXGsMessageVM>(filterString);
            EGExceptionResult pResult = pService.GetActionResult();
            //
            if (pResult != null)
            {
                return Json(pResult);
            }
            return Json(pList);
        }
        /// <summary>
        /// 发送群发消息执行action，以分组信息作为范围
        /// </summary>
        /// <param name="messageid"></param>
        /// <param name="mediaid"></param>
        /// <param name="textcontent"></param>
        /// <param name="msgtype"></param>
        /// <param name="groupid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GroupSendingByGroupID(string messageid, string mediaid, string textcontent, string msgtype, string groupid)
        {
            WeChatMessageService pService = new WeChatMessageService();
            pService.SendGroupMessageByGroupID(messageid, UserID, groupid, mediaid, textcontent, msgtype);
            EGExceptionResult pResult = pService.GetActionResult();
            //
            if (pResult != null)
            {
                return Json(pResult);
            }
            pResult = new EGExceptionResult(true, "", "");
            return Json(pResult);
        }
        /// <summary>
        /// 发送群发消息执行action，以用户OpenID作为范围
        /// </summary>
        /// <param name="messageid"></param>
        /// <param name="mediaid"></param>
        /// <param name="textcontent"></param>
        /// <param name="msgtype"></param>
        /// <param name="sex"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GroupSendingByOpenID(string messageid, string mediaid, string textcontent, string msgtype, string sex)
        {
            WeChatMessageService pService = new WeChatMessageService();
            pService.SendGroupMessageByOpenID(messageid, UserID, mediaid, textcontent, msgtype, sex);
            EGExceptionResult pResult = pService.GetActionResult();
            //
            if (pResult != null)
            {
                return Json(pResult);
            }
            pResult = new EGExceptionResult(true, "", "");
            return Json(pResult);
        }
        #endregion

        #region 测试视图
        [HttpPost]
        /// <summary>
        /// MVC action服务
        /// Post方法   
        /// string带参
        /// </summary>
        /// <param name="content1"></param>
        /// <param name="content2"></param>
        /// <returns></returns>
        public ActionResult MvcPostFormForString(string content1, string content2)
        {
            return Content(string.Format("{0}:{1}", content1, content2));
        }
        //[HttpGet]
        //public ActionResult TestView()
        //{
        //    try
        //    {
        //        string strAccessToken = Senparc.Weixin.MP.CommonAPIs.WeiXinSDKExtension.GetCurrentAccessToken();

        //        NewsModel pEn = new NewsModel();
        //        pEn.author = "作者";
        //        pEn.content = "<!DOCTYPE html><html><head></head><body><p style='margin-left: 40px; color: gray'>ok</p></body></html>";
        //        pEn.content_source_url = "www.baidu.com";
        //        pEn.digest = "描述";
        //        pEn.thumb_media_id = "A_fR-XfVRtGd6qnw2_Vhwmajt3bkVKAl2WvkTLRtKbUVuCGr4TSKwTR2N9UhQH-7";
        //        pEn.title = "标题";
        //        List<NewsModel> plist = new List<NewsModel>();
        //        plist.Add(pEn);
        //        NewsModel[] pArray = plist.ToArray();
        //        Senparc.Weixin.MP.Entities.UploadMediaFileResult pResulte = Senparc.Weixin.MP.AdvancedAPIs.Media.UploadNews(strAccessToken, pArray);
        //        return Content(pResulte.media_id);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content(ex.ToString());
        //    }
        //}
        [HttpPost]
        public ActionResult TestView(string PageIndex)
        {
            ////获取微信用户集合
            //WeChatOrgService pOrgService = new WeChatOrgService();
            //List<WeChatUser> pList = pOrgService.GetWCUserList_Cache();
            //int iPageIndex = 0;
            //if (!string.IsNullOrEmpty(PageIndex) && int.TryParse(PageIndex, out iPageIndex))
            //{
            //    pList = CommonFunction.SubListForTable<WeChatUser>(pList, iPageIndex, 10);
            //}
            //return Json(pList);
            return new EmptyResult();
        }
        [HttpPost]
        public ContentResult TestMessageSendForService(string str)
        {
            return Content("完成发送！" + str);
        }
        [HttpPost]
        public ContentResult TestMessageSendForService2(System.IO.Stream getStream)
        {
            return Content("完成发送！");
        }
        [HttpPost]
        public ContentResult TestMessageSendForService3(String p1, String p2, String p3)
        {
            return Content("完成发送！" + p1 + p2 + p3);
        }
        #endregion
        #endregion
    }

}
