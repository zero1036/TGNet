using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
using System.Diagnostics;

namespace TG.Example
{
    public class RedisList
    {
        private IDatabase redisdb;

        /// <summary>
        /// 
        /// </summary>
        public RedisList()
        {
            redisdb = RedisProvider.redis.GetDatabase();
        }

        public void ListTest()
        {
            string ab = this.redisdb.ListLeftPop("abc");

            long te = this.redisdb.ListRightPush("cbd", "0");

            Debug.WriteLine(te.ToString());
        }
        
    }
}


