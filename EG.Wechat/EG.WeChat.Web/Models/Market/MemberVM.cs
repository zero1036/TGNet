using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using EG.WeChat.Platform.Model;

namespace EG.WeChat.Web.Models
{
    public class MemberVM
    {
        public MemberVM()
        {
            TemData = new T_Member();
        }

        [Required(ErrorMessage = " * Required")]
        public string Name
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

        [Required(ErrorMessage = " * Required")]
        public string Phone
        {
            get
            {
                return TemData.Phone;
            }
            set
            {
                TemData.Phone = value;
            }
        }

        [Required(ErrorMessage = " * Required")]
        public string Mail
        {
            get
            {
                return TemData.Mail;
            }
            set
            {
                TemData.Mail = value;
            }
        }

        public string OpenID
        {
            get
            {
                return TemData.OpenID;
            }
            set
            {
                TemData.OpenID = value;
            }
        }

        public T_Member TemData { get; set; }
    }






}