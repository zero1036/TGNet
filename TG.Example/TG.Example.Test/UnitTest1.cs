using System;
using Xunit;
using TG.Example;

namespace TG.Example.Test
{
    public class UnitTest1
    {
        [Fact]
        public void TestMethod1()
        {
            var ts = new RedisTest();
            ts.StringSet();
        }
    }
}
