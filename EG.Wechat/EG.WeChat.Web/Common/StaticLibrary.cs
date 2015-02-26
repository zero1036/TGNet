using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace EG.WeChat.Web
{
    public class StaticLibrary
    {

        private static Dictionary<string, HttpSessionState> _activeSession;


        /// <summary>
        /// 活跃回话，用于控制在线用户，单点登录可在此属性扩展，现在是支持多点登录。
        /// </summary>
        public static Dictionary<string, HttpSessionState> ActiveSession
        {
            get
            {
                if (_activeSession == null)
                {
                    _activeSession = new Dictionary<string, HttpSessionState>();
                }
                return _activeSession;
            }
            set
            {
                _activeSession = value;
            }
        }



    }
}