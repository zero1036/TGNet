using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TS.Base
{
    public class DelegateClass2
    {
        public delegate void sayHelloDelegate(string strSaySomething);

        public static void function()
        {
            //写法1
            sayHelloDelegate s1 = new sayHelloDelegate(SayInChinese);
            sayHelloDelegate s2 = new sayHelloDelegate(SayInEnglish);
            ManyGuysTalking("我系T哥", s1);
            ManyGuysTalking("i m Tgor", s2);

            //写法2
            ManyGuysTalking("我系T哥", SayInChinese);
            ManyGuysTalking("i m Tgor", SayInEnglish);

            //写法3——一个委托可以放多个事件，并且可以同时执行
            //此处输出：中文:我系T哥    英文:我系T哥
            sayHelloDelegate s3 = new sayHelloDelegate(SayInChinese);
            s3 += SayInEnglish;
            ManyGuysTalking("我系T哥", s3);
        }

        public static void ManyGuysTalking(string str, sayHelloDelegate pSayHelloDelegate)
        {
            pSayHelloDelegate(str);
        }

        public static void SayInChinese(string str)
        {
            Console.WriteLine(string.Format("中文:{0}", str));
        }

        public static void SayInEnglish(string str)
        {
            Console.WriteLine(string.Format("英文:{0}", str));
        }

    }
}
