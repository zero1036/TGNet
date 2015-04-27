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
namespace TW.Platform.Model
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
    /// <summary>
    /// 
    /// </summary>
    public class WXResultJsonWithCvn : WXResultJson, IWXResultJon2
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public void SetUploadResultJson(object obj)
        {
            if (obj is Senparc.Weixin.QY.AdvancedAPIs.Media.UploadResultJson)
            {
                var rj = (Senparc.Weixin.QY.AdvancedAPIs.Media.UploadResultJson)obj;
                this.media_id = rj.media_id;
                this.created_at = rj.created_at;
            }
            else if (obj is Senparc.Weixin.MP.AdvancedAPIs.Media.UploadResultJson)
            {
                var rj = (Senparc.Weixin.MP.AdvancedAPIs.Media.UploadResultJson)obj;
                this.media_id = rj.media_id;
                this.created_at = rj.created_at;
            }
            else if (obj is Senparc.Weixin.MP.Entities.UploadMediaFileResult)
            {
                var rj = (Senparc.Weixin.MP.Entities.UploadMediaFileResult)obj;
                this.media_id = rj.media_id;
                this.created_at = rj.created_at;
            }
        }
    }
}
