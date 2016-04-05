//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Data;
//using EG.Utility.DBCommon.dao;
//using System.Diagnostics;
///*****************************************************
//* 目的：mysql连接测试
//* 创建人：林子聪
//* 创建时间：20150406
//* 备注：
//* 依赖性：
//* 版权：
//* 使用本文件时，必须保留本内容的完整性！
//*****************************************************/
//namespace TG.Example
//{
//    public class MysqlTest
//    {
//        private ADOTemplate _ado;
//        public MysqlTest()
//        {
//            _ado = new ADOTemplate();
//            EG.Business.Common.ConfigCache.LoadAppConfig(new string[] 
//            {//##需要解密的字段 
//                "WX_appID"                                         //数据库
//            });
//        }
//        /// <summary>
//        /// 查询测试
//        /// </summary>
//        /// <returns></returns>
//        public int QueryFunc()
//        {
//            try
//            {
//                var sqlx = "select * from tb1";
//                DataTable dt = _ado.Query(sqlx, null, null, string.Empty);
//                return dt.Rows.Count;
//            }
//            catch (Exception ex)
//            {
//                return -1;
//            }
//        }
//        /// <summary>
//        /// 查询测试
//        /// </summary>
//        /// <returns></returns>
//        public int QueryFunc2()
//        {
//            try
//            {
//                var sqlx = "select * from tb1 t1 left outer join tb2 t2 on t1.id=t2.id where  t1.id>1";
//                DataTable dt = _ado.Query(sqlx, null, null, string.Empty);
//                if (dt != null) Console.WriteLine(dt);
//                return dt.Rows.Count;
//            }
//            catch (Exception ex)
//            {
//                return -1;
//            }
//        }
//        /// <summary>
//        /// 插入测试
//        /// </summary>
//        /// <returns></returns>
//        public string InsertFunc()
//        {
//            try
//            {
//                int ires = 0;
//                Random pRam;
//                //string psql = string.Empty;
//                StringBuilder psql = new StringBuilder();
//                for (int i = 0; i <= 600000; i++)
//                {
//                    pRam = new Random();
//                    int iv = pRam.Next(1, 100);
//                    //psql = string.Format("('1','',{0},'{1}'),{2}", iv, iv, psql);
//                    psql = psql.AppendFormat(",('1','',{0},'{1}')", iv, iv);
//                    //var sqlx = string.Format("insert into tb3(name,nickname,age,remark) values('1','',{0},'{1}')", iv, iv);

//                }
//                string sqlbbb = psql.ToString().Substring(1, psql.Length - 1);
//                var sqlx = string.Format("insert into tb3(name,nickname,age,remark) values{0}", sqlbbb);
//                ires = _ado.Execute(sqlx, null, null, string.Empty);
//                return "insert结果" + ires;
//            }
//            catch (Exception ex)
//            {
//                return ex.Message;
//            }
//        }
//        /// <summary>
//        /// 插入测试
//        /// </summary>
//        /// <returns></returns>
//        public string InsertFunc2()
//        {
//            try
//            {
//                int ires = 0;
//                Random pRam = new Random(); ;
//                StringBuilder psql = new StringBuilder();
//                int itagid = 0;
//                string tagname = string.Empty;
//                int itid = 0;
//                int isysdepartmentid = 0;
//                int isysuserid = 0;
//                for (int i = 0; i <= 200000; i++)
//                {
//                    //pRam. 
//                    itagid = pRam.Next(1, 100);
//                    tagname = itagid.ToString();
//                    itid = pRam.Next(0, 10);
//                    isysdepartmentid = pRam.Next(0, 100);
//                    if (isysdepartmentid > 50)
//                    {
//                        isysdepartmentid = 0;
//                        isysuserid = pRam.Next(0, 1000);
//                    }
//                    else
//                    {
//                        isysuserid = 0;
//                    }

//                    psql = psql.AppendFormat(",({0},'{1}',{2},{3},{4})", itagid, tagname, itid, isysdepartmentid, isysuserid);
//                }
//                string sqlbbb = psql.ToString().Substring(1, psql.Length - 1);
//                var sqlx = string.Format("insert into sys_tag(tagid,tagname,tid,sysdepartmentid,sysuserid) values{0}", sqlbbb);
//                ires = _ado.Execute(sqlx, null, null, string.Empty);
//                return "insert结果" + ires;
//            }
//            catch (Exception ex)
//            {
//                return ex.Message;
//            }
//        }
//        /// <summary>
//        /// 插入测试
//        /// </summary>
//        /// <returns></returns>
//        public string InsertFunc3()
//        {
//            try
//            {
//                int ires = 0;
//                Random pRam = new Random(); ;
//                StringBuilder psql = new StringBuilder();

