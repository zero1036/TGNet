using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;
/*****************************************************
* 目的：PLinqExampleClass
* 创建人：林子聪
* 创建时间：20150227
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace TG.Example
{
    public class PLinqExampleClass
    {
        public void Main()
        {
            var dic = LoadData();

            Stopwatch watch = new Stopwatch();

            watch.Start();

            //串行执行
            var query1 = (from n in dic.Values
                          where n.Age > 20 && n.Age < 25
                          select n).ToList();

            watch.Stop();

            Console.WriteLine("串行计算耗费时间：{0}", watch.ElapsedMilliseconds);

            watch.Restart();

            var query2 = (from n in dic.Values.AsParallel()
                          where n.Age > 20 && n.Age < 25
                          select n).ToList();

            watch.Stop();

            Console.WriteLine("并行计算耗费时间：{0}", watch.ElapsedMilliseconds);

            Console.Read();
        }

        public ConcurrentDictionary<int, Student> LoadData()
        {
            ConcurrentDictionary<int, Student> dic = new ConcurrentDictionary<int, Student>();

            //预加载1500w条记录
            Parallel.For(0, 1500000, (i) =>
            {
                var single = new Student()
                {
                    ID = i,
                    Name = "hxc" + i,
                    Age = i % 151,
                    CreateTime = DateTime.Now.AddSeconds(i)
                };
                dic.TryAdd(i, single);
            });

            return dic;
        }

        public class Student
        {
            public int ID { get; set; }

            public string Name { get; set; }

            public int Age { get; set; }

            public DateTime CreateTime { get; set; }
        }
    }
}
