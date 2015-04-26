using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWJ.EOrdering.ViewModel.Order
{
    /// <summary>
    /// View Model
    /// </summary>
    public class OrderCycleVM : OrderCycleTM
    {
        /// <summary>
        /// 周期状态
        /// </summary>
        public string StatusX
        {
            get
            {
                var dtNow = DateTime.Now;
                if (base.StartDate != null && base.EndDate != null)
                {
                    if (base.StartDate > dtNow && base.EndDate > dtNow)
                        //未开始
                        return "017001";
                    else if (base.StartDate <= dtNow && base.EndDate >= dtNow)
                        //开始
                        return "017002";
                    else if (base.StartDate < dtNow && base.EndDate < dtNow)
                        //截止
                        return "017003";
                    return base.Status;
                }
                else
                {
                    return base.Status;
                }
            }
        }
        /// <summary>
        /// 起始日期
        /// </summary>
        public string StartDateX
        {
            get
            {
                return base.StartDate.ToString("dd-MM-yyyy");
            }
        }
        /// <summary>
        /// 终止日期
        /// </summary>
        public string EndDateX
        {
            get
            {
                return base.EndDate.ToString("dd-MM-yyyy");
            }
        }
    }
    /// <summary>
    /// Table Model
    /// </summary>
    public class OrderCycleTM
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public Guid CycleID
        { get; set; }
        /// <summary>
        /// 周期编号
        /// </summary>
        public string CycleNo
        { get; set; }
        /// <summary>
        /// 起始时间
        /// </summary>
        public DateTime StartDate
        { get; set; }
        /// <summary>
        /// 终止时间
        /// </summary>
        public DateTime EndDate
        { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        { get; set; }
        /// <summary>
        /// 周期状态
        /// </summary>
        public string Status
        { get; set; }
        /// <summary>
        /// 货品类型
        /// </summary>
        public string ModelType
        { get; set; }
    }
}
