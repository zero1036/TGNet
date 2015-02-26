using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EG.WeChat.Platform.Model;
/*****************************************************
* 目的：微信图文资源配置页面绑定Model
* 创建人：林子聪
* 创建时间：20141201
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Web.Models
{
    public class WXArticlesBinding
    {
        /// <summary>
        /// 显示列数
        /// </summary>
        public int ColumnCount
        {
            get;
            set;
        }
        /// <summary>
        /// 文章列表
        /// </summary>
        public List<WXArticleResultJson> ListArticles
        {
            get;
            set;
        }
    }
}