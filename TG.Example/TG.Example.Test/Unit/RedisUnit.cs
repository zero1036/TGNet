using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TG.Example;

namespace TG.Example.Test
{
    /// <summary>
    /// RedisTest 的摘要说明
    /// </summary>
    [TestClass]
    public class RedisUnit
    {
        #region hash 切分
        [TestMethod]
        public void HashSplit_Full()
        {
            var rt = new RedisTestData();
            rt.HashSplit_Full();
        }

        [TestMethod]
        public void HashSplit_ByHash()
        {
            var rt = new RedisTestData();
            rt.HashSplit_ByHash();
        }

        [TestMethod]
        public void HashSplit_OneHash()
        {
            var rt = new RedisTestData();
            rt.HashSplit_OneHash();
        }

        #endregion

        #region Bitmap 测试
        [TestMethod]
        public void TestMethod1()
        {
            var rt = new RedisTestData();
            rt.AddNum();
        }

        [TestMethod]
        public void BitmapSet()
        {
            var rt = new RedisString();
            rt.BitmapSet();
        }

        [TestMethod]
        public void BitmapGet()
        {
            var rt = new RedisString();
            rt.BitmapGet();
        }

        [TestMethod]
        public void BitmapOperation_And()
        {
            var rt = new RedisString();
            rt.BitmapOperation_And();
        }

        [TestMethod]
        public void BitmapCompareMethod()
        {
            var rt = new RedisString();
            rt.BitmapCompareMethod();
        }
        #endregion

        #region 碎片率实验  Fragmentation ratio

        [TestMethod]
        /// <summary>
        /// FragRatioTest
        /// </summary>
        /// <param name="factor">倍数因子</param>
        public void FragRatioTest_10()
        {
            var rt = new RedisTestData();
            rt.FragRatioTest(10, 100 * 10000);
        }

        #region 指定
        [TestMethod]
        /// <summary>
        /// FragRatioTest
        /// </summary>
        /// <param name="factor">倍数因子</param>
        public void FragRatioTest_13()
        {
            var rt = new RedisTestData();
            rt.FragRatioTest(13, 90 * 10000);
        }

        [TestMethod]
        /// <summary>
        /// FragRatioTest
        /// </summary>
        /// <param name="factor">倍数因子</param>
        public void FragRatioTest_17()
        {
            var rt = new RedisTestData();
            rt.FragRatioTest(17, 90 * 10000);
        }

        [TestMethod]
        /// <summary>
        /// FragRatioTest
        /// </summary>
        /// <param name="factor">倍数因子</param>
        public void FragRatioTest_21()
        {
            var rt = new RedisTestData();
            rt.FragRatioTest(21, 90 * 10000);
        }
        #endregion

        #region 随机
        [TestMethod]
        public void FragRatioTestRamdom_13()
        {
            var rt = new RedisTestData();
            rt.FragRatioTestRamdom(13, 90 * 10000);
        }

        [TestMethod]
        public void FragRatioTestRamdom_17()
        {
            var rt = new RedisTestData();
            rt.FragRatioTestRamdom(17, 90 * 10000);
        }

        [TestMethod]
        public void FragRatioTestRamdom_21()
        {
            var rt = new RedisTestData();
            rt.FragRatioTestRamdom(21, 90 * 10000);
        }
        #endregion

        #endregion

        #region 事务
        [TestMethod]
        public void ConditionTest()
        {
            var rt = new RedisTrans();
            rt.ConditionTest();
        }
        #endregion

        #region List
        [TestMethod]
        public void ListTest()
        {
            RedisList list = new RedisList();
            list.ListTest();
        }

        [TestMethod]
        public void ListCapacityTest()
        {
            RedisList list = new RedisList();
            list.ListCapacityTest();
        }
        #endregion

        #region Lua
        [TestMethod]
        public void PPlkMoney1Test()
        {
            RobMoney rb = new RobMoney();
            rb.PPlkMoney1();
        }
        #endregion

    }
}
