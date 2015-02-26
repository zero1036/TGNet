using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using EG.Utility.DBCommon.dao;
using EG.Business.Common;
using System.Collections.Generic;
using Castle.Components.DictionaryAdapter;
using System.Data.Linq.Mapping;
using System.Collections;
using System.Text;
using EG.Common.Entity;

namespace UnitTest.dao
{
    [TestClass]
    public class ADOTemplateUseCase
    {
        ADOTemplateTestClass ado = TransactionAOP.newInstance(typeof(ADOTemplateTestClass)) as ADOTemplateTestClass;

        OracleADOTemplateTestClass GetOracleADOInstance()
        {
            return TransactionAOP.newInstance(typeof(OracleADOTemplateTestClass)) as OracleADOTemplateTestClass;
        }

        SqlServerADOTemplateTestClass GetSqlServerADOInstance()
        {
            return TransactionAOP.newInstance(typeof(SqlServerADOTemplateTestClass)) as SqlServerADOTemplateTestClass;
        }

        MysqlADOTemplateTestClass GetMySqlADOInstance()
        {
            return TransactionAOP.newInstance(typeof(MysqlADOTemplateTestClass)) as MysqlADOTemplateTestClass;
        }
        

        /// <summary>
        /// 初始化数据 加载appconfig、template模板内容
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            ConfigCache.LoadAppConfig(null);
            string s = UnitTest.Config.resource.sql.ToString();
            SqlProvider.LoadXml(new string[] { s });
        }


        /// <summary>
        /// 测试数据库连接是否ok,同时测试oracle、sqlserver数据库源
        /// </summary>
        [TestMethod]
        public void ConnectTest()
        {
            //oracle:
            DataTable dt = GetOracleADOInstance().ConnectTest();
            Assert.IsTrue(dt.Rows.Count > 0);
            //sqlserver:
            dt = GetSqlServerADOInstance().ConnectTest();
            Assert.IsTrue(dt.Rows.Count > 0);
        }


        /// <summary>
        /// 插入数据，同时测试oracle、sqlserver数据库源
        /// </summary>
        [TestMethod]
        public void InsertTest()
        {
            //测试前请保证插入的数据之前是不存在的
            //oracle:
            UserInfo u = new UserInfo() { loginid = 1004, name = "bearaaa", age = 5, birthday = DateTime.Now, gender = "M" };
            int result = GetOracleADOInstance().Insert(u);
            Assert.AreEqual(1, result);

            //sqlserver:
            result = GetSqlServerADOInstance().Insert(u);
            Assert.AreEqual(1, result);
        }


        /// <summary>
        /// 更新数据并获取数据
        /// 同时测试oracle、sqlserver数据库源
        /// </summary>
        [TestMethod]
        public void UpdateAndGetTest()
        {
            UserInfo u = new UserInfo() { id = 2 };
            //oracle:
            Assert.AreEqual(1, GetOracleADOInstance().UpdateAfterGet<UserInfo>(u));
            Assert.AreEqual("bb", GetOracleADOInstance().Get(u).name);
            //sqlserver:
            Assert.AreEqual(1, GetSqlServerADOInstance().UpdateAfterGet<UserInfo>(u));
            Assert.AreEqual("bb", GetSqlServerADOInstance().Get(u).name);
        }



        /// <summary>
        /// 更新数据 -- 数据库中已经有此数据
        /// 同时测试oracle、sqlserver数据库源
        /// </summary>
        [TestMethod]
        public void UpdateTest_hasdata()
        {
            UserInfo u = new UserInfo() { id = 1, loginid = 1009, name = "bear", age = 5, birthday = DateTime.Now, gender = "M" };
            //oracle:
            int result = GetOracleADOInstance().Update(u);
            Assert.AreEqual(1, result);
            Assert.AreEqual(u.name, GetOracleADOInstance().Get(u).name);

            //sqlserver:
            result = GetSqlServerADOInstance().Update(u);
            Assert.AreEqual(1, result);
            Assert.AreEqual(u.name, GetSqlServerADOInstance().Get(u).name);
        }




