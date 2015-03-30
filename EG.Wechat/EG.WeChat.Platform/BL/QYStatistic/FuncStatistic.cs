using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EG.WeChat.Platform.DA;
using EG.WeChat.Utility.Tools;
using EG.WeChat.Platform.Model;
using EG.WeChat.Platform.BL.QYMailList;
using System.Data;

namespace EG.WeChat.Platform.BL
{
    public class FuncStatistic
    {
        public Int64 ID { get; set; }
        public Int64 FuncID { get; set; }
        public Int64 MemberID { get; set; }
        public int IsChecked { get; set; }
        public int FuncType { get; set; }
        public int MsgID { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime DeleteDate { get; set; }
        public string DeleteBy { get; set; }


       /// <summary>
       /// 增加单个统计项
       /// </summary>
        /// <param name="funcID">功能id</param>
       /// <param name="memberID">成员主键</param>
       /// <param name="funcType">功能类型</param>
       /// <param name="msgID">消息id</param>
       /// <param name="userID">操作人员id</param>
       /// <returns></returns>
        private bool AddStatistic(string funcID, string memberID, string funcType, string msgID,string userID)
        {
            try
            {
                QYStatisticDA qyStatisticDA = new QYStatisticDA();
                qyStatisticDA.AddStatistic(funcID,memberID,funcType,msgID,userID);
                return true;
            }
            catch (Exception e)
            {
                Logger.Log4Net.Error("add statistic error:" + e);
                return false;
            }
        }

        /// <summary>
        /// 检查统计数据表中当前消息存在该功能统计
        /// </summary>
        /// <param name="funcID">功能id</param>
        /// <param name="funcType">功能类型</param>
        /// <param name="msgID">消息id</param>
        /// <returns></returns>
        public bool AddStatisticChecked(string funcID, string funcType, string msgID)
        {
            QYStatisticDA qyStatisticDA = new QYStatisticDA();
            int statisticCount=0;
            DataTable dt = qyStatisticDA.AddStatisticChecked(funcID, funcType, msgID);
            if (dt != null)
            {
                statisticCount = int.Parse(dt.Rows[0][0].ToString());
            }

            return statisticCount > 0 ? true : false;
        }

       /// <summary>
        /// 增加该次统计的所有成员
       /// </summary>
       /// <param name="toUser">微信用户id</param>
       /// <param name="toParty">微信部门id</param>
       /// <param name="toTag">标签id</param>
       /// <param name="funcID">功能id</param>
       /// <param name="funcType">功能类型</param>
       /// <param name="msgID">消息id</param>
       /// <param name="userID">操作人员id</param>
        public bool AddStatisticList(string toUser,string toParty,string toTag,string funcID, string funcType, string msgID,string userID,ref string errorMsg)
        {
            try
            {
                if (!AddStatisticChecked(funcID, funcType, msgID))
                {

                    List<Int64> memberIDList = new List<Int64>();
                    List<QYMemberBL> qyMemberList = new List<QYMemberBL>();
                    //SendTarget sendTarget = CommonFunction.FromJsonTo<SendTarget>(toTarget);

                    if (toUser == "@all")
                    {
                        QYDepartmentBL qyDepartBL = QYDepartmentBL.GetByWXID("1");
                        qyMemberList = QYMemberBL.GetMemberAllByDep(qyDepartBL);
                    }
                    else
                    {

                        if (!string.IsNullOrEmpty(toUser))
                        {
                            string[] userIDList = toUser.Split('|');

                            for (int i = 0; i < userIDList.Length; i++)
                            {
                                qyMemberList.Add(QYMemberBL.GetMemberByWXID(userIDList[i]));
                            }
                        }

                        if (!string.IsNullOrEmpty(toParty))
                        {
                            List<QYMemberBL> qyDepartMemberList = new List<QYMemberBL>();
                            string[] departIDList = toParty.Split('|');

                            for (int i = 0; i < departIDList.Length; i++)
                            {
                                QYDepartmentBL qyDepartBL = QYDepartmentBL.GetByWXID(departIDList[i]);
                                qyDepartMemberList = QYMemberBL.GetMemberAllByDep(qyDepartBL);


                                if (qyDepartMemberList.Count > 0)
                                    qyMemberList.AddRange(qyDepartMemberList);

                            }
                        }

                        foreach (QYMemberBL qyMemberItem in qyMemberList)
                        {
                            memberIDList.Add(qyMemberItem.ID);

                        }
                        memberIDList = memberIDList.Distinct().ToList();
                    }

                    foreach (Int64 memberItem in memberIDList)
                    {
                        AddStatistic(funcID, memberItem.ToString(), funcType, msgID, userID);
                    }
                    return true;
                }
                else
                {
                    return false;
                }

            }

            catch (Exception e)
            {
                errorMsg = e.Message;
                Logger.Log4Net.Error("add statistic error:" + e);
                return false;
            }

        }
    }
}
