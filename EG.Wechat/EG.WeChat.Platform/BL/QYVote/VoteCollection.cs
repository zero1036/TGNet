using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EG.WeChat.Platform.DA.QYVote;
using Senparc.Weixin.QY.AdvancedAPIs;
using Senparc.Weixin.QY.CommonAPIs;
using Senparc.Weixin.QY.AdvancedAPIs.MailList;
namespace EG.WeChat.Platform.BL.QYVote
{
    public class VoteCollection
    {
        public List<VoteBL> Votes { get; set; }

        public VoteCollection()
        {
            VoteDA vda = new VoteDA();
            Votes = vda.TableToEntity<VoteBL>(vda.GetVote());
        }
    }
}
