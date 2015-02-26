using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EG.WeChat.Platform.BL;
using EG.WeChat.Platform.Model;
using Senparc.Weixin.MP.AdvancedAPIs;
/*****************************************************
* 目的：模板消息页面（用户管理页面、分组管理页面）——绑定模板
* 创建人：林子聪
* 创建时间：20141107
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Web.Models
{
    public class WXMessageBinding
    {
        /// <summary>
        /// 微信用户列表
        /// </summary>
        public List<WeChatUser> UserList
        {
            get;
            set;
        }
        /// <summary>
        /// 微信分组列表
        /// </summary>
        public List<GroupsJson_Group> GroupList
        {
            get;
            set;
        }
    }
}