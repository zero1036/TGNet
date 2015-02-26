using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EG.WeChat.Model;
using EG.WeChat.Platform.DA;
using EG.WeChat.Platform.Model;

namespace EG.WeChat.Platform.BL
{
    public class TextBL
    {
        private TextDA _textDA;
        protected TextDA TextDA
        {
            get
            {
                if (_textDA == null)
                {
                    _textDA = new TextDA();
                }
                return _textDA;
            }
        }


        /// <summary>
        /// 添加媒體資源
        /// </summary>
        /// <param name="OpenId"></param>
        /// <param name="Media"></param>
        /// <returns></returns>
        public virtual ResultM NewText(string OpenId, string content, float? lng = null, float? lat = null)
        {
            ResultM result = new ResultM();
            WC_Text model = new WC_Text() { OpenID = OpenId, Date = DateTime.Now, Lat = lat, Lng = lng, Content = content };
            result.Affected = TextDA.Add(model);
            return result;
        }



        /// <summary>
        /// 最近三天能獲取的媒體資源
        /// </summary>
        public virtual PagingM List(string openId, DateTime? dt, int pageIndex)
        {
            Hashtable model = new Hashtable();
            model.Add("OpenID", string.Format("%{0}%", openId));
            model.Add("Date", dt);
            return TextDA.GetPageList(model, pageIndex);
        }






    }
}
