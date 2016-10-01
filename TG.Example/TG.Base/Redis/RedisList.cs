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
            redisdb = RedisProvider.redis.GetDatabase(5);
        }

        public void ListTest()
        {
            string ab = this.redisdb.ListLeftPop("abc");

            long te = this.redisdb.ListRightPush("cbd", "0");

            Debug.WriteLine(te.ToString());
        }


        public void ListCapacityTest()
        {
            //初始 337m
            //增加50w数据后：362m
            for (var i = 1; i <= 5; i++)
            {
                var list = Enumerable.Range(1, 100000);

                var res = list.Select(x => (RedisValue)x.ToString()).ToArray();
                this.redisdb.ListRightPush("lkey", res);
            }


            //备注：抽奖系统加50w条抢票库存，耗时：
            //初始：356m
            //推完库存：456m
        }

    }
}


