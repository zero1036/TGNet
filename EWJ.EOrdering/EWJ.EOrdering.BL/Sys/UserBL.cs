using EWJ.EOrdering.DA.Sys;
using EWJ.EOrdering.ViewModel.Sys;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWJ.EOrdering.BL.Sys
{
    public class UserBL : BaseBL
    {
        public IList<UserVM> GetList()
        {
            IList<UserVM> list = new List<UserVM>();

            DataSet ds = new UserDA().Get(true);
            list = ds.ToList<UserVM>();

            return list;
        }

        public UserVM GetByAccountPwd(string account, string pwd)
        {
            UserVM model = null;
            DataSet ds = new UserDA().Get(account, pwd);
            IList<UserVM> list = ds.ToList<UserVM>();
            if (list != null && list.Count == 1)
                model = list.FirstOrDefault();

            return model;
        }
    }
}
