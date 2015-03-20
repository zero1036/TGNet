
using EG.WeChat.Service.WeiXin;
using EG.WeChat.Utility.WeiXin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace EG.WeChat.Web.Controllers
{
    /* 预期的处理流程：
     * 微信Menus，使用View类型，URL使用OAUTH接口，然后跳转到该页面（微信控制平台）；
     * 该页面解析完UserID之后，跳转到具体的“业务系统”，同时传递解析出来的UserID等信息。
     * 
     * 举例：
     * 1.Menus的URL设定为： 
     * https://open.weixin.qq.com/connect/oauth2/authorize?appid=[APPID]&redirect_uri=[当前Controller+业务系统URL]&response_type=code&scope=snsapi_base&state=000#wechat_redirect
     * 
     * 2.来到该页面的时候，会变成：
     * [当前Controller+业务系统URL]&Code=xxxx(微信OAUTH附加Code参数)
     * 当前Controller解析获得UserId和DeviceId(企业号接口才有)等。
     * 
     * 3.当前Contrller，执行跳转：
     * [业务系统URL]&UserId=xxxx&DeivceId=yyyyy。
     * 
     * -------------
     * 如果配置方面，觉得“步骤1”的URL长度过长，不够友好，
     * 请查看下面“辅助处理”的折叠代码，使用友好的跳转处理。
     * 
     */

    /// <summary>
    /// OAUTH跳转的页面控制
    /// </summary>
    public class OAUTHRedirectController : Controller
    {

        //--------ConstVar-------

        #region Session名称常量

        const string SESSION_NAME_OPENID = "WX_OAUTH_OpenId";
        const string SESSION_NAME_DEVICEID = "WX_OAUTH_DeviceId";

        #endregion

        #region Get参数常量

        const string GET_NAME_OPENID = "OpenId";
        const string GET_NAME_DEVICEID = "DeviceId";

        #endregion


        //--------Control--------

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public OAUTHRedirectController()
        {
        }

        #endregion


        #region 解析数据，然后跳转到业务系统

        /// <summary>
        /// 跳转控制
        /// </summary>
        /// <param name="code">微信OAUTH跳转我们URL时，附加的Code参数</param>
        /// <param name="BSUrl">构建Menus的URL时，我们附加的“业务系统URL”</param>
        /// <param name="usage">指定用途</param>
        /// <param name="agentId">应用ID</param>
        [HttpGet]
        [ActionName("Do")]
        public ActionResult Do(string code, string BSUrl, string usage, int agentId = 0)
        {
            string userOpenId = String.Empty;
            string userDeviceId = String.Empty;

            //忽略大小写
            if (usage != null)
                usage = usage.ToLower();

            //##根据"服务号和订阅号"与"企业号"，分别获取不同的用户信息
            switch (usage)
            {
                default:
                case "normal":
                    GetUserBaseInfo_General(this, code, out userOpenId);
                    break;

                case "qy":
                    GetUserBaseInfo_QY(this, code, agentId, out userOpenId, out userDeviceId);
                    break;
            }

            //转码
            BSUrl = HttpUtility.UrlDecode(BSUrl);

            //跳转目标
            string targetUrl = "";
            switch (usage)
            {
                default:
                case "normal":
                    {
                        var query = HttpUtility.ParseQueryString(new Uri(BSUrl).Query);
                        query.Add(GET_NAME_OPENID, userOpenId);
                        targetUrl = new UriBuilder(BSUrl) { Query = query.ToString() }.Uri.ToString();
                    }
                    break;

                case "qy":
                    {
                        var query = HttpUtility.ParseQueryString(new Uri(BSUrl).Query);
                        query.Add(GET_NAME_OPENID, userOpenId);
                        query.Add(GET_NAME_DEVICEID, userDeviceId);
                        targetUrl = new UriBuilder(BSUrl) { Query = query.ToString() }.Uri.ToString();
                    }
                    break;
            }

            //执行跳转
            return this.Redirect(targetUrl);
        }

        #endregion

        #region 解析数据_服务号和订阅号

        /// <summary>
        /// 解析数据_服务号和订阅号
        /// </summary>
        private static void GetUserBaseInfo_General(Controller controller, string code, out string OpenId)
        {
            //初始化结果
            OpenId = String.Empty;

            //参数检查
            if (string.IsNullOrEmpty(code))
                return;

            //判断Session是否有存储，有则直接返回
            string openIdInSession = controller.Session[SESSION_NAME_OPENID] as string;
            if (String.IsNullOrEmpty(openIdInSession) == false)
            {
                OpenId = openIdInSession;
            }

            //目标：只获取OpenID
            Senparc.Weixin.MP.AdvancedAPIs.OAuth.OAuthAccessTokenResult result = null;
            try
            {
                result = Senparc.Weixin.MP.AdvancedAPIs.OAuth.OAuthApi.GetAccessToken(WeiXinConfiguration.appID,
                                                                             WeiXinConfiguration.appsecret,
                                                                             code);
            }
            catch (Exception ex)
            {
                //如果获取不到，返回NULL，外部进行错误处理。
                return;
            }
            if (result != null)
            {
                //存储到Session
                controller.Session[SESSION_NAME_OPENID] = result.openid;

                //返回结果
                OpenId = result.openid;
            }
        }

        #endregion

        #region 解析数据_企业号

        /// <summary>
        /// 解析数据_企业号
        /// </summary>
        private static void GetUserBaseInfo_QY(Controller controller, string code, int agentId, out string OpenId, out string DeviceId)
        {
            //初始化结果
            OpenId = String.Empty;
            DeviceId = String.Empty;

            //参数检查
            if (string.IsNullOrEmpty(code))
                return;

            //判断Session是否有存储，有则直接返回
            string openIdInSession = controller.Session[SESSION_NAME_OPENID] as string;
            if (String.IsNullOrEmpty(openIdInSession) == false)
            {
                OpenId = openIdInSession;
            }

            //目标：只获取OpenID
            Senparc.Weixin.QY.AdvancedAPIs.OAuth2.GetUserIdResult result = null;
            try
            {
                result = Senparc.Weixin.QY.AdvancedAPIs.OAuth2.OAuth2Api.GetUserId(Senparc.Weixin.QY.CommonAPIs.AccessTokenContainer.GetToken(WeiXinConfiguration.cropId),
                                                                         code,
                                                                         agentId);
            }
            catch (Exception ex)
            {
                //如果获取不到，返回NULL，外部进行错误处理。
                return;
            }
            if (result != null)
            {
                //存储到Session
                controller.Session[SESSION_NAME_OPENID] = result.UserId;
                controller.Session[SESSION_NAME_DEVICEID] = "";    //最近的接口才出现DeviceId，准备通过升级SDK来支持。

                //返回结果
                OpenId = result.UserId;
                DeviceId = "";
            }
        }

        #endregion


        #region 辅助处理

        /* 由于“步骤1”,Menus需要配置比较长的URL，对于配置人员来说友好度不高。
         * 因此这里，在“步骤1”之前再提供一次跳转支持，帮忙组合那串比较长的URL。
         * 
         * 缩短之后，可以在菜单配置方面，指向：
         * http://webchat.cloudapp.net/OAUTHRedirect?BSUrl=xxxx&usage=yyyy  企业号请多赋值一个cropId参数
         * 
         * 本地测试用例：
         * http://localhost:18667/OAUTHRedirect?BSUrl=http%3a%2f%2fbaidu.com&usage=Normal
         */

        [HttpGet]
        [ActionName("Index")]
        public ActionResult Index(string BSUrl, string usage, int agentId = 0)
        {
            //忽略大小写
            if (usage != null)
                usage = usage.ToLower();

            //跳转目标(指定为Do的Action，同时附带上参数)
            string targetUrl = String.Format("{0}://{1}{2}",
                                              Request.Url.Scheme,       //协议类型
                                              Request.Url.Authority,    //Host+Port
                                              this.Url.Action("Do", new { BSUrl = BSUrl, usage = usage, agentId = agentId }));

            //##根据"服务号和订阅号"与"企业号"，分别获取不同的 OAUTH2.0的URL
            switch (usage)
            {
                default:
                case "normal":
                    targetUrl = Senparc.Weixin.MP.AdvancedAPIs.OAuth.OAuthApi.GetAuthorizeUrl(WeiXinConfiguration.appID, targetUrl, "000", Senparc.Weixin.MP.AdvancedAPIs.OAuth.OAuthScope.snsapi_base);
                    break;

                case "qy":
                    targetUrl = Senparc.Weixin.QY.AdvancedAPIs.OAuth2.OAuth2Api.GetCode(WeiXinConfiguration.cropId, targetUrl, "000");
                    break;
            }

            return Redirect(targetUrl);
        }

        #endregion

    }
}
