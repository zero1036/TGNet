using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using EG.WeChat.Model;

namespace EG.WeChat.Web.Models
{
    public class LoginVM
    {
        public LoginVM()
        {
            TemData = new T_User();
        }

        [Required(ErrorMessage = " * Required")]
        public string UserID
        {
            get
            {
                return TemData.UserID;
            }
            set
            {
                TemData.UserID = value;
            }
        }

        [Required(ErrorMessage = " * Required")]
        public string Password
        {
            get
            {
                return TemData.Password;
            }
            set
            {
                TemData.Password = value;
            }
        }

        public T_User TemData { get; set; }

    }



    public class UserVM
    {
        public UserVM()
        {
            TemData = new T_User();
        }


        [Required(ErrorMessage = " * Required")]
        public string UserID
        {
            get
            {
                return TemData.UserID;
            }
            set
            {
                TemData.UserID = value;
            }
        }


        [Required(ErrorMessage = " * Required")]
        public string UserName
        {
            get
            {
                return TemData.UserName;
            }
            set
            {
                TemData.UserName = value;
            }
        }


        [Required(ErrorMessage = " * Required")]
        public string Password
        {
            get
            {
                return TemData.Password;
            }
            set
            {
                TemData.Password = value;
            }
        }


        [Required(ErrorMessage = " * Required")]
        public string ConfirmPassword { get; set; }


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


        public T_User TemData { get; set; }

    }








}