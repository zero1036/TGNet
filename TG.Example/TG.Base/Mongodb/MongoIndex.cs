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
                for (int i = 1; i <= 1000000; i++)
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
    }
}
