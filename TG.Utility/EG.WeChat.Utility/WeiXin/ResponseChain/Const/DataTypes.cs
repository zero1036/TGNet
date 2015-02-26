using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace EG.WeChat.Utility.WeiXin.ResponseChain
{
    /// <summary>
    /// 用户的输入类型枚举
    /// </summary>
    [Flags]
    public enum DataTypes : ushort
    {
        
        /// <summary>
        /// 文本
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("文本")]
        Text = 1,

        /// <summary>
        /// 图片
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("图片")]
        Image = 2,

        /// <summary>
        /// 语音
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("语音")]
        Voice = 4,

        /// <summary>
        /// 图文
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("图文")]
        News = 8,

        /// <summary>
        /// 视频
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("视频")]
        Video = 16,

        //目前暂时打算去支持，尚未预见对应需求的场景是怎样：
        //Link,
        

        /// <summary>
        /// 全部的输入
        /// </summary>
        InputAll = Text | Image | Voice | Video,

        /// <summary>
        /// 全部的输出
        /// </summary>
        OutputAll = Text | News,
    }


    /// <summary>
    /// DataTypes枚举值的扩展方法
    /// </summary>
    public static class DataTypesExtension
    {
        /// <summary>
        /// 获得枚举对应的文字描述
        /// </summary>
        /// <param name="targetValue">枚举值</param>
        /// <param name="splitChar">分隔符(当枚举值为多个值的组合时，进行分隔的字符)</param>
        /// <returns>描述</returns>
        public static string GetDescription(this DataTypes targetValue, string separatorChar = ",")
        {
            List<string> result = new List<string>();
            Type targetType = typeof(DataTypes);

            foreach (var field in targetType.GetFields())
            {
                if (field.FieldType.IsEnum == false)
                    continue;

                //获得枚举值
                DataTypes currentValue = (DataTypes)targetType.InvokeMember(field.Name, BindingFlags.GetField, null, null, null);

                //如果targetValue没有此值，跳过
                if (targetValue.HasFlag(currentValue) == false)
                    continue;

                //尝试获取System.ComponentModel.DescriptionAttribute
                var att = Attribute.GetCustomAttribute(field, typeof(System.ComponentModel.DescriptionAttribute), false) as System.ComponentModel.DescriptionAttribute;
                if ( att != null )
                {
                    result.Add(att.Description);
                }
            }

            //组合最终结果
            return String.Join(separatorChar,result);
        }
    }
}