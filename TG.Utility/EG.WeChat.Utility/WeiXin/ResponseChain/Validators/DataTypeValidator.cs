using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EG.WeChat.Utility.WeiXin.ResponseChain
{

    /// <summary>
    /// 数据格式的验证器
    /// </summary>
    internal sealed class DataTypeValidator
    {
        /// <summary>
        /// 验证是否合法
        /// </summary>
        /// <param name="handler">IHandler处理器</param>
        public static bool IsValid(Type CheckedType,DataTypes currentTypes)
        {
            //##获取特性
            InputDataTypeLimitAttribute targetAttribute = Attribute.GetCustomAttribute(CheckedType, typeof(InputDataTypeLimitAttribute)) as InputDataTypeLimitAttribute;

            //默认允许全部格式
            if (targetAttribute == null)
                return true;

            //检查(currentTypes需要在targetAttribute范围内)
            return targetAttribute.LimitedDataTypes.HasFlag(currentTypes);
        }

    }
}