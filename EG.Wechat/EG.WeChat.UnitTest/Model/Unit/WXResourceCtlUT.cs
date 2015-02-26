using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.CommonAPIs;
using EG.WeChat.Service;
using EG.WeChat.Utility.Tools;
using System.Threading;
//using EG.WeChat.Platform.Model;
//using EG.WeChat.Platform.BL;
using System.IO;
using EG.WeChat.Web.Controllers;

namespace EG.WeChat.UnitTest.Model
{
    public class CountX
    {
        public static CountX singlelon = new CountX();

        public int ActionCount { get; set; }

        public void SetAcount(string threadName)
        {
            lock (this)
            {
                this.ActionCount += 1;
                Console.WriteLine(string.Format("線程：{0}；索引：{1}", threadName, this.ActionCount));
            }
        }
    }

    public class WXResourceCtlUT
    {
        #region 私有成员
        protected WXResourceController _Controller;
        #endregion

        #region
        /// <summary>
        /// 绑定解绑接口随机测试
        /// </summary>
        public void ValidationActionTest()
        {
            BaseAction ua;
            _Controller = new WXResourceController();
            _Controller.ControllerContext = MvcContextMockFactory.CreateControllerContext(_Controller, "~/WXResource/sss", "get", "DefaultRoute", "{controller}/{action}", null);

            for (int i = 1; i <= 100; i++)
            {
                //ua = new BaseAction();
                Thread thread = new Thread(run);
                thread.Start();
            }
            //run();
        }
        /// <summary>
        /// 
        /// </summary>
        public void run()
        {
            while (CountX.singlelon.ActionCount < 40)
            {
                try
                {
                    ////System.Threading.Thread.Sleep(10 * 1000);

                    //ActionResult result = _Controller.sss();
                    //CountX.singlelon.SetAcount(System.Threading.Thread.CurrentThread.Name);

                    //Random rr = new Random();
                    //int iTime = rr.Next(1, 3);
                    //System.Threading.Thread.Sleep(iTime * 1000);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

        }
        #endregion
    }
}
