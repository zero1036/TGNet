using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TG.Example
{
    /// <summary>
    /// Http请求及结果解析
    /// </summary>
    internal sealed class HttpHelper
    {
        /// <summary>
        /// http get 请求操作
        /// </summary>
        /// <param name="url">get 地址</param>
        /// <param name="timeout">超时时间(毫秒)</param>
        /// <param name="encoding">编码</param>
        /// <param name="authInfo">The authentication information.</param>
        /// <returns>HttptResult</returns>
        /// <exception cref="System.Exception">请求初始化异常</exception>
        public static HttpResult HttpGet(string url, int timeout, Encoding encoding = null, string authInfo = "")
        {
            var httpWebRequest = WebRequest.Create(url) as HttpWebRequest;
            if (httpWebRequest == null)
            {
                throw new Exception("请求初始化异常");
            }

            httpWebRequest.Method = "GET";
            httpWebRequest.Timeout = timeout;
            if (!string.IsNullOrEmpty(authInfo))
            {
                httpWebRequest.Headers.Add("Authorization",authInfo );
            }            

            var httpReponse = httpWebRequest.GetResponse() as HttpWebResponse;

            string resultData = string.Empty;
            if (httpReponse.StatusCode == HttpStatusCode.OK)
            {
                var resultStream = new StreamReader(httpReponse.GetResponseStream(), encoding ?? Encoding.UTF8);
                resultData = resultStream.ReadToEnd();
                resultStream.Close();
            }

            return new HttpResult(httpReponse.StatusCode, resultData);
        }

        /// <summary>
        /// http post 请求操作
        /// </summary>
        /// <param name="url">post 地址</param>
        /// <param name="timeout">超时时间(毫秒)</param>
        /// <param name="encoding">编码</param>
        /// <param name="content">内容</param>
        /// <returns>HttptResult</returns>
        /// <exception cref="System.Exception">请求初始化异常</exception>
        public static HttpResult HttpPost(string url, int timeout, Encoding encoding = null, string content = "")
        {
            var httpWebRequest = WebRequest.Create(url) as HttpWebRequest;
            if (httpWebRequest == null)
            {
                throw new Exception("请求初始化异常");
            }

            byte[] payload;
            //将URL编码后的字符串转化为字节
            payload = System.Text.Encoding.UTF8.GetBytes(content);

            httpWebRequest.Method = "POST";
            httpWebRequest.Timeout = timeout;
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.ContentLength = payload.Length;

            //获得请 求流
            Stream writer = httpWebRequest.GetRequestStream();
            //将请求参数写入流
            writer.Write(payload, 0, payload.Length);
            // 关闭请求流
            writer.Close();

            var httpReponse = httpWebRequest.GetResponse() as HttpWebResponse;

            string resultData = string.Empty;
            if (httpReponse.StatusCode == HttpStatusCode.OK)
            {
                var resultStream = new StreamReader(httpReponse.GetResponseStream(), encoding ?? Encoding.UTF8);
                resultData = resultStream.ReadToEnd();
                resultStream.Close();
            }

            return new HttpResult(httpReponse.StatusCode, resultData);
        }

        /// <summary>
        /// 字符串转化为key-value字典 根据key获取value
        /// </summary>
        /// <param name="queryString">格式: access_token=dgerjge & expires_in=12345</param>
        /// <param name="key">key</param>
        /// <returns>value</returns>
        public static string GetQueryStringValue(string queryString, string key)
        {
            var queryStringParams = GetQueryStringParams(queryString);
            if (queryStringParams.ContainsKey(key))
            {
                return queryStringParams[key];
            }

            return null;
        }

        /// <summary>
        /// 字符串转化为key-value字典
        /// </summary>
        /// <param name="queryString">格式如:access_token=dfwkhtrwew & expires_in=sfwefwfsdf</param>
        /// <returns>key-value字典</returns>
        public static Dictionary<string, string> GetQueryStringParams(string queryString)
        {
            return queryString
                        .Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(x =>
                        {
                            var tokens = x.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                            return new KeyValuePair<string, string>(tokens[0], tokens[1]);
                        })
                        .ToDictionary(x => x.Key, x => x.Value);
        }

        /// <summary>
        /// http response result
        /// </summary>
        public class HttpResult
        {
            /// <summary>
            /// http statuscode
            /// </summary>
            public HttpStatusCode StatusCode { get; private set; }

            /// <summary>
            /// http data
            /// </summary>
            public string Data { get; private set; }

            /// <summary>
            /// constructor
            /// </summary>
            /// <param name="statusCode">http statusCode</param>
            /// <param name="data">http result</param>
            public HttpResult(HttpStatusCode statusCode, string data)
            {
                this.StatusCode = statusCode;
                this.Data = data;
            }
        }
    }
}
