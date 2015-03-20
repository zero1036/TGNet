using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Senparc.Weixin.MP.TenPayLib;
using EG.WeChat.Service.WeiXin;
using System.Collections;
using EG.WeChat.Utility.WeiXin;
using EG.WeChat.Platform.BL;

namespace EG.WeChat.Web
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        #region 服务启动的处理

        /// <summary>
        /// 服务启动时
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //处理Web.Config
            DealWebConfig();

            //注册公众账号信息
            Senparc.Weixin.MP.CommonAPIs.AccessTokenContainer.Register(WeiXinConfiguration.appID,
                                                                       WeiXinConfiguration.appsecret);
            ////注册企业账号信息
            //Senparc.Weixin.QY.CommonAPIs.AccessTokenContainer.Register(WeiXinConfiguration.cropId,
            //                                                           WeiXinConfiguration.corpSecret);

            #region 提供微信支付信息
            /* 目前无此需求。后续可以预见有类似需求。 */

            //提供微信支付信息
            //var weixinPay_PartnerId = System.Configuration.ConfigurationManager.AppSettings["WeixinPay_PartnerId"];
            //var weixinPay_Key = System.Configuration.ConfigurationManager.AppSettings["WeixinPay_Key"];
            //var weixinPay_AppId = System.Configuration.ConfigurationManager.AppSettings["WeixinPay_AppId"];
            //var weixinPay_AppKey = System.Configuration.ConfigurationManager.AppSettings["WeixinPay_AppKey"];
            //var weixinPay_TenpayNotify = System.Configuration.ConfigurationManager.AppSettings["WeixinPay_TenpayNotify"];
            //var weixinPayInfo = new TenPayInfo(weixinPay_PartnerId, weixinPay_Key, weixinPay_AppId,weixinPay_AppKey, weixinPay_TenpayNotify);
            //TenPayInfoCollection.Register(weixinPayInfo);

            #endregion
        }

        /// <summary>
        /// 执行初始化时
        /// </summary>
        public override void Init()
        {
            base.Init();

            //缓存设置
            this.PreRequestHandlerExecute -= WebApiApplication_PreRequestHandlerExecute;
            this.PreRequestHandlerExecute += WebApiApplication_PreRequestHandlerExecute;
        }

        /// <summary>
        /// Session开始时
        /// </summary>
        protected void Session_Start()
        {

        }

        /// <summary>
        /// Session结束时
        /// </summary>
        protected void Session_End()
        {
            if (StaticLibrary.ActiveSession.ContainsKey(Session.SessionID))
            {
                StaticLibrary.ActiveSession.Remove(Session.SessionID);
            }
        }

        #endregion

        #region 禁用缓存
        /* Web页面会将所有的JS、CSS缓存到微信客户端的内置浏览器。
         * 官方的描述，会超过24小时之后才清理相关缓存（无法确认）。
         * 
         * 缓存没有及时更新，造成的后果是 页面显示不正确(甚至无法提交数据 [因为我们使用JS、JQQuery提交数据])；
         * 然后客户可以做的，目前：
         * 1.【只能】在手机端，选择“微信应用信息”->选择【清空数据】。这样会造成全部微信数据丢失，聊天记录、图片等等；
         * 2.【不能】在手机端，选择“微信应用信息”->选择【清空缓存】。测试得知，这样清除仍然无效；
         * 3.【不能】在微信端，选择“设置”->选择“通用”->“选择清空缓存”。测试得知，这样清除仍然无效；
         * 
         * 因此，目前只有上述【1】的方式有效，但是造成的效果太难让用户接受。
         * 因此在IIS服务端，禁用缓存是必要的。
         * 
         * ------------------------------------
         * 需要注意的是，缓存禁用之后，会造成服务器的访问流量的加大；每次请求，均重新请求所有的静态文件。
         * 
         */

        void WebApiApplication_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            /* 2014-12-24 Team:不使用此逻辑。由具体的Controller去标记Action。 */
            if (HttpContext.Current != null &&
                HttpContext.Current.Response != null &&
                HttpContext.Current.Response.Cache != null)
            {
                HttpContext.Current.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
                HttpContext.Current.Response.Cache.SetValidUntilExpires(false);
                HttpContext.Current.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                HttpContext.Current.Response.Cache.SetNoStore();
            }
        }

        #endregion

        #region 处理Web.Config
        /// <summary>
        /// 处理Web.Config
        /// </summary>
        private void DealWebConfig()
        {
            //##** 2014-12-22:下面的字段可以考虑反射回WebConfigVM的访问器属性。
            EG.Business.Common.ConfigCache.LoadAppConfig(new string[] 
            {//##需要解密的字段 
                "DB_USER","DB_PSW",                                         //数据库
                "WX_appID","WX_appsecret","WX_Token","WX_EncodingAESKey"    //微信账号
            });

            //微信公众账号信息设置
            WeiXinConfiguration.appID = EG.Business.Common.ConfigCache.GetAppConfig("WX_appID");
            WeiXinConfiguration.appsecret = EG.Business.Common.ConfigCache.GetAppConfig("WX_appsecret");
            WeiXinConfiguration.Token = EG.Business.Common.ConfigCache.GetAppConfig("WX_Token");
            WeiXinConfiguration.EncodingAESKey = EG.Business.Common.ConfigCache.GetAppConfig("WX_EncodingAESKey");
            //微信企业账号信息设置
            //WeiXinConfiguration.cropId
            //WeiXinConfiguration.corpSecret
            new WXCorpBaseService().SetCorpConfiguration();
        }

        #endregion
    }
}
