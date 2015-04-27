using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*****************************************************
* 目的：EG定制微信音頻资源ResultJson
* 创建人：林子聪
* 创建时间：20150129
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace TW.Platform.Model
{
    public class WXVoiceResultJson : LCResultJon, IFileUploadResult
    {
        /// <summary>
        /// 文件名稱
        /// </summary>
        public string FileName
        {
            get;
            set;
        }
        /// <summary>
        /// 格式
        /// </summary>
        public string FormatName
        { get; set; }
        /// <summary>
        /// 相對路徑
        /// </summary>
        public string RPath
        {
            get;
            set;
        }
        /// <summary>
        /// 絕對路徑
        /// </summary>
        public string APath
        {
            get;
            set;
        }
        /// <summary>
        /// 縮略圖相對路徑
        /// </summary>
        public string RPathThumb
        { get; set; }
        /// <summary>
        /// 時長
        /// </summary>
        public int VLength
        { get; set; }
    }
}
