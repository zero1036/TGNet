﻿using System;
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

                for (int i = 1; i <= 10; i++)
                {
                    var pArticle2 = new WXArticleResultJson();
                    pArticle2.SendTime = DateTime.Now;
                    pArticle2.byLink = false;
                    pArticle2.lcId = 1;
                    pArticle2.lcName = "测试通告";
                    pArticle2.ListNews = new List<NewsModelX>();
                    var nb1 = new NewsModelX();
                    nb1.RPath = "images/qrcode.png";
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
                    nb3.RPath = "images/qrcode.png";
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
                //System.Threading.Thread.Sleep(5000);
                //pArticles.Add(pArticle);
                //pArticles.Add(pArticle2);
                //pArticles.Add(pArticle3);
                return pArticles;
            });
        }

        [HttpPost]
        public HttpResponseMessage GetInforms()
        {
            return this.ExecuteTryCatch(() =>
            {
                var pa = CreateTestA();

                var pInformNews = new List<InformVM>();
                var pInformNew = new InformVM((new List<int>() { 1, 2 }), (new List<int>() { 1 }), null);
                pInformNew.SendTime = DateTime.Now;
                pInformNew.OwnerID = 1;
                pInformNew.InformType = 2;
                pInformNew.Content = pa;
                pInformNew.ContentType = "news";

                pInformNews.Add(pInformNew);
                return pInformNews;
            });
        }

        private WXArticleResultJson CreateTestA()
        {
            var pArticle2 = new WXArticleResultJson();
            pArticle2.SendTime = DateTime.Now;
            pArticle2.byLink = false;
            pArticle2.lcId = 1;
            pArticle2.lcName = "测试通告";
            pArticle2.ListNews = new List<NewsModelX>();
            var nb1 = new NewsModelX();
            nb1.RPath = "images/qrcode.png";
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
            nb3.RPath = "images/qrcode.png";
            nb3.author = "Mark";
            nb3.content = "<div>正文</div>";
            nb3.digest = "简介";
            nb3.title = "标题标题标题标题标题";
            pArticle2.ListNews.Add(nb3);

            return pArticle2;
        }
    }

    public class InformBL
    {

    }

    public class InformVM : InformM
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pUsers"></param>
        /// <param name="pDeps"></param>
        /// <param name="pTags"></param>
        public InformVM(List<int> pUsers, List<int> pDeps, List<int> pTags)
        {
            base.SendUsers = pUsers;
            base.SendDeps = pDeps;
            base.SendTags = pTags;

            base.SendAllUsers = new List<int>();
            base.SendAllUsers.AddRange(pUsers);

            if (!pDeps.IsNull())
            {
                var org = new OrgBL();
                var pUsersFromDeps = org.GetUserIDsByDeps(pDeps);
                base.SendAllUsers.AddRange(pUsersFromDeps);
            }
        }
        /// <summary>
        /// 发送时间
        /// </summary>
        public string SendTimeStr
        {
            get
            {
                return string.Format("{0:f}", base.SendTime);
            }
        }
        /// <summary>
        /// 确定人数
        /// </summary>
        public int ConfirmCount
        {
            get
            {
                if (!base.ConfirmUsers.IsNull())
                {
                    return base.ConfirmUsers.Count;
                }
                return 0;
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class InformM : BaseMessageM
    {
        /// <summary>
        /// 通告类型:——1：公司新闻；2：内部通告
        /// </summary>
        public int InformType { get; set; }
        /// <summary>
        /// 通告内容
        /// </summary>
        public object Content { get; set; }
        /// <summary>
        /// 内容类型
        /// </summary>
        public string ContentType { get; set; }
        /// <summary>
        /// 是否需要确认
        /// </summary>
        public bool ConfirmNeed { get; set; }
        /// <summary>
        /// 是否允许讨论
        /// </summary>
        public bool Disc { get; set; }
        /// <summary>
        /// 确定用户
        /// </summary>
        public List<int> ConfirmUsers { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class BaseMessageM
    {
        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendTime { get; set; }
        /// <summary>
        /// 发送人员编号
        /// </summary>
        public int OwnerID { get; set; }
        /// <summary>
        /// 发送对象人员
        /// </summary>
        public List<int> SendUsers { get; set; }
        /// <summary>
        /// 发送对象部门
        /// </summary>
        public List<int> SendDeps { get; set; }
        /// <summary>
        /// 发送对象标签
        /// </summary>
        public List<int> SendTags { get; set; }
        /// <summary>
        /// 发送对象所有人员
        /// </summary>
        public List<int> SendAllUsers { get; set; }
    }
}
