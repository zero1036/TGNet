using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace TG.Example
{
    public class MongoBaseEntity
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public ObjectId Id { get; set; }
    }

    public class LuckyMoney : MongoBaseEntity
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 所属package名称
        /// </summary>
        public string PackageName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 创建用户CustomerId
        /// </summary>
        public int CreatorId { get; set; }

        /// <summary>
        /// 创建用户头像昵称
        /// </summary>
        public string CreatorNickName { get; set; }

        /// <summary>
        /// 创建用户头像
        /// </summary>
        public string HeadUrl { get; set; }
    }
}
