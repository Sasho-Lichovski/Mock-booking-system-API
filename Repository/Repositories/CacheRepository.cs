using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Repository.Interfaces;

namespace Repository.Repositories
{
    public class CacheRepository : ICacheRepository
    {
        private readonly IMemoryCache memoryCache;

        public CacheRepository(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        public string Get(string cacheKey, string additionalParam)
        {
            if (memoryCache.TryGetValue($"{cacheKey}{additionalParam}", out object values))
                return values.ToString();
            else
                return string.Empty;
        }

        public void Set(string cacheKey, string additionalParam, object value)
        {
            memoryCache.Set($"{cacheKey}{additionalParam}", JsonConvert.SerializeObject(value));
        }
    }
}
