using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Senparc.Weixin.MP.AdvancedAPIs;
/*****************************************************
* 目的：模板消息基类
* 创建人：林子聪
* 创建时间：20141107
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Platform.Model
{
    /// <summary>
    /// 模板消息基类，包含基础属性OpenID、链接URL地址、顶部颜色、模板消息ID
    /// </summary>
    public class WCTemplateMessage
    {
        /// <summary>
        /// OpenID
        /// </summary>
        public string OpenID { get; set; }
        /// <summary>
        /// 链接地址
        /// </summary>
        public string URL { get; set; }
        /// <summary>
        /// 顶部颜色
        /// </summary>
        public string TopColor { get; set; }
        /// <summary>
        /// 模板消息ID
        /// </summary>
        public string TemplateID
        {
            get
            {
                return "1jrlBA0RQeCaBzN7CaUiWZjfRbIh6I_7tG8eGlg0x-8";
            }
        }
    }
 


}
