using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*****************************************************
* 目的：路由信息表模型DA
* 创建人：林子聪
* 创建时间：20150427
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace TW.Platform.Model
{
    public class TenantRouteTM
    {
        public int TenID { get; set; }
        public string TbName { get; set; }
        public int Tid { get; set; }
        public int Trid { get; set; }
    }

    public class TenantRouteM : TenantRouteTM
    {
        public string TbNameFull
        {
            get
            {
                return string.Format("{0}_{1}", base.TbName, base.Trid);
            }
        }
    }
}
