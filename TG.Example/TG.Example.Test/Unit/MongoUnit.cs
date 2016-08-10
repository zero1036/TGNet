using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TG.Example;

namespace TG.Example.Test
{
    [TestClass]
    public class MongoUnit
    {
        [TestMethod]
        public void FindAPI()
        {
            MongoBase mb = new MongoBase();
            mb.FindAPI();
        }

        [TestMethod]
        public void DynamicDataTest1()
        {
            MongoBase mb = new MongoBase();
            mb.DynamicDataTest1();
        }

        [TestMethod]
        public void DynamicDataTest2()
        {
            MongoBase mb = new MongoBase();
            mb.DynamicDataTest2();
        }

        [TestMethod]
        public void DynamicDataTest3()
        {
            MongoBase mb = new MongoBase();
            mb.DynamicDataTest3();
        }

        [TestMethod]
        public void LoadDynamicData()
        {
            MongoBase mb = new MongoBase();
            mb.LoadDynamicData();
        }

        [TestMethod]
        public void InsertMany()
        {
            MongoIndex mb = new MongoIndex();
            mb.InsertMany();
        }

        [TestMethod]
        public void InsertOne()
        {
            MongoIndex mb = new MongoIndex();
            mb.InsertOne();
        }

        [TestMethod]
        public void ValidateCollectionNull()
        {
            MongoIndex mb = new MongoIndex();
            mb.ValidateCollectionNull();
        }

        [TestMethod]
        public void CreateIndex()
        {
            MongoIndex mb = new MongoIndex();
            mb.CreateIndex();
        }

        [TestMethod]
        public void CountAll()
        {
            MongoAggregation mb = new MongoAggregation();
            mb.CountAll();
        }

        [TestMethod]
        public void Distinct()
        {
            MongoAggregation mb = new MongoAggregation();
            mb.Distinct();
        }

        [TestMethod]
        public void CreateCappedCollection()
        {
            MongoCollectionExp mc = new MongoCollectionExp();
            mc.CreateCappedCollection();
        }

        [TestMethod]
        public void MongoObjectIdSerialize()
        {
            MongoBase mb = new MongoBase();
            mb.MongoObjectIdSerialize();
        }

        [TestMethod]
        public void CappedColVsRedis1()
        {
            MongoCollectionExp mb = new MongoCollectionExp();
            mb.CappedColVsRedis1();
        }

        [TestMethod]
        public void UpdateOne1()
        {
            var up = new MongoUpdate();
            up.UpdateOne1();
        }

        [TestMethod]
        public void UpdateOne2()
        {
            var up = new MongoUpdate();
            up.UpdateOne2();
        }

        [TestMethod]
        public void UpdateOneNoMatched()
        {
            var up = new MongoUpdate();
            up.UpdateOneNoMatched();
        }

        [TestMethod]
        public void UpdateOneManyMatched()
        {
            var up = new MongoUpdate();
            up.UpdateOneManyMatched();
        }

        [TestMethod]
        public void UpdateMany()
        {
            var up = new MongoUpdate();
            up.UpdateMany();
        }

        [TestMethod]
        public void ReplaceOne()
        {
            var up = new MongoUpdate();
            up.ReplaceOne();
        }
        
        [TestMethod]
        public void MongoReplSetTest2()
        {
            int x = 1;
            var count = 10 * 10000;
            while (x <= count)
            {
                Random rd = new Random(DateTime.Now.Millisecond);
                var name = string.Format("olympic_{0}", rd.Next(10000));
                MongoReplSet me = new MongoReplSet();
                me.InsertSingle(name);
                x += 1;
            }
        }
    }
}
