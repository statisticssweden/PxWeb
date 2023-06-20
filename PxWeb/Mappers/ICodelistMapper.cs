using PCAxis.Paxiom;
using Px.Abstractions;
using PCAxis.Sql;

namespace PxWeb.Mappers
{
    public interface ICodelistMapper
    {
        Codelist Map(PCAxis.Paxiom.Grouping grouping);
        Codelist Map(PCAxis.Sql.Models.Grouping grouping);
        Codelist Map(PCAxis.Sql.Models.ValueSet valueset);
    }
}
