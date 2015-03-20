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
        //private QYDepartmentBL _departBL;
        //protected QYDepartmentBL DepartBL
        //{
        //    get
        //    {
        //        if (_departBL == null)
        //        {
        //            _departBL = TransactionProxy.New<QYDepartmentBL>();
        //        }
        //        return _departBL;
        //    }
        //}
        
        // GET: /QYDepart/



        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetDepartMenu()
        {
            QYDepartmentBL departBl= QYDepartmentBL.GetAllDepartments();
            IList<DepartMenuVM> departMenuVM = GetDepartMenuList(departBl);
            return Json(departMenuVM);                   
        }

        [HttpPost]
        public ActionResult AddChildrenDepart(string depPKID,string name)
        {
            QYDepartmentBL departBl = QYDepartmentBL.GetByPKID(depPKID);
            departBl.Name = name;

            return Json(new
            {
                IsSuccess = departBl.AddChildDepartment(departBl.Name, 
                string.Empty, ref meg), ErrorMeg = meg });

 
        }

        [HttpPost]
        public ActionResult UpdateDepart(string depPKID,string name)
        {
            QYDepartmentBL departBl = QYDepartmentBL.GetByPKID(depPKID);
            departBl.Name = name;
            return Json(new
            {
                IsSuccess = departBl.UpdateDepartment(
                    string.Empty, ref meg),
                ErrorMeg = meg
            });
        }

        [HttpPost]
        public ActionResult DeleteDepart(string depPKID)
        {

            QYDepartmentBL departBl = QYDepartmentBL.GetByPKID(depPKID);

            return Json(new
            {
                IsSuccess = departBl.DeleteDepartment(
                    string.Empty, ref meg),
                ErrorMeg = meg
            });
        }

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
