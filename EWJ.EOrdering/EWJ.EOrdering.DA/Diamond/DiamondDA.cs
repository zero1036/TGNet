using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using EWJ.EOrdering.Common;
using EWJ.EOrdering.ViewModel.Diamond;
using EWJ.EOrdering.ViewModel;

namespace EWJ.EOrdering.DA.Diamond
{
    public class DiamondDA:BaseDA
    {
        private const string DIAMOND_SELECT = @"SELECT a.DiamondID,a.ArticleNo,a.DiamondNo,a.Carat,c.DicName ClarityName,d.DicName ShapeName,e.DicName ColorName,b.StoreMake StoreName,f.DicName StatusName,a.UploadDate FROM T_DIAMONDMASTER a LEFT JOIN T_STOREMASTER b ON a.StoreID=b.StoreID
left join (select diccode,dicname from view_DictionaryLocalization where diccode like '008%' and Localization=@LOCALIZATION) c on a.ClarityCode=c.DicCode 
left join (select diccode,dicname from view_DictionaryLocalization where diccode like '009%' and Localization=@LOCALIZATION) d on a.ShapeCode=d.DicCode 
left join (select diccode,dicname from view_DictionaryLocalization where diccode like '007%' and Localization=@LOCALIZATION) e on a.ColorCode=e.DicCode 
left join (select diccode,dicname from view_DictionaryLocalization where diccode like '002%' and Localization=@LOCALIZATION) f on a.StatusCode=f.DicCode 
WHERE 1=1 AND a.IsDelete=0 ";

        private const string PROC_DIAMOND_ASSIGN = "Proc_DiamondAssign";

        private string sql = string.Empty;

