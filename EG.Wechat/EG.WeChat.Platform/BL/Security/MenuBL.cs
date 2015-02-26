using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EG.WeChat.Model;
using EG.WeChat.Model.SecurityM;
using EG.WeChat.Platform.DA;

namespace EG.WeChat.Platform.BL
{
    public class MenuBL  
    {

        private MenuDA _menuDA;
        protected MenuDA MenuDA
        {
            get
            {
                if (_menuDA == null)
                {
                    _menuDA = new MenuDA();
                }
                return _menuDA;
            }
        }


        /// <summary>
        /// 获取菜单列表，至少包含模块的一个权限，才会显示对应菜单。
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public virtual List<T_Menu> GetMenu(List<AccessRight> right)
        {
            var Controllers = right.Select(z => z.Controller.ToLower()).Distinct(); ;

            if (Controllers.Count() > 0)
            {
                Hashtable ht = new Hashtable();
                ht.Add("State", 0);
                List<T_Menu> Menus = MenuDA.GetMenu(ht);
                return Menus.Where(z => Controllers.Any(y => string.IsNullOrEmpty(z.Controller) || y == z.Controller.ToLower())).ToList();
            }
            return new List<T_Menu>();
        }



    }


}
