using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Senparc.Weixin.Entities;
using Senparc.Weixin.QY.Entities;
using Senparc.Weixin.QY.Entities.Menu;
using Senparc.Weixin.QY.CommonAPIs;
using Senparc.Weixin.QY.AdvancedAPIs.App;
/*****************************************************
* 目的：SDK CommonJsonSend类扩展，由于原始类只能局限.net强类型转发发送数据格式，修改后直接发送json object
* 创建人：林子聪
* 创建时间：20150323
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Utility.WeiXin
{
    public static class CommonJsonSendX
    {
        /// <summary>
        /// 向需要AccessToken的API发送消息的公共方法
        /// </summary>
        /// <param name="accessToken">这里的AccessToken是通用接口的AccessToken，非OAuth的。如果不需要，可以为null，此时urlFormat不要提供{0}参数</param>
        /// <param name="urlFormat"></param>
        /// <param name="data">如果是Get方式，可以为null</param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public static WxJsonResult Send(string accessToken, string urlFormat, string data, CommonJsonSendType sendType = CommonJsonSendType.POST, int timeOut = 10000)
        {
            return Send<WxJsonResult>(accessToken, urlFormat, data, sendType, timeOut);
        }

        /// <summary>
        /// 向需要AccessToken的API发送消息的公共方法
        /// </summary>
        /// <param name="accessToken">这里的AccessToken是通用接口的AccessToken，非OAuth的。如果不需要，可以为null，此时urlFormat不要提供{0}参数</param>
        /// <param name="urlFormat"></param>
        /// <param name="data">如果是Get方式，可以为null</param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public static T Send<T>(string accessToken, string urlFormat, string jsonString, CommonJsonSendType sendType = CommonJsonSendType.POST, int timeOut = 10000)
        {
            var url = string.IsNullOrEmpty(accessToken) ? urlFormat : string.Format(urlFormat, accessToken);
            switch (sendType)
            {
                case CommonJsonSendType.GET:
                    return Senparc.Weixin.HttpUtility.Get.GetJson<T>(url);
                case CommonJsonSendType.POST:
                    //Senparc.Weixin.Helpers.SerializerHelper serializerHelper = new Senparc.Weixin.Helpers.SerializerHelper();
                    //var jsonString = serializerHelper.GetJsonString(data);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        var bytes = Encoding.UTF8.GetBytes(jsonString);
                        ms.Write(bytes, 0, bytes.Length);
                        ms.Seek(0, SeekOrigin.Begin);

                        return Senparc.Weixin.HttpUtility.Post.PostGetJson<T>(url, null, ms, timeOut: timeOut);
                    }
                default:
                    throw new ArgumentOutOfRangeException("sendType");
            }
        }
    }
}
