using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EG.WeChat.Web.Models.QY
{
    public class DepartMenuVM
    {
        public long DepPKID { get; set; }

        public string wcOrder { get; set; }

        public string Name { get; set; }

        public string DepartmentID { get; set; }

        public string ParentDepartmentID { get; set; }
    }
}