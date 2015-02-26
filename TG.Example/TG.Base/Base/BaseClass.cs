using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TS.Base
{
    public class BaseClass
    {
        public static void function()
        {
            B b = new B();
            //B b = new A();  //错误哦
            A a = new B();
        }

        class A
        {
            public A()
            {
                PrintFields();
            }
            public virtual void PrintFields() { }

        }
        class B : A
        {
            int x = 1;
            int y;
            public B()
            {
                y = -1;
            }

            public override void PrintFields()
            {
                Console.WriteLine("x={0},y={1}", x, y);
            }
        }
    }
}
