using PxWeb.Api2.Server.Models;
using static PxWeb.Mappers.LinkCreator;

namespace PxWeb.Mappers
{
    public interface ILinkCreator
    {
        Link GetTableMetadataJsonLink(LinkRelationEnum relation, string id);
        Link GetCodelistLink(LinkRelationEnum relation, string id);

    }
}
