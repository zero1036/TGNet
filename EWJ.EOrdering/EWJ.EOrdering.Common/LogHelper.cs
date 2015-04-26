using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using log4net.Config;
using System.IO;

[assembly: XmlConfigurator(Watch = true)]
namespace EWJ.EOrdering.Common
{
    public class LogHelper
    {
        public static void SetXmlConfigurator(FileInfo configFile)
        {
            XmlConfigurator.Configure(configFile);
        }

        /// <summary>
        /// 取得日志所在的方法和位置
        /// </summary>
        /// <returns>位置的字符串</returns>
        public static string LogPosition()
        {
            StackTrace trace = new StackTrace();
            StackFrame frame = trace.GetFrame(3);

            return string.Format("[Method:{0}]", frame.GetMethod().Name);
        }

        /// <summary>
        /// 记录系统运行日志
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="level">日志类型</param>
        /// <param name="message">日志信息</param>
        public static void Write(Type type, Enums.LogType logType, string message)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(type);
            message = string.Format("{2}{2}{0}{2}Message:{1}{2}{2}{3}{2}{2}", LogPosition(), message, Environment.NewLine, "================================================================================");

            switch (logType)
            {
                case Enums.LogType.Fatal:
                    if (log.IsFatalEnabled) log.Fatal(message);
                    break;
                case Enums.LogType.Error:
                    if (log.IsErrorEnabled) log.Error(message);
                    break;
                case Enums.LogType.Warn:
                    if (log.IsWarnEnabled) log.Warn(message);
                    break;
                case Enums.LogType.Debug:
                    if (log.IsDebugEnabled) log.Debug(message);
                    break;
                case Enums.LogType.Info:
                    if (log.IsInfoEnabled) log.Info(message);
                    break;
            }

            log = null;
        }

        /// <summary>
        /// 记录系统异常日志
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="exception">异常</param>
        public static void Write(Type type, Exception exception)
        {
            Exception innerEx = exception.InnerException;
            string message = string.Format("{0}{3}方法:{1}{3}堆栈详细:{2}",
                exception.Message + (innerEx != null ? "内部异常：" + innerEx.Message : string.Empty),
                exception.Source + (innerEx != null ? "内部源：" + innerEx.Source : string.Empty),
                exception.StackTrace + (innerEx != null ? "内部堆栈：" + innerEx.StackTrace : string.Empty),
                Environment.NewLine);
            Write(type, Enums.LogType.Error, message);
        }
    }
}
