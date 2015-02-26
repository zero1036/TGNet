using System;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Senparc.Weixin.MP.Agent;
using Senparc.Weixin.Context;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Helpers;
using Senparc.Weixin.MP.MessageHandlers;
using RC = EG.WeChat.Utility.WeiXin.ResponseChain;

namespace EG.WeChat.Utility.WeiXin
{
    /// <summary>
    /// 自定义MessageHandler
    /// </summary>
    public partial class CustomMessageHandler
    {
        private string GetWelcomeInfo()
        {
            //获取Senparc.Weixin.MP.dll版本信息
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(HttpContext.Current.Server.MapPath("~/bin/Senparc.Weixin.MP.dll"));
            var version = string.Format("{0}.{1}", fileVersionInfo.FileMajorPart, fileVersionInfo.FileMinorPart);
            //return "歡迎關注 英皇金業集團微信服務號。\r\n\r\n請點擊 【帳號管理】->【戶口綁定】，獲得更多功能。\r\n回復0,進入指令功能表。\r\n回復news，獲取最新資訊。\r\n回復me，獲取您的使用者資訊。";

            //演示
            return "歡迎關注 英皇金業集團微信服務號。\r\n\r\n請點擊會話視窗的底部功能表，使用更多的功能。\r\n回復0,進入智能應答。";
        }

        /// <summary>
        /// 进入人工客服
        /// </summary>
        /// <returns></returns>
        protected IResponseMessageBase GetArtificialCustomerService(params string[] accountList)
        {
            var ret = base.CreateResponseMessage<ResponseMessageTransfer_Customer_Service>();

            /* 背景说明：
             * 1.SDK的接口（ret.TransInfo为List<>），可以看出SDK在准备支持“指定多个客服账号”；
             *   微信文档中的多客服对应的XML结构，也看出在准备支持“指定多个客服账号”。
             * 2.目前指定多个时，只有列表中的第一个有效；
             * 3.微信文档建议：目前阶段可以通过“检测接口”，由我们自己去判断哪些。账号是“上线”+“空闲”，
             *   然后才指定那个账号
             * 4.* 上述的3，能够实现多客服；但是属于目前阶段，+时间极短的情况下，仍然会出现：
             *   “1秒前检测上线和空闲，1秒后指定这个账户时它已经忙碌状态；然后一直处于等待”。
             * -- 综合考虑，目前存在小概率的问题，但是这个“模拟多客服”的方式，仍然是非常有价值的，
             *    因此进行实现。待到以后官方开放“多个账号”的处理时，才注释以下的逻辑。
             * 
             * 
             * 思路备忘：
             * 1.指定的账号为0个时，则使用默认规则，不指定客服账号；
             * 2.指定的账号大于1个时，异步检测客服账号在线情况和空闲情况；
             * 3.如果检测不到有效的客服账号，返回消息，告知用户“客服全忙，稍后重试”；
             * 4.*结果账户设置随机顺序，客服账号的指定不均匀。
             */

            #region 指定具体的客服账号进行处理-- 检测，然后指定“在线+空闲”的1个。
            /* 后续官方开放之后，注释/移除 这个折叠代码即可 :) */

            if (accountList != null && accountList.Count() > 0)
            {
                //##准备
                string accessToken = Senparc.Weixin.MP.CommonAPIs.WeiXinSDKExtension.GetCurrentAccessToken();
                Random randomer = new Random();
                
                //##获取当前在线的客服列表
                var OnlineIdleAccountList = from finallyData in
                                                (from custom in Senparc.Weixin.MP.AdvancedAPIs.CustomService.GetCustomOnlineInfo(accessToken).kf_online_list
                                                 where custom.status > 0                    //在线状态；这里不关注分PC或手机。
                                                    && custom.accepted_case <= 0            //空闲状态；正在接待会话数为0。
                                                 select new
                                                 {
                                                     account = custom.kf_account,           //账户
                                                     order   = randomer.Next(byte.MaxValue),//随机个顺序号码
                                                 })
                                            orderby finallyData.order                       //排序
                                            select finallyData.account;

                //从“空闲”列表中，拣出与指定列表匹配的账户(交集)
                OnlineIdleAccountList = accountList.Intersect(OnlineIdleAccountList);

                //处理最终结果
                if (OnlineIdleAccountList != null && OnlineIdleAccountList.FirstOrDefault() != null)
                {
                    ret.TransInfo.Add(new CustomerServiceAccount
                    {
                        KfAccount = OnlineIdleAccountList.FirstOrDefault()
                    });
                }
                else
                {
                    var busyRet = base.CreateResponseMessage<ResponseMessageText>();
                    busyRet.Content = "現在人工客服全忙，請稍後再試。";
                    return busyRet;
                }
            }

            #endregion

            #region 指定具体的客服账号进行处理-- 一次性指定多个

            //if (accountList != null)
            //{
            //    foreach (string account in accountList)
            //    {
            //        ret.TransInfo.Add(new CustomerServiceAccount 
            //        { 
            //            KfAccount = account
            //        });
            //    }
            //}

            #endregion

            return ret;
        }

