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

namespace TG.Example.Mongodb
{
    public class MongoScript
    {
        /// <summary>
        /// 同时更新多个
        /// </summary>
        public void UpdateMany()
        {
            IMongoDatabase db = MongoBase.GetDatabase();
            var col = db.GetCollection<Foo>("foo");

            var doc = new BsonDocument { { "$set", new BsonDocument { { "Name", "droba" }, { "Age", 40 } } } };
            UpdateResult res = col.UpdateMany(x => x.Sex == 1, doc);

            System.Diagnostics.Debug.WriteLine(res.ToJson());
        }
    }
}
