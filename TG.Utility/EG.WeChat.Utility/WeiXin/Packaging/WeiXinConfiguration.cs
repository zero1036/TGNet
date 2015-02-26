using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EG.WeChat.Utility.WeiXin
{
    /// <summary>
    /// 微信与开发相关的账号信息
    /// </summary>
    public sealed class WeiXinConfiguration
    {
        /* 将账号信息相关的内容，存储在这里。
         * 由于涉及敏感信息（得知下面信息之后，可以伪造消息），
         * 因此不存储于Web.config，而编译存储在程序集里。
         * 当绑定新的公众账号时，请调整此处的信息即可。
         * 
         * 2014-10-31:
         * 由于考虑到，正式交付的时候，确实是需要由HK那边,自己去配置以下信息。
         * 因此，最终方案调整为：
         * 存储在Web.config的同时，加密其中敏感的字段(EGCommon程序集去处理)。
         */

        #region 微信账号信息
        /* 只允许赋值一次。请只在Web服务启动的时候，进行赋值。 */

        private static string m_appID       = null;     
        /// <summary>
        /// appID
        /// </summary>
        public static string appID
        {
            get { return WeiXinConfiguration.m_appID; }
            set { if (m_appID != null) return;          WeiXinConfiguration.m_appID = value; }
        }

        private static string m_appsecret   = null;
        /// <summary>
        /// appsecret
        /// </summary>
        public static string appsecret
        {
            get { return WeiXinConfiguration.m_appsecret; }
            set { if (m_appsecret != null) return;      WeiXinConfiguration.m_appsecret = value; }
        }

        private static string m_Token = null;
        /// <summary>
        /// Token
        /// </summary>
        public static string Token
        {
            get { return WeiXinConfiguration.m_Token; }
            set { if (m_Token != null) return;          WeiXinConfiguration.m_Token = value; }
        }

        private static string m_EncodingAESKey = null;
        /// <summary>
        /// EncodingAESKey
        /// </summary>
        public static string EncodingAESKey
        {
            get { return WeiXinConfiguration.m_EncodingAESKey; }
            set { if (m_EncodingAESKey != null) return; WeiXinConfiguration.m_EncodingAESKey = value; }
        }      


        private static string m_cropId = null;
        /// <summary>
        /// 企业ID
        /// </summary>
        public static string cropId
        {
            get { return WeiXinConfiguration.m_cropId; }
            set { if (m_cropId != null) return; WeiXinConfiguration.m_cropId = value; }
        }

        private static string m_corpSecret = null;
        /// <summary>
        /// 企业Secret
        /// </summary>
        public static string corpSecret
        {
            get { return WeiXinConfiguration.m_corpSecret; }
            set { if (m_corpSecret != null) return; WeiXinConfiguration.m_corpSecret = value; }
        }

        #endregion

    }
}