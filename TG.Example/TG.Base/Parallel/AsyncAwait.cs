using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TG.Example
{
    public class AsyncAwait
    {
        /// <summary>
        /// 主执行函数
        /// </summary>
        public void GoFunc()
        {
            //Fn1();
            //Console.WriteLine("主函数结束");

            var res = Fn2();
            Console.WriteLine("主函数结束，结果：" + res.Result);
        }

        /// <summary>
        /// 调试目标：await挂起任务后，async方法内，await以后所有代码的会被包装成委托，等待挂起任务完成后才执行
        /// 
        /// 结果：
        /// 主函数结束
        /// 任务执行中
        /// 任务挂起完成
        /// </summary>
        private async void Fn1()
        {
            await Task.Run(() =>
            {
                Task.Delay(1000);
                Console.WriteLine("任务执行中");
            });

            Console.WriteLine("任务挂起完成");

        }

        /// <summary>
        /// 调试目标：与Fn1不同，Fn2返回内部任务的Task<Result>，并且
        /// 在主执行函数中，调用了Task<Result>结果——错误做法，任务会一直阻塞在“任务执行中”。
        /// 
        /// 解析：GoFunc要等待Fn2的结果99，并打印，那实际起始是同步，异步执行毫无意义；
        /// 假若确实要等待Fn2结果99，就要等待await Fn2()，导致GoFunc的打印方法都会异步在Fn2后执行
        /// </summary>
        /// <remarks>
        /// 结果：
        /// 任务执行中
        /// ......(阻塞)
        /// </remarks>
        private async Task<int> Fn2()
        {
            int res = await Task<int>.Run<int>(() =>
            {
                Task.Delay(1000);
                Console.WriteLine("任务执行中");
                return 99;
            });

            Console.WriteLine("任务挂起完成");

            return res;
        }



    }
}