        /// <summary>
        /// 更新数据 -- 数据库没有此数据
        /// 同时测试oracle、sqlserver数据库源
        /// </summary>
        [TestMethod]
        public void UpdateTest_nodata()
        {
            UserInfo u = new UserInfo() { id = 0, loginid = 1006, name = "bear", age = 5, birthday = DateTime.Now, gender = "M" };
            
            //oracle:
            int result = GetOracleADOInstance().Update(u);
            Assert.AreEqual(0, result);

            //sqlserver:
            result = GetSqlServerADOInstance().Update(u);
            Assert.AreEqual(0, result);
        }




        /// <summary>
        /// 删除数据 -- 有主键并且其余数据正确
        /// 同时测试oracle、sqlserver数据库源
        /// </summary>
        [TestMethod]
        public void DeleteTest_hasPK_correctData()
        {
            UserInfo u = new UserInfo() { id = 4, loginid = 1006, name = "bear", age = 5, birthday = DateTime.Now, gender = "M" };
            //oracle:
            int result = GetOracleADOInstance().Delete(u);
            Assert.AreEqual(1, result);

            //sqlserver:
            result = GetSqlServerADOInstance().Delete(u);
            Assert.AreEqual(1, result);
        }




        /// <summary>
        /// 删除数据 -- 有主键但是含有不正确的数据
        /// 同时测试oracle、sqlserver数据库源
        /// </summary>
        [TestMethod]
        public void DeleteTest_hasPK_wrongData()
        {
            UserInfo u = new UserInfo() { id = 3, loginid = 1006, name = "bear", age = 5, birthday = DateTime.Now, gender = "M" };
            //oracle:
            int result = GetOracleADOInstance().Delete(u);
            Assert.AreEqual(1, result);

            //sqlserver:
            result = GetSqlServerADOInstance().Delete(u);
            Assert.AreEqual(1, result);
        }




        /// <summary>
        /// 删除数据 -- 没有主键
        /// 同时测试oracle、sqlserver数据库源
        /// </summary>
        [TestMethod]
        public void DeleteTest_noPK()
        {
            UserInfo u = new UserInfo() { id = 0, loginid = 1006, name = "bear", age = 5, birthday = DateTime.Now, gender = "M" };
            //oracle:
            int result = GetOracleADOInstance().Delete(u);
            Assert.AreEqual(0, result);

            //sqlserver:
            result = GetSqlServerADOInstance().Delete(u);
            Assert.AreEqual(0, result);
        }

        /// <summary>
        /// 获取数据 -- 通过对象 主键
        /// 同时测试oracle、sqlserver数据库源
        /// </summary>
        [TestMethod]
        public void GetTest_Genericity()//get只查找主键
        {
            UserInfo u = new UserInfo() { id=1};
            //oracle:
            UserInfo u2 = GetOracleADOInstance().Get<UserInfo>(u);
            Assert.AreEqual(1009, u2.loginid);

            //sqlserver:
            u2 = GetSqlServerADOInstance().Get<UserInfo>(u);
            Assert.AreEqual(1009, u2.loginid);
        }

