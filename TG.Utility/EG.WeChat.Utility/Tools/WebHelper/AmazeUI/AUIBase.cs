using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*****************************************************
* 目的：AmazeUI JS辅助
* 创建人：林子聪
* 创建时间：20141215
* 修改目的：
* 修改人：
* 修改时间：
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Utility.Tools
{
    /// <summary>
    /// AUIBase
    /// </summary>
    public class AUIBase
    {
        public string theme
        { get; set; }
    }
    /// <summary>
    /// 轮播图片ASlider
    /// </summary>
    public class ASlider : AUIBase
    {
        public string sliderConfig
        { get; set; }
        public List<ASliderContent> content
        { get; set; }
        public class ASliderContent
        {
            public string thumb
            { get; set; }
            public string img
            { get; set; }
            public string desc
            { get; set; }
        }
    }
    /// <summary>
    /// 分段内容框AAccordion
    /// </summary>
    public class AAccordion : AUIBase
    {
        public AOptions options
        { get; set; }
        public List<AAccordionContent> content
        { get; set; }
        public class AOptions
        {
            public bool multiple
            { get; set; }
        }
        public class AAccordionContent
        {
            public string title
            { get; set; }
            public string content
            { get; set; }
            public bool active
            { get; set; }
        }
    }
    /// <summary>
    /// 列表AList
    /// </summary>
    public class AList
    {
        public AListContent content
        { get; set; }
        public class AListContent
        {
            public AListHeader header
            { get; set; }
            public List<AListMain> main
            { get; set; }
            public class AListHeader
            {
                public string title
                { get; set; }
                public string link
                { get; set; }
                public string moreText
                { get; set; }
                public string morePosition
                { get; set; }
            }
            public class AListMain
            {
                public string title
                { get; set; }
                public string link
                { get; set; }
                public string date
                { get; set; }
            }
        }
    }
}
