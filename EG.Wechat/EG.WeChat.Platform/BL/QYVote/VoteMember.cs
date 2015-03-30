using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EG.WeChat.Platform.DA.QYVote;
using Senparc.Weixin.QY.AdvancedAPIs;
using Senparc.Weixin.QY.CommonAPIs;
using Senparc.Weixin.QY.AdvancedAPIs.MailList;
using System.Data;
using EG.WeChat.Platform.BL.QYMailList;
namespace EG.WeChat.Platform.BL.QYVote
{
    public class VoteMember:QYMemberBL
    {

        public DateTime VoteDate { get; set; }

        public string OptionTitle { get; set; }

        public int VoteOptionID { get; set; }
        public string OptinContext { get; set; }
        public int VoteHeadId { get; set; }

    }
}
