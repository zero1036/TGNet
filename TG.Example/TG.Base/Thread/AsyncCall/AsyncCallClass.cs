using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TS.Base
{
    public class AsyncCallClass
    {
        public delegate double CarculateMethod(int i, int j);
        static CarculateMethod m_CarculateMethod;

        public static void AsyncCallFunction()
        {
            m_CarculateMethod = new CarculateMethod(Carculate);

            for (int i = 1; i <= 100; i++)
            {
                Thread.Sleep(1000);
                m_CarculateMethod.BeginInvoke(i, i + 2, new AsyncCallback(TaskFinished), null);
            }
        }

        public static double Carculate(int i, int j)
        {
            Console.WriteLine(string.Format("{0}+{1}=", i, j));
            return i + j;
        }
        ///<summary>
        ///线程完成之后回调的函数
        ///<summary>
        public static void TaskFinished(IAsyncResult result)
        {
            double db = m_CarculateMethod.EndInvoke(result);
            Console.WriteLine(string.Format("{0}", db));
        }
    }
}
