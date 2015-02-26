using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
/*****************************************************
* 目的：消息模板页面配置Model
* 创建人：林子聪
* 创建时间：20141118
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Web.Models
{
    public class TemplateMessageConfigM
    {
        private string m_Example = string.Empty;
        [Required(ErrorMessage = " * Required")]
        public string ID { get; set; }
        [Required(ErrorMessage = " * Required")]
        public string Title { get; set; }
        [Required(ErrorMessage = " * Required")]
        public string Property { get; set; }
        public string Content { get; set; }
        public string Example { get; set; }
        public string ExampleDisplay
        {
            get
            {
                if (Example == null)
                    return string.Empty;
                m_Example = Example.Replace("\r\n", "<br/>");
                return m_Example;
            }
        }
    }
}