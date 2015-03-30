using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EG.WeChat.Platform.Model
{
    public class SendTarget
    {
        public string touser { get; set; }

        public string toparty { get; set; }

        public string totag { get; set; }
    }
}