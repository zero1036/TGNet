using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
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

        //[Fact]
        //public void TestAdd()
        //{
        //    Assert.Equal<int>(5, 2 + 3);
        //}

        [Fact(Timeout = 900)]//指定超时为900ms 
        public void TestTimeout()
        {
            System.Threading.Thread.Sleep(1000);
            Assert.InRange<double>(new Random().NextDouble() * 10, 5, 10);
        }

        [Fact]
        public void TestAdd1()
        {
            
            var pUser = new CurUserM();
            pUser.SysUserId = 1;
            //设置当前用户
            SysCurUser.SetCurUser(pUser);

            TW.Platform.BL.OrgBL org = new TW.Platform.BL.OrgBL();
            var v = org.GetUsersByDepId(1);
            Assert.NotEmpty(v);
        }

        //[Fact]
        //public void TestObjectX()
        //{           

        //    //var pUser = new CurUserM();
        //    //pUser.SysUserId = 1;
        //    ////设置当前用户
        //    //SysCurUser.SetCurUser(pUser);

        //    //TW.Platform.BL.OrgBL org = new TW.Platform.BL.OrgBL();
        //    //var v = org.GetUsersByDepId(1);
        //    //Assert.NotEmpty(v);

        //    Assert.Equal<int>(5, 2 + 3);
        //}

        public void Dispose()
        {
            System.Console.WriteLine("Dispose");
        }
    }
}
