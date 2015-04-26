using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWJ.EOrdering.DA.Sys
{
    public class DictionaryDA : BaseDA
    {
        public DataSet Get(string code)
        {
            string query = string.Format(@"SELECT * FROM sys_Dictionary
                                            WHERE [DicCode] = @DicCode");
            SqlParameter param = new SqlParameter("@DicCode", code);
            DataSet ds = SQLHelper.ExecuteDataset(DBConnStr, CommandType.Text, query, param);

            return ds;
        }

        public DataSet GetByParent(string pCode)
        {
            string query = string.Format(@"SELECT * FROM sys_Dictionary WHERE ISNULL(ParentCode, '') = @ParentCode ORDER BY [Index]");
            SqlParameter param = new SqlParameter("@ParentCode", pCode);
            DataSet ds = SQLHelper.ExecuteDataset(DBConnStr, CommandType.Text, query, param);

            return ds;
        }

        public DataSet GetByParent(string pCode, bool? isDisable, string localization)
        {
            string query = @"SELECT * 
                                FROM view_DictionaryLocalization
                                WHERE {0}
                                ORDER BY [Index]";

            string where = @"ISNULL(ParentCode, '') = @ParentCode
	                            AND Localization = @Localization";

            IList<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@ParentCode", pCode));
            paramList.Add(new SqlParameter("@Localization", localization));
            //, new SqlParameter("@Localization", localization)

            if (isDisable.HasValue)
            {
                where += " AND ISNULL(IsDisable, 0) = @IsDisable";
                paramList.Add(new SqlParameter("@IsDisable", isDisable));
            }
            query = string.Format(query, where);
            DataSet ds = SQLHelper.ExecuteDataset(DBConnStr, CommandType.Text, query, paramList.ToArray());

            return ds;
        }

        public int Add(string code, string en_name, string chs_name, string cht_name
                       , string pCode, string remark, int? index, bool? isDisable
                       , Guid? createdUser, DateTime dateTime, string ext1, string ext2, string ext3, string ext4)
        {
            string query = @"INSERT INTO [dbo].[sys_Dictionary]
                               ([DicCode]
                               ,[EN_Name]
                               ,[CHS_Name]
                               ,[CHT_Name]
                               ,[ParentCode]
                               ,[Remark]
                               ,[Index]
                               ,[IsDisable]
                               ,[CreatedUser]
                               ,[CreatedDate]
                               ,[Extend1]
                               ,[Extend2]
                               ,[Extend3]
                               ,[Extend4])
                         VALUES
                               (@DicCode
                               ,@EN_Name
                               ,@CHS_Name
                               ,@CHT_Name
                               ,@ParentCode
                               ,@Remark
                               ,@Index
                               ,@IsDisable
                               ,@CreatedUser
                               ,@CreatedDate
                               ,@Extend1
                               ,@Extend2
                               ,@Extend3
                               ,@Extend4)";

            SqlParameter[] paramList = new SqlParameter[] 
            {
                new SqlParameter("@DicCode", code)
                , new SqlParameter("@EN_Name", en_name)
                , new SqlParameter("@CHS_Name", chs_name)
                , new SqlParameter("@CHT_Name", cht_name)
                , new SqlParameter("@ParentCode", pCode)
                , new SqlParameter("@Remark", remark)
                , new SqlParameter("@Index", index)
                , new SqlParameter("@IsDisable", isDisable)
                , new SqlParameter("@CreatedUser", createdUser)
                , new SqlParameter("@CreatedDate", dateTime)
                , new SqlParameter("@Extend1", ext1)
                , new SqlParameter("@Extend2", ext2)
                , new SqlParameter("@Extend3", ext3)
                , new SqlParameter("@Extend4", ext4)
            };
            int iCount = SQLHelper.ExecuteNonQuery(DBConnStr, CommandType.Text, query, paramList);

            return iCount;
        }

        public object GetMaxCode(string pCode)
        {
            string query = string.Format(@"SELECT MAX(DicCode) FROM sys_Dictionary WHERE ISNULL(ParentCode, '') = @ParentCode");
            SqlParameter param = new SqlParameter("@ParentCode", pCode);
            object obj = SQLHelper.ExecuteScalar(DBConnStr, CommandType.Text, query, param);
            return obj;
        }

        public int Delete(string code)
        {
            string query = @"DELETE FROM [sys_Dictionary] WHERE DicCode = @DicCode";
            SqlParameter param = new SqlParameter("@DicCode", code);
            int iCount = SQLHelper.ExecuteNonQuery(DBConnStr, CommandType.Text, query, param);

            return iCount;
        }

        public bool Update(string code, string en_name, string chs_name, string cht_name
                        , string pCode, string remark, int? index, bool? isDisable
                        , Guid modifyUser, DateTime dateTime, string ext1, string ext2, string ext3, string ext4)
        {
            bool result = false;
            string query = @"UPDATE [dbo].[sys_Dictionary]
                               SET [EN_Name] = @EN_Name
                                  ,[CHS_Name] = @CHS_Name
                                  ,[CHT_Name] = @CHT_Name
                                  ,[ParentCode] = @ParentCode
                                  ,[Remark] = @Remark
                                  ,[Index] = @Index
                                  ,[IsDisable] = @IsDisable
                                  ,[ModifyUser] = @ModifyUser
                                  ,[ModifyDate] = @ModifyDate
                                  ,[Extend1] = @Extend1
                                  ,[Extend2] = @Extend2
                                  ,[Extend3] = @Extend3
                                  ,[Extend4] = @Extend4
                             WHERE [DicCode] = @DicCode";
            SqlParameter[] paramList = new SqlParameter[] 
            {
                new SqlParameter("@DicCode", code)
                , new SqlParameter("@EN_Name", en_name)
                , new SqlParameter("@CHS_Name", chs_name)
                , new SqlParameter("@CHT_Name", cht_name)
                , new SqlParameter("@ParentCode", pCode)
                , new SqlParameter("@Remark", remark)
                , new SqlParameter("@Index", index)
                , new SqlParameter("@IsDisable", isDisable)
                , new SqlParameter("@ModifyUser", modifyUser)
                , new SqlParameter("@ModifyDate", dateTime)
                , new SqlParameter("@Extend1", ext1)
                , new SqlParameter("@Extend2", ext2)
                , new SqlParameter("@Extend3", ext3)
                , new SqlParameter("@Extend4", ext4)
            };
            int iCount = SQLHelper.ExecuteNonQuery(DBConnStr, CommandType.Text, query, paramList);
            result = iCount > 0 ? true : false;

            return result;
        }
    }
}
