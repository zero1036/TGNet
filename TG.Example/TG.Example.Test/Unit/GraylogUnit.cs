using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TG.Example;
using System.IO;

namespace TG.Example.Test
{
    [TestClass]
    public class GraylogUnit
    {
        [TestMethod]
        public void SearchKeyword_Start()
        {
            GraylogApi el = new GraylogApi();
            el.SearchKeyword();
        }

    }
}
