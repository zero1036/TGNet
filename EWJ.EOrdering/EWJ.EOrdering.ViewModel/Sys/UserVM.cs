using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace EWJ.EOrdering.ViewModel.Sys
{
    public class UserVM : BaseVM
    {
        public Guid? Id { get; set; }

        public string StaffCode { get; set; }

        public string Account { get; set; }

        public string Password { get; set; }

        public string CHName { get; set; }

        public string ENName { get; set; }

        public string Status { get; set; }

        public string StatusName { get; set; }

        public string Tel { get; set; }

        public string EMail { get; set; }

        public string Address { get; set; }

        //public string Store { get; set; }

        public Guid? StoreId { get; set; }

        public string StoreName { get; set; }

        public string UserType { get; set; }

        public bool? IsDelete { get; set; }

        public string Localization { get; set; }

        public Guid? ModifyUser { get; set; }

        public string ModifyUserName { get; set; }

        public DateTime? ModifyDate { get; set; }

        public Guid? CreatedUser { get; set; }

        public string CreatedUserName { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string Extend1 { get; set; }

        public string Extend2 { get; set; }
    }

    public class UserPostParamVM : BaseVM
    {
        public string Account { get; set; }

        public string Password { get; set; }
    }

    public class UserResultVM : BaseVM
    {
        public UserVM User { get; set; }

        public IList<UserVM> UserList { get; set; }

    }

    public class SitePrincipal : IPrincipal
    {
        private IIdentity _dentity = null;

        public SitePrincipal(IIdentity identity)
        {
            _dentity = identity;
        }

        public IIdentity Identity
        {
            get
            {
                return this._dentity;
            }
        }

        public bool IsInRole(string role)
        {
            throw new NotSupportedException("Not Supported");
        }
    }

    //用户
    public class SiteIdentity : IIdentity
    {
        public SiteIdentity(Guid id, string staffCode, string account, string name, string localization, Guid? storeId)
        {
            this.Id = id;
            this.Name = name;
            this.Account = account;
            this.StaffCode = staffCode;
            this.StoreId = storeId;
            this.Localization = localization;
        }

        #region Interface

        public string AuthenticationType
        {
            get
            {
                return "Form";
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                //return !string.IsNullOrEmpty(Name) ? true : false;
                return this.Id != new Guid() ? true : false;
            }
        }

        public string Name { get; set; }

        #endregion

        public Guid Id { get; set; }

        public string Account { get; set; }

        public string StaffCode { get; set; }

        public Guid? StoreId { get; set; }

        private string _localization = "014001";
        public string Localization
        {
            get
            {
                return _localization;
            }
            set
            {
                _localization = value;
            }
        }
    }
}
