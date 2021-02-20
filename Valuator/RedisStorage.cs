using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;

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
            db.ListRightPush(_textsIdentifiersKey, key);
        }

        public List<string> GetTextsKeys()
        {
            IDatabase db = _connectionMultiplexer.GetDatabase();
            return db.ListRange(_textsIdentifiersKey).Select(x => x.ToString()).ToList();
        }

        public string Load(string key)
        {
            IDatabase db = _connectionMultiplexer.GetDatabase();
            return db.StringGet(key);
        }
    }
}