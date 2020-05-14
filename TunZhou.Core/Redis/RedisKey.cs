using System;
using System.Collections.Generic;
using System.Text;

namespace TunZhou.Core.Redis
{
    public static class RedisKey
    {
        public static string GetAPIRedisKey(string key)
        {
            return GetRedisKey("TunZhouAPI", key);
        }
        public static string GetRedisKey(string applicationKey, string key)
        {
            return $@" {applicationKey + ":" + key}";
        }
    }
}
