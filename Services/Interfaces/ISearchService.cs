using RestSharp;
using Services.Models.Search;

namespace Services.Interfaces
{
    public interface ISearchService
    {
        Task<SearchRes> Search(SearchReq model);
    }
}
