using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace EG.Utility.DBCommon.dao
{
    public class SqlProvider
    {
        //protected static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static IDictionary<string, object> XMLDictionary = new Dictionary<string, object>();

        /// <summary>
        /// Initialization, the XML data into the cache --Huskar
        /// </summary>
        /// <param name="xmls">The XML as string</param>
        public static void LoadXml(string[] xmls)
        {
            for (int i = 0; i < xmls.Length; i++)
            {
                XmlDocument xmlRoot = new XmlDocument();
                xmlRoot.LoadXml(xmls[i]);                 
                LoadToDict(xmlRoot);
            }
        }

        /// <summary>
        /// Initialization, the XML data into the cache --Huskar
        /// </summary>
        /// <param name="xmlNames">The XML file name</param>
        public static void Load(string[] xmlNames)
        {
            for (int i = 0; i < xmlNames.Length; i++)
            {
                XmlDocument xmlRoot = new XmlDocument();
                xmlRoot.Load(xmlNames[i]);            
                LoadToDict(xmlRoot);
            }
        }

        private static void LoadToDict(XmlDocument xmlRoot)
        {
            XmlNode firstNode = xmlRoot.SelectSingleNode("sqls");
            XmlElement xeFirstNode = (XmlElement)firstNode;
            string moduleName = xeFirstNode.GetAttribute("module");
            XmlNodeList nodeList = firstNode.ChildNodes;

            IDictionary<string, string> idList = new Dictionary<string, string>();

            foreach (XmlNode xn in nodeList)
            {
                XmlElement xe = (XmlElement)xn;
                // if (xe.Attributes["genre"].Value.Equals("novel"))
                //log.Debug("add " + xe.GetAttribute("id") + " to cache");

                idList.Add(xe.GetAttribute("id"), xe.InnerText);
            }
            XMLDictionary.Add(moduleName, idList);
        }

        /// <summary>
        /// Initialize again  --huskar
        /// </summary>
        /// <param name="xmls">The XML file name</param>
        public static void ReloadXml(string[] xmls)
        {
            XMLDictionary.Clear();
            LoadXml(xmls);
        }

        /// <summary>
        /// Initialize again  --huskar
        /// </summary>
        /// <param name="xmlName">The XML file name</param>
        public static void Reload(string[] xmlName)
        {
            XMLDictionary.Clear();
            Load(xmlName);
        }
        /// <summary>
        /// Read the SQL from the cache  --huskar
        /// </summary>
        /// <param name="module">module's name</param>
        /// <param name="sqlName">the id of sql</param>
        /// <returns>the sql</returns>
        public static string Get(string module, string sqlName)
        {
            IDictionary<string, string> idList = (IDictionary<string, string>)XMLDictionary[module];
            return idList[sqlName];

        }
    }
}
