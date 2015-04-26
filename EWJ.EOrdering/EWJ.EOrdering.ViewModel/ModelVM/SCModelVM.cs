using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWJ.EOrdering.ViewModel.ModelVM
{
    public class SCModelVM:BaseVM
    {
        public string ModelID { get; set; }
        public string EWJNo { get; set; }
        public string ModelTypeCode { get; set; }
        public string ModelTypeString { get; set; }
        public List<ModelOptionVM> Caizhi { get; set; }
        public List<ModelOptionVM> Color { get; set; }
        public List<ModelOptionVM> Kela { get; set; }
        public string Leixiao { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string ImagePath { get; set; }
    }
}
