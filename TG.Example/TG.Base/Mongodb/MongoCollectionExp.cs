using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Shared;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;
using StackExchange.Redis;
using Newtonsoft.Json;

namespace TG.Example
{
    public class MongoCollectionExp
    {
        IMongoDatabase _db;

        public MongoCollectionExp()
        {

        }

        public void CreateCappedCollection()
        {
            int level = 1000000;
            this._db = MongoBase.GetDatabase();
            this._db.CreateCollection("col2redis", new CreateCollectionOptions()
            {
                Capped = true,
                MaxDocuments = level,
                MaxSize = level * 100
            });
        }


        public void CappedColVsRedis1()
        {
            int level = 1000000;
            this._db = MongoBase.GetDatabase();

            //this._db.CreateCollection("col2redis", new CreateCollectionOptions()
            //{
            //    Capped = true,
            //    MaxDocuments = level,
            //    MaxSize = level * 100
            //});

            var col = _db.GetCollection<MyUser>("col2redis");

            MyUser my = new MyUser()
            {
                name = "tg",
                age = 12,
                amount = "1283"
            };
            col.InsertOne(my);
        }

        public string CappedColVsRedis1_read()
        {
            this._db = MongoBase.GetDatabase();

            var col = _db.GetCollection<MyUser>("cappedcol1");

            var one = col.Find(x => x.age == 12).FirstOrDefault();

            return one.ToJson();
        }

        private IDatabase redisdb;
        public void CappedColVsRedis2()
        {
            redisdb = RedisProvider.redis.GetDatabase();

            MyUser my = new MyUser()
            {
                name = "tg",
                age = 12,
                amount = "1283"
            };

            string val = my.ToJson();

            long te = redisdb.ListRightPush("cappedcol2", val);
        }

        //public string CappedColVsRedis2_read()
        //{
        //    this._db = MongoBase.GetDatabase();

        //    var col = _db.GetCollection<MyUser>("cappedcol1");

        //    var one = col.Find(x => x.age == 12).FirstOrDefault();

        //    return one.ToJson();
        //}


    }
}
