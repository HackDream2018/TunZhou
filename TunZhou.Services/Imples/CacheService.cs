using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using TunZhou.Core.Redis;
using TunZhou.Services.Interfaces;

namespace TunZhou.Services.Imples
{
    public class CacheService : ICacheService
    {
        private readonly ILocalMemoryCacheService _localMemoryCache;
        private readonly IConfiguration _configuration;
        private readonly IRedisService _redis;
        public CacheService(IConfiguration configuration, ILocalMemoryCacheService localMemoryCache, IRedisService redis)
        {
            _configuration = configuration;
            _localMemoryCache = localMemoryCache;
            _redis = redis;
        }

        public bool Get<TValue>(string key, TimeSpan absoluteExpiration, out TValue value)
                where TValue : class
        {
            value = default;
            if (_localMemoryCache.Get(key, out TValue cacheValue))
            {
                value = cacheValue;
            }
            else
            {
                var redisValue = _redis.StringGet(RedisKey.GetAPIRedisKey(key));
                if (!string.IsNullOrEmpty(redisValue))
                {
                    value = JsonConvert.DeserializeObject<TValue>(redisValue);
                    _localMemoryCache.SetDataOrEmptyTag(key, value, absoluteExpiration);
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        public void Set<TValue>(string key, TValue value, TimeSpan absoluteExpiration, int RedisExpiration)
        {
            _localMemoryCache.SetDataOrEmptyTag(key, value, absoluteExpiration);
            try
            {
                _redis.StringSet(RedisKey.GetAPIRedisKey(key), JsonConvert.SerializeObject(value), (int)absoluteExpiration.TotalSeconds);
            }
            catch (Exception e)
            {
                _redis.StringSet(RedisKey.GetAPIRedisKey(key), JsonConvert.SerializeObject(value), int.Parse(_configuration["Redis:ExpiredMinute"]));
            }
        }
    }
}
