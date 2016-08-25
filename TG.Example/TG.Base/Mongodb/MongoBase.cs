using System;
using System.Dynamic;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
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

        //private static IMongoDatabase GetDatabase(string key = "")
        public static IMongoDatabase GetDatabase(string key = "")
        {
            var connectString = "mongodb://localhost/MissionV2";
            var mongoUrl = new MongoUrl(connectString);
            var client = new MongoClient(connectString);
            return client.GetDatabase(mongoUrl.DatabaseName);
        }


        public void LoadCustomerId()
        {
            var connectString = "mongodb://localhost/WeixinApi";
            var mongoUrl = new MongoUrl(connectString);
            var client = new MongoClient(connectString);
            var db = client.GetDatabase(mongoUrl.DatabaseName);

            var prizes = db.GetCollection<LuckyPrize>("LuckyPrizeOlympic1").Find(x => true).ToList();

            var filter = string.Empty;
            var sql = string.Empty;
            List<string> result = new List<string>();

            foreach (var prize in prizes)
            {
                filter = prize.WinnerPhone;
                if (result.Contains(filter))
                {
                    continue;
                }
                else
                {
                    result.Add(filter);
                }

                sql = string.Format("{0},'{1}'", sql, filter);
            }
            sql = sql.Substring(1, sql.Length - 1);
            sql = string.Format("select Id,Phone from Customer where Phone in ({0})", sql);

            //if (System.IO.File.Exists(@"D:\moneyfix\money.sql"))
            //{
            //Console.WriteLine("文件已经存在，是否覆盖？（Y/N）");
            //string o = Console.ReadLine();
            //if (o == "y")
            //{
            System.IO.File.WriteAllText(@"D:\moneyfix\money.sql", sql);
            //}
            //}
        }

        public void LoadJS()
        {
            var dt = CSVFileHelper.OpenCSV(@"D:\moneyfix\lzc_phone.csv");
            Dictionary<string, string> dct = new Dictionary<string, string>();
            foreach (DataRow dr in dt.Rows)
            {
                dct.Add(dr[1].ToString(), dr[0].ToString());
            }

            var connectString = "mongodb://localhost/WeixinApi";
            var mongoUrl = new MongoUrl(connectString);
            var client = new MongoClient(connectString);
            var db = client.GetDatabase(mongoUrl.DatabaseName);

            var col = db.GetCollection<LuckyPrize>("LuckyPrizeOlympic1");
            var prizes = col.Find(x => true).ToList();
            foreach (var prize in prizes)
            {
                if (dct.ContainsKey(prize.WinnerPhone))
                {
                    prize.WinnerId = int.Parse(dct[prize.WinnerPhone]);
                    col.ReplaceOne(x => x.Id == prize.Id, prize);
                }
            }
        }


        public void BuildJs()
        {
            var connectString = "mongodb://localhost/WeixinApi";
            var mongoUrl = new MongoUrl(connectString);
            var client = new MongoClient(connectString);
            var db = client.GetDatabase(mongoUrl.DatabaseName);
            var col = db.GetCollection<LuckyPrize>("LuckyPrizeOlympic1");
            var prizes = col.Find(x => true).ToList();
            var js = JsonConvert.SerializeObject(prizes);
            var js2 = @"for (var i = 0, pEn; pEn = prizes[i++];) {
                            pEn.CreateDate = new Date(pEn.CreateDate);
                            pEn.Content.CreateDate = new Date(pEn.CreateDate);

                            var count = db.LuckyPrizeOlympic.count({ '$and': [{ 'MoneyKey': pEn.MoneyKey }, { 'WinnerPhone': pEn.WinnerPhone }] });                            
                            if (count == 0) {                                
                                db.LuckyPrizeOlympic.insert(pEn);
                                print(pEn.MoneyKey+'_'+pEn.WinnerPhone+':done');
                            }
                            else{
                                print(pEn.MoneyKey+'_'+pEn.WinnerPhone+':exists');
                            }
                            //printjson(pEn);
                        }";

            js = string.Format("var prizes = {0};{1}", js, js2);
            System.IO.File.WriteAllText(@"D:\moneyfix\moneystore.js", js);
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
