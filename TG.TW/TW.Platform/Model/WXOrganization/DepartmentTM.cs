using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*****************************************************
* 目的：部门模型
* 创建人：林子聪
* 创建时间：20150427
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace TW.Platform.Model
{
    /// <summary>
    /// 部门基础属性模型
    /// </summary>
    public class DepartmentBM
    {
        /// <summary>
        /// 系统部门编号
        /// </summary>
        public int SysDepartmentId { get; set; }

        public int Did { get; set; }
        public int ParentDid { get; set; }
        public string Name { get; set; }
        public string Order { get; set; }
    }
    /// <summary>
    /// 部门数据表模型
    /// </summary>
    public class DepartmentTM : DepartmentBM
    {
        public int Tid { get; set; }
    }
    /// <summary>
    /// 部门视图模型
    /// </summary>
    public class DepartmentVM : DepartmentBM
    {
        /// <summary>
        /// 所有用户
        /// </summary>
        public List<UserVM> Users { get; set; }
    }

}
