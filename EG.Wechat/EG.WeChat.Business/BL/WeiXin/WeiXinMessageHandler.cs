using EG.WeChat.Utility.WeiXin;
using Senparc.Weixin.MP.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Senparc.Weixin.MP.Entities.Request;

namespace EG.WeChat.Business.BL.WeiXin
{
    /* EG.WeChat.Utility的CustomMessageHandler，提供通用的消息处理；
     * 需要加入定制化的逻辑时，由各自业务系统的Business项目，处理：
     * 1.处理完之后，需要继续交由通用逻辑时，重载之后使用Base.方法名；
     * 2.处理完之后，不需要交由通用逻辑时，直接在重载结束即可。
     */

    public class WeiXinMessageHandler : CustomMessageHandler
    {

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="inputStream"></param>
        /// <param name="maxRecordCount"></param>
        public WeiXinMessageHandler(Stream inputStream, PostModel postModel, int maxRecordCount = 0)
            : base(inputStream, postModel,maxRecordCount)
        {

        }
        #endregion

        #region 处理重载

        /// <summary>
        /// 定制化 文本请求
        /// </summary>
        public override IResponseMessageBase OnTextRequest(RequestMessageText requestMessage)
        {
            string message = requestMessage.Content;

            switch (message)
            {
                case "?":
                case "？":
                    return ShowRuntimeDebugInfo();

                case "HYK":
                case "hyk":
                    return ShowVipCardNews(requestMessage.FromUserName);
            }

            return base.OnTextRequest(requestMessage);
        }

        /// <summary>
        /// 定制化 菜单点击
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_ClickRequest(RequestMessageEvent_Click requestMessage)
        {
            switch (requestMessage.EventKey)
            {
                case "VipCard":
                    return ShowVipCardNews(requestMessage.FromUserName);
            }

            return base.OnEvent_ClickRequest(requestMessage);
        }

        #endregion


        #region 呈现“运行调试”信息
        /// <summary>
        /// 呈现“运行调试”信息
        /// </summary>
        private IResponseMessageBase ShowRuntimeDebugInfo()
        {
            var ret = base.CreateResponseMessage<ResponseMessageText>();
            ret.Content = "运行中的版本:" + System.Reflection.Assembly.GetExecutingAssembly().FullName;
            return ret;
        }

        #endregion

        #region 呈现“會員卡”图文
        /// <summary>
        /// 呈现“會員卡”图文
        /// </summary>
        private IResponseMessageBase ShowVipCardNews(string OpenID)
        {
            //会员卡
            var ret = base.CreateResponseMessage<ResponseMessageNews>();
            ret.Articles.Add(new Article()
            {
                Title = "會員卡",
                Description = @"快速申請會員卡
查詢會員卡資訊與優惠
消費會員卡積分
",
                Url     = "http://webchat.cloudapp.net/Member?OpenID=" + OpenID,
                PicUrl  = "http://webchat.cloudapp.net/Images/common/vip_NewsImg.jpg"
            });
            return ret;
        }
        #endregion

    }
}
