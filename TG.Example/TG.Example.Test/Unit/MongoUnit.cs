﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TG.Example;

namespace TG.Example.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void FindAPI()
        {
            MongoBase mb = new MongoBase();
            mb.FindAPI();
        }

        [TestMethod]
        public void DynamicDataTest1()
        {
            MongoBase mb = new MongoBase();
            mb.DynamicDataTest1();
        }

        [TestMethod]
        public void DynamicDataTest2()
        {
            MongoBase mb = new MongoBase();
            mb.DynamicDataTest2();
        }

        [TestMethod]
        public void DynamicDataTest3()
        {
            MongoBase mb = new MongoBase();
            mb.DynamicDataTest3();
        }

        [TestMethod]
        public void LoadDynamicData()
        {
            MongoBase mb = new MongoBase();
            mb.LoadDynamicData();
        }

        [TestMethod]
        public void InsertMany()
        {
            MongoIndex mb = new MongoIndex();
            mb.InsertMany();
        }
    }
}
