using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EG.WeChat.Web.Models
{
    /// <summary>
    /// 网站配置的Model
    /// </summary>
    public class WebConfigVM
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        [Description("数据库类型")]
        public DBType DB_TYPE { get; set; }

        /// <summary>
        /// 数据库用户名
        /// </summary>
        [Description("数据库地址")]
        public string DB_IP { get; set; }

        /// <summary>
        /// 数据库端口
        /// </summary>
        [Description("数据库端口")]
        public string DB_PORT { get; set; }

        /// <summary>
        /// 库名称
        /// </summary>
        [Description("库名称")]
        public string DB_DATABASE { get; set; }

        /// <summary>
        /// 数据库用户名
        /// </summary>
        [Description("数据库用户名")]
        [EncryptField]
        public string DB_USER { get; set; }

        /// <summary>
        /// 数据库密码
        /// </summary>
        [Description("数据库密码")]
        [EncryptField]
        [DataType(DataType.Password)]
        public string DB_PSW { get; set; }

        /// <summary>
        /// 微信AppId
        /// </summary>
        [Description("微信AppId")]
        [EncryptField]
        public string WX_appID { get; set; }

        /// <summary>
        /// 微信AppSecret
        /// </summary>
        [Description("微信AppSecret")]
        [EncryptField]
        public string WX_appsecret { get; set; }

        /// <summary>
        /// 微信Token
        /// </summary>
        [Description("微信Token")]
        [EncryptField]
        public string WX_Token { get; set; }


        #region 其它类型

        /// <summary>
        /// 数据库类型枚举
        /// </summary>
        public enum DBType:byte
        {
            Oracle      = 0,
            SqlServer   = 1,
            MySql       = 5,
            Xml         = 9
        }

        #endregion
    }


    /// <summary>
    /// 标记字段是否需要加密处理
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class EncryptFieldAttribute : Attribute
    {
        /// <summary>
        /// 是否需要加密（只读）
        /// </summary>
        public readonly bool NeedEncrypt;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="needEncrypt">默认为“需要加密”</param>
        public EncryptFieldAttribute(bool needEncrypt = true)
        {
            this.NeedEncrypt = needEncrypt;
        }
    }

}