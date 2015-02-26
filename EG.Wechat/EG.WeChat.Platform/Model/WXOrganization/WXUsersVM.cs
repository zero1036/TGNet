using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*****************************************************
* 目的：微信用户实体前台展现VM
* 创建人：林子聪
* 创建时间：20150104
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Platform.Model
{
    public class WXUsersVM : WeChatUser
    {
        /// <summary>
        /// 
        /// </summary>
        public string SexX
        {
            get
            {
                int strBase = base.sex;
                if (strBase == 1)
                {
                    return "男";
                }
                else if (strBase == 2)
                {
                    return "女";
                }
                else if (strBase == 3)
                {
                    return "未知";
                }
                return string.Empty;
            }
        }
    }
}
