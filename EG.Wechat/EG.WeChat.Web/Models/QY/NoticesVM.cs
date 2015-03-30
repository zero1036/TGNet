using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EG.WeChat.Web.Models.QY
{
    public class NoticesVM
    {
        public string Title { get; set; }
        public string id { get; set; }
        public string Context { get; set; }
        public string CreateUserName { get; set; }
        public string CreateDate { get; set; }
        public string Status { get; set; }
        public string showbtn { get; set; }
        public string delbtn { get; set; }

    }
}