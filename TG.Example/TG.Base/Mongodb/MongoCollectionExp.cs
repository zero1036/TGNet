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
            this._db = MongoBase.GetDatabase();
            redisdb = RedisProvider.redis.GetDatabase();
        }

        public void CreateCappedCollection()
        {
            this._db.CreateCollection("cappedcol1", new CreateCollectionOptions()
            {
                Capped = true,
                MaxDocuments = 5,
                MaxSize = 500
            });
        }


        public void CappedColVsRedis1()
        {
            int level = 1000000;
            //this._db.CreateCollection("col2redis", new CreateCollectionOptions()
            //{
            //    Capped = true,
            //    MaxDocuments = level,
            //    MaxSize = level * 100
            //});

            var col = _db.GetCollection<MyUser>("cappedcol1");

            MyUser my = new MyUser()
            {
                name = "tg",
                age = 12,
                amount = "1283"
            };
            col.InsertOne(my);
        }

        private IDatabase redisdb;
        public void CappedColVsRedis2()
        {
            MyUser my = new MyUser()
            {
                name = "tg",
                age = 12,
                amount = "1283"
            };

            string val = JsonConvert.SerializeObject(my);

            long te = redisdb.ListRightPush("cappedcol2", val);
        }
    }
}
