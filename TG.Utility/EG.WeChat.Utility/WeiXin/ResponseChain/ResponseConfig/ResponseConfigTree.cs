using EG.WeChat.Utility.WeiXin.ResponseChain.CustomHandlers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace EG.WeChat.Utility.WeiXin.ResponseChain
{

    //------internal:

    #region 响应链树--配置文件型

    /// <summary>
    /// 响应链树--配置文件型<para />
    /// (节点信息的集合处理类；配置文件方式，根据配置文件进行处理)
    /// </summary>
    internal class ResponseConfigTree_ConfigurationSupport
    {

        //---------Members---------

        #region 配置节点  的索引

        /// <summary>
        /// 配置节点  的索引 (NodeId,Node配置)
        /// </summary>
        private static Dictionary<string, ResponseNodeConfig> ConfigNodeDic = new Dictionary<string, ResponseNodeConfig>();

        #endregion

        //---------Control---------

        #region 构造函数

        /* 思路备忘：
         * 1.“加载配置文件”和“填充ResponseConfigNode”，抽取为Static构造，
         *   好处是全局只需要加载一次。
         * 2.然后，后续需要“替换具体CustomHandler的DLL之后，无须重启，立即生效”，
         *   则可以考虑将Static构造变成普通的构造函数。
         */

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static ResponseConfigTree_ConfigurationSupport()
        {
            //读取配置，并填充配置信息到集合
            ResponseConfiguration.LoadConfig_ResponseChain(ConfigNodeDic);

            //订阅 配置发生变化
            ResponseConfiguration.ConfigurationChanged += ResponseConfiguration_ConfigurationChanged;
            Logger.Log4Net.Info("ResponseConfigTree_ConfigurationSupport Subscribe[应答链配置更新通知]。");
        }

        #endregion

        #region 根据NodeID获取配置

        /// <summary>
        /// 根据NodeID获取配置
        /// </summary>
        /// <param name="nodeID">节点ID</param>
        /// <returns>配置</returns>
        public ResponseNodeConfig GetConfigByNodeID(string nodeID)
        {
            lock (((ICollection)ConfigNodeDic).SyncRoot)
            {
                if (ConfigNodeDic.ContainsKey(nodeID))
                    return ConfigNodeDic[nodeID];
                else
                    return null;
            }
        }

        #endregion


        #region 配置发生变化时

        /// <summary>
        /// 配置发生变化时
        /// </summary>
        static void ResponseConfiguration_ConfigurationChanged()
        {
            Logger.Log4Net.Info("应答链配置发生更新，ResponseConfigTree_ConfigurationSupport Reload。");

            lock (((ICollection)ConfigNodeDic).SyncRoot)
                ResponseConfiguration.LoadConfig_ResponseChain(ConfigNodeDic);
        }

        #endregion
    }

    #endregion


    //------public:

    #region 配置操作类

    /// <summary>
    /// 配置操作类<parm />
    /// (提供存储方法、用于调试开发)
    /// </summary>
    public sealed class ResponseConfiguration
    {
        //---------Members---------

        #region 配置文件路径

        /// <summary>
        /// 配置文件路径(允许外部修改)
        /// </summary>
        public static string CONFIG_FILEPATH = @"~\App_Data\Config\ResponseChainConfig.xml";

        #endregion


        //---------Evnets---------

        #region 配置更改触发事件

        /// <summary>
        /// 配置发生改变的事件 <para />
        /// (请订阅者，自己留意取消订阅的时机，规避静态事件的内存泄露)
        /// </summary>
        internal static event Action ConfigurationChanged;

        #endregion


        //---------Control---------

        #region 读取配置文件
        /// <summary>
        /// 从配置文件，读取 配置信息 。(非线程安全，请外部自己处理。)
        /// </summary>
        /// <param name="ConfigNodeDic">结果集合</param>
        public static void LoadConfig_ResponseChain(Dictionary<string, ResponseNodeConfig> ConfigNodeDic)
        {
            //初始化
            if (ConfigNodeDic == null)
                ConfigNodeDic = new Dictionary<string, ResponseNodeConfig>();
            else
                ConfigNodeDic.Clear();

            //读取配置文件
            string fullPath = System.Web.Hosting.HostingEnvironment.MapPath(CONFIG_FILEPATH);
            if (System.IO.File.Exists(fullPath) == false)
            {
                return;
            }

            //开始解析
            XDocument xdoc = XDocument.Load(fullPath);
            var nodeList = xdoc.Root.Elements("Node");
            foreach (XElement item in nodeList)
            {
                try
                {
                    string nodeID = item.Element("NodeID").Value;
                    IHandlerConfig DealingHandlerConfig = ConfigHelper.GetIConfigFromXElement(item.Element("DealingHandler")) as IHandlerConfig;
                    IHandlerConfig DoneHandlerConfig = ConfigHelper.GetIConfigFromXElement(item.Element("DoneHandler")) as IHandlerConfig;

                    //##生成Node节点
                    ResponseNodeConfig node = new ResponseNodeConfig(nodeID, DealingHandlerConfig, DoneHandlerConfig);

                    //##添加到静态集合
                    lock (((ICollection)ConfigNodeDic).SyncRoot)
                        ConfigNodeDic[nodeID] = node;
                }
                catch (Exception ex)
                {
                    Logger.Log4Net.Error("LoadConfig_ResponseChain 过程发生异常。",ex);
                    continue;
                }
            }
        }

        #endregion

        #region 写入配置文件
        /// <summary>
        /// 将 配置信息 ，写入配置文件。
        /// </summary>
        /// <param name="Nodes">配置信息</param>
        public static void SaveConfig_ResponseChain(ResponseNodeConfig[] Nodes)
        {
            //创建xml文档
            XDocument xdoc = new XDocument
            (
                new XElement("ResponseChaiConfig")
            );

            //解析每个节点
            foreach (ResponseNodeConfig Node in Nodes)
            {
                try
                {
                    //##结果集合
                    List<XElement> result = new List<XElement>();

                    //##记录NodeID
                    result.Add(new XElement("NodeID", Node.NodeID));

                    //##记录DealingHandler
                    if (Node.DealingHandlerConfig != null)
                    {
                        XElement xDealingHandler = new XElement("DealingHandler");
                        Node.DealingHandlerConfig.SerializeConfigToXmlNode(xDealingHandler);
                        xDealingHandler.SetAttributeValue("Type", Node.DealingHandlerConfig.GetType().FullName);
                        result.Add(xDealingHandler);
                    }

                    //##记录DoneHandler
                    if (Node.DoneHandlerConfig != null)
                    {
                        XElement xDoneHandler = new XElement("DoneHandler");
                        Node.DoneHandlerConfig.SerializeConfigToXmlNode(xDoneHandler);
                        xDoneHandler.SetAttributeValue("Type", Node.DoneHandlerConfig.GetType().FullName);
                        result.Add(xDoneHandler);
                    }

                    //##全部成功之后，再附加到根节点
                    xdoc.Root.Add(new XElement("Node", result.ToArray()));
                }
                catch (Exception)
                {
                    continue;
                }
            }

            //存储到文件系统
            xdoc.Save(System.Web.Hosting.HostingEnvironment.MapPath(CONFIG_FILEPATH), SaveOptions.DisableFormatting);

            //触发事件通知
            try
            {
                Action eh = ConfigurationChanged;
                if (eh != null)
                {
                    eh();
                }
            }
            catch(Exception ex)
            {
                //忽略订阅者处理过程的异常
                Logger.Log4Net.Error("应答链触发[配置更新事件]有异常。",ex);
            }
        }

        #endregion


        #region TestOnly 代替界面,进行配置
        /* 请保留此测试方法。 */

        /// <summary>
        /// 代替界面,进行配置(TestOnly)
        /// </summary>
        internal static void TestOnly_DoConfiguration()
        {
            Dictionary<string, ResponseNodeConfig> ConfigNodeDic = new Dictionary<string, ResponseNodeConfig>();

            //##RootNode
            {
                ResponseNodeConfig rootnode = new ResponseNodeConfig(ConstString.ROOT_NODE_ID);
                TextMenuHandlerConfig menusHandler = new TextMenuHandlerConfig();
                menusHandler.Menus.Add(new DictionaryEntry("1", "1"));
                menusHandler.Menus.Add(new DictionaryEntry("2", "2"));
                menusHandler.Menus.Add(new DictionaryEntry("3", "3"));
                menusHandler.Menus.Add(new DictionaryEntry("9", "9"));
                ResponseTextMessageConfig readyMessage = new ResponseTextMessageConfig()
                {
                    Context = @"請輸入以下指令(數位)：
1 查詢即時黄金產品資訊；
2 查询/設置 交易帳戶；
3 參與【日日有禮送】的抽獎活動；
9 查詢 集團公開信息。

*溫馨提示您：
在任意時刻輸入數位0，可以隨時返回主功能表。"
                };
                menusHandler.ReadyMessageConfig = readyMessage;
                rootnode.DealingHandlerConfig = menusHandler;
                ConfigNodeDic[ConstString.ROOT_NODE_ID] = rootnode;
            }

            //##1
            {
                ResponseNodeConfig node_1 = new ResponseNodeConfig("1");
                node_1.DealingHandlerConfig = new CustomHandlerConfig() { HandlerTypeName = "EG.WeChat.Web.Service.ResponseChain.Handlers.CustomHandlers.CustomHandler_QueryEGProductPrice" };
                node_1.DoneHandlerConfig = new DefaultDoneHandlerConfig() { NodeId = "1", TipText = "查詢其他產品" };
                ConfigNodeDic["1"] = node_1;
            }

            //##2
            {
                ResponseNodeConfig node_2 = new ResponseNodeConfig("2");
                TextMenuHandlerConfig menusHandler = new TextMenuHandlerConfig();
                menusHandler.Menus.Add(new DictionaryEntry("1", "2.1"));
                menusHandler.Menus.Add(new DictionaryEntry("2", "2.2"));
                menusHandler.Menus.Add(new DictionaryEntry("3", "2.3"));
                ResponseTextMessageConfig readyMessage = new ResponseTextMessageConfig()
                {
                    Context = @"請輸入以下指令(數位)：
1 查詢 交易帳戶昵称；
2 設置 交易帳戶昵称；
3 查詢 交易帳戶的歷史操作記錄。"
                };
                menusHandler.ReadyMessageConfig = readyMessage;
                node_2.DealingHandlerConfig = menusHandler;
                ConfigNodeDic["2"] = node_2;
            }

            //##3
            {
                ResponseNodeConfig node_3 = new ResponseNodeConfig("3");
                node_3.DealingHandlerConfig = new DefaultDealingHandlerConfig() { DataType = DataTypes.Text, RawData = @"~\(≧▽≦)/~ 恭喜！您獲得1張[消費代金券$1]。" + "\r\n請及時查收您的交易帳戶。" };
                ConfigNodeDic["3"] = node_3;
            }

            //##2.2
            {
                ResponseNodeConfig node_2_2 = new ResponseNodeConfig("2.2");
                node_2_2.DealingHandlerConfig = new CustomHandlerConfig() { HandlerTypeName = "EG.WeChat.Web.Service.ResponseChain.Handlers.CustomHandlers.CustomHandler_UpdateAccountNickname" };
                node_2_2.DoneHandlerConfig = new DefaultDoneHandlerConfig() { NodeId = "2", TipText = "返回上一級菜單" };
                ConfigNodeDic["2.2"] = node_2_2;
            }

            //##2.3
            {
                ResponseNodeConfig node_2_3 = new ResponseNodeConfig("2.3");
                node_2_3.DealingHandlerConfig = new CustomHandlerConfig() { HandlerTypeName = "EG.WeChat.Web.Service.ResponseChain.Handlers.CustomHandlers.CustomHandler_QueryAccountActions" };
                node_2_3.DoneHandlerConfig = new DefaultDoneHandlerConfig() { NodeId = "2", TipText = "返回上一級菜單" };
                ConfigNodeDic["2.3"] = node_2_3;
            }

            //##9
            {
                ResponseNodeConfig node_9 = new ResponseNodeConfig("9");
                TextMenuHandlerConfig menusHandler = new TextMenuHandlerConfig();
                menusHandler.Menus.Add(new DictionaryEntry("1", "9.1"));
                menusHandler.Menus.Add(new DictionaryEntry("2", "9.2"));
                menusHandler.Menus.Add(new DictionaryEntry("3", "9.3"));
                ResponseTextMessageConfig readyMessage = new ResponseTextMessageConfig()
                {
                    Context = @"請輸入以下指令(數位)：
1 查詢 集團網址；
2 查詢 集團總部地址；
3 查詢 集團總部聯繫電話。"
                };
                menusHandler.ReadyMessageConfig = readyMessage;
                node_9.DealingHandlerConfig = menusHandler;
                ConfigNodeDic["9"] = node_9;
            }

            //##9.1
            {
                ResponseNodeConfig node_9_1 = new ResponseNodeConfig("9.1");
                node_9_1.DealingHandlerConfig = new DefaultDealingHandlerConfig() { DataType = DataTypes.Text, RawData = @"www.emperorgroup.com" };
                ConfigNodeDic["9.1"] = node_9_1;
            }
            //##9.2
            {
                ResponseNodeConfig node_9_2 = new ResponseNodeConfig("9.2");
                node_9_2.DealingHandlerConfig = new DefaultDealingHandlerConfig() { DataType = DataTypes.Text, RawData = @"香港灣仔軒尼詩道 288 號" };
                ConfigNodeDic["9.2"] = node_9_2;
            }
            //##9.3
            {
                ResponseNodeConfig node_9_3 = new ResponseNodeConfig("9.3");
                node_9_3.DealingHandlerConfig = new DefaultDealingHandlerConfig() { DataType = DataTypes.Text, RawData = @"852-28356688" };
                ConfigNodeDic["9.3"] = node_9_3;
            }

            SaveConfig_ResponseChain(ConfigNodeDic.Values.ToArray());
        }

        #endregion
    }
        
    #endregion

    #region 配置信息的辅助处理

    /// <summary>
    /// 配置信息的辅助处理
    /// </summary>
    public static class ResponseConfigurationExtension
    {

        /// <summary>
        /// Handler配置类，辅助获取当前配置信息的概要描述
        /// </summary>
        /// <typeparam name="T_IHandler"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        public static string GetSummary(this IHandlerConfig config)
        {
            if (config is DefaultDealingHandlerConfig)
            {
                var realConfig = config as DefaultDealingHandlerConfig;

                switch (realConfig.DataType)
                {
                    case DataTypes.Text:
                        return String.Format("呈現指定的文本({0})", GetShortString(realConfig.RawData.ToString()));

                    case DataTypes.News:
                        return String.Format("呈現指定的文章({0})", GetShortString(((ArticleCan)realConfig.RawData).Title));

                    default:
                        return DescriptionAttribute.GetDescription(config.GetType().GetMethod("CreateInstanceFromConfig").ReturnType);
                }
            }
            else if (config is DefaultDoneHandlerConfig)
            {
                var realConfig = config as DefaultDoneHandlerConfig;

                if (realConfig.NodeId == ConstString.ROOT_NODE_ID)
                    return "輸入任意文字，返回主功能表";
                else
                    return String.Format("輸入#時，{0}(節點:{1})", realConfig.TipText, realConfig.NodeId);
            }
            else if (config is TextMenuHandlerConfig)
            {
                var realConfig = config as TextMenuHandlerConfig;
                string nodes = String.Join(" | ",realConfig.Menus.Select(keyvalue => keyvalue.Value));

                return String.Format("跳轉菜單({0}個功能表項目，節點:{1})", realConfig.Menus.Count, nodes);
            }
            else if (config is CustomHandlerConfig)
            {
                Type targetType = CustomHandlerConfig.GetICustomHandlerTypeFromCurrentDomain(((CustomHandlerConfig)config).HandlerTypeName);
                return DescriptionAttribute.GetDescription( targetType );
            }
            else
            {
                return DescriptionAttribute.GetDescription(config.GetType().GetMethod("CreateInstanceFromConfig").ReturnType);
            }
        }

        /// <summary>
        /// 获取短字符串
        /// </summary>
        private static string GetShortString(string content)
        {
            /* 超过N个字符长度时，后面用...代替 */
            const int LENGTH_LIMIT = 20;

            if (content.Length <= LENGTH_LIMIT)
                return content;
            else
                return content.Substring(0, LENGTH_LIMIT) + "...";
        }
    }
     

    #endregion

    
}
