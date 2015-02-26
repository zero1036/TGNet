using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EG.Utility.DBCommon.dao;

namespace UnitTest.dao
{
    [TestClass]
    public class SqlProviderTest
    {
        [TestInitialize]//加载指定的xml文件
        public void Init()
        {
            string s = UnitTest.Config.resource.sql.ToString();
            SqlProvider.LoadXml(new string[] { s });
        }


        [TestMethod]//读取数据
        public void GetTest()
        {
            string result = SqlProvider.Get("sql", "select");
        }
    }
}
