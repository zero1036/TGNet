using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*****************************************************
* 目的：用户模型
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
    /// 基础属性模型
    /// </summary>
    public class UserBM
    {
        /// <summary>
        /// 系统用户编号
        /// </summary>
        public int SysUserId { get; set; }
        /// <summary>
        /// 用户编号
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 职位
        /// </summary>
        public string Position { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 电邮
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 微信号
        /// </summary>
        public string WeixinId { get; set; }
        /// <summary>
        /// 头像地址
        /// </summary>
        public string Avatar { get; set; }
        /// <summary>
        /// 关注状态
        /// </summary>
        public int Status { get; set; }
    }
    /// <summary>
    /// 数据表模型
    /// </summary>
    public class UserTM : UserBM
    {

        /// <summary>
        /// 租户编号
        /// </summary>
        public int Tid { get; set; }
        /// <summary>
        /// 登陆密码
        /// </summary>
        public string Password { get; set; }
    }
    /// <summary>
    /// 当前用户——系统模型
    /// </summary>
    public class CurUserM : UserTM
    {
        /// <summary>
        /// 路由映射关系
        /// </summary>
        /// <returns></returns>
        public List<TenantRouteM> TenantRoutes { get; set; }
        /// <summary>
        /// 所在部门
        /// </summary>
        public List<DepartmentTM> Departments { get; set; }
        /// <summary>
        /// 所属标签
        /// </summary>
        public List<TagTM> Tags { get; set; }
        /// <summary>
        /// 所有菜单
        /// </summary>
        public List<MenuTM> Menus { get; set; }

    }
    /// <summary>
    /// 视图模型
    /// </summary>
    public class UserVM : UserBM
    {
        /// <summary>
        /// 所在部门集合名称
        /// </summary>
        public List<string> DepartmentsName { get; set; }
        /// <summary>
        /// 所属标签集合名称
        /// </summary>
        public List<string> TagsName { get; set; }
        /// <summary>
        /// 名称首字母
        /// </summary>
        public string FirstNameChar
        {
            get
            {
                if (string.IsNullOrEmpty(base.Name)) return string.Empty;
                return TW.Platform.Sys.StringHelper.GetFirstSpell(base.Name).ToLower();
            }
        }
        /// <summary>
        /// 是否首字母分组第一位
        /// </summary>
        public bool IsFirst { get; set; }
    }

}
