using System;
using System.Xml;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EG.Utility.DBCommon.dao;
using System.Data;
using System.Data.Linq.Mapping;
using System.Collections.Generic;
using EG.Business.Common;

namespace EG.Utility.DBCommon.dao
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void TestPerforman()
        {
            DateTime timeNow = DateTime.Now;
            //Console.Out.WriteLine("time now:" + (DateTime.Now));

            for (int i = 0; i < 1000; i++)
            {
                string constr = ConfigCache.GetDBConnectStr("SQLSERVER");
                MySQLHelper.ExecuteDataset(constr, CommandType.Text, "select * from city ", null);
                MySQLHelper.ExecuteDataset(constr, CommandType.Text, "select * from city ", null);
                MySQLHelper.ExecuteDataset(constr, CommandType.Text, "select * from city ", null);
            }

            //Console.Out.WriteLine("time now:" + (DateTime.Now));
            Console.Out.WriteLine("Aop use time:" + timeNow.Subtract(DateTime.Now).Duration());

            timeNow = DateTime.Now;
            //Console.Out.WriteLine("time now:" + (DateTime.Now));
            
            for (int i = 0; i < 1000; i++)
            {
                BizBO bizBo = TransactionAOP.newInstance(typeof(BizBO)) as BizBO;

                bizBo.queryCity();

            }

            //Console.Out.WriteLine("time now:" + (DateTime.Now));
            Console.Out.WriteLine("No Aop use time:" + timeNow.Subtract(DateTime.Now).Duration());


        }


        [TestMethod]
        public void TestMethod1()
        {

            BizBO bizBo = TransactionAOP.newInstance(typeof(BizBO)) as BizBO;

            bizBo.add();
        }

        //TestMethod2() for testing Row2Object()
        [TestMethod]
        public void TestMethod_Row2Object()
        {
            BizBO bizBo = TransactionAOP.newInstance(typeof(BizBO)) as BizBO;
            DataRow dr = bizBo.add2();
            MyCity city = DBUtil.Row2Object<MyCity>(dr);
            Assert.AreEqual(1, city.city_id, "id is 1");
            //BizBO bizBo = TransactionAOP.newInstance(typeof(BizBO)) as BizBO;
            //DataRow dr = bizBo.add2();
            //TimeSpan ts1 = new TimeSpan(DateTime.Now.Ticks); //获取当前时间的刻度数
            //for (int i = 0; i < 3000; i++)
            //{
            //    MyCity city = DBUtil.Row2Object<MyCity>(dr);
            //}
            //TimeSpan ts2 = new TimeSpan(DateTime.Now.Ticks);
            //TimeSpan ts = ts2.Subtract(ts1).Duration(); //时间差的绝对值
            //string spanTotalSeconds = ts.TotalSeconds.ToString(); //执行时间的总秒数
            //Console.WriteLine("执行时间总秒数:" + spanTotalSeconds);
        }

        //TestMethod3() for testing Row2Object()
        [TestMethod]
        public void TestMethod_Row2Object2()
        {
            BizBO bizBo = TransactionAOP.newInstance(typeof(BizBO)) as BizBO;
            DataRow dr = bizBo.add2();
            MyCity2 city = DBUtil.Row2Object<MyCity2>(dr);
            Assert.AreEqual(0, city.city_id1, "id ");
            Assert.AreEqual("A Corua (La Corua)", city.city, "cityname");
            Assert.AreEqual(87, city.country_id, "countryid");
        }

        //TestMethod4() for testing Table2List()
        [TestMethod]
        public void TestMethod_Table2List()
        {
            BizBO bizBo = TransactionAOP.newInstance(typeof(BizBO)) as BizBO;
            DataTable table = bizBo.add3();
            IList<MyCity> city = DBUtil.Table2List<MyCity>(table);
            string s = city[9].city;
            Assert.AreEqual("Akishima", s, "cityname");           
        }

        //TestMethod5() for testing Table2List()
        [TestMethod]
        public void TestMethod_Table2List2()
        {
            BizBO bizBo = TransactionAOP.newInstance(typeof(BizBO)) as BizBO;
            DataTable table = bizBo.add3();
            IList<MyCity2> city = DBUtil.Table2List<MyCity2>(table);
            int s = city[9].city_id1;
            Assert.AreEqual(0, s, "cityid");
        }

    }

    [DBSourceAttribute("SQLServer")]
    public class BizBO
    {

        public virtual void queryCity()
        {
            ADOTemplate template = new ADOTemplate();
            template.Query("select * from city ", null, null);
            template.Query("select * from city ", null, null);
            template.Query("select * from city ", null, null);

        }

        public virtual void add()
        {
            ADOTemplate template = new ADOTemplate();
            DataTable table = template.Query("select 1 from city where city_id in (@id)", new String[] { "id" }, new Object[] { new int[] { 618, 617 } });

            int count1 = table.Rows.Count;

            template.Execute("insert into city (city_id, city, country_id, last_update) values(@city_id, @city, @country_id, @last_update) ",
                new string[] { "city_id", "city", "country_id", "last_update" },
                new object[] { null, "test city", 19, DateTime.Now });

            table = template.Query("select 1 from city", null, null);

            int count2 = table.Rows.Count;

            Assert.AreEqual(count1 + 1, count2, "insert new row, rows's total will add one!");
        }


        public virtual DataRow add2()
        {
            ADOTemplate template = new ADOTemplate();
            DataTable table = template.Query("select * from city", null, null);
            DataRow row = table.Rows[0];
            return row;
        }

        public virtual DataTable add3()
        {
            ADOTemplate template = new ADOTemplate();
            DataTable table = template.Query("select * from city limit 10", null, null);
            return table;
        }

        public virtual void Insert()
        {
            ADOTemplate template = new ADOTemplate();
            //template.Insert();
        }
    }

    //true test case :for test  DBUtil.Row2Object() & Table2List()
    public class MyCity
    {
        public int city_id { get; set; }
        public string city { get; set; }
        public int country_id { get; set; }
    }

    //wrong test case{DB no the city_id1} :for test DBUtil.Row2Object() & Table2List()
    public class MyCity2
    {
        public int city_id1 { get; set; }
        public string city { get; set; }
        public int country_id { get; set; }
    }



}
