﻿using Valuator;
using Microsoft.Extensions.Logging;

namespace RankCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var loggerFactory = new LoggerFactory())
            {
                var storage = new RedisStorage(loggerFactory.CreateLogger<RedisStorage>());
                var rankCalculator = new RankCalculator(loggerFactory.CreateLogger<RankCalculator>(), storage);
                rankCalculator.Run();
            }
        }
    }
}