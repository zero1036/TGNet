using System;
/*****************************************************
* 目的：Assembly执行实体，用于缓存
* 创建人：林子聪
* 创建时间：20141216
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Utility.Tools
{
    public class AssemblyEn
    {
        public AssemblyEn(object ptarget, System.Reflection.MethodInfo pmethodInfo)
        {
            this.target = ptarget;
            this.methodInfo = pmethodInfo;
        }
        public object target { get; set; }
        public System.Reflection.MethodInfo methodInfo { get; set; }
    }
}
