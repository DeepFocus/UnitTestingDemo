using System;
using System.Runtime.Caching;

namespace MvcDemoSite.Models
{
    public class MemoryCacheManager : ICacheManager
    {
        private readonly ObjectCache _cache = MemoryCache.Default;

        public void Store(string key, object data, int seconds)
        {  
            
            var policy = new CacheItemPolicy
            {
                AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddSeconds(seconds))
            };
            _cache.Add(key, data, policy);
        }

        public object Get(string key)
        {
            return _cache.Get(key);
        }
    }
}