using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EG.WeChat.Utility.WeiXin.ResponseChain
{
    /// <summary>
    /// 当输入的文本匹配时，执行指定的结果
    /// </summary>
    [InputDataTypeLimit(DataTypes.Text)]
    [Description("字元匹配處理器")]
    public class TextFullMatchHandler : IHandler
    {

        //---------Members--------

        #region 状态成员

        public IResponseMessage ReadyMessage { get; set; }

        public IResponseMessage SuccessResponseResult { get; set; }

        public ResponseTextMessage FailResponseResult { get; set; }
        
        #endregion

        #region 需要匹配的文字

        /// <summary>
        /// 需要匹配的文字(只读)
        /// </summary>
        public readonly string MatchText;

        /// <summary>
        /// 是否忽略大小写
        /// </summary>
        readonly bool IgnoreCase;

        #endregion


        //---------Control--------

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="matchText">将要进行匹配的文字</param>
        /// <param name="result">结果</param>
        /// <param name="IgnoreCase">是否忽略大小写(默认为True)</param>
        public TextFullMatchHandler(string matchText,bool IgnoreCase = true)
        {
            this.MatchText  = matchText;
            //this.Result     = result;
            this.IgnoreCase = IgnoreCase;
        }

        #endregion

        #region 数据处理

        /// <summary>
        /// 数据处理
        /// </summary>
        HandlerResult IHandler.HandlerData(string openId, DataTypes intputType, object rawData)
        {
            //检查
            if (rawData == null)
                return HandlerResult.Success;

            //是否忽略大小写
            StringComparison targetComparer = this.IgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

            //字符串匹配
            if (this.MatchText.Equals(rawData.ToString(), targetComparer))
                return HandlerResult.Success;
            else
                return HandlerResult.Fail;
        }
        
        #endregion

    }
}