        /// <summary>
        /// 获取数据 -- 通过对象各种条件查询,返回一条数据
        /// 同时测试oracle、sqlserver数据库源
        /// </summary>
        [TestMethod]
        public void FindTest()
        {
            UserInfo u = new UserInfo() { age = 24,pay=6500 };
            //oracle:
            UserInfo lu = GetOracleADOInstance().Find(u);
            Assert.IsTrue(lu.age == 24);

            //sqlserver:
            lu = GetSqlServerADOInstance().Find(u);
            Assert.IsTrue(lu.age == 24);
        }
        /// <summary>
        /// 获取数据 -- 返回int类型 无参数
        /// 同时测试oracle、sqlserver数据库源
        /// </summary>
        [TestMethod]
        public void GetIntTest()
        {
            string sql = "select age from EGCommonADOTest where id=1";
            //oracle:
            Assert.AreEqual(5, GetOracleADOInstance().GetInt(sql));

            //sqlserver:
            Assert.AreEqual(5, GetSqlServerADOInstance().GetInt(sql));

        }
        /// <summary>
        /// 获取数据 -- 返回int类型 有参数
        /// 同时测试oracle、sqlserver数据库源
        /// </summary>
        [TestMethod]
        public void GetIntTest_hasParam()
        {
            //oracle:
            string sql = "select age from EGCommonADOTest where birthday= :birthday and login_id=:id";
            string[] paramNames = { ":birthday", ":id" };
            object[] paramValues = {new DateTime (2013,12,14) , 1011 };
            Assert.AreEqual(24, GetOracleADOInstance().GetInt(sql, paramNames, paramValues));

            //sqlserver:
            sql = "select age from EGCommonADOTest where birthday= @birthday and login_id=@id";
            string[] paramNames2 = { "@birthday", "@id" };
            object[] paramValues2 = { new DateTime(2013, 12, 09), 1001 };
            Assert.AreEqual(21, GetSqlServerADOInstance().GetInt(sql, paramNames2, paramValues2));
        }
        /// <summary>
        /// 获取数据 -- 返回int类型 有参数 有default值,即查无值,则返回一个规定值
        /// 同时测试oracle、sqlserver数据库源
        /// </summary>
        [TestMethod]
        public void GetIntTest_hasParam_defaultValue()
        {
            //oracle:
            string sql = "select age from EGCommonADOTest where birthday= :birthday and id=:id";
            string[] paramNames = { ":birthday", ":id" };
            object[] paramValues = { new DateTime(2013, 12, 09), "0" };
            Assert.AreEqual(5, GetOracleADOInstance().GetInt(sql, paramNames, paramValues, 5));

            //sqlserver:
            sql = "select age from EGCommonADOTest where birthday= @birthday and id=@id";
            string[] paramNames2 = { "@birthday", "@id" };
            object[] paramValues2 = { new DateTime(2013, 12, 09), "0" };
            Assert.AreEqual(5, GetSqlServerADOInstance().GetInt(sql, paramNames2, paramValues2, 5));
        }

