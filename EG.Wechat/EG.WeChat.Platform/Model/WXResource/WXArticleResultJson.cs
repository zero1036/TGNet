﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using EG.WeChat.Utility.WeiXin;
/*****************************************************
* 目的：EG定制微信图文资源ResultJson
* 创建人：林子聪
* 创建时间：20141124
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Platform.Model
{
    /// <summary>
    /// 圖文生成結果
    /// </summary>
    public class WXArticleResultJson : LCResultJon
    {
        /// <summary>
        /// 
        /// </summary>
        public bool byLink
        {
            get;
            set;
        }
        /// <summary>
        /// 段落集合
        /// </summary>
        public List<NewsModelX> ListNews
        {
            get;
            set;
        }
    }
}
