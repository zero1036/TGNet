using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Configuration;
/*****************************************************
* 目的：控制是否写入模块日志
* 创建人：林子聪
* 创建时间：20150427
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace TW.Platform.Sys
{
    public class LogSwHelper
    {
        /// <summary>
        /// 单例日志
        /// </summary>
        public static LogSwHelper Sing = new LogSwHelper();
        /// <summary>
        /// 是否写入模块日志
        /// </summary>
        private bool? _IsWriteLog = null;
        public bool? IsWriteLog
        {
            get
            {
                if (_IsWriteLog == null)
                {
                    _IsWriteLog = WebConfigurationManager.AppSettings["ModuleLog"].ToString() == "true" ? true : false;
                }
                return _IsWriteLog;
            }
        }
        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="strlog"></param>
        public void Info(string strlog)
        {
            if (IsWriteLog != null && IsWriteLog.Value)
            {
                Logger.Log4Net.Info(strlog);
            }
        }
    }
}