        /// <summary>
        /// 获取数据 -- 返回long类型 无参数
        /// 同时测试oracle、sqlserver数据库源
        /// </summary>
        [TestMethod] 
        public void GetLongTest()
        {
            string sql = "select login_id from EGCommonADOTest where id=1";
            //oracle:
            Assert.AreEqual(1009, GetOracleADOInstance().GetLong(sql));
            //sqlserver:
            Assert.AreEqual(1009, GetSqlServerADOInstance().GetLong(sql));

        }
        /// <summary>
        /// 获取数据 -- 返回Long类型 有参数
        /// 同时测试oracle、sqlserver数据库源
        /// </summary>
        [TestMethod]
        public void GetLongTest_hasParam()
        {
            //oracle:
            string sql = "select login_id from EGCommonADOTest where birthday= :birthday and id=:id";
            string[] paramNames = { ":birthday", ":id" };
            object[] paramValues = { new DateTime(2013, 12, 14), "2" };
            Assert.AreEqual(1011, GetOracleADOInstance().GetLong(sql, paramNames, paramValues));

            //sqlserver:
            sql = "select login_id from EGCommonADOTest where birthday= @birthday and id=@id";
            string[] paramNames2 = { "@birthday", "@id" };
            object[] paramValues2 = { new DateTime(2013, 12, 9), "2" };
            Assert.AreEqual(1001, GetSqlServerADOInstance().GetLong(sql, paramNames2, paramValues2));
        }
        /// <summary>
        /// 获取数据 -- 返回long类型 有参数 有default值,即查无值,则返回一个规定值
        /// 同时测试oracle、sqlserver数据库源
        /// </summary>
        [TestMethod]
        public void GetLongTest_hasParam_defaultValue()
        {
            //oracle:
            string sql = "select login_id from EGCommonADOTest where birthday= :birthday and id=:id";
            string[] paramNames = { ":birthday", ":id" };
            object[] paramValues = { new DateTime(2013, 12, 09), "0" };
            Assert.AreEqual(5, GetOracleADOInstance().GetLong(sql, paramNames, paramValues, 5));
            
            //sqlserver:
            sql = "select login_id from EGCommonADOTest where birthday= @birthday and id=@id";
            string[] paramNames2 = { "@birthday", "@id" };
            object[] paramValues2 = { new DateTime(2013, 12, 09), "0" };
            Assert.AreEqual(5, GetSqlServerADOInstance().GetLong(sql, paramNames2, paramValues2, 5));
        }
        /// <summary>
        /// 获取数据 -- 返回double类型 无参数
        /// 同时测试oracle、sqlserver数据库源
        /// </summary>
        [TestMethod]
        public void GetDoubleTest()
        {
            string sql = "select pay from EGCommonADOTest where id=1";
            //oracle:
            Assert.AreEqual(0.00, GetOracleADOInstance().GetDouble(sql));

            //sqlserver:
            Assert.AreEqual(0.00, GetSqlServerADOInstance().GetDouble(sql));
        }
        /// <summary>
        /// 获取数据 -- 返回double类型 有参数
        /// 同时测试oracle、sqlserver数据库源
        /// </summary>
        [TestMethod]
        public void GetDoubleTest_hasParam()
        {
            //oracle:
            string sql = "select pay from EGCommonADOTest where birthday= :birthday and id=:id";
            string[] paramNames = { ":birthday", ":id" };
            object[] paramValues = { new DateTime(2013, 12, 14), "2" };
            Assert.AreEqual(2500, GetOracleADOInstance().GetDouble(sql, paramNames, paramValues));

            //sqlserver:
            sql = "select pay from EGCommonADOTest where birthday= @birthday and id=@id";
            string[] paramNames2 = { "@birthday", "@id" };
            object[] paramValues2 = { new DateTime(2013, 12, 09), "2" };
            Assert.AreEqual(0, GetSqlServerADOInstance().GetDouble(sql, paramNames2, paramValues2));
        }
        /// <summary>
        /// 获取数据 -- 返回double类型 有参数 有default值,即查无值,则返回一个规定值
        /// 同时测试oracle、sqlserver数据库源
        /// </summary>
        [TestMethod]
        public void GetDoubleTest_hasParam_defaultValue()
        {
            //oracle:
            string sql = "select pay from EGCommonADOTest where birthday= :birthday and id=:id";
            string[] paramNames = { ":birthday", ":id" };
            object[] paramValues = { new DateTime(2013, 12, 09), "0" };
            Assert.AreEqual(5, GetOracleADOInstance().GetDouble(sql, paramNames, paramValues, 5));
            
            //sqlserver:
            sql = "select pay from EGCommonADOTest where birthday= @birthday and id=@id";
            string[] paramNames2 = { "@birthday", "@id" };
            object[] paramValues2 = { new DateTime(2013, 12, 09), "0" };
            Assert.AreEqual(5, GetSqlServerADOInstance().GetDouble(sql, paramNames2, paramValues2,5));
        }
        
        /// <summary>
        /// 获取数据 -- 返回object类型
        /// 同时测试oracle、sqlserver数据库源
        /// </summary>
        [TestMethod]
        public void GetObjectTest()
        {
            //oracle:
            string sql = "select age from EGCommonADOTest where birthday= :birthday and id=:id";
            string[] paramNames = { ":birthday", ":id" };
            object[] paramValues = { new DateTime(2013, 12, 14), "2" };
            object dr = GetOracleADOInstance().GetObject(sql, paramNames, paramValues) ;
            Assert.AreEqual(24, dr);

            //sqlserver:
            sql = "select age from EGCommonADOTest where birthday= @birthday and id=@id";
            string[] paramNames2 = { "@birthday", "@id" };
            object[] paramValues2 = { new DateTime(2013, 12, 09), "2" };
            dr = GetSqlServerADOInstance().GetObject(sql, paramNames2, paramValues2) ;
            Assert.AreEqual(21, dr);
        }


        /// <summary>
        /// 获取数据
        /// 同时测试oracle、sqlserver数据库源
        /// </summary>
        [TestMethod] 
        public void GetTest()
        {
            //oracle:
            string sql = "select * from EGCommonADOTest where birthday= :birthday and id=:id";
            string[] paramNames = { ":birthday", ":id" };
            object[] paramValues = { new DateTime(2013, 12, 14), "2" };
            DataRow dr = GetOracleADOInstance().Get(sql, paramNames, paramValues);
            Assert.AreEqual(24, dr["age"]);

            //sqlserver:
            sql = "select * from EGCommonADOTest where birthday= @birthday and id=@id";
            string[] paramNames2 = { "@birthday", "@id" };
            object[] paramValues2 = { new DateTime(2013, 12, 09), "2" };
            dr = GetSqlServerADOInstance().Get(sql, paramNames2, paramValues2);
            Assert.AreEqual(21, dr["age"]);
        }

