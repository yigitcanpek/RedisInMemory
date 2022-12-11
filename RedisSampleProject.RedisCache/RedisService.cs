using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedisSampleProject.RedisCache
{
    public class RedisService
    {
        private readonly ConnectionMultiplexer _connectionMultiplexer;

        public RedisService(string url)
        {
            _connectionMultiplexer = ConnectionMultiplexer.Connect(url);
            
        }
        public IDatabase Getdb(int dbIndex)
        {
            return _connectionMultiplexer.GetDatabase(dbIndex);
        }
    }
}
