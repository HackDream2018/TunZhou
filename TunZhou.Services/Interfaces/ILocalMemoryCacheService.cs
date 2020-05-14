using System;
using System.Collections.Generic;
using System.Text;

namespace TunZhou.Services.Interfaces
{
    public interface ILocalMemoryCacheService
    {
        /// <summary>
        /// 表示午夜时刻
        /// </summary>
        DateTimeOffset MidnightTime { get; }

        /// <summary>
        /// 默认过期时间（毫秒）
        /// </summary>
        TimeSpan DefaultExpiration { get; }

        void Set<TValue>(string key, TValue value, TimeSpan absoluteExpiration);

        void SetEmpty(string key, TimeSpan absoluteExpiration);

        /// <summary>
        /// 向本地缓存中写入数据，当value为null或者空集合时，写入空标识
        /// </summary>
        void SetDataOrEmptyTag<TValue>(string key, TValue value, TimeSpan absoluteExpiration);

        bool Get<TValue>(string key, out TValue value) where TValue : class;

        /// <summary>
        /// 移除所有缓存项，并返回移除缓存项的数量
        /// </summary>
        int ClearAllCache();
    }
}
