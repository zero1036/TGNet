using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace EG.WeChat.Utility.WeiXin.ResponseChain
{

    #region ResponseJumpNode的配置

    /// <summary>
    /// ResponseJumpNode的配置
    /// </summary>
    public class ResponseJumpNodeConfig : IResponseMessageConfig<ResponseJumpNode>
    {

        #region 自身关注的属性

        /// <summary>
        /// 要跳转的节点ID
        /// </summary>
        public string NodeId = ConstString.ROOT_NODE_ID;
        
        #endregion

        #region IResponseMessageConfig接口

        /// <summary>
        /// 从配置信息中创建出具体的IResponseMessage
        /// </summary>
        /// <returns>ResponseJumpNode</returns>
        public ResponseJumpNode CreateInstanceFromConfig()
        {
            return new ResponseJumpNode(this.NodeId);
        }

        /// <summary>
        /// 将配置信息保存到XML
        /// </summary>
        /// <param name="CurrentNode">当前XML节点</param>
        public void SerializeConfigToXmlNode(System.Xml.Linq.XElement CurrentNode)
        {
            CurrentNode.Add(new XElement("NodeId", this.NodeId));
        }

        /// <summary>
        /// 从XLM中还原配置信息
        /// </summary>
        /// <param name="CurrentNode">当前XML节点</param>
        public void DeserializeConfigFromXmlNode(System.Xml.Linq.XElement CurrentNode)
        {
            XElement xeNodeID = CurrentNode.Element("NodeId");

            //参数检查
            if (xeNodeID == null)
                throw new Exception("配置数据在外部被修改。");

            this.NodeId = xeNodeID.Value;
        }

        #endregion

    }

    #endregion

    #region ResponseNewsResult的配置

    /// <summary>
    /// ResponseJumpNode的配置
    /// </summary>
    public class ResponseNewsResultConfig : IResponseMessageConfig<ResponseNewsResult>
    {

        #region 自身关注的属性

        /// <summary>
        /// 文章列表
        /// </summary>
        public ArticleCan[] ArticleList;

        #endregion

        #region IResponseMessageConfig接口

        /// <summary>
        /// 从配置信息中创建出具体的IResponseMessage
        /// </summary>
        /// <returns>ResponseNewsResult</returns>
        public ResponseNewsResult CreateInstanceFromConfig()
        {
            //参数检查
            if (this.ArticleList == null)
                throw new Exception("ArticleList参数未赋值");

            return new ResponseNewsResult(this.ArticleList);
        }
        
        /// <summary>
        /// 将配置信息保存到XML
        /// </summary>
        /// <param name="CurrentNode">当前XML节点</param>
        public void SerializeConfigToXmlNode(System.Xml.Linq.XElement CurrentNode)
        {
            //参数检查
            if (this.ArticleList == null)
                throw new Exception("ArticleList参数未赋值");

            //定义节点
            XElement xeArticles = new XElement("Articles");

            //解析每一个文章列表
            foreach (ArticleCan article in this.ArticleList)
            {
                List<string> ret = new List<string>();
                ret.Add(article.Title);
                ret.Add(article.Description);
                ret.Add(article.PicUrl);
                ret.Add(article.Url);
                xeArticles.Add(new XElement("Article", ConfigHelper.SerializeToString(ret)));
            }

            //成功之后附加到节点
            CurrentNode.Add(xeArticles);
        }

        /// <summary>
        /// 从XLM中还原配置信息
        /// </summary>
        /// <param name="CurrentNode">当前XML节点</param>
        public void DeserializeConfigFromXmlNode(System.Xml.Linq.XElement CurrentNode)
        {
            //参数检查
            XElement xeArticles = CurrentNode.Element("Articles");
            if (xeArticles == null)
                throw new Exception("配置数据在外部被修改。");

            //获取文章列表
            List<ArticleCan> Result = new List<ArticleCan>();
            foreach (XElement xeArticle in xeArticles.Elements("Article"))
            {
                List<string> ret = ConfigHelper.DeserializeFromString<List<string>>(xeArticle.FirstNode.ToString());
                Result.Add(new ArticleCan(ret[0], ret[1], ret[2], ret[3]));
            }

            //赋值
            this.ArticleList = Result.ToArray();
        }

        #endregion

    }

    #endregion

    #region ResponseTextMessage的配置

    /// <summary>
    /// ResponseJumpNode的配置
    /// </summary>
    public class ResponseTextMessageConfig : IResponseMessageConfig<ResponseTextMessage>
    {

        #region 自身关注的属性

        /// <summary>
        /// 字符串内容
        /// </summary>
        public string Context;

        #endregion

        #region IResponseMessageConfig接口

        /// <summary>
        /// 从配置信息中创建出具体的IResponseMessage
        /// </summary>
        /// <returns>ResponseTextMessage</returns>
        public ResponseTextMessage CreateInstanceFromConfig()
        {
            return new ResponseTextMessage(this.Context);
        }

        /// <summary>
        /// 将配置信息保存到XML
        /// </summary>
        /// <param name="CurrentNode">当前XML节点</param>
        public void SerializeConfigToXmlNode(System.Xml.Linq.XElement CurrentNode)
        {
            CurrentNode.Add(new XElement("Context", this.Context));
        }

        /// <summary>
        /// 从XLM中还原配置信息
        /// </summary>
        /// <param name="CurrentNode">当前XML节点</param>
        public void DeserializeConfigFromXmlNode(System.Xml.Linq.XElement CurrentNode)
        {
            XElement xeContent = CurrentNode.Element("Context");

            //参数检查
            if (xeContent == null)
                throw new Exception("配置数据在外部被修改。");

            this.Context = xeContent.Value;
        }

        #endregion

    }

    #endregion

}