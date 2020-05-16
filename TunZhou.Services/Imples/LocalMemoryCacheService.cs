using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using TunZhou.Core;
using TunZhou.Services.Interfaces;

namespace TunZhou.Services.Imples
{
    public class LocalMemoryCacheService : ILocalMemoryCacheService
    {
        private const string LOCAL_MEMORY_CACHE_EXPIRATION = "cache:localMemoryCacheDefaultExpiration";
        private const string Nothing = "NIL";

        private readonly IMemoryCache MemoryCache;
        private readonly IConfiguration Configuration;

        /// <summary>
        /// 午夜时间
        /// </summary>
        public DateTimeOffset MidnightTime
        {
            get
            {
                var now = DateTime.Now;
                return new DateTimeOffset(new DateTime(now.Year, now.Month, now.Day, 23, 59, 59, 999));
            }
        }

        public TimeSpan DefaultExpiration
        {
            get
            {
                var expiration = int.Parse(Configuration[LOCAL_MEMORY_CACHE_EXPIRATION]);
                return new TimeSpan(0, 0, 0, 0, expiration);
            }
        }

        public LocalMemoryCacheService(IMemoryCache memoryCache, IConfiguration configuration)
        {
            MemoryCache = memoryCache;
            Configuration = configuration;
        }

        public void Set<TValue>(string key, TValue value, TimeSpan absoluteExpiration) => MemoryCache.Set(key, value, absoluteExpiration);

        public void SetEmpty(string key, TimeSpan absoluteExpiration) => Set(key, Nothing, absoluteExpiration);

        public void SetDataOrEmptyTag<TValue>(string key, TValue value, TimeSpan absoluteExpiration)
        {
            var writeEmptyTag = false;
            if (value == null)
            {
                writeEmptyTag = true;
            }
            if (writeEmptyTag == false && value is IEnumerable<TValue>)
            {
                var enumerable = value as IEnumerable<TValue>;
                if (enumerable.Empty())
                {
                    writeEmptyTag = true;
                }
            }
            if (writeEmptyTag)
            {
                SetEmpty(key, absoluteExpiration);
            }
            else
            {
                Set(key, value, absoluteExpiration);
            }
        }

        public bool Get<TValue>(string key, out TValue value)
                where TValue : class
        {
            value = default;
            var cacheValue = MemoryCache.Get(key);
            if (cacheValue == null)
            {
                return false;
            }
            if (cacheValue is string valueStr && valueStr == Nothing)
            {
                return true;
            }

            value = cacheValue as TValue;
            return true;
        }
        public void ClearCacheForKey(string key) => MemoryCache.Remove(key);
        public int ClearAllCache()
        {
            var typeInfo = MemoryCache.GetType();
            var countInfo = typeInfo.GetProperty("Count", BindingFlags.Public | BindingFlags.Instance);
            var count = countInfo.GetValue(MemoryCache) as int?;

            var entriesCollection = typeInfo.GetProperty("EntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance);
            var innerCacheEntries = entriesCollection.GetValue(MemoryCache);
            var entriesCollectionType = innerCacheEntries.GetType();
            var clearMethod = entriesCollectionType.GetMethod("Clear", BindingFlags.Instance | BindingFlags.Public);
            clearMethod.Invoke(innerCacheEntries, null);

            return count.HasValue ? count.Value : 0;
        }

    }
}
