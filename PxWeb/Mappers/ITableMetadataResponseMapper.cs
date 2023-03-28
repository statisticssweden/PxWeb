using PCAxis.Paxiom;
using PxWeb.Api2.Server.Models;

namespace PxWeb.Mappers
{
    public interface ITableMetadataResponseMapper
    {
        TableMetadataResponse Map(PXModel model, string id, string language);
    }
}