        public override IResponseMessageBase OnTextOrEventRequest(RequestMessageText requestMessage)
        {
            // 预处理文字或事件类型请求。
            // 这个请求是一个比较特殊的请求，通常用于统一处理来自文字或菜单按钮的同一个执行逻辑，
            // 会在执行OnTextRequest或OnEventRequest之前触发，具有以下一些特征：
            // 1、如果返回null，则继续执行OnTextRequest或OnEventRequest
            // 2、如果返回不为null，则终止执行OnTextRequest或OnEventRequest，返回最终ResponseMessage
            // 3、如果是事件，则会将RequestMessageEvent自动转为RequestMessageText类型，其中RequestMessageText.Content就是RequestMessageEvent.EventKey

            //if (requestMessage.Content == "OneClick")
            //{
            //    var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
            //    strongResponseMessage.Content = "您点击了底部按钮。\r\n为了测试微信软件换行bug的应对措施，这里做了一个——\r\n换行";
            //    return strongResponseMessage;
            //}
            return null;//返回null，则继续执行OnTextRequest或OnEventRequest
        }

        public override IResponseMessageBase OnEvent_ClickRequest(RequestMessageEvent_Click requestMessage)
        {
            //扩展处理
            {
                string retNodeId;
                if (MessageHandlerExtend.ClickEvent2ResponseChain.CheckThenGetNodeId(requestMessage.EventKey, out retNodeId))
                {
                    RC.ResponseService service = RC.ResponseService_ConfigurationSupport.CreateOrGetService(this.CurrentMessageContext);     //[使用配置文件方式]
                    //跳转到指定节点
                    RC.IResponseMessage ret =  service.JumpToTargetnode(requestMessage.FromUserName, retNodeId);
                    //返回结果
                    return ResponseChainRequest_LevelForNode_ResultHandler(ret);
                }
            }


            IResponseMessageBase reponseMessage = null;
            //菜单点击，需要跟创建菜单时的Key匹配
            switch (requestMessage.EventKey)
            {
                #region SDK使用范例 备份
                /*

                case "OneClick":
                    {
                        //这个过程实际已经在OnTextOrEventRequest中完成，这里不会执行到。
                        var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
                        reponseMessage = strongResponseMessage;
                        strongResponseMessage.Content = "您点击了底部按钮。\r\n为了测试微信软件换行bug的应对措施，这里做了一个——\r\n换行";
                    }
                    break;
                case "SubClickRoot_Text":
                    {
                        var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
                        reponseMessage = strongResponseMessage;
                        strongResponseMessage.Content = "您点击了子菜单按钮。";
                    }
                    break;
                case "SubClickRoot_News":
                    {
                        var strongResponseMessage = CreateResponseMessage<ResponseMessageNews>();
                        reponseMessage = strongResponseMessage;
                        strongResponseMessage.Articles.Add(new Article()
                        {
                            Title = "您点击了子菜单图文按钮",
                            Description = "您点击了子菜单图文按钮，这是一条图文信息。",
                            PicUrl = "http://weixin.senparc.com/Images/qrcode.jpg",
                            Url = "http://weixin.senparc.com"
                        });
                    }
                    break;
                case "SubClickRoot_Music":
                    {
                        var strongResponseMessage = CreateResponseMessage<ResponseMessageMusic>();
                        reponseMessage = strongResponseMessage;
                        strongResponseMessage.Music.MusicUrl = "http://weixin.senparc.com/Content/music1.mp3";
                    }
                    break;
                case "SubClickRoot_Image":
                    {
                        var strongResponseMessage = CreateResponseMessage<ResponseMessageImage>();
                        reponseMessage = strongResponseMessage;
                        strongResponseMessage.Image.MediaId = "Mj0WUTZeeG9yuBKhGP7iR5n1xUJO9IpTjGNC4buMuswfEOmk6QSIRb_i98do5nwo";
                    }
                    break;
                case "SubClickRoot_Agent"://代理消息
                    {
                        //获取返回的XML
                        DateTime dt1 = DateTime.Now;
                        reponseMessage = MessageAgent.RequestResponseMessage(this, agentUrl, agentToken, RequestDocument.ToString());
                        //上面的方法也可以使用扩展方法：this.RequestResponseMessage(this,agentUrl, agentToken, RequestDocument.ToString());

                        DateTime dt2 = DateTime.Now;

                        if (reponseMessage is ResponseMessageNews)
                        {
                            (reponseMessage as ResponseMessageNews)
                                .Articles[0]
                                .Description += string.Format("\r\n\r\n代理过程总耗时：{0}毫秒", (dt2 - dt1).Milliseconds);
                        }
                    }
                    break;
                case "Member"://托管代理会员信息
                    {
                        //原始方法为：MessageAgent.RequestXml(this,agentUrl, agentToken, RequestDocument.ToString());//获取返回的XML
                        reponseMessage = this.RequestResponseMessage(agentUrl, agentToken, RequestDocument.ToString());
                    }
                    break;
                case "OAuth"://OAuth授权测试
                    {
                        var strongResponseMessage = CreateResponseMessage<ResponseMessageNews>();
                        strongResponseMessage.Articles.Add(new Article()
                        {
                            Title = "OAuth2.0测试",
                            Description = "点击【查看全文】进入授权页面。\r\n注意：此页面仅供测试（是专门的一个临时测试账号的授权，并非Senparc.Weixin.MP SDK官方账号，所以如果授权后出现错误页面数正常情况），测试号随时可能过期。请将此DEMO部署到您自己的服务器上，并使用自己的appid和secret。",
                            Url = "http://weixin.senparc.com/oauth2",
                            PicUrl = "http://weixin.senparc.com/Images/qrcode.jpg"
                        });
                        reponseMessage = strongResponseMessage;
                    }
                    break;
                case "Description":
                    {
                        var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
                        strongResponseMessage.Content = GetWelcomeInfo();
                        reponseMessage = strongResponseMessage;
                    }
                    break;
                default:
                    {
                        var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
                        strongResponseMessage.Content = "您点击了按钮，EventKey：" + requestMessage.EventKey;
                        reponseMessage = strongResponseMessage;
                    }
                    break;

                */
                #endregion
            }

            return reponseMessage;
        }

