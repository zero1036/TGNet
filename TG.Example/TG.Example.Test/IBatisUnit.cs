using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TG.Example;

namespace TG.Example.Test
{
    [TestClass]
    public class IBatisUnit
    {
        private readonly UserService _service;
        public IBatisUnit()
        {
            _service = new UserService();
        }

        [TestMethod]
        public void TestMethod1()
        {
            User user = _service.GetUser(3);
            Assert.IsTrue(user.SysUserId == 1);
        }
    }
}
