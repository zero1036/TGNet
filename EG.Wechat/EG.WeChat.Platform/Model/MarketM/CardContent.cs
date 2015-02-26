using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*****************************************************
* 目的：本地会员卡内容模板模型
* 创建人：林子聪
* 创建时间：20141215
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Platform.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class CardContent
    {
        /// <summary>
        /// 会员卡内容编号
        /// </summary>
        public string MediaID
        {
            get;
            set;
        }
        /// <summary>
        /// 会员卡编码
        /// </summary>
        public int CardID
        {
            get;
            set;
        }
        /// <summary>
        /// 会员卡显示信息段落集合
        /// </summary>
        public List<CContent> ListCardInfo
        {
            get;
            set;
        }
        /// <summary>
        /// 会员卡图片路径
        /// </summary>
        public string ImgPath
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public class CContent
        {
            /// <summary>
            /// 标题3
            /// </summary>
            public string Title
            {
                get;
                set;
            }
            /// <summary>
            /// 说明
            /// </summary>
            public string Desc
            {
                get;
                set;
            }
            /// <summary>
            /// 内容编号
            /// </summary>
            public string ContentID
            {
                get;
                set;
            }
            /// <summary>
            /// 内容文本
            /// </summary>
            public string Content
            {
                get;
                set;
            }
        }
    }
}
