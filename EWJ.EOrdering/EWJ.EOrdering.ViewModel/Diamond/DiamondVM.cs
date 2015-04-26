using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWJ.EOrdering.ViewModel.Diamond
{
    //base diamonditem class
    public class BaseDiamondItem
    {
        public Guid DiamondID { get; set; }

        public string ArticleNo { get; set; }

        public string DiamondNo { get; set; }
      
        public string PairID { get; set; }

        public DateTime UploadDate { get; set; }
 
    }

    //查询条件
    public class DiamondQueryItem:BaseDiamondItem
    {
        public List<CaratRange> CaratRangeList { get; set; }

        public List<DiamondAttr> ClarityCodeList { get; set; }

        public List<DiamondAttr> ShapeCodeList { get; set; }

        public List<DiamondAttr> ColorCodeList { get; set; }

        public List<DiamondAttr> StoreIDList { get; set; }

        public string StatusCode { get; set; }
    }

    public class DiamondQueryVM:BaseScendingVM
    {
        public DiamondQueryItem DiamondQueryItem { get; set; }
    }


    //显示的每一条diamond数据
    public class DiamondItem:BaseDiamondItem
    {
        public decimal Carat { get; set; }

        public string ClarityName { get; set; }

        public string ShapeName { get; set; }

        public string ColorName { get; set; }

        public string StoreName { get; set; }

        public string StatusName {get; set; }
    }

    //diamond列表
    public class DiamondListResultVM:BaseVM
    {
        public IList<DiamondItem> DiamondList { get; set; }
    }

    public class DiamondAttr
    {
        public string attrVal { get; set; }
    }

    public class CaratRange
    {
        public decimal CaratMin { get; set; }

        public decimal CaratMax { get; set; }
    }


}
