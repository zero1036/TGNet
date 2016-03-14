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
            rt.FragRatioTest(10, 10 * 10000);
        }

        [TestMethod]
        /// <summary>
        /// FragRatioTest
        /// </summary>
        /// <param name="factor">倍数因子</param>
        public void FragRatioTest_13()
        {
            var rt = new RedisTestData();
            rt.FragRatioTest(13, 9 * 10000);
        }
        #endregion
    }
}
