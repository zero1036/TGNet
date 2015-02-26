using EG.UnitTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EG.Business.Common
{
    [TestClass]
    public class ConfigCacheDBLoaderTest:BaseTest
    {
        public ConfigCacheDBLoaderTest()
        {
            dataPath = System.AppDomain.CurrentDomain.BaseDirectory + "/AppCommon/data/";
        }

        public override void AfterDataInit()
        {
            CacheConfiguration config = new CacheConfiguration();

            config.Add("SQLSERVER", "CacheTestTable", "select * from CacheTestTable", null);
            config.Add("SQLSERVER", "CacheTestTable1", "select * from CacheTestTable", null);

            ConfigCache.Load(config);
        }

        [TestMethod]
        public void LoadCacheTest()
        {
            Assert.AreEqual("cxr", ConfigCache.Get("CacheTestTable", 1000L)["name_"]);
            Assert.AreEqual("cxr", ConfigCache.Get("CacheTestTable1", 1000L)["name_"]);
            Assert.AreEqual(1000L, ConfigCache.GetLong("CacheTestTable", "id", 1000L));
            Assert.AreEqual("cxr", ConfigCache.GetObject("CacheTestTable", "name_", 1000L));
            Assert.AreEqual(100.01D, ConfigCache.GetDouble("CacheTestTable", "amount_", 1000L));
            Assert.AreEqual("2013-12-14 11:10:10", ConfigCache.GetDateTime("CacheTestTable", "created_on", 1000L).ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}
