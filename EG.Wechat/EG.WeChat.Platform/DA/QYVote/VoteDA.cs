using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
namespace EG.WeChat.Platform.DA.QYVote
{
    public class VoteDA:DataBase
    {
        private ADOTemplateX template = new ADOTemplateX();
        /// <summary>
        /// 新增投票
        /// </summary>
        /// <param name="Title">投票項目名</param>
        /// <param name="Context">投票內容</param>
        /// <param name="Vtype">類型（1是單選，2是多選）</param>
        /// <param name="CreateBy">操作者ID</param>
        /// <param name="OptionContexts">選項，項與項以豎線分隔，格式是xxxx|xxxxxx|</param>
        /// <returns></returns>
        public bool AddVote(string Title, string Context, string Vtype, string CreateBy, string OptionContexts, string PicUrl)
        {
            try
            {

                string sql = @"exec PRO_WC_QY_AddVote @Title,@Context,@Vtype,@CreateBy,@OptionContexts,@PicUrl";
                template.Execute(sql, new string[] { "@Title", "@Context", "@Vtype", "@CreateBy", "@OptionContexts","@PicUrl" },
                                     new string[] { Title, Context, Vtype, CreateBy, OptionContexts,PicUrl }, null);
                return true;
            }
            catch (Exception e)
            {
                Logger.Log4Net.Error("QY insert T_QY_Department error:", e);
            }
            return false;
        }

        /// <summary>
        /// 更新投票信息
        /// </summary>
        /// <param name="HeadID">投票頭ID</param>
        /// <param name="Title">投票Title</param>
        /// <param name="Context">投票內容</param>
        /// <param name="Vtype">投票內容</param>
        /// <param name="CreateBy">操作者ID</param>
        /// <param name="OptionContexts">選項，列格式是（id int,optioncontext varchar(200),ActionType varchar(1)）</param>
        /// <param name="PicUrl"></param>
        /// <returns>操作是否成功</returns>
        public bool UpdateVote(string HeadID,string Title, string Context, string Vtype, string CreateBy, DataTable OptionContexts, string PicUrl)
        {
            try
            {

                string sql = @"PRO_WC_QY_UpdateVote";
                template.dbType = ADOTemplateX.DB_TYPE_SQLSERVER;
                Type t = OptionContexts.GetType();
                template.ExecuteX(sql, new string[] { "@HeadID","@Title","@Context","@Vtype","@UpdateBy","@OptionContexts","@PicUrl"},
                                     new object[] {HeadID,Title,Context,Vtype,CreateBy,OptionContexts,PicUrl },null,CommandType.StoredProcedure);
                return true;
            }
            catch (Exception e)
            {
                Logger.Log4Net.Error("QY Update T_QY_Department error:", e);
            }
            return false;
        }

        /// <summary>
        /// 刪除投票
        /// </summary>
        /// <param name="ID">投票HeadID</param>
        /// <param name="DeleteBy">操作者ID</param>
        /// <returns></returns>
        public bool DeleteVote(string ID,string DeleteBy)
        {
            try
            {

                string sql = @"update T_QY_VoteHead set DeleteDate=GETDATE(),DeleteBy=@DeleteBy where id=@id";
                template.Execute(sql, new string[] { "@id", "@DeleteBy"},
                                     new object[] { ID, DeleteBy}, null);
                return true;
            }
            catch (Exception e)
            {
                Logger.Log4Net.Error("QY Update T_QY_Department error:", e);
            }
            return false;
        }

