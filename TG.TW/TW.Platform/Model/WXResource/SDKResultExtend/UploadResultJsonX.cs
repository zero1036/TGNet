using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TW.Platform.Model;
//using Senparc.Weixin.MP.AdvancedAPIs;
//using Senparc.Weixin.MP.AdvancedAPIs.Media;
//using Senparc.Weixin.MP.CommonAPIs;
/*****************************************************
* 目的：UploadResultJsonX 擴展
* 创建人：林子聪
* 创建时间：20141129
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace Senparc.Weixin.MP.AdvancedAPIs.Media
{
    public class UploadResultJsonX : WXResultJson, IWXResultJon2
    {
        /// <summary>
        /// 創建時間
        /// </summary>
        public override long created_at
        {
            get;
            set;
        }
        /// <summary>
        /// 微信回馈Media ID
        /// </summary>
        public override string media_id
        {
            get;
            set;
        }
        private UploadResultJson _UploadResultJson;
        /// <summary>
        /// 
        /// </summary>
        public UploadResultJson UploadResultJson
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public void SetUploadResultJson(object obj)
        {
            if (obj is UploadResultJson)
            {
                this.UploadResultJson = (UploadResultJson)obj;
            }
        }
    }
}
namespace Senparc.Weixin.QY.AdvancedAPIs.Media
{
    public class UploadResultJsonX : WXResultJson, IWXResultJon2
    {
        /// <summary>
        /// 創建時間
        /// </summary>
        public override long created_at
        {
            get;
            set;
        }
        /// <summary>
        /// 微信回馈Media ID
        /// </summary>
        public override string media_id
        {
            get;
            set;
        }
        private UploadResultJson _UploadResultJson;
        /// <summary>
        /// 
        /// </summary>
        public UploadResultJson UploadResultJson
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public void SetUploadResultJson(object obj)
        {
            if (obj is UploadResultJson)
            {
                this.UploadResultJson = (UploadResultJson)obj;
            }
        }
    }
}
