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
    public class QYMemberBL
    {
        public Int64 ID { get; set; }
        public string UserId { get; set; }
        public string  Name { get; set; }
        public string Position { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Weixinid { get; set; }
        public string Avatar { get; set; }
        public string Status { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime DeleteDate { get; set; }
        public string DeleteBy { get; set; }
        public Int64 DepartmentPKId { get; set; }
        public string DepartmentWXId { get; set; }
        public QYMemberBL()
        {

        }

        public  QYMemberDA MembertDA = new QYMemberDA();

        private QYDepartmentBL _department;

        public QYDepartmentBL DepartMent
        {
            get {

                return QYDepartmentBL.GetByPKID(DepartmentPKId.ToString());
            
            
            }
            set { _department = value; }
        }




        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="CreateUserid">操作者ID</param>
        /// <param name="errMsg">返回错误信息</param>
        /// <param name="wxDepartmentID"></param>
        /// <returns></returns>
        public bool AddMember(string CreateUserid, ref string errMsg, string wxDepartmentID=null)
        {
            try
            {
                QYConfig.RegistWX();
                var accessToken = AccessTokenContainer.GetToken(QYConfig.CorpId);
                var result = MailListApi.CreateMember(accessToken, UserId, Name, new int[] { int.Parse(wxDepartmentID ?? DepartMent.DepartmentID) }, Position, Mobile, null, Email, Weixinid, 0);
                if (result.errcode.ToString() == "请求成功")
                {
                    var gresult = MailListApi.GetMember(accessToken, UserId);
                    Avatar = gresult.avatar;
                    if (MembertDA.AddMember(UserId, Name, Position, Mobile, Email, Weixinid, Avatar, Status, CreateBy, DepartmentPKId.ToString()))
                    {

                        return true;
                    }
                    else
                    {
                        errMsg = result.errcode + ":" + result.errmsg;
                        return false;
                    }

                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Logger.Log4Net.Error("add Member error:" + e);
                errMsg = "操作失敗:" + e.Message;
                return false;
            }
        }
        /// <summary>
        /// 新增Member
        /// </summary>
        /// <param name="CreateUserid">操作者ID</param>
        /// <param name="errMsg">返回错误信息</param>
        /// <returns></returns>
        public bool UpdateMember(string CreateUserid, ref string errMsg)
        {
            try
            {
                QYConfig.RegistWX();
                var accessToken = AccessTokenContainer.GetToken(QYConfig.CorpId);
                var result = MailListApi.UpdateMember(accessToken, UserId, Name, new int[] { int.Parse(DepartmentWXId) }, Position, Mobile, null, Email, Weixinid, 0);
                if (result.errcode.ToString() == "请求成功")
                {
                    if (MembertDA.UpdateMember(ID.ToString(),UserId, Name, Position, Mobile, Email, Weixinid, Avatar, Status, CreateBy,  DepartmentPKId.ToString()))
                    {

                        return true;
                    }
                    else
                    {
                        errMsg = result.errcode + ":" + result.errmsg;
                        return false;
                    }

                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Logger.Log4Net.Error("Update Member error:" + e);
                errMsg = "操作失敗:" + e.Message;
                return false;
            }
        }

        /// <summary>
        /// 删除Member
        /// </summary>
        /// <param name="DeleteUserid">操作者ID</param>
        /// <param name="errMsg">返回错误信息</param>
        /// <returns></returns>
        public bool DeleteMember(string DeleteUserid, ref string errMsg)
        {
            try
            {
                QYConfig.RegistWX();
                var accessToken = AccessTokenContainer.GetToken(QYConfig.CorpId);
                var result = MailListApi.DeleteMember(accessToken, UserId);
                if (result.errcode.ToString() == "请求成功")
                {
                    if (MembertDA.DeleteMember(ID.ToString(), DeleteUserid))
                    {

                        return true;
                    }
                    else
                    {
                        errMsg = result.errcode + ":" + result.errmsg;
                        return false;
                    }

                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Logger.Log4Net.Error("add Member error:" + e);
                errMsg = "操作失敗:" + e.Message;
                return false;
            }
        }

        public bool Invite(ref string errMsg)
        {
            try
            {
                QYConfig.RegistWX();
                var accessToken = AccessTokenContainer.GetToken(QYConfig.CorpId);
                var result = MailListApi.InviteMember(accessToken, UserId, QYConfig.InvateMsg);
                if (result.errcode.ToString() == "请求成功")
                {
                    return true;
                }
                else
                {

                    errMsg = result.errcode + ":" + result.errmsg;
                    return false;
                }
            }
            catch (Exception e)
            {
                Logger.Log4Net.Error("Invite error:" + e);
                errMsg = "操作失敗:" + e.Message;
                return false;
            }
        }



        /// <summary>
        /// 根据部门获取该部门下所有的Member
        /// </summary>
        /// <param name="DepPKID">部门在数据库的主键ID</param>
        /// <returns></returns>
        public static List<QYMemberBL> GetMemberAllByDep(QYDepartmentBL department)
        {
            List<string> l = GetAllDepID(department);
            
            QYMemberDA MembertDA = new QYMemberDA();
            List<QYMemberBL> members =MembertDA.TableToEntity<QYMemberBL>(MembertDA.GetMemberByDepPKIDs(l));
            if (members.Count > 0)
            {
                QYConfig.RegistWX();

                var accessToken = AccessTokenContainer.GetToken(QYConfig.CorpId);
                var mlist = MailListApi.GetDepartmentMemberInfo(accessToken, int.Parse(members.First().DepartMent.DepartmentID), 1, 0).userlist;
                GetMemberResult item;
                for (int i = 0; i < members.Count; i++)
                {
                    item = mlist.Find(p => p.userid == members[i].UserId);
                    if (item != null)
                    {
                        if (item.status.ToString() != members[i].Status)
                        {
                            members[i].Status = item.status.ToString();
                            string errMsg = "";
                            members[i].UpdateMember("1", ref errMsg);
                        }
                    }
                }
            }
            return members;

        }

        /// <summary>
        /// 迭代取出department下所有子department的PKID
        /// </summary>
        /// <param name="dep"></param>
        /// <returns></returns>
        static List<string> GetAllDepID(QYDepartmentBL dep)
        {
            List<string> l = new List<string>();
            l.Add(dep.ID.ToString());
            for (int i = 0; i < dep.Departments.Count; i++)
            {
                l.AddRange(GetAllDepID(dep.Departments[i]));
            }
            return l;

        }

        public static List<QYMemberBL> GetMemberByDepPKID(string DepPKID)
        {

            QYMemberDA MembertDA = new QYMemberDA();
            List<QYMemberBL> members=MembertDA.TableToEntity<QYMemberBL>(MembertDA.GetMemberByDepPKID(DepPKID));
         
            return members;

        }

        /// <summary>
        /// 根据Member的主键ID获取Member对象
        /// </summary>
        /// <param name="ID">Menber的主键ID</param>
        /// <returns></returns>
        public static QYMemberBL GetMemberByPKID(string ID)
        {
            QYMemberDA MembertDA = new QYMemberDA();
            QYMemberBL member=MembertDA.TableToEntity<QYMemberBL>(MembertDA.GetMemberByPKID(ID)).First();
            var accessToken = AccessTokenContainer.GetToken(QYConfig.CorpId);
            var m = MailListApi.GetMember(accessToken,member.UserId);
             if (m != null)
                {
                    if (m.status.ToString() != member.Status)
                    {
                        member.Status = m.status.ToString();
                        string errMsg="";
                        member.UpdateMember("1", ref errMsg);
                    }
                }

             return member;
        }

    }
}
