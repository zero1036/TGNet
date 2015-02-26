using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EG.WeChat.Utility.WeiXin.ResponseChain
{

    /// <summary>
    /// DealingHandler附加行为
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class DealingHandlerAdditionalBehaviorAttribute : System.Attribute
    {
        /// <summary>
        /// 附加行为(只读)
        /// </summary>
        public DealingHandlerAdditionalBehaviorType AdditionalBehavior { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="limitedDataTypes">要限制的数据类型</param>
        public DealingHandlerAdditionalBehaviorAttribute(DealingHandlerAdditionalBehaviorType additionalBehavior)
        {
            this.AdditionalBehavior = additionalBehavior;
        }
    }
}