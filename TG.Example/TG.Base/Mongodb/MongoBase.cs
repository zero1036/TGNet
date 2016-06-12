using System;
using System.Dynamic;
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
            IMongoCollection<MyUser> col = db.GetCollection<MyUser>("users");

            List<MyUser> users = db.GetCollection<MyUser>("users").Aggregate().Match(x => x.name == "tg").ToList();
        }


        public void CreateCollectionlkmoney()
        {
            IMongoDatabase db = GetDatabase();
            db.CreateCollection("lkmoney");
        }

        public void DynamicDataTest1()
        {
            lkmoney my = new lkmoney()
            {
                name = "51money",
                creator = "tg",
                Metadata = new BsonDocument("content", "10元现金券")
            };

            IMongoDatabase db = GetDatabase();
            db.GetCollection<lkmoney>("lkmoney").InsertOne(my);
        }

        public void DynamicDataTest2()
        {
            lkmoney2 my = new lkmoney2()
            {
                name = "51money",
                creator = "tg",
                Metadata = new BsonDocument  
                {  
                   {"NO","1000"},  
                   {"Name","Name1"}  
                }
            };

            IMongoDatabase db = GetDatabase();
            db.GetCollection<lkmoney2>("lkmoney").InsertOne(my);
        }

        public void DynamicDataTest3()
        {
            dynamic person = new ExpandoObject();
            person.FirstName = "Jane";
            person.Age = 12;
            person.PetNames = new List<dynamic> { "Sherlock", "Watson" };

            lkmoney3 my = new lkmoney3()
            {
                name = "51money",
                creator = "tg",
                Content = person
            };

            IMongoDatabase db = GetDatabase();
            db.GetCollection<lkmoney3>("lkmoney").InsertOne(my);
        }

        public void LoadDynamicData()
        {
            IMongoDatabase db = GetDatabase();
            var money = db.GetCollection<lkmoney3>("lkmoney").Find(x => x.name == "519money").SingleOrDefault();
            var age = (money.Content as IDictionary<String, Object>)["Age"];

            System.Diagnostics.Debug.WriteLine(money.name);
        }

        public void MongoObjectIdSerialize()
        {
            IDatabase redisdb = RedisProvider.redis.GetDatabase();
            IMongoDatabase mongodb = GetDatabase();
            MyUser user = mongodb.GetCollection<MyUser>("users").Find(x => x.name == "tg99942").ToList()[0];

            var jsonResult = JsonConvert.SerializeObject(user);

            redisdb.StringSet("tst", jsonResult);

            var jsonResultOut = redisdb.StringGet("tst").ToString();


            MyUser userOut = JsonConvert.DeserializeObject<MyUser>(jsonResultOut);

            System.Diagnostics.Debug.WriteLine(jsonResultOut);
        }

        private static IMongoDatabase GetDatabase(string key = "")
        {
            var connectString = "mongodb://localhost/MissionV2";
            var mongoUrl = new MongoUrl(connectString);
            var client = new MongoClient(connectString);
            return client.GetDatabase(mongoUrl.DatabaseName);
        }
    }

    public class ObjectIdConverter : JsonConverter
    {

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            //serializer.Serialize(writer, value.ToString());


            serializer.Serialize(writer, value.ToJson(typeof(ObjectId)));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return ObjectId.Parse(existingValue.ToString());

            //throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(ObjectId).IsAssignableFrom(objectType);
            //return true;
        }


    }


    public class MongoEntityBase
    {
        [BsonSerializer(typeof(ObjectIdSerializer))]
        //[JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId _id { get; set; }
    }

    public class lkmoney : MongoEntityBase
    {
        public string name { get; set; }

        public string creator { get; set; }
        public BsonDocument Metadata { get; set; }
    }

    public class lkmoney2 : MongoEntityBase
    {
        public string name { get; set; }

        public string creator { get; set; }

        [BsonExtraElements]
        public BsonDocument Metadata { get; set; }
    }

    public class lkmoney3 : MongoEntityBase
    {
        public string name { get; set; }

        public string creator { get; set; }

        [BsonSerializer(typeof(ExpandoObjectSerializer))]
        public ExpandoObject Content { get; set; }
    }

    public class MyUser : MongoEntityBase
    {
        public string name { get; set; }
        public int age { get; set; }
        public string editDate { get; set; }
        public string amount { get; set; }
        public IEnumerable<string> qq { get; set; }
    }
}
