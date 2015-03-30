using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EG.WeChat.Platform.BL.QYNotice;
using EG.WeChat.Web.Models.QY;
using System.IO;
using Senparc.Weixin.QY.AdvancedAPIs;
using Senparc.Weixin.QY.CommonAPIs;
using Senparc.Weixin.QY.AdvancedAPIs.MailList;
using Senparc.Weixin.QY.AdvancedAPIs.OAuth2;
using EG.WeChat.Platform.BL.QYMailList;
using Senparc.Weixin.QY.AdvancedAPIs.Mass;
namespace EG.WeChat.Web.Controllers.QY
{
    public class NoticeController : Controller
    {
        //
        // GET: /Notice/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetNotices()
        {
            NoticeCollection list = new NoticeCollection();
            NoticesVM v;
            List<NoticesVM> l = new List<NoticesVM>();
            for (int i = 0; i < list.Notices.Count; i++)
            {
                v = new NoticesVM();
                v.id = list.Notices[i].id.ToString();
                v.Title = list.Notices[i].Title;
                v.CreateDate = list.Notices[i].CreateDate.ToString("yyyy-MM-dd HH:mm:ss");
                v.CreateUserName = list.Notices[i].CreateUserName;
                v.Status = "<a href='#' onclick='showread(" + v.id + ")'>查閱情況</a>";
                v.Context = list.Notices[i].Context;
                v.showbtn = "<a href='#' onclick='showpdf(" + v.id + ")'>預覽</a>";
                v.delbtn = "<a href='#' onclick='deleten("+v.id+")'>刪除</a>";
                l.Add(v);
            }

            return Json(l);
        }

        [HttpPost]
        public ActionResult AddNotice(string title, string context)
        {


            NoticeBL n = new NoticeBL();
            n.Title = title;
            n.CreateBy = "nick";
            n.Context = context;
            n.AddNotice();
            return Json(new { rescode = 200, msg = "" });
        }

        public ActionResult ShowNotice(string id)
        {
            NoticeBL n = NoticeBL.CreateNotice(id);
            return View(n);
        }

        public ActionResult ShowNoticeReg(string id)
        {
            string code = Request.QueryString["code"];
            QYConfig.RegistWX();
            var accessToken = AccessTokenContainer.GetToken(QYConfig.CorpId);
            GetUserIdResult user = OAuth2Api.GetUserId(accessToken, code, QYConfig.VoteAgenID);
            QYMemberBL member = QYMemberBL.GetMemberByWXID(user.UserId);
            NoticeBL n = NoticeBL.CreateNotice(id);
            if (n != null && member != null)
            {
                member.ReadNotice(id);
            }
            return View(n);
        }

        [HttpPost]
        public ActionResult DeleteNotice(string id)
        {
            NoticeBL n = NoticeBL.CreateNotice(id);
            n.DeleteBy = "1";
            n.DeleteNotice();
            return Json(new { rescode = 200, msg = "" });
        }

        [HttpPost]
        public ActionResult GetNoticeMember(string id)
        {
            NoticeBL n = NoticeBL.CreateNotice(id);
            NoticeMembersVM ms = new NoticeMembersVM();
            NoticeMember m;
            ms.total = n.Members.Count;
            ms.rows = new List<NoticeMember>();
            for (int i = 0; i < n.Members.Count; i++)
            {
                m = new NoticeMember();
                m.name = "<img style='width:30px' src='" + n.Members[i].Avatar + "' />" + n.Members[i].Name;
                m.createdate = n.Members[i].CreateDate.ToString("yyyy-MM-dd HH:mm:ss");
                m.alreadyread = n.Members[i].Status.Trim() == "1" ? "已閱讀" : "";
                ms.rows.Add(m);
            }

            
            return Json(ms);
        }
    }
}
