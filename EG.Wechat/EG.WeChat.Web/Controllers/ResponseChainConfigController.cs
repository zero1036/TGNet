using System;
using System.IO;
using System.Web.Mvc;
using System.Xml.Linq;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Agent;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Helpers;
using System.Collections.Generic;
using EG.WeChat.Utility.WeiXin.ResponseChain;
using System.Linq;
using System.Collections;
using System.Text;
using EG.WeChat.Utility.WeiXin.ResponseChain.CustomHandlers;
using EG.WeChat.Utility.WeiXin.ResponseChain.Handlers.CustomHandlers;
using EG.WeChat.Platform.BL;

namespace EG.WeChat.Web.Controllers
{
    /// <summary>
    /// 应答链配置的Controller
    /// </summary>
    public class ResponseChainConfigController : Controller
    {
        /* ViewData目前使用情况：
         * # "ConfigNodeList"，      ResponseNodeConfig集合；
         * # "CurrentEditedNode"，   当前正在编辑状态中的节点（编辑状态才赋值）；
         * # "AllCustomHandlerTypes" 所有自定义处理器的类型集合；
         * 
         * 
         * Session目前使用情况:
         * # "AdvantanceConfig_CustomHandler_Result",   自定义处理器的参数配置的结果  Dictionary<Type,IConfigClassOfCustomHandler>
         * 
         */


        //------------Members-------------

        #region 数据成员

