using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using EG.WeChat.Platform.Model;
using EG.WeChat.Utility.WeiXin;
using EG.WeChat.Utility.Tools;
/*****************************************************
* 目的：企业号基础设置
* 创建人：林子聪
* 创建时间：20150313
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Platform.BL
{
    public class WXCorpBaseService
    {
        /// <summary>
        /// 设置企业号基础配置
        /// </summary>
        public static void SetCorpConfiguration()
        {

            string corpId = string.Empty;
            string corpSecret = string.Empty;
            var pList = GetCorpConfigs<WXCorpInfo>(out corpId, out corpSecret);
            if (pList == null || string.IsNullOrEmpty(corpId) || string.IsNullOrEmpty(corpSecret))
            {
                WeiXinConfiguration.cropId = string.Empty;
                WeiXinConfiguration.corpSecret = string.Empty;
                WeiXinConfiguration.corpInfos = null;
            }
            else
            {
                WeiXinConfiguration.cropId = corpId;
                WeiXinConfiguration.corpSecret = corpSecret;
                WeiXinConfiguration.corpInfos = pList;
            }

        }
        /// <summary>
        /// 微信企业号后台验证地址
        /// </summary>
        /// <param name="msg_signature"></param>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <param name="echostr"></param>
        /// <returns></returns>
        public static string VerifyWXUrl(string msg_signature, string timestamp, string nonce, string echostr)
        {
            if (!CheckCorpConfig())
                return null;

            string op = null;
            WeiXinConfiguration.corpInfos.ForEach((p) =>
            {
                var verifyUrl = Senparc.Weixin.QY.Signature.VerifyURL(p.token, p.aeskey, WeiXinConfiguration.cropId, msg_signature, timestamp, nonce, echostr);
                if (verifyUrl != null)
                    op = verifyUrl;
            });
            return op;
        }
        /// <summary>
        /// 验证企业号配置正常
        /// </summary>
        /// <returns></returns>
        private static bool CheckCorpConfig()
        {
            if (WeiXinConfiguration.corpInfos == null || string.IsNullOrEmpty(WeiXinConfiguration.cropId) || string.IsNullOrEmpty(WeiXinConfiguration.corpSecret))
            {
                WeiXinConfiguration.cropId = string.Empty;
                WeiXinConfiguration.corpSecret = string.Empty;
                WeiXinConfiguration.corpInfos = null;
                return false;
            }
            return true;
        }
        /// <summary>
        /// 更新企业号基础配置
        /// </summary>
        public static void UpdateCorpConfiguration()
        {

        }
        /// <summary>
        /// 获取配置实体并解密
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static List<IWXCorpInfo> GetCorpConfigs<T>(out string corpId, out string corpSecret)
            where T : WXConfigM, IWXCorpInfo
        {
            corpId = string.Empty;
            corpSecret = string.Empty;

            EG.WeChat.Platform.DA.WXConfigDA pDA = new Platform.DA.WXConfigDA();
            DataTable dt = pDA.GetWXConfig();
            if (dt == null || dt.Rows.Count == 0)
                return null;
            var plist = CommonFunction.GetEntitiesFromDataTable<T>(dt);
            if (plist.Count == 0) return null;
            corpId = plist[0].ACID;
            corpSecret = plist[0].ACSECRET;

            var pListOut = plist
                .Where(p => p.aid != null && !string.IsNullOrEmpty(p.token) && !string.IsNullOrEmpty(p.aeskey))
                .Select<T, IWXCorpInfo>(p => p as IWXCorpInfo).ToList();
            if (pListOut.Count == 0) return null;
            return pListOut;
        }
    }
}
