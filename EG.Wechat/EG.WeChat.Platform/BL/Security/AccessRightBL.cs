using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EG.WeChat.Model;
using EG.WeChat.Platform.DA;

namespace EG.WeChat.Platform.BL
{
    public class AccessRightBL
    {
        private AccessRightDA _accessRightDA;
        protected AccessRightDA AccessRightDA
        {
            get
            {
                if (_accessRightDA == null)
                {
                    _accessRightDA = new AccessRightDA();
                }
                return _accessRightDA;
            }
        }


        public virtual List<TR_Group_Right> List(int GroupID)
        {
            Hashtable model = new Hashtable();
            model.Add("GroupID", GroupID);
            var result = AccessRightDA.GetRightList(model);
            return result;
        }


        public virtual ResultM Save(int GroupID, string Data)
        {
            ResultM result = new ResultM();

            //删除全部
            int r = AccessRightDA.Del(GroupID);

            //添加
            foreach (var item in Data.Split(';').Where(z=> z.Length > 0))
            {
                var info = item.Split(':');
                foreach (var a in info[1].Split(',').Where(z=> z.Length>0))
                {
                    TR_Group_Right model = new TR_Group_Right() { GroupID = GroupID, Controller= info[0], Action = a };
                    result.Affected += AccessRightDA.Add(model);
                }
            }

            return result;
        }

        




    }
}
