using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EG.WeChat.Model
{
    public class T_User
    {
        [Column(IsPrimaryKey = true)]
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int State { get; set; }

        public string CreatedBy { get; set; }
        public DateTime? CreatedTime { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedTime { get; set; }

        public bool IsDeleted { get; set; }

        public string DeletedBy { get; set; }
        public DateTime? DeletedTime { get; set; }
    }


    public class TR_User_Group
    {
        [Column(IsPrimaryKey = true)]
        public int GroupID { get; set; }
        [Column(IsPrimaryKey = true)]
        public string UserID { get; set; }
    }


    public enum Level
    {
        /// <summary>
        /// 判断用户是否登录，并且拥有该功能的访问权限。
        /// </summary>
        OneLevel = 1,

        /// <summary>
        /// 判断用户是否登录
        /// </summary>
        TwoLevel = 2
    }

    public class AccessRight
    {
        public string Controller { get; set; }
        public string Action { get; set; }
    }






}
