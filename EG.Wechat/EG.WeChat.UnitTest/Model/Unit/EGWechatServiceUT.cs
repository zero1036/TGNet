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
* 目的：微信项目 向ODS提供服务接口单元测试
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
    /// 测试用例
    /// </summary>
    [TestClass]
    public class EGWechatServiceUT
    {
        #region 模拟测试
        /// <summary>
        /// WCF服务接口测试，带参，并stream解析
        /// </summary>
        public static string SimulateRequestWCF()
        {
            try
            {
                string strPost = "测试模型";
                byte[] buffer = Encoding.UTF8.GetBytes(strPost);
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://localhost:54694/Service/EGWechatService.svc/postMostStr/" + buffer.Length);
                request.Method = "POST";
                request.ContentType = "text/plain";

                request.ContentLength = buffer.Length;
                Stream requestStram = request.GetRequestStream();
                requestStram.Write(buffer, 0, buffer.Length);
                requestStram.Close();

                System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
                System.IO.Stream getStream = response.GetResponseStream();

                byte[] resultByte = new byte[response.ContentLength];
                getStream.Read(resultByte, 0, resultByte.Length);

                string strResult = Encoding.UTF8.GetString(resultByte);
                Console.WriteLine(strResult);
                return strResult;
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.ToString());
                Console.WriteLine(ex.ToString());
                return ex.ToString();
            }
        }
        /// <summary>
        /// WCF服务接口测试，带参，并stream解析
        /// </summary>
        public static string SimulateRequestWCF2()
        {
            try
            {
                //string strPost = "测试模型";

                //HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://localhost:54694/Service/EGWechatService.svc/postMostStr2/");
                //request.Method = "POST";
                ////request.ContentType = "text/plain";
                //request.ContentType = "application/x-www-form-urlencoded";

                ////这是原始代码：
                //string strPost = string.Format("streamLength={0}&strValue={1}", "lin", "Mark");
                //byte[] buffer = Encoding.UTF8.GetBytes(strPost);
                //request.ContentLength = buffer.Length;
                //Stream requestStram = request.GetRequestStream();
                //requestStram.Write(buffer, 0, buffer.Length);
                //requestStram.Close();

                //System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
                //System.IO.Stream getStream = response.GetResponseStream();

                //byte[] resultByte = new byte[response.ContentLength];
                //getStream.Read(resultByte, 0, resultByte.Length);

                //string strResult = Encoding.UTF8.GetString(resultByte);
                //Console.WriteLine(strResult);
                //return strResult;

                //这是原始代码：
                string paraUrlCoded = string.Format("streamLength={0}&strValue={1}", "lin", "Mark");

                string strURL = "http://localhost:54694/Service/EGWechatService.svc/postMostStr2/" + paraUrlCoded.Length.ToString() + "/mark";
                System.Net.HttpWebRequest request;
                request = (System.Net.HttpWebRequest)HttpWebRequest.Create(strURL);
                //Post请求方式
                request.Method = "POST";
                // 内容类型
                request.ContentType = "text/plain";

                byte[] payload;
                //将URL编码后的字符串转化为字节
                payload = System.Text.Encoding.UTF8.GetBytes(paraUrlCoded);
                //设置请求的 ContentLength 
                request.ContentLength = payload.Length;
                //获得请 求流
                Stream writer = request.GetRequestStream();
                //将请求参数写入流
                writer.Write(payload, 0, payload.Length);
                // 关闭请求流
                writer.Close();
                //获取请求对应响应
                System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
                System.IO.Stream getStream = response.GetResponseStream();
                //
                byte[] resultByte = new byte[response.ContentLength];
                getStream.Read(resultByte, 0, resultByte.Length);

                string strResult = System.Text.Encoding.UTF8.GetString(resultByte);
                return strResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return ex.ToString();
            }
        }
        /// <summary>
        /// MVC服务接口测试，带参数，文本
        /// </summary>
        public static string SimulateRequestMVC1()
        {
            try
            {
                string strPost = "测试模型";
                byte[] buffer = Encoding.UTF8.GetBytes(strPost);
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(" http://localhost:18667/TemplateMessage/Test_MessageSendForService?str=" + strPost);
                request.Method = "POST";
                request.ContentType = "text/plain";

                request.ContentLength = buffer.Length;
                Stream requestStram = request.GetRequestStream();
                requestStram.Write(buffer, 0, buffer.Length);
                requestStram.Close();

                System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
                System.IO.Stream getStream = response.GetResponseStream();

                byte[] resultByte = new byte[response.ContentLength];
                getStream.Read(resultByte, 0, resultByte.Length);

                string strResult = Encoding.UTF8.GetString(resultByte);
                return strResult;
                //Console.WriteLine(strResult);
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.ToString());
                return ex.ToString();
            }
        }
        /// <summary>
        /// MVC服务接口测试，带参，并stream解析
        /// </summary>
        public static string SimulateRequestMVC2()
        {
            try
            {
                string strPost = "测试模型";
                byte[] buffer = Encoding.UTF8.GetBytes(strPost);
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(" http://localhost:18667/TemplateMessage/Test_MessageSendForService2");
                request.Method = "POST";
                request.ContentType = "text/plain";

                request.ContentLength = buffer.Length;
                Stream requestStram = request.GetRequestStream();
                requestStram.Write(buffer, 0, buffer.Length);
                requestStram.Close();

                System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
                System.IO.Stream getStream = response.GetResponseStream();

                byte[] resultByte = new byte[response.ContentLength];
                getStream.Read(resultByte, 0, resultByte.Length);

                string strResult = Encoding.UTF8.GetString(resultByte);
                return strResult;
                //Console.WriteLine(strResult);
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.ToString());
                return ex.ToString();
            }
        }
        /// <summary>
        /// MVC服务接口测试，带参，手动填写参数
        /// </summary>
        /// <returns></returns>
        public static string SimulateRequestMVC3()
        {
            try
            {
                string strURL = "http://localhost:18667/TemplateMessage/TemplateMessageService";
                System.Net.HttpWebRequest request;
                request = (System.Net.HttpWebRequest)HttpWebRequest.Create(strURL);
                //Post请求方式
                request.Method = "POST";
                // 内容类型
                request.ContentType = "application/x-www-form-urlencoded";

                //这是原始代码：
                string paraUrlCoded = ExampleParaUrlCoded();
                byte[] payload;
                //将URL编码后的字符串转化为字节
                payload = System.Text.Encoding.UTF8.GetBytes(paraUrlCoded);
                //设置请求的 ContentLength 
                request.ContentLength = payload.Length;
                //获得请 求流
                Stream writer = request.GetRequestStream();
                //将请求参数写入流
                writer.Write(payload, 0, payload.Length);
                // 关闭请求流
                writer.Close();
                //获取请求对应响应
                System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
                System.IO.Stream getStream = response.GetResponseStream();
                //
                byte[] resultByte = new byte[response.ContentLength];
                getStream.Read(resultByte, 0, resultByte.Length);

                string strResult = System.Text.Encoding.UTF8.GetString(resultByte);
                return strResult;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        /// <summary>
        /// MVC服务接口测试——获取模板消息列表
        /// </summary>
        /// <returns></returns>
        public static string SimulateGetMessageListMVC()
        {
            try
            {
                string strURL = "http://localhost:18667/TemplateMessage/GetMessageConfigService";
                System.Net.HttpWebRequest request;
                request = (System.Net.HttpWebRequest)HttpWebRequest.Create(strURL);
                //Post请求方式
                request.Method = "POST";
                // 内容类型
                request.ContentType = "application/x-www-form-urlencoded";

                //这是原始代码：
                string paraUrlCoded = string.Format("State={0}", "");
                byte[] payload;
                //将URL编码后的字符串转化为字节
                payload = System.Text.Encoding.UTF8.GetBytes(paraUrlCoded);
                //设置请求的 ContentLength 
                request.ContentLength = payload.Length;
                //获得请 求流
                Stream writer = request.GetRequestStream();
                //将请求参数写入流
                writer.Write(payload, 0, payload.Length);
                // 关闭请求流
                writer.Close();
                //获取请求对应响应
                System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
                System.IO.Stream getStream = response.GetResponseStream();
                //
                byte[] resultByte = new byte[response.ContentLength];
                getStream.Read(resultByte, 0, resultByte.Length);

                string strResult = System.Text.Encoding.UTF8.GetString(resultByte);
                return strResult;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static string ExampleParaUrlCoded()
        {
            string strJsonResult = @"{""productType"":""[0]"",""name"":""[1]"",""number"":""[2]"",""expDate"":""[3]"",""remark"":""[4]""}";
            strJsonResult = strJsonResult.Replace("[0]", "sony");//;, "sony", "xperia", "2", "2014年1月1号", "感谢购买");
            strJsonResult = strJsonResult.Replace("[1]", "xperia");
            strJsonResult = strJsonResult.Replace("[2]", "2");
            strJsonResult = strJsonResult.Replace("[3]", "2014年1月1号");
            strJsonResult = strJsonResult.Replace("[4]", "感谢购买");

            string Account = "Tgor";
            string TemplateID = "eBfdUggBTbh4V1NdipXxeOGnqMazI-4nmcfdWMpIomE";
            string URL = "www.sogou.com";
            //这是原始代码：
            string paraUrlCoded = string.Format("Account={0}&TemplateID={1}&URL={2}&TemData={3}", Account, TemplateID, URL, strJsonResult);
            return paraUrlCoded;
        }
        #endregion
    }
   

}
