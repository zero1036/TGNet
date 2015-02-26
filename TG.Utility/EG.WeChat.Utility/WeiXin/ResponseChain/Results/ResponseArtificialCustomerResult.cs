using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EG.WeChat.Utility.WeiXin.ResponseChain
{
    /// <summary>
    /// 人工客服 响应结果
    /// </summary>
    public class ResponseArtificialCustomerResultMessage : IResponseMessage
    {
        /// <summary>
        /// 文本内容
        /// </summary>
        public string[] AccountList = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context">文本内容</param>
        public ResponseArtificialCustomerResultMessage(string[] accountList = null)
        {
            this.AccountList = accountList;
        }
    }
}