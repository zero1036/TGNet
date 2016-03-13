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
        private IDatabase redisdb;

        /// <summary>
        /// 
        /// </summary>
        public RedisString()
        {
            redisdb = RedisProvider.redis.GetDatabase();
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        public void StringSet()
        {
            var dic = new Dictionary<RedisKey, RedisValue>();
            dic.Add("Num1", "1");
            dic.Add("Num2", "3");

            redisdb.StringSet(dic.ToArray(), When.Always);
        }

        /// <summary>
        /// Bit Set测试
        /// </summary>
        public void BitmapSet()
        {
            bool a1 = redisdb.StringSetBit("BitTest", 100000005, true);
            System.Diagnostics.Debug.WriteLine("操作：" + a1);

            bool a2 = redisdb.StringSetBit("BitTest", 100000004, false);
            System.Diagnostics.Debug.WriteLine("操作：" + a2);
        }

        /// <summary>
        /// Bit get测试
        /// </summary>
        public void BitmapGet()
        {
            bool res = false;

            res = redisdb.StringGetBit("BitTest", 100000005);
            System.Diagnostics.Debug.WriteLine("偏移100000005，结果：" + res);

            res = redisdb.StringGetBit("BitTest", 100000004);
            System.Diagnostics.Debug.WriteLine("偏移100000004，结果：" + res);
        }

        ///// <summary>
        ///// 
        ///// </summary>
        //public void BitOperation_And()
        //{
        //    redisdb.StringBitOperation(Bitwise.And,)
        //}
    }
}
