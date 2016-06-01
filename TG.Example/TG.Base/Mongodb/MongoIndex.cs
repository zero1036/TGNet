using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TG.Example
{
    /// <summary>
    /// 索引
    /// </summary>
    public class MongoIndex
    {
        private static IMongoDatabase GetDatabase(string key = "")
        {
            var connectString = "mongodb://localhost/MissionV2";
            var mongoUrl = new MongoUrl(connectString);
            var client = new MongoClient(connectString);
            return client.GetDatabase(mongoUrl.DatabaseName);
        }

        public void InsertMany()
        {
            IMongoDatabase db = GetDatabase();
            var col = db.GetCollection<MyUser>("users");

            for (int j = 1; j <= 10; j++)
            {
                MyUser my;
                IList<MyUser> list = new List<MyUser>();
                for (int i = 1; i <= 100000; i++)
                {
                    my = new MyUser()
                    {
                        name = "tg" + i,
                        age = i,
                        amount = "1283"
                    };
                    list.Add(my);
                }
                col.InsertMany(list);
                list = null;
                GC.Collect();
            }
        }

        public void InsertOne()
        {
            IMongoDatabase db = GetDatabase();
            var col = db.GetCollection<MyUser>("users");

            MyUser my;

            my = new MyUser()
            {
                name = "tg" + 1,
                age = 3,
                amount = "1283"
            };

            col.InsertOne(my);
        }

        public void CountAll()
        {
            IMongoDatabase db = GetDatabase();
            var col = db.GetCollection<MyUser>("users");

            long count = col.Count<MyUser>(x => true);
        }

        public void ValidateCollectionNull()
        {
            IMongoDatabase db = GetDatabase();
            var col = db.GetCollection<LuckyMoney>("LuckyMoney11Day");

            long count = col.Count(x => true);
            IFindFluent<LuckyMoney, LuckyMoney> findFluent = col.Find(x => x.Id != null).Limit(1);

            //findFluent.Count()
        }


        public void CreateIndex()
        {
            IMongoDatabase db = GetDatabase();
            var col = db.GetCollection<LuckyMoney>("users");

            col.Indexes.CreateOne(new BsonDocument() { { "name", 1 } }, new CreateIndexOptions() { Background = true });
        }
    }
}
