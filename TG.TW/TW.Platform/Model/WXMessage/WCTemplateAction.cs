using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
/*****************************************************
* 目的：模板消息——执行模板
* 创建人：林子聪
* 创建时间：20141107
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace TW.Platform.Model
{
    /// <summary>
    /// 模板消息——执行模板
    /// </summary>
    public class WCTemplateAction
    {
        public TemplateDataItem first { get; set; }
        public TemplateDataItem keyword1 { get; set; }
        public TemplateDataItem keyword2 { get; set; }
        public TemplateDataItem keyword3 { get; set; }
        public TemplateDataItem keyword4 { get; set; }
        public TemplateDataItem keyword5 { get; set; }
        public TemplateDataItem keyword6 { get; set; }
        public TemplateDataItem remark { get; set; }
        public TemplateDataItem productType { get; set; }
        public TemplateDataItem name { get; set; }
        public TemplateDataItem number { get; set; }
        public TemplateDataItem expDate { get; set; }
    }

}
