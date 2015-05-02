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
        /// <summary>
        /// OrgVM1
        /// 获取组织结构视图1
        /// 用于：WA通讯录部门首页
        /// </summary>
        /// <returns></returns>
        public object GetDepsFromOrgM()
        {
            var orgM = GetOrgM();
            return orgM.Departments;
        }


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
