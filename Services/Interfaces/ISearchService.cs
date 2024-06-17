using Services.Models.Search;

namespace Services.Interfaces
{
    public interface ISearchService
    {
        Task<ResponseModel> Search(SearchModel model);
    }
}