        /// <summary>
        /// 获取数据 -- 从template模板中获取sql 对象中获取条件
        /// 同时测试oracle、sqlserver数据库源
        /// </summary>
        [TestMethod]
        public void GetNameTest()
        {
            UserInfo u = new UserInfo() { id = 1 };
            //oracle:
            DataRow dr = GetOracleADOInstance().GetName("test","select2", u);
            Assert.AreEqual(5, dr["age"]);

            //sqlserver:
            dr = GetSqlServerADOInstance().GetName("test", "select2", u);
            Assert.AreEqual(5, dr["age"]);

        }

        /// <summary>
        /// 获取数据 -- 从对象中获取条件
        /// 同时测试oracle、sqlserver数据库源
        /// </summary>
        [TestMethod]
        public void GetTest_module()
        {
            string sql = "select * from EGCommonADOTest where login_id=[loginid]";
            UserInfo u = new UserInfo() { id = 4, name = "bear", loginid = 1004 };
            //oracle:
            DataRow dr = GetOracleADOInstance().Get(sql, u);
            Assert.AreEqual(5, dr["age"]);

            //sqlserver:
            dr = GetSqlServerADOInstance().Get(sql, u);
            Assert.AreEqual(5, dr["age"]);
        }

        /// <summary>
        /// 获取数据 -- 从dictionary中获取查找条件
        /// 同时测试oracle、sqlserver数据库源
        /// </summary>
        [TestMethod]
        public void GetTest_module_dictionary()
        {
            string sql = "select * from EGCommonADOTest where login_id=[loginid]";
            Hashtable d = new Hashtable();
            d.Add("loginid", 1004);
            //oracle:
            DataRow dr = GetOracleADOInstance().Get(sql, d);
            Assert.AreEqual(5, dr["age"]);

            //sqlserver:
            dr = GetSqlServerADOInstance().Get(sql, d);
            Assert.AreEqual(5, dr["age"]);
        }

        /// <summary>
        /// 获取数据 -- 返回多条数据
        /// 同时测试oracle、sqlserver数据库源
        /// </summary>
        [TestMethod]
        public void QueryTest_select()
        {
            //oracle:
            string sql = "select age from EGCommonADOTest where birthday= :birthday and id=:id";
            string[] paramNames = { ":birthday", ":id" };
            object[] paramValues = { new DateTime(2013, 12, 14), "2" };
            DataTable dt = GetOracleADOInstance().Query(sql, paramNames, paramValues);
            Assert.AreEqual(1,dt.Rows.Count );

            //sqlserver:
            sql = "select age from EGCommonADOTest where birthday= @birthday and id=@id";
            string[] paramNames2 = { "@birthday", "@id" };
            object[] paramValues2 = { new DateTime(2013, 12, 09), "2" };
            dt = GetSqlServerADOInstance().Query(sql, paramNames2, paramValues2);
            Assert.AreEqual(1, dt.Rows.Count);
        }

        /// <summary>
        /// 获取数据 -- 返回多条数据 从对象中获取查找条件
        /// 同时测试oracle、sqlserver数据库源
        /// </summary>
        [TestMethod]
        public void QueryTest_module()
        {
            string sql = "select * from EGCommonADOTest where login_id=[loginid]";//建议使用这种,参数使用中括号的,屏蔽数据库类型
            UserInfo u = new UserInfo() { id = 4, name = "bear", loginid = 1004 };
            //oracle:
            DataTable dt = GetOracleADOInstance().Query(sql, u);
            Assert.AreEqual(9,dt.Rows.Count);

            //sqlserver:
            dt = GetSqlServerADOInstance().Query(sql, u);
            Assert.AreEqual(3,dt.Rows.Count);
        }

