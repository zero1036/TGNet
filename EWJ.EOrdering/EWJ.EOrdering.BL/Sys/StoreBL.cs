using EWJ.EOrdering.ViewModel.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWJ.EOrdering.BL.Sys
{
    public class StoreBL
    {
        public IList<StoreVM> GetStoreList(Guid userID)
        {
            IList<StoreVM> list = new List<StoreVM>();
            StoreVM model = new StoreVM()
            {
                StoreID = Guid.NewGuid(),
                StoreMark = "A001",
                StoreCode = "B001",
                StoreName = "天河店",
                StoreArea = "GZ",
                Address1 = "Address11"
            };
            list.Add(model);

            return list;
        }
    }
}
