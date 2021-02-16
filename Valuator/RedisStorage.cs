using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Collections.Generic;

namespace Valuator
{
    public class RedisStorage : IStorage
    {
        private readonly ILogger<RedisStorage> _logger;
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public RedisStorage(ILogger<RedisStorage> logger)
        {
            _logger = logger;
            _connectionMultiplexer = ConnectionMultiplexer.Connect("localhost");
        }

        public void Store(string key, string value)
        {
            IDatabase db = _connectionMultiplexer.GetDatabase();
            db.StringSet(key, value);
        }

        public string Load(string key)
        {
            IDatabase db = _connectionMultiplexer.GetDatabase();
            return db.StringGet(key);
        }

        public Dictionary<string, string> LoadAll()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();

            var keys = _connectionMultiplexer.GetServer("localhost:6379").Keys();

            foreach (var item in keys)
            {
                string key = item.ToString();
                data.Add(key, Load(key));
            }

            return data;
        }
    }
}