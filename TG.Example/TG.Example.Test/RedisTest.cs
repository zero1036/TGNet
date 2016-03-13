using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TG.Example.Test
{
    [TestClass]
    public class RedisTest
    {
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
    }
}
