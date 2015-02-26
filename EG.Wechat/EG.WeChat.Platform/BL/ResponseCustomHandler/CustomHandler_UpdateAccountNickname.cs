using EG.WeChat.Utility.WeiXin.ResponseChain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace EG.WeChat.Platform.BL
{
    /// <summary>
    /// 更新交易账户昵称 
    /// </summary>
    [InputDataTypeLimit(DataTypes.Text)]
    [Description("更新交易帳戶昵稱")]
    public class CustomHandler_UpdateAccountNickname : ICustomHandler
    {

        //---------Members--------

        #region 状态成员

        public IResponseMessage ReadyMessage
        {
            get
            {
                return new ResponseTextMessage("請輸入6位元長度文字的昵稱。");
            }
            //忽略设置
            set { }
        }
        public IResponseMessage SuccessResponseResult { get; set; }
        public ResponseTextMessage FailResponseResult { get; set; }
        
        #endregion


        //---------Control--------

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public CustomHandler_UpdateAccountNickname()
        {
        }
        
        #endregion

        #region 处理数据

        /// <summary>
        /// 处理数据
        /// </summary>
        public HandlerResult HandlerData(string openId, DataTypes intputType, object rawData)
        {
            if (rawData == null || rawData is string == false)
            {
                this.FailResponseResult = new ResponseTextMessage("您剛才輸入的資訊不是文字類型。");
                return HandlerResult.Fail;
            }
            else if (rawData.ToString().Length > 6)
            {
                this.FailResponseResult = new ResponseTextMessage("您剛才輸入的文字超過6位元長度。");
                return HandlerResult.Fail;
            }
            else
            {
                this.SuccessResponseResult = new ResponseTextMessage(String.Format("設置成功，新的昵稱為：{0}", rawData));
                return HandlerResult.Success;
            }
        }

        #endregion

    }
}