using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TG.Example;

namespace TG.Example.Test
{
      [TestClass]
    public class QuartzUnit
    {
        [TestMethod]
        public void Main_Test()
        {
            QuartzClass qu = new QuartzClass();
            qu.Main();
        }
    }
}
