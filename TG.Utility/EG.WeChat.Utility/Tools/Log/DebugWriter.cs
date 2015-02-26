using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/* 这里保留这个命名空间，因为“扩展方法”对命名空间匹配。 */
namespace System
{
    /// <summary>
    /// 调试信息工具类
    /// <para />
    /// (将调试用途的信息，记录下来)[非最终用途的日志模块；仅仅用于，辅助地记录调试信息，然后不影响日志文件]
    /// </summary>
    sealed class DebugWriter
    {
        /// <summary>
        /// 写入到文件<para />
        /// (调用之后，请到App_Data目录，查看DebugOnly开头的txT文件。)
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        [Obsolete("目前已经正式接入日志记录模块，请使用System.Logger。DebugWriter只作为作为Web的调试用途工具。", false)]
        public static void WriteToFile(string title,string message)
        {
#if DEBUG
            try
            {
                //拼凑路径
                string path = System.Web.Hosting.HostingEnvironment.MapPath(String.Format("~/App_Data/DebugOnly_{0}_.txt", title));

                //写入日志
                using (System.IO.TextWriter tw = new System.IO.StreamWriter(path,true))
                {
                    tw.Write("\r\n");
                    tw.Write("-------- Time: --------" + DateTime.Now);
                    tw.Write("\r\n");
                    tw.WriteLine(message);
                    tw.Flush();
                    tw.Close();
                }
            }
            catch (Exception)
            {
                //不处理
            }
#endif
        }
    }
}