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
    [Obsolete("此模块目前不使用。准备进行删除。请使用OAUTHRedirectController模块", false)]
    public partial class RedirectController : Controller
    {
        public RedirectController()
        {
        }

        [HttpGet]
        [ActionName("Index")]
        public ActionResult Get(string type,string redirectUrl)
        {
            //跳转目标
            string targetUrl = String.Empty;

            switch (type)
            {
                case "OAUTH2":
                    /* OAUTH2接口的跳转。
                     * 1.背景：OAUTH接口，跳转的时候，微信有时会卡住若干秒；这个时候，如果客户端发生刷新操作，会造成40029(Code多次使用的现象)；
                     *         新的流程变为：  Client Menu Click => View:http://webchat.cloudapp.net/Redirect (该网页) => OAUTH2 => 具体的管理页面，
                     *         这样即使发生刷新的操作，也是刷新该网页。
                     * 2.安全：OAUTH接口，本身要求配置“回调有效域名”，因此即使其他域的请求，调用我们这个页面，也无法最终得到OAUTH的最终跳转。安全。
                     */
                    targetUrl = String.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base&state=000#wechat_redirect",
                                              WeiXinConfiguration.appID, 
                                              redirectUrl);
                    break;

                default:
                    targetUrl = "http://webchat.cloudapp.net/";
                    break;
            }

            //执行跳转
            if (String.IsNullOrEmpty(targetUrl) == false)
                //Response.Redirect(targetUrl,true);    //Url改变
                return this.Redirect(targetUrl);        //Url改变
                //Server.Transfer()                     //Error!


            //if (String.IsNullOrEmpty(targetUrl) == false)
            //    return this.Redirect(targetUrl);
            //else
                return Content("Empty");


            /*
            http://webchat.cloudapp.net/Redirect?type=OAUTH2&redirectUrl=http%3a%2f%2fwebchat.cloudapp.net%2fAccount%2fValidation

            http://webchat.cloudapp.net/Redirect?type=OAUTH2&redirectUrl=http%3a%2f%2fwebchat.cloudapp.net%2fAccount%2fChangeValidation

            http://webchat.cloudapp.net/Redirect?type=OAUTH2&redirectUrl=http%3a%2f%2fwebchat.cloudapp.net%2fAccount%2fUnValidation 
             */
        }

    }
}
