using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EG.WeChat.Platform.BL.QYMailList;
namespace EG.WeChat.Web.Models.QY
{
    public class NoticeMembersVM
    {
        public int total { get; set; }
        public List<NoticeMember> rows { get; set; }
    }
}