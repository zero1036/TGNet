using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace EG.WeChat.Utility.WeiXin.ResponseChain.Handlers.CustomHandlers
{
    /// <summary>
    /// 自定义处理器参数配置的Helper
    /// </summary>
    public sealed class ConfigClassOfCustomHandlerHelper
    {

        //---------Setting----------

        #region 配置文件存储路径

        const string ConfigDirectoryPath = @"~/App_Data/ConfigOfCustomHandler/";

        /* ------自定义处理器的参数配置文件，文件名规则------
         * 1.目录固定为上述字符串路径；
         * 2.以类的FullName，作为文件名(命名空间和类名);
         * 3.将文件名中的"."，转为为"_"。
         */ 

        #endregion


        //---------ConstVar---------

        #region 字符串

        const string ROOT_NAME = "ConfigData";

        const string DATA_ITEM = "ConfigDataItem";

        const string NODE_ID = "NodeID";

        #endregion

        
        //---------Control----------

        #region 根据类型获取配置文件的完整路径

        /// <summary>
        /// 根据类型获取配置文件的完整路径
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>结果</returns>
        private static string GetFullPathOfConfigByType(Type type)
        {
            //获取类型的全名，按照规则构建出文件名
            string fileName =  type.FullName.Replace('.', '_') + ".xml";

            //返回结果
            return System.Web.Hosting.HostingEnvironment.MapPath(ConfigDirectoryPath + fileName );
        }

        #endregion

        #region 检查指定的类型，是否继承于IConfigClassOfCustomHandler

        /// <summary>
        /// 检查指定的类型，是否继承于IConfigClassOfCustomHandler
        /// </summary>
        /// <param name="targetType">指定类型</param>
        private static void CheckClassType(Type targetType)
        {
            if (targetType == null)
                throw new ArgumentNullException("targetType","类型为Null。");

            if (typeof(IConfigClassOfCustomHandler).IsAssignableFrom(targetType) == false)
                throw new ArgumentException("targetType", targetType.FullName + "类型未正确继承自IConfigClassOfCustomHandler接口。");
        }

        #endregion

        #region 根据CustomHandler类型，获取其对应的ConfigClass类型

        /// <summary>
        /// 根据CustomHandler类型，获取其对应的ConfigClass类型
        /// </summary>
        /// <param name="CustomHandlerType">自定义处理器的类型</param>
        /// <returns>对应的配置类的类型</returns>
        public static Type GetConfigClassType(Type CustomHandlerType)
        {
            //获取实现的接口类型
            var implementedInterfaces = CustomHandlerType.GetInterfaces();
            foreach( Type interfaceType in implementedInterfaces ) 
            {
                //跳过非泛型的类型
                if ( interfaceType.IsGenericType == false )
                    continue;

                //获取T
                var genericType = interfaceType.GetGenericTypeDefinition();
                if (genericType == typeof( ICustomHandlerConfigable<> ) ) {
                    return interfaceType.GetGenericArguments()[0];
                }
            }

            return null;
        }

        #endregion


        #region 读取配置文件

        /// <summary>
        /// 根据类型，读取配置文件
        /// </summary>
        /// <typeparam name="TConfigClass">类型,要求继承自IConfigClassOfCustomHandler</typeparam>
        /// <returns>XDocument文档</returns>
        public static XDocument LoadConfigByType<TConfigClass>() 
                                           where TConfigClass : IConfigClassOfCustomHandler
        {
            Type targetType = typeof(TConfigClass);
            return LoadConfigByType(targetType);
        }

        /// <summary>
        /// 根据类型，读取配置文件
        /// </summary>
        /// <param name="targetType">类型,要求继承自IConfigClassOfCustomHandler</param>
        /// <returns>XDocument文档</returns>
        public static XDocument LoadConfigByType(Type targetType)
        {
            try
            {
                string fullPath = GetFullPathOfConfigByType(targetType);

                if (System.IO.File.Exists(fullPath))
                    return XDocument.Load(fullPath);
                else
                    return new XDocument(new XElement(ROOT_NAME));
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        #region 保存配置文件

        /// <summary>
        /// 根据类型，保存配置文件
        /// </summary>
        /// <typeparam name="TConfigClass">类型,要求继承自IConfigClassOfCustomHandler</typeparam>
        /// <param name="xdoc">XDocument文档</param>
        /// <returns>成功与否</returns>
        public static bool SaveConfigByType<TConfigClass>(XDocument xdoc) 
                                      where TConfigClass : IConfigClassOfCustomHandler
        {
            Type targetType = typeof(TConfigClass);
            return SaveConfigByType(targetType, xdoc);
        }

        /// <summary>
        /// 根据类型，保存配置文件
        /// </summary>
        /// <param name="targetType">类型,要求继承自IConfigClassOfCustomHandler</param>
        /// <param name="xdoc">XDocument文档</param>
        /// <returns>成功与否</returns>
        public static bool SaveConfigByType(Type targetType, XDocument xdoc)
        {
            try
            {
                string fullPath = GetFullPathOfConfigByType(targetType);

                //检查目录
                string dirPath  = System.IO.Path.GetDirectoryName(fullPath);
                if (System.IO.Directory.Exists(dirPath) == false)
                {
                    System.IO.Directory.CreateDirectory(dirPath);
                }

                //保存文件
                xdoc.Save(fullPath, SaveOptions.DisableFormatting);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        #endregion


        #region 获取ConfigData对象

        /// <summary>
        /// 根据类型和NodeID，获取具体的IConfigClassOfCustomHandler实例
        /// </summary>
        /// <typeparam name="TConfigClass">类型,要求继承自IConfigClassOfCustomHandler</typeparam>
        /// <param name="NodeId">节点ID</param>
        /// <returns>实例</returns>
        public static TConfigClass GetConfigClassInstance<TConfigClass>(string NodeId)
                                                    where TConfigClass : IConfigClassOfCustomHandler
        {
            //获取结果
            Type targetType = typeof(TConfigClass);
            IConfigClassOfCustomHandler result = GetConfigClassInstance(targetType, NodeId);

            //空and非空 的处理
            return result == null ? default(TConfigClass) : (TConfigClass)result;
        }

        /// <summary>
        /// 根据类型和NodeID，获取具体的IConfigClassOfCustomHandler实例
        /// </summary>
        /// <param name="ConfigClassType">类型,要求继承自IConfigClassOfCustomHandler</param>
        /// <param name="NodeId">节点ID</param>
        /// <returns>实例</returns>
        public static IConfigClassOfCustomHandler GetConfigClassInstance(Type ConfigClassType, string NodeId)
        {
            CheckClassType(ConfigClassType);

            //创建容器
            IConfigClassOfCustomHandler DataInstance = Activator.CreateInstance(ConfigClassType) as IConfigClassOfCustomHandler;

            //读取数据
            XDocument xdoc = LoadConfigByType(ConfigClassType);
            if (xdoc == null)
            {
                return DataInstance;
            }

            XElement xeDataItem = xdoc.Root.Elements(DATA_ITEM)
                                           .Where(xe => xe.Attribute(NODE_ID).Value == NodeId)
                                           .FirstOrDefault();

            //反序列化数据      
            if (xeDataItem != null)
                DataInstance.DeserializeConfigFromXmlNode(xeDataItem);

            //返回结果
            return DataInstance;
        }

        #endregion

        #region 移除指定节点ID的配置
        /// <summary>
        /// 移除指定节点ID的配置
        /// </summary>
        /// <param name="xdoc">XDocument文档</param>
        /// <param name="NodeId">节点ID</param>
        /// <returns>成功与否</returns>
        public static bool RemoveConfig(XDocument xdoc,string NodeId)
        {
            try
            {
                XElement xeDataItem = xdoc.Root.Elements(DATA_ITEM)
                                               .Where(xe => xe.Attribute(NODE_ID).Value == NodeId)
                                               .FirstOrDefault();
                xeDataItem.Remove();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region 添加指定节点ID的配置
        /// <summary>
        /// 更新指定节点ID的配置(存在则修改，不存在则更新)
        /// </summary>
        /// <param name="xdoc">XDocument文档</param>
        /// <param name="NodeId">节点ID</param>
        /// <param name="config">IConfigClassOfCustomHandler对象</param>
        /// <returns>成功与否</returns>
        public static bool UpdateConfig(XDocument xdoc, string NodeId, IConfigClassOfCustomHandler config)
        {
            try
            {
                //检查是否存在
                XElement xeTargetItem = xdoc.Root.Elements(DATA_ITEM)
                                               .Where(xe => xe.Attribute(NODE_ID).Value == NodeId)
                                               .FirstOrDefault();
                if (xeTargetItem == null)
                {
                    xeTargetItem = new XElement(DATA_ITEM);                    
                }

                xeTargetItem.SetAttributeValue(NODE_ID, NodeId);        //重新赋值，防止序列化过程，某些实现不规范，将nodeID移除。
                config.SerializeConfigToXmlNode(xeTargetItem);          //序列化

                xdoc.Root.Add(xeTargetItem);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion


        #region 注入ConfigData

        /// <summary>
        /// 注入ConfigData
        /// </summary>
        /// <typeparam name="TConfigClass">类型,要求继承自IConfigClassOfCustomHandler</typeparam>
        /// <param name="Handler">标记为“参数可配置”的自定义处理器</param>
        /// <param name="ConfigData">参数配置实例</param>
        /// <returns>成功与否</returns>
        public static bool ImportConfigData<TConfigClass>(ICustomHandlerConfigable<TConfigClass> Handler,TConfigClass ConfigData)  
                                      where TConfigClass : IConfigClassOfCustomHandler
        {
            return ImportConfigData(Handler, ConfigData);
        }

        /// <summary>
        /// 注入ConfigData
        /// </summary>
        /// <param name="Handler">标记为“参数可配置”的自定义处理器</param>
        /// <param name="ConfigData">参数配置实例</param>
        /// <returns>成功与否</returns>
        internal static bool ImportConfigData(ICustomHandlerConfigable Handler, IConfigClassOfCustomHandler ConfigData)
        {
            dynamic handler     = Handler;
            handler.ConfigData  = ConfigData;

            return true;
        }

        #endregion


    }
}
