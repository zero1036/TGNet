using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*****************************************************
* 目的：部门to用户关系模型
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
    /// 部门to用户关系模型
    /// </summary>
    public class Dep2UserRelTM
    {
        /// <summary>
        /// 系统部门编号
        /// </summary>
        public int SysDepartmentId { get; set; }
        /// <summary>
        /// 系统用户编号
        /// </summary>
        public int SysUserId { get; set; }
    }
}
