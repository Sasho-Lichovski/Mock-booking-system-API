namespace Repository.Interfaces
{
    public interface ICacheRepository
    {
        string Get(string cacheKey, string additionalParam);
        void Set(string cacheKey, string additionalParam, object value);
        void LogError(string cacheKey, object value);
    }
}
