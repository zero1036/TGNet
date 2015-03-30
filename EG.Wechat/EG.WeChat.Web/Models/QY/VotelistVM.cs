using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EG.WeChat.Web.Models.QY
{
    public class VotelistVM
    {
       public int total { get; set; }
        public List<VoteVM> rows { get; set; }
    }
}