using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TG.Example
{
    public class SingleInstanceAndStatic
    {
        public void Go()
        {
            for (var i = 1; i <= 20; i++)
            {
                Task t = new Task(SingleCls.Singleton.Run);
                t.Start();
            }

            //for (var i = 1; i <= 20; i++)
            //{
            //    Task t = new Task(SingleCls.RunStatic);
            //    t.Start();
            //}
        }

    }

    public class SingleCls
    {
        public static SingleCls Singleton = new SingleCls();

        public void Run()
        {
            //int cot = 5;
            while (_cot > 0)
            {
                System.Diagnostics.Debug.WriteLine("Start:" + _cot);
                Thread.Sleep(1000);
                System.Diagnostics.Debug.WriteLine("End:" + _cot);
                _cot--;
            }
        }

        private int _cot = 5;

        //public static void RunStatic()
        //{
        //    //int cot = 5;
        //    while (_cot > 0)
        //    {
        //        System.Diagnostics.Debug.WriteLine("Start:" + _cot);
        //        Thread.Sleep(1000);
        //        System.Diagnostics.Debug.WriteLine("End:" + _cot);
        //        _cot--;
        //    }
        //}
        
    }
}
