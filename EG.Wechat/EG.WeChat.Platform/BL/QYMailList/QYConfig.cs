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
            get { 

               // return _corpId; 
                return "wx4b192556da80dfcc"; 
            
            }
            set { _corpId = value; }
        }

        private static string _Secret;

        public static string Secret
        {
            get { 
                
               // return _Secret; 
                return "7h7LcZJyOVQB_iSOx1t07dG-rr3TQa6gcGM5Dcvl4I5_F8h3y8QXIvWqQHpLsDF4"; 
            
            }
            set { _Secret = value; }
        }

        public static void RegistWX()
        {
            AccessTokenContainer.Register(CorpId, Secret);
        }

        private static string _invateMsg="GZIT邀請你關注我們的企業微信號";

        public static string InvateMsg
        {
            get {

                return _invateMsg; 
            }
            set { _invateMsg = value; }
        }

    }
}
