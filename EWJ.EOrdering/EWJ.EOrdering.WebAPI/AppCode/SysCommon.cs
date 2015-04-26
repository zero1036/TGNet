using EWJ.EOrdering.Common;
using EWJ.EOrdering.ViewModel.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EWJ.EOrdering.WebAPI.AppCode
{
    public class SysCommon
    {
        public static SiteIdentity CurrentUser
        {
            get
            {
                //SiteIdentity identity = HttpContext.Current.User.Identity as SiteIdentity;
                SiteIdentity identity = null;
                if (HttpContext.Current.Session[SysHelper.SESSION_KEY_CURRENT_USER] != null)
                {
                    identity = HttpContext.Current.Session[SysHelper.SESSION_KEY_CURRENT_USER] as SiteIdentity;
                }
                else
                {
                    //identity = HttpContext.Current.User.Identity as SiteIdentity;
                }
                return identity;
            }
            set
            {
                //SitePrincipal principal = new SitePrincipal(value);
                //Thread.CurrentPrincipal = principal;
                //HttpContext.Current.User = principal;

                HttpContext.Current.Session[SysHelper.SESSION_KEY_CURRENT_USER] = value;
            }
        }
    }
}