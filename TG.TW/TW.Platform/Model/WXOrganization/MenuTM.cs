using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*****************************************************
* 目的：标签模型
* 创建人：林子聪
* 创建时间：20150427
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace TW.Platform.Model
{
    public class MenuTM
    {
        /// <summary>
        /// 菜单代码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 菜单类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 父级菜单代码
        /// </summary>
        public string FatherCode { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 连接action
        /// </summary>
        public string Href { get; set; }
    }
}
