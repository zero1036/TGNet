using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Shared;

namespace TG.Example
{
    public class MongoBase
    {
        //public void Example()
        //{
        //    //连接信息
        //    string conn = "mongodb://localhost";
        //    string database = "demoBase";
        //    string collection = "demoCollection";

        //    MongoServer mongodb = MongoServer.Create(conn);//连接数据库
        //    IMongoDatabase mongoDataBase = mongodb.GetDatabase(database);//选择数据库名
        //    MongoCollection mongoCollection = mongoDataBase.GetCollection(collection);//选择集合，相当于表

        //    mongodb.Connect();

        //    //普通插入
        //    var o = new { Uid = 123, Name = "xixiNormal", PassWord = "111111" };
        //    mongoCollection.Insert(o);

        //    //对象插入
        //    Person p = new Person { Uid = 124, Name = "xixiObject", PassWord = "222222" };
        //    mongoCollection.Insert(p);

        //    //BsonDocument 插入
        //    BsonDocument b = new BsonDocument();
        //    b.Add("Uid", 125);
        //    b.Add("Name", "xixiBson");
        //    b.Add("PassWord", "333333");
        //    mongoCollection.Insert(b);

        //    Console.ReadLine();
        //}


        public void FindAPI()
        {
            IMongoDatabase db = GetDatabase();
            IMongoCollection<User> col = db.GetCollection<User>("users");

            List<User> users = db.GetCollection<User>("users").Aggregate().Match(x => x.name == "tg").ToList();
        }

        private static IMongoDatabase GetDatabase(string key = "")
        {
            var connectString = "mongodb://localhost/MissionV2";
            var mongoUrl = new MongoUrl(connectString);
            var client = new MongoClient(connectString);
            return client.GetDatabase(mongoUrl.DatabaseName);
        }


        public class User
        {
            public ObjectId _id { get; set; }
            public string name { get; set; }
            public int age { get; set; }
            public string editDate { get; set; }
            public string amount { get; set; }
            public IEnumerable<string> qq { get; set; }
        }
    }
}
