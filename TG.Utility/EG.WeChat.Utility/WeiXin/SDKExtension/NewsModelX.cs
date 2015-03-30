using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Senparc.Weixin.MP.AdvancedAPIs.GroupMessage;

namespace EG.WeChat.Utility.WeiXin
{
    /// <summary>
    /// 段落擴展，添加縮略圖相對路徑
    /// </summary>
    public class NewsModelX : NewsModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string RPath
        {
            get;
            set;
        }
    }
}
