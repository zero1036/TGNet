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
    public class OrgM
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pUsers"></param>
        /// <param name="pDepartments"></param>
        /// <param name="pDep2UserRel"></param>
        /// <param name="pTags"></param>
        public OrgM(List<UserVM> pUsers, List<DepartmentVM> pDepartments, List<Dep2UserRelTM> pDep2UserRel, List<TagBM> pTags)
        {
            this.Users = pUsers;
            this.Departments = pDepartments;
            this.Dep2UserRel = pDep2UserRel;
            this.Tags = pTags;
            InitDepartmentV();
        }
        /// <summary>
        /// 租户所有用户
        /// </summary>
        public List<UserVM> Users { get; set; }
        /// <summary>
        /// 租户所有部门
        /// </summary>
        public List<DepartmentVM> Departments { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Dep2UserRelTM> Dep2UserRel { get; set; }
        /// <summary>
        /// 租户所有标签
        /// </summary>
        public List<TagBM> Tags { get; set; }
        /// <summary>
        /// 初始化部门模型
        /// </summary>
        protected void InitDepartmentV()
        {
            if (!this.Users.IsNull() && !this.Departments.IsNull() && !this.Dep2UserRel.IsNull())
            {
                this.Departments.ForEach(d =>
                {
                    //通过部门-人员关系，获取部门关系集拥有的用户
                    IEnumerable<int> pRel = this.Dep2UserRel.Where(r => r.SysDepartmentId == d.SysDepartmentId).Select(r => r.SysUserId);
                    //设置到d的uses——首先BM强转VM，然后再where查询
                    d.SysUserIDs = this.Users.Where(u => pRel.Contains(u.SysUserId)).Select(u => u.SysUserId).ToList();

                    if (!d.SysUserIDs.IsNull())
                        //设置每个用户的所在部门名称及标签名称
                        d.SysUserIDs.ForEach(u =>
                        {
                            IEnumerable<int> pRel2 = this.Dep2UserRel.Where(r => r.SysUserId == u).Select(r => r.SysDepartmentId);

                            var pUser = this.Users.Single(us => us.SysUserId == u);
                            //设置用户关联部门
                            pUser.DepartmentsName = this.Departments.Where(ds => pRel2.Contains(ds.SysDepartmentId)).Select(dp => dp.Name).ToList();
                            if (!this.Tags.IsNull())
                                //设置用户关联标签
                                pUser.TagsName = this.Tags.Where(t => t.SysUserId == u).Select(t => t.TagName).ToList();
                        });
                });
            }
        }
        /// <summary>
        /// 获取部门上级关系树
        /// </summary>
        /// <param name="did"></param>
        /// <returns></returns>
        protected List<int> GetDepartmentTreeUp(int did)
        {
            var parentDid = this.Departments.Single(de => de.Did == did).ParentDid;
            if (parentDid != 0)
            {
                List<int> plist = new List<int>();
                plist.Add(parentDid);
                var pPids = GetDepartmentTreeUp(did);
                if (!pPids.IsNull()) plist.AddRange(pPids);
                return plist;
            }
            return null;
        }
        /// <summary>
        /// 获取部门下级关系树
        /// </summary>
        /// <param name="did"></param>
        /// <returns></returns>
        protected List<int> GetDepartmentTreeDown(int did)
        {
            var childDids = this.Departments.Where(d => d.ParentDid == did).Select(d => d.Did).ToList();
            if (!childDids.IsNull())
            {
                List<int> plist = new List<int>();
                childDids.ForEach((cid) =>
                {
                    plist.Add(cid);
                    var dd = GetDepartmentTreeDown(cid);
                    if (!dd.IsNull())
                        plist.AddRange(dd);
                });
                return plist;
            }
            return null;
        }
    }

    ///// <summary>
    ///// 组织机构视图模型——用于以部门为管理单位的视图，例如通讯录中的部门
    ///// </summary>
    //public class OrgVM4 : OrgM<UserVM, DepartmentVM, TagTM>
    //{
    //    private List<DepartmentVM> _DepartmentsV = null;
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public List<DepartmentVM> DepartmentsV
    //    {
    //        get
    //        {
    //            if (_DepartmentsV != null && _DepartmentsV.Count != 0)
    //                return _DepartmentsV;
    //            else
    //            {
    //                if (!base.Departments.IsNull() && !base.Users.IsNull() && !base.Dep2UserRel.IsNull())
    //                {
    //                    base.Departments.ForEach((d) =>
    //                    {
    //                        //通过部门-人员关系，获取部门拥有的用户
    //                        IEnumerable<int> pRel = base.Dep2UserRel.Where(r => r.SysDepartmentId == d.SysDepartmentId).Select(r => r.SysUserId);
    //                        //设置到d的uses
    //                        d.Users = base.Users.Where(u => pRel.Contains(u.SysUserId)).ToList();
    //                        //设置每个用户的所在部门名称及标签名称
    //                        d.Users.ForEach(u =>
    //                        {
    //                            //设置用户关联部门
    //                            var deps = base.Dep2UserRel.Where(r => r.SysUserId == u.SysUserId).Select(r => r.SysDepartmentId);
    //                            u.DepartmentsName = base.Departments.Where(dp => deps.Contains(dp.SysDepartmentId)).Select(dp => dp.Name).ToList();
    //                            if (!base.Tags.IsNull())
    //                                //设置用户关联标签
    //                                u.TagsName = base.Tags.Where(t => t.SysUserId == u.SysUserId).Select(t => t.TagName).ToList();
    //                        });
    //                    });
    //                    _DepartmentsV = base.Departments;
    //                }
    //                return _DepartmentsV;
    //            }
    //        }
    //    }
    //}
}
