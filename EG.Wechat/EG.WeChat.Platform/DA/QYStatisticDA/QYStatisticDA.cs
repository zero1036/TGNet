using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using EG.WeChat.Platform.BL;

namespace EG.WeChat.Platform.DA
{
    public class QYStatisticDA : DataBase
    {
        private ADOTemplateX template = new ADOTemplateX();

        private const string PROC_ADDSTATISTIC = "PRO_WC_QY_AddStatistic";
        private const string STATISTIC_SELECT = @"select count(1) from T_QY_Statistic where 1=1";
        //private const string PROC_ADDSTATISTIC_SQL = "exec PRO_WC_QY_AddStatistic @FuncID,@MemberID,@FuncType,@MsgID,@CreateBy";

        /// <summary>
        /// 增加单个统计项
        /// </summary>
        /// <param name="funcID">功能id</param>
        /// <param name="memberID">成员主键</param>
        /// <param name="funcType">功能类型</param>
        /// <param name="msgID">消息id</param>
        /// <param name="userID">操作人员id</param>
        /// <returns></returns>
        public bool AddStatistic(string funcID, string memberID, string funcType, string msgID, string userID)
        {
            try
            {
                template.Execute(PROC_ADDSTATISTIC, new string[] { "@FuncID", "@MemberID", "@FuncType", "@MsgID", "@CreateBy" },
                                   new object[] { funcID, memberID, funcType, msgID, userID }, null,CommandType.StoredProcedure);
                return true;
            }
            catch (Exception e)
            {
                Logger.Log4Net.Error("QY insert T_QY_AddStatistic error:", e);
            }
            return false;
        }

        public DataTable AddStatisticChecked(string funcID, string funcType, string msgID)
        {
            try
            {
                List<string> paramNameList = new List<string>();
                List<object> paramValList = new List<object>();

                if (!string.IsNullOrEmpty(funcID))
                {
                    paramNameList.Add("@funcID");
                    paramValList.Add(funcID);
                }
                if (!string.IsNullOrEmpty(funcType))
                {
                    paramNameList.Add("@FuncType");
                    paramValList.Add(funcType);
                }
                if (!string.IsNullOrEmpty(msgID))
                {
                    paramNameList.Add("@MsgID");
                    paramValList.Add(msgID);
                }

                return template.Query(STATISTIC_SELECT,paramNameList.ToArray(),
                                   paramValList.ToArray(), null);
            }
            catch (Exception e)
            {
                
                Logger.Log4Net.Error("QY check T_QY_AddStatistic error:", e);
                return null;
            }


 
        }
    }
}
