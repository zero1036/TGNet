using EG.Utility.AppCommon;
using EG.WeChat.Utility.WeiXin;
using EG.WeChat.Web.Models;
//using Emperor.UtilityLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace EG.WeChat.Web.Controllers
{
    /// <summary>
    /// 网站配置的Controller
    /// </summary>
    public class WebConfigController : Controller
    {
        //------------Setting-------------

        #region UserKey备注

        /* 目前，固定下来整个Web.config中的“UserKey”(DES算法的Key)，
         * 统一使用当前配置文件中的Key，不暴露到配置界面去处理。
         * 
         * 当页面不暴露配置，而又需要修改上述userKey时，
         * 可以不修改代码，直接使用EG共用的加密工具，直接修改Web.config的UserKey即可。
         * 
         */

        #endregion


        //------------Members-------------

        #region 成员
        
        #endregion


        //------------Control-------------

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public WebConfigController()
        {
        }
        #endregion


        #region 默认页面

        /// <summary>
        /// 默认页面
        /// </summary>
        public ActionResult Index()
        {
            WebConfigVM model = new WebConfigVM();

            #region 遍历进行赋值
            /* 思考对比之后，最终调整为：反射方式去获取数值。
             * 后续只需要维护[WebConfigVM]类即可，不用再硬编码相关的读取和保存语句。
             */

            foreach (var fieldInfo in typeof(WebConfigVM).GetProperties())
            {
                //获得Raw值
                string rawValue = EG.Business.Common.ConfigCache.GetAppConfig(fieldInfo.Name);
                if (rawValue == null)
                {
                    Logger.Log4Net.Warn(String.Format("WebConfigController Load config missing data,Key name is [{0}]", fieldInfo.Name));
                    continue;
                }

                Type targetType = fieldInfo.PropertyType;
                if (targetType == typeof(String))
                {
                    //##字符串，直接赋值
                    fieldInfo.SetValue(model, rawValue);
                }
                else if (targetType.IsEnum)
                {
                    //##枚举值，尝试转换找到合适的值
                    object targetEnum = null;
                    var converter = TypeDescriptor.GetConverter(targetType);
                    if (converter != null)
                    {
                        try
                        {
                            targetEnum = converter.ConvertFromString(rawValue);
                        }
                        catch
                        {
                            targetEnum = null;
                        }
                    }

                    //##最终处理
                    if (targetEnum != null)
                    {
                        fieldInfo.SetValue(model, targetEnum);
                    }
                    else
                    {
                        Logger.Log4Net.Warn(String.Format("WebConfigController Load config missing data,Key name is [{0}],Type is [{1}] and value is [{2}]", 
                                                          fieldInfo.Name,
                                                          targetType,
                                                          rawValue));
                        continue;
                    }

                }
            }

            #endregion

            return View(model);
        }
        
        #endregion

        #region 保存配置

        /// <summary>
        /// 保存配置
        /// </summary>
        /// <param name="model">数据</param>
        public ActionResult SaveConfig(WebConfigVM model)
        {   
            try
            {
                //获取userKey
                string userKey          = EG.Business.Common.ConfigCache.GetAppConfig("UserKey");

                //获取配置文件
                string configPath       = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
                XDocument xdoc          = XDocument.Load(configPath);
                XElement xeAppsetting   = xdoc.Root.Element("appSettings");

                foreach (var fieldInfo in typeof(WebConfigVM).GetProperties())
                {
                    //获取是否需要加密
                    bool IsNeedEncrypt = false;
                    {
                        EncryptFieldAttribute ef = Attribute.GetCustomAttribute(fieldInfo, typeof(EncryptFieldAttribute)) as EncryptFieldAttribute;
                        if (ef != null)
                            IsNeedEncrypt = ef.NeedEncrypt;
                    }

                    //获取数值
                    string value = "";
                    var rawValue = fieldInfo.GetValue(model);
                    if (rawValue is string)
                    {
                        value = rawValue as string;
                    }
                    else if (rawValue is Enum)
                    {
                        value = rawValue.GetHashCode().ToString();
                    }

                    //密码类型，如果空白则保持不变
                    {
                        DataTypeAttribute da = Attribute.GetCustomAttribute(fieldInfo, typeof(DataTypeAttribute)) as DataTypeAttribute;
                        if (da != null && 
                            da.DataType == DataType.Password &&
                            String.IsNullOrEmpty(value))
                            value = EG.Business.Common.ConfigCache.GetAppConfig(fieldInfo.Name);
                    }

                    //写入
                    WriteValue(xeAppsetting, fieldInfo.Name, value, IsNeedEncrypt ? userKey : null);
                }

                //最终一次性写入
                xdoc.Save(configPath);

                //返回结果
                return Json(new { IsSuccess = true } );
            }
            catch (Exception ex)
            {
                //返回结果
                return Json(new { IsSuccess = false, Message = ex.Message });
            }            
            
        }

        /// <summary>
        /// 写入到配置文件
        /// </summary>
        /// <param name="key">配置项</param>
        /// <param name="value">配置的值</param>
        /// <param name="userKey">需要加密时，传递userKey；不需要加密时，传递空即可。</param>
        private void WriteValue(XElement xeAppsetting, string key, string value, string userKey = null)
        {
            //需要加密时的处理
            if (userKey != null)
            {
                value = new Security().Encrypt(value, userKey);
            }

            //获取目标元素
            XElement targetElement = xeAppsetting.Elements("add")                                   //##查找 add元素
                                                 .Where(el => el.Attribute("key").Value == key)     //##匹配 key属性=指定字符串
                                                 .FirstOrDefault();
            if (targetElement == null)
            {
                xeAppsetting.Add(targetElement = new XElement("add"));
                targetElement.SetAttributeValue("key",key);
            }

            //赋值
            targetElement.SetAttributeValue("value",value);
        }

        #endregion

    }
}
