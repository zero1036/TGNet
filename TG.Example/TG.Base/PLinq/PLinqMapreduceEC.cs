using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;
/*****************************************************
* 目的：PLinqMapreduceEC
* 创建人：林子聪
* 创建时间：20150227
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace TG.Example
{
    public class PLinqMapreduceEC
    {
        public void Main()
        {
            List<Student> list = new List<Student>()
            {
                new Student(){ ID=1, Name="jack", Age=20},       
                new Student(){ ID=1, Name="mary", Age=25},   
                new Student(){ ID=1, Name="joe", Age=29},       
                new Student(){ ID=1, Name="Aaron", Age=25},
            };


            list.GroupBy<Student, int>((stu) =>
                      {
                          return stu.Age;
                      });

            //这里我们会对age建立一组键值对
            var map = list.AsParallel().ToLookup(i => i.Age, count => 1);

            //化简统计
            var reduce = from IGrouping<int, int> singleMap
                         in map.AsParallel()
                         select new
                         {
                             Age = singleMap.Key,
                             Count = singleMap.Count()
                         };

            ///最后遍历
            reduce.ForAll(i =>
            {
                Console.WriteLine("当前Age={0}的人数有:{1}人", i.Age, i.Count);
            });
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
