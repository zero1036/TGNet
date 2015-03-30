using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EG.WeChat.Platform.BL.QYVote;
using EG.WeChat.Web.Models.QY;
using Senparc.Weixin.QY.AdvancedAPIs.OAuth2;
using EG.WeChat.Platform.BL.QYMailList;
using Senparc.Weixin.QY.AdvancedAPIs;
using Senparc.Weixin.QY.CommonAPIs;
using Senparc.Weixin.QY.AdvancedAPIs.MailList;
using EG.WeChat.Web.Models;
 
namespace EG.WeChat.Web.Controllers.QY
{
    public class VoteManageController : Controller
    {
        //
        // GET: /VoteManage/

        public ActionResult Index()
        {
          
             
            return View();
        }

        [HttpPost]
        public ActionResult GetVotes()
        {
            //VoteBL v = VoteBL.GetVote("1");
            //string link = v.Link;
            VoteCollection vc = new VoteCollection();
            VoteVM vote;
            VotelistVM vl = new VotelistVM();
            List<VoteVM> vlist = new List<VoteVM>();
            vc.Votes.ForEach(p =>
            {
                vote = new VoteVM();
                vote.CreateDate = p.CreateDate.ToString("yyyy-MM-dd HH:mm:ss");
                vote.Num = p.VoteMemberCount.ToString();
                vote.Title = "<a href='#' onclick='showvote("+p.ID+")'>" + p.Title+"</a>";
                vote.buttonlist = @"<a href='#' onclick='showsum(" + p.ID + ")'>匯總查詢</a>    <a href='#' onclick='showmembers("+p.ID+")'>明細查詢</a>";
                vote.delbutton = @"<a href='#' onclick=delet("+p.ID+")>刪除</a>";
                vlist.Add(vote);

            });
            vl.rows = vlist;
            vl.total = vlist.Count;
            return Json(vl);
        }

        [HttpPost]
        public ActionResult AddVote(AddvoteVM vote)
        {
            try
            {
                VoteBL v = new VoteBL();
                v.ID = int.Parse(vote.id);
                v.Title = vote.title;
                v.Context = vote.context;
                v.vtype = vote.vtype;
                v.Options = new List<VoteOption>();
                VoteOption opt;
                for (int i = 0; i < vote.options.Count; i++)
                {
                    opt = new VoteOption();
                    opt.OptionTitle = vote.options[i].OptionTitle;
                    opt.OptinContext = vote.options[i].OptinContext;
                    opt.ID = int.Parse(vote.options[i].id);
                    opt.ActionType = vote.options[i].ActionType;
                    opt.CreateBy = "1";
                    v.Options.Add(opt);
                }
                if (v.AddVote())
                {
                    return Json(new { rescode = 200, msg = "" });
                }
                else
                {
                    return Json(new { rescode = 500, msg = "操作失敗" });
                }
            }
            catch (Exception e)
            {
                return Json(new { rescode = 500, msg = "操作失敗" });
            }
            
        }

        [HttpPost]
        public ActionResult GetVote(string id)
        {
            VoteBL v = VoteBL.GetVote(id);
            AddvoteVM avm = new AddvoteVM();
            avm.id = v.ID.ToString();
            avm.title = v.Title;
            avm.context = v.Context;
            avm.vtype = v.vtype;
            avm.options = new List<VoteOptionsVM>();
            for (int i = 0; i < v.Options.Count; i++)
            {
                avm.options.Add(new VoteOptionsVM() { id=v.Options[i].ID.ToString(), ActionType=null,
                 OptinContext=v.Options[i].OptinContext, OptionTitle=v.Options[i].OptionTitle});
            }


            return Json(avm);
        }

        [HttpPost]
        public ActionResult UpdateVote(AddvoteVM vote)
        {
            VoteBL v = VoteBL.GetVote(vote.id);
            v.Title = vote.title;
            v.UpdateBy = "1";
            v.vtype = vote.vtype;
            v.Context = vote.context;
            
            for (int i = 0; i < v.Options.Count; i++)
            {
                if (!vote.options.Exists(p => p.id == v.Options[i].ID.ToString()))
                {
                    v.Options[i].ActionType = "2";
                }

            }

            VoteOption vop;
            for (int i = 0; i < vote.options.Count; i++)
            {
                
                if (v.Options.Exists(p => p.ID.ToString() == vote.options[i].id))
                {
                    vop = v.Options.Find(p => p.ID.ToString() == vote.options[i].id);
                    vop.OptinContext = vote.options[i].OptinContext;
                    vop.OptionTitle = vote.options[i].OptionTitle;
                    vop.UpdateBy = "1";
                    vop.ActionType = null;
                }
                else
                {
                    vop = new VoteOption();
                    vop.OptinContext = vote.options[i].OptinContext;
                    vop.OptionTitle = vote.options[i].OptionTitle;
                    vop.UpdateBy = "1";
                    vop.ActionType = "1";
                    v.Options.Add(vop);
                }

            }
            if (v.UpdateVote())
            {
                return Json(new { rescode = 200, msg = "" });
            }
            else
            {
                return Json(new { rescode = 500, msg = "操作失敗" });
            }

        }


        [HttpPost]
        public ActionResult DeleteVote(string id)
        {
            VoteBL v = VoteBL.GetVote(id);
            v.DeleteBy = "1";
            if (v.DeleteVote())
            {
                return Json(new { rescode = 200, msg = "" });
            }
            else
            {
                return Json(new { rescode = 500, msg = "操作失敗" });
            }
        }

