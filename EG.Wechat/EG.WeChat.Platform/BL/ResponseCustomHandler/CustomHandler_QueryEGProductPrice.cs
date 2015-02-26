using EG.WeChat.Utility.WeiXin.ResponseChain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace EG.WeChat.Platform.BL
{
    /// <summary>
    /// 查询EG的产品即时现价
    /// </summary>
    [InputDataTypeLimit(DataTypes.Text)]
    [Description("查詢英皇的產品即時現價")]
    public class CustomHandler_QueryEGProductPrice : ICustomHandler
    {

        //---------Members--------

        #region 状态成员

        public IResponseMessage ReadyMessage
        {
            get
            {
                StringBuilder result = new StringBuilder();
                result.Append("请输入以下指令(数字)：");
                foreach (string key in RelationDic.Keys)
                {
                    result.Append("\r\n");
                    result.Append(key);
                    result.Append(" 查詢");
                    result.Append(RelationDic[key]);
                }

                return new ResponseTextMessage(result.ToString());
            }
            //忽略设置
            set { }
        }
        public IResponseMessage SuccessResponseResult { get; set; }
        public ResponseTextMessage FailResponseResult { get; set; }

        #endregion

        #region 指令列表

        /*此Hanlder自己Hold住关系，比如：  
         * 
         1 - LLG
         2 - LLS
         3 - HKG
         */

        //Dictionary<string, ProductId>
        Dictionary<string, string> RelationDic = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        
        #endregion


        //---------Control--------

        #region 构造函数
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public CustomHandler_QueryEGProductPrice()
        {
            RelationDic.Add("1", "LLG");
            RelationDic.Add("2", "LLS");
            RelationDic.Add("3", "HKG");
        }

        #endregion

        #region 处理数据
 
        /// <summary>
        /// 处理数据
        /// </summary>
        public HandlerResult HandlerData(string openId, DataTypes intputType, object rawData)
        {
            if (rawData == null)
                return HandlerResult.Fail;

            string targetKey = rawData.ToString();

            if (this.RelationDic.ContainsKey(targetKey))
            {
                Random randomer = new Random();
                string result = String.Format("{0} {1:00000}/{2:00}", RelationDic[targetKey], randomer.Next(1, 69999), randomer.Next(0, 99));

                this.SuccessResponseResult = new ResponseTextMessage(result);
                return HandlerResult.Success;
            }
            else
            {
                return HandlerResult.Fail;
            }
        }

        #endregion

    }
}