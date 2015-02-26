using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EG.WeChat.Business.Account;
using EG.WeChat.Model;
using EG.WeChat.Web.Models;


namespace EG.WeChat.Web.Controllers
{
    public class AccountController : Controller
    {
        private AccountBL _accountBL;
        protected AccountBL AccountBL
        {
            get
            {
                if (_accountBL == null)
                {
                    _accountBL = TransactionProxy.New<AccountBL>();
                }
                return _accountBL;
            }
        }

        [HttpPost]
        public ActionResult CheckAccountVM(AccountVM model)
        {
            return Json(new { IsSuccess = ModelState.IsValid });
        }

        public ActionResult Validation(string Code)
        {
            AccountVM model = new AccountVM();
            model.OpenID = this.GetOpenId(Code);
            model.AccountNumber = AccountBL.GetAccountName(model.OpenID);
            return View(model);
        }


        [HttpPost]
        public ActionResult Validation(AccountVM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ResultM result = AccountBL.SaveAccount(model.TemData);

                    if (result.IsSuccess)
                    {
                        EG.WeChat.Service.WeiXin.WeixinMessageSender.SendText(model.OpenID, String.Format("戶口綁定 成功！\r\n關聯的交易帳號為：{0}。\r\n{1}", model.AccountNumber, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                    }
                    return Json(result);
                }
                catch (Exception ex)
                {
                    return Json(new { IsSuccess = false, Message = ex.Message });
                }
            }
            return View(model);
        }


        public ActionResult UnValidation(string Code)
        {
            AccountVM model = new AccountVM();
            model.OpenID = this.GetOpenId(Code);
            model.AccountNumber = AccountBL.GetAccountName(model.OpenID);
            return View(model);
        }


        [HttpPost]
        public ActionResult UnValidation(AccountVM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ResultM result = AccountBL.DelAccount(model.TemData);

                    if (result.IsSuccess)
                    {
                        EG.WeChat.Service.WeiXin.WeixinMessageSender.SendText(model.OpenID, "戶口解綁 成功！\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                    return Json(result);
                }
                catch (Exception ex)
                {
                    return Json(new { IsSuccess = false, Message = ex.Message });
                }
            }
            return View(model);
        }


        [HttpPost]
        public ActionResult CheckChangeAccountVM(ChangeAccountVM model)
        {
            return Json(new { IsSuccess = ModelState.IsValid });
        }

        public ActionResult ChangeValidation(string Code)
        {
            ChangeAccountVM model = new ChangeAccountVM();
            model.OpenID = this.GetOpenId(Code);
            model.OldAccountNumber = AccountBL.GetAccountName(model.OpenID);
            return View(model);
        }


        [HttpPost]
        public ActionResult ChangeValidation(ChangeAccountVM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ResultM result = AccountBL.SaveChangeAccount(model.TemData, model.OldAccountNumber, model.OldPassword);

                    if (result.IsSuccess)
                    {
                        EG.WeChat.Service.WeiXin.WeixinMessageSender.SendText(model.OpenID, String.Format("戶口修改 成功！\r\n新的關聯的交易帳號為：{0}。\r\n{1}", model.AccountNumber, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                    }
                    return Json(result);
                }
                catch (Exception ex)
                {
                    return Json(new { IsSuccess = false, Message = ex.Message });
                }
            }
            return View(model);
        }

        /// <summary>
        /// 提示重新打开
        /// </summary>
        /// <returns></returns>
        private ContentResult TipReOpen()
        {
            /* 当无法正确获取openId的时候，提示用户稍后重试。 */
            return this.Content("抱歉，微信服務無法正常回應。請稍後重新打開頁面。");
        }
    }
}