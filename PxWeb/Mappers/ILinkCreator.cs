using PxWeb.Api2.Server.Models;
using static PxWeb.Mappers.LinkCreator;

namespace PxWeb.Mappers
{
    public interface ILinkCreator
    {
        Link GetTableMetadataJsonLink(LinkRelationEnum relation, string id, string language, bool currentLanguage = true);
        Link GetTableDataLink(LinkRelationEnum relation, string id, string language, bool currentLanguage = true);
        Link GetCodelistLink(LinkRelationEnum relation, string id, string language, bool currentLanguage = true);

    }
}
