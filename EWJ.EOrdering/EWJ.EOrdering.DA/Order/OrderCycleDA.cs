using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace EWJ.EOrdering.DA.Order
{
    public class OrderCycleAOP : BaseDA
    {
        /// <summary>
        /// Update data
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public virtual bool Update(DataTable dt) { return false; }
    }

    /// <summary>
    /// Table DA
    /// </summary>
    public class OrderCycleDA : OrderCycleAOP
    {
        #region Struct
        public static readonly string FIELD_NAME_CycleID = "CycleID";
        public static readonly string FIELD_NAME_CycleNo = "CycleNo";
        public static readonly string FIELD_NAME_StartDate = "StartDate";
        public static readonly string FIELD_NAME_EndDate = "EndDate";
        public static readonly string FIELD_NAME_Remark = "Remark";
        public static readonly string FIELD_NAME_Status = "Status";
        public static readonly string FIELD_NAME_ModelType = "ModelType";
        public static readonly string TABLE_NAME = "t_OrderCycle";
        //新增and更新存储过程
        private static readonly string PROCE_NAME_UPDATE = "PRO_t_OrderCycle_UPDATE";
        //获取最大周期编号——当前仅支持sql server
        private static readonly string PROCE_NAME_GETMAXCYLENO = "select top 1 CycleNo from t_OrderCycle order by CycleNo desc";
        //原生
        private static readonly string PROCE_NAME_GET = "select * from t_OrderCycle oc order by oc.CycleNo desc";
        //带国际化转换status
        private static readonly string PROCE_NAME_GET_DL = "select oc.CycleID,oc.CycleNo,oc.StartDate,oc.EndDate,dl.DicName as Status,oc.ModelType from t_OrderCycle oc left outer join view_DictionaryLocalization dl on oc.Status=dl.DicCode Where dl.Localization=@Localization order by oc.CycleNo desc";
        #endregion

        #region Action
        /// <summary>
        /// Get Data
        /// </summary>
        /// <returns></returns>
        public DataTable Get(string pLocalization)
        {
            IList<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@Localization", pLocalization));
            var ds = SQLHelper.ExecuteDataset(DBConnStr, CommandType.Text, PROCE_NAME_GET, paramList.ToArray());
            if (ds != null && ds.Tables.Count != 0)
            {
                return ds.Tables[0];
            }
            return null;
        }
        /// <summary>
        /// Get Data
        /// </summary>
        /// <returns></returns>
        public string GetMaxNo()
        {
            var ds = SQLHelper.ExecuteDataset(DBConnStr, CommandType.Text, PROCE_NAME_GETMAXCYLENO);
            if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0 && ds.Tables[0].Rows[0][0] != DBNull.Value)
            {
                return ds.Tables[0].Rows[0][0].ToString();
            }
            return string.Empty;
        }
        /// <summary>
        /// Update data
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public override bool Update(DataTable dt)
        {
            IList<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@tb", dt));
            var ds = SQLHelper.ExecuteDataset(DBConnStr, CommandType.StoredProcedure, PROCE_NAME_UPDATE, paramList.ToArray());
            if (ds != null && ds.Tables.Count != 0)
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}
