using PCAxis.Paxiom;
using Px.Abstractions;
using PxWeb.Api2.Server.Models;

namespace PxWeb.Mappers
{
    public interface ICodelistResponseMapper
    {
        CodeListResponse Map(Codelist codelist, string language);
    }
}
