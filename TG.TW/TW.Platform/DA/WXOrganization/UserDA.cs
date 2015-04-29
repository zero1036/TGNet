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
        private ADOTemplateX _pADO = new ADOTemplateX();
        /// <summary>
        /// 通过UserID获取用户信息
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public DataTable GetUserByUserID(string UserId)
        {
            DataTable dt = _pADO.Query(SqlScriptHelper.SEL_USER2TID, new string[] { "?userid", "?tbname" }, new object[] { UserId, SqlScriptHelper.T_USER }, string.Empty);

            if (!VerificationHelper.VDTableNull(dt))
                return null;
            string tbname = dt.Rows[0][0] + string.Empty;
            int tid = Convert.ToInt32(dt.Rows[0][1] + string.Empty);

            var pfilter = string.Format("userid=?userid");
            var psql = string.Format(SqlScriptHelper.SEL_SINGLEUSER, tbname, pfilter);
            return _pADO.Query(psql, new string[] { "?tid", "?userid" }, new object[] { tid, UserId }, string.Empty);
        }
        /// <summary>
        /// 通过微信号获取用户信息
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public DataTable GetUserByWeixinid(string Wexinid)
        {
            string tbn = string.Empty;
            for (int i = 1; i <= ConstStr.LM_USERTABLECOUNT; i++)
            {
                tbn = string.Format("{0}_{1}", SqlScriptHelper.T_USER, i);
                var sql = string.Format(SqlScriptHelper.SEL_USERBYWEIXINID, tbn);

                var dt = _pADO.Query(sql, null, null, string.Empty);
                if (!VerificationHelper.VDTableNull(dt))
                {
                    return dt;
                }
            }
            return null;
        }
        /// <summary>
        /// 通过sysuseerid获取用户所在部门
        /// </summary>
        /// <param name="tid"></param>
        /// <param name="iSysUserID"></param>
        /// <returns></returns>
        public DataTable GetDepartmentBySysUserID(int tid, int iSysUserID)
        {
            return _pADO.Query(SqlScriptHelper.SEL_DEPARTMENT4USER, new string[] { "?tid", "?sysuserid" }, new object[] { tid, iSysUserID }, string.Empty);
        }
        /// <summary>
        /// 通过sysuserid获取用户所属标签
        /// </summary>
        /// <param name="tid"></param>
        /// <param name="iSysUserID"></param>
        /// <returns></returns>
        public DataTable GetTagsBySysUserID(int tid, int iSysUserID, int[] iSysDepartmentIDs)
        {
            var sFilter = string.Format("sysuserid={0} or", iSysUserID);
            foreach (int idid in iSysDepartmentIDs)
            {
                sFilter = string.Format("{0} sysdepartmentid={1}", sFilter, idid);
            }
            sFilter = string.Format("({0})", sFilter);
            sFilter = string.Format(SqlScriptHelper.SEL_TAG4USER, sFilter);
            return _pADO.Query(sFilter, new string[] { "?tid", }, new object[] { tid, }, string.Empty);
        }
    }
}
