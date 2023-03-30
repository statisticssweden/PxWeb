using PCAxis.Paxiom;
using Px.Search;
using PxWeb.Api2.Server.Models;
using System.Collections.Generic;

namespace PxWeb.Mappers
{
    public interface ITablesResponseMapper
    {
        TablesResponse Map(IEnumerable<SearchResult> searchResult, string lang, string query);
    }
}
