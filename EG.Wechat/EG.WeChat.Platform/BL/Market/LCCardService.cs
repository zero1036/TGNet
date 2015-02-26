using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Caching;
using EG.WeChat.Platform.DA;
using EG.WeChat.Platform.Model;
using EG.WeChat.Utility.Tools;
/*****************************************************
* 目的：本地会员卡服务
* 创建人：林子聪
* 创建时间：20141215
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Platform.BL
{
    public class LCCardService : WeChatResourcesService, IServiceXCache
    {
        /// <summary>
        /// 微信分组缓存项
        /// </summary>
        public CardContentCacheConfig m_CacheConfig = new CardContentCacheConfig();
        protected string m_strTargetType = "LCCard";

        #region 会员卡数据表读写
        /// <summary>
        /// 从库表中，读取会员卡内容模板数据
        /// </summary>
        /// <param name="pListMediaID"></param>
        /// <returns></returns>
        public List<CardContent> LoadResourcesFromData()
        {
            //获取配置，并匹配实体集合
            List<CardContent> pList = base.LoadResources<CardContent>(m_strTargetType);
            return pList;
        }
        /// <summary>
        /// 更新模板消息配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pT"></param>
        /// <param name="pMediaID"></param>
        public void UpdateCardData(string exa)
        {
            this.ExecuteTryCatch(() =>
            {
                CardContent pCard = new CardContent();
                string pMediaID = string.Empty;

                if (exa == "1")
                {
                    pCard = Example1();
                    pMediaID = "0";
                }
                else if (exa == "2")
                {
                    pCard = Example2();
                    pMediaID = "99999";
                }
                else
                {
                    return;
                }

                base.UpdateResources<CardContent>(m_strTargetType, pMediaID, pCard);
            });
        }
        /// <summary>
        /// 生成测试数据——临时
        /// </summary>
        /// <returns></returns>
        private CardContent Example2()
        {
            string strCOntent = "<div data-am-widget='intro' class='am-intro am-cf am-intro-default'><div class='am-intro-hd'><h2 class='am-intro-title'>Info</h2><a class='am-intro-more am-intro-more-top' href='#more'>更多细节</a></div><div class='am-g am-intro-bd'><div class='am-intro-left am-u-sm-5'><img src='http://i.static.amazeui.org/assets/i/cpts/intro/WP_Cortana_China.png'alt='小娜' /></div><div class='am-intro-right am-u-sm-7'><p>XXXXX电子会员卡，随时申请随时使用</p><p>10秒快速申请，填写电邮地址，即可免费申请XXX尊享会员卡</p><p>凭卡用戶可享受积分返点或折扣优惠；畅享会员专属优惠活动、赠书、免费试吃、免费菜品；定期获得各种商家优惠券</p><br /><p style='font-size: 24px'>急不及待<button onclick='ScrollToBottom()' class='am-btn am-btn-success' data-am-popover='{content: '填写认证立即领取会员卡', trigger: 'hover focus'}'>马上领取</button></p></div></div><img src='http://e.ims.365imgs.cn/e/3/bc/218814.jpg' /></div>";

            string pMediaID = "99999";
            CardContent pCard = new CardContent();
            pCard.CardID = 99;
            pCard.ImgPath = "/Images/common/会员卡示例.png";
            pCard.MediaID = pMediaID;
            pCard.ListCardInfo = new List<CardContent.CContent>();

            CardContent.CContent pContent = new CardContent.CContent();
            pContent.Title = "開卡即送";
            pContent.Content = strCOntent;
            pContent.Desc = "";
            pCard.ListCardInfo.Add(pContent);

            return pCard;
        }
        /// <summary>
        /// 生成测试数据——临时
        /// </summary>
        /// <returns></returns>
        private CardContent Example1()
        {
            string pMediaID = "0";
            CardContent pCard = new CardContent();
            pCard.CardID = 1;
            pCard.ImgPath = "/Images/common/会员卡示例.png";
            pCard.MediaID = pMediaID;
            pCard.ListCardInfo = new List<CardContent.CContent>();

            CardContent.CContent pContent = new CardContent.CContent();
            pContent.Title = "會員特權";
            pContent.Content = "<p>1、會員尊享[[Deposit]]折優惠</p><p>2、持本卡可領取會員特有優惠券或代金券</p><p>3、持本卡买苹果送IPhone 6 Plus</p>";
            pContent.Desc = "會員金卡";
            pCard.ListCardInfo.Add(pContent);

            pContent = new CardContent.CContent();
            pContent.Title = "開卡即送";
            pContent.Content = "<p>開卡送大禮：[[Integral]]積分</p><p>領卡送大禮！立即獲得[[Integral]]個積分參與活動</p>";
            pContent.Desc = "";
            pCard.ListCardInfo.Add(pContent);

            pContent = new CardContent.CContent();
            pContent.Title = "聖誕優惠活動";
            pContent.Content = "<img src='http://img10.3lian.com/sc6/show/02/03/20101222225310238.jpg' /><p>聖誕送優惠券滿100减10元</p>";
            pContent.Desc = "";
            pCard.ListCardInfo.Add(pContent);

            return pCard;
        }
        #endregion

        #region 会员卡缓存读写
        /// <summary>
        /// 从缓存中，读取会员卡内容模板数据
        /// </summary>
        /// <param name="pListMediaID"></param>
        /// <returns></returns>
        public List<CardContent> GetCardContentCache(List<string> pListMediaID)
        {
            List<CardContent> pList = this.GetCacheList<CardContent>(LoadResourcesFromData, m_CacheConfig, this.CardCacheRemovedCallback);
            if (pList == null || pList.Count == 0)
            {
                EGExceptionOperator.ThrowX<Exception>("缺少本地会员卡内容模板数据，请配置相关资源", EGActionCode.缺少目标数据);
            }
            //根据条件过滤
            return pList.Where(con => pListMediaID.Contains(con.MediaID)).ToList<CardContent>();
        }
        #endregion

        #region 回调事件
        /// <summary>
        /// 当前缓存滑动清空后，自动重新加载并写入缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="vvalue"></param>
        /// <param name="r"></param>
        private void CardCacheRemovedCallback(String key, Object vvalue, CacheItemRemovedReason r)
        {
            if (r == CacheItemRemovedReason.Expired)
            {
                //GetWXGroupsFromDB();
            }
        }
        #endregion
    }
}
