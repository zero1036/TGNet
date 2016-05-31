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

            string pwd = "lkM0ney*";
            string content = "195739";
            content.
            var cc = TextEncrypt("1223334", "ppM0ney*2");
            string ss = new String(cc);

            Debug.WriteLine(ss);
        }
        private char[] TextEncrypt(string content, string secretKey)
        {
            char[] data = content.ToCharArray();
            char[] key = secretKey.ToCharArray();
            for (int i = 0; i < data.Length; i++)
            {
                data[i] ^= key[i % key.Length];
            }
            return data;
        }
        private string TextDecrypt(char[] data, string secretKey)
        {
            char[] key = secretKey.ToCharArray();
            for (int i = 0; i < data.Length; i++)
            {
                data[i] ^= key[i % key.Length];
            }
            return new string(data);
        }
    }
}

    1-3000-10000元体验金
    2-3000-8元现金券

    3-5000-迅雷会员
    3-5000-3500元体验金
    4-5000-10元话费充值卡
    4-5000-3500元体验金

    5-1万-20元现金券
    5-1万-爱奇艺会员
    6-1万-10元现金券
    6-1万-30元话费充值卡

    7-5万-100元现金券
    7-5万-100元话费充值卡
    8-5万-100元现金券
    8-5万-蜘蛛网电影票*2

    9-10万-300元现金券
    9-10万-100元京东E卡
    10-10万-300现金券
    10-10万-100话费充值卡

    11-50万-2000元现金券
    11-50万-500话费充值卡
    12-50万-2500元现金券

    13-100万-5000元现金券
    13-100万-1000元京东E卡
    14-100万-6000元现金券

