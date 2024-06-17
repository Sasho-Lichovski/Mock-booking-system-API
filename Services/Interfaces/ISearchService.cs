using Services.Models.Search;

namespace Services.Interfaces
{
    public interface ISearchService
    {
        Task<ResponseReq> Search(SearchReq model);
    }
}
