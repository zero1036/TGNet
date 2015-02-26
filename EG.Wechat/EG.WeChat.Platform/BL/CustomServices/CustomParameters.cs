using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*****************************************************
* 目的：定制服务，指定参数名称
* 创建人：林子聪
* 创建时间：20141216
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Platform.BL
{
    public static class CustomParameters
    {
        /// <summary>
        /// 微信会员OpenID
        /// </summary>
        public static readonly string OPENID = "OPENID";
        /// <summary>
        /// 会员卡类型编码
        /// </summary>
        public static readonly string CARDTYPE = "CARDTYPE";
    }
}
