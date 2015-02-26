using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EG.Common.Entity
{
    [SerializableAttribute]
    public class PagingList
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public long Total { get; set; }

        public Object Datas {get;set;}
    }
}
