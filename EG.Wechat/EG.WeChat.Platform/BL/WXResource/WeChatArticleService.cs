using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.GroupMessage;
using Senparc.Weixin.MP.CommonAPIs;
using System.Net;
using System.Web;
using EG.WeChat.Service;
using EG.WeChat.Utility.Tools;
using EG.WeChat.Utility.WeiXin;
using EG.WeChat.Platform.Model;
using System.Web.Caching;
/*****************************************************
* 目的：微信图文资源管理服务
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
    /// 微信图文资源服务
    /// </summary>
    public class WeChatArticleService : WeChatResourcesService
    {
        protected string m_strTargetType = "news";
        protected UploadMediaFileType m_UploadMediaFileType = UploadMediaFileType.news;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sdkType"></param>
        public WeChatArticleService(string sdkType)
            : base(sdkType)
        { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sdkType"></param>
        public WeChatArticleService(string sdkType, string targetType)
            : base(sdkType)
        { m_strTargetType = targetType; }
        /// <summary>
        /// 传图文资源到微信服务器，并写入本地服务器配置文件
        /// </summary>
        /// <param name="ListNews"></param>
        public void UpdateResources(int? lcid, string lcname, string lcclassify, string ListNews, bool byLink)
        {
            this.ExecuteTryCatch(() =>
            {
                if (string.IsNullOrEmpty(ListNews))
                    EGExceptionOperator.ThrowX<Exception>("请输入文章内容", EGActionCode.缺少必要参数);

                WXArticleResultJson pResult = new WXArticleResultJson();
                pResult.lcId = lcid;
                pResult.lcName = lcname;
                pResult.lcClassify = lcclassify;
                pResult.byLink = byLink;

                //将前端生成json串转为段落实体集合:save用于保存本地数据库，段落增加Rpath字段，用于显示图片；out用于上传数据库
                List<NewsModelX> pListNewsSave = CommonFunction.FromJsonTo<List<NewsModelX>>(ListNews);
                List<NewsModel> pListNewsOut = CommonFunction.FromJsonTo<List<NewsModel>>(ListNews);
                //转换图文内容的HTML标签
                ConvertHTMLP(ref pListNewsSave);
                ConvertHTMLP(ref pListNewsOut);
                //段落实体集合受限赋值到ResultJson
                pResult.ListNews = pListNewsSave;
                //再将段落实体集合转换为数组
                NewsModel[] pArray = pListNewsOut.ToArray();
                //更新资源（上传至微信端，并写入本地配置）
                base.UpdateResources<WXArticleResultJson>(pResult, m_strTargetType, pArray);
            });
        }
        /// <summary>
        /// 读取图文资源本地配置
        /// </summary>
        /// <param name="iPageIndex"></param>
        /// <param name="iRowCountInPage"></param>
        /// <param name="filterString"></param>
        /// <param name="iRecordCount"></param>
        /// <returns></returns>
        public List<WXArticleResultJson> LoadResources(int iPageIndex, int iRowCountInPage, string filterString, out int iRecordCount)
        {
            var pList = new List<WXArticleResultJson>();
            int piRecordCount1 = 0;
            int piRecordCount2 = 0;
            this.ExecuteTryCatch(() =>
            {
                //获取配置，并匹配实体集合
                var pListnews = base.LoadResourcesXForPages<WXArticleResultJson>("news", iPageIndex, iRowCountInPage, filterString, out piRecordCount1);
                //获取配置，并匹配实体集合
                var pListmpnews = base.LoadResourcesXForPages<WXArticleResultJson>("mpnews", iPageIndex, iRowCountInPage, filterString, out piRecordCount2);

                if (pListnews != null && pListnews.Count != 0) pList.AddRange(pListnews);
                if (pListmpnews != null && pListmpnews.Count != 0) pList.AddRange(pListmpnews);
            });
            iRecordCount = piRecordCount1 + piRecordCount2;
            return pList;
        }
        ///// <summary>
        ///// 读取单个图文资源本地配置
        ///// </summary>
        ///// <param name="media_id"></param>
        ///// <returns></returns>
        //public WXArticleResultJson LoadResourcesSingle(string media_id)
        //{
        //    WXArticleResultJson pResult = null;
        //    this.ExecuteTryCatch(() =>
        //    {
        //        //获取配置，并匹配实体集合
        //        List<WXArticleResultJson> pList = base.LoadResourcesX<WXArticleResultJson>(m_strTargetType);
        //        pResult = pList.Single(p => p.media_id == media_id);
        //    });
        //    return pResult;
        //}
        /// <summary>
        /// 读取单个图文资源本地配置
        /// </summary>
        /// <param name="media_id"></param>
        /// <returns></returns>
        public WXArticleResultJson LoadResourcesSingleBylcID(int lcid)
        {
            WXArticleResultJson pResult = null;
            this.ExecuteTryCatch(() =>
            {
                //获取配置，并匹配实体集合
                List<WXArticleResultJson> pList = base.LoadResourcesX<WXArticleResultJson>(m_strTargetType);
                pResult = pList.Single(p => p.lcId == lcid);
            });
            return pResult;
        }
        /// <summary>
        /// 读取本地单个图文资源本地配置
        /// </summary>
        /// <param name="pHost"></param>
        /// <param name="media_id"></param>
        /// <returns></returns>
        public List<Article> LoadResources2LocalArticles(string pHost, int lcId)
        {
            List<Article> pResult = new List<Article>();
            this.ExecuteTryCatch(() =>
            {
                //获取配置，并匹配实体集合
                List<WXArticleResultJson> pList = base.LoadResourcesX<WXArticleResultJson>("news");
                WXArticleResultJson pEn = pList.Single(p => p.lcId == lcId);
                if (pEn == null || pEn.ListNews == null || pEn.ListNews.Count == 0)
                {
                    EGExceptionOperator.ThrowX<Exception>("没有编号对应图文消息", EGActionCode.缺少目标数据);
                }

                Article pArticle;
                int idx = 0;
                foreach (NewsModelX pNewsModel in pEn.ListNews)
                {
                    pArticle = new Article()
                    {
                        Description = pNewsModel.digest,
                        Title = pNewsModel.title,
                        PicUrl = string.Format("http://{0}{1}", pHost, pNewsModel.RPath),
                        Url = string.Format("http://{0}/WXArticle/Index?mid={1}&idx={2}", pHost, pEn.media_id, idx)
                    };
                    pResult.Add(pArticle);
                    idx += 1;
                }
            });
            return pResult;

        }
        /// <summary>
        /// 读取本地单个图文资源本地配置
        /// </summary>
        /// <param name="lcId"></param>
        /// <returns></returns>
        public List<object> LoadResources2News(object objlcId, string targetType = "")
        {
            var pResult = new List<object>();
            this.ExecuteTryCatch(() =>
            {
                if (!(objlcId is Int32)) return;
                var lcId = Convert.ToInt32(objlcId);

                targetType = targetType == "" ? m_strTargetType : targetType;

                //获取配置，并匹配实体集合
                List<WXArticleResultJson> pList = base.LoadResourcesX<WXArticleResultJson>(targetType);
                if (pList == null || pList.Count == 0)
                    EGExceptionOperator.ThrowX<Exception>("没有编号对应图文消息", EGActionCode.缺少目标数据);
                WXArticleResultJson pEn = pList.Single(p => p.lcId == lcId);
                if (pEn == null || pEn.ListNews == null || pEn.ListNews.Count == 0)
                    EGExceptionOperator.ThrowX<Exception>("没有编号对应图文消息", EGActionCode.缺少目标数据);
                if (this.ArticleConvertFunc == null)
                    EGExceptionOperator.ThrowX<Exception>("请设置图文消息转换方法", EGActionCode.未知错误);
                //var pSelectFunc = m_sdk.GetNewsConvertFunc(targetType);
                pResult = pEn.ListNews.Select(this.ArticleConvertFunc).ToList<object>();
            });
            return pResult;
        }
        ///// <summary>
        ///// 读取本地单个图文资源本地配置
        ///// </summary>
        ///// <param name="lcId"></param>
        ///// <returns></returns>
        //public List<T> LoadResources2News<T>(object objlcId, string targetType = "")
        //{
        //    var pResult = new List<object>();
        //    this.ExecuteTryCatch(() =>
        //    {
        //        if (!(objlcId is Int32)) return;
        //        var lcId = Convert.ToInt32(objlcId);

        //        targetType = targetType == "" ? m_strTargetType : targetType;

        //        //获取配置，并匹配实体集合
        //        List<WXArticleResultJson> pList = base.LoadResourcesX<WXArticleResultJson>(targetType);
        //        if (pList == null || pList.Count == 0)
        //            EGExceptionOperator.ThrowX<Exception>("没有编号对应图文消息", EGActionCode.缺少目标数据);
        //        WXArticleResultJson pEn = pList.Single(p => p.lcId == lcId);
        //        if (pEn == null || pEn.ListNews == null || pEn.ListNews.Count == 0)
        //            EGExceptionOperator.ThrowX<Exception>("没有编号对应图文消息", EGActionCode.缺少目标数据);
        //        if (this.ArticleConvertFunc == null)
        //            EGExceptionOperator.ThrowX<Exception>("请设置图文消息转换方法", EGActionCode.未知错误);
        //        //var pSelectFunc = m_sdk.GetNewsConvertFunc(targetType);

        //        if(this.ArticleConvertFunc as Func<

        //        pResult = pEn.ListNews.Select(this.ArticleConvertFunc).ToList<T>();
        //    });
        //    return pResult;
        //}
        /// <summary>
        /// 图文转换执行方法
        /// </summary>
        public Func<NewsModelX, object> ArticleConvertFunc { get; set; }

        #region 私有方法
        /// <summary>
        /// 由于服务端有HTML输入标识限制，防止脚本注入，因此，将图文内容的HTML标签全部转换为<> &lt; &gt;,在此需要转换回来正常标签
        /// </summary>
        /// <param name="pListNews"></param>
        private void ConvertHTMLP<T>(ref List<T> pListNews)
            where T : NewsModel
        {
            string pl = "&lt;";
            string pg = "&gt;";
            foreach (NewsModel pEn in pListNews)
            {
                if (!string.IsNullOrEmpty(pEn.content))
                {
                    pEn.content = pEn.content.Replace(pl, "<");
                    pEn.content = pEn.content.Replace(pg, ">");
                }
            }
        }
        #endregion
    }
}
