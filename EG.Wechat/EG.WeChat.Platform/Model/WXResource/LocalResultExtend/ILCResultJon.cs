using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*****************************************************
* 目的：本地資源resultjson
* 创建人：林子聪
* 创建时间：20150129
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Platform.Model
{
    public interface ILCResultJon
    {
        /// <summary>
        /// 本地编号
        /// </summary>
        int? lcId
        {
            get;
            set;
        }
        /// <summary>
        /// 本地名称
        /// </summary>
        string lcName
        {
            get;
            set;
        }
        /// <summary>
        /// 本地分类
        /// </summary>
        string lcClassify
        {
            get;
            set;
        }
    }
}
