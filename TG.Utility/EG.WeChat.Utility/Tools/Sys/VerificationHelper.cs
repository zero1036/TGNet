using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
/*****************************************************
* 目的：系统验证扩展类
* 创建人：林子聪
* 创建时间：20150427
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace System
{
    public static class VerificationHelper
    {
        /// <summary>
        /// 验证datatable是否为空
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static bool IsNull(this System.Data.DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
                return true;
            return false;
        }
        /// <summary>
        /// 验证集合是否为空
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool IsNull(this IList p)
        {
            if (p == null || p.Count == 0)
                return true;
            return false;
        }
    }
}
