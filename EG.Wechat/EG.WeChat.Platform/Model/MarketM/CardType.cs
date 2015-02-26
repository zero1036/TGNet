using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*****************************************************
* 目的：本地会员卡类型模型
* 创建人：林子聪
* 创建时间：20141215
* 备注：服务暂未实现
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Platform.Model
{
    /// <summary>
    /// 
    /// </class>
    public class CardType
    {
        /// <summary>
        /// 会员卡编码
        /// </summary>
        public int CardID
        {
            get;
            set;
        }
        /// <summary>
        /// 会员卡名称
        /// </summary>
        public string Name
        {
            get;
            set;
        }
    }
}
