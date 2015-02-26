using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace EG.WeChat.Utility.WeiXin.ResponseChain
{

    #region DefaultDealingHandler的配置信息

    /// <summary>
    /// DefaultDealingHandler的配置信息
    /// </summary>
    public class DefaultDealingHandlerConfig : IHandlerConfig<DefaultDealingHandler>
    {
        #region 自身关注的属性

        /* DefaultDealingHandler 比较特殊，允许通过 CreateInstance_Text、CreateInstance_SingleNews等方法创建。
         * 因此抽离配置逻辑的时候，需要可以支撑这多种方式。
         */

        /// <summary>
        /// 选用的方式
        /// </summary>
        public DataTypes DataType = DataTypes.Text;

        /// <summary>
        /// Raw格式数据<para />
        /// (根据DataType而不同：string、ArticleCan;等)
        /// </summary>
        public object RawData = null;

        #endregion

        #region IConfig接口

        /// <summary>
        /// 从配置信息中创建出具体的IHandler
        /// </summary>
        /// <returns>DefaultDealingHandler</returns>
        public DefaultDealingHandler CreateInstanceFromConfig()
        {
            //检查参数
            if (RawData == null)
                return null;

            //根据类型进行处理
            DefaultDealingHandler resultHandler;
            switch (this.DataType)
            {
                default:
                    return null;

                case DataTypes.Text:
                    {
                        if (RawData is string == false)
                            throw new Exception("配置数据在外部被修改。RawData非string类型。");

                        resultHandler = DefaultDealingHandler.CreateInstance_Text(this.RawData.ToString());
                        break;
                    }
                case DataTypes.News:
                    {
                        if (RawData is ArticleCan == false)
                            throw new Exception("配置数据在外部被修改。RawData非ArticleCan类型。");

                        resultHandler = DefaultDealingHandler.CreateInstance_SingleNews(this.RawData as ArticleCan);
                        break;
                    }
            }

            //处理状态成员
            IResponseMessage readyMessage, successResult, failResult;
            this.CreateInstanceStatusMember(out readyMessage,out successResult,out failResult);
            resultHandler.ReadyMessage          = readyMessage;
            resultHandler.SuccessResponseResult = successResult;
            resultHandler.FailResponseResult    = failResult as ResponseTextMessage;

            //结果
            return resultHandler;
        }

        /// <summary>
        /// 将配置信息保存到XML
        /// </summary>
        /// <param name="CurrentNode">当前XML节点</param>
        public void SerializeConfigToXmlNode(System.Xml.Linq.XElement CurrentNode)
        {
            //参数检查
            if (this.RawData == null)
                throw new Exception("RawData属性未设置。");

            //根据类型进行处理
            switch (this.DataType)
            {
                default:
                    return;

                case DataTypes.Text:
                {
                    CurrentNode.Add(new XElement("RawData", this.RawData.ToString()));
                    break;
                }
                case DataTypes.News:
                {
                    ArticleCan ac = RawData as ArticleCan;
                    List<string> ret = new List<string>();
                    ret.Add(ac.Title);
                    ret.Add(ac.Description);
                    ret.Add(ac.PicUrl);
                    ret.Add(ac.Url);

                    CurrentNode.Add(new XElement("RawData", ConfigHelper.SerializeToXE(ret)));
                    break;
                }
            }
            CurrentNode.Add(new XElement("DataType", this.DataType));

            //处理状态成员
            XElement xeReady, xeSuccess, xeFail;
            this.SerializeStatusMember(out xeReady, out xeSuccess, out xeFail);
            if (xeReady != null)    CurrentNode.Add(xeReady);
            if (xeSuccess != null)  CurrentNode.Add(xeSuccess);
            if (xeFail != null)     CurrentNode.Add(xeFail);
        }

        /// <summary>
        /// 从XLM中还原配置信息
        /// </summary>
        /// <param name="CurrentNode">当前XML节点</param>
        public void DeserializeConfigFromXmlNode(System.Xml.Linq.XElement CurrentNode)
        {
            XElement xeDataType = CurrentNode.Element("DataType");
            XElement xeRawData = CurrentNode.Element("RawData");

            //参数检查
            if (xeDataType == null || xeRawData == null)
                throw new Exception("配置数据在外部被修改。");

            //##DataType
            this.DataType = (DataTypes)Enum.Parse(typeof(DataTypes), xeDataType.Value);     //看看是否补充 枚举值 检查。

            //##RawData
            switch (this.DataType)
            {
                default:
                    throw new Exception("还原配置失败。");

                case DataTypes.Text:
                    {
                        this.RawData = xeRawData.Value;
                        break;
                    }

                case DataTypes.News:
                    {
                        List<string> lstData = ConfigHelper.DeserializeFromString<List<string>>(xeRawData.FirstNode.ToString());
                        if (lstData == null)
                            throw new Exception("配置数据在外部被修改。");

                        this.RawData = new ArticleCan(lstData.Count > 1 ? lstData[0] : String.Empty,
                                                      lstData.Count > 2 ? lstData[1] : String.Empty,
                                                      lstData.Count > 3 ? lstData[2] : String.Empty,
                                                      lstData.Count > 4 ? lstData[3] : String.Empty);
                        break;
                    }
            }

            //处理状态成员
            this.DeserializeStatusMember(CurrentNode);
        }

        #endregion

        #region 状态成员

        /// <summary>
        /// ReadyMessage的配置
        /// </summary>
        public IResponseMessageConfig ReadyMessageConfig { get; set; }

        /// <summary>
        /// SuccessResult的配置
        /// </summary>
        public IResponseMessageConfig SuccessResultConfig { get; set; }

        /// <summary>
        /// FailResult的配置
        /// </summary>
        public IResponseMessageConfig FailResultConfig { get; set; }

        #endregion
    }

    #endregion

    #region DefaultDoneHandler的配置信息

    /// <summary>
    /// DefaultDoneHandler的配置信息
    /// </summary>
    public class DefaultDoneHandlerConfig : IHandlerConfig<DefaultDoneHandler>
    {
        #region 自身关注的属性

        /// <summary>
        /// 提示信息
        /// </summary>
        public string TipText;

        /// <summary>
        /// 输入#时，跳转的节点
        /// </summary>
        public string NodeId = ConstString.ROOT_NODE_ID;        
        
        #endregion

        #region IConfig接口

        /// <summary>
        /// 从配置信息中创建出具体的IHandler
        /// </summary>
        /// <returns>DefaultDoneHandler</returns>
        public DefaultDoneHandler CreateInstanceFromConfig()
        {
            string targetNodeId = String.IsNullOrEmpty(NodeId)  ? ConstString.ROOT_NODE_ID : NodeId;

            //** 只是出于向下兼容的处理目的 (当TipText为空同时节点ID为根节点  ==等同=>  “输入任意字符跳转到根节点”的Done处理器  )
            if (String.IsNullOrEmpty(TipText) && 
                targetNodeId == ConstString.ROOT_NODE_ID
                )
            {
                return null;        //新版本中，Done为null时，处理模块会转换为“输入任意字符跳转到根节点”的Done处理器
            }

            DefaultDoneHandler resultHandler = new DefaultDoneHandler(TipText, targetNodeId);

            //处理状态成员(不关注failResult)
            IResponseMessage readyMessage, successResult, failResult;
            this.CreateInstanceStatusMember(out readyMessage, out successResult, out failResult);
            resultHandler.ReadyMessage          = readyMessage;
            resultHandler.SuccessResponseResult = successResult;

            //结果
            return resultHandler;
        }

        /// <summary>
        /// 将配置信息保存到XML
        /// </summary>
        /// <param name="CurrentNode">当前XML节点</param>
        public void SerializeConfigToXmlNode(System.Xml.Linq.XElement CurrentNode)
        {
            CurrentNode.Add(new XElement("TipText", this.TipText));
            CurrentNode.Add(new XElement("NodeId",  this.NodeId));

            //处理状态成员
            XElement xeReady, xeSuccess, xeFail;
            this.SerializeStatusMember(out xeReady, out xeSuccess, out xeFail);
            if (xeReady != null)    CurrentNode.Add(xeReady);
            if (xeSuccess != null)  CurrentNode.Add(xeSuccess);
            if (xeFail != null)     CurrentNode.Add(xeFail);
        }

        /// <summary>
        /// 从XLM中还原配置信息
        /// </summary>
        /// <param name="CurrentNode">当前XML节点</param>
        public void DeserializeConfigFromXmlNode(System.Xml.Linq.XElement CurrentNode)
        {
            XElement xeTipText = CurrentNode.Element("TipText");
            XElement xeNodeId  = CurrentNode.Element("NodeId");

            //参数检查
            if (xeTipText == null || xeNodeId == null)
                throw new Exception("配置数据在外部被修改。");

            this.TipText    = xeTipText.Value;
            this.NodeId     = xeNodeId.Value;

            //处理状态成员
            this.DeserializeStatusMember(CurrentNode);
        }

        #endregion

        #region 状态成员

        /// <summary>
        /// ReadyMessage的配置
        /// </summary>
        public IResponseMessageConfig ReadyMessageConfig { get; set; }

        /// <summary>
        /// SuccessResult的配置
        /// </summary>
        public IResponseMessageConfig SuccessResultConfig { get; set; }

        /// <summary>
        /// FailResult的配置
        /// </summary>
        public IResponseMessageConfig FailResultConfig { get; set; }

        #endregion
    }
        
    #endregion

    #region TextMenuHandler的配置信息

    /// <summary>
    /// TextMenuHandler的配置信息
    /// </summary>
    public class TextMenuHandlerConfig : IHandlerConfig<TextMenuHandler>
    {
        #region 自身关注的属性

        /// <summary>
        /// 菜单项 (Key=用户输入的内容; Value=节点ID)
        /// </summary>
        public readonly List<DictionaryEntry> Menus = new List<DictionaryEntry>();

        #endregion

        #region IConfig接口

        /// <summary>
        /// 从配置信息中创建出具体的IHandler
        /// </summary>
        /// <returns>TextMenuHandler</returns>
        public TextMenuHandler CreateInstanceFromConfig()
        {
            //参数检查
            if (this.Menus == null)
                return null;

            //处理菜单项
            List<TextMenuHandler.TextMenuItem> ret = new List<TextMenuHandler.TextMenuItem>();
            foreach (DictionaryEntry menu in Menus)
            {
                ret.Add(TextMenuHandler.TextMenuItem.CreateItem(menu.Value.ToString(), menu.Key.ToString()));
            }

            //创建Handler
            TextMenuHandler resultHandler = TextMenuHandler.CreateInstance(ret.ToArray());

            //处理状态成员
            IResponseMessage readyMessage, successResult, failResult;
            this.CreateInstanceStatusMember(out readyMessage, out successResult, out failResult);
            resultHandler.ReadyMessage          = readyMessage;
            resultHandler.SuccessResponseResult = successResult;
            resultHandler.FailResponseResult    = failResult as ResponseTextMessage;

            //结果
            return resultHandler;
        }

        /// <summary>
        /// 将配置信息保存到XML
        /// </summary>
        /// <param name="CurrentNode">当前XML节点</param>
        public void SerializeConfigToXmlNode(System.Xml.Linq.XElement CurrentNode)
        {
            //参数检查
            if (this.Menus == null)
                return;

            //处理菜单项
            XElement xMenus = new XElement("Menus");
            foreach (DictionaryEntry item in Menus)
            {
                xMenus.Add(new XElement("MenuItem",
                                        new XElement("Text",    item.Key),
                                        new XElement("NodeId",  item.Value)
                           ));
            }

            //成功之后附加到节点
            CurrentNode.Add(xMenus);

            //处理状态成员(只关注xeReady)
            XElement xeReady, xeSuccess, xeFail;
            this.SerializeStatusMember(out xeReady, out xeSuccess, out xeFail);
            if (xeReady != null) CurrentNode.Add(xeReady);
        }

        /// <summary>
        /// 从XLM中还原配置信息
        /// </summary>
        /// <param name="CurrentNode">当前XML节点</param>
        public void DeserializeConfigFromXmlNode(System.Xml.Linq.XElement CurrentNode)
        {
            //清空旧数据
            Menus.Clear();

            XElement xMenus = CurrentNode.Element("Menus");
            var items       = xMenus.Elements("MenuItem");
            if (items == null)
                throw new Exception("配置数据在外部被修改。");

            //解析菜单配置
            List<DictionaryEntry> result = new List<DictionaryEntry>();
            foreach (XElement item in items)
            {
                result.Add(new DictionaryEntry(item.Element("Text").Value,item.Element("NodeId").Value));
            }
            Menus.AddRange(result);

            //处理状态成员(只关注xeReady)
            this.DeserializeStatusMember(CurrentNode);
        }

        #endregion

        #region 状态成员

        /// <summary>
        /// ReadyMessage的配置
        /// </summary>
        public IResponseMessageConfig ReadyMessageConfig { get; set; }

        /// <summary>
        /// SuccessResult的配置
        /// </summary>
        public IResponseMessageConfig SuccessResultConfig { get; set; }

        /// <summary>
        /// FailResult的配置
        /// </summary>
        public IResponseMessageConfig FailResultConfig { get; set; }

        #endregion
    }

    #endregion

    #region TextFullMatchHandler的配置信息

    /// <summary>
    /// TextFullMatchHandler的配置信息
    /// </summary>
    public class TextFullMatchHandlerConfig : IHandlerConfig<TextFullMatchHandler>
    {
        #region 自身关注的属性

        /// <summary>
        /// 要进行匹配的文字
        /// </summary>
        public string MatchText;

        #endregion

        #region IConfig接口

        /// <summary>
        /// 从配置信息中创建出具体的IHandler
        /// </summary>
        /// <returns>TextFullMatchHandler</returns>
        public TextFullMatchHandler CreateInstanceFromConfig()
        {
            TextFullMatchHandler resultHandler = new TextFullMatchHandler(this.MatchText);

            //处理状态成员
            IResponseMessage readyMessage, successResult, failResult;
            this.CreateInstanceStatusMember(out readyMessage, out successResult, out failResult);
            resultHandler.ReadyMessage          = readyMessage;
            resultHandler.SuccessResponseResult = successResult;
            resultHandler.FailResponseResult    = failResult as ResponseTextMessage;

            //结果
            return resultHandler;
        }

        /// <summary>
        /// 将配置信息保存到XML
        /// </summary>
        /// <param name="CurrentNode">当前XML节点</param>
        public void SerializeConfigToXmlNode(System.Xml.Linq.XElement CurrentNode)
        {
            CurrentNode.Add(new XElement("MatchText", this.MatchText));

            //处理状态成员
            XElement xeReady, xeSuccess, xeFail;
            this.SerializeStatusMember(out xeReady, out xeSuccess, out xeFail);
            if (xeReady != null)    CurrentNode.Add(xeReady);
            if (xeSuccess != null)  CurrentNode.Add(xeSuccess);
            if (xeFail != null)     CurrentNode.Add(xeFail);
        }

        /// <summary>
        /// 从XLM中还原配置信息
        /// </summary>
        /// <param name="CurrentNode">当前XML节点</param>
        public void DeserializeConfigFromXmlNode(System.Xml.Linq.XElement CurrentNode)
        {
            XElement xeMatchText = CurrentNode.Element("MatchText");

            //参数检查
            if (xeMatchText == null)
                throw new Exception("配置数据在外部被修改。");

            this.MatchText = xeMatchText.Value;

            //处理状态成员
            this.DeserializeStatusMember(CurrentNode);
        }

        #endregion

        #region 状态成员

        /// <summary>
        /// ReadyMessage的配置
        /// </summary>
        public IResponseMessageConfig ReadyMessageConfig { get; set; }

        /// <summary>
        /// SuccessResult的配置
        /// </summary>
        public IResponseMessageConfig SuccessResultConfig { get; set; }

        /// <summary>
        /// FailResult的配置
        /// </summary>
        public IResponseMessageConfig FailResultConfig { get; set; }

        #endregion
    }

    #endregion

}
