using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Senparc.Weixin.MP.AdvancedAPIs;
/*****************************************************
* 目的：微信图文资源配置页面绑定Model——单个段落
* 创建人：林子聪
* 创建时间：20141127
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Web.Models
{
    public class WXArticleBinding
    {
        public int iTest
        {
            get;
            set;
        }
        /// <summary>
        /// 段落集合
        /// </summary>
        public List<NewsModel> ListNews
        {
            get;
            set;
        }
    }
}