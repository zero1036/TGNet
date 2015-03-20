using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EG.WeChat.Web.Models.QY
{
    public class MemberVM
    {
        public long MemberID { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Weixinid { get; set; }
        public string Status { get; set; }
        public string Avatar { get; set; }
        public string DepartPKId { get; set; }
    }
}