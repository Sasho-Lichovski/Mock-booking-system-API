using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Repository.Interfaces;
using Utils.Constants;

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

        public void LogError(string cacheKey, object value)
        {
            var jsonString = Get(Error.Controller, "");
            if (!string.IsNullOrEmpty(jsonString))
                jsonString = $"{jsonString},{JsonConvert.SerializeObject(value)}";
            else
                jsonString = JsonConvert.SerializeObject(value);

            memoryCache.Set(cacheKey, jsonString);
        }
    }
}
