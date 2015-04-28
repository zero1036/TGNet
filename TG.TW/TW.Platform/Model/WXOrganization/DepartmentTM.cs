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
    /// 
    /// </summary>
    public class DepartmentTM
    {
        /// <summary>
        /// 系统部门编号
        /// </summary>
        public int SysDepartmentId { get; set; }
        public int Tid { get; set; }
        public int Did { get; set; }
        public int ParentDid { get; set; }
        public string Name { get; set; }
        public string Order { get; set; }
    }
}
