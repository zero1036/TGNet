using EWJ.EOrdering.ViewModel.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWJ.EOrdering.BL.Sys
{
    public class PermissionBL : BaseBL
    {
        public IList<MenuVM> GetMenu(Guid userId, string localization)
        {
            IList<MenuVM> list = new List<MenuVM>();

            MenuVM menu = new MenuVM()
            {
                Code = "001",
                Name = "发镶珠宝",
                Index = 1,
                Path = ""
            };

            IList<MenuVM> childList = new List<MenuVM>();
            MenuVM child1 = new MenuVM()
            {
                Code = "001001",
                Name = "新增货品",
                Index = 1,
                Path = "userList"
            };
            childList.Add(child1);

            MenuVM child2 = new MenuVM()
            {
                Code = "001002",
                Name = "修正货品资料",
                Index = 2,
                Path = "userList"
            };
            childList.Add(child2);
            menu.ChildList = childList;

            list.Add(menu);


            MenuVM menu2 = new MenuVM()
            {
                Code = "002",
                Name = "钻石",
                Index = 2,
                Path = ""
            };

            IList<MenuVM> childList2 = new List<MenuVM>();
            MenuVM child3 = new MenuVM()
            {
                Code = "002001",
                Name = "Diamond Query",
                Index = 1,
                Path = "/html/diamondQuery"
            };
            childList2.Add(child3);
            MenuVM child4 = new MenuVM()
            {
                Code = "002002",
                Name = "Diamond Assign",
                Index = 2,
                Path = "/html/diamondAssign"
            };
            childList2.Add(child4);
            MenuVM child5 = new MenuVM()
            {
                Code = "002003",
                Name = "Diamond Upload",
                Index = 2,
                Path = "/html/diamondUpload"
            };
            childList2.Add(child5);
            menu2.ChildList = childList2;

            list.Add(menu2);

            return list;
        }

        public IList<PermissionVM> GetPermission(Guid userID)
        {
            IList<PermissionVM> list = new List<PermissionVM>();
            PermissionVM model = new PermissionVM();
            list.Add(model);

            return null;
        }
    }
}
