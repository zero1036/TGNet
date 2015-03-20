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
        protected string m_strTargetType = "News";
        protected UploadMediaFileType m_UploadMediaFileType = UploadMediaFileType.news;
        /// <summary>
        /// 传图文资源到微信服务器，并写入本地服务器配置文件
        /// </summary>
        /// <param name="ListNews"></param>
        public void UpdateResources(int? lcid, string lcname, string lcclassify, string ListNews)
        {
            this.ExecuteTryCatch(() =>
            {
                if (string.IsNullOrEmpty(ListNews))
                    EGExceptionOperator.ThrowX<Exception>("请输入文章内容", EGActionCode.缺少必要参数);

                WXArticleResultJson pResult = new WXArticleResultJson();
                pResult.lcId = lcid;
                pResult.lcName = lcname;
                pResult.lcClassify = lcclassify;

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
                base.UpdateResources<UploadMediaFileResultX, UploadMediaFileResult, WXArticleResultJson>(pResult, m_UploadMediaFileType, m_strTargetType, pArray);
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
            List<WXArticleResultJson> pList = null;
            int piRecordCount = 0;
            this.ExecuteTryCatch(() =>
            {
                //获取配置，并匹配实体集合
                pList = base.LoadResourcesXForPages<WXArticleResultJson>(m_strTargetType, iPageIndex, iRowCountInPage, filterString, out piRecordCount);
            });
            iRecordCount = piRecordCount;
            return pList;
        }
        /// <summary>
        /// 读取单个图文资源本地配置
        /// </summary>
        /// <param name="media_id"></param>
        /// <returns></returns>
        public WXArticleResultJson LoadResourcesSingle(string media_id)
        {
            WXArticleResultJson pResult = null;
            this.ExecuteTryCatch(() =>
            {
                //获取配置，并匹配实体集合
                List<WXArticleResultJson> pList = base.LoadResourcesX<WXArticleResultJson>(m_strTargetType);

                pResult = pList.Single(p => p.media_id == media_id);
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
                List<WXArticleResultJson> pList = base.LoadResourcesX<WXArticleResultJson>(m_strTargetType);
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
