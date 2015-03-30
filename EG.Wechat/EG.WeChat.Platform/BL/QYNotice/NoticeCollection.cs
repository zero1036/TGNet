using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EG.WeChat.Platform.DA.QYNoticeDA;
namespace EG.WeChat.Platform.BL.QYNotice
{
    public class NoticeCollection
    {
        private List<NoticeBL> _notices;

        public List<NoticeBL> Notices
        {
            get { return _notices; }
            set { _notices = value; }
        }

        public NoticeCollection()
        {
            NoticeDA da = new NoticeDA();
            Notices = da.TableToEntity<NoticeBL>(da.GetNotices());
            
        }
    }
}
