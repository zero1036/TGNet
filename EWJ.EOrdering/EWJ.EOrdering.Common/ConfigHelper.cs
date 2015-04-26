
using System.Configuration;

namespace EWJ.EOrdering.Common
{
    public class ConfigHelper
    {
        /// <summary>
        /// 默认数据库连接字符串
        /// </summary>
        public static string DEFAULT_CONNECTION
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            }
        }

      

        public static string DEFAULT_ImageFolder
        {
            get { return ConfigurationManager.AppSettings["ImageFolderUrl"].ToString(); }
            
        }
        
    }
}
