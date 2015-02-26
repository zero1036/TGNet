using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TG.Example
{
    public class ExaminationClass
    {
        public static void function(int i1, int i2, bool bMie, int iIndex, int iTarget)
        {
            if (iIndex == iTarget)
                return;
            else
            {
                if(bMie)
                {
                    System.Diagnostics.Debug.WriteLine("{0}+{1}", i1, i2);
                    function(i1 + i2, i2 + 1, !bMie, iIndex + 1, iTarget);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("{0}-{1}", i1, i2);
                    function(i1 - i2, i2 + 1, !bMie, iIndex + 1, iTarget);
                }
            }

        }
    }
}
