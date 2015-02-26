using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EG.WeChat.Utility.WeiXin.ResponseChain
{
    /// <summary>
    /// 文章类型 响应结果
    /// </summary>
    public class ResponseNewsResult : IResponseMessage
    {

        //--------Members---------

        #region 文章集合
        
        /// <summary>
        /// 文章集合
        /// </summary>
        public readonly ArticleCan[] ArticleList;

        #endregion


        //--------Control---------

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public ResponseNewsResult(params ArticleCan[] list)
        {
            this.ArticleList = list;
        }

        #endregion
        
    }
}