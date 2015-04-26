using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWJ.EOrdering.ViewModel
{
    public class BaseScendingVM:BaseVM
    {
        public List<ScendingItem> ScendingList { get; set; }
    }

    public class ScendingItem
    {
        public ScendingType ScendingType { get; set; }

        public string ColumnName { get; set; }
    }

    public enum ScendingType
    {
        ASC,
        DESC
    }
}
