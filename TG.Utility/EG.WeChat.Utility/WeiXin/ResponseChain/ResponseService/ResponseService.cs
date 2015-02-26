using EG.WeChat.Utility.WeiXin.ResponseChain.Handlers.CustomHandlers;
using Senparc.Weixin.MP.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace EG.WeChat.Utility.WeiXin.ResponseChain
{

    /* ------------ 思路备忘 ------------:
     * 响应服务(ResponseService)，目前提供两种类型的服务类，满足不同的应用场景。
     * 
     * 1.默认，当需要使用[ResponseChain]模块进行开发时，请使用“ResponseService_MemorySupport”；
     *   直接当做SDK，进行相关的调用。
     *   主要是实现其CreateServicFunction方法，输入“节点ID”，输出代码层面的“NodeInstanceService”对象。
     * 
     * 2.当希望使用“配置文件”作为介质，实现应答功能的时候，请使用“ResponseService_ConfigurationSupport”；
     *   2.1通过将预期的数据，填充到“ResponseConfiguration”，然后调用保存方法。到此，配置流程完成。
     *   2.2通过建立“ResponseService_ConfigurationSupport”对象，即可自动读取配置，并生成相应的服务对象。
     *   
     * 
     * ------------ 创建服务 ------------:
     * 通过“ResponseService_MemorySupport”或“ResponseService_ConfigurationSupport”，
     * 调用其静态方法CreateOrGetService，即可创建对应的一种服务对象。
     * 
     * 
     * ------------   参考---------------:
     * 两种类型的使用，可以参考“ResponseConfig\_ReadMe.txt文件”。
     */


    #region ResponseService抽象类

    /// <summary>
    /// 答应链服务类 抽象类
    /// </summary>
    public abstract class ResponseService
    {
        //---------Members---------

        #region 当前状态

        /// <summary>
        /// 当前节点
        /// </summary>
        NodeInstanceService CurrentNode;

        /// <summary>
        /// 标记是否为 首次加载 状态
        /// </summary>
        bool FirstLoaded = false;

        #endregion


        //---------Control---------

        #region 处理数据

        /// <summary>
        /// 处理数据
        /// </summary>
        /// <param name="openId">OpenID</param>
        /// <param name="inputType">输入的类型</param>
        /// <param name="rawData">Raw格式的数据</param>
        /// <returns>响应结果</returns>
        public IResponseMessage HanderData(string openId,DataTypes inputType, object rawData)
        {
            if (FirstLoaded == false)
            {
                //初始化
                FirstLoaded = true;
                this.Loaded();      

                //##创建初始节点
                CurrentNode =  this.CreateServicInstance(ConstString.ROOT_NODE_ID);
            }

            //##任意时刻都采纳的指令
            if (rawData !=null && rawData.ToString() == "0")
                CurrentNode = this.CreateServicInstance(ConstString.ROOT_NODE_ID);

            //处理数据，如果响应结果为“跳转节点”，则继续进行处理；否则，返回结果到外部。
            IResponseMessage ret = ProcessData(openId, inputType, rawData);
            if (ret is ResponseJumpNode)
            {
                ResponseJumpNode jumpResult = ret as ResponseJumpNode;

                if (NodeIdValidator.IsValid(jumpResult.NodeId) == false)
                    throw new ArgumentException("节点ID格式不正确", "nodeId");
                CurrentNode = this.CreateServicInstance(jumpResult.NodeId);

                //如果无法创建
                if (CurrentNode == null)
                    return null;

                //处理附加的行为

                #region 流程特性 DealingHandlerAdditionalBehaviorAttribute

                if (CurrentNode.NodeStatus == NodeStatusTypes.Created &&
                    CurrentNode.DealingHandler != null)
                {
                    DealingHandlerAdditionalBehaviorAttribute attribute = Attribute.GetCustomAttribute(CurrentNode.DealingHandler.GetType(),
                                                                          typeof(DealingHandlerAdditionalBehaviorAttribute))
                                                                          as DealingHandlerAdditionalBehaviorAttribute;
                    if (attribute != null)
                    {
                        //忽略 呈现ReadyMessage流程
                        if (attribute.AdditionalBehavior.HasFlag(DealingHandlerAdditionalBehaviorType.IgnoreReadyMessage))
                            CurrentNode.NodeStatus = NodeStatusTypes.Dealing;
                    }
                }

                #endregion

                return ProcessData(openId, inputType, rawData);
            }
            else
            {
                return ret;
            }
        }

        /// <summary>
        /// 处理数据(内部)
        /// </summary>
        /// <param name="openId">OpenID</param>
        /// <param name="inputType">输入的类型</param>
        /// <param name="rawData">Raw格式的数据</param>
        /// <returns>响应结果</returns>
        private IResponseMessage ProcessData(string openId, DataTypes inputType, object rawData)
        {

            #region 输入的数据格式

            {
                DataTypes allowTypes = DataTypes.InputAll;      //默认为接收全部类型
                Type handlerType = null;

                //获取目标处理器的类型
                switch (CurrentNode.NodeStatus)
                {
                    case NodeStatusTypes.Dealing:
                        if (CurrentNode.DealingHandler != null)
                            handlerType = CurrentNode.DealingHandler.GetType();
                        break;

                    case NodeStatusTypes.Doned:
                        if (CurrentNode.DonedHandler != null)
                            handlerType = CurrentNode.DealingHandler.GetType();
                        break;
                }

                //根据类型，获得 InputDataTypeLimit 特性
                if (handlerType != null)
                {
                    InputDataTypeLimitAttribute attribute = Attribute.GetCustomAttribute(handlerType, typeof(InputDataTypeLimitAttribute)) as InputDataTypeLimitAttribute;
                    if (attribute != null)  //有标记具体的特性，则使用具体的类型；否则默认为全部格式。
                        allowTypes = attribute.LimitedDataTypes;
                }

                //执行此次输入数据的类型检查
                if (allowTypes.HasFlag(inputType) == false)     //允许列表不包含“此次类型”
                {
                    return new ResponseTextMessage(String.Format(@"您輸入的內容，其類型不符合要求，請重新輸入。
預期的類型有：{0}
此次的類型是：{1}
", allowTypes.GetDescription(),
                                                   inputType.GetDescription()
                                                   ));
                }
            }

            #endregion


            switch (CurrentNode.NodeStatus)
            {
                case NodeStatusTypes.Created:
                    {//##刚创建成功，切换状态并显示“处理的提示消息”
                        CurrentNode.MarkSuccess_ThisNodeStatus();
                        return CurrentNode.DealingHandler.ReadyMessage;
                    }

                case NodeStatusTypes.Dealing:
                    {//##进行“处理中”状态
                        if (CurrentNode.DealingHandler == null)
                        {
                            return null;
                        }

                        //##检查类型，允许指定多种格式
                        if (DataTypeValidator.IsValid(CurrentNode.DealingHandler.GetType(),inputType) == false)
                        {
                            return DealFailResponseResult(CurrentNode.DealingHandler, "輸入的資料類型不正確，請重新輸入。");
                        }

                        //##处理
                        HandlerResult ret = CurrentNode.DealingHandler.HandlerData(openId, inputType, rawData);
                        if (ret == HandlerResult.Success)
                        {
                            CurrentNode.MarkSuccess_ThisNodeStatus();
                            return DealSuccessResponseResult(CurrentNode.DealingHandler,CurrentNode.DonedHandler);
                        }
                        else
                        {
                            return DealFailResponseResult(CurrentNode.DealingHandler);
                        }
                    }

                case NodeStatusTypes.Doned:
                    {//##处理完成，等待用户选择其他处理。
                        if (CurrentNode.DonedHandler == null)
                        {
                            //返回主菜单
                            return new ResponseJumpNode(ConstString.ROOT_NODE_ID);
                        }

                        //##检查类型
                        if (DataTypeValidator.IsValid(CurrentNode.DonedHandler.GetType(), inputType) == false)
                        {
                            return DealFailResponseResult(CurrentNode.DonedHandler, "輸入的資料類型不正確，請重新輸入。");
                        }

                        //##处理
                        HandlerResult ret = CurrentNode.DonedHandler.HandlerData(openId, inputType, rawData);
                        if (ret == HandlerResult.Success)
                        {
                            CurrentNode.MarkSuccess_ThisNodeStatus();
                            return CurrentNode.DonedHandler.SuccessResponseResult;
                        }
                        else
                        {
                            return CurrentNode.DonedHandler.FailResponseResult;
                        }
                    }

                case NodeStatusTypes.Finalized:
                    {
                        throw new InvalidOperationException("CurrentNode.NodeStatus状态异常", new ArgumentOutOfRangeException("状态为Finalized是不合理的应用结果。"));
                    }
            }

            return null;
        }

        /// <summary>
        /// 补充处理成功的响应结果
        /// </summary>
        /// <param name="dealingHandler"></param>
        /// <param name="doneHandler"></param>
        /// <returns></returns>
        private IResponseMessage DealSuccessResponseResult(IHandler dealingHandler,IHandler doneHandler)
        {
            //##如果不存在成功结果
            if (dealingHandler.SuccessResponseResult == null)
                return null;

            //##如果Done状态的Ready消息存在，同时当前结果也是Text，则拼合在一起进行提示。
            if (doneHandler != null &&
                doneHandler.ReadyMessage != null && 
                doneHandler.ReadyMessage is ResponseTextMessage &&
                dealingHandler.SuccessResponseResult is ResponseTextMessage)
            {
                StringBuilder tipText = new StringBuilder();
                tipText.Append(((ResponseTextMessage)dealingHandler.SuccessResponseResult).Context);
                tipText.Append("\r\n\r\n");
                tipText.Append(((ResponseTextMessage)doneHandler.ReadyMessage).Context);

                return new ResponseTextMessage(tipText.ToString());
            }

            //##如果Done状态的Ready不消息存在，同时当前结果是Text，则拼凑默认的“任意内容返回主菜单”。
            if ((doneHandler == null || doneHandler.ReadyMessage == null) &&
                 dealingHandler.SuccessResponseResult is ResponseTextMessage)
            {
                StringBuilder tipText = new StringBuilder();
                tipText.Append(((ResponseTextMessage)dealingHandler.SuccessResponseResult).Context);
                tipText.Append("\r\n\r\n输入任意内容，返回主菜单。");

                return new ResponseTextMessage(tipText.ToString());
            }

            //##其他情况
            return dealingHandler.SuccessResponseResult;
        }

        /// <summary>
        /// 补充处理失败的响应结果
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        private IResponseMessage DealFailResponseResult(IHandler handler, string TipText = "指令無效，請重新輸入。")
        {
            //如果FailResponseResult未指定，则主题消息，默认为“指令無效，請重新輸入。”信息。
            StringBuilder tipText = new StringBuilder();
            if (handler.FailResponseResult != null)
                tipText.Append(handler.FailResponseResult.Context);
            else
                tipText.Append(TipText);

            //如果Ready消息存在，则附加在消息尾部，再次进行提示。
            if (handler.ReadyMessage != null &&
                handler.ReadyMessage is ResponseTextMessage)
            {
                tipText.Append("\r\n\r\n");
                tipText.Append(((ResponseTextMessage)handler.ReadyMessage).Context);
            }

            return new ResponseTextMessage(tipText.ToString());
        }

        /// <summary>
        /// 补充处理ReadyMessage
        /// </summary>
        /// <param name="dealingHandler"></param>
        /// <param name="doneHandler"></param>
        /// <returns></returns>
        private IResponseMessage DealReadyMessage(IHandler dealingHandler, IHandler doneHandler)
        {
            //##如果Done状态的Ready不消息存在，同时当前结果是Text，则拼凑默认的“任意内容返回主菜单”。
            if ((doneHandler == null || doneHandler.ReadyMessage == null) &&
                 dealingHandler.ReadyMessage is ResponseTextMessage)
            {
                StringBuilder tipText = new StringBuilder();
                tipText.Append(((ResponseTextMessage)dealingHandler.ReadyMessage).Context);
                tipText.Append("\r\n\r\n输入任意内容，返回主菜单。");

                return new ResponseTextMessage(tipText.ToString());
            }

            //##默认，直接返回Dealing处理器的Ready信息
            return CurrentNode.DealingHandler.ReadyMessage;
        }

        #endregion

        #region 要求子类进行事先

        /// <summary>
        /// 初始化方法
        /// </summary>
        internal abstract void Loaded();

        /// <summary>
        /// 根据节点ID，获取服务实例
        /// </summary>
        /// <param name="nodeID">节点ID</param>
        internal abstract NodeInstanceService CreateServicInstance(string nodeID);

        #endregion


        #region 支持直接跳转到指定的节点(开发和测试中，未保证稳定，未保证不影响原来逻辑)

        /// <summary>
        /// 支持直接跳转到指定的节点
        /// </summary>
        /// <param name="targetNodeID">指定的节点</param>
        /// <returns></returns>
        public IResponseMessage JumpToTargetnode(string openId, string targetNodeID)
        {
            if (FirstLoaded == false)
            {
                //初始化
                FirstLoaded = true;
                this.Loaded();
            }

            //##创建节点
            NodeInstanceService newNode = this.CreateServicInstance(targetNodeID);
            if (newNode == null)
            {
                return null;
            }
            else
            {
                //替换当前节点
                CurrentNode = newNode;

                //处理附加的行为
                if (CurrentNode.DealingHandler != null)
                {
                    DealingHandlerAdditionalBehaviorAttribute attribute = Attribute.GetCustomAttribute(CurrentNode.DealingHandler.GetType(),
                                                                          typeof(DealingHandlerAdditionalBehaviorAttribute))
                                                                          as DealingHandlerAdditionalBehaviorAttribute;
                    if (attribute != null)
                    {
                        //忽略 呈现ReadyMessage流程
                        if (attribute.AdditionalBehavior.HasFlag(DealingHandlerAdditionalBehaviorType.IgnoreReadyMessage))
                            CurrentNode.NodeStatus = NodeStatusTypes.Dealing;
                    }
                }

                //执行
                IResponseMessage ret = ProcessData(openId, DataTypes.InputAll, null);
                return ret;
            }
        }

        #endregion

        //---------Static----------

        #region 创建实例

        /// <summary>
        /// 静态锁
        /// </summary>
        static object locker = new object();

        /// <summary>
        /// 获取或创建服务实例(线程安全)
        /// </summary>
        /// <param name="messageContext">上下文对象</param>
        /// <returns>服务对象</returns>
        protected static ResponseService CreateOrGetService<T_ResponseService>(CustomMessageContext messageContext)
                                                      where T_ResponseService : ResponseService
        {
            const string DataName =  "ResponseChainInstance";
            ResponseService result;

            /* 需要处理： messageContext.ContextDatas，外部的读写是无限制的。。。  */
            lock (locker)
            {   
                if (messageContext.ContextDatas[DataName] != null &&
                    messageContext.ContextDatas[DataName] is ResponseService
                    )
                {
                    result = messageContext.ContextDatas[DataName] as ResponseService;
                }
                else
                {
                    //创建
                    messageContext.ContextDatas[DataName] = result = Activator.CreateInstance<T_ResponseService>();
                }
            }

            return result;
        }

        #endregion
      
    }

    #endregion


    #region SDK方式的ResponseService

    /// <summary>
    /// 答应链服务类--内存型<para/>
    /// (SDK方式，直接使用内存进行处理)
    /// </summary>
    public class ResponseService_MemorySupport : ResponseService
    {
        /// <summary>
        /// 创建服务的外部方法
        /// </summary>
        Func<string, NodeInstanceService> CreateServicFunction;

        /// <summary>
        /// 初始化方法
        /// </summary>
        internal override void Loaded()
        {
        }

        /// <summary>
        /// 根据节点ID，获取服务实例
        /// </summary>
        /// <param name="nodeID">节点ID</param>
        internal override NodeInstanceService CreateServicInstance(string nodeID)
        {
            return CreateServicFunction(nodeID);
        }

        /// <summary>
        /// 创建服务
        /// </summary>
        /// <param name="messageContext">上下文对象</param>
        /// <param name="createServicFunction">外部方法</param>
        public static ResponseService_MemorySupport CreateOrGetService(CustomMessageContext messageContext, Func<string, NodeInstanceService> createServicFunction)
        {
            //参数检查
            if (createServicFunction == null)
                throw new ArgumentNullException("createServicFunction");

            //创建实例
            ResponseService_MemorySupport result    = ResponseService.CreateOrGetService<ResponseService_MemorySupport>(messageContext) as ResponseService_MemorySupport;
            result.CreateServicFunction             = createServicFunction;
            return result;
        }
    }

    #endregion

    #region 配置文件方式的ResponseService

    /// <summary>
    /// 答应链服务类--配置文件型<para/>
    /// (根据配置文件进行处理)
    /// </summary>
    public class ResponseService_ConfigurationSupport : ResponseService
    {
        /// <summary>
        /// 配置树
        /// </summary>
        ResponseConfigTree_ConfigurationSupport ConfigTree;

        /// <summary>
        /// 初始化方法
        /// </summary>
        internal override void Loaded()
        {
            this.ConfigTree = new ResponseConfigTree_ConfigurationSupport();
        }

        /// <summary>
        /// 根据节点ID，获取服务实例
        /// </summary>
        /// <param name="nodeID">节点ID</param>
        internal override NodeInstanceService CreateServicInstance(string nodeID)
        {
            //查找 节点ID对应 的配置信息
            ResponseNodeConfig configNodeInfo = ConfigTree.GetConfigByNodeID(nodeID);
            if (configNodeInfo == null)
                return null;

            //找不到配置节点时的处理
            if (configNodeInfo == null)
            {
                NodeInstanceService node_unknown    = new NodeInstanceService(ConstString.UNKNOWN_NODE_ID);
                node_unknown.DealingHandler         = DefaultDealingHandler.CreateInstance_Text("抱歉，當前指令現在無法為您提供服務。");
                return node_unknown;
            }

            //根据配置文件处理
            NodeInstanceService result = new NodeInstanceService(configNodeInfo.NodeID);

            if (configNodeInfo.DealingHandlerConfig != null)
            {
                dynamic dealingHandlerConfig    = configNodeInfo.DealingHandlerConfig;
                result.DealingHandler           = dealingHandlerConfig.CreateInstanceFromConfig();

                #region 如果是自定义处理器，再注入参数配置的内容
                try
                {
                    if (result.DealingHandler is ICustomHandler && result.DealingHandler is ICustomHandlerConfigable)
                    {
                        Type configClassType = ConfigClassOfCustomHandlerHelper.GetConfigClassType(result.DealingHandler.GetType());
                        if (configClassType != null)
                        {
                            dynamic configData  = ConfigClassOfCustomHandlerHelper.GetConfigClassInstance(configClassType, configNodeInfo.NodeID);
                            dynamic dh          = result.DealingHandler;
                            dh.ConfigData       = configData;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log4Net.Error("自定义处理器的参数配置，注入失败。", ex);
                }

                #endregion
            }
            if (configNodeInfo.DoneHandlerConfig != null)
            {
                dynamic doneHandlerConfig       = configNodeInfo.DoneHandlerConfig;
                result.DonedHandler             = doneHandlerConfig.CreateInstanceFromConfig();
            }

            return result;
        }

        /// <summary>
        /// 创建服务
        /// </summary>
        /// <param name="messageContext">上下文对象</param>
        public static ResponseService_ConfigurationSupport CreateOrGetService(CustomMessageContext messageContext)
        {
            //创建实例
            ResponseService_ConfigurationSupport result = ResponseService.CreateOrGetService<ResponseService_ConfigurationSupport>(messageContext) as ResponseService_ConfigurationSupport;
            return result;
        }
    }
    
    #endregion

}