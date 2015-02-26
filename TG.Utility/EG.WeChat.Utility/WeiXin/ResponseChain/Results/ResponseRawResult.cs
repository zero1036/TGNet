using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EG.WeChat.Utility.WeiXin.ResponseChain
{
    /* 故意破坏封闭性。
     * 通过这种方式，允许由 自定义处理器，返回自己包含的数据。
     * 
     * 但是最终是否支持，需要对应回CustomMessageHandler的处理。
     * 请具体的业务，关注回CustomMessageHandler的处理。
     */

    /// <summary>
    /// Raw格式的 响应结果
    /// </summary>
    public class ResponseRawResult : IResponseMessage
    {
        /// <summary>
        /// 文本内容
        /// </summary>
        public object RawData = "";

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context">文本内容</param>
        public ResponseRawResult(object RawData)
        {
            this.RawData = RawData;
        }
    }
}