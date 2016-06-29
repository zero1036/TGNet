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
    public class MongoCollectionExp
    {
        IMongoDatabase _db;

        public MongoCollectionExp()
        {
            this._db = MongoBase.GetDatabase();
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
    }
}
