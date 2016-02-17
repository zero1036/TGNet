using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace TG.Example
{
    public class RedisSortedSets
    {
        /// <summary>
        /// 根据score从低到高，返回member在有续集中的index
        /// 从高到低：zrevrank
        /// </summary>
        public void ZRank()
        {
            IDatabase redisdb = RedisProvider.redis.GetDatabase();

            long? day = redisdb.SortedSetRank("days", "119");

            Console.WriteLine("dd");
        }
    }
}