        /// <summary>
        /// 获取数据 -- 返回多条数据 从template中获取sql语句
        /// 同时测试oracle、sqlserver数据库源
        /// </summary>
        [TestMethod]
        public void QueryName_Test()
        {
            //oracle:
            DataTable dt = GetOracleADOInstance().QueryName("test", "select", null);
            Assert.IsTrue(dt.Rows.Count > 0);

            //sqlserver:
            dt = GetSqlServerADOInstance().QueryName("test", "select", null);
            Assert.IsTrue(dt.Rows.Count > 0);
        }

        /// <summary>
        /// 获取数据 -- 返回多条数据 从template中获取sql语句 template内容中{}的用法 -- 存在则用,null则不用
        /// 同时测试oracle、sqlserver数据库源
        /// </summary>
        [TestMethod]
        public void QueryName_select_brace()
        {
            //oracle:
            DataTable dt = GetOracleADOInstance().QueryName("test", "select_brace", new UserInfo() { name = "e", age = 21 });
            Assert.AreEqual(1,dt.Rows.Count);

            //sqlserver:
            dt = GetSqlServerADOInstance().QueryName("test", "select_brace", new UserInfo() { name = "e", age = 21 });
            Assert.AreEqual(2, dt.Rows.Count);
        }

        /// <summary>
        /// 获取数据 -- 返回多条数据 从template中获取sql语句 template内容中<<>>的用法-- 替代指定内容
        /// 同时测试oracle、sqlserver数据库源
        /// </summary>
        [TestMethod]
        public void QueryName_select_bracket()
        {
            //oracle:
            DataTable dt = GetOracleADOInstance().QueryName("test", "select_bracket", new UserInfo() { name = "age" });
            Assert.IsTrue(dt.Rows.Count > 0);

            //sqlserver:
            dt = GetSqlServerADOInstance().QueryName("test", "select_bracket", new UserInfo() { name = "age" });
            Assert.IsTrue(dt.Rows.Count > 0);
        }
     
        /// <summary>
        /// 插入数据 --从template中获取sql语句
        /// 同时测试oracle、sqlserver数据库源
        /// </summary>
        [TestMethod]
        public void ExecuteName_Test()
        {
            UserInfo u = new UserInfo() { id = 11, loginid = 1012, name = "yoga", birthday = DateTime.Now, age = 24, gender = "M", pay = 6500 };
            //oracle:
            Assert.AreEqual(1,GetOracleADOInstance().ExecuteName("test", "insert", u));

            ////sqlserver:
            u = new UserInfo() { loginid = 1012, name = "yoga", birthday = DateTime.Now, age = 24, gender = "M", pay = 6500 };
            Assert.AreEqual(1, GetSqlServerADOInstance().ExecuteName("test", "insert2", u));
        }
        /// <summary>
        /// 插入数据
        /// 同时测试oracle、sqlserver数据库源
        /// </summary>
        [TestMethod]
        public void Execute_insert()
        {
            //oracle:
            string sql = "insert into EGCommonADOTest " +
                            "(id,login_id,user_name,age,Birthday,Gender,Pay)" +
                              "values(:id,:login_id,:name,:age,:birthday,:gender,:pay) ";
            string[] paramNames = {"id", "login_id", "name", "age", "birthday", "gender", "pay" };
            object[] paramValues = { 13,1011, "jodie", 24, new DateTime (2013,12,14), 0, 6801 };
            Assert.AreEqual(1, GetOracleADOInstance().Execute(sql, paramNames, paramValues));
            
            //sqlserver:
            sql = "insert into EGCommonADOTest " +
                   "(login_id,user_name,age,Birthday,Gender,Pay)" +
                     "values(@login_id,@name,@age,@birthday,@gender,@pay) ";
            string[] paramNames2 = {  "login_id", "name", "age", "birthday", "gender", "pay" };
            object[] paramValues2 = { 1011, "jodie", 24, new DateTime(2013, 12, 14), 0, 6801 };
            Assert.AreEqual(1, GetSqlServerADOInstance().Execute(sql, paramNames2, paramValues2));
        }
        /// <summary>
        /// 删除数据
        /// 同时测试oracle、sqlserver数据库源
        /// </summary>
        [TestMethod]
        public void Execute_del()
        {
            //oracle:
            string sql = "delete from EGCommonADOTest where id>:id";
            string[] paramNames = { "id" };
            object[] paramValues = { 35 };
            Assert.AreEqual(0, GetOracleADOInstance().Execute(sql, paramNames, paramValues));

            //sqlserver:
            sql = "delete from EGCommonADOTest where id>@id";
            string[] paramNames2 = { "id" };
            object[] paramValues2 = { 35 };
            Assert.AreEqual(0, GetSqlServerADOInstance().Execute(sql, paramNames2, paramValues2));
        }

