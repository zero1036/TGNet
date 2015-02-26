using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EG.Utility.AppCommon
{
   public class BussinessException:Exception
    {
       public  const int No_landing_permissions = 3000;
       public  const int More_than_one_authority = 3001;
        public long code { get; set; }
        public BussinessException():base(){}

        public BussinessException(string str) :
            base(str) { }
        public BussinessException(long _code, string str) :
            base(str) { code = _code; }
    }
}
