using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EG.WeChat.Web.Controllers;
using System.Web.Routing;
using Moq;
using System.Web;
using EG.WeChat.Web.Models;
using EG.WeChat.Web.Service;
using System.Threading;
using EG.WeChat.UnitTest.Define;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using System.Net;
using System.IO;
/*****************************************************
* 目的：微信项目 账户绑定相关Controller单元测试
* 创建人：林子聪
* 创建时间：20141104
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.UnitTest.Model
{
    /// <summary>
    /// 测试用例
    /// </summary>
    [TestClass]
    public class AccountControllerUT
    {
        #region 测试客户端初始化调用
        /// <summary>
        /// 测试客户端初始化调用
        /// </summary>
        public static void Init()
        {
            #region 处理Web.Config
            EG.Business.Common.ConfigCache.LoadAppConfig(new string[] 
            {//##需要解密的字段 
                "DB_USER","DB_PSW",                         //数据库
                "WX_appID","WX_appsecret","WX_Token"        //微信账号
            });

            ////微信账号信息设置
            //WeiXinConfiguration.appID = EG.Business.Common.ConfigCache.GetAppConfig("WX_appID");
            //WeiXinConfiguration.appsecret = EG.Business.Common.ConfigCache.GetAppConfig("WX_appsecret");
            //WeiXinConfiguration.Token = EG.Business.Common.ConfigCache.GetAppConfig("WX_Token");
            #endregion
        }
        #endregion

        #region 单元测试
        [TestMethod]
        /// <summary>
        /// 绑定解绑接口随机测试
        /// </summary>
        public static void ValidationActionTest()
        {
            //accountController.ControllerContext = MvcContextMockFactory.CreateControllerContext(accountController, "~/Account/UnValidation", "post", "DefaultRoute", "{controller}/{action}", null);
            BaseAction ua;
            for (int i = 0; i < 20; i++)
            {
                ua = new BaseAction();
                //ua.run();
                //Thread thread = new Thread(ua.run);
                //thread.Start();
            }

        }
        [TestMethod]
        /// <summary>
        /// 发送模板消息测试
        /// </summary>
        public static void SendTemplateMessageTest()
        {
            //string strAT = "PczdlQ-MZKyl6GsfOADL1DL2wpXu88lRE5kXIlbA-1QkhqNb6fDw4AYtxXnPwyhESf9GlzFy-JrWMZlIIAUQiRPVLiZeaCCmhkU_wFypiuc";
            //OpenIdResultJson result = Senparc.Weixin.MP.AdvancedAPIs.User.Get(strAT, "");//(accessToken, requestMessage.FromUserName, Senparc.Weixin.Language.zh_TW);

            //do
            //{

            //} while (String.IsNullOrEmpty(result.next_openid) == false);
            //Console.WriteLine(result.data.openid[0]);
        }
        #endregion

    }   
    /// <summary>
    /// 
    /// </summary>
    public class BaseAction
    {
        //#region 私有成员
        //protected AccountController m_accountController;
        //private string m_CurAccount;
        //private string m_OldAccount;
        //private string m_CurOpenID;
        //protected AccountVM m_CurAccountVM;
        //private IList<string> m_ListAccount;
        //private IList<string> m_ListOpenID;

        //private int m_AccountNum = 5;
        //#endregion

        //#region
        ///// <summary>
        ///// 
        ///// </summary>
        //public BaseAction()
        //{
        //    //m_accountController = accountController;
        //    //m_accountController = new AccountController();
        //    //m_accountController.ControllerContext = MvcContextMockFactory.CreateControllerContext(m_accountController, "~/Account/UnValidation", "post", "DefaultRoute", "{controller}/{action}", null);

        //    //accountController_UnBinding = new AccountController();
        //    //accountController_UnBinding.ControllerContext = MvcContextMockFactory.CreateControllerContext(accountController_UnBinding, "~/Account/UnValidation", "post", "DefaultRoute", "{controller}/{action}", null);
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        //public IList<string> ListAccount
        //{
        //    get
        //    {
        //        if (m_ListAccount == null || m_ListAccount.Count == 0)
        //        {
        //            string strAccount;
        //            m_ListAccount = new List<string>();
        //            for (int i = 1; i <= m_AccountNum; i++)
        //            {
        //                strAccount = string.Format("{0}{0}{0}{0}{0}{0}{0}{0}", i);
        //                m_ListAccount.Add(strAccount);
        //            }
        //        }
        //        return m_ListAccount;
        //    }
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        //public IList<string> ListOpenID
        //{
        //    get
        //    {
        //        if (m_ListOpenID == null || m_ListOpenID.Count == 0)
        //        {
        //            string strOpenID;
        //            m_ListOpenID = new List<string>();
        //            for (int i = 1; i <= m_AccountNum; i++)
        //            {
        //                strOpenID = string.Format("qGOJuOetJUM0kdIkZ3XmYigHjdA{0}", i);
        //                m_ListOpenID.Add(strOpenID);
        //            }
        //        }
        //        return m_ListOpenID;
        //    }
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        //public void run()
        //{
        //    while (true)
        //    {
        //        try
        //        {
        //            var pRam = new Random();
        //            int iType = pRam.Next(1, 6);
        //            //int iType = 3;
        //            int iAccount = pRam.Next(0, m_AccountNum + 1);
        //            int iOldAccount = pRam.Next(0, m_AccountNum + 1);
        //            int iOpenID = pRam.Next(0, m_AccountNum + 1);

        //            if (iAccount <= ListAccount.Count - 1)
        //                m_CurAccount = ListAccount[iAccount];
        //            if (iOpenID <= ListOpenID.Count - 1)
        //                m_CurOpenID = ListOpenID[iOpenID];
        //            if (iOldAccount <= ListAccount.Count - 1)
        //                m_OldAccount = ListAccount[iOldAccount];

        //            if (string.IsNullOrEmpty(m_CurAccount) || string.IsNullOrEmpty(m_CurOpenID))
        //                continue;
        //            if (iAccount == iOldAccount)
        //                continue;


        //            string strActionName = string.Empty;
        //            m_accountController = new AccountController();
        //            m_accountController.ControllerContext = MvcContextMockFactory.CreateControllerContext(m_accountController, "~/Account/UnValidation", "post", "DefaultRoute", "{controller}/{action}", null);



        //            if (iType >= 3)
        //            {
        //                ActionResult result;
        //                ChangeAccountVM changeEN = new ChangeAccountVM();
        //                changeEN.AccountNumber = m_CurAccount;
        //                changeEN.OldAccountNumber = m_OldAccount;
        //                changeEN.OldPassword = "2";
        //                changeEN.Password = "2";
        //                changeEN.OpenID = m_CurOpenID;
        //                result = m_accountController.ChangeValidation(changeEN);
        //                strActionName = "改变绑定";

        //                JsonResult pJson = result as JsonResult;
        //                string strError = string.Format("OpenID：{0}--新账号：{1}--旧账号：{2}", m_CurOpenID.Substring(m_CurOpenID.Length - 1, 1), m_CurAccount, m_OldAccount);
        //                Console.WriteLine(strError);
        //                strError = string.Format("类型：{0}--消息：{1}", strActionName, pJson.Data.ToString());
        //                Console.WriteLine(strError);
        //                Console.WriteLine("   ");
        //            }
        //            else
        //            {
        //                ActionResult result;
        //                m_CurAccountVM = new AccountVM();
        //                m_CurAccountVM.OpenID = m_CurOpenID;
        //                m_CurAccountVM.AccountNumber = m_CurAccount;
        //                m_CurAccountVM.Password = "2";


        //                if (iType == 1)
        //                {
        //                    result = m_accountController.Validation(m_CurAccountVM);
        //                    strActionName = "绑定";
        //                }
        //                else
        //                {
        //                    result = m_accountController.UnValidation(m_CurAccountVM);
        //                    strActionName = "解绑";
        //                }

        //                JsonResult pJson = result as JsonResult;
        //                string strError = string.Format("OpenID：{0}--账号：{1}", m_CurOpenID.Substring(m_CurOpenID.Length - 1, 1), m_CurAccount);
        //                Console.WriteLine(strError);
        //                strError = string.Format("类型：{0}--消息：{1}", strActionName, pJson.Data.ToString());
        //                Console.WriteLine(strError);
        //                Console.WriteLine("   ");
        //            }




        //            int iTime = pRam.Next(1, 5);
        //            System.Threading.Thread.Sleep(iTime * 1000);
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e);
        //            // TODO: handle exception
        //            //System.out.printf("错误信息%s%n", e.getMessage());
        //        }
        //    }

        //}
        //#endregion

    }
}
