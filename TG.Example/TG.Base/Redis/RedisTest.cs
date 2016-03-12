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

    }
}
