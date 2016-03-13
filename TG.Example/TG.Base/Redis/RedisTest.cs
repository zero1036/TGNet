using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace TG.Example
{
    public class RedisTestData
    {
        /// <summary>
        /// 批量插入
        /// </summary>
        public void AddNum()
        {
            IDatabase redisdb = RedisProvider.redis.GetDatabase();

            var dic = new Dictionary<RedisKey, RedisValue>();

            for (var i = 600001; i <= 900000; i++)
            {
                dic.Add("Num" + i, "this.aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
            }

            redisdb.StringSet(dic.ToArray(), When.Always);
        }
    }
}
