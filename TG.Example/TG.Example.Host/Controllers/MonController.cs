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
            me.CappedColVsRedis1();

            return Content("Ok");
        }
    }
}