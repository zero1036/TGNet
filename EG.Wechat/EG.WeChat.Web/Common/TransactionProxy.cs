using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EG.Utility.DBCommon.dao;

namespace EG.WeChat.Web
{
    public class TransactionProxy
    {
        public static T New<T>()
        {
            try
            {
                return (T)TransactionAOP.newInstance(typeof(T));
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }
    }
}