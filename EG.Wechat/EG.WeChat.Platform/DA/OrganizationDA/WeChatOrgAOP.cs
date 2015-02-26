using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
/*****************************************************
* 目的：微信用户及分组数据更新时AOP执行抽象类
* 创建人：林子聪
* 创建时间：20141107
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Platform.DA
{
    /// <summary>
    /// 对应数据变更接口
    /// </summary>
    public class WeChatOrgAOP
    {
        #region group
        /// <summary>
        /// 保存微信分组信息至数据库
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public virtual bool SaveGroup(DataTable dt)
        {
            return false;
        }
        /// <summary>
        /// 同步目标分组ID在分组表与用户表的数量数据
        /// </summary>
        /// <returns></returns>
        public virtual bool ReloadGroup()
        {
            return false;
        }
        #endregion

        #region User
        /// <summary>
        /// 保存微信用户至数据库
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public virtual bool SaveUser(DataTable dt)
        {
            return false;
        }
        /// <summary>
        /// 通过OpenID更新GroupID
        /// </summary>
        /// <param name="GroupID"></param>
        /// <param name="OpenID"></param>
        /// <returns></returns>
        public virtual int UpdateGroupIDByOpenID(int GroupID, string OpenID)
        {
            return -1;
        }
        #endregion

    }
}
