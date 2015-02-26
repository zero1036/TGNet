using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;


namespace EG.Utility.Log
{
    /// <summary>
    /// LOG System Info
    /// </summary>
    public class Log
    {
        private static ILog log;

        private static void initLog()
        {
            if (log == null)
                log = LogManager.GetLogger("LOG");
        }

        /// <summary>
        /// DEBUG LOG
        /// </summary>
        /// <param name="module">Module Name</param>
        /// <param name="message">DEBUG Message</param>
        public static void Log_DEBUG(string module, string message)
        {
            initLog();
            log.Debug("Module:" + module + " Message:" + message);
        }


        /// <summary>
        /// Info LOG
        /// </summary>
        /// <param name="module">Module Name</param>
        /// <param name="message">INFO Message</param>
        public static void Log_Info(string module, string message)
        {
            initLog();
            log.Info("Module:" + module + " Message:" + message);
        }

        /// <summary>
        /// check is Debug Enable
        /// </summary>
        /// <returns></returns>
        public static bool isDebugEnable()
        {
            initLog();
            return log.IsDebugEnabled;
        }

        /// <summary>
        /// check is Info Enable
        /// </summary>
        /// <returns></returns>
        public static bool isInfoEnable()
        {
            initLog();
            return log.IsInfoEnabled;
        }

        /// <summary>
        /// Info LOG
        /// </summary>
        /// <param name="module">Module Name</param>
        /// <param name="message">INFO Message</param>
        public static void Log_Info(Type module, string message)
        {
            initLog();
            log.Info("Type:" + module.FullName + " Message:" + message);
        }

        /// <summary>
        /// DEBUG LOG
        /// </summary>
        /// <param name="module">Module Name</param>
        /// <param name="message">LOG Message</param>
        public static void Log_DEBUG(Type module, string message)
        {
            initLog();
            log.Debug("Type:" + module.FullName + " Message:" + message);
        }

        /// <summary>
        /// check is Error Enable
        /// </summary>
        /// <returns></returns>
        public static bool isErrorEnable()
        {
            initLog();
            return log.IsErrorEnabled;
        }


        /// <summary>
        /// ERROR LOG
        /// </summary>
        /// <param name="module">Module Name</param>
        /// <param name="ex">Exception</param>
        public static void Log_ERROR(Type module, Exception ex)
        {
            initLog();
            log.Error("Type:" + module.FullName + " Exception:" + ex.Message + "\r\nStackTrace:" + ex.StackTrace);
        }



        /// <summary>
        /// ERROR LOG
        /// </summary>
        /// <param name="module">Module Name</param>
        /// <param name="ex">Exception</param>
        public static void Log_ERROR(string module, Exception ex)
        {
            initLog();
            log.Error("Module:" + module + " Exception:" + ex.Message + "\r\nStackTrace:" + ex.StackTrace);
        }
    }
}
