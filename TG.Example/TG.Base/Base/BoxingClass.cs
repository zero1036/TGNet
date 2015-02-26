using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TS.Base
{
    public class BoxingClass
    {
        public static void function_DataType()
        {
            try
            {
                int i1 = 10 / 3;      //编译通过，并可以运行，计算结果float自动转换为int32，并输出3
                System.Diagnostics.Debug.WriteLine(i1);

                //int i2 = 10 / 0;    //编译不通过
                //System.Diagnostics.Debug.WriteLine(i2);

                int i3 = 0 / 10;      //编译通过，并可以运行
                System.Diagnostics.Debug.WriteLine(i3);


                short s1 = 4323;

                int i4 = s1;            //编译通过，向高精度宽度转换
                //short s2 = i4;          //编译不通过，向低精度窄度转换
                i4 = 1474836;
                short s3 = (short)i4;   //编译通过，强制转换即可，如果超出精度则缩减精度
                System.Diagnostics.Debug.WriteLine(s3);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        public static void function_NoBoxing()
        {
            int iValue = 19;
            int iStartTime = DateTime.Now.Minute * 1000 * 60 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;


            for (int i = 0; i <= 10000; i++)
            {
                Console.WriteLine("i={0}", iValue);
            }

            int iEndTime = DateTime.Now.Minute * 1000 * 60 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
            System.Diagnostics.Debug.WriteLine("无装箱时间{0}", (iEndTime - iStartTime));

        }

        public static void function_Boxing()
        {
            int iValue = 19;
            object objValue = iValue;
            int iStartTime = DateTime.Now.Minute * 1000 * 60 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;

            for (int i = 0; i <= 10000; i++)
            {
                Console.WriteLine("i={0}", objValue);
            }

            int iEndTime = DateTime.Now.Minute * 1000 * 60 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
            System.Diagnostics.Debug.WriteLine("有装箱时间{0}", (iEndTime - iStartTime));

        }


    }

}
