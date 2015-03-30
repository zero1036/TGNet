using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EG.WeChat.Web.Models.QY
{
    public class VoteResVM
    {
        public string vtype { get; set; }
        public int vnum { get; set; }
        public List<VoteOptResVM> opts { get; set; }
    }
}