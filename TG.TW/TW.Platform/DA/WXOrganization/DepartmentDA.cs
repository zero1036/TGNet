using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TW.Platform.Model;
using TW.Platform.Sys;
/*****************************************************
* 目的：部门DA
* 创建人：林子聪
* 创建时间：20140427
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace TW.Platform.DA
{
    public class DepartmentDA : BaseDA
    {
        protected ADOTemplateX _pADO = new ADOTemplateX();
        /// <summary>
        /// 获取用户
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public DataTable GetDepartments()
        {
            var tid = SysCurUser.GetCurUser().Tid;
            if (tid == -1) return null;

            DataTable dt = _pADO.Query(SqlScriptHelper.Department.SEL_DEPARTMENTS, new string[] { "?tid" }, new object[] { tid }, string.Empty);
            return dt;
        }
        /// <summary>
        /// 获取租户所有部门对应用户关系
        /// </summary>
        /// <returns></returns>
        public DataTable GetDep2UserRel()
        {
            var tid = SysCurUser.GetCurUser().Tid;
            if (tid == -1) return null;

            DataTable dt = _pADO.Query(SqlScriptHelper.Department.SEL_DEP2USERREL, new string[] { "?tid" }, new object[] { tid }, string.Empty);
            return dt;
        }
    }
}
