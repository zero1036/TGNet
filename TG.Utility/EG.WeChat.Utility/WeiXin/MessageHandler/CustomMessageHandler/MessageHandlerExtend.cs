using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EG.WeChat.Utility.WeiXin
{
    /// <summary>
    /// 消息处理扩展功能
    /// </summary>
    public static class MessageHandlerExtend
    {

        #region 会话菜单 触发 应答链

        public static class ClickEvent2ResponseChain
        {
            #region 说明 关联的文本

            /* 定义格式为:  JumpToResponseChain[节点ID]    */

            #endregion

            #region 判断EventKey是否为“会话菜单触发应答链”，如果是，同时获取节点ID

            static Regex regex = new Regex(@"^JumpToResponseChain\[(?<NodeId>(\d+([\.]\d+)*)+)\]$",
                                           RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

            /// <summary>
            /// 判断EventKey是否为“会话菜单触发应答链”
            /// </summary>
            /// <param name="eventKey">会话菜单触发的eventKey</param>
            /// <param name="nodeId">节点ID</param>
            /// <returns>True为属于预期数据;False为不符合</returns>
            public static bool CheckThenGetNodeId(string eventKey,out string nodeId)
            {
                Match mc = regex.Match(eventKey);

                if (mc.Success == false)
                {
                    //##匹配失败
                    nodeId = String.Empty;
                    return false;
                }
                else
                {
                    //##匹配成功
                    nodeId = mc.Groups["NodeId"].Value;
                    return true;
                }
            }

            #endregion

        }


        #endregion


    }
}