        /// <summary>
        /// 更新数据
        /// 同时测试oracle、sqlserver数据库源
        /// </summary>
        [TestMethod]
        public void Execute_update()
        {
            //oracle:
            string sql = "update EGCommonADOTest set age=:age where id=:id";
            string[] paramNames = { "age", "id" };
            object[] paramValues = { 9, 24 };
            Assert.IsTrue(GetOracleADOInstance().Execute(sql, paramNames, paramValues) == 1);

            //sqlserver:
            sql = "update EGCommonADOTest set age=@age where id=@id";
            string[] paramNames2 = { "age", "id" };
            object[] paramValues2 = { 9, 5 };
            Assert.IsTrue(GetSqlServerADOInstance().Execute(sql, paramNames2, paramValues2) == 1);

        }

        /// <summary>
        /// 插入数据
        /// 同时测试oracle、sqlserver数据库源
        /// </summary>
        [TestMethod]
        public void Execute_module_insert()//建议使用这种方法,带有[]的
        {
            //oracle:
            string sql = "insert into EGCommonADOTest " +
                           "(id,login_id,user_name,age,Birthday,Gender,Pay)" +
                             "values([id],[loginid],[name],[age],[birthday],[gender],[pay]) ";
            UserInfo u = new UserInfo() { id = 16, loginid = 1012, name = "yoga", birthday = DateTime.Now, age = 24, gender = "M", pay = 6500 };
            Assert.IsTrue(GetOracleADOInstance().Execute(sql, u) == 1);

            //sqlserver:  
            sql = "insert into EGCommonADOTest " +
                           "(login_id,user_name,age,Birthday,Gender,Pay)" +
                             "values([loginid],[name],[age],[birthday],[gender],[pay]) ";
            Assert.IsTrue(GetSqlServerADOInstance().Execute(sql, u) == 1);
        }


        [TestMethod]
        public void MysqlTest()
        {
            DataTable dt = this.GetMySqlADOInstance().ConnectTest();
            string title = dt.Rows[0]["title"] as string;
            title = Encoding.UTF8.GetString(Encoding.GetEncoding("latin1").GetBytes(title));
            Console.Out.WriteLine(title);
        }


