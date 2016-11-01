using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace TG.Example
{
    public static class RedisProvider
    {
        //private static string constr = "127.0.0.1";
        private static string constr = "192.168.1.72";

        //private static string constr = "192.168.1.112";
        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            return ConnectionMultiplexer.Connect(constr);
        });

        public static ConnectionMultiplexer redis
        {
            get
            {
                return lazyConnection.Value;
            }
        }
    }
}
