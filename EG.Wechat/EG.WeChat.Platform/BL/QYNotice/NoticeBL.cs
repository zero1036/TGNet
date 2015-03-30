using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EG.WeChat.Platform.DA.QYNoticeDA;
using Senparc.Weixin.QY.AdvancedAPIs.OAuth2;
using EG.WeChat.Platform.BL.QYMailList;
namespace EG.WeChat.Platform.BL.QYNotice
{
    public class NoticeBL
    {
        public int id { get; set; }
        public string Title { get; set; }
        public string Context { get; set; }
        public string Status { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime DeleteDate { get; set; }
        public string DeleteBy { get; set; }
        public string CreateUserName { get; set; }
        public  enum StatusType 
        {
            check=2,
            send=3
        
        }

        public NoticeBL()
        {

        }
        private NoticeDA da=new NoticeDA();

        public static NoticeBL CreateNotice(string id)
        {
            NoticeDA da = new NoticeDA();
            return da.TableToEntity<NoticeBL>(da.GetNotices(id)).First();
        }

        public bool AddNotice()
        {
            NoticeDA da = new NoticeDA();
            return da.AddNotice(Title, Context, CreateBy);
        }

        public bool UpdateNotice()
        {
            NoticeDA da = new NoticeDA();
            return da.UpdateNotice(id.ToString(), UpdateBy, Title, Context, Status);
        }

        public bool DeleteNotice()
        {
            NoticeDA da = new NoticeDA();
            return da.DeleteNotice(id.ToString(), DeleteBy);
        }

        private List<QYMemberBL> _members;

        public List<QYMemberBL> Members
        {
            get {
                if (_members == null)
                {
                    NoticeDA da = new NoticeDA();
                    _members = da.TableToEntity<QYMemberBL>(da.GetNoticeMember(id.ToString()));
                }
                return _members; }
            set { _members = value; }
        }

        

        public string Link
        {
            get
            {
                return OAuth2Api.GetCode(QYMailList.QYConfig.CorpId, QYMailList.QYConfig.NoticeLink + id.ToString(), "");

            }
        }
    }
}
