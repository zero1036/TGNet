using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*****************************************************
* 目的：資源元數據類型枚舉
* 创建人：林子聪
* 创建时间：20150203
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Platform.Model
{
    public enum EnumWXSourceType
    {
        /// <summary>
        /// 本地數據
        /// </summary>
        local = 1,
        /// <summary>
        /// 微信接收用戶數據
        /// </summary>
        wechatLoad = 2
    }
}
