using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EG.WeChat.Platform.DA.QYVote;
using System.Data;
namespace EG.WeChat.Platform.BL.QYVote
{
    public class MemberVote
    {
        public int ID { get; set; }
        public int  MemberID { get; set; }
        public int VoteID { get; set; }
        public List<VoteOption> opts { get; set; }
        public string optids { get; set; }
        private VoteDA _vda;
        public VoteDA vda
        {
            get
            {
                if (_vda == null)
                {
                    _vda = new VoteDA();
                }
                return _vda;
            }
            
        }
        public bool AddMemberVote(string userid)
        {

            
            return vda.AddMemberVote(VoteID.ToString(),MemberID.ToString(),userid,optids);
        }

        public MemberVote()
        {

        }
        public MemberVote(string mid,string vid)
        {
            MemberID = int.Parse(mid);
            VoteID = int.Parse(vid);
            opts = vda.TableToEntity<QYVote.VoteOption>(vda.GetMemberVote(mid, vid));
           
            
        }
    }
}
