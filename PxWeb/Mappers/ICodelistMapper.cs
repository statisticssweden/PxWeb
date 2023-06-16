using PCAxis.Paxiom;
using Px.Abstractions;

namespace PxWeb.Mappers
{
    public interface ICodelistMapper
    {
        Codelist Map(Grouping grouping);
        Codelist Map(Valueset valueset);
    }
}
