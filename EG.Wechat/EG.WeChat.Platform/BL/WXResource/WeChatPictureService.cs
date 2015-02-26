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
/*****************************************************
* 目的：微信图片资源管理服务
* 创建人：林子聪
* 创建时间：20141124
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Platform.BL
{
    /// <summary>
    /// 微信图片资源服务
    /// </summary>
    public class WeChatPictureService : WeChatResourcesService
    {
        protected string m_strTargetType = "Picture";
        protected string m_strFormat = "jpg";
        protected UploadMediaFileType m_UploadMediaFileType = UploadMediaFileType.image;
        private string m_ServerPath = "/Images/WXResources/Image";
        /// <summary>
        /// 上传图片资源到微信服务器，并写入本地服务器配置文件
        /// </summary>
        /// <param name="pRequest"></param>
        /// <param name="pInputFileName"></param>
        /// <returns></returns>
        public List<WXPictureResultJson> UpdateResources(HttpRequestBase pRequest, string pInputFileName)
        {
            List<WXPictureResultJson> pResultList = new List<WXPictureResultJson>();
            this.ExecuteTryCatch(() =>
            {
                //从请求中加载上传文件——兼容多个上传
                LoadFileFromRequestMulti<WXPictureResultJson>(pRequest, m_ServerPath, pResultList);
                //从请求中加载上传文件——单个上传
                //LoadFileFromRequest(pRequest, pInputFileName, m_ServerPath, ref pResult);
                foreach (WXPictureResultJson pResult in pResultList)
                {
                    //更新资源（上传至微信端，并写入本地配置）
                    base.UpdateResources<UploadResultJsonX, UploadResultJson, WXPictureResultJson>(pResult, m_UploadMediaFileType, m_strTargetType, pResult.APath);
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
                iLcid = base.DownLoadResources<WXPictureResultJson>(this.m_strTargetType, mediaId, m_strFormat, this.m_ServerPath);
            });
            return iLcid;
        }
        /// <summary>
        /// 读取图片资源本地配置
        /// </summary>
        /// <param name="strTargetType"></param>
        /// <returns></returns>
        public List<WXPictureResultJson> LoadResources(int iPageIndex, int iRowCountInPage, string filterString, out int iRecordCount)
        {
            List<WXPictureResultJson> pList = null;
            int piRecordCount = 0;
            this.ExecuteTryCatch(() =>
            {
                pList = base.LoadResourcesXForPages<WXPictureResultJson>(m_strTargetType, iPageIndex, iRowCountInPage, filterString, out piRecordCount);
            });
            iRecordCount = piRecordCount;
            return pList;
        }
        /// <summary>
        /// 读取单个图片资源本地配置
        /// </summary>
        /// <param name="media_id"></param>
        /// <returns></returns>
        public WXPictureResultJson LoadResourcesSingle(string media_id)
        {
            WXPictureResultJson pResult = null;
            this.ExecuteTryCatch(() =>
            {
                //获取配置，并匹配实体集合
                List<WXPictureResultJson> pList = base.LoadResourcesX<WXPictureResultJson>(m_strTargetType);
                pResult = pList.Single(p => p.media_id == media_id);
            });
            return pResult;
        }

    }
}