        /// <summary>
        /// 获取数据 -- 分页测试
        /// 同时测试oracle、sqlserver数据库源
        /// </summary>
        [TestMethod]
        public void QueryTest_Pagination()
        {
            //oracle:
            string sql = "select id,age from EGCommonADOTest where id<>[id] order by id";
            UserSO so = new UserSO() { id=0, PageIndex=1, PageSize=5};
            DataTable dt = GetOracleADOInstance().Query(sql, so);
            Assert.AreEqual(20, so.Total);
            Assert.AreEqual(5, dt.Rows.Count);

            //sqlserver:
            so = new UserSO() { id = 0, PageIndex = 2, PageSize = 5 };
            dt = GetSqlServerADOInstance().Query(sql, so);
            Assert.AreEqual(20, so.Total);
            Assert.AreEqual(5, dt.Rows.Count);
        }
    }

    [DBSourceAttribute("ORACLE")]
    public class OracleADOTemplateTestClass :ADOTemplateTestClass
    {}
    
    [DBSourceAttribute("SQLSERVER")]
    public class SqlServerADOTemplateTestClass : ADOTemplateTestClass
    { }

    [DBSourceAttribute("MYSQL")]
    public class MysqlADOTemplateTestClass : ADOTemplateTestClass
    {

        public virtual DataTable ConnectTest()
        {
            DataTable dt = template.Query("select * from news_category", null, null);
            return dt;
        }
    }

    

    public class ADOTemplateTestClass
    {
        protected ADOTemplate template = null;

        public ADOTemplateTestClass()
        {
            template = new ADOTemplate();
        }

        public virtual DataTable ConnectTest()
        {
            DataTable dt = template.Query("select * from EGCommonADOTest", null, null);
            return dt;
        }

        public virtual int Insert(Object tableObject)
        {
            return template.Insert(tableObject);
        }

        public virtual int Update(Object tableObject)
        {
            return template.Update(tableObject);
        }

        public virtual int Delete(Object tableObject)
        {
            return template.Delete(tableObject);
        }

        public virtual T Get<T>(T tableObject) where T : new()
        {
            return template.Get<T>(tableObject);
        }

        public virtual T Find<T>(T tableObject) where T : new()
        {
            return template.Find(tableObject);
        }

        public virtual int GetInt(string sql)
        {
            return template.GetInt(sql);
        }

        public virtual int GetInt(String sql, String[] paramNames, Object[] paramValues)
        {
            return template.GetInt(sql, paramNames, paramValues);
        }

        public virtual int GetInt(String sql, String[] paramNames, Object[] paramValues, int defaultValue)
        {
            return template.GetInt(sql, paramNames, paramValues, defaultValue);
        }

        public virtual long GetLong(string sql)
        {
            return template.GetLong(sql);
        }

        public virtual long GetLong(String sql, String[] paramNames, Object[] paramValues)
        {
            return template.GetLong(sql, paramNames, paramValues);
        }

        public virtual long GetLong(String sql, String[] paramNames, Object[] paramValues, long defaultValue)
        {
            return template.GetLong(sql, paramNames, paramValues, defaultValue);
        }

        public virtual double GetDouble(string sql)
        {
            return template.GetDouble(sql);
        }

        public virtual double GetDouble(string sql, string[] paramNames, object[] paramValues)
        {
            return template.GetDouble(sql, paramNames, paramValues);
        }

        public virtual double GetDouble(string sql, string[] paramNames, object[] paramValues, int defaultValue)
        {
            return template.GetDouble(sql, paramNames, paramValues, defaultValue);
        }

        public virtual object GetObject(String sql, String[] paramNames, Object[] paramValues)
        {
            return template.GetObject(sql, paramNames, paramValues);
        }

        public virtual DataRow GetName(string TemplateModule, string TemplateName, object model)
        {
            return template.GetName(TemplateModule,TemplateName, model);
        }

        public virtual DataRow Get(string sql, String[] paramNames, Object[] paramValues)
        {
            return template.Get(sql, paramNames, paramValues);
        }

        public virtual DataRow Get(string sql, object module)
        {
            return template.Get(sql, module);
        }

        public virtual DataTable QueryName(string TemplateModule, string TemplateName, object module)
        {
            return template.QueryName(TemplateModule,TemplateName, module);
        }

        public virtual DataTable Query(string sql, String[] paramNames, Object[] paramValues)
        {
            return template.Query(sql, paramNames, paramValues);
        }

        public virtual DataTable Query(string sql, object module)
        {
            return template.Query(sql, module);
        }

        public virtual int ExecuteName(string TemplateModule, string TemplateName, object model)
        {
            return template.ExecuteName(TemplateModule, TemplateName, model);
        }

        public virtual int Execute(string sql, String[] paramNames, Object[] paramValues)
        {
            return template.Execute(sql, paramNames, paramValues);
        }

        public virtual int Execute(string sql, object module)
        {
            return template.Execute(sql, module);
        }

        public virtual int UpdateAfterGet<T>(T tableObject) where T : new()
        {
            UserInfo u2 = template.Get(tableObject) as UserInfo;
            u2.name = "bb";
            return template.Update(u2);
        }
    }


    [Table(Name = "EGCommonADOTest")]
    class UserInfo
    {
        [Column(IsPrimaryKey = true, Name = "id",Expression="seq")]
        public long? id { get; set; }
        [Column(Name = "login_id")]
        public long? loginid{ get; set; }
        [Column (Name ="user_name")]
        public string name{ get; set; }
        public int? age{ get; set; }
        public DateTime? birthday{ get; set; }
        public string gender { get; set; }
        public double pay { get; set; }
    }

    class UserSO : BaseSO 
    {
        public long id { get; set; }
    }
}
