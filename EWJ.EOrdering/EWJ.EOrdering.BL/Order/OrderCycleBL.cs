using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EWJ.EOrdering.DA.Order;
using EWJ.EOrdering.ViewModel.Order;
using System.Data;
using System.Reflection;
using EWJ.EOrdering.Common;

namespace EWJ.EOrdering.BL.Order
{
    public class OrderCycleBL : BaseBL
    {
        private string _Location;
        private string _DicKey = "CycleNo";

        #region public function
        /// <summary>
        /// 
        /// </summary>
        public OrderCycleBL(string pLocation)
        {
            _Location = pLocation;
        }
        /// <summary>
        /// 获取最大周期编号
        /// </summary>
        /// <returns></returns>
        public string GetOrderCycleMaxNo()
        {
            var pDA = new OrderCycleDA();
            return pDA.GetMaxNo();
        }
        /// <summary>
        /// 获取订购周期集合
        /// </summary>
        /// <typeparam name="TM"></typeparam>
        /// <returns></returns>
        public List<TM> GetOrderCycles<TM>()
            where TM : OrderCycleTM
        {
            var pDA = new OrderCycleDA();
            var pDT = pDA.Get(_Location);
            return pDT.ToList<TM>().ToList();
        }
        /// <summary>
        /// 获取周期编号配置
        /// </summary>
        /// <typeparam name="TM"></typeparam>
        /// <returns></returns>
        public CycleNoConfig GetOrderCycleConfig()
        {
            //获取最大周期编号
            var pMaxCycleNo = GetOrderCycleMaxNo();
            //生成下一周期编号配置
            return CreateCycleNo(pMaxCycleNo);
        }
        /// <summary>
        /// 更新订购周期
        /// </summary>
        /// <typeparam name="TM"></typeparam>
        /// <returns></returns>
        public bool UpdateOrderCycle<TM>(TM pCycle)
            where TM : OrderCycleTM
        {
            var pCycles = new List<TM>();
            pCycles.Add(pCycle);
            var dtCycles = CommonFunction.GetDataTableFromEntities<TM>(pCycles);
            //生成用户创建时输入的最大流水号是否与系统当前最大流水号一致
            CycleNoConfig pCurData = CreateCycleNo(pCycle.CycleNo);
            int pMaxNoInSys = NumberHelper.Singleton.GetNo(_DicKey);
            if (pMaxNoInSys != -1 && pCurData.MaxNo != pMaxNoInSys)
                throw new Exception("");
            //定义数据更新后执行action
            Action pAfter = () => { NumberHelper.Singleton.SetNo(_DicKey, GetNextCycleNo); };
            //添加AOP拦截，当有周期维护更新时，同时更新最大流水号（周期编号CycleNo）
            var pDataWriting = new DataWritingInterceptor(() => { }, pAfter);
            var pDA = CastleAOPUtil.NewPxyByClass<OrderCycleDA>(pDataWriting);
            return pDA.Update(dtCycles);
        }
        #endregion

        #region private function
        /// <summary>
        /// 获取订购周期最大编号
        /// </summary>
        /// <param name="pMaxCycleNo"></param>
        /// <returns></returns>
        private CycleNoConfig CreateCycleNo(string pMaxCycleNo)
        {
            int iCycleNo = 0;
            if (string.IsNullOrEmpty(pMaxCycleNo) || !int.TryParse(pMaxCycleNo, out iCycleNo))
                return null;
            //获取周期编号的前4位年份及后n位编号
            var pMaxYear = pMaxCycleNo.Length >= 4 ? pMaxCycleNo.Substring(0, 4) : "";
            var pMaxNo = pMaxCycleNo.Length >= 4 ? pMaxCycleNo.Substring(4, pMaxCycleNo.Length - 4) : "";

            int iMaxYear = 0;
            int iMaxNo = 0;
            if (!int.TryParse(pMaxYear, out iMaxYear) || !int.TryParse(pMaxNo, out iMaxNo))
                return null;

            var pCfg = new CycleNoConfig();
            pCfg.CurYear = DateTime.Now.Year;
            pCfg.CurMonth = DateTime.Now.Month;
            pCfg.CurDay = DateTime.Now.Day;
            pCfg.CurHour = DateTime.Now.Hour;
            pCfg.CurMinute = DateTime.Now.Minute;
            pCfg.MaxYear = iMaxYear;
            pCfg.MaxNo = iMaxNo;
            pCfg.CurNo = (pCfg.CurYear > pCfg.MaxYear) ? 1 : (iMaxNo + 1);
            return pCfg;
        }
        /// <summary>
        /// 获取下一个周期流水号
        /// </summary>
        /// <returns></returns>
        private int GetNextCycleNo()
        {
            //获取数据表中最大周期编号
            var pno = GetOrderCycleMaxNo();
            if (string.IsNullOrEmpty(pno))
                return -1;
            //生成下一个周期编号
            var pConfig = CreateCycleNo(pno);
            return pConfig.CurNo;
        }
        #endregion
    }

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
    }
}
