【Results】目录，用于 应答链的服务类。

ResponseService服务，是对外可见的顶层对象；
NodeInstanceService服务，是程序集内可见。主要是想将“对应节点”的逻辑再分离出一层，方便人的思维理解。




-------------- [两种ResponseService的使用范例] --------------
* 以下两种的范例，最终效果是一致的。

----[1] SDK方式的ResponseService (ResponseService_MemorySupport)
----## 1.定义“创建实例的外部函数”：
        /// <summary>
        /// 创建实例的外部函数
        /// </summary>
        public static NodeInstanceService CreateInstance(string nodeId)
        {
            //根据NodeId进行处理
            switch (nodeId)
            {
                case ConstString.ROOT_NODE_ID:
                    {
                        NodeInstanceService rootNode    = new NodeInstanceService(nodeId);
                        rootNode.DealingHandler         = TextMenuHandler.CreateInstance(TextMenuHandler.TextMenuItem.CreateItem("1", "1"),
                                                                                         TextMenuHandler.TextMenuItem.CreateItem("2", "2"),
                                                                                         TextMenuHandler.TextMenuItem.CreateItem("3", "3"),
                                                                                         TextMenuHandler.TextMenuItem.CreateItem("9", "9")
                                                                                         );
                        rootNode.DealingHandler.ReadyMessage = new ResponseTextMessage(@"請輸入以下指令(數位)：
1 查詢即時黄金產品資訊；
2 查询/設置 交易帳戶；
3 參與【日日有禮送】的抽獎活動；
9 查詢 集團公開信息。

*溫馨提示您：
在任意時刻輸入數位0，可以隨時返回主功能表。");
                        return rootNode;
                    }

                case "1":
                    {
                        NodeInstanceService node_1  = new NodeInstanceService(nodeId);
                        node_1.DealingHandler       = new CustomHandler_QueryEGProductPrice();
                        node_1.DonedHandler         = new DefaultDoneHandler("查詢其他產品", "1");
                        return node_1;

                    }
                case "2":
                    {
                        NodeInstanceService node_2      = new NodeInstanceService(nodeId);
                        node_2.DealingHandler           = TextMenuHandler.CreateInstance(TextMenuHandler.TextMenuItem.CreateItem("2.1", "1"),
                                                         TextMenuHandler.TextMenuItem.CreateItem("2.2", "2"),
                                                         TextMenuHandler.TextMenuItem.CreateItem("2.3", "3")
                                                         );
                        node_2.DealingHandler.ReadyMessage = new ResponseTextMessage(@"請輸入以下指令(數位)：
1 查詢 交易帳戶昵称；
2 設置 交易帳戶昵称；
3 查詢 交易帳戶的歷史操作記錄。");

                        return node_2;

                    }
                case "3":
                    {
                        NodeInstanceService node_3      = new NodeInstanceService(nodeId);
                        node_3.DealingHandler           = DefaultDealingHandler.CreateInstance_Text(@"~\(≧▽≦)/~ 恭喜！您獲得1張[消費代金券$1]。" + "\r\n請及時查收您的交易帳戶。");
                        return node_3;
                    }

                case "2.2":
                    {
                        NodeInstanceService node_2_2    = new NodeInstanceService(nodeId);
                        node_2_2.DealingHandler         = new CustomHandler_UpdateAccountNickname();
                        node_2_2.DonedHandler           = new DefaultDoneHandler("返回上一級菜單", "2");
                        return node_2_2;
                    }
                case "2.3":
                    {
                        NodeInstanceService node_2_3    = new NodeInstanceService(nodeId);
                        node_2_3.DealingHandler         = new CustomHandler_QueryAccountActions();
                        node_2_3.DonedHandler = new DefaultDoneHandler("返回上一級菜單", "2");
                        return node_2_3;
                    }

                case "4":
                    {

                        return null;
                    }

                case "9":
                    {

                        NodeInstanceService node_9 = new NodeInstanceService(nodeId);
                        node_9.DealingHandler = TextMenuHandler.CreateInstance(TextMenuHandler.TextMenuItem.CreateItem("9.1", "1"),
                                                         TextMenuHandler.TextMenuItem.CreateItem("9.2", "2"),
                                                         TextMenuHandler.TextMenuItem.CreateItem("9.3", "3"),
                                                         TextMenuHandler.TextMenuItem.CreateItem("9.4", "4")
                                                         );
                        node_9.DealingHandler.ReadyMessage = new ResponseTextMessage(@"請輸入以下指令(數位)：
1 查詢 集團網址；
2 查詢 集團總部地址；
3 查詢 集團總部聯繫電話。");

                        return node_9;
                    }

                case "9.1":
                    {
                        NodeInstanceService node_9_1    = new NodeInstanceService(nodeId);
                        node_9_1.DealingHandler         = DefaultDealingHandler.CreateInstance_Text(@"www.emperorgroup.com");
                        return node_9_1;
                    }
                case "9.2":
                    {
                        NodeInstanceService node_9_2    = new NodeInstanceService(nodeId);
                        node_9_2.DealingHandler         = DefaultDealingHandler.CreateInstance_Text(@"香港灣仔軒尼詩道 288 號");
                        return node_9_2;
                    }
                case "9.3":
                    {
                        NodeInstanceService node_9_3    = new NodeInstanceService(nodeId);
                        node_9_3.DealingHandler         = DefaultDealingHandler.CreateInstance_Text(@"852-28356688");
                        return node_9_3;
                    }

                default:
                    {
                        NodeInstanceService node_other  = new NodeInstanceService(nodeId);
                        node_other.DealingHandler       = DefaultDealingHandler.CreateInstance_Text("抱歉，當前指令現在無法為您提供服務。");
                        return node_other;
                    }
            }
        }
----## 2.创建服务实例，并赋值上述函数：
	ResponseService_MemorySupport.CreateOrGetService(messageContext, CreateInstance);



----[2]配置文件方式的ResponseService (ResponseService_ConfigurationSupport)
----## 1.进行配置：
	    /// <summary>
        /// 执行配置
        /// </summary>
		public static void DoConfiguration()
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
                node_2.DealingHandlerConfig     = menusHandler;
                ConfigNodeDic["2"]              = node_2;
            }

            //##3
            {
                ResponseNodeConfig node_3   = new ResponseNodeConfig("3");
                node_3.DealingHandlerConfig = new DefaultDealingHandlerConfig() { DataType = DataTypes.Text, RawData = @"~\(≧▽≦)/~ 恭喜！您獲得1張[消費代金券$1]。" + "\r\n請及時查收您的交易帳戶。" };
                ConfigNodeDic["3"]          = node_3;
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

            ResponseConfiguration.SaveConfig_ResponseChain(ConfigNodeDic.Values.ToArray());
        }
----## 2.直接创建配置类型的服务实例 ：
		ResponseService_ConfigurationSupport(messageContext);

