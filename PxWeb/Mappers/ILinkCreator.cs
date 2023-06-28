using PxWeb.Api2.Server.Models;
using static PxWeb.Mappers.LinkCreator;

namespace PxWeb.Mappers
{
    public interface ILinkCreator
    {
        Link GetTablesLink(LinkRelationEnum relation, string language, string query, int pagesize, int pageNumber, bool showLangParam = true);
        Link GetTableLink(LinkRelationEnum relation, string id, string language, bool showLangParam = true);
        Link GetTableMetadataJsonLink(LinkRelationEnum relation, string id, string language, bool showLangParam = true);
        Link GetTableDataLink(LinkRelationEnum relation, string id, string language, bool showLangParam = true);
        Link GetCodelistLink(LinkRelationEnum relation, string id, string language, bool showLangParam = true);
        Link GetFolderLink(LinkRelationEnum relation, string id, string language, bool showLangParam = true);
    }
}
