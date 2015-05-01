using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TW.Platform.DA;
using EG.WeChat.Utility.Tools;
/*****************************************************
* 目的：部门逻辑处理
* 创建人：林子聪
* 创建时间：20140427
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace TW.Platform.BL
{
    public class DepartmentBL
    {
        private DepartmentDA _da = new DepartmentDA();
        /// <summary>
        /// 获取租户所有部门
        /// </summary>
        /// <typeparam name="Dep"></typeparam>
        /// <returns></returns>
        public List<Dep> GetDepartments<Dep>()
        {
            var dt = _da.GetDepartments();
            return dt.ToList<Dep>();
        }
        /// <summary>
        /// 获取租户所有部门对应用户关系
        /// </summary>
        /// <typeparam name="Dep"></typeparam>
        /// <returns></returns>
        public List<Rel> GetDep2UserRel<Rel>()
        {
            var dt = _da.GetDep2UserRel();
            return dt.ToList<Rel>();
        }
    }
}