        /// <summary>
        /// 獲取投票項目
        /// </summary>
        /// <param name="id">投票ID，若為null則全部獲取</param>
        /// <returns></returns>
        public DataTable GetVote(string id=null)
        {
            string sql;
            if (id != null)
            {
                sql = @"select  h.*, t.VoteMemberCount from T_QY_VoteHead h 
                        left join (select VoteHeadId,count(distinct QYMemberID) VoteMemberCount 
	                     from  T_QY_VoteResult where DeleteDate is null group by VoteHeadId) t
                            on h.ID=t.VoteHeadId and h.DeleteDate is null 
                            where h.id=@id and h.deletedate is null
                       order by h.CreateDate desc";
                return template.Query(sql, new string[] { "@id" },
                                    new object[] { id }, null);
            }
            else
            {
                sql = @"select  h.*, t.VoteMemberCount from T_QY_VoteHead h 
                        left join (select VoteHeadId,count(distinct QYMemberID) VoteMemberCount 
	                     from  T_QY_VoteResult where DeleteDate is null group by VoteHeadId) t
                            on h.ID=t.VoteHeadId and h.DeleteDate is null 
                        where h.deletedate is null
                       order by h.CreateDate desc";
                return template.Query(sql, new string[] {  },
                                    new object[] {  }, null);
            }
        }

        /// <summary>
        /// 根據投票項ID獲取投票選項
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable GetVoteOptions(string id)
        {
            string sql;
            sql = @"select * from T_QY_VoteOption where DeleteDate is null and HeadID=@id";
            return template.Query(sql, new string[] { "@id" },
                                new object[] { id }, null);
        }
        /// <summary>
        /// 獲取投票結果
        /// </summary>
        /// <param name="VoteID">投票項目ID</param>
        /// <returns></returns>
        public DataTable GetVoteMemberByVote(string VoteID)
        {
            string sql;
            sql = @" select distinct m.*,t.CreateDate VoteDate,p.OptionTitle ,p.OptinContext,t.VoteOptionID,t.VoteHeadId
	from T_QY_Member m inner join  T_QY_VoteResult t
	on m.ID=t.QYMemberID and m.DeleteDate is null and t.DeleteDate is null
	inner join T_QY_VoteOption p on p.ID=t.VoteOptionID and t.DeleteDate is null and p.DeleteDate is null
	  where t.VoteHeadId =@id";
            return template.Query(sql, new string[] { "@id" },
                                new string[] { VoteID }, null);
        }

        /// <summary>
        /// 獲取投票結果
        /// </summary>
        /// <param name="VoteID">投票選項ID</param>
        /// <returns></returns>
        public DataTable GetVoteMemberByVoteOption(string optionid)
        {
            string sql;
            sql = @"  select distinct m.*,t.CreateDate VoteDate,p.OptionTitle ,p.OptinContext,t.VoteOptionID,t.VoteHeadId
	from T_QY_Member m inner join  T_QY_VoteResult t
	on m.ID=t.QYMemberID and m.DeleteDate is null and t.DeleteDate is null
	inner join T_QY_VoteOption p on p.ID=t.VoteOptionID and t.DeleteDate is null and p.DeleteDate is null
	  where t.VoteOptionID=@optionid";
            return template.Query(sql, new string[] { "@optionid" },
                                new string[] { optionid }, null);
        }


        /// <summary>
        /// 成員開始投票
        /// </summary>
        /// <param name="vid"></param>
        /// <param name="mid"></param>
        /// <param name="userid"></param>
        /// <param name="optids"></param>
        /// <returns></returns>
        public bool AddMemberVote(string vid, string mid,string userid,string optids)
        {
            try
            {

                string sql = @"PRO_WC_QY_AddMemberVote";
                template.dbType = ADOTemplateX.DB_TYPE_SQLSERVER;

                template.ExecuteX(sql, new string[] { "@vid", "@mid", "@userid", "@Optionid" },
                                     new object[] { vid, mid, userid, optids }, null, CommandType.StoredProcedure);
                return true;
            }
            catch (Exception e)
            {
                
                Logger.Log4Net.Error("QY Update T_QY_Department error:", e);
                throw e;
            }
            return false;
        }

        public DataTable GetMemberVote(string mid,string vid)
        {
            string sql;
            sql = @"select p.* from T_QY_VoteResult t inner join T_QY_VoteOption p on t.VoteOptionID=p.ID
	 where qymemberid=@mid and VoteHeadId=@vid";
            return template.Query(sql, new string[] { "@mid","@vid" },
                                new string[] { mid,vid }, null);
        }

    }
}
