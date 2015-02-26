using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace EG.Utility.AppCommon
{
    public class XMLProvider
    {
        public static Dictionary<string, Dictionary<string, string>> XMLDictionary = new Dictionary<string, Dictionary<string, string>>();

        private static readonly string FIRSTNODE = "Nodes";

        /// <summary>
        /// Load the XML data into the cache
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
        /// Load the XML data into the cache 
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
            XmlNode firstNode = xmlRoot.SelectSingleNode(FIRSTNODE);
            XmlElement xeFirstNode = (XmlElement)firstNode;
            string moduleName = xeFirstNode.GetAttribute("id");
            XmlNodeList nodeList = firstNode.ChildNodes;

            Dictionary<string, string> idList = new Dictionary<string, string>();

            foreach (XmlNode xn in nodeList)
            {
                XmlElement xe = (XmlElement)xn;

                idList.Add(xe.GetAttribute("id"), xe.InnerText);
            }
            XMLDictionary.Add(moduleName, idList);
        }

        /// <summary>
        /// Reload
        /// </summary>
        /// <param name="xmls">The XML  as string</param>
        public static void ReloadXml(string[] xmls)
        {
            XMLDictionary.Clear();
            LoadXml(xmls);
        }

        /// <summary>
        /// Reload
        /// </summary>
        /// <param name="xmlName">The XML file name</param>
        public static void Reload(string[] xmlName)
        {
            XMLDictionary.Clear();
            Load(xmlName);
        }
    }
}
