using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TG.Example;

namespace TG.Example.Test
{
    [TestClass]
    public class AutofacUnit
    {
        [TestMethod]
        public void TestMethod1()
        {
            var op = new AutofacIIndex();
            op.RegistDym();
        }
    }
}
