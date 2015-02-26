using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EG.WeChat.Utility.WeiXin.ResponseChain
{

    /// <summary>
    /// 描述用途的特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class DescriptionAttribute : System.Attribute
    {
        /// <summary>
        /// 描述(只读)
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="limitedDataTypes">要限制的数据类型</param>
        public DescriptionAttribute(string description)
        {
            this.Description = description;
        }

        /// <summary>
        /// 获取指定类型的Description信息
        /// </summary>
        /// <param name="type">指定的类型</param>
        public static string GetDescription(Type type)
        {
            if (type == null)
                return "未知类型";

            DescriptionAttribute attribute = Attribute.GetCustomAttribute(type, typeof(DescriptionAttribute)) as DescriptionAttribute;
            if (attribute == null)
                return null;
            else
                return attribute.Description;
        }
    }
}