        public override IResponseMessageBase OnEvent_EnterRequest(RequestMessageEvent_Enter requestMessage)
        {
            //var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            //responseMessage.Content = "您刚才发送了ENTER事件请求。";
            //return responseMessage;
            return null;
        }

        public override IResponseMessageBase OnEvent_LocationRequest(RequestMessageEvent_Location requestMessage)
        {
            ////这里是微信客户端（通过微信服务器）自动发送过来的位置信息
            //var responseMessage = CreateResponseMessage<ResponseMessageText>();
            //responseMessage.Content = "这里写什么都无所谓，比如：上帝爱你！";
            //return responseMessage;//这里也可以返回null（需要注意写日志时候null的问题）

            //Logger.Log4Net.Info(String.Format("地理位置Z：維度：{0}，經度：{1}，比例尺：{2}",
            //                                  requestMessage.Latitude,
            //                                  requestMessage.Longitude,
            //                                  requestMessage.Precision));

            return null;
        }

        public override IResponseMessageBase OnEvent_ScanRequest(RequestMessageEvent_Scan requestMessage)
        {
            ////通过扫描关注
            //var responseMessage = CreateResponseMessage<ResponseMessageText>();
            //responseMessage.Content = "通过扫描关注。";
            //return responseMessage;

            return null;
        }

        public override IResponseMessageBase OnEvent_ViewRequest(RequestMessageEvent_View requestMessage)
        {
            ////说明：这条消息只作为接收，下面的responseMessage到达不了客户端，类似OnEvent_UnsubscribeRequest
            //var responseMessage = CreateResponseMessage<ResponseMessageText>();
            //responseMessage.Content = "您点击了view按钮，将打开网页：" + requestMessage.EventKey;
            //return responseMessage;

            return null;
        }

        //public override IResponseMessageBase OneEvent_MassSendJobFinisRequest(RequestMessageEvent_MassSendJobFinish requestMessage)
        //{
        //    var responseMessage = CreateResponseMessage<ResponseMessageText>();
        //    responseMessage.Content = "接收到了群发完成的信息。";
        //    return responseMessage;
        //}

        /// <summary>
        /// 订阅（关注）事件
        /// </summary>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_SubscribeRequest(RequestMessageEvent_Subscribe requestMessage)
        {
            var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            responseMessage.Content = GetWelcomeInfo();
            return responseMessage;
        }

        /// <summary>
        /// 退订
        /// 实际上用户无法收到非订阅账号的消息，所以这里可以随便写。
        /// unsubscribe事件的意义在于及时删除网站应用中已经记录的OpenID绑定，消除冗余数据。并且关注用户流失的情况。
        /// </summary>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_UnsubscribeRequest(RequestMessageEvent_Unsubscribe requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            
            /* DoraemonYu 2014-10-28:
             * Here:当用户取消关注的时候，是否应该移除数据库的对应关系？
             */ 

            return responseMessage;
        }
    }
}