        /// <summary>
        /// 查询diamond
        /// </summary>
        /// <param name="diamondQueryItem"></param>
        /// <param name="localization">国际化标记</param>
        /// <returns></returns>
        public DataSet GetDiamondList(DiamondQueryItem queryItem,List<ScendingItem> scendingList,  string localization)
        {
            SqlParameter[] paramList = GetDiamondWhere(queryItem,scendingList, DIAMOND_SELECT, localization, out sql);

            using (DataSet ds = SQLHelper.ExecuteDataset(DBConnStr, CommandType.Text, sql, paramList))
            {
                try
                {
                    return ds;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }

        /// <summary>
        /// 钻石调配
        /// </summary>
        /// <param name="assignTb"></param>
        /// <returns></returns>
        public DataSet DiamondsAssign(DataTable assignTb)
        {
            SqlParameter[] sqlParamList=new SqlParameter[]{
              new SqlParameter("@tb",assignTb)
            };
            using (DataSet ds = SQLHelper.ExecuteDataset(DBConnStr, CommandType.StoredProcedure, PROC_DIAMOND_ASSIGN,sqlParamList))
            {
                try
                {
                    return ds;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 增加where参数字符串
        /// </summary>
        /// <param name="queryItem"></param>
        /// <param name="selectSql"></param>
        /// <param name="localization"></param>
        /// <param name="querySql"></param>
        /// <returns></returns>
        private SqlParameter[] GetDiamondWhere(DiamondQueryItem queryItem,List<ScendingItem> ScendingList,string selectSql, string localization, out string querySql)
        {
            StringBuilder sbWhere = new StringBuilder();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            //List<SqlParameter> sqlParamList2 = new List<SqlParameter>();

            //sqlParamList.Add(new SqlParameter("@TOTALROWS", SqlDbType.Int));
            //sqlParamList.Add(new SqlParameter("@TOTALPAGES", SqlDbType.Int));

            sqlParamList.Add(new SqlParameter("@LOCALIZATION", localization));

            if (!string.IsNullOrEmpty(queryItem.ArticleNo))
            {
                sbWhere.Append(" and DiamondNo like @DIAMONDNO");
                sqlParamList.Add(new SqlParameter("@DIAMONDNO", "%" + queryItem.DiamondNo + "%"));
            }
            if (queryItem.CaratRangeList != null)
            {
                if (queryItem.CaratRangeList.Count > 0)
                {
                    string caratWhere = string.Empty;
                    for (int i = 0; i < queryItem.CaratRangeList.Count; i++)
                    {
                        caratWhere += "(Carat>=@CARATMIN" + i.ToString() + " and Carat<=@CARATMAX" + i.ToString() + ") or ";
                        sqlParamList.Add(new SqlParameter("@CARATMIN" + i.ToString(), queryItem.CaratRangeList[i].CaratMin));
                        sqlParamList.Add(new SqlParameter("@CARATMAX" + i.ToString(), queryItem.CaratRangeList[i].CaratMax));
                    }
                    caratWhere = caratWhere.Substring(0, caratWhere.Length - 4);
                    sbWhere.Append( " and ("+caratWhere+")");
                }
            }
            if (queryItem.StoreIDList!=null)
            {              
                sbWhere.Append(" and a.storeid in (" + InsertParamList(queryItem.StoreIDList, "@STOREID", ref sqlParamList) + ")");
            }
            if (queryItem.ShapeCodeList!=null)
            {
                sbWhere.Append(" and ShapeCode in (" + InsertParamList(queryItem.ShapeCodeList, "@SHAPECODE", ref sqlParamList) + ")");

            }
            if (queryItem.ColorCodeList!=null)
            {
                sbWhere.Append(" and ColorCode in (" + InsertParamList(queryItem.ColorCodeList, "@COLORCODE", ref sqlParamList) + ")");
            }
            if (queryItem.ClarityCodeList!=null)
            {
                sbWhere.Append(" and ClarityCode in (" + InsertParamList(queryItem.ClarityCodeList, "@CLARITYCODE", ref sqlParamList) + ")");
            }
            if (!string.IsNullOrEmpty(queryItem.StatusCode))
            {
                sbWhere.Append(" and StatusCode=@STATUSCODE");
                sqlParamList.Add(new SqlParameter("@STATUSCODE",queryItem.StatusCode));
            }
            if (!string.IsNullOrEmpty(queryItem.PairID))
            {
                sbWhere.Append(" and PairID like @PAIRID");
                sqlParamList.Add(new SqlParameter("@PAIRID", "%" + queryItem.PairID + "%"));
            }
            if (!string.IsNullOrEmpty(queryItem.ArticleNo))
            {
                sbWhere.Append(" and ArticleNo like @ARTICLENO");
                sqlParamList.Add(new SqlParameter("@ARTICLENO", "%" + queryItem.ArticleNo + "%"));
            }
            if (queryItem.UploadDate != DateTime.MinValue)
            {
                sbWhere.Append(" and UploadDate>=@UploadDate");
                sqlParamList.Add(new SqlParameter("@UploadDate", queryItem.UploadDate));
            }
            sbWhere.Append(" order by");
            foreach (var scendingItem in ScendingList)
            {
                sbWhere.Append(" "+scendingItem.ColumnName+" "+scendingItem.ScendingType.ToString());
            }
            //sqlParamList.Add(new SqlParameter("@PAGESIZE", diamondSearch.PageSize));
            //sqlParamList.Add(new SqlParameter("@PAGEINDEX", diamondSearch.PageIndex));


            querySql = selectSql + sbWhere.ToString();
            SqlParameter[] paramList = sqlParamList.ToArray();
            return paramList;
        }

        /// <summary>
        /// 字符串数组参数化
        /// </summary>
        /// <param name="paramList"></param>
        /// <param name="paramName"></param>
        /// <param name="sqlParamList"></param>
        /// <returns></returns>
        private string InsertParamList(List<DiamondAttr> paramList, string paramName, ref List<SqlParameter> sqlParamList)
        {
            string inStr = string.Empty;
            for (int i = 0; i < paramList.Count; i++)
            {
                inStr += paramName + (i + 1).ToString() + ",";
                sqlParamList.Add(new SqlParameter(paramName + (i + 1).ToString(), paramList[i].attrVal));
            }
            inStr = inStr.Substring(0, inStr.Length - 1);
            return inStr;
        }
    }


}
