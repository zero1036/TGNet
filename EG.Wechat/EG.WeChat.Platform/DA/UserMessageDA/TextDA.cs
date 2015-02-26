using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EG.WeChat.Model;

namespace EG.WeChat.Platform.DA
{

    public class TextDA : DataBase
    {

        public PagingM GetPageList(Hashtable model, int pageIndex)
        {
            //string sql = @"select ID,OpenID,Date, from WC_Text where openid like [OpenID] and date <= [Date] ";

            string sql = @" select convert(varchar(100),Date,23) as 'Date',
                            'UserName' = case when t2.openid = null then  t1.openid else t2.nickname COLLATE Chinese_PRC_CI_AS end,
                            Content,Lng,Lat
                            from WC_Text t1
                            left join WC_USER as t2 on t1.OpenId = t2.openid
                            where t1.openid like [OpenID] ";
            if (model["Date"] != null)
            {
                sql += " and t1.date = [Date] ";
            }

            return QueryByPage(sql, model, pageIndex, "date");
        }

    }



}
