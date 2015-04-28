using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TW.Platform.Model;
using TW.Platform.Sys;

namespace TW.Platform.DA
{
    public class UserDA
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public DataTable GetUserByUserID(string UserId)
        {
            ADOTemplateX pADO = new ADOTemplateX();
            DataTable dt = pADO.Query(SqlScriptHelper.SEL_USER2TID, new string[] { "?userid", "?tbname" }, new object[] { UserId, SqlScriptHelper.T_USER }, string.Empty);

            if (!VerificationHelper.VDTableNull(dt))
                return null;
            string tbname = dt.Rows[0][0] + string.Empty;
            int tid = Convert.ToInt32(dt.Rows[0][1] + string.Empty);

            var pfilter = string.Format("userid=?userid");
            var psql = string.Format(SqlScriptHelper.SEL_SINGLEUSER, tbname, pfilter);
            return pADO.Query(psql, new string[] { "?tid", "?userid" }, new object[] { tid, UserId }, string.Empty);
        }
    }
}
