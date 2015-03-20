using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EG.WeChat.Platform.BL.QYMailList;
using EG.WeChat.Platform.DA.QYVote;
namespace EG.WeChat.Platform.BL.QYVote
{
    public class VoteOption
    {
        public int ID { get; set; }
        public string OptinContext { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime DeleteDate { get; set; }
        public string DeleteBy { get; set; }
        public string ActionType { get; set; }
        public string OptionTitle { get; set; }
        private List<VoteMember> _VoteMember;

        public List<VoteMember> VoteMember
        {
            get
            {
                if (_VoteMember == null)
                {
                     VoteDA vda = new VoteDA();
                    _VoteMember = vda.TableToEntity<VoteMember>(vda.GetVoteMemberByVote(ID.ToString()));
                }

                return _VoteMember;
            }
            set { _VoteMember = value; }
        }


        

    }
}
