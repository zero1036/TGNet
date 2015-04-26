using EWJ.EOrdering.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace EWJ.EOrdering.DA.Sys
{
    public class UserDA : BaseDA
    {
        public DataSet Get(bool isShowAdmin)
        {
            string query = @"SELECT DISTINCT u.Id, u.StaffCode, u.Account, u.CHName, u.ENName
	                            , u.[Status], u.Tel, u.EMail, u.[Address], u.StoreId, u.UserType, Localization
	                            , d.DicName StatusName, s.StoreName
                            FROM sys_User u
                                LEFT JOIN sys_Dictionary d ON u.[Status] = d.DicCode
	                            LEFT JOIN t_StoreMaster s ON u.StoreId = s.StoreId
	                            LEFT JOIN sys_UserRoleRelation ur ON u.Id = ur.UserId
                            WHERE ISNULL(u.[IsDelete], 0) = @IsDelete {where}";

            IList<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@IsDelete", false));
            string strWhere = string.Empty;
            if (!isShowAdmin)
            {
                strWhere = " AND ur.RoleId <> @RoleId";
                paramList.Add(new SqlParameter("@RoleId", SysHelper.AMINISTRATOR_ROLE_ID));
            }
            query = query.Replace("{where}", strWhere);
            DataSet ds = SQLHelper.ExecuteDataset(DBConnStr, CommandType.Text, query, paramList.ToArray());

            return ds;
        }

        public DataSet Get(string account, string pwd)
        {
            string query = string.Format(@"SELECT u.Id, u.StaffCode, u.Account, u.CHName, u.ENName
	                                            , u.[Status], u.Tel, u.EMail, u.[Address], u.StoreId, u.UserType, Localization
                                            FROM sys_User u
                                            WHERE u.[Account] = @Account
	                                            AND u.[Password] = @Password
	                                            AND u.[Status] = @Status
	                                            AND ISNULL(u.[IsDelete], 0) = @IsDelete");
            SqlParameter[] paramList = new SqlParameter[]
            {
                new SqlParameter("@Account", account)
                , new SqlParameter("@Password", pwd)
                , new SqlParameter("@Status", "001001")
                , new SqlParameter("@IsDelete", false)
            };
            DataSet ds = SQLHelper.ExecuteDataset(DBConnStr, CommandType.Text, query, paramList);

            return ds;
        }
    }
}
