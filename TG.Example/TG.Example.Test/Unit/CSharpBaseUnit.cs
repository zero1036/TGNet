using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TG.Example;
using System.IO;

namespace TG.Example.Test
{
    [TestClass]
    public class CSharpBaseUnit
    {
        [TestMethod]
        public void ExpressLearn_Start()
        {
            ExpressLearn el = new ExpressLearn();
            el.Start();
        }

        [TestMethod]
        public void ExpressAndLearn()
        {
            ExpressLearn el = new ExpressLearn();
            el.TestAnd();
        }

    }
}
