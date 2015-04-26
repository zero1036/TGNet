using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWJ.EOrdering.ViewModel.Sys
{
    public class DictionaryVM : BaseVM
    {
        public string DicCode { get; set; }

        public string DicName { get; set; }
    }

    public class DictionaryModel : BaseVM
    {
        public string DicCode { get; set; }

        public string EN_Name { get; set; }

        public string CHS_Name { get; set; }

        public string CHT_Name { get; set; }

        public string ParentCode { get; set; }

        public string Remark { get; set; }

        public int? Index { get; set; }

        public bool? IsDisable { get; set; }

        public Guid? ModifyUser { get; set; }

        public DateTime? ModifyDate { get; set; }

        public Guid? CreatedUser { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string Extend1 { get; set; }

        public string Extend2 { get; set; }

        public string Extend3 { get; set; }

        public string Extend4 { get; set; }
    }

    public class DictonaryParamVM : BaseVM
    {
        public string DicCode { get; set; }

        public string ParentCode { get; set; }

        public DictionaryModel Model { get; set; }
    }
}
