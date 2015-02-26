using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*****************************************************
* 目的：基础委托
* 创建人：林子聪
* 创建时间：20141106
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Utility.Tools
{
    /// <summary>
    /// 清空缓存委托
    /// </summary>
    /// <param name="pConfig"></param>
    public delegate void RemoveCache(ICacheConfig pConfig);
    /// <summary>
    /// 通用委托——参数string，返回值string
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public delegate string DlgCommonString(string str);
}
