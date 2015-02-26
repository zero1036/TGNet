using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EG.WeChat.Model;

namespace EG.WeChat.Web.Controllers
{
    public class ActiveUserController : AccessController
    {

        public ActionResult List()
        {
            var modle = ActiveList(string.Empty);
            return View("../User/ActiveList",modle);
        }


        [HttpPost]
        public ActionResult List(string userID)
        {
            var modle = ActiveList(userID);
            return View("../User/ActiveList",modle);
        }


        [HttpPost]
        public ActionResult Delete(string sessionId)
        {
            ResultM result = new ResultM();

            if (StaticLibrary.ActiveSession.ContainsKey(sessionId))
            {
                var activeUser = StaticLibrary.ActiveSession.FirstOrDefault(z => z.Key == sessionId).Value;
                activeUser.RemoveAll();
                activeUser.Abandon();
                result.Affected = 1;
            }

            return Json(result);
        }


        private DataTable ActiveList(string userID)
        {
            DataTable tb = new DataTable();

            tb.Columns.Add("SessionID");
            tb.Constraints.Add("pk_id", tb.Columns["SessionID"], true);
            tb.Columns.Add("UserID");

            DataRow dr;
            foreach (var item in StaticLibrary.ActiveSession)
            {
                if (string.IsNullOrEmpty(userID) || item.Value["UserID"].Equals(userID))
                {
                    if (item.Value["UserID"] != null)
                    {
                        dr = tb.NewRow();
                        dr["SessionID"] = item.Key;
                        dr["UserID"] = item.Value["UserID"];
                        tb.Rows.Add(dr);
                    }
                }
            }
            return tb;
        }




    }
}
