using System;
using System.Collections.Generic;
using System.Text;

namespace TunZhou.Services.Interfaces
{
    public interface ICacheService
    {
        bool Get<TValue>(string key, TimeSpan absoluteExpiration, out TValue value) where TValue : class;
        void Set<TValue>(string key, TValue value, TimeSpan absoluteExpiration, int RedisExpiration);
    }
}
