using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using Senparc.Weixin.Context;
using Senparc.Weixin.MP.Entities;
using System.Collections;

namespace EG.WeChat.Utility.WeiXin
{
    public class CustomMessageContext : MessageContext<IRequestMessageBase,IResponseMessageBase>
    {

        #region 提供类似原Session变量的存取支持

        /* 支持类似于 Web原Session变量 的使用。 
         * 存储， ContextDatas["Code"] = "123";
         * 读取， return ContextDatas["Code"] ;
         * */

        /// <summary>
        /// 数据存储类<para />
        /// (使用方式类似于原Session变量；当该上下文过期移除时，对应的数据将被清空。)
        /// </summary>
        internal readonly ContextDataClass ContextDatas = new ContextDataClass();

        /// <summary>
        /// 上下文的数据存储类(线程安全)
        /// </summary>
        internal class ContextDataClass
        {
            /// <summary>
            /// 数据集合
            /// </summary>
            Dictionary<string, object> Datas = new Dictionary<string, object>();

            /// <summary>
            /// 索引
            /// </summary>
            /// <param name="index">索引值</param>
            /// <returns>存储的对象</returns>
            public object this[string index]
            {
                set 
                {
                    //参数检查
                    if (String.IsNullOrEmpty(index))
                        throw new ArgumentNullException("index","index不允许为空");

                    //按照类型进行处理(引用类型，使用WeakReference，断开跟GC的引用关系；外部不主动清空该数值时，不会造成内存泄露)
                    object targetValue;
                    if (value == null)
                    {
                        targetValue = null;
                    }
                    else if (value.GetType().IsValueType == false)
                    {
                        targetValue = new WeakReference(value);
                    }
                    else
                    {
                        targetValue = value;
                    }

                    //存储
                    lock (((ICollection)Datas).SyncRoot)
                        Datas[index] = targetValue;
                }

                get 
                {
                    //参数检查
                    if (String.IsNullOrEmpty(index))
                        throw new ArgumentNullException("index", "index不允许为空");

                    //读取
                    object Value = null;
                    lock (((ICollection)Datas).SyncRoot)
                        Datas.TryGetValue(index, out Value);

                    //按照类型进行处理
                    object targetValue;
                    if (Value == null)
                    {
                        targetValue = null;
                    }
                    else if (Value is WeakReference)
                    {
                        WeakReference wr    = (WeakReference)Value;
                        targetValue         = wr.IsAlive ? wr.Target : null;
                    }
                    else
                    {
                        targetValue = Value;
                    }

                    return targetValue;
                }                
            }
        }

        #endregion

        #region 上下文ID

        /* 本类的UserName(String)，其实也可以作为Context的对象标示。
         * 不过从比较效率来看，从UserName往后的数据来看，
         * 使用Guid重新作为标记，效果会更佳。
         */

        /// <summary>
        /// 上下文的标示ID
        /// </summary>
        internal Guid ContextId = Guid.NewGuid();

        #endregion


        //-----------------------

        #region 代码备份，目前未有使用的需求  -- SDK默认提供的事件处理

        public CustomMessageContext()
        {
            //base.MessageContextRemoved += CustomMessageContext_MessageContextRemoved;
        }

        void CustomMessageContext_MessageContextRemoved(object sender, Senparc.Weixin.Context.WeixinContextRemovedEventArgs<IRequestMessageBase,IResponseMessageBase> e)
        {
            /* 注意，这个事件不是实时触发的（当然你也可以专门写一个线程监控）
             * 为了提高效率，根据WeixinContext中的算法，这里的过期消息会在过期后下一条请求执行之前被清除
             */

            var messageContext = e.MessageContext as CustomMessageContext;
            if (messageContext == null)
            {
                return;//如果是正常的调用，messageContext不会为null
            }

            //TODO:这里根据需要执行消息过期时候的逻辑，下面的代码仅供参考

            //Log.InfoFormat("{0}的消息上下文已过期",e.OpenId);
            //api.SendMessage(e.OpenId, "由于长时间未搭理客服，您的客服状态已退出！");
        }

        #endregion
    }
}
