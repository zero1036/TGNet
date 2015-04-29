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
    /// <summary>
    /// 
    /// </summary>
    public class TagTM
    {
        public int SysTagId { get; set; }
        public int Tid { get; set; }
        public int TagId { get; set; }
        public string TagName { get; set; }
    }
}
