using Services.Models.Error;

namespace Services.Interfaces
{
    public interface IErrorService
    {
        List<ErrorModel> GetErrors(DateTime dateFrom);
        void LogError(ErrorModel ex);
    }
}
