using Newtonsoft.Json;
using Repository.Interfaces;
using Services.Interfaces;
using Services.Models.Error;
using Utils.Constants;

namespace Services.Services
{
    public class ErrorService : IErrorService
    {
        private readonly ICacheRepository cacheRepository;

        public ErrorService(ICacheRepository cacheRepository)
        {
            this.cacheRepository = cacheRepository;
        }

        public List<ErrorModel> GetErrors(DateTime dateFrom)
        {
            var errors = new List<ErrorModel>();
            var jsonString = cacheRepository.Get(Error.Controller, "");
            if (string.IsNullOrEmpty(jsonString))
                return errors;

            errors = JsonConvert.DeserializeObject<List<ErrorModel>>($"[{jsonString}]");
            return errors.Where(x => x.LogTime >= dateFrom).ToList();
        }

        public void LogError(ErrorModel ex)
        {
            cacheRepository.LogError(Error.Controller, ex);
        }
    }
}
