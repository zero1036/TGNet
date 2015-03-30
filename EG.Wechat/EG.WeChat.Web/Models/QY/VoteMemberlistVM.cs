using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EG.WeChat.Web.Models.QY
{
    public class VoteMemberlistVM
    {
        public int total { get; set; }
        public List<VoteMemberVM> rows { get; set; }
    }
}