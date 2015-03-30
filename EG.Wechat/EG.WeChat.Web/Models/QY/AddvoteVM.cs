using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EG.WeChat.Web.Models.QY
{
    public class AddvoteVM
    {
        public string id { get; set; }
        public string title { get; set; }
        public string context { get; set; }
        public string vtype { get; set; }
        public List<VoteOptionsVM> options { get; set; }
    }
}