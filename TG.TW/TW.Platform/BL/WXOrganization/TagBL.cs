using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TW.Platform.DA;
using EG.WeChat.Utility.Tools;
/*****************************************************
* 目的：标签DA
* 创建人：林子聪
* 创建时间：20150427
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace TW.Platform.BL
{
    public class TagBL
    {
        private TagDA _da = new TagDA();
        /// <summary>
        /// 获取租户所有标签
        /// </summary>
        /// <typeparam name="Tag"></typeparam>
        /// <returns></returns>
        public List<Tag> GetTags<Tag>()
        {
            var dt = _da.GetTags();
            return dt.ToList<Tag>();
        }
    }
}
