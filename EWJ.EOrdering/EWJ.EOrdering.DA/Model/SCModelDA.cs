using EWJ.EOrdering.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace EWJ.EOrdering.DA.Model
{
    public class SCModelDA:BaseDA
    {

        private string GetModelsStr = @"PRO_SearchSCModels";

        private string DeleteModelStr = @" update t_SCModelHeader set deleteUser=@deleteUser,DeleteDate=GETDATE() where  ModelID=@ModelID ";

        public DataSet GetModels(string Localization, string OrderType = "1", int BeginNo = 1, int PageNo=int.MaxValue, string EWJNo = null, string ModelTypeCode = null, string CaizhiCode = null,
                string ColorCode=null,string KelaWeight=null)
        {
             
          
            IList<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@Localization", Localization));
            paramList.Add(new SqlParameter("@OrderType", OrderType));
            paramList.Add(new SqlParameter("@BeginNo", BeginNo));
            paramList.Add(new SqlParameter("@PageNo", PageNo));
          
               
                paramList.Add(new SqlParameter("@EWJNo", EWJNo));
           

        
                paramList.Add(new SqlParameter("@ModelTypeID", ModelTypeCode));   
               
                paramList.Add(new SqlParameter("@CaizhiCode", CaizhiCode));
          
                paramList.Add(new SqlParameter("@ColorCode", ColorCode));
                  
                paramList.Add(new SqlParameter("@KelaWeight", KelaWeight));

           
            DataSet ds = SQLHelper.ExecuteDataset(DBConnStr, CommandType.StoredProcedure, GetModelsStr,paramList.ToArray());
            
            return ds;
        }

        public bool GetUpdateModel(string id, string userid, string EWJNo, string ModelTypeID, string ImagePath, string FromDate,string ToDate, DataTable dt)
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
                })>=0;

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
