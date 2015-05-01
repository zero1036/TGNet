using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TW.Platform.Model;
using TW.Platform.Sys;
/*****************************************************
* 目的：标签DA
* 创建人：林子聪
* 创建时间：20140427
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace TW.Platform.DA
{
    public class TagDA
    {
        protected ADOTemplateX _pADO = new ADOTemplateX();
        /// <summary>
        /// 获取标签
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public DataTable GetTags()
        {
            var tid = SysCurUser.GetCurUser().Tid;
            if (tid == -1) return null;

            DataTable dt = _pADO.Query(SqlScriptHelper.Tag.SEL_TAGS, new string[] { "?tid" }, new object[] { tid }, string.Empty);
            return dt;
        }
    }
}
