using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using TW.Platform.Sys;
using TW.Platform.Model;

namespace xUnitTest
{
    public class XunitDemo : IDisposable
    {
        public XunitDemo()
        {
            System.Console.WriteLine("Init");
        }

        [Fact]
        public void TestAdd()
        {
            Assert.Equal<int>(5, 2 + 3);
        }

        [Fact(Timeout = 900)]//指定超时为900ms 
        public void TestTimeout()
        {
            System.Threading.Thread.Sleep(1000);
            Assert.InRange<double>(new Random().NextDouble() * 10, 5, 10);
        }

        [Fact]
        public void TestMoqInterface()
        {
            var mo = new Mock<IFake>();

            mo.Setup(p => p.DoSomething("Ping")).Returns(true);

            Assert.Equal<bool>(true, mo.Object.DoSomething("Ping"));
        }

        [Fact]
        public void TestMoqClass()
        {
            var mo = new Mock<FakeClass>();

            mo.Setup(p => p.DoSomething("Ping")).Returns(true);

            Assert.Equal<bool>(true, mo.Object.DoSomething("Ping"));
        }

        [Fact]
        public void TestOrg1()
        {
            var pUser = new CurUserM();
            pUser.SysUserId = 1;
            //设置当前用户
            SysCurUser.SetCurUser(pUser);

            TW.Platform.BL.OrgBL org = new TW.Platform.BL.OrgBL();
            var v = org.GetUsersByDepId(1);
            Assert.NotEmpty(v);
        }

        public void Dispose()
        {
            System.Console.WriteLine("Dispose");
        }
    }

    public interface IFake
    {
        bool DoSomething(string actionName);
    }

    public class FakeClass
    {
        public virtual bool DoSomething(string actionName)
        {
            return false;
        }
    }
}
