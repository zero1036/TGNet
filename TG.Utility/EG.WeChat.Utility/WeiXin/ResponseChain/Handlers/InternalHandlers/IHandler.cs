using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EG.WeChat.Utility.WeiXin.ResponseChain
{
    /// <summary>
    /// 处理器的接口
    /// </summary>
    public interface IHandler
    {
        /// <summary>
        /// 准备就绪的处理
        /// </summary>
        IResponseMessage ReadyMessage {get;set;}

        /// <summary>
        /// 处理数据
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="intputType"></param>
        /// <param name="rawData"></param>
        /// <returns></returns>
        HandlerResult HandlerData(string openId, DataTypes intputType, object rawData);

        /// <summary>
        /// 成功时的结果
        /// </summary>
        IResponseMessage SuccessResponseResult { get; set; }

        /// <summary>
        /// 失败时的结果(限定为文本提示)
        /// </summary>
        ResponseTextMessage FailResponseResult { get; set; }
    }
}