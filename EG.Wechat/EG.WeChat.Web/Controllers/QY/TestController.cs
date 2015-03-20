using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EG.WeChat.Platform;
using EG.WeChat.Platform.BL.QYMailList;
using EG.WeChat.Platform.BL.QYVote;
namespace EG.WeChat.Web.Controllers.QY
{
    public class TestController : Controller
    {
        //
        // GET: /Test/

        public ActionResult Index()
        {
            //QYDepartmentBL p = QYDepartmentBL.GetAllDepartments();
            //Json(p);
           // QYDepartmentBL dep = p.Departments.Find(g => g.DepartmentID == "13");
           // QYDepartmentBL.DownloadDatafromWX();
            //dep.ParentDepartmentID = "12";
            //dep.UpdateDepartment("1", ref errMsg);

            //QYMemberBL member = new QYMemberBL();
            //string errMsg = "";

            //member.Email = "1222@aa.com";
            //member.UserId = "boiis12223";
            //member.Name = "Nick3";
            //member.DepartmentPKId = 3;
            //member.AddMember("1", ref errMsg, "13");
            
            //member.DepartmentPKId = 3;
            //member.DepartmentWXId = "12";
            //member.UpdateMember("1", ref errMsg);
            
            //member.AddMember();

            VoteBL v = VoteBL.GetVote("1");
            //v.Options[1].OptionTitle = "234";
            //v.UpdateVote();
            List<VoteMember> l = v.VoteMember;
            //VoteCollection vc = new VoteCollection();
            //v.Title = v.Title + "we";
            //v.Context = "^_^" + v.Context;
            //v.Options[2].OptinContext = "^_^" + v.Options[2].OptinContext;
            //v.Options[1].ActionType="2";
            //v.Options.Add(new VoteOption(){ CreateBy="1", OptinContext="111", ActionType="1"});
            //v.UpdateBy = "1";
            //v.UpdateVote();
            //VoteBL v2 = new VoteBL();
            //v2.Title = "123";
            //v2.CreateBy = "1";
            //v2.Context = "2132131";
            //v2.vtype = "1";
            //v2.PicUrl = "";
            //v2.Options = new List<VoteOption>() {
            //    new VoteOption(){ CreateBy="1", OptinContext="111"},
            //    new VoteOption(){ CreateBy="1", OptinContext="222"},
            //    new VoteOption(){ CreateBy="1", OptinContext="333"}
            //};
            //v2.AddVote();


            return View();
        }

        //
        // GET: /Test/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Test/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Test/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Test/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Test/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Test/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Test/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
