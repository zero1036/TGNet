using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWJ.EOrdering.ViewModel.Sys
{
    public class PermissionVM
    {
        public string Code { get; set; }
    }

    public class MenuVM : BaseVM
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }

        public int Index { get; set; }

        public IList<MenuVM> ChildList { get; set; }
    }
}
