using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
//using System.Data.OracleClient;

namespace EG.Utility.DBCommon.dao
{
    public class Dialect
    {

        public static Dialect GetDialect(short dbType)
        {
            if (ADOTemplate.DB_TYPE_MYSQL == dbType) 
            {
                return new MySQLDialect();
            }
            else if (ADOTemplate.DB_TYPE_ORACLE == dbType)
            {
                return new Oracle9iDialect();
            }
            else if (ADOTemplate.DB_TYPE_SQLSERVER == dbType)
            {
                return new SQLServerDialect();
            }
            else
            {
                return null;
            }
        }

        public virtual String GetCountSql(String sql)
        {
            String tempSql = sql.ToLower();

            int beginPos = 7; // start after select
            int nextFormPos = tempSql.IndexOf("from ", beginPos);
            int nextSelectPos = tempSql.IndexOf("select ", beginPos);

            while (nextSelectPos > 0 && nextFormPos > nextSelectPos)
            {
                beginPos = nextFormPos + 4;

                nextFormPos = tempSql.IndexOf("from ", beginPos);
                nextSelectPos = tempSql.IndexOf("select ", beginPos);
            }


            int endPos = tempSql.IndexOf("order by");

            if (endPos > 0)
            {
                return "SELECT COUNT(*) " + sql.Substring(nextFormPos, endPos - nextFormPos);
            }
            else
            {
                return "SELECT COUNT(*) " + sql.Substring(nextFormPos);
            }
        }

        public virtual String GetLimitSql(String sql, int pageIndex, int pageSize)
        {
            return null;
        }

        public virtual IDbDataParameter[] GetLimitParams(int pageIndex, int pageSize)
        {
            return null;
        }

        public virtual string[] GetLimitParamNames(int pageIndex, int pageSize)
        {
            return null;
        }

        public virtual object[] GetLimitParamValues(int pageIndex, int pageSize)
        {
            return null;
        }
    }

    public class Oracle9iDialect : Dialect {

        public override String GetLimitSql(String sql, int pageIndex, int pageSize)
        {
            bool hasOffset = (pageIndex != 1);
            sql = sql.Trim();
            //bool isForUpdate = false;
            /* -- 一般for update，是小数据量
            if (sql.ToLower().endsWith(" for update"))
            {
                sql = sql.Substring(0, sql.Length - 11);
                isForUpdate = true;
            }*/

            StringBuilder pagingSelect = new StringBuilder(sql.Length + 100);
            if (hasOffset)
            {
                pagingSelect.Append("select * from ( select row_.*, rownum rownum_ from ( ");
            }
            else
            {
                pagingSelect.Append("select * from ( ");
            }
            pagingSelect.Append(sql);
            if (hasOffset)
            {
                pagingSelect.Append(" ) row_ ) where rownum_ <= :x_rownum_to and rownum_ > :x_rownum_from");
            }
            else
            {
                pagingSelect.Append(" ) where rownum <= :x_rownum_to");
            }
            /*
            if (isForUpdate)
            {
                pagingSelect.Append(" for update");
            }
            */
            return pagingSelect.ToString();
        }

        public override IDbDataParameter[] GetLimitParams(int pageIndex, int pageSize)
        {

            bool hasOffset = (pageIndex != 1);

            if (hasOffset)
            {
                return new IDbDataParameter[] { };
            }
            else
            {
                return new IDbDataParameter[] {};
            }
        }

        public override string[] GetLimitParamNames(int pageIndex, int pageSize)
        {

            bool hasOffset = (pageIndex != 1);

            if (hasOffset)
            {
                return new string[] {":x_rownum_to", ":x_rownum_from"};
            }
            else
            {
                return new string[] {":x_rownum_to"};
            }
        }

        public override object[] GetLimitParamValues(int pageIndex, int pageSize)
        {

            bool hasOffset = (pageIndex != 1);

            if (hasOffset)
            {
                return new object[] {(pageIndex * pageSize), (pageIndex - 1) * pageSize};
            }
            else
            {
                return new object[] {(pageIndex * pageSize)};
            }
        }
    }

    public class MySQLDialect : Dialect
    {
        public override String GetLimitSql(String sql, int pageIndex, int pageSize)
        {
            bool hasOffset = (pageIndex != 1);

            return new StringBuilder(sql.Length + 20)
                .Append(sql)
                .Append(hasOffset ? " limit @x_rownum_from, @x_row_size" : " limit @x_row_size")
                .ToString();
        }

        public override IDbDataParameter[] GetLimitParams(int pageIndex, int pageSize)
        {
            
            bool hasOffset = (pageIndex != 1);

            if (hasOffset)
            {
                return new IDbDataParameter[] {
                new MySqlParameter("@x_rownum_from", ((pageIndex - 1) * pageSize)) , 
                new MySqlParameter("@x_row_size", pageSize )};
            }
            else
            {
                return new IDbDataParameter[] {new MySqlParameter("@x_row_size", pageSize )};
            }
        }

