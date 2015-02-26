using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EG.WeChat.Utility.Tools
{
    /// <summary>
    /// Unix时间戳 辅助服务<para />
    /// (.Net目前不提供此时间戳的支持)
    /// </summary>
    public sealed class UnixTimeStampHelper
    {
        /* 微信使用的时间戳，为“Unix时间戳”。一种比较通用的时间戳方式
         * 
         * 大概对应关系：
         * 1.起始时间为：UTC的1970-01-01 0:0:0;
         * 2.单位是秒。
         * 
         * 时间戳对应的DateTime并不包含时区信息。
         */

        /// <summary>
        /// 起始时间
        /// </summary>
        static DateTime BasicStatrTime = new DateTime(1970,1,1,0,0,0,0,System.DateTimeKind.Utc);

        /// <summary>
        /// 将时间戳，转换成DateTime<para/>
        /// (结果，不包含时区信息)
        /// </summary>
        /// <param name="timeStamp">Unix时间戳</param>
        /// <returns>DateTime</returns>
        public static DateTime ConvertToDateTime(long timeStamp)
        {
            //##添加秒数  此处的ToLocalTime，只是对应回BasicStatrTime的UTC时差
            System.DateTime result = BasicStatrTime.AddSeconds(timeStamp).ToLocalTime();
            return result;
        }

        /// <summary>
        /// 将DateTime，转换成时间戳
        /// </summary>
        /// <param name="dateTime">DateTime</param>
        /// <returns>Unix时间戳</returns>
        public static long ConvertToTimeStamp(DateTime dateTime)
        {
            //##返回秒数 此处的ToLocalTime，只是对应回BasicStatrTime的UTC时差
            return (long)(dateTime - BasicStatrTime.ToLocalTime()).TotalSeconds;
        }
    }
}