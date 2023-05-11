using Px.Search;
using PxWeb.Api2.Server.Models;


namespace PxWeb.Mappers
{
    public interface ITableResponseMapper
    {
        TableResponse Map(SearchResult searchResult, string lang);
    }
}
