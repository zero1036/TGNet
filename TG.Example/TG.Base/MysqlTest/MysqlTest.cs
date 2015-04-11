using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using EG.Utility.DBCommon.dao;
/*****************************************************
* 目的：mysql连接测试
* 创建人：林子聪
* 创建时间：20150406
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace TG.Example
{
    public class MysqlTest
    {
        private ADOTemplate _ado;
        public MysqlTest()
        {
            _ado = new ADOTemplate();
            EG.Business.Common.ConfigCache.LoadAppConfig(new string[] 
            {//##需要解密的字段 
                "WX_appID"                                         //数据库
            });
        }
        /// <summary>
        /// 查询测试
        /// </summary>
        /// <returns></returns>
        public int QueryFunc()
        {
            try
            {
                var sqlx = "select * from tb1";
                DataTable dt = _ado.Query(sqlx, null, null, string.Empty);
                return dt.Rows.Count;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        /// <summary>
        /// 查询测试
        /// </summary>
        /// <returns></returns>
        public int QueryFunc2()
        {
            try
            {
                var sqlx = "select * from tb1 t1 left outer join tb2 t2 on t1.id=t2.id where  t1.id>1";
                DataTable dt = _ado.Query(sqlx, null, null, string.Empty);
                if (dt != null) Console.WriteLine(dt);
                return dt.Rows.Count;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        /// <summary>
        /// 插入测试
        /// </summary>
        /// <returns></returns>
        public string InsertFunc()
        {
            try
            {
                var sqlx = "insert into tb2(id,name) values(3,'apple')";
                var ires = _ado.Execute(sqlx, null, null, string.Empty);
                return "insert结果" + ires;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
