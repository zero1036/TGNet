using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TW.Platform.DA;
using TW.Platform.Model;
using EG.WeChat.Utility.Tools;
using TW.Platform.Sys;
/*****************************************************
* 目的：组织机构业务逻辑处理
* 创建人：林子聪
* 创建时间：20140427
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace TW.Platform.BL
{
    public class OrgBL
    {
        private CustomCacheHelper _Cache = new CustomCacheHelper();

        #region
        /// <summary>
        /// 获取部门
        /// </summary>
        /// <returns></returns>
        public object GetDepsFromOrgM()
        {
            var orgM = GetOrgM();
            return orgM.Departments;
        }
        /// <summary>
        /// 通过部门获取部门拥有的用户
        /// </summary>
        /// <returns></returns>
        public List<UserVM> GetUsersByDepId(int depId)
        {
            var orgM = GetOrgM();
            var dep = orgM.Departments.Single(d => d.Did == depId);
            var users = orgM.Users.Where(u => dep.SysUserIDs.Contains(u.SysUserId)).ToList();
            if (users.IsNull())
                EGExceptionOperator.ThrowX<Exception>("缺少用户数据", string.Empty);
            return users;
        }
        /// <summary>
        /// 通过部门集合获取这些部门拥有的用户
        /// </summary>
        /// <returns></returns>
        public List<UserVM> GetUsersByDeps(List<int> depIds)
        {
            var orgM = GetOrgM();
            var userIDs = orgM.Departments.Where(d => depIds.Contains(d.Did)).SelectMany(d => d.SysUserIDs);
            var users = orgM.Users.Where(u => userIDs.Contains(u.SysUserId)).ToList();
            if (users.IsNull())
                EGExceptionOperator.ThrowX<Exception>("缺少用户数据", string.Empty);
            return users;
        }
        /// <summary>
        /// 通过部门集合获取这些部门拥有的用户ID
        /// </summary>
        /// <returns></returns>
        public List<int> GetUserIDsByDeps(List<int> depIds)
        {
            var orgM = GetOrgM();
            var userIDs = orgM.Departments.Where(d => depIds.Contains(d.Did)).SelectMany(d => d.SysUserIDs).ToList();
            if (userIDs.IsNull())
                EGExceptionOperator.ThrowX<Exception>("缺少用户数据", string.Empty);
            return userIDs;
        }
        #endregion

        #region 获取基础租户模型
        /// <summary>
        /// 获取组织机构基础数据模型
        /// </summary>
        /// <returns></returns>
        private OrgM GetOrgM()
        {
            var pOrgM = _Cache.Get<OrgM>(ConstStr.CACHE_ORGNIZATIONMODEL, GetOrgMFromData, 5);
            return pOrgM;
        }
        /// <summary>
        /// 从数据库中获取组织机构数据模型
        /// </summary>
        /// <returns></returns>
        private OrgM GetOrgMFromData()
        {
            var userBL = new UserBL();
            var depBL = new DepartmentBL();
            var tagBL = new TagBL();

            //var vm = new OrgM<UserBM, DepartmentBM, TagBM>()

            var pUsers = userBL.GetUsers<UserVM>();
            var pDepartments = depBL.GetDepartments<DepartmentVM>();
            var pDep2UserRel = depBL.GetDep2UserRel<Dep2UserRelTM>();
            var pTags = tagBL.GetTags<TagBM>();
            var vm = new OrgM(pUsers, pDepartments, pDep2UserRel, pTags);
            return vm;
        }
        #endregion

    }
}
