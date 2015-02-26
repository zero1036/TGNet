using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using EG.WeChat.Model;


namespace EG.WeChat.Web.Models
{
    public class GroupVM
    {
        public GroupVM()
        {
            TemData = new T_Group();
        }


        [Required(ErrorMessage = " * Required")]
        public int GroupID
        {
            get
            {
                return TemData.GroupID;
            }
            set
            {
                TemData.GroupID = value;
            }
        }


        [Required(ErrorMessage = " * Required")]
        public string GroupName
        {
            get
            {
                return TemData.GroupName;
            }
            set
            {
                TemData.GroupName = value;
            }
        }


        public string Description
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


        public int State
        {
            get
            {
                return TemData.State;
            }
            set
            {
                TemData.State = value;
            }
        }


        public T_Group TemData { get; set; }


    }
}