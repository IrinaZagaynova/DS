using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Collections.Generic;

namespace Valuator
{
    public class RedisStorage : IStorage
    {
        private readonly ILogger<RedisStorage> _logger;
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly RedisKey _textsIdentifiersKey = "textsIdentifiers";
        private readonly string _host = "localhost";
 
        public RedisStorage(ILogger<RedisStorage> logger)
        {
            _logger = logger;
            _connectionMultiplexer = ConnectionMultiplexer.Connect(_host);
        }

        public void Store(string key, string value)
        {
            IDatabase db = _connectionMultiplexer.GetDatabase();
            db.StringSet(key, value);
        }

        public void StoreTextKey(string key)
        {
            IDatabase db = _connectionMultiplexer.GetDatabase();
            db.SetAdd(_textsIdentifiersKey, key);
        }

        public bool IsTextExist(string text)
        {
            IDatabase db = _connectionMultiplexer.GetDatabase();

            var keys = db.SetMembers(_textsIdentifiersKey);

            foreach (var key in keys)
            {
                if (Load(key) == text)
                {
                    return true;
                }
            }

            return false;
        }

        public string Load(string key)
        {
            IDatabase db = _connectionMultiplexer.GetDatabase();
            return db.StringGet(key);
        }
    }
}