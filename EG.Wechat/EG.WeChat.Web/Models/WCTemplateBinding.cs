using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using EG.WeChat.Platform.BL;
using EG.WeChat.Platform.Model;
/*****************************************************
* 目的：模板消息——绑定模板
* 创建人：林子聪
* 创建时间：20141107
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Web.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class WCTemplateBindingA : WCTemplateMessage
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public WCTemplateBindingA()
        {
            TemData = new WCTemplateAction();
        }
        /// <summary>
        /// 标题
        /// </summary>
        public string first
        {
            get
            {
                if (TemData.first == null)
                    return string.Empty;
                return TemData.first.value;
            }
            set
            {
                TemData.first = new TemplateDataItem(value);
            }
        }
        /// <summary>
        /// 段落1
        /// </summary>
        public string keyword1
        {
            get
            {
                if (TemData.keyword1 == null)
                    return string.Empty;
                return TemData.keyword1.value;
            }
            set
            {
                TemData.keyword1 = new TemplateDataItem(value);
            }
        }
        /// <summary>
        /// 段落2
        /// </summary>
        public string keyword2
        {
            get
            {
                if (TemData.keyword2 == null)
                    return string.Empty;
                return TemData.keyword2.value;
            }
            set
            {
                TemData.keyword2 = new TemplateDataItem(value);
            }
        }
        /// <summary>
        /// 段落3
        /// </summary>
        public string keyword3
        {
            get
            {
                if (TemData.keyword3 == null)
                    return string.Empty;
                return TemData.keyword3.value;
            }
            set
            {
                TemData.keyword3 = new TemplateDataItem(value);
            }
        }
        /// <summary>
        /// 段落4
        /// </summary>
        public string keyword4
        {
            get
            {
                if (TemData.keyword4 == null)
                    return string.Empty;
                return TemData.keyword4.value;
            }
            set
            {
                TemData.keyword4 = new TemplateDataItem(value);
            }
        }
        /// <summary>
        /// 段落5
        /// </summary>
        public string keyword5
        {
            get
            {
                if (TemData.keyword5 == null)
                    return string.Empty;
                return TemData.keyword5.value;
            }
            set
            {
                TemData.keyword5 = new TemplateDataItem(value);
            }
        }
        /// <summary>
        /// 详情
        /// </summary>
        public string remark
        {
            get
            {
                if (TemData.remark == null)
                    return string.Empty;
                return TemData.remark.value;
            }
            set
            {
                TemData.remark = new TemplateDataItem(value);
            }
        }
        /// <summary>
        /// 模板Data
        /// </summary>
        public WCTemplateAction TemData { get; set; }
    }
}