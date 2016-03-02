using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace TG.Example
{
    public class RedisTest
    {
        /// <summary>
        /// 批量插入
        /// </summary>
        public void StringSet()
        {
            IDatabase redisdb = RedisProvider.redis.GetDatabase();

            var dic = new Dictionary<RedisKey, RedisValue>();

            for (var i = 1; i <= 100000; i++)
            {
                dic.Add("Num" + i, i);
            }

            redisdb.StringSet(dic.ToArray(), When.Always);
        }
    }
}
