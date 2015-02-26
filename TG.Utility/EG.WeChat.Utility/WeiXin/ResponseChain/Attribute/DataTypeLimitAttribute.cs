using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EG.WeChat.Utility.WeiXin.ResponseChain
{

    /// <summary>
    /// 数据类型限制 特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class InputDataTypeLimitAttribute : System.Attribute
    {
        /// <summary>
        /// 限制的类型(只读)
        /// </summary>
        public DataTypes LimitedDataTypes { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="limitedDataTypes">要限制的数据类型</param>
        public InputDataTypeLimitAttribute(DataTypes limitedDataTypes)
        {
            this.LimitedDataTypes = limitedDataTypes;
        }
    }
}