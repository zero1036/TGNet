using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EG.Common.Entity
{
    [SerializableAttribute]
    public class BaseSO
    {
        public readonly static int PAGE_INDEX_NO_PAGE = -1;
        public readonly static int PAGE_INDEX_NO_TOTAL = -1;
        
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public long Total { get; set; }
    }
}
