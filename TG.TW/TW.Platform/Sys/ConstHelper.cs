using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*****************************************************
* 目的：常量设置
* 创建人：林子聪
* 创建时间：20150427
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace TW.Platform.Sys
{
    public class ConstStr
    {
        #region Session const
        public const string SESSION_CURRENT_USER = "CURRENT_USER";
        public const string SESSION_CURRENT_USERID = "CURRENT_USERID";
        public const string SESSION_CURRENT_TID = "CURRENT_TID";
        #endregion

        #region Limit 限制
        /// <summary>
        /// 用户分支表最大分支表数
        /// </summary>
        public const int LM_USERTABLECOUNT = 10;
        #endregion

        #region CacheKey
        /// <summary>
        /// 组织机构模型
        /// </summary>
        public const string CACHE_ORGNIZATIONMODEL = "CACHE_ORG";
        #endregion
    }
}
