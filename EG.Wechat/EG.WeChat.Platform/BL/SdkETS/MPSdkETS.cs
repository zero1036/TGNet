using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.Media;
using Senparc.Weixin.MP.CommonAPIs;
using System.Net;
using System.Web;
using EG.WeChat.Service;
using EG.WeChat.Utility.Tools;
using EG.WeChat.Utility.WeiXin;
using EG.WeChat.Platform.Model;

namespace EG.WeChat.Platform.BL
{
    /// <summary>
    /// 
    /// </summary>
    public class MPSdkETS : CommonSdkETS, ISdkETS_MP, ISdkETS
    {
        #region Singleon
        public static MPSdkETS Singleon = new MPSdkETS();
        #endregion

        #region Base
        /// <summary>
        /// 微信账号类型：1：公众号；2：企业号
        /// </summary>
        public int WXType { get { return 1; } }
        /// <summary>
        /// RHost
        /// </summary>
        public string RHost { get; set; }
        /// <summary>
        /// 获取AccessToken
        /// </summary>
        public Func<string> GetAToken
        {
            get { return WeiXinSDKExtension.GetCurrentAccessToken; }
        }
        #endregion

        #region Media
        /// <summary>
        /// 媒体上传方法
        /// </summary>
        public Func<string, string, string, int, object> MediaUpload
        {
            get { return Senparc.Weixin.MP.AdvancedAPIs.Media.MediaApiX.Upload; }
        }
        ///// <summary>
        ///// 获取媒体类型
        ///// </summary>
        ///// <returns></returns>
        //public object GetUploadMediaFileType()
        //{
        //    return Senparc.Weixin.MP.UploadMediaFileType.image;
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Func<NewsModelX, object> CNews2SendArticle(string type)
        {
            var dic = GetDicNewsModel();
            if (dic.ContainsKey(type))
            {
                return dic[type];
            }
            return null;
        }
        /// <summary>
        /// 图文消息实体转换委托——NewsModel转换被动回复Article
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Func<NewsModelX, object> CNews2RspArticle(string type)
        {
            return null;
        }
        /// <summary>
        /// 获取素材转换方法
        /// </summary>
        /// <param name="msgType"></param>
        /// <returns></returns>
        public Func<object, string, object> GetResConvert(string msgType)
        {
            return base.GetDicResourceConvert(this.WXType, msgType, this.CNews2SendArticle(msgType));
        }
        #endregion
    }
}
