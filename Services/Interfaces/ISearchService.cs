using Services.Models.Search;

namespace Services.Interfaces
{
    public interface ISearchService
    {
        ResponseModel Search(SearchModel model);
    }
}