        [HttpPost]
        public ActionResult GetVoteRes(string id)
        {
            VoteBL v = VoteBL.GetVote(id);
            int sum = v.VoteMember.Count;
            VoteOptResVM orv  ;
            VoteResVM list = new VoteResVM();
            list.opts = new List<VoteOptResVM>();
            list.vtype = v.vtype;
            list.vnum = sum;
            v.Options.ForEach(p => {
                orv = new VoteOptResVM();
                orv.title = p.OptinContext;
                orv.num = p.VoteMember.Count;
                if (sum == 0)
                {
                    orv.persent = 0;
                }
                else
                {
                    orv.persent =Convert.ToInt32( Math.Round((((double)p.VoteMember.Count) * 100 / sum),0));
                }
                
                list.opts.Add(orv);
            });

            return Json(list);
        }

        [HttpPost]
        public ActionResult GetVoteMember(string id)
        {
            VoteBL v = VoteBL.GetVote(id);
            VoteMemberlistVM ms = new VoteMemberlistVM();
            ms.total = v.VoteMember.Count;
            VoteMemberVM m;
            ms.rows = new List<VoteMemberVM>();
            for (int i = 0; i < v.VoteMember.Count; i++)
            {
                m = new VoteMemberVM();
                m.name = "<img style='width:30px' src='" + v.VoteMember[i].Avatar + "' />" + v.VoteMember[i].Name;
                m.title = v.VoteMember[i].OptionTitle;
                m.context = v.VoteMember[i].OptinContext;
                m.picurl = v.VoteMember[i].Avatar;
                ms.rows.Add(m);
            }
            return Json(ms);
        }

        [HttpPost]
        public ActionResult GetVoteMemberByType(string vid,string vtype)
        {
            VoteBL v = VoteBL.GetVote(vid);
            if (!string.IsNullOrEmpty(vtype) && vtype != "ALL")
            {
                v.VoteMember = v.VoteMember.Where(p => p.OptionTitle == vtype).ToList();
            }
            VoteMemberlistVM ms = new VoteMemberlistVM();
            ms.total = v.VoteMember.Count;
            VoteMemberVM m;
            ms.rows = new List<VoteMemberVM>();
            for (int i = 0; i < v.VoteMember.Count; i++)
            {
                m = new VoteMemberVM();
                m.name ="<img src='"+v.VoteMember[i].Avatar+"' />" +v.VoteMember[i].Name;
                m.title = v.VoteMember[i].OptionTitle;
                m.context = v.VoteMember[i].OptinContext;
                m.picurl =  v.VoteMember[i].Avatar;
                ms.rows.Add(m);
            }
            return Json(ms);
        }

        [HttpPost]
        public ActionResult GetVoteOptCombo(string id)
        {
            OptComboMV t = new OptComboMV();
            t.id = "ALL";
            t.text = "ALL";
            t.desc = "ALL";
            List<OptComboMV> list = new List<OptComboMV>();
            list.Add(t);
            VoteBL v = VoteBL.GetVote(id);
            for (int i = 0; i < v.Options.Count; i++)
            {
                t = new OptComboMV();
                t.id = (i).ToString();
                t.text = v.Options[i].OptionTitle;
                t.desc = v.Options[i].OptinContext;
                list.Add(t);
            }
            return Json(list);
        }


        public ActionResult VoteMobel(string id)
        {
            string code = Request.QueryString["code"];
            QYConfig.RegistWX();
            var accessToken = AccessTokenContainer.GetToken(QYConfig.CorpId);
            GetUserIdResult user = OAuth2Api.GetUserId(accessToken, code, QYConfig.VoteAgenID);
            QYMemberBL member = QYMemberBL.GetMemberByWXID(user.UserId);
            ViewBag.userid = member.ID;
           
            ViewBag.vid = id;
            MemberVote mv = new MemberVote(member.ID.ToString(), id);

            ViewBag.mv = mv;
            VoteBL v = VoteBL.GetVote(id);
            return View(v);
        }

        public ActionResult VoteMobelForShow(string id)
        {
            //string code = Request.QueryString["code"];
            //QYConfig.RegistWX();
            //var accessToken = AccessTokenContainer.GetToken(QYConfig.CorpId);
            //GetUserIdResult user = OAuth2Api.GetUserId(accessToken, code, QYConfig.VoteAgenID);
            //QYMemberBL member = QYMemberBL.GetMemberByWXID(user.UserId);
            ViewBag.userid ="";

            ViewBag.vid = id;
            MemberVote mv = new MemberVote();
            mv.opts = new List<VoteOption>();
            ViewBag.mv = mv;
            VoteBL v = VoteBL.GetVote(id);
            return View(v);
        }

        [HttpPost]
        public ActionResult AddMemberVote(MemberVote vm)
        {
            if (vm.AddMemberVote("1"))
            {
                return Json(new { rescode = 200, msg = "" });
            }
            else
            {
                return Json(new { rescode = 500, msg = "操作失敗" });
            }
            
        }


        [HttpPost]
        public ActionResult AddVoteT( string id)
        {
            return Json(new { rescode = 500, msg = "操作失敗" });
        }

        [HttpPost]
        public ActionResult GetVoteList()
        {
            VoteCollection voteCollection = new VoteCollection();
            List<VoteBL> voteList = voteCollection.Votes;
            List<VoteSelVM> voteSelList = new List<VoteSelVM>();
            foreach (VoteBL voteItem in voteList)
            {
                VoteSelVM vote = new VoteSelVM();
                vote.ID = voteItem.ID;
                vote.Title = voteItem.Title;
                voteSelList.Add(vote);

            }
            return Json(voteSelList);
        }
    }
}
