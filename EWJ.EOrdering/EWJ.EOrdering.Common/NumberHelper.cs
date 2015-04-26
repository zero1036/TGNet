using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWJ.EOrdering.Common
{
    /// <summary>
    /// 流水号管理
    /// </summary>
    public class NumberHelper
    {
        /// <summary>
        /// 单例
        /// </summary>
        public static NumberHelper Singleton = new NumberHelper();
        /// <summary>
        /// 构造函数
        /// </summary>
        public NumberHelper()
        {
            DicRollingNumber = new Dictionary<string, int>();
        }
        /// <summary>
        /// 流水号载体
        /// </summary>
        public Dictionary<string, int> DicRollingNumber
        {
            get;
            set;
        }
        /// <summary>
        /// 设置最大号
        /// </summary>
        private void SetNumber(string pDicKey, int iv)
        {
            lock (this.DicRollingNumber)
            {
                if (!this.DicRollingNumber.ContainsKey(pDicKey))
                    this.DicRollingNumber.Add(pDicKey, iv);
                else
                    this.DicRollingNumber[pDicKey] = iv;
            }
        }
        /// <summary>
        /// 设置最大号
        /// </summary>
        /// <param name="pFunc"></param>
        public virtual void SetNo(string pKey, Func<int> pFunc)
        {
            var pNo = pFunc.Invoke();
            SetNumber(pKey, pNo);
        }
        /// <summary>
        /// 获取最大号
        /// </summary>
        public virtual int GetNo(string pKey)
        {
            if (this.DicRollingNumber.ContainsKey(pKey))
                return this.DicRollingNumber[pKey];
            return -1;
        }
    }
}
