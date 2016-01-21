using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace TG.Example
{
    public class RedisString
    {
        /// <summary>
        /// 批量插入
        /// </summary>
        public void StringSet()
        {
            IDatabase redisdb = RedisProvider.redis.GetDatabase();

            var dic = new Dictionary<RedisKey, RedisValue>();
            dic.Add("Num1", "1");
            dic.Add("Num2", "3");

            redisdb.StringSet(dic.ToArray(), When.Always);
        }
    }
}
