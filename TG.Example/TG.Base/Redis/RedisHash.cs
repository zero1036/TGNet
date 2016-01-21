using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
using Newtonsoft.Json;

namespace TG.Example
{
    public class RedisHash
    {
        public void hset()
        {
            IDatabase redisdb = RedisProvider.redis.GetDatabase();
            //var he = new HashEntry("cus:148", "{amount:99}");
            //redisdb.HashSet("bill", he, CommandFlags.None);
            redisdb.HashSet("bill", "cus:148", "{amount:99}", When.Always, CommandFlags.None);
        }

        public void hget()
        {
            IDatabase redisdb = RedisProvider.redis.GetDatabase();
            //var he = new HashEntry("cus:148", "{amount:99}");
            //redisdb.HashSet("bill", he, CommandFlags.None);
            var vall = redisdb.HashGet("bill", "cus:148", CommandFlags.None);
            Console.WriteLine(vall);
        }

        public void hmset()
        {
            IDatabase redisdb = RedisProvider.redis.GetDatabase();

            redisdb.HashSet("bill", new HashEntry[] { 
                new HashEntry("hmset1","tg1"),
                new HashEntry("hmset2","tg2")
            });
        }
    }
}
