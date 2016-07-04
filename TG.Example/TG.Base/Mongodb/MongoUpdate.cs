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

namespace TG.Example
{
    public class MongoUpdate
    {

        public void UpdateOne1()
        {
            IMongoDatabase db = MongoBase.GetDatabase();
            var col = db.GetCollection<Foo>("foo");

            var opt = new UpdateOptions();
            opt.IsUpsert = true; //是否插入

            var doc = new BsonDocument { { "$set", new BsonDocument { { "Sex", "wowen" } } } };
            UpdateResult res = col.UpdateOne(x => x.Name == "tg", doc, opt);

            System.Diagnostics.Debug.WriteLine(res.ToJson());
        }

        public void UpdateOne2()
        {
            IMongoDatabase db = MongoBase.GetDatabase();
            var col = db.GetCollection<Foo>("foo");

            var opt = new UpdateOptions();
            opt.IsUpsert = true; //是否插入

            var myfoo = col.Find(x => x.Name == "tg").SingleOrDefault();
            myfoo.Name = "tg2";
            myfoo.Age += 1;

            var doc = new BsonDocument { { "$set", myfoo.ToBsonDocument() } };
            //var doc = myfoo.ToBsonDocument();
            UpdateResult res = col.UpdateOne(x => x.Id == myfoo.Id, doc, opt);

            System.Diagnostics.Debug.WriteLine(res.ToJson());
        }

        public void UpdateOneNoMatched()
        {
            IMongoDatabase db = MongoBase.GetDatabase();
            var col = db.GetCollection<Foo>("foo");

            var opt = new UpdateOptions();
            opt.IsUpsert = true; //是否插入

            //读取数据，ID为不为空，再更新，会报以下错误
            //insertDocument :: caused by :: 11000 E11000 duplicate key error index: MissionV2.foo.$_id_  dup key: { : ObjectId('577a1b2a9b3252d0c80529ae') }
            //var myfoo = col.Find(x => x.Name == "tg").SingleOrDefault();
            //myfoo.Name = "tg" + myfoo.Age;
            //myfoo.Age += 1;

            var myfoo = new Foo() { Name = "droba", Age = 40 };
            var doc = new BsonDocument { { "$set", myfoo.ToBsonDocument() } };
            UpdateResult res = col.UpdateOne(x => x.Name == "nonamematch", doc, opt);

            System.Diagnostics.Debug.WriteLine(res.ToJson());
        }

        /// <summary>
        /// 更新一个，但多个匹配
        /// 只会更新第一个匹配
        /// </summary>
        public void UpdateOneManyMatched()
        {
            IMongoDatabase db = MongoBase.GetDatabase();
            var col = db.GetCollection<Foo>("foo");

            var doc = new BsonDocument { { "$set", new BsonDocument { { "Name", "droba" }, { "Age", 40 } } } };
            UpdateResult res = col.UpdateOne(x => x.Sex == 1, doc);

            System.Diagnostics.Debug.WriteLine(res.ToJson());
        }

        public void UpdateMany()
        {
            IMongoDatabase db = MongoBase.GetDatabase();
            var col = db.GetCollection<Foo>("foo");

            var doc = new BsonDocument { { "$set", new BsonDocument { { "Name", "droba" }, { "Age", 40 } } } };
            UpdateResult res = col.UpdateMany(x => x.Sex == 1, doc);

            System.Diagnostics.Debug.WriteLine(res.ToJson());
        }

        public void UpdateMany()
        {
            IMongoDatabase db = MongoBase.GetDatabase();
            var col = db.GetCollection<Foo>("foo");

            var doc = new BsonDocument { { "$set", new BsonDocument { { "Name", "droba" }, { "Age", 40 } } } };
            UpdateResult res = col.UpdateMany(x => x.Sex == 1, doc);

            System.Diagnostics.Debug.WriteLine(res.ToJson());
        }


    }

    public class Foo
    {
        [BsonSerializer(typeof(ObjectIdSerializer))]
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int Sex { get; set; }
    }
}
