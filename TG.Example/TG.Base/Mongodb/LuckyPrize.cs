using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;

namespace TG.Example
{
    public class LuckyPrize: MongoBaseEntity
    {
        /// <summary>
        /// 所属红包Key
        /// </summary>
        public string MoneyKey { get; set; }

        /// <summary>
        /// 中奖用户CustomerId
        /// </summary>
        public int WinnerId { get; set; }

        /// <summary>
        /// 中奖用户OpenId
        /// </summary>
        public string WinnerPhone { get; set; }

        /// <summary>
        /// 抽奖时间
        /// </summary>
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>The content.</value>
        [BsonSerializer(typeof(ExpandoObjectSerializer))]
        public ExpandoObject Content { get; set; }
    }
}
