using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TW.Platform.Model;
using TW.Platform.Sys;
/*****************************************************
* 目的：用户相关DA
* 创建人：林子聪
* 创建时间：20150427
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace TW.Platform.DA
{
    /// <summary>
    /// 组织机构DA模块，主要用于登录验证，无需限制租户编号
    /// </summary>
    public class OrgDA
    {
        protected ADOTemplateX _pADO = new ADOTemplateX();
        #region 登录验证
        /// <summary>
        /// 通过租户ID获取路由信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetTenantRoutesByTid(int tid)
        {
            DataTable dt = _pADO.Query(SqlScriptHelper.SEL_TENANTROUTES, new string[] { "?tid" }, new object[] { tid }, string.Empty);
            return dt;
        }
        /// <summary>
        /// 通过UserID获取用户信息
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public DataTable GetUserByUserID(string UserId)
        {
            DataTable dt = _pADO.Query(SqlScriptHelper.SEL_USER2TID, new string[] { "?userid", "?tbname" }, new object[] { UserId, SqlScriptHelper.T_USER }, string.Empty);

            if (dt.IsNull())
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
                if (!dt.IsNull())
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
            var sFilter = string.Format("sysuserid={0} ", iSysUserID);
            foreach (int idid in iSysDepartmentIDs)
            {
                sFilter = string.Format("{0} or sysdepartmentid={1}", sFilter, idid);
            }
            sFilter = string.Format("({0})", sFilter);
            sFilter = string.Format(SqlScriptHelper.SEL_TAG4USER, sFilter);
            return _pADO.Query(sFilter, new string[] { "?tid", }, new object[] { tid, }, string.Empty);
        }
        /// <summary>
        /// 查询标签拥有菜单
        /// </summary>
        /// <param name="iSysTagID"></param>
        /// <returns></returns>
        public DataTable GetMenuBySysTagID(int[] iSysTagIDs)
        {
            var sFilter = string.Empty;
            foreach (int itagid in iSysTagIDs)
            {
                sFilter = string.Format("{0} and r.systagid<>{1}", sFilter, itagid);
            }
            sFilter = sFilter.Substring(4, sFilter.Length - 4);
            sFilter = string.Format("({0})", sFilter);
            sFilter = string.Format(SqlScriptHelper.SEL_MENU4TAG, sFilter);
            return _pADO.Query(sFilter, null, null, string.Empty);
        }

        #endregion
    }
    public class UserDA : BaseDA
    {
        protected ADOTemplateX _pADO = new ADOTemplateX();
        /// <summary>
        /// 获取用户
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public DataTable GetUsers()
        {
            var tid = SysCurUser.GetCurUser().Tid;
            if (tid == -1) return null;
            var tbNameFull = SysCurUser.GetCurUser().TenantRoutes.Single(t => t.TbName == SqlScriptHelper.T_USER).TbNameFull;

            var sFIlter = string.Format(SqlScriptHelper.SEL_SINGLEUSER, tbNameFull, "1=1");
            DataTable dt = _pADO.Query(sFIlter, new string[] { "?tid" }, new object[] { tid }, string.Empty);
            return dt;
        }
    }
}
