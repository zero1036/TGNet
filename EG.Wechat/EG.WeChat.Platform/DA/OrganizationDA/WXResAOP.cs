using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*****************************************************
* 目的：資源表数据更新时AOP执行抽象类
* 创建人：林子聪
* 创建时间：20150202
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Platform.DA
{
    /// <summary>
    /// 对应数据变更接口
    /// </summary>
    public class WXResAOP
    {
        /// <summary>
        /// 保存单个资源
        /// </summary>
        /// <param name="lcid"></param>
        /// <param name="lcname"></param>
        /// <param name="lcclassify"></param>
        /// <param name="media_Id"></param>
        /// <param name="media_Type"></param>
        /// <param name="content"></param>
        /// <param name="iCreateTime"></param>
        /// <param name="iSourceType"></param>
        /// <returns></returns>
        public virtual int? SaveResource(int? lcid, string lcname, string lcclassify, string media_Id, string media_Type, string content, DateTime iCreateTime, int iSourceType)
        {
            return null;
        }
        /// <summary>
        /// 刪除資源
        /// </summary>
        /// <param name="lcid"></param>
        /// <returns></returns>
        public virtual int DeleteResource(int lcid) { return -1; }
    }
}
