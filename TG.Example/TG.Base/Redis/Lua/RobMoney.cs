using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
using Newtonsoft.Json;

namespace TG.Example
{
    public class RobMoney
    {

        public class Hongbao
        {
            public int id { get; set; }

            public int userId { get; set; }

            public int money { get; set; }
        }

        private string hongBaoList = "hongBaoList";
        private string hongBaoConsumedList = "hongBaoConsumedList";
        private string hongBaoConsumedMap = "hongBaoConsumedMap";
        private string getHongBaoScript = @"-- 函数：尝试获得红包，如果成功，则返回json字符串，如果不成功，则返回空
                                        -- 参数：红包队列名， 已消费的队列名，去重的Map名，用户ID
                                        -- 返回值：nil 或者 json字符串，包含用户ID：userId，红包ID：id，红包金额：money

                                        -- 如果用户已抢过红包，则返回nil
                                        if redis.call('hexists', KEYS[3], KEYS[4]) ~= 0 then
                                          return nil
                                        else
                                          -- 先取出一个小红包
                                          local hongBao = redis.call('rpop', KEYS[1]);
                                          if hongBao then
                                            local x = cjson.decode(hongBao);
                                            -- 加入用户ID信息
                                            x['userId'] = KEYS[4];
                                            local re = cjson.encode(x);
                                            -- 把用户ID放到去重的set里
                                            redis.call('hset', KEYS[3], KEYS[4], KEYS[4]);
                                            -- 把红包放到已消费队列里
                                            redis.call('lpush', KEYS[2], re);
                                            return re;
                                          end
                                        end
                                        return nil";


        public void AddMoneyToRedis()
        {
            IDatabase redisdb = RedisProvider.redis.GetDatabase();

            Hongbao hb = null;
            for (var i = 1; i <= 100; i++)
            {
                hb = new Hongbao()
                {
                    id = i,
                    userId = -1,
                    money = 100
                };

                var hbJson = JsonConvert.SerializeObject(hb);

                redisdb.ListLeftPush(hongBaoList, hbJson);

            }

        }


        public void PPlkMoney1()
        {
            IDatabase redisdb = RedisProvider.redis.GetDatabase(5);

            string moneyScript = @"-- 函数：尝试获得红包，如果成功，则返回json字符串，如果不成功，则返回空
                                        -- 参数：红包队列名， 已消费的队列名，去重的Map名，用户ID
                                        -- 返回值：nil 或者 json字符串，包含用户ID：userId，红包ID：id，红包金额：money

local result={};
local inventoriesCount=redis.call('LLEN', KEYS[1]);
local inventoriesTotalCount=redis.call('LLEN', KEYS[2]);

if inventoriesCount ~= 0 then
    result['CurrentRecord']=inventoriesCount-1;
    result['TotalRecord']=inventoriesTotalCount-1;
    
    local prize = redis.call('LRANGE', KEYS[1], 0, 50); 
    local prizeTemp = 0; 
    for i,v in ipairs(prize) do
	    prizeTemp = prizeTemp + v;
    end  
    result['CurrentPrize']=prizeTemp;
    
    local prizeTotal = redis.call('LRANGE', KEYS[2], 0, 50);  
    prizeTemp = 0; 
    for i,v in ipairs(prizeTotal) do
	    prizeTemp = prizeTemp + v;
    end  
    result['TotalPrize']=prizeTemp
end

local re = cjson.encode(result);
return re;";

            var res = redisdb.ScriptEvaluate(moneyScript, new RedisKey[] { "Activity:LuckyMoney2Inventory:DragonBoat:148_196745", "Activity:LuckyMoney2Inventory:DragonBoat:148_196745Total" }, new RedisValue[] { }, CommandFlags.None);

            Console.WriteLine(res.ToString());
        }


        public void GoRobMoney()
        {
            IDatabase redisdb = RedisProvider.redis.GetDatabase();
            var isexist = redisdb.KeyExists("name");  //可直接调用
            //((StackExchange.Redis.RedisResult.SingleRedisResult)(res)).value
            //var res = redisdb.ScriptEvaluate(getHongBaoScript, new RedisKey[] { hongBaoList, hongBaoConsumedList, hongBaoConsumedMap, "2" }, new RedisValue[] { }, CommandFlags.None);

            Console.WriteLine(isexist.ToString());

            Task task = null;
            for (var it = 1; it <= 5; it++)
            {
                task = new Task(() =>
                {
                    int userId = 100 / it * 1;

                    while (true)
                    {
                        var exres = redisdb.ScriptEvaluate(getHongBaoScript, new RedisKey[] { hongBaoList, hongBaoConsumedList, hongBaoConsumedMap, userId.ToString() }, new RedisValue[] { }, CommandFlags.None);
                        userId++;
                        if (!exres.IsNull)
                        {
                            Console.WriteLine(userId + " is robbing");
                        }
                        else
                        {
                            break;
                        }

                    }

                });
                task.Start();
            }


        }

        public void TestConsumedHongbao()
        {
            IDatabase redisdb = RedisProvider.redis.GetDatabase();
            RedisValue[] res = redisdb.ListRange(hongBaoConsumedList, 0, 99);  //可直接调用

            var list = res.Select(p => JsonConvert.DeserializeObject<Hongbao>(p.ToString())).ToList();

            var listDis = list.Distinct(new HongbaoComparer()).ToList();

            Console.WriteLine(list.Count == 100 && listDis.Count == 100 ? "分发成功" : "分发失败");
        }

        public class HongbaoComparer : IEqualityComparer<Hongbao>
        {
            public bool Equals(Hongbao x, Hongbao y)
            {
                if (x.userId == y.userId)
                {
                    return false;
                }
                return true;
            }

            public int GetHashCode(Hongbao obj)
            {
                return obj.GetHashCode();
            }

        }
    }
}
