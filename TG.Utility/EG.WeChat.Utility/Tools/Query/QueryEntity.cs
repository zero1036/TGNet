using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*****************************************************
* 目的：查询实体
* 创建人：林子聪
* 创建时间：20141202
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Utility.Tools
{
    public class QueryEntity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pName">查询字段名称</param>
        /// <param name="pValue">查询值</param>
        /// <param name="pConPic">true-and；false-or</param>
        /// <param name="pLike">false：模糊查询</param>
        public QueryEntity(string pName, object pValue, bool pConPic, bool pLike)
        {
            this.Name = pName;
            this.Value = pValue;
            this.ConPic = pConPic;
            this.IsLike = pLike;
        }
        /// <summary>
        /// 查询字段名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 查询值
        /// </summary>
        public object Value { get; set; }
        /// <summary>
        /// 连接符：true-and；false-or
        /// </summary>
        public bool ConPic { get; set; }
        /// <summary>
        /// 是否模糊查询
        /// </summary>
        public bool IsLike { get; set; }
    }
}
