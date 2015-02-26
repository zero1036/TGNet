using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
/*****************************************************
* 目的：AspectF AOP类
* 创建人：林子聪
* 创建时间：20141204
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Utility.Tools
{
    /// <summary>
    ///AspectF 的摘要说明
    /// </summary>
    public class AspectF
    {
        internal Action<Action> Chain = null;

        internal Delegate WorkDelegate;

        [DebuggerStepThrough]
        public void Do(Action work)
        {
            if (this.Chain == null)
            {
                work();
            }
            else
            {
                this.Chain(work);
            }
        }

        [DebuggerStepThrough]
        public TReturnType Return<TReturnType>(Func<TReturnType> work)
        {
            this.WorkDelegate = work;

            if (this.Chain == null)
            {
                return work();
            }
            else
            {
                TReturnType returnValue = default(TReturnType);
                this.Chain(() =>
                {
                    Func<TReturnType> workDelegate = WorkDelegate as Func<TReturnType>;
                    returnValue = workDelegate();
                });
                return returnValue;
            }
        }

        public static AspectF Define
        {
            [DebuggerStepThrough]
            get
            {
                return new AspectF();
            }
        }

        [DebuggerStepThrough]
        public AspectF Combine(Action<Action> newAspectDelegate)
        {
            if (this.Chain == null)
            {
                this.Chain = newAspectDelegate;
            }
            else
            {
                Action<Action> existingChain = this.Chain;
                Action<Action> callAnother = (work) =>
                    existingChain(() => newAspectDelegate(work));
                this.Chain = callAnother;
            }

            return this;
        }

        public static void Retry(int retryDuration, int retryCount, Action<Exception> errorHandler, Action retryFaild, Action work)
        {
            do
            {
                try
                {
                    work();
                }
                catch (Exception ex)
                {
                    errorHandler(ex);
                    System.Threading.Thread.Sleep(retryDuration);
                    throw;
                }
            } while (retryCount-- > 0);

        }
    }
    /// <summary>
    /// 扩展
    /// </summary>
    public static class AspectExtensions
    {
        [DebuggerStepThrough]
        public static void DoNothing()
        {

        }

        [DebuggerStepThrough]
        public static void DoNothing(params object[] whatever)
        {

        }

        [DebuggerStepThrough]
        public static AspectF Delay(this AspectF aspect, int milliseconds)
        {
            return aspect.Combine((work) =>
            {
                System.Threading.Thread.Sleep(milliseconds);
                work();
            });
        }

        [DebuggerStepThrough]
        public static AspectF MustBeNonNull(this AspectF aspect, params object[] args)
        {
            return aspect.Combine((work) =>
            {
                for (int i = 0; i < args.Length; i++)
                {
                    object arg = args[i];
                    if (arg == null)
                    {
                        throw new ArgumentException(string.Format("Parameter at index {0} is null", i));
                    }
                }
                work();
            });
        }

        public static AspectF MustBeNonDefault<T>(this AspectF aspect, params T[] args) where T : IComparable
        {
            return aspect.Combine((work) =>
            {
                T defaultvalue = default(T);
                for (int i = 0; i < args.Length; i++)
                {
                    T arg = args[i];
                    if (arg == null || arg.Equals(defaultvalue))
                    {
                        throw new ArgumentException(string.Format("Parameter at index {0} is null", i));
                    }
                }
                work();
            });
        }

        public static AspectF WhenTrue(this AspectF aspect, params Func<bool>[] conditions)
        {
            return aspect.Combine((work) =>
            {
                foreach (Func<bool> condition in conditions)
                {
                    if (!condition())
                    {
                        return;
                    }
                }
                work();
            });
        }

        [DebuggerStepThrough]
        public static AspectF RunAsync(this AspectF aspect, Action completeCallback)
        {
            return aspect.Combine((work) => work.BeginInvoke(asyncresult =>
            {
                work.EndInvoke(asyncresult); completeCallback();
            }, null));
        }

        [DebuggerStepThrough]
        public static AspectF RunAsync(this AspectF aspect)
        {
            return aspect.Combine((work) => work.BeginInvoke(asyncresult =>
            {
                work.EndInvoke(asyncresult);
            }, null));
        }
    }
}
