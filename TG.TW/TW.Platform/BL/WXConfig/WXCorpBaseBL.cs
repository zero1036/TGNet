using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using TW.Platform.Model;
using EG.WeChat.Utility.WeiXin;
using EG.WeChat.Utility.Tools;
using TW.Platform.DA;
using Senparc.Weixin.Entities;
using Senparc.Weixin.QY.Entities;
using Senparc.Weixin.QY.Entities.Menu;
using Senparc.Weixin.QY.CommonAPIs;
using Senparc.Weixin.QY.AdvancedAPIs.App;
/*****************************************************
* 目的：企业号基础设置
* 创建人：林子聪
* 创建时间：20150313
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace TW.Platform.BL
{
    public class WXCorpBaseBL : IServiceX
    {
        #region public
        /// <summary>
        /// 设置企业号基础配置
        /// </summary>
        public void SetCorpConfiguration()
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
        public string VerifyWXUrl(string msg_signature, string timestamp, string nonce, string echostr)
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
        private bool CheckCorpConfig()
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
        public void UpdateCorpConfiguration(int aid, string token, string aeskey)
        {
            this.ExecuteTryCatch(() =>
            {
                TW.Platform.DA.WXConfigDA pDA = new Platform.DA.WXConfigDA();
                DataTable dt = pDA.GetWXConfig();
                if (dt == null || dt.Rows.Count == 0)
                    EGExceptionOperator.ThrowX<Exception>("缺少企業應用配置信息", EGActionCode.数据库表保存错误);
                var plist = CommonFunction.GetEntitiesFromDataTable<WXConfigM>(dt);
                if (plist.Count == 0)
                    EGExceptionOperator.ThrowX<Exception>("缺少企業應用配置信息", EGActionCode.数据库表保存错误);
                var pmodel = plist.Single(p => p.AID == aid && p.ACTYPE == 2);
                if (pmodel == null)
                {
                    //Emperor.UtilityLib.CyberUtils.Decrypt("Aes", 256, base.TOKEN, WXConfigDA.FIELD_NAME_TOKEN);
                }
                else
                {
                    pmodel.TOKEN = Emperor.UtilityLib.CyberUtils.Encrypt("Aes", 256, token, WXConfigDA.FIELD_NAME_TOKEN);
                    pmodel.AESKEY = Emperor.UtilityLib.CyberUtils.Encrypt("Aes", 256, aeskey, WXConfigDA.FIELD_NAME_AESKEY);
                }
                var ps = new List<WXConfigM>();
                ps.Add(pmodel);
                DataTable dto = CommonFunction.GetDataTableFromEntities<WXConfigM>(ps);
                if (pDA.SetWXConfig(dto))
                {
                    WeiXinConfiguration.corpInfos.ForEach((p) =>
                    {
                        if (p.aid == aid)
                        {
                            p.token = token;
                            p.aeskey = aeskey;
                        }
                    });
                }
                else { EGExceptionOperator.ThrowX<Exception>("缺少企業應用配置信息", EGActionCode.数据库表保存错误); }

            });
        }
        /// <summary>
        /// 获取企业号应用集合
        /// </summary>
        public List<GetAppInfoResult> GetCorpInfos()
        {
            var pList = new List<GetAppInfoResult>();
            this.ExecuteTryCatch(() =>
            {
                //获取AccessToken
                string strAccessToken = Senparc.Weixin.MP.CommonAPIs.WeiXinSDKExtension.GetCurrentAccessTokenQY();
                WeiXinConfiguration.corpInfos.ForEach((pCorp) =>
                {
                    var papps = AppApi.GetAppInfo(strAccessToken, pCorp.aid);
                    pList.Add(papps);
                    pCorp.aname = papps.name;
                    pCorp.round_logo_url = papps.round_logo_url;
                });
            });
            return pList;
        }
        /// <summary>
        /// 获取企业应用菜单
        /// </summary>
        /// <param name="agentid"></param>
        /// <returns></returns>
        public GetMenuResult GetQYAppMenu(int agentid)
        {
            GetMenuResult pMenu = null;
            this.ExecuteTryCatch(() =>
            {
                //获取AccessToken
                string strAccessToken = Senparc.Weixin.MP.CommonAPIs.WeiXinSDKExtension.GetCurrentAccessTokenQY();
                pMenu = Senparc.Weixin.QY.CommonAPIs.CommonApi.GetMenu(strAccessToken, agentid);
            });
            return pMenu;
        }
        /// <summary>
        /// 设置企业应用菜单
        /// </summary>
        /// <param name="agentid"></param>
        /// <param name="svl"></param>
        public void SetQYAppMenu(int agentid, string svl)
        {
            this.ExecuteTryCatch(() =>
            {
                //object obj = CommonFunction.FromJsonTo<object>(svl);
                //ButtonGroup bg = obj as ButtonGroup;
                //获取AccessToken
                string strAccessToken = Senparc.Weixin.MP.CommonAPIs.WeiXinSDKExtension.GetCurrentAccessTokenQY();
                //CommonApi.CreateMenu(strAccessToken, agentid, bg);


                var urlFormat = string.Format("https://qyapi.weixin.qq.com/cgi-bin/menu/create?access_token={0}&agentid={1}", strAccessToken, agentid);

                CommonJsonSendX.Send(null, urlFormat, svl, CommonJsonSendType.POST);
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strAccessToken"></param>
        /// <param name="agentid"></param>
        /// <returns></returns>
        public object sendX(string strAccessToken, string agentid)
        {
            var url = string.Format("https://qyapi.weixin.qq.com/cgi-bin/agent/get?access_token={0}&agentid={1}", strAccessToken, agentid);

            return CommonJsonSend.Send<object>(strAccessToken, url, null, CommonJsonSendType.GET);
        }
        #endregion

        #region private
        /// <summary>
        /// 获取配置实体并解密
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private List<IWXCorpInfo> GetCorpConfigs<T>(out string corpId, out string corpSecret)
            where T : WXConfigM, IWXCorpInfo
        {
            corpId = string.Empty;
            corpSecret = string.Empty;

            TW.Platform.DA.WXConfigDA pDA = new Platform.DA.WXConfigDA();
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
        #endregion
    }

}
