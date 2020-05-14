using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TunZhou.Core.Redis
{
    public class RedisService : IRedisService
    {
        private readonly IConfiguration _configuration;
        private static string _conn;
        private static string _pwd;
        private static int _dbIndex;

        public RedisService(IConfiguration configuration)
        {
            RedisConfig config = new RedisConfig(configuration);
            _configuration = configuration;
            _conn = $"{config.RedisIP}:{config.RedisPort}";
            _pwd = config.Password;
            _dbIndex = config.DBIndex;
        }

        static ConnectionMultiplexer _redis;

        static readonly object _locker = new object();

        #region 单例模式

        public static ConnectionMultiplexer Manager
        {
            get
            {
                if (_redis == null)
                {
                    lock (_locker)
                    {
                        if (_redis != null) return _redis;

                        _redis = GetManager();

                        return _redis;
                    }
                }
                return _redis;
            }
        }

        private static ConnectionMultiplexer GetManager(string connectionString = null)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = _conn;
            }

            var options = ConfigurationOptions.Parse(connectionString);

            options.Password = _pwd;

            return ConnectionMultiplexer.Connect(options);
        }

        #endregion

        #region 同步

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="folder">目录</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expireMinutes">过期时间，单位：分钟。默认60000000分钟</param>
        /// <param name="db">库，默认第一个。0~15共16个库</param>
        /// <returns></returns>
        public bool StringSet(string key, string value, int expireMinutes = 60000000, int db = -1)
        {
            return Manager.GetDatabase(db).StringSet(key, value, TimeSpan.FromMinutes(expireMinutes));
        }


        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="folder">目录</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expireMinutes">过期时间，单位：分钟。默认60000000分钟</param>
        /// <param name="db">库，默认第一个。0~15共16个库</param>
        /// <returns></returns>
        public Task<bool> StringSetAsync(string key, string value, int expireMinutes = 60000000, int db = -1)
        {
            try
            {
                var expireTime = DateTime.Now.AddMinutes(expireMinutes).Subtract(DateTime.Now);
                return Manager.GetDatabase(db).StringSetAsync(key, value, expireTime);
            }
            catch (System.Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="folder">目录</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expireMinutes">过期时间，单位：分钟。默认60000000分钟</param>
        /// <param name="db">库，默认第一个。0~15共16个库</param>
        /// <returns></returns>
        public Task<bool> StringSetAsync(string key, string value, int db = -1)
        {
            try
            {
                return Manager.GetDatabase(db).StringSetAsync(key, value);
            }
            catch (System.Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="folder">目录</param>
        /// <param name="key">键</param>
        /// <param name="db">库，默认第一个。0~15共16个库</param>
        /// <returns></returns>
        public string StringGet(string key, int db = -1)
        {
            try
            {
                return Manager.GetDatabase(db).StringGet(key);
            }
            catch (System.Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="folder">目录</param>
        /// <param name="key">键</param>
        /// <param name="db">库，默认第一个。0~15共16个库</param>
        /// <returns></returns>
        public async Task<RedisValue> StringGetAsync(string key, int db = -1)
        {
            try
            {
                return await Manager.GetDatabase(db).StringGetAsync(key);
            }
            catch (System.Exception)
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="folder">目录</param>
        /// <param name="key">键</param>
        /// <param name="db">库，默认第一个。0~15共16个库</param>
        /// <returns></returns>
        public bool StringRemove(string key, int db = -1)
        {
            try
            {
                return Manager.GetDatabase(db).KeyDelete(key);
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 设置 Key 的时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expiry"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool KeyExpire(string key, TimeSpan expiry, int db = -1)
        {
            return Manager.GetDatabase(db).KeyExpire(key, expiry);
        }

        /// <summary>
        /// 设置 Key 的时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expiry"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public Task<bool> KeyExpireAsync(string key, TimeSpan expiry, int db = -1)
        {
            try
            {
                return Manager.GetDatabase(db).KeyExpireAsync(key, expiry);
            }
            catch (System.Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 设置 Key 的时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expiry"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public Task<bool> KeyExpireAsync(string key, DateTime expiry, int db = -1)
        {
            try
            {
                return Manager.GetDatabase(db).KeyExpireAsync(key, expiry);
            }
            catch (System.Exception)
            {
                return null;
            }
        }

        #endregion

        #region 异步

        #endregion

        /// <summary>
        /// 在列表尾部插入值。如果键不存在，先创建再插入值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public long ListRightPush(string redisKey, string redisValue, int db = -1)
        {
            return Manager.GetDatabase(db).ListRightPush(redisKey, redisValue);
        }

        /// <summary>
        /// 在列表头部插入值。如果键不存在，先创建再插入值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public long ListLeftPush(string redisKey, string redisValue, int db = -1)
        {
            try
            {
                return Manager.GetDatabase(db).ListLeftPush(redisKey, redisValue);
            }
            catch (System.Exception)
            {
                return 0;
            }
        }

        public Task<long> ListLeftPushAsync(string redisKey, string redisValue, int db = -1)
        {
            try
            {
                return Manager.GetDatabase(db).ListLeftPushAsync(redisKey, redisValue);
            }
            catch (System.Exception)
            {
                return null;
            }

        }

        /// <summary>
        /// 返回列表上该键的长度，如果不存在，返回 0
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public long ListLength(string redisKey, int db = -1)
        {
            try
            {
                return Manager.GetDatabase(db).ListLength(redisKey);
            }
            catch (System.Exception)
            {
                return 0;
            }

        }

        /// <summary>
        /// 检查key是否存在
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool KeyExists(string redisKey, int db = -1)
        {
            try
            {
                return Manager.GetDatabase(db).KeyExists(redisKey);
            }
            catch (System.Exception)
            {
                return false;
            }

        }

        /// <summary>
        /// 检查key是否存在
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public Task<bool> KeyExistsAsync(string redisKey, int db = -1)
        {
            try
            {
                return Manager.GetDatabase(db).KeyExistsAsync(redisKey);
            }
            catch (System.Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取list值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public RedisValue[] ListRange(string redisKey, int db = -1)
        {
            try
            {
                return Manager.GetDatabase(db).ListRange(redisKey);
            }
            catch (System.Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取list值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public Task<RedisValue[]> ListRangeAsync(string redisKey, int db = -1)
        {
            try
            {
                return Manager.GetDatabase(db).ListRangeAsync(redisKey, 1, 10);
            }
            catch (System.Exception)
            {
                return null;
            }
        }


        /// <summary>
        /// 异步删除key
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public Task<bool> KeyDeleteAsync(string redisKey, int db = -1)
        {
            try
            {
                return Manager.GetDatabase(db).KeyDeleteAsync(redisKey);
            }
            catch (System.Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 同步删除key
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool KeyDelete(string redisKey, int db = -1)
        {
            try
            {
                return Manager.GetDatabase(db).KeyDelete(redisKey);
            }
            catch (System.Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 为数字增长val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val">可以为负数</param>
        /// <returns>增长后的值</returns>
        public long StringIncrement(string key, int val, int db = -1)
        {
            try
            {
                return Manager.GetDatabase(db).StringIncrement(key, val);
            }
            catch (System.Exception)
            {
                return 0;
            }
        }
        /// <summary>
        /// 为数字增长val +1操作
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val">可以为负数</param>
        /// <returns>增长后的值</returns>
        public Task<long> StringIncrementAsync(string key, int val, int db = -1)
        {
            try
            {
                return Manager.GetDatabase(db).StringIncrementAsync(key, val);
            }
            catch (System.Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 为数字减少val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val">可以为负数</param>
        /// <returns>减少后的值</returns>
        public long StringDecrement(string key, int val, int db = -1)
        {
            try
            {
                return Manager.GetDatabase(db).StringDecrement(key, val);
            }
            catch (System.Exception)
            {
                return 0;
            }
        }
        /// <summary>
        /// 为数字减少val -1操作
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val">可以为负数</param>
        /// <returns>减少后的值</returns>
        public Task<long> StringDecrementAsync(string key, int val, int db = -1)
        {
            try
            {
                return Manager.GetDatabase(db).StringDecrementAsync(key, val);
            }
            catch (System.Exception)
            {
                return null;
            }
        }
    }
}
