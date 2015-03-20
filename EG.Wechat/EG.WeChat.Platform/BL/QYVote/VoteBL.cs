using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EG.WeChat.Platform.DA.QYVote;
using Senparc.Weixin.QY.AdvancedAPIs;
using Senparc.Weixin.QY.CommonAPIs;
using Senparc.Weixin.QY.AdvancedAPIs.MailList;
using System.Data;
namespace EG.WeChat.Platform.BL.QYVote
{
    public class VoteBL
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Context { get; set; }
        public string vtype { get; set; }
        public string PicUrl { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime DeleteDate { get; set; }
        public string DeleteBy { get; set; }
         
        private List<VoteOption> _options;

        public List<VoteOption> Options
        {
            get 
            {
                if (_options == null)
                {
                    _options = vda.TableToEntity<VoteOption>(vda.GetVoteOptions(ID.ToString()));
                }
                return _options; 
            
            }
            set { _options = value; }
        }

        private List<VoteMember> _VoteMember;

        public List<VoteMember> VoteMember
        {
            get {
                if (_VoteMember == null)
                {
                    _VoteMember = vda.TableToEntity<VoteMember>(vda.GetVoteMemberByVote(ID.ToString()));
                }

                return _VoteMember; }
            set { _VoteMember = value; }
        }

        private int _VoteMemberCount;

        public int VoteMemberCount
        {
            get {

               
                return _VoteMemberCount; 
            
            }
            set { _VoteMemberCount = value; }
        }



        private VoteDA vda = new VoteDA();
        public VoteBL()
        {
            
        }

        /// <summary>
        /// 獲取投票信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static VoteBL GetVote(string id)
        {
            VoteDA vda = new VoteDA();
            return vda.TableToEntity<VoteBL>(vda.GetVote(id)).First();

        }

        /// <summary>
        /// 更新投票
        /// </summary>
        /// <returns></returns>
        public bool UpdateVote()
        {
           return  vda.UpdateVote(ID.ToString(), Title, Context, vtype, UpdateBy, GetUpdateFromOpts(),PicUrl);
        }
        /// <summary>
        /// 新增投票
        /// </summary>
        /// <returns></returns>
        public bool AddVote()
        {
            return vda.AddVote(Title, Context, vtype, CreateBy, GetOptsStr(), PicUrl);
        }

        /// <summary>
        /// 刪除投票
        /// </summary>
        /// <returns></returns>
        public bool DeleteVote()
        {
            return vda.DeleteVote(ID.ToString(), DeleteBy);
        }




        /// <summary>
        /// 根據修改的投票選項生成datatable，用於作為參數傳入procdure
        /// </summary>
        /// <returns></returns>
        private DataTable GetUpdateFromOpts()
        {
            if (_options != null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("id"));
                dt.Columns.Add(new DataColumn("optioncontext"));
                dt.Columns.Add(new DataColumn("ActionType"));
                dt.Columns.Add(new DataColumn("OptionTitle"));
                DataRow dr;
                for (int i = 0; i < _options.Count; i++)
                {
                    dr = dt.NewRow();
                    dr[0] = _options[i].ID;
                    dr[1] = _options[i].OptinContext;
                    dr[2] = _options[i].ActionType;
                    dr[3] = _options[i].OptionTitle;
                    dt.Rows.Add(dr);
                }
                return dt;
            }
            return null;
        }

        /// <summary>
        /// 將選項轉換為字符串，用於傳入數據庫
        /// </summary>
        /// <returns></returns>
        private string GetOptsStr()
        {
            if (_options != null)
            {
                string str = "";
                for (int i = 0; i < _options.Count; i++)
                {
                    str += _options[i].OptinContext + "|";
                }
                return str;
            }
            return null;
        }

      
    }
}
