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
    public class PLinqExampleClass2
    {
        public void Main()
        {
            ConcurrentBag<int> bag = new ConcurrentBag<int>();

            var list = ParallelEnumerable.Range(0, 10000);

            list.ForAll((i) =>
            {
                bag.Add(i);
            });

            Console.WriteLine("bag集合中元素个数有:{0}", bag.Count);

            Console.WriteLine("list集合中元素个数总和为:{0}", list.Sum());

            Console.WriteLine("list集合中元素最大值为:{0}", list.Max());

            Console.WriteLine("list集合中元素第一个元素为:{0}", list.FirstOrDefault());

            Console.Read();
        }
    }
}
