using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EG.WeChat.Model.SecurityM;

namespace EG.WeChat.Web.Models.Security
{
    public class MenuVM
    {
        public MenuVM()
        {
            TemData = new T_Menu();
        }

        public string accessPath
        {
            get
            {
                return TemData.Href;
            }
            set
            {
                TemData.Href = value;
            }
        }

        public int delFlag { get; set; }

        public string parentID
        {
            get
            {
                return TemData.FatherCode;
            }
            set
            {
                TemData.FatherCode = value;
            }
        }

        public string resourceCode { get; set; }

        public string resourceDesc
        {
            get
            {
                return TemData.Description;
            }
            set
            {
                TemData.Description = value;
            }
        }

        public string resourceGrade { get; set; }

        public string resourceID
        {
            get
            {
                return TemData.Code;
            }
            set
            {
                TemData.Code = value;
            }
        }

        public string resourceName
        {
            get
            {
                return TemData.Name;
            }
            set
            {
                TemData.Name = value;
            }
        }

        public int resourceOrder
        {
            get
            {
                return TemData.Sort;
            }
            set
            {
                TemData.Sort = value;
            }
        }

        public string resourceType { get; set; }

        public T_Menu TemData { get; set; }

    }
}