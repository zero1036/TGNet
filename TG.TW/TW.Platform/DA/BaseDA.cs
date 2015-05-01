using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TW.Platform.Model;
using TW.Platform.Sys;
/*****************************************************
* 目的：基础DA
* 创建人：林子聪
* 创建时间：20150427
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace TW.Platform.DA
{
    public class BaseDA
    {
        /// <summary>
        /// 获取当前用户所属租户ID
        /// </summary>
        /// <returns></returns>
        protected int GetCurTid()
        {
            var pCurSysUser = SysCurUser.GetCurUser();
            if (pCurSysUser != null)
                return pCurSysUser.Tid;
            return -1;
        }
    }
}
