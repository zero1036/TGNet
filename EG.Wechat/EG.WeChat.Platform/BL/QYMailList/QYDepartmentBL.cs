using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EG.WeChat.Platform.DA.QYMaiListDA;
using Senparc.Weixin.QY.AdvancedAPIs;
using Senparc.Weixin.QY.CommonAPIs;
using Senparc.Weixin.QY.AdvancedAPIs.MailList;
namespace EG.WeChat.Platform.BL.QYMailList
{
    public class QYDepartmentBL
    {
        public Int64 ID { get; set; }
        public string DepartmentID { get; set; }
        public string ParentDepartmentID { get; set; }
        public string Name { get; set; }
        public string wcOrder { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime DeleteDate { get; set; }
        public string DeleteBy { get; set; }

        private List<QYDepartmentBL> _departments;

        public List<QYDepartmentBL> Departments
        {
            get
            {

                return _departments;
            }
            set { _departments = value; }
        }





        private List<QYMemberBL> _members;

        public List<QYMemberBL> Members
        {
            get { 

                return QYMemberBL.GetMemberByDepPKID(ID.ToString());
            
            }
            set { _members = value; }
        }

        private static QYDepartmentDA DepartmentDA = new QYDepartmentDA();

        public static  void DownloadDatafromWX()
        {
            QYConfig.RegistWX();
            var accessToken = AccessTokenContainer.GetToken(QYConfig.CorpId);
            var result = MailListApi.GetDepartmentList(accessToken);
            for(int i=0;i<result.department.Count;i++)
            {
                DepartmentDA.AddDepartment(result.department[i].id.ToString(), result.department[i].parentid.ToString(), result.department[i].name, "1", "1");
            }
             QYDepartmentBL d= GetAllDepartments();
            var l= MailListApi.GetDepartmentMemberInfo(accessToken, 1, 1, 0);
            for (int i = 0; i < l.userlist.Count; i++)
            {
                QYMemberDA mda = new QYMemberDA();
                QYDepartmentBL p=   QYDepartmentBL.GetByWXID(l.userlist[i].department[0].ToString());
                mda.AddMember(l.userlist[i].userid, l.userlist[i].name, l.userlist[i].position, l.userlist[i].mobile, l.userlist[i].email, l.userlist[i].weixinid,
                    l.userlist[i].avatar, l.userlist[i].status.ToString(), "1", p.ID.ToString());
            }


        }

        /// <summary>
        /// 根据数据库的ID获取部门对象
        /// </summary>
        /// <param name="id">数据库部门ID</param>
        /// <returns></returns>
        public static QYDepartmentBL GetByPKID(string id)
        {
            DepartmentFactury depfac = new DepartmentFactury();
            return depfac.GetByPKID(id);
        }

        /// <summary>
        /// 根据wechat的ID获取部门对象
        /// </summary>
        /// <param name="id">数据库部门ID</param>
        /// <returns></returns>
        public static QYDepartmentBL GetByWXID(string id)
        {
            DepartmentFactury depfac = new DepartmentFactury();
            return depfac.GetByWXID(id);
        }

        /// <summary>
        /// 获取包含所有部门的对象
        /// </summary>
        /// <returns></returns>
        public static QYDepartmentBL GetAllDepartments()
        {
            DepartmentFactury depfac = new DepartmentFactury();
            return depfac.GetAllDepartments();
        }



        /// <summary>
        /// 新增子部门
        /// </summary>
        /// <param name="NewDepartmentName">新增子部門的名字</param>
        /// <param name="errMsg">返回錯誤信息</param>
        /// <returns>返回操作結果</returns>
        public bool AddChildDepartment(string NewDepartmentName,string UserID, ref string errMsg)
        {
            try
            {
                QYConfig.RegistWX();
                var accessToken = AccessTokenContainer.GetToken(QYConfig.CorpId);
                var result = MailListApi.CreateDepartment(accessToken, NewDepartmentName, int.Parse(DepartmentID));
                if (result.errcode.ToString() == "请求成功")
                {
                    if (DepartmentDA.AddDepartment(result.id.ToString(), DepartmentID, NewDepartmentName, "1", UserID))
                    {
                        QYDepartmentBL newdep = GetByWXID(result.id.ToString());
                        Departments.Add(newdep);
                        return true;
                    }
                    else
                    {
                        errMsg = "数据库新增部门失败";
                        return false;

                    }
                    
                }
                else
                {
                    Logger.Log4Net.Error(result.errcode + ":" + result.errmsg);
                    errMsg = result.errcode + ":" + result.errmsg;
                }
            }
            catch (Exception e)
            {
                Logger.Log4Net.Error("add department error:" + e);
                errMsg = "操作失敗:" + e.Message;
                return false;
            }
            return false;
        }

        /// <summary>
        /// 删除本部门
        /// </summary>
        /// <param name="errMsg">返回錯誤信息</param>
        /// <returns>返回操作結果</returns>
        public bool DeleteDepartment(string UserID,ref string errMsg,List<QYDepartmentBL> list=null)
        {
            try
            {
                QYConfig.RegistWX();
                var accessToken = AccessTokenContainer.GetToken(QYConfig.CorpId);
                var result = MailListApi.DeleteDepartment(accessToken, DepartmentID);
                if (result.errcode.ToString() == "请求成功")
                {
                    if (DepartmentDA.DeleteDepartmentByPkID(ID.ToString(), UserID))
                    {
                        DeleteBy = UserID;
                        DeleteDate = DateTime.Now;
                        if (list != null)
                        {
                            list.Remove(this);
                        }
                        
                        return true;
                    }
                    else
                    {
                        errMsg = "数据库刪除部门失败";
                        return false;
                        
                    }
                   
                }
                else
                {
                    Logger.Log4Net.Error(result.errcode + ":" + result.errmsg);
                    errMsg = result.errcode + ":" + result.errmsg;
                }
            }
            catch (Exception e)
            {
                Logger.Log4Net.Error("Delete department error:" + e);
                errMsg = "操作失敗:" + e.Message;
                return false;
            }
            return false;
        }

        /// <summary>
        /// 更新部门信息到微信与数据库
        /// </summary>
        /// <param name="UserID">操作者ID</param>
        /// <param name="errMsg">返回信息</param>
        /// <returns>操作是否成功</returns>
        public bool UpdateDepartment(string UserID, ref string errMsg)
        {
            try
            {
                QYConfig.RegistWX();
                var accessToken = AccessTokenContainer.GetToken(QYConfig.CorpId);
                var result = MailListApi.UpdateDepartment(accessToken, DepartmentID, Name, int.Parse(ParentDepartmentID));
                if (result.errcode.ToString() == "请求成功")
                {
                    if (DepartmentDA.UpdateDepartmentByPkID(ID.ToString(), ParentDepartmentID, Name, wcOrder, UserID))
                    {

                        return true;
                    }
                    else
                    {
                        errMsg = "数据库修改部门失败";
                        return false;
                    }
                     
                }
                else
                {
                    Logger.Log4Net.Error(result.errcode + ":" + result.errmsg);
                    errMsg = result.errcode + ":" + result.errmsg;
                }
            }
            catch (Exception e)
            {
                Logger.Log4Net.Error("Delete department error:" + e);
                errMsg = "操作失敗:" + e.Message;
                return false;
            }
            return false;
        }


       

    }

   

}
