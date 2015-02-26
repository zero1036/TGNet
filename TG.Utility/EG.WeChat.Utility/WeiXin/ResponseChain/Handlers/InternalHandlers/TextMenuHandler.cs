using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EG.WeChat.Utility.WeiXin.ResponseChain
{
    /// <summary>
    /// 文本菜单的处理器<para />
    /// (允许快速建立"用户输入文本"-"跳转节点"的支持)
    /// </summary>
    [InputDataTypeLimit(DataTypes.Text)]
    [Description("跳轉菜單處理器")]
    public class TextMenuHandler : IHandler
    {

        //---------Members-----------

        #region 状态成员

        public IResponseMessage ReadyMessage { get; set; }

        public IResponseMessage SuccessResponseResult { get; set; }

        public ResponseTextMessage FailResponseResult { get; set; }
        
        #endregion

        #region 菜单项集合

        Dictionary<string, TextMenuItem> MenuItems = new Dictionary<string, TextMenuItem>();

        #endregion


        //---------Stucts------------

        #region 文本菜单项

        /// <summary>
        /// 文本菜单项
        /// </summary>
        public class TextMenuItem
        {
            internal TextFullMatchHandler TextFullMatch;
            internal ResponseJumpNode JumpResult;

            /// <summary>
            /// 隐藏构造函数
            /// </summary>
            private TextMenuItem()
            {}

            /// <summary>
            /// 创建
            /// </summary>
            /// <param name="targetNodeID"></param>
            /// <param name="matchText"></param>
            /// <param name="IgnoreCase"></param>
            /// <returns></returns>
            public static TextMenuItem CreateItem(string targetNodeID,string matchText,bool IgnoreCase = true)
            {
                //参数检查
                if (NodeIdValidator.IsValid(targetNodeID) == false)
                    throw new ArgumentException("节点ID格式不正确", "nodeId");

                TextMenuItem result     = new TextMenuItem();
                result.JumpResult       = new ResponseJumpNode(targetNodeID);
                result.TextFullMatch    = new TextFullMatchHandler(matchText, IgnoreCase);
                return result;
            }
        }

        #endregion


        //---------Control-----------

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        private TextMenuHandler()
        {

        }
        #endregion

        #region 数据处理

        /// <summary>
        /// 数据处理
        /// </summary>
        HandlerResult IHandler.HandlerData(string openId, DataTypes intputType, object rawData)
        {
            //参数检查
            if (rawData == null)
                throw new ArgumentNullException("rawData");

            string text = rawData.ToString();
            if (MenuItems.ContainsKey(text))
            {
                TextMenuItem meunItem = MenuItems[text];
                HandlerResult ret = ((IHandler)meunItem.TextFullMatch).HandlerData(openId, intputType, text);

                if (ret == HandlerResult.Success)
                    this.SuccessResponseResult = meunItem.JumpResult;

                return ret;
            }
            else
            {
                return HandlerResult.Fail;
            }
        }

        #endregion


        //---------Static------------

        #region 创建实例
        /// <summary>
        /// 创建实例
        /// </summary>
        /// <param name="Items"></param>
        /// <returns></returns>
        public static TextMenuHandler CreateInstance(params TextMenuItem[] Items)
        {
            //参数检查
            if (Items == null)
                throw new ArgumentNullException("Items");

            //根据菜单项进行处理
            TextMenuHandler result = new TextMenuHandler();
            foreach (TextMenuItem item in Items)
            {
                string key = item.TextFullMatch.MatchText;

                if (result.MenuItems.ContainsKey(key) == false)
                    result.MenuItems.Add(key,item);
            }

            return result;
        }

        #endregion

    }
}