using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TG.Example;

namespace TG.Example.Host.Controllers
{
    public class MonController : Controller
    {
        //
        // GET: /Mon/
        [ActionName("mongo")]
        public ActionResult MongoInsertOneToCappedCollection()
        {
            MongoCollectionExp me = new MongoCollectionExp();
            me.CappedColVsRedis1();

            return Content("Good");
        }

        [ActionName("redis")]
        public ActionResult RedisInsertOneToCappedCollection()
        {
            MongoCollectionExp me = new MongoCollectionExp();
            me.CappedColVsRedis2();

            return Content("Ok");
        }

        [ActionName("rs1")]
        public ActionResult MongoReplSetTest1()
        {
            while (true)
            {
                Random rd = new Random(DateTime.Now.Millisecond);
                var name = string.Format("olympic_{0}", rd.Next(10000));
                MongoReplSet me = new MongoReplSet();
                me.InsertSingle(name);
            }
            return Content("Ok");
        }

        [ActionName("rs2")]
        public ActionResult MongoReplSetTest2()
        {
            int x = 1;
            while (x <= 100000)
            {
                Random rd = new Random(DateTime.Now.Millisecond);
                var name = string.Format("olympic_{0}", rd.Next(10000));
                MongoReplSet me = new MongoReplSet();
                me.InsertSingle(name);
                x += 1;
            }
            return Content("Ok");
        }
    }
}