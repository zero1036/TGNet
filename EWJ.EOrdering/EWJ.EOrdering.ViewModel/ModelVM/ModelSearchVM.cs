using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWJ.EOrdering.ViewModel.ModelVM
{
    public class ModelSearchVM:BaseVM
    {
        public string EWJNo { get; set; }
        public string ModelTypeCode { get; set; }
        public string CaiZhiCode { get; set; }
        public string ColorCode { get; set; }
        public string KelaWeightCode { get; set; }
        public string Localization { get; set; }
        public string OrderType { get; set; }
        public int BeginNo { get; set; }
        public int PageNo { get; set; }
    }
}
