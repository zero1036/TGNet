using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EG.WeChat.Platform.DA;
/*****************************************************
* 目的：微信基础配置表模型
* 创建人：林子聪
* 创建时间：20150313
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Platform.Model
{
    public class WXConfigM
    {
        #region 属性
        /// <summary>
        /// 自编号ID
        /// </summary>
        public int ID
        { get; set; }
        /// <summary>
        /// 公众号/企业号类型
        /// </summary>
        public int ACTYPE
        { get; set; }
        /// <summary>
        /// 公众号/企业号编号
        /// </summary>
        public string ACID
        { get; set; }
        /// <summary>
        /// 公众号/企业号Secret
        /// </summary>
        public string ACSECRET
        { get; set; }
        /// <summary>
        /// TOKEN
        /// </summary>
        public string TOKEN
        { get; set; }
        /// <summary>
        /// AESKey
        /// </summary>
        public string AESKEY
        { get; set; }
        /// <summary>
        /// 企业号应用ID
        /// </summary>
        public int AID
        { get; set; }
        #endregion
    }

    public class WXCorpInfo : WXConfigM, EG.WeChat.Utility.WeiXin.IWXCorpInfo, EG.WeChat.Platform.BL.IServiceX
    {
        private int? _aid = null;
        public int aid
        {
            get
            {
                if (_aid == null)
                {
                    _aid = base.AID;
                }
                return _aid.Value;
            }
        }
        private bool _hasToken = false;
        private string _token = string.Empty;
        public string token
        {
            get
            {
                if (!_hasToken)
                {
                    try
                    {
                        _token = Emperor.UtilityLib.CyberUtils.Decrypt("Aes", 256, base.TOKEN, WXConfigDA.FIELD_NAME_TOKEN);
                        _hasToken = true;
                    }
                    catch { }
                }
                return _token;
            }
            set { _token = value; }
        }
        private bool _hasAesKey = false;
        private string _aeskey = string.Empty;
        public string aeskey
        {
            get
            {
                if (!_hasAesKey)
                {
                    try
                    {
                        _aeskey = Emperor.UtilityLib.CyberUtils.Decrypt("Aes", 256, base.AESKEY, WXConfigDA.FIELD_NAME_AESKEY);
                        _hasAesKey = true;
                    }
                    catch { }
                }
                return _aeskey;
            }
            set { _aeskey = value; }
        }
        public string aname
        {
            get;
            set;
        }
        public string round_logo_url
        {
            get;
            set;
        }
    }
}
