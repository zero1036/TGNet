using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*****************************************************
* 目的：标签模型
* 创建人：林子聪
* 创建时间：20150427
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace TW.Platform.Model
{
    public class MenuVM
    {
        public MenuVM()
        {
            TemData = new MenuTM();
        }

        public string accessPath
        {
            get
            {
                return TemData.Href;
            }
            set
            {
                TemData.Href = value;
            }
        }

        public int delFlag { get; set; }

        public string parentID
        {
            get
            {
                return TemData.FatherCode;
            }
            set
            {
                TemData.FatherCode = value;
            }
        }

        public string resourceCode { get; set; }

        public string resourceDesc
        {
            get
            {
                return TemData.Description;
            }
            set
            {
                TemData.Description = value;
            }
        }

        public string resourceGrade { get; set; }

        public string resourceID
        {
            get
            {
                return TemData.Code;
            }
            set
            {
                TemData.Code = value;
            }
        }

        public string resourceName
        {
            get
            {
                return TemData.Name;
            }
            set
            {
                TemData.Name = value;
            }
        }

        public int resourceOrder
        {
            get
            {
                return TemData.Sort;
            }
            set
            {
                TemData.Sort = value;
            }
        }

        public string resourceType { get; set; }

        public MenuTM TemData { get; set; }

    }
    public class MenuTM
    {
        /// <summary>
        /// 菜单代码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 菜单类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 父级菜单代码
        /// </summary>
        public string FatherCode { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 连接action
        /// </summary>
        public string Href { get; set; }
    }
}
