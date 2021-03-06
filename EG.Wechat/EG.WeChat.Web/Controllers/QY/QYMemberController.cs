﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EG.WeChat.Web.Models.QY;
using EG.WeChat.Platform.BL.QYMailList;

namespace EG.WeChat.Web.Controllers.QY
{
    public class QYMemberController : Controller
    {
        private string meg = string.Empty;
        //
        // GET: /QYMember/

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 根据部门主键id获取该部门下（包括子部门的所有成员）
        /// </summary>
        /// <param name="ID">部门主键id</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetMemberByDepPKID(string ID)
        {
            QYDepartmentBL depart=QYDepartmentBL.GetByPKID(ID);
            List<QYMemberBL> memberList = QYMemberBL.GetMemberAllByDep(depart);
            List<MemberVM> memberVMList = new List<MemberVM>();
            foreach (var memberItem in memberList)
            {
                MemberVM member=new MemberVM();
                member.MemberID = memberItem.ID;
                member.Name = memberItem.Name;
                member.UserId = memberItem.UserId;
                member.Weixinid = memberItem.Weixinid;
                member.Mobile = memberItem.Mobile;
                member.Email = memberItem.Email;
                member.Position = memberItem.Position;
                member.Status = memberItem.Status;
                member.Avatar = string.IsNullOrEmpty(memberItem.Avatar) ? string.Empty : memberItem.Avatar+"64";
                memberVMList.Add(member);
            }
            return Json(memberVMList);
        }

        /// <summary>
        /// 增加部门
        /// </summary>
        /// <param name="memberBl">部门类</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddMember(QYMemberBL memberBl)
        {
            if (memberBl.UserId == QYMemberBL.GetMemberByWXID(memberBl.UserId).UserId)
            {
                return Json(new { IsSuccess = false, ErrorMeg = "賬號" + memberBl.UserId + "已存在！" });
            }
            memberBl.Status = "4";
            QYDepartmentBL departBl = QYDepartmentBL.GetByPKID(memberBl.DepartmentPKId.ToString());
            return Json(new { IsSuccess =memberBl.AddMember(string.Empty,ref meg,departBl.DepartmentID), ErrorMeg = meg });
        }

        /// <summary>
        /// 获取单一成员信息
        /// </summary>
        /// <param name="memberID">成员主键id</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetMemberItem(string memberID)
        {
            QYMemberBL memberBl = QYMemberBL.GetMemberByPKID(memberID);
            MemberVM member = new MemberVM();
            member.MemberID = memberBl.ID;
            member.Name = memberBl.Name;
            member.Weixinid = memberBl.Weixinid;
            member.UserId = memberBl.UserId;
            member.Mobile = memberBl.Mobile;
            member.Email = memberBl.Email;
            member.Position = memberBl.Position;
            member.Status = memberBl.Status;
            member.Avatar = memberBl.Avatar;
            member.DepartPKId = memberBl.DepartmentPKId.ToString();
            return Json(member);
        }

        /// <summary>
        /// 更新部门
        /// </summary>
        /// <param name="memberBl"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateMember(QYMemberBL memberBl)
        {
            QYDepartmentBL departBl = QYDepartmentBL.GetByPKID(memberBl.DepartmentPKId.ToString());
            memberBl.DepartmentWXId = departBl.DepartmentID;

            return Json(new { IsSuccess = memberBl.UpdateMember(string.Empty,ref meg), ErrorMeg = meg });
        }

        /// <summary>
        /// 删除成员
        /// </summary>
        /// <param name="memberID">成员主键id</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteMember(string memberID)
        {
            QYMemberBL memberBl =QYMemberBL.GetMemberByPKID(memberID);
            return Json(new { IsSuccess = memberBl.DeleteMember(string.Empty,ref meg), ErrorMeg = meg });
        }
    }
}
