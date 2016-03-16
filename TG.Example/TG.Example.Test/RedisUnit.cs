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
        public void HashSplit_Common()
        {
            var rt = new RedisTestData();
            rt.HashSplit_Common();
        }

        [TestMethod]
        public void HashSplit_ByHashHead2()
        {
            var rt = new RedisTestData();
            rt.HashSplit_ByHashHead2();
        }

        [TestMethod]
        public void HashSplit_ByHashBack2()
        {
            var rt = new RedisTestData();
            rt.HashSplit_ByHashBack2();
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
    }
}
