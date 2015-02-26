using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace EG.WeChat.Platform.Model
{

    public class T_Member
    {
        /// <summary>
        /// 卡号
        /// </summary>
        [Column(IsPrimaryKey = true)]
        public string ID { get; set; }
        /// <summary>
        /// 微信OpenID
        /// </summary>
        public string OpenID { get; set; }
        /// <summary>
        /// 会员姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 邮件
        /// </summary>
        public string Mail { get; set; }
        /// <summary>
        /// 类型（例如：金，银，铜，红，白，蓝等。）
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public int Integral { get; set; }
        /// <summary>
        /// 存款（余额）
        /// </summary>
        public decimal Deposit { get; set; }
        /// <summary>
        /// 折扣
        /// </summary>
        //public decimal Discount { get; set; }
        /// <summary>
        /// 状态（页面一般不会显示，根据实际情况使用。）
        /// </summary>
        public int State { get; set; }

        public string CreatedBy { get; set; }
        public DateTime? CreatedTime { get; set; }
        public string LastModifiedBy { get; set; }
        /// <summary>
        /// 最后一次变更时间。（如：积分和存款的加减）
        /// </summary>
        public DateTime? LastModifiedTime { get; set; }
        public bool IsDeleted { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedTime { get; set; }
    }

}
