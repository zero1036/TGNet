using EG.WeChat.Platform.BL;
using EG.WeChat.Utility.WeiXin.ResponseChain;
using EG.WeChat.Utility.WeiXin.ResponseChain.CustomHandlers;
using EG.WeChat.Utility.WeiXin.ResponseChain.Handlers.CustomHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace EG.WeChat.Web.Controllers.ResponseChainCustomHandler
{
    public class AdvantanceConfig_CustomHandler_ArtificialServiceController : Controller
    {

        /* ViewData目前使用情况：
         * # "CurrentEditedNode"，   当前正在编辑状态中的节点；
         */


        //----------Members------------




        //----------Control------------

        #region 默认页面

        /// <summary>
        /// 指务必传送 nodeId
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        public ActionResult Index(string nodeId)
        {
            ViewData["CurrentEditedNode"] = nodeId;
            return View("~/Views/ResponseChainCustomHandler/AdvantanceConfig_CustomHandler_ArtificialService/Index.cshtml");
        }

        #endregion


        #region 读取数据

        /// <summary>
        /// 读取数据
        /// </summary>
        [HttpPost]
        public ActionResult GetConfigData(string nodeId)
        {
            //加载配置
            CustomHandler_ArtificialService_ConfigClass config = null;
            Type targetType = typeof(CustomHandler_ArtificialService);

            //##如果Session里有结果，则读取Session里的
            var dictionary = Session["AdvantanceConfig_CustomHandler_Result"] as IDictionary<Type, IConfigClassOfCustomHandler>;
            if (dictionary != null && dictionary.ContainsKey(targetType))
            {
                config = dictionary[targetType] as CustomHandler_ArtificialService_ConfigClass;
            }
            //##如果获取不到(比如Session超时、当前Session首次配置)，则读取配置文件里的
            if (config == null)
            {
                config = ConfigClassOfCustomHandlerHelper.GetConfigClassInstance<CustomHandler_ArtificialService_ConfigClass>(nodeId);
            }
            //##依然无法获取的情况
            if (config == null)
                return Json(new { UseDefaultOrTargetAcctoun = true, AccountList = String.Empty });

            //构建参数
            bool useDefaultOrTargetAcctoun = config.TargetAccountList.Count() <= 0;
            string accountList = String.Empty;
            if (useDefaultOrTargetAcctoun == false)
                accountList = String.Join("\r\n", config.TargetAccountList);

            //返回结果
            return Json(new { UseDefaultOrTargetAcctoun = useDefaultOrTargetAcctoun, AccountList = accountList });
        }
        #endregion

        #region 保存数据到内存

        /// <summary>
        /// 验证账号格式( 中间有一个@，左边是数字，右边是数字或字母 )
        /// </summary>
        static Regex regex4AccountFormat = new Regex(@"^\d+@[A-Za-z0-9]+$", RegexOptions.Compiled);

        /// <summary>
        /// 保存数据到内存
        /// </summary>
        /// <param name="UseDefaultOrTargetAcctoun">True为使用默认规则;False为使用指定账号的规则</param>
        /// <param name="accountList">指定的用户的列表</param>
        [HttpPost]
        public ActionResult SaveConfigData(bool UseDefaultOrTargetAcctoun, string accountList)
        {
            try
            {
                //检查
                if (UseDefaultOrTargetAcctoun == false && accountList.Count() <= 0)
                {
                    //如果指定账号规则，但是又不输入任何内容
                    return Json(new { IsSuccess = false, Message = "指定客服帳號的規則時，需要至少指定一個客服的帳號。" });
                }

                //创建结果容器
                CustomHandler_ArtificialService_ConfigClass result = new CustomHandler_ArtificialService_ConfigClass();

                //开始处理
                if (UseDefaultOrTargetAcctoun == false)
                {
                    //##使用指定的账户
                    List<string> InvalidAccount = new List<string>();

                    //检查账户格式
                    string[] accounts = accountList.Replace("\r", "").Split('\n');
                    foreach (var item in accounts)
                    {
                        string currentAccount = item.Trim();
                        if (String.IsNullOrWhiteSpace(currentAccount))
                            continue;       //过滤空白的内容行

                        if (regex4AccountFormat.IsMatch(currentAccount))
                        {
                            result.TargetAccountList.Add(currentAccount);
                        }
                        else
                        {
                            InvalidAccount.Add(currentAccount);
                        }
                    }

                    //处理结果
                    if (InvalidAccount.Count() > 0)
                    {
                        return Json(new { IsSuccess = false, Message = "以下帳號不符合格式：\r\n" + String.Join("\r\n", InvalidAccount) });
                    }
                }

                //存储到session里
                var dictionary = Session["AdvantanceConfig_CustomHandler_Result"] as IDictionary<Type, IConfigClassOfCustomHandler>;
                if (dictionary == null)
                {
                    Session["AdvantanceConfig_CustomHandler_Result"] = dictionary = new Dictionary<Type, IConfigClassOfCustomHandler>();
                }
                dictionary[typeof(CustomHandler_ArtificialService)] = result;
                
                //返回响应结果
                return Json(new { IsSuccess = true });
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess= false,Message =  ex.Message });
            }
        }

        #endregion

    }
}
