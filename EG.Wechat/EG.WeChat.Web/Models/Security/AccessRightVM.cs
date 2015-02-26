using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EG.WeChat.Model;

namespace EG.WeChat.Web.Models.Security
{
    public class AccessRightVM
    {
        public AccessRightVM()
        {
            TemData = new TR_Group_Right();
        }

        public string Controller
        {
            get
            {
                return TemData.Controller;
            }
        }

        public string Action
        {
            get
            {
                return TemData.Action;
            }
        }

        public bool HaveRight
        {
            get
            {
                return TemData.GroupID != null;
            }
        }

        public TR_Group_Right TemData { get; set; }

    }
}