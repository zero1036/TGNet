using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*****************************************************
* 目的：本地資源上傳結果result接口
* 创建人：林子聪
* 创建时间：20150129
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
    public interface IFileUploadResult
    {
        /// <summary>
        /// 文件名稱
        /// </summary>
        string FileName
        {
            get;
            set;
        }
        /// <summary>
        /// 格式
        /// </summary>
        string FormatName
        {
            get;
            set;
        }
        /// <summary>
        /// 相對路徑
        /// </summary>
        string RPath
        {
            get;
            set;
        }
        /// <summary>
        /// 絕對路徑
        /// </summary>
        string APath
        {
            get;
            set;
        }
        /// <summary>
        /// 縮略圖相對路徑
        /// </summary>
        string RPathThumb
        {
            get;
            set;
        }
    }
}
