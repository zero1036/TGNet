using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EG.WeChat.Web.Models;
using Senparc.Weixin.MP.AdvancedAPIs;
using EG.WeChat.Service.WeiXin;
using Senparc.Weixin.MP.CommonAPIs;
using System.IO;
using EG.WeChat.Service;
//using EG.WeChat.Business.Interface;
using EG.WeChat.Utility.Tools;
using EG.WeChat.Platform.Model;
using EG.WeChat.Platform.BL;
using EG.WeChat.Web.Models;
/*****************************************************
* 目的：微信组织（用户、分组）Controller
* 创建人：林子聪
* 创建时间：20141202
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Web.Controllers
{

    public class WXOrganizationController : AccessController
    {
        #region 微信用戶
        /// <summary>
        /// 微信用户管理页面加载action
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult WXUserManager()
        {
            return View("Message2");
        }
        /// <summary>
        /// 微信用户同步action
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ReLoadWXUser()
        {
            //获取基础服务接口、微信端服务接口、本地端服务接口
            WeChatOrgService pOrgService = new WeChatOrgService();
            //重新从微信端获取微信用户数据并保存在数据库
            pOrgService.ReLoadAllWeChatUser();
            List<GroupsJson_Group> pGroups = pOrgService.ReLoadAllWeChatGroup();
            EGExceptionResult pResult = pOrgService.GetActionResult();
            if (pResult != null)
            {
                return Json(pResult);
            }
            pResult = new EGExceptionResult(true, "", "");
            //返回视图
            return Json(pResult);
        }
        ///// <summary>
        ///// 加载微信用户列表action
        ///// </summary>
        ///// <param name="PageIndex"></param>
        ///// <param name="RowCountInPage"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult LoadUserTable(string PageIndex, string RowCountInPage, string GroupId)
        //{
        //    if (string.IsNullOrEmpty(GroupId))
        //        GroupId = "-1";
        //    //目标跳转页码
        //    int iPageIndex = 1;
        //    //受限每页行数
        //    int iRowCountInPage = 10;
        //    //获取微信用户集合
        //    WeChatOrgService pOrgService = new WeChatOrgService();
        //    List<WeChatUser> pList = pOrgService.GetWCUserList_Cache(Convert.ToInt16(GroupId));
        //    //受限总共显示记录数，由服务端控制，暂时写死2000条，现行最大记录可以去到1万条json记录，扩展jsonresult可以去到50万条
        //    pList = CommonFunction.SubListForTable<WeChatUser>(pList, 2000);
        //    if (!string.IsNullOrEmpty(PageIndex) && int.TryParse(PageIndex, out iPageIndex) && int.TryParse(RowCountInPage, out iRowCountInPage))
        //    {
        //        //筛选目标页记录数
        //        pList = CommonFunction.SubListForTable<WeChatUser>(pList, iPageIndex, iRowCountInPage);
        //    }
        //    return Json(pList);
        //}
        /// <summary>
        /// 查询微信用户列表action
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <param name="RowCountInPage"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult QueryUserTable(int PageIndex = 1, int RowCountInPage = 10, string filterString = "")
        {
            int iRecCount = -1;
            //获取微信用户集合
            WeChatOrgService pOrgService = new WeChatOrgService();
            List<WeChatUser> pList = pOrgService.QueryWCUserList_Cache(PageIndex, RowCountInPage, filterString, ref iRecCount);
            EGExceptionResult pResult = pOrgService.GetActionResult();
            if (pResult != null)
            {
                return Json(pResult);
            }
            //生成结果json，包含目标条件所有记录数，及经过分页后记录集合
            var param = new { RecordsCount = iRecCount, Users = pList };
            return Json(param);
        }
        #endregion

        #region 微信分組
        /// <summary>
        /// 启动新建微信分组页面
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateWXGroupView()
        {
            return View("WXCreateGroup");
        }
        /// <summary>
        /// 新建微信分组
        /// </summary>
        /// <param name="groupname"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateWXGroup(string groupname)
        {
            //服务
            WeChatOrgService pOrgService = new WeChatOrgService();
            //新建微信分组，保存数据库，但不获取新集合
            List<GroupsJson_Group> pList = pOrgService.CreateWXGroupAndLoad(groupname);
            EGExceptionResult pResult = pOrgService.GetActionResult();
            if (pResult != null)
            {
                return Json(pResult);
            }
            return Json(pList);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ListOpenID"></param>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public ActionResult ChangeGroup(string ListOpenID, string GroupID)
        {
            WeChatOrgService pService = new WeChatOrgService();
            pService.ChangeWXGroup(ListOpenID, GroupID);
            EGExceptionResult pResult = pService.GetActionResult();
            if (pResult == null)
            {
                pResult = new EGExceptionResult(true, "创建成功", ((int)EGActionCode.执行成功).ToString());
            }
            return Json(pResult);
        }
        /// <summary>
        /// 获取微信用户分组Json
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetWXGroups()
        {
            //从缓存中换取微信分组信息集合
            WeChatOrgService pOrgService = new WeChatOrgService();
            //pOrgService.ReLoadAllWeChatGroup();
            List<GroupsJson_Group> pList = pOrgService.GetWCGroupList_Cache();
            EGExceptionResult pResult = pOrgService.GetActionResult();
            if (pResult != null)
            {
                return new EmptyResult();
            }
            return Json(pList);
        }
        #endregion
    }
}
