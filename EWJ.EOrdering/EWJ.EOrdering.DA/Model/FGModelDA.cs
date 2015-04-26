using EWJ.EOrdering.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace EWJ.EOrdering.DA.Model
{
    public class FGModelDA:BaseDA
    {
        private string GetModelsStr = @"  select * from t_FGModelHeader h 
		inner join t_FGModelDetail d on h.ModelID=d.ModelID
		inner join view_DictionaryLocalization v on v.DicCode=d.DisCode and v.Localization=@Localization
		inner join view_DictionaryLocalization vh on vh.DicCode=h.ModelTypeID and vh.Localization=@Localization
		 where 1=1 and  h.ModelID in( select distinct ModelID from t_FGModelDetail 
			group by ModelID
		    having 1=1 {cwhere}) {where} ";

        private string DeleteModelStr = @" update t_FGModelHeader set deleteUser=@deleteUser,DeleteDate=GETDATE() where  ModelID=@ModelID ";

        public DataSet GetModels(string Localization, string EWJNo = null, string ModelTypeCode = null, string CaizhiCode = null,
                string ColorCode = null, string KelaWeight = null, string FromDate = null)
        {

            string strWhere = "";
            string strCWhere = "";
            IList<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@Localization", Localization));
            if (EWJNo != null)
            {
                strWhere += @" and (h.EWJNo=@EWJNo)  ";
                paramList.Add(new SqlParameter("@EWJNo", EWJNo));
            }

            if (ModelTypeCode != null)
            {
                strWhere += @" and h.ModelTypeID=@ModelTypeID ";
                paramList.Add(new SqlParameter("@ModelTypeID", ModelTypeCode));
            }

            if (CaizhiCode != null)
            {
                strCWhere += " and sum(IIF(DisCode=@CaizhiCode,1,0))>0 ";
                paramList.Add(new SqlParameter("@CaizhiCode", CaizhiCode));
            }

            if (ColorCode != null)
            {
                strCWhere += " and sum(IIF(DisCode=@ColorCode,1,0))>0 ";
                paramList.Add(new SqlParameter("@ColorCode", ColorCode));
            }

            if (KelaWeight != null)
            {
                strCWhere += " and sum(IIF(DisCode=@KelaWeight,1,0))>0 ";
                paramList.Add(new SqlParameter("@KelaWeight", KelaWeight));
            }

            if (FromDate != null)
            {
                strWhere += " and h.FromDate=@FromDate ";
                paramList.Add(new SqlParameter("@FromDate", FromDate));
            }

            GetModelsStr = GetModelsStr.Replace("{where}", strWhere).Replace("{cwhere}", strCWhere);
            DataSet ds = SQLHelper.ExecuteDataset(DBConnStr, CommandType.Text, GetModelsStr);

            return ds;
        }

        public bool GetUpdateModel(string id, string userid, string EWJNo, string ModelTypeID, string ImagePath, string FromDate, string ToDate, DataTable dt)
        {

            return
             SQLHelper.ExecuteNonQuery(DBConnStr, CommandType.StoredProcedure, "PRO_UpdateSCModel",
                 new SqlParameter[] {
                    new SqlParameter("@hid",id),
                    new SqlParameter("@EWJNo",EWJNo),
                    new SqlParameter("@ModelTypeID",ModelTypeID),
                    new SqlParameter("@ImagePath",ImagePath),
                    new SqlParameter("@FromDate",FromDate),
                    new SqlParameter("@ToDate",ToDate),
                    new SqlParameter("@ModifyUser",userid),
                    new SqlParameter("@t",dt)
                }) >= 0;

        }

        public bool DeleteModel(string id, string userid)
        {

            return
            SQLHelper.ExecuteNonQuery(DBConnStr, CommandType.Text, DeleteModelStr,
                new SqlParameter[] {
                    new SqlParameter("@ModelID",id),
                new SqlParameter("@deleteUser",userid)
                
                }) >= 0;
        }

        public bool InsertModel(string id, string userid, string EWJNo, string ModelTypeID, string ImagePath, string FromDate, string ToDate, DataTable dt)
        {
            return
            SQLHelper.ExecuteNonQuery(DBConnStr, CommandType.StoredProcedure, "PRO_InsertSCModel",
               new SqlParameter[] {
                    new SqlParameter("@hid",id),
                    new SqlParameter("@EWJNo",EWJNo),
                    new SqlParameter("@ModelTypeID",ModelTypeID),
                    new SqlParameter("@ImagePath",ImagePath),
                    new SqlParameter("@FromDate",FromDate),
                    new SqlParameter("@ToDate",ToDate),
                    new SqlParameter("@ModifyUser",userid),
                    new SqlParameter("@t",dt)
                }) >= 0;
        }



    }
}
