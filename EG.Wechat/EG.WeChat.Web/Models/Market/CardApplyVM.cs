using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EG.WeChat.Platform.Model;
using EG.WeChat.Utility.Tools;

namespace EG.WeChat.Web.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class CardApplyVM
    {
        private ASlider m_ASliderData;
        private AAccordion m_AccordionData;
        private List<CardContent> m_CardForMember;

        public CardApplyVM()
        {
            TemData = new T_Member();
        }
        /// <summary>
        /// 是否会员
        /// </summary>
        public bool IsMembers
        { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get
            {
                return TemData.Name;
            }
            set
            {
                TemData.Name = value;
            }
        }
        /// <summary>
        /// 电话
        /// </summary>
        public string Phone
        {
            get
            {
                return TemData.Phone;
            }
            set
            {
                TemData.Phone = value;
            }
        }
        /// <summary>
        /// 电邮
        /// </summary>
        public string Mail
        {
            get
            {
                return TemData.Mail;
            }
            set
            {
                TemData.Mail = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OpenID
        {
            get
            {
                return TemData.OpenID;
            }
            set
            {
                TemData.OpenID = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public T_Member TemData
        {
            get;
            set;
        }
        /// <summary>
        /// 会员信息
        /// </summary>
        public List<CardContent> CardForMember
        {
            set
            {
                m_CardForMember = value;
                if (m_CardForMember != null && m_CardForMember.Count != 0)
                {
                    CardContent pCard = m_CardForMember[0];

                    m_ASliderData = new ASlider();
                    m_ASliderData.theme = "c2";
                    m_ASliderData.sliderConfig = "{\"directionNav\":false}";
                    m_ASliderData.content = new List<ASlider.ASliderContent>();
                    ASlider.ASliderContent pSliContent;

                    foreach (CardContent.CContent pCardContent in pCard.ListCardInfo)
                    {
                        pSliContent = new ASlider.ASliderContent();
                        pSliContent.img = pCard.ImgPath;
                        pSliContent.img = pCard.ImgPath;
                        pSliContent.desc = pCardContent.Desc;
                        m_ASliderData.content.Add(pSliContent);
                    }



                    m_AccordionData = new AAccordion();
                    m_AccordionData.theme = "one";
                    AAccordion.AOptions pOt = new AAccordion.AOptions();
                    pOt.multiple = true;
                    m_AccordionData.options = pOt;
                    m_AccordionData.content = new List<AAccordion.AAccordionContent>();
                    AAccordion.AAccordionContent pAAContent;

                    foreach (CardContent.CContent pCardContent in pCard.ListCardInfo)
                    {
                        pAAContent = new AAccordion.AAccordionContent();
                        pAAContent.title = pCardContent.Title;
                        pAAContent.content = pCardContent.Content;
                        pAAContent.active = true;
                        m_AccordionData.content.Add(pAAContent);
                    }
                }
            }
        }
        /// <summary>
        /// ASlider
        /// </summary>
        public ASlider ASliderData
        {
            get
            {
                return m_ASliderData;
            }
        }
        /// <summary>
        /// AAccordion
        /// </summary>
        public AAccordion AccordionData
        {
            get
            {
                return m_AccordionData;
            }
        }
        /// <summary>
        /// 新会员信息
        /// </summary>
        public string NewMemberInfo
        {
            get;
            set;
        }
    }
}