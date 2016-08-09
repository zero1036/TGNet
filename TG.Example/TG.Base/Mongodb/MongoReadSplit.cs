using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TG.Example
{
    /// <summary>
    /// Mongo读写分离.
    /// </summary>
    public class MongoReadSplit
    {
        public bool ReadFromSecondary()
        {
            IMongoDatabase db = GetDatabase();

            //显示db setting isFrozen，未知原因，预计是从节点只读只能设置在Collection，不能设在主节点
            //db.Settings.ReadPreference = ReadPreference.Secondary;

            //primary:默认参数，只从主节点上进行读取操作； 
            //primaryPreferred:大部分从主节点上读取数据,只有主节点不可用时从secondary节点读取数据。 
            //secondary:只从secondary节点上进行读取操作，存在的问题是secondary节点的数据会比primary节点数据“旧”。 
            //secondaryPreferred:优先从secondary节点进行读取操作，secondary节点不可用时从主节点读取数据； 
            //nearest:不管是主节点、secondary节点，从网络延迟最低的节点上读取数据
            var collectionSetting = new MongoCollectionSettings();
            //collectionSetting.ReadPreference = ReadPreference.Secondary;

            //指定只读Collection
            IList<TagSet> tagSets = new List<TagSet>();
            IList<Tag> tags = new List<Tag>();
            tags.Add(new Tag("", ""));
            tagSets.Add(new TagSet(tags));
            collectionSetting.ReadPreference = new ReadPreference(ReadPreferenceMode.Secondary, tagSets);

            var col = db.GetCollection<lkmoney>("lkmoney", collectionSetting);
            var obj = col.Find(x => x.creator == "sys").SingleOrDefault();
            Debug.WriteLine(obj.name);

            return !string.IsNullOrEmpty(obj.name);
        }

        private static IMongoDatabase GetDatabase(string key = "")
        {
            List<MongoServerAddress> servers = new List<MongoServerAddress>();
            servers.Add(new MongoServerAddress("morton", 10001));
            servers.Add(new MongoServerAddress("morton", 10002));
            servers.Add(new MongoServerAddress("morton", 10003));

            var mongoSetting = new MongoClientSettings();
            mongoSetting.Servers = servers;

            var client = new MongoClient(mongoSetting);
            return client.GetDatabase("test");
        }
    }
}
