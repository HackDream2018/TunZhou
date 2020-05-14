using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TunZhou.Core.Redis;
using TunZhou.DtoModel.DTO;
using TunZhou.DtoModel.Response;
using TunZhou.Repositories.Interfaces;
using TunZhou.Services.Interfaces;

namespace TunZhou.Services.Imples
{
    public class TestService : ITestService
    {
        #region 接口注入
        private readonly ITestRepository _testRepository;
        private readonly ILocalMemoryCacheService _localMemoryCache;
        private readonly IConfiguration _configuration;
        private readonly IRedisService _redis;
        private readonly ICacheService _cache;
        private readonly TimeSpan Expiration;
        private readonly int RedisExpiration;

        public TestService(ITestRepository testRepository, IConfiguration configuration, ILocalMemoryCacheService localMemoryCache, IRedisService redis, ICacheService cache)
        {
            _testRepository = testRepository;
            _configuration = configuration;
            _localMemoryCache = localMemoryCache;
            _redis = redis;
            _cache = cache;
            //缓存过期时间10min
            Expiration = TimeSpan.FromMinutes(10);
            RedisExpiration = 10 * 60;
        }
        #endregion

        /// <summary>
        /// 本地DB数据，走MemoryCache
        /// </summary>
        /// <returns></returns>
        public async Task<TestResponse> GetUser()
        {
            IEnumerable<UserDTO> userList;
            var cacheKey = "userListcacheKey";
            if (_localMemoryCache.Get(cacheKey, out IEnumerable<UserDTO> cacheValue))
            {
                userList = cacheValue;
            }
            else
            {
                userList = await _testRepository.GetUser();
                _localMemoryCache.SetDataOrEmptyTag(cacheKey, userList, Expiration);
            }

            var res = new TestResponse();
            res.Data = userList.ToList();
            return res;
        }

        /// <summary>
        /// 本地DB数据，走Redis
        /// </summary>
        /// <returns></returns>
        public async Task<TestResponse> GetUserForRedis()
        {
            IEnumerable<UserDTO> userList;
            var cacheKey = "userListcacheKey";
            var rediscacheKey = RedisKey.GetAPIRedisKey(cacheKey);
            var rediscache = await _redis.StringGetAsync(rediscacheKey);
            if (!string.IsNullOrEmpty(rediscache))
            {
                userList = JsonConvert.DeserializeObject<IEnumerable<UserDTO>>(rediscache);
            }
            else
            {
                userList = await _testRepository.GetUser();
                _redis.StringSet(rediscacheKey, JsonConvert.SerializeObject(userList), RedisExpiration);
            }
            var res = new TestResponse();
            res.Data = userList.ToList();
            return res;
        }

        /// <summary>
        /// 本地DB数据，走2级缓存，先走MemoryCache，没有走Redis
        /// </summary>
        /// <returns></returns>
        public async Task<TestResponse> GetUserForRedisAndMemoryCache()
        {
            IEnumerable<UserDTO> userList;
            var cacheKey = "userListcacheKey";
            if (_cache.Get(cacheKey, Expiration, out IEnumerable<UserDTO> cacheValue))
            {
                userList = cacheValue;
            }
            else
            {
                userList = await _testRepository.GetUser();
                _cache.Set(cacheKey, userList, Expiration, RedisExpiration);
            }

            var res = new TestResponse();
            res.Data = userList.ToList();
            return res;
        }
    }
}
