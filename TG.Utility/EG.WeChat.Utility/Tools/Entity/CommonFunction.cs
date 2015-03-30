using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using EG.WeChat.Service;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
/*****************************************************
* 目的：CommonFunction
* 创建人：林子聪
* 创建时间：
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Utility.Tools
{
    /// <summary>
    /// 通用方法
    /// </summary>
    public static class CommonFunction
    {
        #region 集合与数据表
        /// <summary>
        /// 将数据表映射到具有同名属性值对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        /// <remarks>林子聪20130618</remarks>
        public static List<T> GetEntitiesFromDataTable<T>(DataTable dt)
        {
            List<T> pEntityCollect = new List<T>();
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                System.Collections.IEnumerator pEnumerator = dt.Rows.GetEnumerator();

                foreach (DataRow pRow in dt.Rows)
                {
                    T pEntity = Activator.CreateInstance<T>(); //使用反射创建泛型的对象实例
                    CommonFunction.ReflectingEntity(pEntity, pRow); //使用反射设置对象属性值
                    pEntityCollect.Add(pEntity);
                }
                dt.Dispose();
            }
            return pEntityCollect;
        }
        /// <summary>    
        /// 转化一个DataTable
        /// 自动创建表结构并填充值
        /// </summary>    
        /// <typeparam name="T"></typeparam>    
        /// <param name="list"></param>    
        /// <returns></returns>    
        public static DataTable GetDataTableFromEntities<T>(IEnumerable<T> list)
        {
            //创建属性的集合    
            List<PropertyInfo> pList = new List<PropertyInfo>();
            //获得反射的入口    

            Type type = typeof(T);
            DataTable dt = new DataTable();
            //把所有的public属性加入到集合 并添加DataTable的列    
            Array.ForEach<PropertyInfo>(type.GetProperties(), p => { pList.Add(p); dt.Columns.Add(p.Name, p.PropertyType); });
            foreach (var item in list)
            {
                //创建一个DataRow实例    
                DataRow row = dt.NewRow();
                //给row 赋值    
                pList.ForEach(p => row[p.Name] = p.GetValue(item, null));
                //加入到DataTable    
                dt.Rows.Add(row);
            }
            return dt;
        }
        /// <summary>
        /// 转化一个DataTable
        /// 以TConstruct创建表结构
        /// 以TValue并填充值
        /// </summary>
        /// <typeparam name="TConstruct">以TConstruct创建表结构</typeparam>
        /// <typeparam name="TValue">以TValue并填充值</typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataTable GetDataTableFromEntities<TConstruct, TValue>(IEnumerable<TValue> list)
        {
            //创建属性的集合    
            List<PropertyInfo> pList = new List<PropertyInfo>();
            //获得反射的入口    

            Type type = typeof(TConstruct);
            DataTable dt = new DataTable();
            //把所有的public属性加入到集合 并添加DataTable的列    
            Array.ForEach<PropertyInfo>(type.GetProperties(), p => { pList.Add(p); dt.Columns.Add(p.Name, p.PropertyType); });

            PropertyInfo[] propertys = typeof(TValue).GetProperties();
            foreach (var item in list)
            {
                //创建一个DataRow实例    
                DataRow row = dt.NewRow();

                ////给row 赋值    
                //pList.ForEach(p => row[p.Name] = p.GetValue(item, null));
                foreach (PropertyInfo info in propertys)
                {
                    if (row.Table.Columns.Contains(info.Name) && info.Name != "headimgurl")
                        row[info.Name] = info.GetValue(item, null);
                }
                //加入到DataTable    
                dt.Rows.Add(row);
            }
            return dt;
        }
        /// <summary>
        /// 将数据行中的字段值设置到对象同名的属性值。
        /// </summary>
        /// <param name="entity">对象实例</param>
        /// <param name="row">数据记录</param>
        /// <remarks>林子聪20130618</remarks>
        public static void ReflectingEntity(object entity, DataRow row)
        {
            //使用反射根据对象获取属性集
            Type typeEntity = entity.GetType();
            PropertyInfo[] propertyInfos = typeEntity.GetProperties();
            //为每个属性设置数据行中的相应值
            foreach (PropertyInfo info in propertyInfos)
            {
                //做两级判断:属性名在DataRow所对应的表中存在;属性值不为空
                if (row.Table.Columns.Contains(info.Name))
                {
                    object propertyValue = row[info.Name];
                    if (propertyValue != DBNull.Value)
                    {
                        //由于Oracle的整型字段都变成Decimal，在.NET下无法通过反射转成Integer
                        Type genericType = info.PropertyType;
                        if (genericType == typeof(int))
                        {
                            propertyValue = System.Convert.ToInt32(propertyValue);
                        }
                        //else if (genericType == typeof(string) && )
                        //{
                        //    propertyValue = System.Convert.ToDouble(propertyValue);
                        //}
                        try
                        {
                            if (genericType == typeof(string) && row.Table.Columns[info.Name].DataType == typeof(decimal))
                            {
                                string strFieldType = row.Table.Columns[info.Name].DataType.ToString().ToLower();
                                info.SetValue(entity, propertyValue.ToString(), null);
                            }
                            else
                            {
                                info.SetValue(entity, propertyValue, null);
                            }
                        }
                        catch (Exception ex)
                        {
                            //System.Windows.Forms.MessageBox.Show(ex.Message);
                            //SouthGIS.SysConfig.Model.LogServices.WriteExceptionLog(ex, "");
                        }
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(string.Format("Table( {0} ) doesn't exist Column( {1} )", row.Table.TableName, info.Name));
                }
            }
        }
        #endregion

        #region 集合与Json
        /// <summary>
        /// 序列化——实体转换Json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pEntity"></param>
        /// <returns></returns>
        /// <remarks>林子聪20130618</remarks>
        public static string ConvertToJson<T>(T pEntity)
        {
            DataContractJsonSerializer ds = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();

            ds.WriteObject(ms, pEntity);

            string strReturn = Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();
            return strReturn;
        }
        /// <summary>
        /// 反序列化——Json转换实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static T FromJsonTo<T>(string jsonString)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
            {
                T jsonObject = (T)ser.ReadObject(ms);
                return jsonObject;
            }
        }
        /// <summary>
        /// json文本集合转换为实体集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="plist"></param>
        /// <returns></returns>
        public static List<T> FromJsonListToEntityList<T>(List<string> plist)
        //where T : new()
        {
            List<T> pListT = new List<T>();
            foreach (string str in plist)
            {
                //T pT = new T();
                T pT = CommonFunction.FromJsonTo<T>(str);
                pListT.Add(pT);
            }
            return pListT;
        }
        /// <summary>
        /// unicode解码
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public static string DecodeUnicode(Match match)
        {
            if (!match.Success)
            {
                return null;
            }

            char outStr = (char)int.Parse(match.Value.Remove(0, 2), System.Globalization.NumberStyles.HexNumber);
            return new string(outStr, 1);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Json_Serialize(object data)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            var jsonString = js.Serialize(data);

            //解码Unicode，也可以通过设置App.Config（Web.Config）设置来做，这里只是暂时弥补一下，用到的地方不多
            MatchEvaluator evaluator = new MatchEvaluator(DecodeUnicode);
            var json = Regex.Replace(jsonString, @"\\u[0123456789abcdef]{4}", evaluator);//或：[\\u007f-\\uffff]，\对应为\u000a，但一般情况下会保持\
            return json;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static object Json_DeserializeObject(string data)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            var objjson = js.DeserializeObject(data);
            return objjson;
        }
        #endregion

        #region 集合
        /// <summary>
        /// 配合页面表格分页，按起始索引与截取长度，截取部分输入集合
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="pList">输入集合</param>
        /// <param name="pPageIndex">页面索引</param>
        /// <param name="pResultLength">截取集合长度</param>
        /// <returns></returns>
        public static List<T> SubListForTable<T>(List<T> pList, int pPageIndex, int pResultLength)
        {
            int iCount = pList.Count;
            int iStartIndex = pResultLength * (pPageIndex - 1);

            List<T> pResultList = new List<T>();
            for (int i = 1; i <= pResultLength; i++)
            {
                if (iStartIndex <= iCount - 1)
                    pResultList.Add(pList[iStartIndex]);
                iStartIndex += 1;
            }
            return pResultList;
        }
        /// <summary>
        /// 截取部分输入集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pList"></param>
        /// <param name="iCount"></param>
        /// <returns></returns>
        public static List<T> SubListForTable<T>(List<T> pList, int iCount)
        {
            if (iCount >= pList.Count)
                return pList;
            List<T> pListOut = new List<T>();
            for (int i = 1; i <= iCount; i++)
            {
                pListOut.Add(pList[i]);
            }
            return pListOut;
        }
        /// <summary>
        /// 构建动态查询Select表达式
        /// </summary>
        public static void SelectEnumerable()
        {
            ////依据IQueryable数据源构造一个查询
            //IQueryable<Customer> custs = db.Customers;
            ////组建一个表达式树来创建一个参数
            //ParameterExpression param =
            //    Expression.Parameter(typeof(Customer), "c");
            ////组建表达式树:c.ContactName
            //Expression selector = Expression.Property(param,
            //    typeof(Customer).GetProperty("ContactName"));
            //Expression pred = Expression.Lambda(selector, param);
            ////组建表达式树:Select(c=>c.ContactName)
            //Expression expr = Expression.Call(typeof(Queryable), "Select", new Type[] { typeof(Customer), typeof(string) }, Expression.Constant(custs), pred);

            ////使用表达式树来生成动态查询
            //IQueryable<string> query = db.Customers.AsQueryable()
            //    .Provider.CreateQuery<string>(expr);
            ////使用GetCommand方法获取SQL语句
            //System.Data.Common.DbCommand cmd = db.GetCommand(query);
            //Console.WriteLine(cmd.CommandText);
        }
        /// <summary>
        /// 构建动态查询Where表达式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pEnumerable"></param>
        /// <param name="paramQue"></param>
        /// <returns></returns>
        /// <remarks>林子聪</remarks>
        public static IQueryable<T> QueryEnumerable<T>(IEnumerable<T> pEnumerable, Queue<QueryEntity> paramQue)
        {
            IQueryable<T> custs = pEnumerable.AsQueryable<T>();
            if (paramQue == null || paramQue.Count == 0)
                return custs;

            //创建一个参数c
            ParameterExpression param = Expression.Parameter(typeof(T), "c");
            //空Expression
            Expression filter = Expression.Empty();

            int i = -1;
            while (i != 0)
            {
                QueryEntity pQueryEntity = paramQue.Dequeue();
                if (pQueryEntity.Value == null || string.IsNullOrEmpty(pQueryEntity.Name))
                {
                    i = paramQue.Count;
                    continue;
                }

                Expression left = Expression.Property(param, typeof(T).GetProperty(pQueryEntity.Name));
                Expression right = Expression.Constant(pQueryEntity.Value);
                Expression filterNow = Expression.Empty();
                //模糊查询——暂未实现
                if (pQueryEntity.IsLike)
                {
                    //filterNow = PredicateExtensions.BuildContainsExpression<T, T>(d => d.Equals("mmXX"), pQueryEntity.Value);
                }
                //等量查询
                else
                {
                    filterNow = Expression.Equal(left, right);
                }

                //除第一个参数以为，其他参数需要加入条件符
                if (i != -1)
                {
                    //And
                    if (pQueryEntity.ConPic)
                        filter = Expression.And(filter, filterNow);
                    //Or
                    else
                        filter = Expression.Or(filter, filterNow);
                }
                else
                {
                    filter = filterNow;
                }
                //赋值队列长度
                i = paramQue.Count;
            }

            Expression pred = Expression.Lambda(filter, param);
            //Where(c=>c.City=="London")
            Expression expr = Expression.Call(typeof(Queryable), "Where", new Type[] { typeof(T) }, Expression.Constant(custs), pred);

            IQueryable<T> query = pEnumerable.AsQueryable().Provider.CreateQuery<T>(expr);
            return query;
        }
        #endregion

        #region 反射与动态创建
        /// <summary>
        /// 动态调用类
        /// </summary>
        public static void DynamicInvokeClass()
        {
            //动态创建的类类型
            Type classType = DynamicCreateType();
            //调用有参数的构造函数
            Type[] ciParamsTypes = new Type[] { typeof(string) };
            object[] ciParamsValues = new object[] { "Hello World" };
            ConstructorInfo ci = classType.GetConstructor(ciParamsTypes);
            object Vector = ci.Invoke(ciParamsValues);
            //调用方法
            object[] methedParams = new object[] { };
            Console.WriteLine(classType.InvokeMember("get_Property", BindingFlags.InvokeMethod, null, Vector, methedParams));
            Console.ReadKey();
        }
        /// <summary>
        /// 动态生成类
        /// </summary>
        /// <returns></returns>
        public static Type DynamicCreateType()
        {
            //动态创建程序集
            AssemblyName DemoName = new AssemblyName("DynamicAssembly");
            AssemblyBuilder dynamicAssembly = AppDomain.CurrentDomain.DefineDynamicAssembly(DemoName, AssemblyBuilderAccess.RunAndSave);
            //动态创建模块
            ModuleBuilder mb = dynamicAssembly.DefineDynamicModule(DemoName.Name, DemoName.Name + ".dll");
            //动态创建类MyClass
            TypeBuilder tb = mb.DefineType("MyClass", TypeAttributes.Public);
            //动态创建字段
            FieldBuilder fb = tb.DefineField("myField", typeof(System.String), FieldAttributes.Private);
            //动态创建构造函数
            Type[] clorType = new Type[] { typeof(System.String) };
            ConstructorBuilder cb1 = tb.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, clorType);
            //生成指令
            ILGenerator ilg = cb1.GetILGenerator();//生成 Microsoft 中间语言 (MSIL) 指令
            ilg.Emit(OpCodes.Ldarg_0);
            ilg.Emit(OpCodes.Call, typeof(object).GetConstructor(Type.EmptyTypes));
            ilg.Emit(OpCodes.Ldarg_0);
            ilg.Emit(OpCodes.Ldarg_1);
            ilg.Emit(OpCodes.Stfld, fb);
            ilg.Emit(OpCodes.Ret);
            //动态创建属性
            PropertyBuilder pb = tb.DefineProperty("MyProperty", System.Reflection.PropertyAttributes.HasDefault, typeof(string), null);
            //动态创建方法
            MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.SpecialName;
            MethodBuilder myMethod = tb.DefineMethod("get_Field", getSetAttr, typeof(string), Type.EmptyTypes);
            //生成指令
            ILGenerator numberGetIL = myMethod.GetILGenerator();
            numberGetIL.Emit(OpCodes.Ldarg_0);
            numberGetIL.Emit(OpCodes.Ldfld, fb);
            numberGetIL.Emit(OpCodes.Ret);
            //使用动态类创建类型
            Type classType = tb.CreateType();
            //保存动态创建的程序集 (程序集将保存在程序目录下调试时就在Debug下)
            dynamicAssembly.Save(DemoName.Name + ".dll");
            //创建类
            return classType;
        }
        #endregion

        #region XML
        //      /// <summary>
        ///// 获取配置，并匹配实体集合
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="strPath"></param>
        ///// <param name="strRootName"></param>
        ///// <param name="strTargetType"></param>
        ///// <returns></returns>
        //public static List<T> MatchConfigList<T>(string strPath, string strRootName, string strTargetType)
        // where T : new()
        //{
        //    XmlDocument xmlDoc = new XmlDocument();
        //    xmlDoc.Load(System.Web.HttpContext.Current.Server.MapPath(strPath));
        //    //获取Employees节点的所有子节点
        //    XmlNodeList nodeList = xmlDoc.SelectSingleNode(strRootName).ChildNodes;
        //    //创建输出集合实例
        //    List<T> pList = new List<T>();
        //    //创建单个配置实例
        //    T pEn;
        //    //使用反射根据对象获取属性集
        //    Type typeEntity = typeof(T);
        //    PropertyInfo[] propertyInfos = typeEntity.GetProperties();
        //    //遍历所有子节点
        //    foreach (XmlNode xn in nodeList)
        //    {
        //        pEn = new T();
        //        //将子节点类型转换为XmlElement类型，并且只获取目标类型值
        //        XmlElement xe = (XmlElement)xn;
        //        if (xe.Name != strTargetType)
        //            continue;
        //        XmlNodeList nls = xe.ChildNodes;//继续获取xe子节点的所有子节点 
        //        if (nls == null || nls.Count < 1)
        //            continue;

        //        foreach (XmlNode xn1 in nls)//遍历 
        //        {
        //            XmlElement xe2 = (XmlElement)xn1;//转换类型 
        //            //为每个属性设置数据行中的相应值
        //            foreach (PropertyInfo info in propertyInfos)
        //            {
        //                if (info.Name != xe2.Name)
        //                    continue;
        //                object propertyValue = xe2.InnerText;
        //                if (propertyValue == DBNull.Value)
        //                    continue;

        //                //由于Oracle的整型字段都变成Decimal，在.NET下无法通过反射转成Integer
        //                Type genericType = info.PropertyType;
        //                if (genericType == typeof(int))
        //                {
        //                    propertyValue = System.Convert.ToInt32(propertyValue);
        //                }
        //                else if (genericType == typeof(double))
        //                {
        //                    propertyValue = System.Convert.ToDouble(propertyValue);
        //                }
        //                try
        //                {
        //                    info.SetValue(pEn, propertyValue, null);
        //                }
        //                catch
        //                { continue; }
        //            }

        //        }
        //        pList.Add(pEn);
        //    }
        //    //xmlDoc.Save(Server.MapPath("data.xml"));//保存。
        //    return pList;
        //}
        /// <summary>
        /// 读取xml配置——固定xml与json搭配配置模板
        /// </summary>
        /// <param name="strPath"></param>
        /// <param name="strRootName"></param>
        /// <param name="strTargetType"></param>
        /// <returns></returns>
        public static List<string> ReadXMLConfig(string strPath, string strRootName, string strTargetType)
        {
            List<string> pList = new List<string>();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(System.Web.HttpContext.Current.Server.MapPath(strPath));
            ////获取Employees节点的所有子节点
            XmlNode pRootNode = xmlDoc.SelectSingleNode(strRootName);
            XmlNodeList pNodeList = pRootNode.ChildNodes;
            foreach (XmlNode xn in pNodeList)
            {
                XmlElement xele = (xn as XmlElement);
                if (xele.Name != strTargetType)
                    continue;
                //添加到集合
                if (!pList.Contains(xele.InnerText))
                    pList.Add(xele.InnerText);
            }
            return pList;
        }
        /// <summary>
        /// 更新xml配置——固定xml与json搭配配置模板
        /// </summary>
        /// <param name="strPath">配置路径</param>
        /// <param name="strRootName">根节点名称</param>
        /// <param name="strTargetType">目标节点名称</param>
        /// <param name="strTargetValue">目标节点值</param>
        /// <param name="strPrimaryKeyValue">目标节点Key属性值</param>
        public static void UpdateXMLConfig(string strPath, string strRootName, string strTargetType, string strTargetValue, string strPrimaryKeyValue)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(System.Web.HttpContext.Current.Server.MapPath(strPath));
            lock (xmlDoc)
            {

                //获取根节点的所有子节点
                XmlNode pRootNode = xmlDoc.SelectSingleNode(strRootName);
                XmlNodeList pNodeList = pRootNode.ChildNodes;
                foreach (XmlNode xn in pNodeList)
                {
                    XmlElement xele = (xn as XmlElement);
                    if (!xele.HasAttribute("key"))
                        continue;
                    string strKeyValue = xele.GetAttribute("key");

                    if (strKeyValue != strPrimaryKeyValue)
                        continue;

                    xele.InnerText = strTargetValue;
                    xmlDoc.Save(System.Web.HttpContext.Current.Server.MapPath(strPath));
                    return;
                }

                XmlElement pNewEle = xmlDoc.CreateElement(strTargetType);
                pNewEle.SetAttribute("key", strPrimaryKeyValue);
                pNewEle.InnerText = strTargetValue;
                pRootNode.AppendChild(pNewEle);
                //保存。
                xmlDoc.Save(System.Web.HttpContext.Current.Server.MapPath(strPath));

            }

        }
        /// <summary>
        /// 获取配置，并匹配实体集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strPath"></param>
        /// <param name="strRootName"></param>
        /// <param name="strTargetType"></param>
        /// <returns></returns>
        public static List<T> MatchConfigList<T>(string strPath, string strRootName, string strTargetType)
         where T : new()
        {
            List<string> plist = CommonFunction.ReadXMLConfig(strPath, strRootName, strTargetType);
            return FromJsonListToEntityList<T>(plist);
        }
        #endregion

        #region 日期时间

        /// <summary>
        /// 获取下x月的1月1号
        /// </summary>
        /// <returns></returns>
        public static string GetNextMonth(int x)
        {
            int iYear = DateTime.Now.Year;
            int iMon = DateTime.Now.Month;
            if ((iMon + x) > 12)
            {
                iMon = (iMon + x) - 12;
                iYear += 1;
            }
            else
            {
                iMon += x;
            }

            string strMon = iMon.ToString();
            if (strMon.Length == 1)
                return string.Format("{0}0{1}01", iYear, strMon);
            else
                return string.Format("{0}{1}01", iYear, strMon);
        }
        /// <summary>
        /// 获取上x月的1月1号
        /// </summary>
        /// <returns></returns>
        public static string GetLastMonth(int x)
        {
            int iYear = DateTime.Now.Year;
            int iMon = DateTime.Now.Month;
            if (iMon <= x)
            {
                iMon = (iMon + 12) - x;
                iYear -= 1;
            }
            else
            {
                iMon -= 1;
            }

            string strMon = iMon.ToString();
            if (strMon.Length == 1)
                return string.Format("{0}0{1}01", iYear, strMon);
            else
                return string.Format("{0}{1}01", iYear, strMon);
        }
        #endregion
    }
}
