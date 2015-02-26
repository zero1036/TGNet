using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using EG.WeChat.Platform.DA;
using EG.WeChat.Platform.Model;
using EG.Utility.DBCommon.dao;
/*****************************************************
* 目的：本地会员及会员卡服务
* 创建人：林子聪
* 创建时间：20141215
* 备注：服务暂未实现
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Platform.BL
{
    public class MemberCardService : IServiceX
    {
        /// <summary>
        /// 通过微信用户OpenID获取用户拥有的会员卡模板集合
        /// </summary>
        /// <param name="OpenID"></param>
        /// <returns></returns>
        public List<CardContent> GetMemberCard(string OpenID)
        {
            //获取会员卡初始数据模板
            List<CardContent> pListCards = GetMemberCardTemplate(OpenID);
            //如果微信用户不是会员，没有模板，则回复推广消息，由于暂时没有推广功能，暂时写死内容-林子聪-20141215
            if (pListCards == null || pListCards.Count == 0)
            {
                CardContent pCard = new CardContent();

                pCard.ListCardInfo = new List<CardContent.CContent>();
                CardContent.CContent pCardContent = new CardContent.CContent();
                pCardContent.Title = "会员特权";
                pCardContent.ContentID = string.Empty;
                pCardContent.Content = "测试内容";
                pCard.ListCardInfo.Add(pCardContent);

                pListCards = new List<CardContent>();
                pListCards.Add(pCard);
                return pListCards;
            }
            else
            {
                //暂时不考虑定制化服务问题，直接返回结果
                CustomService pCService = new CustomService();
                Hashtable hsParam = new Hashtable();


                //判断是否具有资源
                if (pCService.IsReady)
                {
                    foreach (CardContent pCard in pListCards)
                    {
                        //添加服务参数，微信用户OPENID及会员卡类型编码
                        hsParam.Add(CustomParameters.OPENID, OpenID);
                        hsParam.Add(CustomParameters.CARDTYPE, pCard.CardID);

                        if (pCard.ListCardInfo == null || pCard.ListCardInfo.Count == 0)
                            continue;
                        foreach (CardContent.CContent pContent in pCard.ListCardInfo)
                        {
                            if (pContent.Content.Contains("[[") && pContent.Content.Contains("]]"))
                            {
                                pContent.Content = pCService.ExcuteCService(pContent.Content, hsParam);
                            }
                        }

                    }
                }

            }
            return pListCards;
        }

        #region 私有成员
        /// <summary>
        /// 获取会员卡初始数据模板
        /// </summary>
        /// <param name="OpenID"></param>
        /// <returns></returns>
        private List<CardContent> GetMemberCardTemplate(string OpenID)
        {
            List<CardContent> pResult = new List<CardContent>();
            //初始化服务
            LCMemberService pMemberService = new LCMemberService();
            LCCardService pCardService = new LCCardService();
            //获取会员对应拥有的会员卡媒体ID（一个会员可以拥有多张会员卡）
            List<string> pListMediaID = pMemberService.GetMemberCardTypes(OpenID);
            if (pListMediaID == null || pListMediaID.Count == 0)
            {
                //如果用户无会员卡，则默认调出99999编号的推广数据
                pListMediaID.Add("99999");
                //return null;
            }
            //根据会员卡媒体资源ID获取对应实体集合
            pResult = pCardService.GetCardContentCache(pListMediaID);

            return pResult;
        }
        #endregion

    }
}
