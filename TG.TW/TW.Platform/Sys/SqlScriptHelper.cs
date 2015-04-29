﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EG.WeChat.Utility.Tools;
/*****************************************************
* 目的：SqlScriptHelper
* 创建人：林子聪
* 创建时间：20150427
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace TW.Platform.Sys
{
    public class SqlScriptHelper
    {
        #region
        public const string T_USER = "t_user";
        /// <summary>
        /// 通过用户ID（UserID）查询用户所在租户及制定表分支
        /// </summary>
        public const string SEL_USER2TID = "select concat(tbname , '_', trid),tid from sys_tenantroute where tid =(select tid from sys_user where userid=?userid) and tbname=?tbname";
        /// <summary>
        /// 查询用户
        /// </summary>
        public const string SEL_SINGLEUSER = "select * FROM {0} t left outer join sys_user s on t.sysuserid = s.sysuserid  where tid = ?tid and {1}";
        /// <summary>
        /// 通过微信号查询用户
        /// </summary>
        public const string SEL_USERBYWEIXINID = "select * from {0} where weixinid={1}";
        /// <summary>
        /// 查询用户所在部门
        /// </summary>
        public const string SEL_DEPARTMENT4USER = "select r.`sysdepartmentid`,r.`tid`,d.`did`,d.`parentdid`,d.`name`,d.`order` from sys_depuserrel r left outer join sys_department d on r.sysdepartmentid=d.sysdepartmentid where r.tid=?tid and r.sysuserid=?sysuserid;";
        /// <summary>
        /// 查询用户及部门所属标签
        /// </summary>
        //public const string SEL_TAG4USER = "select * from sys_tag where tid=?tid and (sysuserid=?sysuserid or sysdepartmentid=?sysdepartmentid);";
        public const string SEL_TAG4USER = "select * from sys_tag where tid=?tid and {0};";
        #endregion
    }



}