        public override string[] GetLimitParamNames(int pageIndex, int pageSize)
        {
            bool hasOffset = (pageIndex != 1);

            if (hasOffset)
            {
                return new string[] {"@x_rownum_from", "@x_row_size"};
            }
            else
            {
                return new string[] {"@x_row_size"};
            }
        }

        public override object[] GetLimitParamValues(int pageIndex, int pageSize)
        {
            bool hasOffset = (pageIndex != 1);

            if (hasOffset)
            {
                return new object[] {(pageIndex - 1) * pageSize, pageSize};
            }
            else
            {
                return new object[] {pageSize};
            }
        }
    }

    public class SQLServerDialect : Dialect
    {
        /*
        public override String GetLimitSql(String querySelect, int pageIndex, int pageSize)
        {
            bool hasOffset = (pageIndex != 1);

            //以下这句就是第2段测试代码出异常的原因了，呵呵。由此可知，对于SQLServer的分页，Hibernate中对于SQLServer的分页，只会使用一次top子句取出前面的部分记录，来通过操作游标来获取指定页的数据，是假分页。
            //if (offset > 0)
            if (hasOffset)
            {
                throw new Exception("sql server has no offset");
            }
            return new StringBuilder(querySelect.Length + 8)
                .Append(querySelect)
                .Insert(getAfterSelectInsertPoint(querySelect), " top " + limit)
                .ToString();
        }

        static int getAfterSelectInsertPoint(String sql) {
            String sqlLower = sql.ToLower() ;
            int selectIndex = sqlLower.IndexOf("select");
            int selectDistinctIndex = sqlLower.IndexOf("select distinct");
 		    return selectIndex + ( selectDistinctIndex == selectIndex ? 15 : 6 );
        }
        */
        /***
         * Render the <tt>rownumber() over ( .... ) as rownumber_,</tt>
         * bit, that goes in the select list
         */
        private String GetRowNumber(String sql) {

            StringBuilder rownumber = new StringBuilder(50).Append("ROW_NUMBER() OVER(");

            int orderByIndex = sql.ToLower().LastIndexOf("order by");

            if (orderByIndex > 0 && !hasDistinct(sql))
            {
                rownumber.Append(sql.Substring(orderByIndex));
            }
            else 
            {
                rownumber.Append(" order by %%physloc%% ");
            }

            rownumber.Append(") as rownumber_,");

            return rownumber.ToString();

        }

        /*
         SELECT * FROM (
		SELECT *,
		ROW_NUMBER() OVER(ORDER BY PublishToDate DESC, CategorySort ASC, Sort ASC, LastModifiedTime DESC) AS RN
		FROM vw_ContentList) a
WHERE RN BETWEEN @PageSize*@PageIndex+1 AND @PageSize*(@PageIndex+1)

         */
        public override String GetLimitSql(String sql, int pageIndex, int pageSize)
        {

            bool hasOffset = (pageIndex != 1);

            int startOfSelect = sql.ToLower().IndexOf("select");

            int orderByIndex = sql.ToLower().LastIndexOf("order by");

            StringBuilder pagingSelect = new StringBuilder(sql.Length + 100)
                    //.Append( sql.Substring(0, startOfSelect) )  // add the comment
                    .Append("select * from ( select ")          // nest the main query in an outer select
                    .Append( GetRowNumber(sql) );               // add the rownnumber bit into the outer query select list

            if (orderByIndex > 0)
            {
                sql = sql.Substring(0, orderByIndex);
            }

            if ( hasDistinct(sql) ) 
            {
                pagingSelect.Append(" row_.* from ( ")          // add another (inner) nested select
                        .Append( sql.Substring(startOfSelect) ) // add the main query
                        .Append(" ) as row_");                  // close off the inner nested select
            }
            else
            {
                pagingSelect.Append( sql.Substring( startOfSelect + 6 ) ); // add the main query
            }

            pagingSelect.Append(" ) as temp_ where rownumber_ ");

            //add the restriction to the outer select
            if (hasOffset)
            {
                pagingSelect.Append("between @x_rownum_from and @x_rownum_to");
            }
            else 
            {
                pagingSelect.Append("<= @x_rownum_to");
            }

            return pagingSelect.ToString();

        }

        private static bool hasDistinct(String sql) {
            return sql.ToLower().IndexOf("select distinct")>=0;

        }


        public override string[] GetLimitParamNames(int pageIndex, int pageSize)
        {
            bool hasOffset = (pageIndex != 1);

            if (hasOffset)
            {
                return new string[] { "@x_rownum_from", "@x_rownum_to" };
            }
            else
            {
                return new string[] { "@x_rownum_to" };
            }
        }

        public override object[] GetLimitParamValues(int pageIndex, int pageSize)
        {
            bool hasOffset = (pageIndex != 1);

            if (hasOffset)
            {
                return new object[] { (pageIndex - 1) * pageSize + 1, pageIndex * pageSize };
            }
            else
            {
                return new object[] { pageSize };
            }
        }
    }
}
