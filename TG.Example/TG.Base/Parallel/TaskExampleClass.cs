using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace TS.Base
{
    /// <summary>
    /// 
    /// </summary>
    public class TaskExampleClass
    {
        #region 公有
        /// <summary>
        /// 非异步函数——有异步——没有await则没有async
        /// </summary>
        public void Fun1()
        {
            //正确调用
            TaskFunA();
            //以下方式报错，TaskFunA为异步方法，如果要获取TaskFunA返回值，
            //不能直接调用，要加入await关键字，获取Task<TResult>，参考Fun2()
            //double xx = TaskFunA();

            WriteLog(1, null);
        }
        /// <summary>
        /// 异步函数——有异步——有await有async
        /// </summary>
        public async void Fun2()
        {
            double xx = await TaskFunA();

            WriteLog(1, xx);
        }
        #endregion

        #region Task
        /// <summary>
        /// Task.Run()静态函数run
        /// </summary>
        /// <returns></returns>
        public async Task<double> TaskFunA()
        {
            double db = 1;
            return await Task.Run(() =>
            {
                Thread.Sleep(6000);
                db = 50;
                WriteLog(2, db);
                return db;
            });
        }
        /// <summary>
        /// Task实例:ContinueWith、Start
        /// </summary>
        /// <returns></returns>
        public void TaskFunB()
        {
            Task task = new Task(
                () =>
                {
                    Thread.Sleep(6000);
                    WriteLog(3, "Tasking");
                });
            //在使用能够Task类的Wait方法等待一个任务时或派生类的Result属性获得任务执行结果都有可能阻塞线程，
            //为了解决这个问题可以使用ContinueWith方法，他能在一个任务完成时自动启动一个新的任务来处理执行结果。
            task.ContinueWith(t =>
            {
                WriteLog(4, "ContinueWith");
            });
            task.Start();

            WriteLog(5, "主线程执行完毕");
        }
        /// <summary>
        /// Task实例：wait()
        /// Task开始以后wait的话，会线程会阻塞
        /// </summary>
        /// <returns></returns>
        public void TaskFunC()
        {
            Task task = new Task(
                () =>
                {
                    Thread.Sleep(3000);
                    WriteLog(3, "Tasking");
                });

            //在使用能够Task类的Wait方法等待一个任务时或派生类的Result属性获得任务执行结果都有可能阻塞线程，
            //为了解决这个问题可以使用ContinueWith方法，他能在一个任务完成时自动启动一个新的任务来处理执行结果。
            task.ContinueWith(t =>
            {
                WriteLog(4, "ContinueWith");
            });
            //Task开始以后wait的话，会线程会阻塞
            task.Start();
            task.Wait();

            WriteLog(5, "主线程执行完毕");
        }

        #endregion

        #region Task.Factory
        /// <summary>
        /// Task实例：Factory
        /// </summary>
        /// <returns></returns>
        public void TaskFunD()
        {
            List<int> pList = new List<int>();
            Random pRam = new Random();
            Func<int> pFunc = () =>
            {
                for (int y = 1; y <= 10; y++)
                {
                    int pi = pRam.Next(1, 10);
                    if (pList.Contains(pi))
                    {
                        WriteLog(6, "repeat:" + pi);
                    }
                    else
                    {
                        pList.Add(pi);
                        WriteLog(7, "add:" + pi);
                    }
                }
                return 1;
            };

            var t1 = Task<int>.Factory.StartNew(pFunc);
            var t2 = Task<int>.Factory.StartNew(pFunc);
            var t3 = Task<int>.Factory.StartNew(pFunc);

            Task[] pts = { t1, t2, t3 };

            Task.Factory.ContinueWhenAll(pts, (tasks) =>
            {
                foreach (Task t in tasks)
                {
                    if (!t.IsCompleted)
                    { }
                }

                pList.ForEach((ii) =>
                {
                    WriteLog(8, "each:" + ii);
                });
            });


            WriteLog(9, "主线程执行完毕");
        }
        /// <summary>
        /// Task vs Task.Factory状态对比
        /// </summary>
        public void TaskFunE()
        {
            Action pac1 = () =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("\n我是任务1");
            };
            Action pac2 = () =>
            {
                Thread.Sleep(2000);
                Console.WriteLine("我是任务2");
            };

            //第一种方式开启
            var task1 = new Task(pac1);

            //第二种方式开启
            var task2 = Task.Factory.StartNew(pac2);

            Console.WriteLine("调用start之前****************************\n");

            //调用start之前的“任务状态”
            Console.WriteLine("task1的状态:{0}", task1.Status);

            Console.WriteLine("task2的状态:{0}", task2.Status);

            task1.Start();

            Console.WriteLine("\n调用start之后****************************");

            //调用start之前的“任务状态”
            Console.WriteLine("\ntask1的状态:{0}", task1.Status);

            Console.WriteLine("task2的状态:{0}", task2.Status);

            //主线程等待任务执行完
            Task.WaitAll(task1, task2);

            Console.WriteLine("\n任务执行完后的状态****************************");

            //调用start之前的“任务状态”
            Console.WriteLine("\ntask1的状态:{0}", task1.Status);

            Console.WriteLine("task2的状态:{0}", task2.Status);

            Console.Read();
        }
        #endregion

        #region Tack cancel
        /// <summary>
        /// Task cancel
        /// </summary>
        public void TaskFunF()
        {
            var cts = new CancellationTokenSource();
            var ct = cts.Token;

            Task task1 = new Task(() =>
            {
                ct.ThrowIfCancellationRequested();

                Console.WriteLine("我是任务1");

                Thread.Sleep(2000);

                ct.ThrowIfCancellationRequested();

                Console.WriteLine("我是任务1的第二部分信息");
            }, ct);

            Task task2 = new Task(() => { Console.WriteLine("我是任务2"); });

            try
            {
                task1.Start();
                task2.Start();

                Thread.Sleep(1000);

                cts.Cancel();

                Task.WaitAll(task1, task2);
            }
            catch (AggregateException ex)
            {
                foreach (var e in ex.InnerExceptions)
                {
                    Console.WriteLine("\nhi,我是OperationCanceledException：{0}\n", e.Message);
                }

                //task1是否取消
                Console.WriteLine("task1是不是被取消了？ {0}", task1.IsCanceled);
                Console.WriteLine("task2是不是被取消了？ {0}", task2.IsCanceled);
            }

            Console.Read();
        }
        #endregion

        #region Task Delay与Thread Sleep
        // The following method runs asynchronously. The UI thread is not
        // blocked during the delay. You can move or resize the Form1 window 
        // while Task.Delay is running.
        public async Task<string> WaitAsynchronouslyAsync()
        {
            await Task.Delay(1000);
            return "Finished";
        }
        // The following method runs synchronously, despite the use of async.
        // You cannot move or resize the Form1 window while Thread.Sleep
        // is running because the UI thread is blocked.
        public async Task<string> WaitSynchronously()
        {
            // Add a using directive for System.Threading.
            Thread.Sleep(1000);
            return "Finished";
        }
        #endregion

        #region Parallel实例
        /// <summary>
        /// 单线For
        /// </summary>
        public void For_Local_Test()
        {
            int[] nums = Enumerable.Range(0, 10000000).ToArray<int>();
            long total = 0;
            foreach (int i in nums)
            {
                total += i;
                string str = total.ToString();
                total = Convert.ToInt64(str);
            }
        }
        /// <summary>
        /// 分区For
        /// </summary>
        public void Parallel_For_Local_Test()
        {
            int[] nums = Enumerable.Range(0, 10000000).ToArray<int>();
            long total = 0;
            ParallelLoopResult result = Parallel.For<long>(0, nums.Length,
                 () => { return 0; },
                 (j, loop, subtotal) =>
                 {
                     // 延长任务时间，更方便观察下面得出的结论
                     //Thread.SpinWait(200);
                     //System.Diagnostics.Debug.WriteLine(string.Format("当前线程ID为：{0},j为{1}，subtotal为：{2}。"
                     //    , Thread.CurrentThread.ManagedThreadId, j.ToString(), subtotal.ToString()));
                     if (j == 23)
                         loop.Break();
                     if (j > loop.LowestBreakIteration)
                     {
                         //Thread.Sleep(4000);
                         //System.Diagnostics.Debug.WriteLine(string.Format("j为{0},等待4s种，用于判断已开启且大于阻断迭代是否会运行完。", j.ToString()));
                     }
                     //System.Diagnostics.Debug.WriteLine(string.Format("j为{0},LowestBreakIteration为：{1}", j.ToString(), loop.LowestBreakIteration));
                     subtotal += nums[j];
                     string str = subtotal.ToString();
                     subtotal = Convert.ToInt64(str);
                     return subtotal;
                 },
                 (finalResult) => Interlocked.Add(ref total, finalResult)
            );
            //System.Diagnostics.Debug.WriteLine(string.Format("total值为：{0}", total.ToString()));
            //if (result.IsCompleted)
            //    System.Diagnostics.Debug.WriteLine("循环执行完毕");
            //else
            //    System.Diagnostics.Debug.WriteLine(string.Format("{0}"
            //        , result.LowestBreakIteration.HasValue ? "调用了Break()阻断循环." : "调用了Stop()终止循环."));
        }
        /// <summary>
        /// 分区For
        /// </summary>
        public void Parallel_For_Local_Test2()
        {
            int[] nums = Enumerable.Range(0, 1000000).ToArray<int>();
            long total = 0;
            ParallelLoopResult result = Parallel.For<long>(0, nums.Length,
                 () => { return 0; },
                 (j, loop, subtotal) =>
                 {
                     // 延长任务时间，更方便观察下面得出的结论
                     Thread.SpinWait(200);
                     Console.WriteLine("当前线程ID为：{0},j为{1}，subtotal为：{2}。"
                         , Thread.CurrentThread.ManagedThreadId, j.ToString(), subtotal.ToString());
                     if (j == 23)
                         loop.Break();
                     if (j > loop.LowestBreakIteration)
                     {
                         Thread.Sleep(4000);
                         Console.WriteLine("j为{0},等待4s种，用于判断已开启且大于阻断迭代是否会运行完。", j.ToString());
                     }
                     Console.WriteLine("j为{0},LowestBreakIteration为：{1}", j.ToString(), loop.LowestBreakIteration);
                     subtotal += nums[j];
                     return subtotal;
                 },
                 (finalResult) => Interlocked.Add(ref total, finalResult)
            );
            Console.WriteLine("total值为：{0}", total.ToString());
            if (result.IsCompleted)
                Console.WriteLine("循环执行完毕");
            else
                Console.WriteLine("{0}"
                    , result.LowestBreakIteration.HasValue ? "调用了Break()阻断循环." : "调用了Stop()终止循环.");
        }
        #endregion

        #region 辅助
        /// <summary>
        /// 写入
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="objValue"></param>
        private void WriteLog(int actionId, object objValue)
        {
            string str = string.Format("action {0} ;thread {1}; value {2};time {3} ", actionId, Thread.CurrentThread.ManagedThreadId, objValue, DateTime.Now);
            System.Diagnostics.Debug.WriteLine(str);
        }
        #endregion
    }
}
