using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EG.WeChat.Model;
using EG.WeChat.Platform.BL;
using EG.WeChat.Platform.Model;
using EG.WeChat.Web.Models;
using EG.WeChat.Utility.Tools;
using EG.WeChat.Web.Common;
/*****************************************************
* 目的：本地会员及会员卡控制器
* 创建人：
* 创建时间：
* 修改目的：添加会员卡页面生成Action
* 修改人：林子聪
* 修改时间：20141215
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Web.Controllers
{
    public class MemberController : Controller
    {
        private MemberBL _memberBL;
        protected MemberBL MemberBL
        {
            get
            {
                if (_memberBL == null)
                {
                    _memberBL = TransactionProxy.New<MemberBL>();
                }
                return _memberBL;
            }
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Register(string Code)
        {
            MemberVM model = new MemberVM();
            model.OpenID = this.GetOpenId(Code);
            var members = MemberBL.GetMembers(model.OpenID);
            if (members.Count > 0)
            {
                return View("Detail", members);
            }
            return View(model);
        }

        /// <summary>
        /// 注册会员
        /// </summary>
        /// <param name="pModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Register(CardApplyVM pModel)
        {
            try
            {
                ResultM result = MemberBL.Register(pModel.TemData);

                if (result.IsSuccess)
                {
                    //return Detail(pModel.OpenID);
                    EGExceptionResult pResult = CreateCardApplyVM(pModel.OpenID, pModel);
                    if (pResult != null)
                        return Json(pResult);

                    pModel.NewMemberInfo = "恭喜你成為我們的會員";
                    return View("CardApply", pModel);
                }
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// 销户
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="openID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Cancellation(string ID, string openID)
        {
            try
            {
                ResultM result = MemberBL.Cancellation(ID, openID);

                if (result.IsSuccess)
                {
                    return View("Register", new MemberVM() { OpenID = openID });
                }
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="openID"></param>
        /// <returns></returns>
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Detail(string openID)
        {
            var model = MemberBL.GetMembers(openID);
            return View(model);
        }

        /// <summary>
        /// 启动会员页
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Index(string OpenID)
        {
            CardApplyVM pModel = new CardApplyVM();
            EGExceptionResult pResult = CreateCardApplyVM(OpenID, pModel);
            if (pResult != null)
                return Json(pResult);

            return View("CardApply", pModel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OpenID"></param>
        /// <param name="pModel"></param>
        /// <returns></returns>
        private EGExceptionResult CreateCardApplyVM(string OpenID, CardApplyVM pModel)
        {
            pModel.OpenID = OpenID;
            //加载
            MemberCardService pService = new MemberCardService();
            List<CardContent> pList = pService.GetMemberCard(OpenID);
            EGExceptionResult pResult = pService.GetActionResult();
            if (pResult != null)
            {
                return pResult;
            }
            //会员信息数据赋予到页面模型
            pModel.CardForMember = pList;
            return null;
        }

        /// <summary>
        /// 测试Action——用于生成会员卡临时数据
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateX(string exa)
        {
            LCCardService pp = new LCCardService();
            pp.UpdateCardData(exa);
            return new EmptyResult();
        }



    }



}