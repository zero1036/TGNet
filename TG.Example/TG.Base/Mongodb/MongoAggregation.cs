using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace TG.Example
{
    public class MongoAggregation
    {
        private static IMongoDatabase GetDatabase(string key = "")
        {
            var connectString = "mongodb://localhost/MissionV2";
            var mongoUrl = new MongoUrl(connectString);
            var client = new MongoClient(connectString);
            return client.GetDatabase(mongoUrl.DatabaseName);
        }

        /// <summary>
        /// Count 不论集合有多大，都会很快返回总的文档数量
        /// 用作验证collection是否为空，安全
        /// </summary>
        public void CountAll()
        {
            IMongoDatabase db = GetDatabase();
            var col = db.GetCollection<MyUser>("users");

            long count = col.Count<MyUser>(x => true);
        }


        public void Distinct()
        {
            IMongoDatabase db = GetDatabase();

            var col = db.GetCollection<MyUser>("users");

            IAsyncCursor<MyUser> ac = col.Distinct(x => x, "age");

            MyUser sf = ac.SingleOrDefault();

            Console.WriteLine(sf.age);
            //List<string> list = new List<string>();
            //list.AsQueryable().
        }
    }
}
