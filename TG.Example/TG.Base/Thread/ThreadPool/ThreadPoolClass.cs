using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TG.Example
{
    public class ThreadPoolClass
    {
        public static void function()
        {
            Console.WriteLine("Begin in Main");

            ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadDo), 1);
            Thread.Sleep(300);
            ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadDo), 2);

            Thread.Sleep(300);
            Console.WriteLine("End in Main");

            Thread.Sleep(5000);
        }

        static void ThreadDo(Object o)
        {
            for (int i = 0; i < 5; i++)
            {
                //Thread.CurrentThread.Name
                Console.WriteLine("[Thread " + (int)o + "] Execute in ThreadDo");
                Thread.Sleep(100);
            }
        }
    }
}
