﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TW.Web.Controllers
{
    public class LoginController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage Login([FromBody]UserPostParamVM param)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            string strUserName = param.Account;
            string strPassword = param.Password;
            var accountModel = new AccountHelper();

            try
            {
                //验证用户是否是系统注册用户
                if (accountModel.ValidateUserLogin(strUserName, strPassword))
                {
                    //创建用户ticket信息
                    accountModel.CreateLoginUserTicket(strUserName, strPassword);

                    //读取用户权限数据
                    accountModel.GetUserAuthorities(strUserName);
                    response = Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    response = Request.CreateResponse(HttpStatusCode.Forbidden);
                }
            }
            catch (Exception ex)
            {
                //LogHelper.Write(this.GetType(), ex);
                //response = Request.CreateResponse<string>(HttpStatusCode.InternalServerError, ex.Message);
            }
            return response;
        }

        [HttpPost]
        public HttpResponseMessage Logout()
        {
            HttpResponseMessage response = new HttpResponseMessage();

            //SysCommon.CurrentUser = null;
            response = Request.CreateResponse(HttpStatusCode.OK);

            return response;
        }
    }

    public class UserPostParamVM
    {
        public string Account { get; set; }

        public string Password { get; set; }
    }
}