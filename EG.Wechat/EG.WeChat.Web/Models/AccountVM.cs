using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using EG.WeChat.Business.JR.Model;
using EG.WeChat.Model;

namespace EG.WeChat.Web.Models
{
    public class AccountVM 
    {
        public AccountVM()
        {
            TemData = new ODS_Account();
        }

        [Required(ErrorMessage = " * Required")]
        //[RegularExpression("\\A\\d{8}\\z", ErrorMessage = " * must be alphaNumeric 8 digit")]
        public string AccountNumber
        {
            get
            {
                return TemData.AccountNumber;
            }
            set
            {
                TemData.AccountNumber = value;
            }
        }

        [Required(ErrorMessage = " * Required")]
        //[RegularExpression("^[2]", ErrorMessage = " * must start with 2 ")]
        public string Password {
            get
            {
                return TemData.Password;
            }
            set
            {
                TemData.Password = value;
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

        public ODS_Account TemData { get; set; }

    }


    public class ChangeAccountVM 
    {
        public ChangeAccountVM()
        {
            TemData = new ODS_Account();
        }

        [Required(ErrorMessage = " * Required")]
        public string OldPassword { get; set; }

        public string OldAccountNumber { get; set; }

        [Required(ErrorMessage = " * Required")]
        //[RegularExpression("\\A\\d{8}\\z", ErrorMessage = " * must be alphaNumeric 8 digit")]
        public string AccountNumber
        {
            get
            {
                return TemData.AccountNumber;
            }
            set
            {
                TemData.AccountNumber = value;
            }
        }

        [Required(ErrorMessage = " * Required")]
        //[RegularExpression("^[2]", ErrorMessage = " * must start with 2 ")]
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

        public ODS_Account TemData { get; set; }
    }


 



}