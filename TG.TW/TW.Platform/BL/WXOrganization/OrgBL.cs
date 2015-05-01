using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TW.Platform.DA;
using TW.Platform.Model;
using EG.WeChat.Utility.Tools;
/*****************************************************
* 目的：组织机构业务逻辑处理
* 创建人：林子聪
* 创建时间：20140427
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace TW.Platform.BL.BL
{
    public class OrgBL
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public OrgVM4 GetOrgVM4()
        {
            var userBL = new UserBL();
            var depBL = new DepartmentBL();
            var tagBL = new TagBL();

            OrgVM4 vm = new OrgVM4()
            {
                Users = userBL.GetUsers<UserVM>(),
                Departments = depBL.GetDepartments<DepartmentVM>(),
                Dep2UserRel = depBL.GetDep2UserRel<Dep2UserRelTM>(),
                Tags = tagBL.GetTags<TagTM>()
            };
            return vm;
        }
    }
}
