using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EG.WeChat.Model;
using EG.WeChat.Platform.DA;

namespace EG.WeChat.Platform.BL
{
    public class UserBL
    {
        private UserDA _userDA;
        protected UserDA UserDA
        {
            get
            {
                if (_userDA == null)
                {
                    _userDA = new UserDA();
                }
                return _userDA;
            }
        }

        #region 登录管理

        public virtual LoginInfo Login(T_User model)
        {
            LoginInfo result = new LoginInfo();

            model.Password = GetSHA512Encrypt(model.Password);

            T_User dt = UserDA.Login(model);

            if (dt == null)
            {
                return null;
            }

            result.UserID = dt.UserID;
            result.UserName = dt.UserName;
            result.AccessRight = GetUserAccessRight(dt.UserID);

            return result;
        }


        public virtual List<AccessRight> GetUserAccessRight(string userID)
        {
            List<AccessRight> list = new List<AccessRight>();

            DataTable data = UserDA.GetUserAccessRight(userID);

            foreach (DataRow item in data.Rows)
            {
                string controller = item["Controller"].ToString();
                string action = item["Action"].ToString();

                if (!list.Exists(z => z.Controller == controller && z.Action == action))
                {
                    list.Add(new AccessRight() { Controller = controller, Action = action });
                }
            }
            return list;
        }


        private string GetSHA512Encrypt(string password)
        {
            string ciphertext = string.Empty;

            foreach (var item in System.Security.Cryptography.SHA512.Create().ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)))
            {
                ciphertext += item.ToString("X2");
            }
            return ciphertext;
        }


        #endregion


        #region 用戶管理

        public virtual PagingM List(string UserID, string UserName, int pageIndex)
        {
            Hashtable model = new Hashtable();
            model.Add("UserID", string.Format("%{0}%", UserID));
            model.Add("UserName", string.Format("%{0}%", UserName));

            return UserDA.GetPageList(model, pageIndex);
        }


        public virtual ResultM New(T_User model, string userID)
        {
            ResultM result = new ResultM();

            T_User data = GetUser(model.UserID);

            if (data != null)
            {
                result.Message = "Existing UserID!";
            }
            else
            {
                model.CreatedBy = userID;
                model.LastModifiedBy = userID;
                model.Password = GetSHA512Encrypt(model.Password);
                result.Affected = UserDA.Add(model);
            }
            return result;
        }


        public virtual ResultM Edit(T_User model, string userID)
        {
            ResultM result = new ResultM();

            T_User data = GetUser(model.UserID);

            data.UserName = model.UserName;
            data.State = model.State;
            data.LastModifiedBy = userID;
            data.LastModifiedTime = DateTime.Now;

            if (!string.IsNullOrEmpty(model.Password))
            {
                data.Password = GetSHA512Encrypt(model.Password);
            }

            result.Affected = UserDA.Edit(data);

            return result;
        }


        public virtual ResultM Delete(string userId, string currentUserID)
        {
            ResultM result = new ResultM();
            T_User data = GetUser(userId);
            data.IsDeleted = true;
            data.DeletedBy = currentUserID;
            data.DeletedTime = DateTime.Now;
            result.Affected = UserDA.Edit(data);
            return result;
        }


        public virtual T_User GetUser(string userID)
        {
            T_User result = UserDA.GetByPKey(new T_User() { UserID = userID, IsDeleted = false });
            return result;
        }

        #endregion



        #region 用户组

        /// <summary>
        /// 获取已选用户
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public virtual List<T_User> GetSelectedUser(int GroupID, string UserName)
        {
            Hashtable model = new Hashtable();
            model.Add("GroupID", GroupID);
            model.Add("UserName", string.Format("%{0}%", UserName));
            var table = UserDA.GetSelectedUser(model);
            return UserDA.TableToEntity<T_User>(table);
        }


        /// <summary>
        /// 获取可选用户
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public virtual List<T_User> GetSelectUser(int GroupID, string UserName)
        {
            Hashtable model = new Hashtable();
            model.Add("GroupID", GroupID);
            model.Add("UserName", string.Format("%{0}%", UserName));
            var table = UserDA.GetSelectUser(model);
            return UserDA.TableToEntity<T_User>(table);
        }


        public virtual ResultM AddUserGroup(int groupID, string userIDs)
        {
            ResultM result = new ResultM();
            foreach (var item in userIDs.Split(',').Where(z => z.Length > 0))
            {
                TR_User_Group model = new TR_User_Group() { UserID = item, GroupID = groupID };
                result.Affected = result.Affected + UserDA.AddUserGroup(model);
            }
            return result;
        }

        public virtual ResultM DelUserGroup(int groupID, string userIDs)
        {
            ResultM result = new ResultM();
            foreach (var item in userIDs.Split(',').Where(z => z.Length > 0))
            {
                TR_User_Group model = new TR_User_Group() { UserID = item, GroupID = groupID };
                result.Affected = result.Affected + UserDA.Del(model);
            }
            return result;
        }


        #endregion



    }












}
