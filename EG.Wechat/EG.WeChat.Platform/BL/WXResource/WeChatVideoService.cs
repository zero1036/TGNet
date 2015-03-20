using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.CommonAPIs;
using System.Net;
using System.Web;
using EG.WeChat.Service;
using EG.WeChat.Utility.Tools;
using EG.WeChat.Platform.Model;
using Senparc.Weixin.MP.AdvancedAPIs.Media;
/*****************************************************
* 目的：微信視頻资源管理服务
* 创建人：林子聪
* 创建时间：20150127
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Platform.BL
{
    public class WeChatVideoService : WeChatResourcesService
    {
        protected string m_strTargetType = "Video";
        protected string m_strFormat = "mp4";
        protected UploadMediaFileType m_UploadMediaFileType = UploadMediaFileType.video;
        private string m_ServerPath = "/Images/WXResources/Video";
        /// <summary>
        /// 上传視頻资源到微信服务器，并写入本地服务器配置文件
        /// 由於視頻需要轉換，需要等待，會引起幷發，需要加入等待處理隊列
        /// </summary>
        /// <param name="pRequest">請求</param>
        /// <param name="lcName">本地名稱</param>
        /// <param name="lcClassify">本地分類</param>
        /// <returns></returns>
        public List<WXVideoResultJson> UpdateResources(HttpRequestBase pRequest, string lcName, string lcClassify)
        {
            List<WXVideoResultJson> pResultList = new List<WXVideoResultJson>();
            this.ExecuteTryCatch(() =>
            {
                //創建視頻轉換事務委託及實體
                DlgMediaResourceConvert pDlg = new DlgMediaResourceConvert(UpdateResourcesQueue);
                MediaConverter pMC = new MediaConverter()
                {
                    Request = pRequest,
                    lcName = lcName,
                    lcClassify = lcClassify,
                    Dlg = pDlg
                };
                //加入等待隊列
                MediaConverterQueue.singlelon.AddQueue(pMC);
                //執行轉換
                pResultList = MediaConverterQueue.singlelon.ExecuteActions();
            });
            return pResultList;
        }
        /// <summary>
        /// 上传視頻资源到微信服务器，并写入本地服务器配置文件
        /// </summary>
        /// <param name="pRequest"></param>
        /// <param name="lcName"></param>
        /// <param name="lcClassify"></param>
        /// <returns></returns>
        public List<WXVideoResultJson> UpdateResourcesQueue(HttpRequestBase pRequest, string lcName, string lcClassify)
        {
            List<WXVideoResultJson> pResultList = new List<WXVideoResultJson>();
            //从请求中加载上传文件——兼容多个上传
            LoadFileFromRequestMulti<WXVideoResultJson>(pRequest, m_ServerPath, pResultList);
            //生成縮略圖
            ConvertAndCreateThumb(pResultList, m_ServerPath, "_stb");
            //从请求中加载上传文件——单个上传
            //LoadFileFromRequest(pRequest, pInputFileName, m_ServerPath, ref pResult);
            foreach (WXVideoResultJson pResult in pResultList)
            {
                pResult.lcName = lcName;
                pResult.lcClassify = lcClassify;
                //更新资源（上传至微信端，并写入本地配置）
                base.UpdateResources<UploadResultJsonX, UploadResultJson, WXVideoResultJson>(pResult, m_UploadMediaFileType, m_strTargetType, pResult.APath);
            }

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
                iLcid = base.DownLoadResources<WXVideoResultJson>(this.m_strTargetType, mediaId, m_strFormat, this.m_ServerPath);
            });
            return iLcid;
        }
        /// <summary>
        /// 读取視頻资源本地配置
        /// </summary>
        /// <param name="iPageIndex"></param>
        /// <param name="iRowCountInPage"></param>
        /// <param name="filterString"></param>
        /// <param name="iRecordCount"></param>
        /// <returns></returns>
        public List<WXVideoResultJson> LoadResources(int iPageIndex, int iRowCountInPage, string filterString, out int iRecordCount)
        {
            List<WXVideoResultJson> pList = null;
            int piRecordCount = 0;
            this.ExecuteTryCatch(() =>
            {
                pList = base.LoadResourcesXForPages<WXVideoResultJson>(m_strTargetType, iPageIndex, iRowCountInPage, filterString, out piRecordCount);
            });
            iRecordCount = piRecordCount;
            return pList;
        }
        /// <summary>
        /// 读取单个視頻资源本地配置
        /// </summary>
        /// <param name="media_id"></param>
        /// <returns></returns>
        public WXVideoResultJson LoadResourcesSingle(string media_id)
        {
            WXVideoResultJson pResult = null;
            this.ExecuteTryCatch(() =>
            {
                //获取配置，并匹配实体集合
                List<WXVideoResultJson> pList = base.LoadResourcesX<WXVideoResultJson>(m_strTargetType);
                pResult = pList.Single(p => p.media_id == media_id);
            });
            return pResult;
        }

    }
}
