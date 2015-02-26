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
    public class GroupBL
    {
        private GroupDA _groupDA;
        protected GroupDA GroupDA
        {
            get
            {
                if (_groupDA == null)
                {
                    _groupDA = new GroupDA();
                }
                return _groupDA;
            }
        }

        #region 组

        public virtual PagingM List(string GroupID, string GroupName, int pageIndex)
        {
            Hashtable model = new Hashtable();
            model.Add("GroupID", string.Format("%{0}%", GroupID));
            model.Add("GroupName", string.Format("%{0}%", GroupName));

            return GroupDA.GetPageList(model, pageIndex);
        }


        public virtual ResultM New(T_Group model, string userID)
        {
            ResultM result = new ResultM();
            model.CreatedBy = userID;
            model.LastModifiedBy = userID;
            result.Affected = GroupDA.Add(model);

            return result;
        }


        public virtual ResultM Edit(T_Group model, string userID)
        {
            ResultM result = new ResultM();

            T_Group data = GetGroup(model.GroupID);

            data.GroupName = model.GroupName;
            data.Description = model.Description;
            data.State = model.State;
            data.LastModifiedBy = userID;
            data.LastModifiedTime = DateTime.Now;

            result.Affected = GroupDA.Edit(data);

            return result;
        }


        public virtual ResultM Delete(int groupID, string currentUserID)
        {
            ResultM result = new ResultM();
            T_Group data = GetGroup(groupID);
            data.IsDeleted = true;
            data.DeletedBy = currentUserID;
            data.DeletedTime = DateTime.Now;
            result.Affected = GroupDA.Edit(data);
            return result;
        }


        public virtual T_Group GetGroup(int groupID)
        {
            T_Group result = GroupDA.GetByPKey(new T_Group() {  GroupID = groupID , IsDeleted = false });
            return result;
        }

        
        #endregion

        


    }
}