        /// <summary>
        /// 配置信息的集合
        /// </summary>
        Dictionary<string, ResponseNodeConfig> ConfigNodeList
        {
            get
            {
                var currentSession = System.Web.HttpContext.Current.Session;

                Dictionary<string, ResponseNodeConfig> result = currentSession["ResponseChainConfigController_ConfigNodeList"] as Dictionary<string, ResponseNodeConfig>;
                //当列表为空时，进行创建
                if (result == null)
                {
                    currentSession["ResponseChainConfigController_ConfigNodeList"] = result = new Dictionary<string, ResponseNodeConfig>();

                    lock (((ICollection)result).SyncRoot)
                    {
                        ResponseConfiguration.LoadConfig_ResponseChain(result);
                        if (result.Count() <= 0)
                        {
                            //添加默认的根节点
                            result.Add(ConstString.ROOT_NODE_ID, new ResponseNodeConfig(ConstString.ROOT_NODE_ID));
                        }
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// 获取当前的配置信息列表
        /// </summary>
        /// <returns></returns>
        List<ResponseNodeConfig> GetList()
        {
            var result = ConfigNodeList.Values.ToList();
            result.Sort((x, y) =>
            {
                //相等的情况
                if (String.Equals(x.NodeID, y.NodeID))
                    return 0;

                //根节点 始终最前
                if (x.NodeID == ConstString.ROOT_NODE_ID)
                    return -1;
                if (y.NodeID == ConstString.ROOT_NODE_ID)
                    return 1;

                //按照数字，从小到大
                char[] splitChars = new char[] { '.' };
                string[] numX = x.NodeID.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
                string[] numY = y.NodeID.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
                //##比较长度一样的部分( 1.1  1.3 )
                for (int i = 0; i < Math.Min(numX.Length, numY.Length); i++)
                {
                    try
                    {
                        int currentX = Convert.ToInt32(numX[i]), currentY = Convert.ToInt32(numY[i]);       //NodeIdValidator已经检测过格式，因此这里就不检查“是否转为有效数字”。
                        int ret = currentX - currentY;
                        if (ret != 0)       //##比较出大小，则直接返回；比较不出，则继续检查下一个长度
                            return ret;
                    }
                    catch (Exception)
                    {
                        return 0;
                    }
                }
                //##长度一致的部分都相等，则总长度大的在后( 1.2   1.2.1  则后者在后  )
                return numX.Length - numY.Length;
            });
            return result;
        }

        #endregion

        #region 自定义处理器高级设置的URL关系

        /// <summary>
        /// 自定义处理器高级设置的URL关系
        /// </summary>
        ResponseChainConfig_CustomHandlerAdvantanceConfig_Urls CustomHandlerAdvantanceConfigUrls = new ResponseChainConfig_CustomHandlerAdvantanceConfig_Urls();

        #endregion


        //------------Control-------------

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public ResponseChainConfigController()
        {

        }
        #endregion

        #region 默认页面

        /// <summary>
        /// 默认页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            LoadDatas();
            return View();
        }

        #endregion

        #region 添加删除 节点

        /// <summary>
        /// 删除指定的节点
        /// </summary>
        /// <returns></returns>
        public ActionResult Delete(string nodeID, string CallBackIndexView = "Index")
        {
            if (ConfigNodeList != null)
                lock (((ICollection)ConfigNodeList).SyncRoot)
                {
                    if (nodeID != ConstString.ROOT_NODE_ID && ConfigNodeList.ContainsKey(nodeID))
                    {
                        #region 自定义处理器的额外处理

                        //自定义处理的参数配置
                        if (ConfigNodeList[nodeID].DealingHandlerConfig is CustomHandlerConfig)
                        {
                            CustomHandlerConfig hd = ConfigNodeList[nodeID].DealingHandlerConfig as CustomHandlerConfig;
                            Type targetType = CustomHandlerConfig.GetICustomHandlerTypeFromCurrentDomain(hd.HandlerTypeName);
                            bool isConfigable = typeof(ICustomHandlerConfigable).IsAssignableFrom(targetType);
                            if (isConfigable)
                            {
                                //获取配置类
                                Type configClassType = ConfigClassOfCustomHandlerHelper.GetConfigClassType(targetType);
                                if (configClassType != null)
                                {
                                    //加载
                                    XDocument xdoc = ConfigClassOfCustomHandlerHelper.LoadConfigByType(configClassType);
                                    ConfigClassOfCustomHandlerHelper.RemoveConfig(xdoc, nodeID);
                                    //保存
                                    ConfigClassOfCustomHandlerHelper.SaveConfigByType(configClassType, xdoc);
                                }
                            }
                        }

                        #endregion

                        ConfigNodeList.Remove(nodeID);

                        //写入到配置文件
                        ResponseConfiguration.SaveConfig_ResponseChain(ConfigNodeList.Values.ToArray());

                    }
                }

            LoadDatas();
            return View(CallBackIndexView);
        }

        /// <summary>
        /// 添加新的节点
        /// </summary>
        /// <returns></returns>
        public ActionResult AddNewNode(string CallBackIndexView = "Index")
        {
            //获取合法的新节点ID
            int targetDefaultNodeId = 0;
            lock (((ICollection)ConfigNodeList).SyncRoot)
            {
                do
                {
                    targetDefaultNodeId++;
                } while (ConfigNodeList.ContainsKey(targetDefaultNodeId.ToString()));

                //添加默认的数据
                ResponseNodeConfig newOne = new ResponseNodeConfig(targetDefaultNodeId.ToString());
                DefaultDealingHandlerConfig dealing = new DefaultDealingHandlerConfig()
                {
                    DataType = DataTypes.Text,
                    RawData = "呈現的文本"
                };
                newOne.DealingHandlerConfig = dealing;
                newOne.DoneHandlerConfig = new DefaultDoneHandlerConfig();
                ConfigNodeList.Add(newOne.NodeID, newOne);

                //写入到配置文件
                ResponseConfiguration.SaveConfig_ResponseChain(ConfigNodeList.Values.ToArray());

                ViewData["CurrentEditedNode"] = newOne;       //进入编辑状态
                Session["AdvantanceConfig_CustomHandler_Result"] = new Dictionary<Type, IConfigClassOfCustomHandler>();  //每次进去编辑状态，清空自定义处理器的参数配置结果
            }

            LoadDatas();
            ViewBag.CallBackIndexView = CallBackIndexView;
            return View("Index");
        }

        #endregion

        #region 编辑节点

        //编辑指定的节点
        public ActionResult Edit(string nodeID, string CallBackIndexView = "Index")
        {

            lock (((ICollection)ConfigNodeList).SyncRoot)
            {
                if (ConfigNodeList.ContainsKey(nodeID))
                {
                    ViewData["CurrentEditedNode"] = ConfigNodeList[nodeID];
                }
                else
                {
                    //并发的场合会出现，指定的ID，对应的节点已经不存在
                    //此时刷新界面，但不进去 编辑状态
                }
            }

            LoadDatas();
            ViewBag.CallBackIndexView = CallBackIndexView;
            Session["AdvantanceConfig_CustomHandler_Result"] = new Dictionary<Type, IConfigClassOfCustomHandler>();  //每次进去编辑状态，清空自定义处理器的参数配置结果
            return View("Index");
        }

        /// <summary>
        /// 保存编辑结果
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveEdit(Dictionary<string, string> parmDic)
        {
            /* Doubt: 只能序列化成 <Stirng,String>?  当使用<Stirng,dynamic>，无法为Value赋值"集合"/"JObject"；会转换成无法使用的 System.Object? 
             * 
             * 因此这里的“DefaultNews”和 “TextMenu”，
             * 从View层序列化成 JsonString，然后再在Controller还原。
             * 求解决方案。
             */

            //抽取基本数据
            string targetNodeId = parmDic["NodeID"];
            string NewNodeId = parmDic["NewNodeId"];
            string DealingHandler = parmDic["DealingHandler"];
            string DoneHandler = parmDic["DoneHandler"];

            //检查ID是否重复、格式是否正确
            if (targetNodeId != ConstString.ROOT_NODE_ID &&
                targetNodeId != NewNodeId)        //修改了NodeID时才检查
            {
                if (NodeIdValidator.IsValid(NewNodeId) == false)
                    return Json(new { IsSuccess = false, errorMessage = "節點ID格式不正確。必須為 x.y.z 序號格式。" });

                bool isExisted = false;
                lock (((ICollection)ConfigNodeList).SyncRoot)
                    isExisted = ConfigNodeList.ContainsKey(NewNodeId);

                if (isExisted)
                {
                    return Json(new { IsSuccess = false, errorMessage = "修改的節點ID已經存在，不可以重複。" });
                }
            }

            ResponseNodeConfig configNode = new ResponseNodeConfig(NewNodeId);
            StringBuilder errorMessage = new StringBuilder();

            //开始根据不同类型，解析具体的数据
            bool isNeedDoneHandler = true;      //标记是否需要Done阶段；因为有部分Dealing处理器，处理完则跳转节点，此时不需要再配置Done。
            #region DealingHandler

            switch (DealingHandler)
            {
                default:
                    errorMessage.AppendLine("找不到對應的Dealing類型。");
                    break;

                case "DefaultText":
                    {
                        DefaultDealingHandlerConfig HandlerResult = new DefaultDealingHandlerConfig();
                        HandlerResult.DataType = DataTypes.Text;
                        HandlerResult.RawData = parmDic["DealingHandler_DefaultText"];
                        configNode.DealingHandlerConfig = HandlerResult;
                    }
                    break;

                case "DefaultNews":
                    {
                        DefaultDealingHandlerConfig HandlerResult = new DefaultDealingHandlerConfig();
                        HandlerResult.DataType = DataTypes.News;
                        dynamic news = Newtonsoft.Json.Linq.JObject.Parse(parmDic["DealingHandler_DefaultNews"]);
                        HandlerResult.RawData = new ArticleCan(news.title.ToString(),
                                                                 news.description.ToString(),
                                                                 news.picUrl.ToString(),
                                                                 news.pageUrl.ToString());
                        configNode.DealingHandlerConfig = HandlerResult;
                    }
                    break;

                case "TextMenu":
                    {
                        TextMenuHandlerConfig HandlerResult = new TextMenuHandlerConfig();

                        //##提示文字
                        ResponseTextMessageConfig ready = new ResponseTextMessageConfig();
                        ready.Context = parmDic["DealingHandler_TextMenu_ReadyMessage"];
                        HandlerResult.ReadyMessageConfig = ready;

                        //##菜单项
                        var menus = Newtonsoft.Json.Linq.JObject.Parse(parmDic["DealingHandler_TextMenu_Menus"]);
                        foreach (var menu in menus)
                        {
                            string theNodeid = menu.Value["Id"].ToString().Replace("#", String.Empty);       //??Json序列化时，如果Key为纯数字（即使类型是字符串），会被排序。因此这个用#前缀处理。

                            //有效性检查
                            if (NodeIdValidator.IsValid(theNodeid) == false)
                            {
                                errorMessage.Append(theNodeid);
                                errorMessage.AppendLine("不是有效的節點ID格式。必須為 x.y.z 序號格式。");
                                break;
                            }

                            //重复检查(紧跟其下的目标跳转的节点ID，不能与当前节点ID一样，否则会直接循环跳转 ；  跨节点的，不限制。)
                            if (String.Equals(NewNodeId, theNodeid, StringComparison.OrdinalIgnoreCase))
                            {
                                errorMessage.Append("子節點");
                                errorMessage.Append(theNodeid);
                                errorMessage.AppendLine("，不能與當前節點的ID重複。");
                                break;
                            }

                            HandlerResult.Menus.Add(new DictionaryEntry(menu.Value["data"].ToString(), theNodeid));
                        }

                        configNode.DealingHandlerConfig = HandlerResult;
                        isNeedDoneHandler = false;        //标记不处理Done
                    }
                    break;

                case "CustomHandler":
                    {
                        CustomHandlerConfig HandlerResult = new CustomHandlerConfig();
                        HandlerResult.HandlerTypeName = parmDic["DealingHandler_CustomHandler"];
                        configNode.DealingHandlerConfig = HandlerResult;

                        //自定义处理的参数配置
                        Type targetType = CustomHandlerConfig.GetICustomHandlerTypeFromCurrentDomain(HandlerResult.HandlerTypeName);
                        bool isConfigable = typeof(ICustomHandlerConfigable).IsAssignableFrom(targetType);
                        if (isConfigable && Session["AdvantanceConfig_CustomHandler_Result"] != null)
                        {
                            var dictionary = Session["AdvantanceConfig_CustomHandler_Result"] as IDictionary<Type, IConfigClassOfCustomHandler>;
                            if (dictionary != null && dictionary.ContainsKey(targetType))
                            {
                                IConfigClassOfCustomHandler configData = dictionary[targetType] as IConfigClassOfCustomHandler;
                                if (configData != null)
                                {
                                    //获取配置类
                                    Type configClassType = ConfigClassOfCustomHandlerHelper.GetConfigClassType(targetType);
                                    if (configClassType != null)
                                    {
                                        //加载
                                        XDocument xdoc = ConfigClassOfCustomHandlerHelper.LoadConfigByType(configClassType);
                                        ConfigClassOfCustomHandlerHelper.RemoveConfig(xdoc, targetNodeId);
                                        ConfigClassOfCustomHandlerHelper.UpdateConfig(xdoc, NewNodeId, configData);

                                        //保存
                                        ConfigClassOfCustomHandlerHelper.SaveConfigByType(configClassType, xdoc);
                                    }
                                }
                            }
                        }
                    }
                    break;
            }

            #endregion

            #region DoneHandler

            if (isNeedDoneHandler)
                switch (DoneHandler)
                {
                    default:
                        errorMessage.AppendLine("找不到對應的Done類型。");
                        break;

                    case "DefaultRootNode":
                        {
                            //DefaultDoneHandlerConfig HandlerResult = new DefaultDoneHandlerConfig();
                            configNode.DoneHandlerConfig = null;
                        }
                        break;

                    case "DefaultTargetNode":
                        {
                            if (NodeIdValidator.IsValid(parmDic["DoneHandler_JumpNode"]) == false)
                            {
                                errorMessage.Append(parmDic["DoneHandler_JumpNode"]);
                                errorMessage.AppendLine("不是有效的節點ID格式。必須為 x.y.z 序號格式。");
                                break;
                            }

                            DefaultDoneHandlerConfig HandlerResult = new DefaultDoneHandlerConfig();
                            HandlerResult.NodeId = parmDic["DoneHandler_JumpNode"];
                            HandlerResult.TipText = parmDic["DoneHandler_TipText"];
                            configNode.DoneHandlerConfig = HandlerResult;
                        }
                        break;
                }

            #endregion

            //反馈结果
            if (errorMessage.Length <= 0)
            {
                SaveOneConfig(configNode, targetNodeId);
                return Json(new { IsSuccess = true });
            }
            else
                return Json(new { IsSuccess = false, errorMessage = errorMessage.ToString() });
        }

        /// <summary>
        /// 保存单个节点的配置
        /// </summary>
        private void SaveOneConfig(ResponseNodeConfig configNode, string oldNodeId)
        {
            lock (((ICollection)ConfigNodeList).SyncRoot)
            {
                //删除旧的
                if (ConfigNodeList.ContainsKey(oldNodeId))
                    ConfigNodeList.Remove(oldNodeId);

                //新增
                ConfigNodeList[configNode.NodeID] = configNode;
            }

            //写入到配置文件
            ResponseConfiguration.SaveConfig_ResponseChain(ConfigNodeList.Values.ToArray());
        }
        #endregion


        #region 页面需要加载的数据
        /// <summary>
        /// 页面需要加载的数据
        /// </summary>
        private void LoadDatas()
        {
            LoadAllCustomHandlerTypes();
            ViewData["ConfigNodeList"] = GetList();
        }

        #endregion

        #region 获取当前加载的所有 自定义处理器
        /// <summary>
        /// 获取当前加载的所有 自定义处理器
        /// </summary>
        private void LoadAllCustomHandlerTypes()
        {
            //获得加载的全部程序集，所有继承ICustomHandler的类型
            var ret = AppDomain.CurrentDomain.GetAssemblies().Where(ass => ass.IsDynamic == false)
                      .SelectMany((System.Reflection.Assembly assembly) =>
                      {
                          try
                          {
                              return assembly.GetExportedTypes();
                          }
                          catch (Exception)         //过滤掉 GetTypes失败的程序集
                          {
                              return new Type[0];
                          }
                      })
                      .Where(type => type != null && typeof(ICustomHandler).IsAssignableFrom(type) && type.IsClass)     //Class类型 + 继承ICustomHandler接口
                      .Distinct();

            //生成结果
            var result =
            ret.ToDictionary(
            type => type,
            type =>
            {
                DescriptionAttribute attribute = Attribute.GetCustomAttribute(type, typeof(DescriptionAttribute)) as DescriptionAttribute;
                //判断非空
                if (attribute == null)
                    return type.Name;
                else
                    return String.Format("{0} - {1}", type.Name, attribute.Description);
            });

            //绑定到UI
            ViewData["AllCustomHandlerTypes"] = result;
        }

        #endregion


        #region 树形预览

        /* 2015-01-27 
         * “树形”模拟“网状”的模型，存在“循环引用”的情景需要过滤，
         * 今天使用新的策略，重新调整呈现出来的效果：
         * 1.重复TextMenuHandler节点的过滤，限定为“单条链路出现重复时才启用过滤”(之前是过滤整棵树)。
         * 
         * 
         * 2015-01-27
         * 由于jquery.ztree的Json数据，“id字段”不可以重复，而我们希望节点可以重复出现，
         * 因此Json结果数据中，新增“NodeID”属性，而“id字段”按照规律生成，不再表示对应的NodeId.
         */

        /// <summary>
        /// 默认页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Preview()
        {
            return View("Preview");
        }

        /// <summary>
        /// 默认页面
        /// </summary>
        /// <returns></returns>
        public ActionResult TreeIndex()
        {
            return View("TreeIndex");
        }

        /// <summary>
        /// 用来标记"有父亲关联"的节点的集合 <para />
        /// (由于View每个会话独立，因此这个成员不会相互干扰，没有线程问题要处理)
        /// </summary>
        HashSet<string> UsefulNodes = new HashSet<string>();

        /// <summary>
        /// 获取树形结构
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetTreeStruct()
        {
            StringBuilder result = new StringBuilder();

            //遍历数据，并生成结构
            var datas = GetList();

            //##从根节点开始检索
            UsefulNodes.Clear();            
            result.AppendLine(GetTreeStruct_Helper_GenerateBranch(GetTreeStruct_Helper_GetConfigByNodeId(ConstString.ROOT_NODE_ID),     //配置对象
                                                                  ConstString.ROOT_NODE_ID,
                                                                  "-1",                                                                 //父节点ID 理解为-1吧
                                                                  new HashSet<string>()));                                                //用于记录，当前单链的ID们

            //##处理没有父亲的节点
            var select_unUsefulNodes = from config in ConfigNodeList.Values
                                       where UsefulNodes.Contains(config.NodeID) == false
                                       select config;
            ResponseNodeConfig[] unUsefulNodes = null;
            lock (((ICollection)ConfigNodeList).SyncRoot)
                unUsefulNodes = select_unUsefulNodes.ToArray();
            if (unUsefulNodes.Count() > 0)
            {//##存在这类节点时，才额外显示“未关联父亲”
                const string unUseParentId = "unUse";
                result.AppendLine(BuildTreeNodeJson(unUseParentId, "-1",
                                                    "<斷鏈的節點>", unUseParentId,
                                                    true, false, false));
                foreach (var item in unUsefulNodes)
                {
                    result.AppendLine(GetTreeStruct_Helper_Convert2Json(item, "unUse_" + item.NodeID, unUseParentId));
                }
            }

            //补充json头尾的括号
            result.Insert(0, "[");
            result.Append("]");

            var test = result.ToString();
            Console.WriteLine(test);

            return Content(result.ToString());
        }

        /// <summary>
        /// 根据节点ID获取配置项
        /// </summary>
        /// <param name="nodeId">节点ID</param>
        /// <returns>配置项</returns>
        private ResponseNodeConfig GetTreeStruct_Helper_GetConfigByNodeId(string nodeId)
        {
            lock (((ICollection)ConfigNodeList).SyncRoot)
            {
                if (ConfigNodeList.ContainsKey(nodeId) == false)
                    return null;
                else
                    return ConfigNodeList[nodeId];
            }
        }

        /// <summary>
        /// 将节点信息转化为Json格式
        /// </summary>
        /// <param name="nodeConfig">节点配置</param>
        /// <param name="pID">节点ID（用于ZTree）</param>
        /// <param name="parentID">父节点ID（用于ZTree）</param>
        /// <returns>Json描述</returns>
        private string GetTreeStruct_Helper_Convert2Json(ResponseNodeConfig nodeConfig, string pID,string parentID,bool MarkFlag_Repeat = false)
        {
            //格式：{ id: 1, pId: 0, name: "父节点 1", open: true ,Editable:true, Deleteable:true },

            string currentNodeId = nodeConfig.NodeID;
            string summary = nodeConfig.GetSummary();

            string json = BuildTreeNodeJson(pID,
                                            parentID,
                                            MarkFlag_Repeat ? (summary + " (鏈路迴圈的節點，停止展開)") : (summary),
                                            currentNodeId,
                                            true,
                                            true,
                                            currentNodeId == ConstString.ROOT_NODE_ID ? false : true,   //根节点不允许删除
                                            MarkFlag_Repeat ? "#ECFFE8" : "#0"
                                            
                                            );
            return json;
        }

        /// <summary>
        /// 建立树节点
        /// </summary>
        /// <param name="Id">节点ID（用于ZTree）</param>
        /// <param name="parentId">父节点ID（用于ZTree）</param>
        /// <param name="display">呈现显示的文字</param>
        /// <param name="expaned">是否展开</param>
        /// <param name="editable">能否编辑</param>
        /// <param name="deleteable">能否删除</param>
        /// <returns>结果Json</returns>
        private static string BuildTreeNodeJson(string Id, string parentId,
                                                string display,string NodeID,
                                                bool expaned, bool editable, bool deleteable,string background = null)
        {
            string json = String.Format("{{ id: '{0}', pId: '{1}', name: '節點ID:{7}　　　{2}', open: {3}, Editable:{4}, Deleteable:{5} , background:'{6}', NodeID:'{7}' }},",
                                        Id,
                                        parentId,
                                        display,
                                        expaned.ToString().ToLower(),
                                        editable.ToString().ToLower(),
                                        deleteable.ToString().ToLower(),
                                        String.IsNullOrEmpty(background) ? "#0" : background,
                                        NodeID);
            return json;
        }

        /// <summary>
        /// 生成树的枝干
        /// </summary>
        /// <param name="nodeConfig">节点配置</param>
        /// <param name="pID">节点ID（用于ZTree）</param>
        /// <param name="parentID">父节点ID（用于ZTree）</param>
        /// <param name="parentID">记录单条链路出现过的ID<para/>(当FindSubNodes标记为False时，此集合不关注，此情况下可以传NULL)</param>
        /// <param name="FindSubNodes">行为：是否展开处理子节点</param>
        /// <param name="ShowNextNodeThenStopFindSubNodes">最后呈现多一次子节点，后续不再呈现(目前Obsolete此属性，请勿使用)</param>
        /// <returns></returns>
        private string GetTreeStruct_Helper_GenerateBranch(ResponseNodeConfig nodeConfig, 
                                                           string pID,string parentID, 
                                                           HashSet<string> usedIDs,
                                                           bool FindSubNodes = true, bool ShowNextNodeThenStopFindSubNodes = false)
        {
            //创建当前节点的信息
            string json = GetTreeStruct_Helper_Convert2Json(nodeConfig, pID, parentID);

            //当前节点ID
            string currentNodeId = nodeConfig.NodeID;
            usedIDs.Add(currentNodeId);

            //记录：该节点属于"有父亲关联"，非孤儿状态(断链状态)
            UsefulNodes.Add(currentNodeId);            

            //处理子节点们
            if (FindSubNodes == true &&
                nodeConfig.DealingHandlerConfig is TextMenuHandlerConfig)
            {
                StringBuilder jsonSub = new StringBuilder();
                int pID_Index = 0;

                TextMenuHandlerConfig menus = (TextMenuHandlerConfig)nodeConfig.DealingHandlerConfig;
                foreach (var menu in menus.Menus)
                {
                    //获得目标子节点的Config
                    string subID = menu.Value.ToString();
                    ResponseNodeConfig targetSubConfig = GetTreeStruct_Helper_GetConfigByNodeId(subID);

                    if (targetSubConfig == null)
                        continue;

                    #region 树形结构的过滤

                    //## subID指定"根节点"，只显示节点信息，不再呈现子节点们
                    if (subID == ConstString.ROOT_NODE_ID)
                    {
                        //jsonSub.AppendLine(GetTreeStruct_Helper_GenerateBranch(targetSubConfig, currentNodeId, null, false));       //3:false，不再展开呈现子节点
                        jsonSub.AppendLine(GetTreeStruct_Helper_Convert2Json(targetSubConfig, pID + "." + pID_Index++, pID, true));
                        continue;
                    }

                    #region 准备废弃
                    /* 考虑之后，还是不再呈现子节点的效果比较好，下面方案先注释。 */ 
                    /*

                    //## 最后呈现多一次子节点，后续不再呈现
                    if (ShowNextNodeThenStopFindSubNodes)
                    {
                        jsonSub.AppendLine(GetTreeStruct_Helper_GenerateBranch(targetSubConfig, currentNodeId, false));       //3:false，不再展开呈现子节点
                        continue;
                    }
                    //## 子节点如果是TextMenuHandler,当开始重复时，标记为“最后呈现多一次子节点，后续不再呈现”，然后递归处理      //3:true，呈现子节点;  4:true，如果有孙节点不再展开
                    {
                        jsonSub.AppendLine(GetTreeStruct_Helper_GenerateBranch(targetSubConfig, currentNodeId,true,true));
                        continue;
                    }

                     */ 
                    #endregion

                    //## 子节点如果是TextMenuHandler，当开始重复时，只呈现信息但是不再展开孙节点
                    if (usedIDs.Contains(subID))
                    {
                        //jsonSub.AppendLine(GetTreeStruct_Helper_GenerateBranch(targetSubConfig, currentNodeId, null, false));       //3:false，不再展开呈现子节点
                        jsonSub.AppendLine(GetTreeStruct_Helper_Convert2Json(targetSubConfig, pID + "." + pID_Index++, pID, true));
                        UsefulNodes.Add(subID); 
                        continue;
                    }

                    //## 其他情景，正常呈现节点信息 + 递归子节点
                    HashSet<string> currentLink_usedIDs = new HashSet<string>(usedIDs);
                    currentLink_usedIDs.Add(currentNodeId);
                    jsonSub.AppendLine(GetTreeStruct_Helper_GenerateBranch(targetSubConfig, pID + "." + pID_Index++, pID, currentLink_usedIDs));

                    #endregion
                }

                json += jsonSub.ToString();
            }

            return json;
        }


        #endregion

        #region 自定义处理器参数配置的支持
        /// <summary>
        /// 自定义处理器参数配置的支持
        /// </summary>
        /// <param name="customHandlerTypeName">自定义处理器的类型</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetConfigClassInfoOfCustomHandler(string customHandlerTypeName)
        {
            //获取目标类型
            Type targetType = CustomHandlerConfig.GetICustomHandlerTypeFromCurrentDomain(customHandlerTypeName);
            if (targetType == null)
            {
                return Json(new { IsConfigable = false }  );
            }

            //检查是否可配置
            bool isConfigable = typeof(ICustomHandlerConfigable).IsAssignableFrom(targetType);
            if (isConfigable == false)
            {
                return Json(new { IsConfigable = false });
            }

            //获取配置用的URL
            string url = CustomHandlerAdvantanceConfigUrls.GetConfigUrl(targetType);
            if (String.IsNullOrEmpty(url))
                url = "about:blank";                //BlankPage
            else
                url = Url.Action("Index", url);     //ActionName,ControllName

            //返回结果
            return Json(new { IsConfigable = true, Url = url });
        }

        #endregion
    }


    /// <summary>
    /// 自定义处理器 的参数配置URL
    /// </summary>
    internal class ResponseChainConfig_CustomHandlerAdvantanceConfig_Urls
    {
        Dictionary<Type, string> Urls = new Dictionary<Type, string>();

        /// <summary>
        /// 构造函数
        /// </summary>
        public ResponseChainConfig_CustomHandlerAdvantanceConfig_Urls()
        {
            /* 新的对应关系，请在下面进行补充   Key=Type,Value=ControllerName  */

            //##通用自定义处理器 URL对应关系：
            AppendRelation(typeof(CustomHandler_ArtificialService), "AdvantanceConfig_CustomHandler_ArtificialService");

            //##该项目才有的自定义处理器  URL对应关系：
            //xx
        }

        /// <summary>
        /// 添加 URL关系
        /// </summary>
        private void AppendRelation(Type targetType,string url)
        {
            try
            {
                Urls.Add(targetType, url);
            }
            catch
            { /* 不理会类型加载失败的情况 */}
        }


        /// <summary>
        /// 获取配置的URL
        /// </summary>
        public string GetConfigUrl(Type targetType)
        {
            if (Urls.ContainsKey(targetType))
                return Urls[targetType];
            else
                return String.Empty;
        }
    }
}
