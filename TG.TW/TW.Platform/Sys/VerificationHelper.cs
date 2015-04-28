using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*****************************************************
* 目的：业务验证类
* 创建人：林子聪
* 创建时间：20150427
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace TW.Platform.Sys
{
    public static class VerificationHelper
    {
        /// <summary>
        /// 验证datatable是否为空
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static bool VDTableNull(System.Data.DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
                return false;
            return true;
        }
    }
}
