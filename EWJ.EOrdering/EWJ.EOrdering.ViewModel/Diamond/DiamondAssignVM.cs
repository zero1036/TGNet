using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWJ.EOrdering.ViewModel.Diamond
{
    //调配集合
    public class DiamondAssignVM
    {
        public List<AssignItem> AssignList { get; set; }
    }

    //调配item
    public class AssignItem
    {
        public Guid DiamondID { get; set; }

        public Guid StoreID { get; set; }

        //public string StatusCode { get {return "002002"; }}

        public string PairID { get; set; }
    }

    //调配返回信息
    public class AssignResultVM : BaseVM
    {
        public IList<AssignResultItem> AssignResultList { get; set; }
    }

    //调配不成功的item
    public class AssignResultItem
    {
        public string DiamondNo { get; set; }

        public string StoreName { get; set; }
    }
}
