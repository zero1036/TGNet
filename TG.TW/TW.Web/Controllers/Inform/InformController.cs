using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TW.Platform.BL;
using TW.Platform.Model;
using TW.Platform.Sys;
using EG.WeChat.Utility.WeiXin;

namespace TW.Web.Controllers
{
    public class InformController : ApiControllerBase
    {
        /// <summary>
        /// 获取公司通告
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        //#if Publishes
        //        [WXOAuth]
        //#endif
        public HttpResponseMessage GetInformPub()
        {
            return this.ExecuteTryCatch(() =>
            {
                var pArticles = new List<WXArticleResultJson>();
                var pArticle = new WXArticleResultJson();
                pArticle.SendTime = DateTime.Now;
                pArticle.byLink = false;
                pArticle.lcId = 1;
                pArticle.lcName = "测试通告";
                pArticle.ListNews = new List<NewsModelX>();
                var n1 = new NewsModelX();
                n1.author = "Mark";
                n1.content = "<div>正文</div>";
                n1.digest = "简介";
                n1.title = "标题";
                pArticle.ListNews.Add(n1);

                for (int i = 1; i <= 20; i++)
                {
                    var pArticle2 = new WXArticleResultJson();
                    pArticle2.SendTime = DateTime.Now;
                    pArticle2.byLink = false;
                    pArticle2.lcId = 1;
                    pArticle2.lcName = "测试通告";
                    pArticle2.ListNews = new List<NewsModelX>();
                    var nb1 = new NewsModelX();
                    nb1.RPath = "images/financial.jpg";
                    nb1.author = "Mark";
                    nb1.content = "<div>正文</div>";
                    nb1.digest = "简介";
                    nb1.title = "标题TTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT";
                    pArticle2.ListNews.Add(nb1);
                    var nb2 = new NewsModelX();
                    nb2.author = "Mark";
                    nb2.content = "<div>正文</div>";
                    nb2.digest = "简介";
                    nb2.title = "标题TTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT";
                    pArticle2.ListNews.Add(nb2);
                    var nb3 = new NewsModelX();
                    nb3.RPath = "images/financial.jpg";
                    nb3.author = "Mark";
                    nb3.content = "<div>正文</div>";
                    nb3.digest = "简介";
                    nb3.title = "标题标题标题标题标题";
                    pArticle2.ListNews.Add(nb3);

                    pArticles.Add(pArticle2);

                    //var pArticle3 = new WXArticleResultJson();
                    //pArticle3.SendTime = DateTime.Now;
                    //pArticle3.byLink = false;
                    //pArticle3.lcId = 1;
                    //pArticle3.lcName = "测试通告";
                    //pArticle3.ListNews = new List<NewsModelX>();
                    //var nc1 = new NewsModelX();
                    //nc1.author = "Mark";
                    //nc1.content = "<div>正文</div>";
                    //nc1.digest = "简介";
                    //nc1.title = "标题";
                    //pArticle3.ListNews.Add(nc1);
                }

                //pArticles.Add(pArticle);
                //pArticles.Add(pArticle2);
                //pArticles.Add(pArticle3);
                return pArticles;
            });
        }

    }
}
