using System;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EG.Utility.DBCommon.dao;
using System.Data.Linq.Mapping;
using System.Collections.Generic;
using EG.Utility.DBCommon;

namespace EG.Utility.Test.dao
{
    [TestClass]
    public class Object2SQLTest
    {
        [TestMethod]
        public void TestObject2Object()
        {
            Object2Delete obj2del = new Object2Delete();

            TestObject testObject = new TestObject();
            testObject.id = 100;
            testObject.name = "MyName";
            obj2del.parse(testObject);

            Assert.AreEqual("delete from test_table where test_Id=@test_Id", obj2del.AsSql(), "sql assert");
            Assert.AreEqual(100, obj2del.GetSqlParameterValues()[0], "id assert");

            Object2Update obj2update = new Object2Update();
            obj2update.parse(testObject);

            Assert.AreEqual("update test_table set name=@name, desc_cn=@desc_cn where test_Id=@test_Id", obj2update.AsSql(), "sql assert");
            Assert.AreEqual(100, obj2update.GetSqlParameterValues()[obj2update.GetSqlParameterValues().Length - 1], "id assert");

            Object2Insert obj2insert = new Object2Insert();
            obj2insert.parse(testObject);

            Assert.AreEqual("insert into test_table(name, desc_cn, test_Id)values(@name, @desc_cn, @test_Id)", obj2insert.AsSql(), "sql assert");
            Assert.AreEqual("MyName", obj2insert.GetSqlParameterValues()[0], "name assert");

            testObject.id = null;
            obj2insert.parse(testObject);

            Assert.AreEqual("insert into test_table(name, desc_cn)values(@name, @desc_cn)", obj2insert.AsSql(), "sql assert");
            Assert.AreEqual("MyName", obj2insert.GetSqlParameterValues()[0], "name assert");
        }


        [TestMethod]
        public void SQLParser()
        {
            //SQLParser parser = new SQLParser();

            //TestObject testObject = new TestObject();
            //testObject.id = 100;
            //parser.Parse("select * from where id=[id] and name like [name]", testObject);

            //Assert.AreEqual("select * from where id=@id and name like @name", parser.AsSql(), "sql assert");
            //Assert.AreEqual(100, parser.GetSqlParameterValues()[0], "id assert");

        }

        [TestMethod]
        public void Dictionary2Where() {
            Dictionary2Where dict2where = new Dictionary2Where();
            IDictionary<string, object> dict = new Dictionary<string, object>();

            dict.Add("col1$>=", 1);
            dict.Add("col2", new List<object>() {2, 3});

            dict2where.parse(dict);

            Assert.AreEqual(" where 1=1 and col1>=@col1 and col2 in(@col2_0, @col2_1)", dict2where.AsSql(), "sql assert");
            Assert.AreEqual(2, dict2where.GetSqlParameterValues()[1], "id assert");
        }
    }

    [Table(Name="test_table")]
    class TestObject {
        [Column(IsPrimaryKey=true, Name="test_Id")]
        public int? id{get;set;}
        public string name{get;set;}
        
        [Column(Name="desc_cn")]
        public string desc_{get;set;} 
    }
}
