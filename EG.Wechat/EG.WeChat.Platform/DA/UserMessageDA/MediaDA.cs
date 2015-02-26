using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EG.WeChat.Model;

namespace EG.WeChat.Platform.DA
{
    public class MediaDA : DataBase
    {

        public PagingM GetPageList(Hashtable model, int pageIndex)
        {
            /* nickname 使用了 Chinese_PRC_CS_AS_WS ，因此使用的時候要轉為 Chinese_PRC_CI_AS */
            string sql = @" select convert(varchar(100),Date,23) as 'Date',
                            'UserName' = case when t2.openid = null then  t1.openid else t2.nickname COLLATE Chinese_PRC_CI_AS end,
                            id as 'MediaID',
                            t1.OpenId as 'OpenId',
                            t1.Type as 'Type'

                            from WC_Media t1
                            left join WC_USER as t2 on t1.OpenId = t2.openid
                            where t1.type = [Type] and t1.openid like [OpenID] ";
            if (model["Date"] != null)
            {
                sql += " and t1.date == [Date] ";
            }

            var result = QueryByPage(sql, model, pageIndex, "date");
            return result;
        }


        /// <summary>
        /// 保存资源记录
        /// </summary>
        public int SaveMediaResource(EG.WeChat.Platform.Model.WCR_Media_Resource model )
        {
            return template.Insert(model);
        }

    }
}
