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
    public class MongoQuery
    {
        public void QueryMultiConditions()
        {
            IMongoDatabase db = MongoBase.GetDatabase();
            var col = db.GetCollection<Foo>("foo");

            //通过 x == null获得Mongodb属性为空对象
            var res = col.Find(x => x.Phone == "15902050034" || x.Phone == null).ToList();

        }
    }
}
