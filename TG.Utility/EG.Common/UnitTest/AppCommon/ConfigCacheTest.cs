using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EG.Business.Common;
using EG.Utility.AppCommon;

namespace UnitTest.AppCommon
{
    [TestClass]
    public class ConfigCacheTest
    {
        [TestMethod]//加载指定的xml文件
        public void LoadAppConfigTest_files()
        {
            ConfigCache.LoadAppConfig(
                new string[] { "DB_PSW", "ZIP_PASSWORD_FORMAT", "MAIL_PASSWORD" },
                new string[] { "app.xml", "app.config" });
            Assert.IsTrue(ConfigCache.DC.Tables[0].Rows.Count == 25);
        }

        [TestMethod]//加载app.config文件
        public void LoadAppConfigTest()
        {
            ConfigCache.LoadAppConfig(new string[] { "DB_PSW", "ZIP_PASSWORD_FORMAT", "MAIL_PASSWORD" });
            Assert.IsTrue(ConfigCache.DC.Tables[0].Rows.Count == 18);
        }


        [TestMethod]
        public void LoadAppConfigTest2()
        {
            ConfigCache.LoadAppConfig(new string[] { "EncryptionStringTest" });
        }

        [TestMethod]//3.5时使用 调试之用,并非user case
        public void Appconfig_Decryption35()
        {
            ConfigCache.LoadAppConfig(new string[] { "MYSQL_DB_PSW" });
            Assert.AreEqual("Pw123456", ConfigCache.GetAppConfig("MYSQL_DB_PSW"));
        }

        [TestMethod]//4.0时使用 调试之用,并非user case
        public void Appconfig_Decryption40()
        {
            ConfigCache.LoadAppConfig(new string[] { "TEST_ENCRYPTION" });
            Assert.AreEqual("123456789", ConfigCache.GetAppConfig("TEST_ENCRYPTION"));
        }

        [TestMethod]//获取appconfig内容
        public void GetAppConfigTest()
        {
            ConfigCache.LoadAppConfig(new string[] { "DB_PSW", "ZIP_PASSWORD_FORMAT", "MAIL_PASSWORD" });
            Assert.AreEqual("sa", ConfigCache.GetAppConfig("SQLSERVER_DB_USER"));
        }

        [TestMethod]//获取appconfig内容 返回int类型
        public void GetIntAppConfigTest()
        {
            ConfigCache.LoadAppConfig(null);
            Assert.AreEqual(1, ConfigCache.GetIntAppConfig("TEST_INT"));
        }

        [TestMethod]//获取appconfig内容 返回long类型
        public void GetLongAppConfigTest()
        {
            ConfigCache.LoadAppConfig(null);
            Assert.AreEqual(123456789, ConfigCache.GetLongAppConfig("TEST_LONG"));
        }

        [TestMethod]//获取appconfig内容 返回double类型
        public void GetDoubleAppConfigTest()
        {
            ConfigCache.LoadAppConfig(null);
            Assert.AreEqual(5.23, ConfigCache.GetDoubleAppConfig("TEST_DOUBLE"));
        }

        [TestMethod]//获取appconfig内容 返回datetime类型
        public void GetDateTimeAppConfigTest()
        {
            ConfigCache.LoadAppConfig(null);
            Assert.AreEqual(new DateTime(2013, 2, 25), ConfigCache.GetDateTimeAppConfig("TEST_DATETIME"));
        }

        [TestMethod]//获取数据库连接语句 sqlserver
        public void GetDBConnectStrTest_SQLSERVER()
        {
            ConfigCache.LoadAppConfig(null);
            Assert.AreEqual(
                "User ID=sa;initial catalog=DCETLBig;Data Source=sep;Password=Pw123456;",
                ConfigCache.GetDBConnectStr("SQLSERVER"));
        }

        [TestMethod]//获取数据库连接语句 oracle
        public void GetDBConnectStrTest_ORACLE()
        {
            ConfigCache.LoadAppConfig(null);
            Assert.AreEqual(
                "Data Source=172.30.1.65:1522/xe;user id=czjd_sd;password=czjd_sd;",
                ConfigCache.GetDBConnectStr("ORACLE"));
        }

        [TestMethod]//获取数据库连接语句 mysql 暂时未实现
        public void GetDBConnectStrTest_MYSQL()
        {
            ConfigCache.LoadAppConfig(null); Assert.AreEqual(
            "",
        ConfigCache.GetDBConnectStr("MYSQL"));
        }

        [TestMethod]//获取数据库类型 返回short类型 oracle-0 sqlserver-1
        public void GetDBTypeTest()
        {
            ConfigCache.LoadAppConfig(null);
            Assert.AreEqual(0, ConfigCache.GetDBType("ORACLE"));
        }


    }
}
