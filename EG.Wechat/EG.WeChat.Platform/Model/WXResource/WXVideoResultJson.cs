using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.CommonAPIs;
using System.Runtime.Serialization;
/*****************************************************
* 目的：EG定制微信視頻资源ResultJson
* 创建人：林子聪
* 创建时间：20150127
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
    public class WXVideoResultJson : LCResultJon, IFileUploadResult
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
    }
}