//                int itid = 0;
//                int roleid = 0;
//                for (int i = 1; i <= 200000; i++)
//                {
//                    itid = pRam.Next(0, 10);
//                    roleid = pRam.Next(1, 4);


//                    psql = psql.AppendFormat(",('{0}',{1},{2})", i, itid, roleid);
//                }
//                string sqlbbb = psql.ToString().Substring(1, psql.Length - 1);
//                var sqlx = string.Format("insert into sys_User(userid,tid,roleid) values{0}", sqlbbb);
//                ires = _ado.Execute(sqlx, null, null, string.Empty);
//                return "insert结果" + ires;
//            }
//            catch (Exception ex)
//            {
//                return ex.Message;
//            }
//        }
//        /// <summary>
//        /// 模拟测试
//        /// </summary>
//        /// <returns></returns>
//        public string ProcedureFunc()
//        {
//            try
//            {
//                int ires = 0;
//                long lonetime = 0;
//                long ltotaltime = 0;
//                Stopwatch watch = new Stopwatch();
//                watch.Start();
//                for (int i = 1; i <= 1000; i++)
//                {
//                    //ires = _ado.Execute("pr_User_SingleAdd2", new string[] { "ptid", "puserid", "pname", "pposition", "pmobile", "pemail", "pweixinid", "pavatar", "pstatus", "ppassword", "plitmitcount" }, new object[] { 1, "Mark2", "M2", "PG", "602424", "799", "zero1036", "touxiang", 1, "504", 1000000 }, string.Empty, CommandType.StoredProcedure);

//                    //ires = _ado.Execute("pr_User_SingleAdd3", new string[] { "ptid", "puserid", "pname", "pposition", "pmobile", "pemail", "pweixinid", "pavatar", "pstatus", "ppassword", "plitmitcount", "pcurtrid", "ptbname" }, new object[] { 1, "Mark2", "M2", "PG", "602424", "799", "zero1036", "touxiang", 1, "504", 1000000, 1, "t_User_1" }, string.Empty, CommandType.StoredProcedure);

//                    //ires = _ado.Execute("pr_User_SingleAdd4", new string[] { "ptid", "pcurtrid", "ptbname" }, new object[] { 1, 1, "t_User_1" }, string.Empty, CommandType.StoredProcedure);

//                    //ires = _ado.Execute("p16", null, null, string.Empty, CommandType.StoredProcedure);
//                    //ires = _ado.Execute("p17", null, null, string.Empty, CommandType.StoredProcedure);
//                    //ires = _ado.Execute("p18", null, null, string.Empty, CommandType.StoredProcedure);
//                    //ires = _ado.Execute("p19", null, null, string.Empty, CommandType.StoredProcedure);
//                }
//                watch.Stop();
//                ltotaltime = watch.ElapsedMilliseconds;
//                watch.Reset();
//                //ltotaltime += lonetime;
//                //double pv=ltotaltime / 100;
//                Debug.WriteLine(ltotaltime);

//                return "insert结果" + ires;
//            }
//            catch (Exception ex)
//            {
//                return ex.Message;
//            }
//        }

//        //public string ProcedureFunc2()
//        //{
//        //    try
//        //    {
//        //        int ires = 0;
//        //        long lonetime = 0;
//        //        long ltotaltime = 0;
//        //        Stopwatch watch = new Stopwatch();
//        //        watch.Start();

//        //         var sqlx = "insert into sys_User(trid,tid) values(1,1)";
//        //         ires = _ado.Execute(sqlx, null, null, string.Empty);

//        //         _ado.Query("select max(sysuserid) from t_User_1 where ");

//        //        watch.Stop();
//        //        ltotaltime = watch.ElapsedMilliseconds;
//        //        watch.Reset();
//        //        //ltotaltime += lonetime;
//        //        //double pv=ltotaltime / 100;
//        //        Debug.WriteLine(ltotaltime);

//        //        return "insert结果" + ires;
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        return ex.Message;
//        //    }
//        //}
//    }
//}
