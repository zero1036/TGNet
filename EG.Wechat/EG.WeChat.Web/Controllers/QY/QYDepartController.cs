using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EG.WeChat.Platform.BL.QYMailList;
using EG.WeChat.Web.Models.QY;

namespace EG.WeChat.Web.Controllers.QY
{
    public class QYDepartController :AccessController
    {
        private string meg = string.Empty;

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取部门下自身以及所有子部门
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetDepartMenu()
        {
            QYDepartmentBL departBl= QYDepartmentBL.GetAllDepartments();
            IList<DepartMenuVM> departMenuVM = GetDepartMenuList(departBl);
            return Json(departMenuVM);                   
        }

        /// <summary>
        /// 增加子部门
        /// </summary>
        /// <param name="depPKID">部门主键id</param>
        /// <param name="name">部门名</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddChildrenDepart(string depPKID,string name)
        {
            QYDepartmentBL departBl = QYDepartmentBL.GetByPKID(depPKID);
            departBl.Name = name;

            return Json(new
            {
                IsSuccess = departBl.AddChildDepartment(departBl.Name, 
                UserID, ref meg), ErrorMeg = meg });

 
        }

        /// <summary>
        /// 更新部门
        /// </summary>
        /// <param name="depPKID">部门主键id</param>
        /// <param name="name">部门名</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateDepart(string depPKID,string name)
        {
            QYDepartmentBL departBl = QYDepartmentBL.GetByPKID(depPKID);
            departBl.Name = name;
            return Json(new
            {
                IsSuccess = departBl.UpdateDepartment(
                    UserID, ref meg),
                ErrorMeg = meg
            });
        }

        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="depPKID">部门主键id</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteDepart(string depPKID)
        {

            QYDepartmentBL departBl = QYDepartmentBL.GetByPKID(depPKID);
            if (departBl.Departments.Count == 0 && departBl.Members.Count == 0)
            {
                return Json(new
                {
                    IsSuccess = departBl.DeleteDepartment(
                        UserID, ref meg),
                    ErrorMeg = meg
                });
            }
            else
            {
                return Json(new
                {
                    IsSuccess = false,
                    ErrorMeg = "沒有子部門且沒有成員的部門才可以被刪除！"
                });
            }
        }

        /// <summary>
        /// 获取部门下自身以及所有子部门
        /// </summary>
        /// <param name="qyDepartBl">部门类</param>
        /// <returns></returns>
        private IList<DepartMenuVM> GetDepartMenuList(QYDepartmentBL qyDepartBl)
        {
            List<DepartMenuVM> list = new List<DepartMenuVM>();
            DepartMenuVM departMenuVM = new DepartMenuVM();
            departMenuVM.DepPKID = qyDepartBl.ID;
            departMenuVM.Name = qyDepartBl.Name;
            departMenuVM.DepartmentID = qyDepartBl.DepartmentID;
            departMenuVM.ParentDepartmentID = qyDepartBl.ParentDepartmentID;
            departMenuVM.wcOrder = qyDepartBl.wcOrder;
            list.Add(departMenuVM);
           

            if(qyDepartBl.Departments.Count()!=0)
            {
                foreach (var item in qyDepartBl.Departments)
                {
                    list.AddRange(GetDepartMenuList(item));
                }
            }
            return list;
        }
    }
}
