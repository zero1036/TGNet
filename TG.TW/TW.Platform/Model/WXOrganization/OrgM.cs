using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TW.Platform.Sys;
/*****************************************************
* 目的：组织机构模型
* 创建人：林子聪
* 创建时间：20150427
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace TW.Platform.Model
{
    /// <summary>
    /// 组织机构模型
    /// </summary>
    /// <typeparam name="User"></typeparam>
    /// <typeparam name="Department"></typeparam>
    /// <typeparam name="Tag"></typeparam>
    public class OrgM<User, Department, Tag>
        where User : UserBM
        where Department : DepartmentBM
        where Tag : TagTM
    {
        /// <summary>
        /// 租户所有用户
        /// </summary>
        public List<User> Users { get; set; }
        /// <summary>
        /// 租户所有部门
        /// </summary>
        public List<Department> Departments { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Dep2UserRelTM> Dep2UserRel { get; set; }
        /// <summary>
        /// 租户所有标签
        /// </summary>
        public List<Tag> Tags { get; set; }
    }
    /// <summary>
    /// 组织机构视图模型——用于以部门为管理单位的视图，例如通讯录中的部门
    /// </summary>
    public class OrgVM4 : OrgM<UserVM, DepartmentVM, TagTM>
    {
        private List<DepartmentVM> _DepartmentsV = null;
        /// <summary>
        /// 
        /// </summary>
        public List<DepartmentVM> DepartmentsV
        {
            get
            {
                if (_DepartmentsV != null && _DepartmentsV.Count != 0)
                    return _DepartmentsV;
                else
                {
                    if (!base.Departments.IsNull() && !base.Users.IsNull() && !base.Dep2UserRel.IsNull())
                    {
                        base.Departments.ForEach((d) =>
                        {
                            //通过部门-人员关系，获取部门拥有的用户
                            IEnumerable<int> pRel = base.Dep2UserRel.Where(r => r.SysDepartmentId == d.SysDepartmentId).Select(r => r.SysUserId);
                            //设置到d的uses
                            d.Users = base.Users.Where(u => pRel.Contains(u.SysUserId)).ToList();
                            //设置每个用户的所在部门名称及标签名称
                            d.Users.ForEach(u =>
                            {
                                //设置用户关联部门
                                var deps = base.Dep2UserRel.Where(r => r.SysUserId == u.SysUserId).Select(r => r.SysDepartmentId);
                                u.DepartmentsName = base.Departments.Where(dp => deps.Contains(dp.SysDepartmentId)).Select(dp => dp.Name).ToList();
                                if (!base.Tags.IsNull())
                                    //设置用户关联标签
                                    u.TagsName = base.Tags.Where(t => t.SysUserId == u.SysUserId).Select(t => t.TagName).ToList();
                            });
                        });
                        _DepartmentsV = base.Departments;
                    }
                    return _DepartmentsV;
                }
            }
        }
    }
}
