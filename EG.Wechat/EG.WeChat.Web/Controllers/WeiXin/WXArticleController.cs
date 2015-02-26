using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EG.WeChat.Platform.BL;
using EG.WeChat.Platform.Model;
using EG.WeChat.Utility;
using Senparc.Weixin.MP.Entities;
using System.Threading;
using System.Threading.Tasks;
/*****************************************************
* 目的：图文段落外部预览Controller
* 创建人：林子聪
* 创建时间：20150121
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Web
{
    public class WXArticleController : Controller
    {
        /// <summary>
        /// action view
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="idx"></param>
        /// <returns></returns>
        public ActionResult Index(string mid, int idx)
        {
            return View();
        }
        /// <summary>
        /// action view
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="idx"></param>
        /// <returns></returns>
        public ActionResult LoadModel(string mid, int idx)
        {
            WeChatArticleService pService = new WeChatArticleService();
            WXArticleResultJson pArticle = pService.LoadResourcesSingle(mid);
            var pResult = pService.GetActionResult();
            if (pResult != null || pArticle == null || pArticle.ListNews == null || pArticle.ListNews.Count <= idx)
                return Content("没有找到对应图文段落！");

            return Json(pArticle.ListNews[idx]);
        }



        public ActionResult ac()
        {
            Fun4();
            string d = DateTime.Now.ToString();
            return Content("hello" + "：" + d);
        }

        private void Fun1()
        {
            //Console.WriteLine("Count: ");
            double xx = TaskFun();
            System.Diagnostics.Debug.WriteLine("Value is : " + xx);
        }
        private void Fun2()
        {
            Task task = new Task(
                () =>
                {
                    for (int i = 0; i < 10; i++)
                    {
                        System.Diagnostics.Debug.WriteLine("Index:{0},ThreadID:{1}", i, Thread.CurrentThread.ManagedThreadId);
                        Thread.Sleep(1000);
                    }
                });
            //在使用能够Task类的Wait方法等待一个任务时或派生类的Result属性获得任务执行结果都有可能阻塞线程，
            //为了解决这个问题可以使用ContinueWith方法，他能在一个任务完成时自动启动一个新的任务来处理执行结果。
            task.ContinueWith(t =>
            {
                System.Diagnostics.Debug.WriteLine("执行完毕!,ThreadID:{0}", Thread.CurrentThread.ManagedThreadId);
            });
            task.Start();


            System.Diagnostics.Debug.WriteLine("主线程执行完毕!ThreadID:{0}", Thread.CurrentThread.ManagedThreadId);
        }
        private void Fun3()
        {
            MyClass pc = new MyClass();
        }
        private async void Fun4()
        {
            double xx = await TaskFun2();
            System.Diagnostics.Debug.WriteLine("action B : " + DateTime.Now);
        }

        private double TaskFun()
        {
            //System.Threading.Thread.Sleep(6000);
            //StringBuilder str = new StringBuilder();
            //for (int i = 1; i <= 10; i++)
            //{
            //    str.Append(i);
            //}
            //return str.ToString();

            double db = 1;
            Task pT = new Task(() =>
            {
                Thread.Sleep(6000);
                db = 50;
                System.Diagnostics.Debug.WriteLine("Value is : " + db);
            });
            pT.Start();
            return db;
        }

        private async Task<double> TaskFun2()
        {
            double db = 1;
            return await Task.Run(() =>
              {
                  Thread.Sleep(6000);
                  db = 50;
                  System.Diagnostics.Debug.WriteLine("action A : " + DateTime.Now);
                  return db;
              });
            //return db;
        }
    }

    public class MyClass
    {
        public MyClass()
        {
            DisplayValue(); //这里不会阻塞  
            System.Diagnostics.Debug.WriteLine("MyClass() End.");
        }
        public Task<double> GetValueAsync(double num1, double num2)
        {
            return Task.Run(() =>
            {
                System.Threading.Thread.Sleep(6000);
                //for (int i = 0; i < 1000000; i++)
                //{
                //    num1 = num1 / num2;
                //}
                return num1;
            });
        }
        public async void DisplayValue()
        {
            double result = await GetValueAsync(1234.5, 1.01);//此处会开新线程处理GetValueAsync任务，然后方法马上返回  
            string d = DateTime.Now.ToString();
            //这之后的所有代码都会被封装成委托，在GetValueAsync任务完成时调用  
            System.Diagnostics.Debug.WriteLine("Value is : " + result + "：" + d);
        }
    }

}
