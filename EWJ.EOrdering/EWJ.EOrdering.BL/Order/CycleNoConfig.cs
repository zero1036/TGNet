using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWJ.EOrdering.BL.Order
{
    public class CycleNoConfig
    {
        /// <summary>
        /// 当前服务器年份
        /// </summary>
        public int CurYear
        { get; set; }
        /// <summary>
        /// 当前服务器月份
        /// </summary>
        public int CurMonth
        { get; set; }
        /// <summary>
        /// 当前服务器日
        /// </summary>
        public int CurDay
        { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int CurHour
        { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int CurMinute
        { get; set; }
        /// <summary>
        /// 当前生成周期编号
        /// </summary>
        public int CurNo
        { get; set; }
        /// <summary>
        /// 当前生成周期编号文本
        /// </summary>
        public string CurCyNo
        {
            get
            {
                if (this.CurNo < 10)
                {
                    return string.Format("{0}0{1}", this.CurYear, this.CurNo);
                }
                else
                {
                    return string.Format("{0}{1}", this.CurYear, this.CurNo);
                }
            }
        }
        /// <summary>
        /// 周期最大年份
        /// </summary>
        public int MaxYear
        { get; set; }
        /// <summary>
        /// 当前订购周期编号No最大号
        /// </summary>
        public int MaxNo
        { get; set; }
    }
}
