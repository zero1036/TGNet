using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using System.Reflection;
using System.Diagnostics;
using EG.WeChat.Utility.Tools;

namespace TG.Example
{
    public class AutofacExampleClass
    {
        public void RegisterAssemblyAutofac()
        {
            var watch = new Stopwatch();
            watch.Start();

            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).
                Where(t => t.Name.EndsWith("SerCla2")).
                AsImplementedInterfaces();

            IContainer container = builder.Build();
            var repository = container.Resolve<IBaseSer>();
            repository.Say();

            watch.Stop();
            Console.WriteLine(string.Format("time:" + watch.ElapsedMilliseconds));
        }

        public void RegisterAssemblyReflection()
        {
            var watch = new Stopwatch();
            watch.Start();

            //var builder = new ContainerBuilder();
            //builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).
            //    Where(t => t.Name.EndsWith("SerCla2")).
            //    AsImplementedInterfaces();

            //IContainer container = builder.Build();
            //var repository = container.Resolve<IBaseSer>();
            //repository.Say();

            object target = EG.WeChat.Utility.Tools.AssemblyHelper.LoadObjectFromAssembly(@"D:\TGProject\Others\TGNet\TG.Example\TG.Example.View\bin\Debug\TG.Example.dll", "TG.Example.SerCla2");
            //执行对象加载函数
            Type typeEntity = target.GetType();
            //获取函数
            MethodInfo pMethodInfo = typeEntity.GetMethod("Say");
            AssemblyHelper.InvokeMethod(target, pMethodInfo, null);

            watch.Stop();
            Console.WriteLine(string.Format("time:" + watch.ElapsedMilliseconds));
        }
    }
    public interface IBaseSer
    {
        void Say();
    }
    public class SerCla1 : IBaseSer
    {
        public void Say()
        {
            System.Diagnostics.Debug.WriteLine("OK HEllo");
            //System.Threading.Thread.Sleep(500);
        }
    }
    public class SerCla2 : IBaseSer
    {
        public void Say()
        {
            System.Diagnostics.Debug.WriteLine("bbu");
            System.Threading.Thread.Sleep(10);
        }
        public void Say2(string Name)
        {
            int i = 2342 - 2213 + 235521 + 235 - 2345;
            double x = i / 234;
            //System.Diagnostics.Debug.WriteLine(Name);
            //System.Threading.Thread.Sleep(500);
        }
    }
}
