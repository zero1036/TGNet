using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Senparc.Weixin.Exceptions;
using Senparc.Weixin.QY.AdvancedAPIs;
using Senparc.Weixin.QY.CommonAPIs;
namespace EG.WeChat.Platform.BL.QYMailList
{
    public class QYConfig
    {
        private static string _corpId;

        public static string CorpId
        {
            get
            {

                // return _corpId; 
                return EG.WeChat.Utility.WeiXin.WeiXinConfiguration.cropId;

            }
            set { _corpId = value; }
        }

        private static string _Secret;

        public static string Secret
        {
            get
            {

                // return _Secret; 
                return EG.WeChat.Utility.WeiXin.WeiXinConfiguration.corpSecret;

            }
            set { _Secret = value; }
        }

        public static void RegistWX()
        {
            AccessTokenContainer.Register(CorpId, Secret);
        }

        private static string _invateMsg = "GZIT邀請你關注我們的企業微信號";

        public static string InvateMsg
        {
            get
            {

                return _invateMsg;
            }
            set { _invateMsg = value; }
        }

        private static int _voteagenid;

        public static int VoteAgenID
        {
            get
            {
                _voteagenid = 47;
                return _voteagenid;
            }
            set { _voteagenid = value; }
        }


        public static string VoteLink = @"http://testwechat.cloudapp.net/VoteManage/VoteMobelForShow/";
        public static string NoticeLink = @"http://testwechat.cloudapp.net/Notice/ShowNoticeReg/";
    }
}
