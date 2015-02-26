using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using System.Net;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
/*****************************************************
* 目的：MVC与WCF服务连接综合测试
* 创建人：林子聪
* 创建时间：20141118
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.UnitTest.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class WebServiceBaseUT
    {
        /// <summary>
        /// 创建http请求
        /// </summary>
        /// <param name="strURL"></param>
        /// <param name="webMethod"></param>
        /// <param name="contentType"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        protected System.Net.HttpWebRequest CreateWebRequest(string strURL, string webMethod, string contentType, string content)
        {
            System.Net.HttpWebRequest request;
            request = (System.Net.HttpWebRequest)HttpWebRequest.Create(strURL);
            //Post请求方式
            request.Method = webMethod;
            // 内容类型
            request.ContentType = contentType;
            //
            byte[] payload;
            //将URL编码后的字符串转化为字节
            payload = System.Text.Encoding.UTF8.GetBytes(content);
            //设置请求的 ContentLength 
            request.ContentLength = payload.Length;
            //获得请 求流
            Stream writer = request.GetRequestStream();
            //将请求参数写入流
            writer.Write(payload, 0, payload.Length);
            // 关闭请求流
            writer.Close();
            return request;
        }
        /// <summary>
        /// 获取响应结果
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected string GetResponse(System.Net.HttpWebRequest request)
        {
            //获取请求对应响应
            System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
            System.IO.Stream getStream = response.GetResponseStream();
            //
            byte[] resultByte = new byte[response.ContentLength];
            getStream.Read(resultByte, 0, resultByte.Length);

            string strResult = System.Text.Encoding.UTF8.GetString(resultByte);
            Console.WriteLine(strResult);
            return strResult;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class WebServiceUT : WebServiceBaseUT
    {
        /// <summary>
        /// 
        /// </summary>
        public class ConA
        {
            public string content1
            { get; set; }
            public string content2
            { get; set; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string MainFunction(string strTargetGet)
        {
            int iTargetGet = Convert.ToInt16(strTargetGet);
            switch (iTargetGet)
            {
                case 1:
                    return Simulate_WcfPostRestForStream_WebRequest();
                case 2:
                    return Simulate_WcfPostRestForString_WebClient();
                case 3:
                    return Simulate_WcfPostFormForString_WebRequest();
                case 4:
                    return Simulate_MvcPostFormForString_WebRequest();
                case 5:
                    return Simulate_MvcPostFormForString_WebClient();
                //break;
            }
            return string.Empty;
        }
        /// <summary>
        /// WCF服务接口测试，带参，并stream解析——WebRequest方法
        /// </summary>
        public string Simulate_WcfPostRestForStream_WebRequest()
        {
            try
            {
                //请求内容
                string content = string.Format("sony xperia xl39h");
                //请求地址
                string strURL = string.Format("http://localhost:54694/Service/EGWechatService.svc/WcfPostRestForStream/{0}/{1}", content.Length, "rest参数1");
                //Post——注意：带stream参数下，不能使用GET作为web方法
                string webMethod = "POST";
                //text/plain：是将文件设置为纯文本的形式，浏览器在获取到这种文件时并不会对其进行处理。
                string contentType = "text/plain";
                //创建请求并响应
                System.Net.HttpWebRequest request = CreateWebRequest(strURL, webMethod, contentType, content);
                return GetResponse(request);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return ex.ToString();
            }
        }
        /// <summary>
        /// WCF服务接口测试，带参，并stream解析——WebClient方法
        /// </summary>
        public string Simulate_WcfPostRestForString_WebClient()
        {
            try
            {
                var webClient = new WebClient();
                //请求内容
                string content1 = string.Format("sony xperia xl39h");
                //请求内容
                string content2 = string.Format("huawei 3c");
                //请求地址
                string strURL = string.Format("http://localhost:54694/Service/EGWechatService.svc/WcfPostRestForString/{0}/{1}", content1, content2);
                //Post——注意：带stream参数下，不能使用GET作为web方法
                string webMethod = "POST";
                //
                return webClient.UploadString(strURL, webMethod, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return ex.ToString();
            }
        }
        /// <summary>
        /// WCF服务接口测试，x-www-form-urlencoded解析参数，并stream解析——WebClient方法
        /// </summary>
        /// <returns>失败</returns>
        public string Simulate_WcfPostFormForString_WebRequest()
        {
            try
            {
                //请求内容
                string content1 = "sony xperia xl39h";
                //请求内容
                string content2 = "Huawei 3c";
                //注意：由于采用x-www-form-urlencoded编码方式，参数需要填写一下方式
                string content = string.Format("content1={0}&content2={1}", content1, content2);
                //请求地址
                string strURL = "http://localhost:54694/Service/EGWechatService.svc/WcfPostFormForString";
                //Post——注意：带stream参数下，不能使用GET作为web方法
                string webMethod = "POST";
                //application/x-www-form-urlencoded：是一种编码格式，窗体数据被编码为名称/值对，是标准的编码格式。浏览器用x-www-form-urlencoded的编码方式把form数据转换成一个字串（name1=value1&name2=value2...），然后把这个字串append到url后面，用?分割，加载这个新的url。 当action为post时候，浏览器把form数据封装到http body中，然后发送到server
                string contentType = "application/x-www-form-urlencoded";
                //创建请求并响应
                System.Net.HttpWebRequest request = CreateWebRequest(strURL, webMethod, contentType, content);
                return GetResponse(request);

            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        /// <summary>
        /// MVC服务接口测试，带参string——WebRequest方法
        /// </summary>
        public string Simulate_MvcPostFormForString_WebRequest()
        {
            try
            {
                //请求内容
                string content1 = "sony xperia xl39h";
                //请求内容
                string content2 = "Huawei 3c";
                //注意：由于采用x-www-form-urlencoded编码方式，参数需要填写一下方式
                string content = string.Format("content1={0}&content2={1}", content1, content2);
                //请求地址
                string strURL = "http://localhost:18667/TemplateMessage/MvcPostFormForString";
                //Post——注意：带stream参数下，不能使用GET作为web方法
                string webMethod = "POST";
                //application/x-www-form-urlencoded：是一种编码格式，窗体数据被编码为名称/值对，是标准的编码格式。浏览器用x-www-form-urlencoded的编码方式把form数据转换成一个字串（name1=value1&name2=value2...），然后把这个字串append到url后面，用?分割，加载这个新的url。 当action为post时候，浏览器把form数据封装到http body中，然后发送到server
                string contentType = "application/x-www-form-urlencoded";
                //创建请求并响应
                System.Net.HttpWebRequest request = CreateWebRequest(strURL, webMethod, contentType, content);
                return GetResponse(request);

            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        /// <summary>
        /// MVC服务接口测试，带参string——WebClient方法
        /// </summary>
        public string Simulate_MvcPostFormForString_WebClient()
        {
            try
            {
                var webClient = new WebClient();
                //请求内容
                string content1 = string.Format("sony xperia xl39h");
                //请求内容
                string content2 = string.Format("huawei 3c");
                //请求地址
                string strURL = "http://localhost:18667/TemplateMessage/MvcPostFormForString";
                //Post——注意：带stream参数下，不能使用GET作为web方法
                string webMethod = "POST";
                //form填写方法——无论边种方法，mvc底下都系获取唔到参数
                string content = string.Format("content1={0}&content2={1}", content1, content2);
                //json填写方法——无论边种方法，mvc底下都系获取唔到参数
                var pen = new ConA() { content1 = content1, content2 = content2 };
                content = EG.WeChat.Utility.Tools.CommonFunction.ConvertToJson<ConA>(pen);

                return webClient.UploadString(strURL, webMethod, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return ex.ToString();
            }
        }
    }
}
