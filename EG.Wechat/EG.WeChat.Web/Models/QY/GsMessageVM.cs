using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EG.WeChat.Web.Models
{
    public class GsMessageVM
    {
        public string ToTarget { get; set; }

        public string MsgType { get; set; }

        public string MediaId { get; set; }

        public string Content { get; set; }

        public int AgentId { get; set; }

        public int Safe { get; set; }

        public int FuncID { get; set; }

        public int FuncType { get; set; }
    }
}