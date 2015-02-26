using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*****************************************************
* 目的：ResultJson擴展
* 创建人：林子聪
* 创建时间：20150129
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Platform.Model
{
    public class WXResultJson : IWXResultJon1
    {
        /// <summary>
        /// 創建時間
        /// </summary>
        public virtual long created_at
        {
            get;
            set;
        }
        /// <summary>
        /// 微信回馈Media ID
        /// </summary>
        public virtual string media_id
        {
            get;
            set;
        }
    }
}
