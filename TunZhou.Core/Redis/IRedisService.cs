using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TunZhou.Core.Redis
{
    public interface IRedisService
    {
        bool StringSet(string key, string value, int expireMinutes = 60000000, int db = -1);
        Task<bool> StringSetAsync(string key, string value, int expireMinutes = 60000000, int db = -1);
        Task<bool> StringSetAsync(string key, string value, int db = -1);
        string StringGet(string key, int db = -1);
        Task<RedisValue> StringGetAsync(string key, int db = -1);
        bool StringRemove(string key, int db = -1);
        bool KeyExpire(string key, TimeSpan expiry, int db = -1);
        Task<bool> KeyExpireAsync(string key, TimeSpan expiry, int db = -1);
        Task<bool> KeyExpireAsync(string key, DateTime expiry, int db = -1);
        long ListRightPush(string redisKey, string redisValue, int db = -1);
        long ListLeftPush(string redisKey, string redisValue, int db = -1);
        Task<long> ListLeftPushAsync(string redisKey, string redisValue, int db = -1);
        long ListLength(string redisKey, int db = -1);
        bool KeyExists(string redisKey, int db = -1);
        Task<bool> KeyExistsAsync(string redisKey, int db = -1);
        RedisValue[] ListRange(string redisKey, int db = -1);
        Task<RedisValue[]> ListRangeAsync(string redisKey, int db = -1);
        Task<bool> KeyDeleteAsync(string redisKey, int db = -1);
        bool KeyDelete(string redisKey, int db = -1);
        long StringIncrement(string key, int val, int db = -1);
        Task<long> StringIncrementAsync(string key, int val, int db = -1);
        long StringDecrement(string key, int val, int db = -1);
        Task<long> StringDecrementAsync(string key, int val, int db = -1);
    }
}
