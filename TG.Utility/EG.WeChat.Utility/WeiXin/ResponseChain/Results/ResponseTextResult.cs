using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EG.WeChat.Utility.WeiXin.ResponseChain
{
    /// <summary>
    /// 文本类型 响应结果
    /// </summary>
    public class ResponseTextMessage : IResponseMessage
    {
        /// <summary>
        /// 文本内容
        /// </summary>
        public string Context = "";

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context">文本内容</param>
        public ResponseTextMessage(string context)
        {
            this.Context = context;
        }
    }
}