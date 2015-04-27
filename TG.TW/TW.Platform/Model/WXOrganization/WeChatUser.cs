using EG.WeChat.Service;
using EG.WeChat.Utility.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*****************************************************
* 目的：微信用户实体，继承UserInfoJson
* 创建人：林子聪
* 创建时间：20141106
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace TW.Platform.Model
{
    public class WeChatUser : Senparc.Weixin.MP.AdvancedAPIs.User.UserInfoJson
    {
        /// <summary>
        /// 备注名称
        /// </summary>
        public string remarkname
        {
            get;
            set;
        }
        /// <summary>
        /// 分组ID
        /// </summary>
        public int groupid
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="nickname"></param>
        /// <param name="groupid"></param>
        /// <param name="country"></param>
        /// <param name="province"></param>
        /// <param name="city"></param>
        /// <param name="sex"></param>
        /// <returns></returns>
        public static Queue<QueryEntity> Query(string openid, string nickname, int groupid, string country, string province, string city, int sex)
        {
            Queue<QueryEntity> paramQue = new Queue<QueryEntity>();

            if (!string.IsNullOrEmpty(openid))
                paramQue.Enqueue(new QueryEntity("openid", openid, true, false));
            if (!string.IsNullOrEmpty(nickname))
                paramQue.Enqueue(new QueryEntity("nickname", nickname, true, false));
            if (groupid != -1)
                paramQue.Enqueue(new QueryEntity("groupid", groupid, true, false));
            if (!string.IsNullOrEmpty(country))
                paramQue.Enqueue(new QueryEntity("country", country, true, false));
            if (!string.IsNullOrEmpty(province))
                paramQue.Enqueue(new QueryEntity("province", province, true, false));
            if (!string.IsNullOrEmpty(city))
                paramQue.Enqueue(new QueryEntity("city", city, true, false));
            if (sex != -1)
                paramQue.Enqueue(new QueryEntity("sex", sex, true, false));

            return paramQue;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="nickname"></param>
        /// <param name="groupid"></param>
        /// <param name="country"></param>
        /// <param name="province"></param>
        /// <param name="city"></param>
        /// <param name="sex"></param>
        /// <returns></returns>
        public static Queue<QueryEntity> Query2(List<object> pQueryItems)
        {
            Queue<QueryEntity> paramQue = new Queue<QueryEntity>();

            if (pQueryItems != null && pQueryItems.Count > 0 && pQueryItems[0] != null)
                paramQue.Enqueue(new QueryEntity("openid", pQueryItems[0], true, false));
            if (pQueryItems != null && pQueryItems.Count > 1 && pQueryItems[1] != null)
                paramQue.Enqueue(new QueryEntity("nickname", pQueryItems[1], true, false));
            if (pQueryItems != null && pQueryItems.Count > 2 && pQueryItems[2] != null)
            {
                if (!pQueryItems[2].Equals(-1))
                    paramQue.Enqueue(new QueryEntity("groupid", pQueryItems[2], true, false));
            }
            if (pQueryItems != null && pQueryItems.Count > 3 && pQueryItems[3] != null)
                paramQue.Enqueue(new QueryEntity("country", pQueryItems[3], true, false));
            if (pQueryItems != null && pQueryItems.Count > 4 && pQueryItems[4] != null)
                paramQue.Enqueue(new QueryEntity("province", pQueryItems[4], true, false));
            if (pQueryItems != null && pQueryItems.Count > 4 && pQueryItems[5] != null)
                paramQue.Enqueue(new QueryEntity("city", pQueryItems[5], true, false));
            if (pQueryItems != null && pQueryItems.Count > 4 && pQueryItems[6] != null)
                paramQue.Enqueue(new QueryEntity("sex", pQueryItems[6], true, false));

            return paramQue;
        }
    }
}
