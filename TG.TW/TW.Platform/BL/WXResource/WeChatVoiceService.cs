using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.Media;
using Senparc.Weixin.MP.CommonAPIs;
using System.Net;
using System.Web;
using EG.WeChat.Service;
using EG.WeChat.Utility.Tools;
using EG.WeChat.Utility.WeiXin;
using TW.Platform.Model;
/*****************************************************
* 目的：微信音頻资源管理服务
* 创建人：林子聪
* 创建时间：20150129
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace TW.Platform.BL
{
    public class WeChatVoiceService : WeChatResourcesService
    {
        protected string m_strTargetType = "voice";
        protected string m_strFormat = "amr";
        //protected UploadMediaFileType m_UploadMediaFileType = UploadMediaFileType.voice;
        private string m_ServerPath = "/Images/WXResources/Voice";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sdkType"></param>
        public WeChatVoiceService(string sdkType)
            : base(sdkType)
        { }
        /// <summary>
        /// 上传音頻资源到微信服务器，并写入本地服务器配置文件
        /// </summary>
        /// <param name="pRequest">請求</param>
        /// <param name="lcName">本地名稱</param>
        /// <param name="lcClassify">本地分類</param>
        /// <returns></returns>
        public List<WXVoiceResultJson> UpdateResources(HttpRequestBase pRequest, string lcName, string lcClassify)
        {
            List<WXVoiceResultJson> pResultList = new List<WXVoiceResultJson>();
            this.ExecuteTryCatch(() =>
            {
                //从请求中加载上传文件——兼容多个上传
                LoadFileFromRequestMulti<WXVoiceResultJson>(pRequest, m_ServerPath, pResultList);

                foreach (WXVoiceResultJson pResult in pResultList)
                {
                    pResult.lcName = lcName;
                    pResult.lcClassify = lcClassify;
                    //更新资源（上传至微信端，并写入本地配置）
                    base.UpdateResources<WXVoiceResultJson>(pResult, m_strTargetType, pResult.APath);
                }
            });
            return pResultList;
        }
        /// <summary>
        /// 下載微信接收資源
        /// </summary>
        /// <param name="mediaId"></param>
        public int DownLoadResources(string mediaId)
        {
            int iLcid = -1;
            this.ExecuteTryCatch(() =>
            {
                iLcid = base.DownLoadResources<WXVoiceResultJson>(this.m_strTargetType, mediaId, m_strFormat, this.m_ServerPath);
            });
            return iLcid;
        }
        /// <summary>
        /// 读取音頻資源本地配置
        /// </summary>
        /// <param name="iPageIndex"></param>
        /// <param name="iRowCountInPage"></param>
        /// <param name="filterString"></param>
        /// <param name="iRecordCount"></param>
        /// <returns></returns>
        public List<WXVoiceResultJson> LoadResources(int iPageIndex, int iRowCountInPage, string filterString, out int iRecordCount)
        {
            List<WXVoiceResultJson> pList = null;
            int piRecordCount = 0;
            this.ExecuteTryCatch(() =>
            {
                pList = base.LoadResourcesXForPages<WXVoiceResultJson>(m_strTargetType, iPageIndex, iRowCountInPage, filterString, out piRecordCount);
            });
            iRecordCount = piRecordCount;
            return pList;
        }
        /// <summary>
        /// 读取单个音頻资源本地配置
        /// </summary>
        /// <param name="media_id"></param>
        /// <returns></returns>
        public WXVoiceResultJson LoadResourcesSingle(string media_id)
        {
            WXVoiceResultJson pResult = null;
            this.ExecuteTryCatch(() =>
            {
                //获取配置，并匹配实体集合
                List<WXVoiceResultJson> pList = base.LoadResourcesX<WXVoiceResultJson>(m_strTargetType);
                pResult = pList.Single(p => p.media_id == media_id);
            });
            return pResult;
        }
        /// <summary>
        /// 读取单个音頻资源本地配置
        /// </summary>
        /// <param name="media_id"></param>
        /// <returns></returns>
        public WXVoiceResultJson LoadResourcesSingleBylcId(int lcid)
        {
            WXVoiceResultJson pResult = null;
            this.ExecuteTryCatch(() =>
            {
                //获取配置，并匹配实体集合
                List<WXVoiceResultJson> pList = base.LoadResourcesX<WXVoiceResultJson>(m_strTargetType);
                pResult = pList.Single(p => p.lcId == lcid);
            });
            return pResult;
        }
    }
}
