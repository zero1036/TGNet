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
        private IDatabase _redisdb;

        public RedisTestData()
        {
            _redisdb = RedisProvider.redis.GetDatabase();
        }

        #region
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
        #endregion

        #region hash 切分
        /// <summary>
        /// Hash对象切分——普通无切分
        /// </summary>
        public void HashSplit_Common()
        {
            IDatabase redisdb = RedisProvider.redis.GetDatabase();

            var dic = new Dictionary<RedisKey, RedisValue>();
            for (var i = 1; i <= 100000; i++)
            {
                dic.Add("object:" + i, "val");
            }

            redisdb.StringSet(dic.ToArray(), When.Always);
        }

        /// <summary>
        /// Hash对象切分——划分成Hash对象，每个Hash切分100
        /// 
        /// </summary>
        public void HashSplit_ByHashHead2()
        {
            IDatabase redisdb = RedisProvider.redis.GetDatabase();

            var less100 = new List<HashEntry>();

            for (var i = 1; i <= 100000; i++)
            {
                var si = i.ToString();
                if (si.Length > 2)
                {
                    var key = si.Substring(0, 2);
                    var hkey = si.Substring(2, si.Length - 2);

                    redisdb.HashSet("object:" + key, new HashEntry[] { 
                        new HashEntry(hkey, "val")});
                }
                else
                {
                    less100.Add(new HashEntry(si, "val"));

                }


                redisdb.HashSet("object:", less100.ToArray());
            }
        }


        /// <summary>
        /// Hash对象切分——划分成Hash对象，每个Hash切分100
        /// 
        /// </summary>
        public void HashSplit_ByHashBack2()
        {
            IDatabase redisdb = RedisProvider.redis.GetDatabase();

            var less100 = new List<HashEntry>();

            for (var i = 1; i <= 100000; i++)
            {
                var si = i.ToString();
                if (si.Length > 2)
                {
                    var key = si.Substring(0, si.Length - 2);
                    var hkey = si.Substring(si.Length - 2, 2);

                    redisdb.HashSet("object:" + key, new HashEntry[] { 
                        new HashEntry(hkey, "val")});
                }
                else
                {
                    less100.Add(new HashEntry(si, "val"));

                }


                redisdb.HashSet("object:", less100.ToArray());
            }
        }

        #endregion

        #region 碎片率实验  Fragmentation ratio

        /// <summary>
        /// FragRatioTest
        /// </summary>
        /// <param name="factor">倍数因子</param>
        /// <param name="count">产生样本数，必须小于等于10000000（1千万）</param>
        public void FragRatioTest(int factor, int count)
        {
            var dic = new Dictionary<RedisKey, RedisValue>();

            var listVal = Enumerable.Range(1, factor).Select((p) =>
            {
                return "1111111111";
            }).ToArray();
            string val = string.Join("", listVal);

            Random rm = new Random();

            //10万条数据，key是8字节byte，value是100字节byte
            for (var i = 1; i <= count; i++)
            {
                string key = string.Empty;
                //非随机
                key = (count * 2 - i).ToString();

                dic.Add(key, val);
            }

            _redisdb.StringSet(dic.ToArray(), When.Always);
        }

        /// <summary>
        /// FragRatioTest
        /// </summary>
        /// <param name="factor">倍数因子</param>
        /// <param name="count">产生样本数，必须小于等于10000000（1千万）</param>
        public void FragRatioTestRamdom(int factor, int count)
        {
            var dic = new Dictionary<RedisKey, RedisValue>();

            var listVal = Enumerable.Range(1, factor).Select((p) =>
            {
                return "1111111111";
            }).ToArray();
            string val = string.Join("", listVal);

            Random rm = new Random();

            //10万条数据，key是8字节byte，value是100字节byte
            for (var i = 1; i <= count; i++)
            {
                string key = string.Empty;

                if (isRam)
                {
                    //随机
                    var target = rm.Next(1, count);
                    key = (count * 2 - i).ToString();
                }
              
                dic.Add(key, val);
            }

            _redisdb.StringSet(dic.ToArray(), When.Always);
        }


        private 
        #endregion
    }
}
