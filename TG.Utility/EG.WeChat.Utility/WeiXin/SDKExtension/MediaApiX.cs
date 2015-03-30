using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*****************************************************
* 目的：扩展SDK MediaApi，避免使用混淆，命名空间与SDK保持一致
* 创建人：林子聪
* 创建时间：20141124
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace Senparc.Weixin.QY.AdvancedAPIs.Media
{
    public class MediaApiX
    {
        /// <summary>
        /// 上传媒体文件
        /// </summary>
        /// <param name="accessToken">调用接口凭证</param>
        /// <param name="type">媒体文件类型，分别有图片（image）、语音（voice）、视频（video），普通文件(file)</param>
        /// <param name="media">form-data中媒体文件标识，有filename、filelength、content-type等信息</param>
        /// <param name="timeOut">代理请求超时时间（毫秒）</param>
        /// <returns></returns>
        public static Senparc.Weixin.QY.AdvancedAPIs.Media.UploadResultJson Upload(string accessToken, string type, string media, int timeOut = Senparc.Weixin.Config.TIME_OUT)
        {
            var url = string.Format("https://qyapi.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type={1}", accessToken, type);
            var fileDictionary = new Dictionary<string, string>();
            fileDictionary["media"] = media;
            return Senparc.Weixin.HttpUtility.Post.PostFileGetJson<Senparc.Weixin.QY.AdvancedAPIs.Media.UploadResultJson>(url, null, fileDictionary, null, timeOut);
        }
    }
}
namespace Senparc.Weixin.MP.AdvancedAPIs.Media
{
    public class MediaApiX
    {
        /// <summary>
        /// 上传媒体文件
        /// </summary>
        /// <param name="accessToken">调用接口凭证</param>
        /// <param name="type">媒体文件类型，分别有图片（image）、语音（voice）、视频（video），普通文件(file)</param>
        /// <param name="media">form-data中媒体文件标识，有filename、filelength、content-type等信息</param>
        /// <param name="timeOut">代理请求超时时间（毫秒）</param>
        /// <returns></returns>
        public static Senparc.Weixin.MP.AdvancedAPIs.Media.UploadResultJson Upload(string accessToken, string type, string media, int timeOut = Senparc.Weixin.Config.TIME_OUT)
        {
            var url = string.Format("https://qyapi.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type={1}", accessToken, type);
            var fileDictionary = new Dictionary<string, string>();
            fileDictionary["media"] = media;
            return Senparc.Weixin.HttpUtility.Post.PostFileGetJson<Senparc.Weixin.MP.AdvancedAPIs.Media.UploadResultJson>(url, null, fileDictionary, null, timeOut);
        }
    }
}
