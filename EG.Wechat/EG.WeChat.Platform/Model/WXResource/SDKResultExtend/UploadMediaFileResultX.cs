using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Senparc.Weixin.MP.Entities;
/*****************************************************
* 目的：UploadMediaFileResult擴展
* 创建人：林子聪
* 创建时间：20150129
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Platform.Model
{
    public class UploadMediaFileResultX : WXResultJson, IWXResultJon2<UploadMediaFileResult>
    {
        private UploadMediaFileResult _UploadResultJson;
        /// <summary>
        /// 
        /// </summary>
        public UploadMediaFileResult UploadResultJson
        {
            get
            {
                return _UploadResultJson;
            }
            set
            {
                _UploadResultJson = value;
                this.media_id = value.media_id;
                this.created_at = value.created_at;
            }
        }
    }
}
