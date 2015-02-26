using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using log4net;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = @"log4net.config", Watch = true)]
/*  为了方便开发者直接使用，将下面的日志类的命名空间，定义为System。 */


/* 这里保留这个命名空间，因为“扩展方法”对命名空间匹配。 */
namespace System
{
    /// <summary>
    /// 日志记录器
    /// </summary>
    public sealed class Logger
    {
        
        #region 构造函数

        static Logger()
        {
            //配置 日志记录器

            //##配置log4Net
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(System.Web.Hosting.HostingEnvironment.MapPath(@"~/bin/log4net.config")));
        }
        
        #endregion


        #region log4Net

        /// <summary>
        /// 获取log4Net类型的日志记录器(只读)[推荐]
        /// </summary>
        public static ILog Log4Net
        {
            get
            {
                //获取默认类型的记录器(调用方的方法的类)
                Type targetType = new StackFrame(1).GetMethod().DeclaringType;
                return LogManager.GetLogger(targetType);
            }
        }
        
        /// <summary>
        /// 获取log4Net类型的日志记录器<para />
        /// (如果确实有需要，需要指定为其他类型，则使用此方法)
        /// </summary>
        /// <typeparam name="T">日志记录器对应的模块类型</typeparam>
        public static ILog Log4NetWithType<T>()
        {
            return LogManager.GetLogger(typeof(T));
        }

        #endregion

    }
}
