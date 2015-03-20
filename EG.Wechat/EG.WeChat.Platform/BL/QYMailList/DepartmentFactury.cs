using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EG.WeChat.Platform.DA.QYMaiListDA;
namespace EG.WeChat.Platform.BL.QYMailList
{
    public  class DepartmentFactury
    {

        private   QYDepartmentDA DepartmentDA = new QYDepartmentDA();

        public   QYDepartmentBL GetByPKID(string id)
        {
            List<QYDepartmentBL> AllDepartment = DepartmentDA.TableToEntity<QYDepartmentBL>(DepartmentDA.GetAllDepartmentData());
            QYDepartmentBL Department = AllDepartment.Find(p => p.ID == int.Parse(id));
            Department.Departments = GetDepartmentsByParent(Department.DepartmentID, AllDepartment);

            return Department;
        }

        public QYDepartmentBL GetByWXID(string id)
        {
            List<QYDepartmentBL> AllDepartment = DepartmentDA.TableToEntity<QYDepartmentBL>(DepartmentDA.GetAllDepartmentData());
            QYDepartmentBL Department = AllDepartment.Find(p => p.DepartmentID == (id));
            Department.Departments = GetDepartmentsByParent(Department.DepartmentID, AllDepartment);

            return Department;
        }

        public QYDepartmentBL GetAllDepartments()
        {
            List<QYDepartmentBL> AllDepartment = DepartmentDA.TableToEntity<QYDepartmentBL>(DepartmentDA.GetAllDepartmentData());
            QYDepartmentBL Department = AllDepartment.Find(p => p.ParentDepartmentID == "0");
            Department.Departments = GetDepartmentsByParent(Department.DepartmentID, AllDepartment);

            return Department;
        }



        private List<QYDepartmentBL> GetDepartmentsByParent(string wxid, List<QYDepartmentBL> list)
        {
            List<QYDepartmentBL> l = new List<QYDepartmentBL>();
            l.AddRange(list.FindAll(p => p.ParentDepartmentID == (wxid)));
            for (int i = 0; i < l.Count; i++)
            {
                l[i].Departments = (GetDepartmentsByParent(l[i].DepartmentID, list));
            }
            return l;
        }


    }
}
