using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
using System.Threading;

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
            redisdb = RedisProvider.redis.GetDatabase(5);
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


        /// <summary>
        /// 报错：
        /// StackExchange.Redis.RedisConnectionException: Unexpected response to EXEC: MultiBulk: 0 items
        /// at StackExchange.Redis.ConnectionMultiplexer.ExecuteSyncImpl[T](Message message, ResultProcessor`1 processor, ServerEndPoint server)
        /// at StackExchange.Redis.RedisTransaction.Execute(CommandFlags flags)
        /// at WHTR.WeiXin.Activity.Core.Service.LuckyMoneyBaseService.AddLotteryResultToRedisData(String prizeKey, String winnerPhone, String lotteryResultStr)
        /// at WHTR.WeiXin.Activity.Core.Service.LuckyMoneyBaseService.Draw(String lotteryId, Int32 ownerCustomerId, String urlKey, Int32 winnerCustomerId, String winnerPhone, String winnerNickName, String winnerHeadUrl)
        /// </summary>
        public void MoneyBug_MultiBulk()
        {
            for (var i = 1; i <= 20; i++)
            {
                Thread th = new Thread(this.MultiBulk);
                th.Start();
            }
            //this.MultiBulk();
        }

        public void MultiBulk()
        {
            var ts = DateTime.Now.AddDays(7).Ticks;
            var now = DateTime.Now;

            Random rand = new Random(Thread.CurrentThread.ManagedThreadId * DateTime.Now.Second);
            while (true)
            {
                if (now.Hour == 10 && now.Minute < 38)
                {
                    var sec = rand.Next(100, 1000);
                    Thread.Sleep(sec);
                    continue;
                }

                try
                {
                    string prizeKey = "myPrize";
                    string lotteryResultStr = "test prize data";

                    //var phone = rand.Next(20);
                    //string phoneNum = phone.ToString();

                    string phoneNum = "999";

                    var trans = redisdb.CreateTransaction();
                    trans.AddCondition(Condition.HashNotExists(prizeKey, phoneNum));
                    trans.HashSetAsync(prizeKey, phoneNum, lotteryResultStr);
                    trans.KeyExpireAsync(prizeKey, new TimeSpan(ts));

                    if (trans.Execute())
                    {
                        Console.WriteLine(string.Format("线程：{0}；成功：{1}", Thread.CurrentThread.ManagedThreadId, phoneNum));
                    }
                    else
                    {
                        Console.WriteLine(string.Format("线程：{0}；存在：{1}", Thread.CurrentThread.ManagedThreadId, phoneNum));
                    }

                    var sec = rand.Next(100, 1000);
                    Thread.Sleep(sec);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    Console.WriteLine(ex.Message);
                    break;
                }
            }
        }

        //public void MultiBulk()
        //{
        //    var ts = DateTime.Now.AddDays(7).Ticks;

        //    Random rand = new Random(Thread.CurrentThread.ManagedThreadId * DateTime.Now.Second);
        //    while (true)
        //    {
        //        try
        //        {
        //            string prizeKey = "myPrize";
        //            string lotteryResultStr = "test prize data";

        //            var phone = rand.Next(20);
        //            string phoneNum = phone.ToString();

        //            //string phoneNum = "999";

        //            var trans = redisdb.CreateTransaction();
        //            trans.AddCondition(Condition.HashNotExists(prizeKey, phoneNum));
        //            trans.HashSetAsync(prizeKey, phoneNum, lotteryResultStr);
        //            trans.KeyExpireAsync(prizeKey, new TimeSpan(ts));

        //            if (trans.Execute())
        //            {
        //                Console.WriteLine(string.Format("线程：{0}；成功：{1}", Thread.CurrentThread.ManagedThreadId, phoneNum));
        //            }
        //            else
        //            {
        //                Console.WriteLine(string.Format("线程：{0}；存在：{1}", Thread.CurrentThread.ManagedThreadId, phoneNum));
        //            }

        //            var sec = rand.Next(100, 1000);
        //            Thread.Sleep(sec);
        //        }
        //        catch (Exception ex)
        //        {
        //            System.Diagnostics.Debug.WriteLine(ex.Message);
        //            Console.WriteLine(ex.Message);
        //            break;
        //        }
        //    }
        //}
    }
}
