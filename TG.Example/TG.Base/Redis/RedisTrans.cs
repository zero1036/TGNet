using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace TG.Example
{
    public class RedisTrans
    {
        private IDatabase redisdb;

        /// <summary>
        /// 
        /// </summary>
        public RedisTrans()
        {
            redisdb = RedisProvider.redis.GetDatabase();
        }

        /// <summary>
        /// 正常事务，AddCondition带上锁
        /// 结论：完全无len用，根本无watch key
        /// 任意客户端对key作出修改，只要Condition条件满足
        /// Execute一定通过为true
        /// </summary>
        public void ConditionTest()
        {
            var custKey = "moneystore";
            var tran = redisdb.CreateTransaction();
            tran.AddCondition(Condition.KeyNotExists(custKey));
            tran.StringIncrementAsync(custKey, 1);
            //执行Execute前，redis-cli set moneystore 1
            bool committed = tran.Execute();
            System.Diagnostics.Debug.WriteLine(committed.ToString());
        }
    }
}
