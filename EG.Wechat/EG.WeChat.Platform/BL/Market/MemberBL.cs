using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using EG.WeChat.Model;
using EG.WeChat.Platform.DA;
using EG.WeChat.Platform.Model;

namespace EG.WeChat.Platform.BL
{
    public class MemberBL
    {
        private MemberDA _memberDA;
        protected MemberDA MemberDA
        {
            get
            {
                if (_memberDA == null)
                {
                    _memberDA = new MemberDA();
                }
                return _memberDA;
            }
        }


        #region 客戶端功能

        public virtual ResultM Register(T_Member model)
        {
            ResultM result = new ResultM();
            model.ID = DateTime.Now.Ticks.ToString();
            model.CreatedBy = model.OpenID;
            model.LastModifiedBy = model.OpenID; 
            result.Affected = MemberDA.Add(model);
            return result;
        }


        public virtual ResultM Cancellation(string ID, string openID)
        {
            ResultM result = new ResultM();
            T_Member data = MemberDA.GetByPKey(new T_Member() { ID = ID, OpenID = openID, IsDeleted = false });
            data.IsDeleted = true;
            data.DeletedBy = openID;
            data.DeletedTime = DateTime.Now;
            result.Affected = MemberDA.Edit(data);
            return result;
        }



        public virtual List<T_Member> GetMembers(string openID)
        {
            Hashtable model = new Hashtable();
            model.Add("OpenID", openID);
            return MemberDA.GetMemberList(model);
        }

        #endregion


        #region 服務端功能


        public virtual PagingM List(string Name, int pageIndex)
        {
            Hashtable model = new Hashtable();
            model.Add("Name", string.Format("%{0}%", Name));

            return MemberDA.GetPageList(model, pageIndex);
        }


        #endregion

    }
}
