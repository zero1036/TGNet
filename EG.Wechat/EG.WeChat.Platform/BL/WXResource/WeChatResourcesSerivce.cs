using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Senparc.Weixin.Exceptions;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.CommonAPIs;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Utilities;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Xml;
using EG.WeChat.Service;
using System.Web;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin;
using System.IO;
using System.Net;
using Senparc.Weixin.Helpers;
using System.Data;
using EG.WeChat.Platform.DA;
using EG.WeChat.Utility.Tools;
using EG.WeChat.Platform.Model;
using System.Web.Caching;
/*****************************************************
* 目的：微信资源管理（图文、图片、音频、视频）服务
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
    /// 微信资源基础服务
    /// </summary>
    public class WeChatResourcesService : IServiceX, IServiceXCache
    {
        #region 成员
        /// <summary>
        /// 微信分组缓存项
        /// </summary>
        public WXResourceCacheConfig m_CacheConfig;
        /// <summary>
        /// 数据访问DAL
        /// </summary>
        private WXResourceDA m_DA;
        #endregion

        #region 派生类接口

        #region Update
        /// <summary>
        /// 上传资源到微信服务器，并写入Database资源配置表
        /// </summary>
        /// <typeparam name="SDKResult"></typeparam>
        /// <typeparam name="EGResult"></typeparam>
        /// <param name="pWCResultJson"></param>
        /// <param name="pType"></param>
        /// <param name="strTargetType"></param>
        /// <param name="objResouceValue"></param>
        protected void UpdateResources<SDKResultX, SDKResult, EGResult>(EGResult pWCResultJson, UploadMediaFileType pType, string strTargetType, object objResouceValue)
            where EGResult : IWXResultJon1, ILCResultJon, new()
            where SDKResultX : IWXResultJon1, IWXResultJon2<SDKResult>, new()
        {
            try
            {
                //将上传文件至微信服务器
                object ova = UploadToWX(pType, objResouceValue);
                //上传结果写入本地结果中
                var pResult = new SDKResultX();
                pResult.UploadResultJson = (SDKResult)ova;
                //SDKResult pResult = (SDKResult)ova;
                pWCResultJson.created_at = pResult.created_at;
                pWCResultJson.media_id = pResult.media_id;
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("because it is being used by another process"))
                {
                    throw ex;
                }
            }
            finally
            {
                //将上传文件的UploadResultJson转换成json
                string strValue = CommonFunction.ConvertToJson<EGResult>(pWCResultJson);
                //保存微信资源至DB
                int lcid = SaveWXResource(pWCResultJson.lcId, pWCResultJson.lcName, pWCResultJson.lcClassify, strTargetType, strValue, pWCResultJson.media_id, (int)EnumWXSourceType.local);
            }
        }
        /// <summary>
        /// 写入Database资源配置表
        /// </summary>
        /// <typeparam name="EGResult"></typeparam>
        /// <param name="strTargetType"></param>
        /// <param name="strMediaID"></param>
        /// <param name="objResouceValue"></param>
        /// <param name="pEnum"></param>
        /// <param name="lcName"></param>
        /// <param name="lcClassify"></param>
        /// <returns></returns>
        protected int UpdateResources<EGResult>(string strTargetType, string strMediaID, EGResult objResouceValue, EnumWXSourceType pEnum = EnumWXSourceType.local, string lcName = "", string lcClassify = "")
        {
            //将实体转换为Json文本
            string strValue = CommonFunction.ConvertToJson<EGResult>(objResouceValue);
            //保存微信资源至DB
            return SaveWXResource(null, string.Empty, string.Empty, strTargetType, strValue, strMediaID, (int)pEnum);
        }
        /// <summary>
        /// 下載微信接收資源至本地
        /// </summary>
        /// <param name="mediaId"></param>
        /// <param name="pFormat"></param>
        /// <param name="pServerPath"></param>
        protected int DownLoadResources<EGResult>(string strTargetType, string mediaId, string pFormat, string pServerPath, string lcName = "", string lcClassify = "")
            where EGResult : IWXResultJon1, ILCResultJon, IFileUploadResult, new()
        {
            string pFileName = DateTime.Now.Ticks.ToString();
            string pPathA = pServerPath.Replace("/", @"\");
            pPathA = System.Web.HttpContext.Current.Server.MapPath(pPathA);
            pPathA = string.Format(@"{0}\{1}.{2}", pPathA, pFileName, pFormat);
            string pPathR = string.Format("{0}/{1}.{2}", pServerPath, pFileName, pFormat);
            //下載資源至本地
            DownloadToLC(mediaId, pPathA);
            //創建實體
            EGResult pRJ = new EGResult();
            pRJ.media_id = mediaId;
            pRJ.APath = pPathA;
            pRJ.RPath = pPathR;
            pRJ.FileName = pFileName;
            pRJ.FormatName = pFormat;
            pRJ.lcName = lcName;
            pRJ.lcClassify = lcClassify;
            //写入Database资源配置表
            return UpdateResources<EGResult>(strTargetType, mediaId, pRJ, EnumWXSourceType.wechatLoad, lcName, lcClassify);
        }
        #endregion

        #region Load
        /// <summary>
        /// 通过微信类型，获取微信资源——旧有方式
        /// </summary>
        /// <param name="strTargetType"></param>
        /// <returns></returns>
        protected List<T> LoadResources<T>(string strTargetType, string keyWord = "", bool? bLose = null, int? iSourceType = null)
             where T : new()
        {
            DataTable dt = GetWXResourcesFromCache(strTargetType);
            if (dt == null || dt.Rows.Count == 0)
                return new List<T>();

            //根據條件過濾
            IEnumerable<DataRow> pRows = FilterResourceTable(dt, keyWord, bLose, iSourceType);

            //遍历DataTable，取出所有的资源模板Json文本结合
            List<string> pList = (from d in pRows
                                  select d.Field<string>(WXResourceDA.FIELD_NAME_CONTENT)).ToList<string>();
            //json文本集合转换为实体集合
            return CommonFunction.FromJsonListToEntityList<T>(pList);
        }
        /// <summary>
        /// 通过微信类型，获取微信资源——新方式
        /// </summary>
        /// <param name="strTargetType"></param>
        /// <returns></returns>
        protected List<T> LoadResourcesX<T>(string strTargetType, string keyWord = "", bool? bLose = null, int? iSourceType = null)
             where T : ILCResultJon, new()
        {
            DataTable dt = GetWXResourcesFromCache(strTargetType);
            if (dt == null || dt.Rows.Count == 0)
                return new List<T>();

            //根據條件過濾
            IEnumerable<DataRow> pRows = FilterResourceTable(dt, keyWord, bLose, iSourceType);

            //遍历DataTable，取出所有的资源模板Json文本结合
            List<string> pList = (from d in pRows
                                  select d.Field<string>(WXResourceDA.FIELD_NAME_CONTENT)).ToList<string>();

            List<int> pListLCID = (from d in pRows
                                   select d.Field<int>(WXResourceDA.FIELD_NAME_LCID)).ToList<int>();

            //json文本集合转换为实体集合
            List<T> pEn = CommonFunction.FromJsonListToEntityList<T>(pList);

            for (int i = 0; i <= pEn.Count - 1; i++)
            {
                pEn[i].lcId = pListLCID[i];
            }
            return pEn;
        }
        /// <summary>
        /// 分頁
        /// </summary>
        /// <typeparam name="EGResult"></typeparam>
        /// <param name="strTargetType"></param>
        /// <param name="iPageIndex"></param>
        /// <param name="iRowCountInPage"></param>
        /// <param name="filterString"></param>
        /// <param name="iRecordCount"></param>
        /// <returns></returns>
        protected List<EGResult> LoadResourcesXForPages<EGResult>(string strTargetType, int iPageIndex, int iRowCountInPage, string filterString, out int iRecordCount)
          where EGResult : ILCResultJon, new()
        {
            List<EGResult> pList;
            if (!string.IsNullOrEmpty(filterString))
            {
                //转换前端查询目标
                List<object> pItems = CommonFunction.FromJsonTo<List<object>>(filterString);
                var o0 = pItems[0] != null ? pItems[0].ToString() : "";
                var o1 = pItems[1] != null ? (bool?)pItems[1] : null;
                var o2 = pItems[2] != null ? (int?)pItems[2] : null;
                pList = LoadResourcesX<EGResult>(strTargetType, o0, o1, o2);
            }
            else
            {
                pList = LoadResourcesX<EGResult>(strTargetType);
            }

            if (pList == null || pList.Count == 0)
            {
                EGExceptionOperator.ThrowX<Exception>("沒有找到資源哦，點擊‘新建’按鈕添加！", EGActionCode.缺少目标数据);
            }
            //查询目标总记录数
            iRecordCount = pList.Count;

            ////受限总共显示记录数，由服务端控制，暂时写死2000条，现行最大记录可以去到1万条json记录，扩展jsonresult可以去到50万条
            //pList = CommonFunction.SubListForTable<WeChatUser>(pList, 2000);
            //筛选目标页记录数
            return CommonFunction.SubListForTable<EGResult>(pList, iPageIndex, iRowCountInPage);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 刪除資源
        /// </summary>
        /// <param name="strTargetType"></param>
        /// <param name="lcid"></param>
        public void DeleteResource(string strTargetType, int lcid)
        {
            if (string.IsNullOrEmpty(strTargetType) || lcid < 1)
                return;

            m_CacheConfig = new WXResourceCacheConfig(strTargetType);
            //初始化数据访问DA
            m_DA = CastleAOPUtil.NewPxyByClass<WXResourceDA>(new DataWritingInterceptor(this.RemoveCache, this.m_CacheConfig));
           
            int iRes = m_DA.DeleteResource(lcid);
            if (lcid == null)
                EGExceptionOperator.ThrowX<Exception>("微信资源刪除错误", EGActionCode.未知错误);
        }
        #endregion

        #region Others
        /// <summary>
        /// 从请求中加载上传文件
        /// </summary>
        /// <param name="pRequest"></param>
        /// <param name="pInputFileName"></param>
        /// <param name="pServerPath"></param>
        /// <returns></returns>
        protected void LoadFileFromRequest(HttpRequestBase pRequest, string pInputFileName, string pServerPath, ref WXPictureResultJson pResult)
        {
            //定义错误消息
            string msg = "";
            //接受上传文件
            HttpPostedFileBase hp = pRequest.Files[pInputFileName];
            if (hp == null)
            {
                msg = "请选择文件";
                EGExceptionOperator.ThrowX<Exception>(msg, EGActionCode.缺少必要参数);
            }
            //获取上传目录 转换为物理路径
            string uploadPath = System.Web.HttpContext.Current.Server.MapPath(pServerPath);
            //获取文件名
            string fileName = DateTime.Now.Ticks.ToString() + System.IO.Path.GetExtension(hp.FileName);
            //获取文件大小
            long contentLength = hp.ContentLength;
            //文件不能大于1M
            if (contentLength > 1024 * 1024)
            {
                msg = "文件大小超过限制要求.";
                EGExceptionOperator.ThrowX<Exception>(msg, EGActionCode.输入参数错误);
            }
            if (!checkFileExtension(fileName))
            {
                msg = "文件为不可上传的类型.";
                EGExceptionOperator.ThrowX<Exception>(msg, EGActionCode.输入参数错误);
            }
            //保存文件的物理路径
            string saveFile = uploadPath + fileName;
            try
            {
                //保存文件
                hp.SaveAs(saveFile);
                //写入绝对路径
                pResult.APath = saveFile;
                //写入相对路径
                pResult.RPath = pServerPath + fileName;
                //写入文件名称
                pResult.FileName = fileName;
                //msg = saveFile;
                //msg = pServerPath + fileName;
            }
            catch
            {
                msg = "图片文件上传失败";
                EGExceptionOperator.ThrowX<Exception>(msg, EGActionCode.文件上传失败);
            }
            //return msg;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Request"></param>
        /// <param name="pServerPath"></param>
        /// <param name="pResult"></param>
        protected void LoadFileFromRequestMulti<T>(HttpRequestBase Request, string pServerPath, List<T> pResult, bool pUseRealName = false)
            where T : IFileUploadResult, new()
        {
            //if (pServerPath.StartsWith("/"))
            //    pServerPath = pServerPath.Substring(1, pServerPath.Length - 1);
            //   Images/WXResources/
            string pPathR = pServerPath;
            //   Images\WXResources\
            string pPathA = pServerPath.Replace("/", @"\");
            // xxx
            string pFileName = string.Empty;
            // mp4
            string pFormatName = string.Empty;

            T pT;
            foreach (string fileName in Request.Files)
            {
                //创建新对象
                pT = new T();

                HttpPostedFileBase file = Request.Files[fileName];
                if (file == null)
                {
                    continue;
                    //msg = "请选择文件";
                    //EGExceptionOperator.ThrowX<Exception>(msg, EGActionCode.缺少必要参数);
                }
                //Save file content goes here
                ////使用源名稱
                //if (pUseRealName)
                //{
                //    pT.FileName = file.FileName;
                //}
                //else
                //{
                //    pT.FileName = DateTime.Now.Ticks.ToString();
                //}
                if (file != null && file.ContentLength > 0)
                {
                    // D:\EM_Project\WeChat\Standard\trunk\EG.WeChat.Standard\EG.WeChat.Web\Images\WXResources
                    var originalDirectory = new DirectoryInfo(string.Format("{0}", System.Web.HttpContext.Current.Server.MapPath(pPathA)));

                    // D:\EM_Project\WeChat\Standard\trunk\EG.WeChat.Standard\EG.WeChat.Web\Images\WXResources
                    //string pathString = System.IO.Path.Combine(originalDirectory.ToString(), "WXResources");
                    string pathString = originalDirectory.ToString();

                    //xxxx.mp4
                    var fileName1 = Path.GetFileName(file.FileName);
                    // xxxx
                    pFileName = fileName1.Substring(fileName1.LastIndexOf("\\") + 1, (fileName1.LastIndexOf(".") - fileName1.LastIndexOf("\\") - 1));
                    // mp4
                    pFormatName = fileName1.Substring(fileName1.LastIndexOf(".") + 1, (fileName1.Length - fileName1.LastIndexOf(".") - 1));   //扩展名
                    // 234878234312.mp4 如果不使用源名稱，則使用datetime Ticks作為名稱
                    if (!pUseRealName)
                    {
                        pFileName = DateTime.Now.Ticks.ToString();
                        fileName1 = string.Format("{0}.{1}", pFileName, pFormatName);
                    }

                    bool isExists = System.IO.Directory.Exists(pathString);

                    if (!isExists)
                        System.IO.Directory.CreateDirectory(pathString);

                    var path = string.Format("{0}\\{1}", pathString, fileName1);

                    try
                    {
                        file.SaveAs(path);
                    }
                    catch
                    {
                        //msg = "文件上传失败";
                        //EGExceptionOperator.ThrowX<Exception>(msg, EGActionCode.文件上传失败);
                        continue;
                    }

                    pT.APath = path;
                    pT.RPath = string.Format("{0}/{1}", pPathR, fileName1);
                    pT.FileName = pFileName;
                    pT.FormatName = pFormatName;

                    pResult.Add(pT);
                }

            }
            if (pResult == null || pResult.Count == 0)
            {
                string msg = "文件上传失败";
                EGExceptionOperator.ThrowX<Exception>(msg, EGActionCode.文件上传失败);
            }

        }
        /// <summary>
        /// 創建縮略圖
        /// </summary>
        /// <typeparam name="T">IFileUploadResult</typeparam>
        /// <param name="pResults">文件集合</param>
        /// <param name="serverRPath">服務器存放相對路徑</param>
        /// <param name="thumbHeadName">縮略圖頭名稱</param>
        protected void ConvertAndCreateThumb<T>(List<T> pResults, string serverRPath, string thumbHeadName)
         where T : IFileUploadResult, new()
        {
            string strThumbFileName = string.Empty;
            string serverAPath = serverRPath.Replace("/", @"\");
            //轉換操作
            FormatConverter pCon = new FormatConverter();
            foreach (T pT in pResults)
            {
                // 25487815728572_stb.mp4
                strThumbFileName = string.Format("{0}{1}", pT.FileName, thumbHeadName);
                //
                string err = pCon.Convert(pT.APath, serverAPath, strThumbFileName, FormatConverter.VideoType.MP4, true, true);
                if (!string.IsNullOrEmpty(err))
                {
                    EGExceptionOperator.ThrowX<Exception>(err, EGActionCode.文件上传失败);
                }
                else
                {
                    pT.APath = pT.APath.Replace(pT.FileName, strThumbFileName);
                    pT.FileName = strThumbFileName;
                    pT.RPath = string.Format("{0}/{1}.{2}", serverRPath, strThumbFileName, pT.FormatName);
                    pT.RPathThumb = string.Format("{0}/{1}.{2}", serverRPath, strThumbFileName, "jpg");
                }
            }
        }
        #endregion

        #endregion

        #region 私有方法
        /// <summary>
        /// 检查文件后缀名是否符合要求
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private bool checkFileExtension(string fileName)
        {
            //获取文件后缀
            string fileExtension = System.IO.Path.GetExtension(fileName);
            if (fileExtension != null)
                fileExtension = fileExtension.ToLower();
            else
                return false;

            if (fileExtension != ".jpg" && fileExtension != ".gif" && fileExtension != ".png")
                return false;

            return true;
        }
        /// <summary>
        /// 当前缓存滑动清空后，自动重新加载并写入缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="vvalue"></param>
        /// <param name="r"></param>
        private void CacheRemovedCallback(String key, Object vvalue, CacheItemRemovedReason r)
        {
            if (r == CacheItemRemovedReason.Expired)
            {
                //GetWXGroupsFromDB();
            }
        }
        /// 將资源通过SDK接口上传至微信服务器
        /// </summary>
        /// <param name="pType"></param>
        /// <param name="objResouceValue"></param>
        /// <returns></returns>
        private object UploadToWX(UploadMediaFileType pType, object objResouceValue)
        {
            //获取AccessToken
            string strAccessToken = Senparc.Weixin.MP.CommonAPIs.WeiXinSDKExtension.GetCurrentAccessToken();
            if (pType == UploadMediaFileType.news)
                return Senparc.Weixin.MP.AdvancedAPIs.Media.UploadNews(strAccessToken, (NewsModel[])objResouceValue);
            else
                //将上传文件至微信服务器
                return Senparc.Weixin.MP.AdvancedAPIs.Media.Upload(strAccessToken, pType, (string)objResouceValue);
        }
        /// <summary>
        /// 從微信服務器下載資源至本地
        /// </summary>
        /// <param name="mediaId"></param>
        /// <param name="pPathA"></param>
        /// <returns></returns>
        private void DownloadToLC(string mediaId, string pPathA)
        {
            //获取AccessToken
            string strAccessToken = Senparc.Weixin.MP.CommonAPIs.WeiXinSDKExtension.GetCurrentAccessToken();

            this.ExecuteTryCatch(() =>
            {
                using (FileStream pStream = new FileStream(pPathA, FileMode.OpenOrCreate, FileAccess.Write))
                {

                    Senparc.Weixin.MP.AdvancedAPIs.Media.Get(strAccessToken, mediaId, pStream);
                    pStream.Flush();
                    pStream.Close();
                }
            });
        }
        /// <summary>
        /// 過濾資源表，無法動態，遺憾
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="keyWord"></param>
        /// <param name="bLose"></param>
        /// <param name="iSourceType"></param>
        /// <returns></returns>
        private IEnumerable<DataRow> FilterResourceTable(DataTable dt, string keyWord, bool? bLose, int? iSourceType)
        {
            EnumerableRowCollection<DataRow> pRows = dt.AsEnumerable();
            if (!string.IsNullOrEmpty(keyWord))
                pRows = pRows.Where(
                    p =>
                        p[WXResourceDA.FIELD_NAME_LCNAME].ToString().Contains(keyWord) ||
                        p[WXResourceDA.FIELD_NAME_LCCLASSIFY].ToString().Contains(keyWord)
                    );

            if (bLose != null)
                pRows = pRows.Where(
                   p =>
                       p[WXResourceDA.FIELD_NAME_LOSE].Equals(bLose)
                   );

            if (iSourceType != null)
                pRows = pRows.Where(
                   p =>
                       p[WXResourceDA.FIELD_NAME_SOURCE_TYPE].Equals(iSourceType)
                   );

            return pRows;

        }
        /// <summary>
        /// 從緩存中讀取資源表
        /// </summary>
        /// <param name="strTargetType"></param>
        /// <returns></returns>
        private DataTable GetWXResourcesFromCache(string strTargetType)
        {
            m_CacheConfig = new WXResourceCacheConfig(strTargetType);
            object obj = this.GetCache(m_CacheConfig);
            if (obj == null)
            {
                //初始化数据访问DA
                m_DA = new WXResourceDA();
                DataTable dt = m_DA.GetWXResources(strTargetType);
                m_CacheConfig.CacheContent = dt;
                this.InsertCache(m_CacheConfig, this.CacheRemovedCallback);

                return dt;
            }
            else
            {
                return obj as DataTable;
            }
        }
        /// <summary>
        /// 保存微信资源至DB
        /// </summary>
        /// <param name="strTargetType"></param>
        /// <param name="strValue"></param>
        /// <param name="media_ID"></param>
        private int SaveWXResource(int? lcid, string lcName, string lcClassify, string strTargetType, string strValue, string media_ID, int iSourceType)
        {
            m_CacheConfig = new WXResourceCacheConfig(strTargetType);
            //初始化数据访问DA
            m_DA = CastleAOPUtil.NewPxyByClass<WXResourceDA>(new DataWritingInterceptor(this.RemoveCache, this.m_CacheConfig));
            if (lcid == 0)
                lcid = null;

            //保存资源——暂时不能分类及命名
            lcid = m_DA.SaveResource(lcid, lcName, lcClassify, media_ID, strTargetType, strValue, DateTime.Now, iSourceType);
            if (lcid == null)
                EGExceptionOperator.ThrowX<Exception>("微信资源DataBase保存错误", EGActionCode.未知错误);
            return lcid.Value;
        }
        #endregion
    }

}
