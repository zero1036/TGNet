using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TG.Example
{
    /// <summary>
    /// Class LuckyMoneyPackage.
    /// </summary>
    public class LuckyMoneyPackage : MongoBaseEntity
    {
        /// <summary>
        /// Package名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 起始时间
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 有效期类型
        /// </summary>
        /// <value>1：自然日；2：累计时数</value>
        public int ValidityType { get; set; }

        /// <summary>
        /// 有效期
        /// </summary>
        public int Validity { get; set; }

        /// <summary>
        /// 对应红包WHTR.WeiXin.Activity.Core Service版本
        /// </summary>
        public int Service { get; set; }

        /// <summary>
        /// 抽奖类型，1：抽奖系统，2：活动系统
        /// </summary>
        public int LotteryType { get; set; }

        /// <summary>
        /// 抽奖ID--1
        /// </summary>
        public string LotteryId1 { get; set; }

        /// <summary>
        /// 抽奖ID--2
        /// </summary>
        public string LotteryId2 { get; set; }

        /// <summary>
        /// 是否自定义分享
        /// </summary>
        public bool IsCustomShare { get; set; }

        /// <summary>
        /// 奖品配置.
        /// </summary>      
        public string[] PrizeConfig { get; set; }

        /// <summary>
        /// 系统视图
        /// </summary>
        public string View { get; set; }
    }
}
