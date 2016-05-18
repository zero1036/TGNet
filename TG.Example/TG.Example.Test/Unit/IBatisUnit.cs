using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TG.Example;
using System.IO;

namespace TG.Example.Test
{
    [TestClass]
    public class IBatisUnit
    {
        private readonly UserService _service;
        public IBatisUnit()
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Log4net.config"));

            _service = new UserService();
        }

        [TestMethod]
        public void TestMethod1()
        {
            User user = _service.GetUser(3);
            Assert.IsTrue(user.SysUserId == 1);
        }

        [TestMethod]
        public void TestMethod2()
        {
            int res = _service.Update();
            Assert.IsTrue(res == 1);
        }

        [TestMethod]
        public void Transaction_Commit()
        {
            var ts = new TransactionTest();
            ts.Commit();
        }

        [TestMethod]
        public void Transaction_Rolback()
        {
            var ts = new TransactionTest();
            ts.Rollback();
        }
    }
}
