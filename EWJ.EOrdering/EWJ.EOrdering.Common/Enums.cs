using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWJ.EOrdering.Common
{
    public class Enums
    {
        public enum LogType
        {
            /// <summary>
            /// 关闭，不记录日志
            /// </summary>
            [Description("关闭")]
            Off,
            /// <summary>
            /// 致命错误
            /// </summary>
            [Description("致命错误")]
            Fatal,
            /// <summary>
            /// 错误
            /// </summary>
            [Description("错误")]
            Error,
            /// <summary>
            /// 警告
            /// </summary>
            [Description("警告")]
            Warn,
            /// <summary>
            /// 信息
            /// </summary>
            [Description("信息")]
            Info,
            /// <summary>
            /// 调试
            /// </summary>
            [Description("调试")]
            Debug,
            /// <summary>
            /// 所有信息
            /// </summary>
            [Description("所有信息")]
            All
        }
    }
}
