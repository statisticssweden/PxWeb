using Microsoft.AspNetCore.Http;
using PCAxis.Menu;
using PCAxis.Paxiom;
using Px.Search;
using PxWeb.Api2.Server.Models;

namespace PxWeb.Mappers
{
    public interface IFolderResponseMapper
    {
        public Folder GetFolder(PxMenuItem currentItem, string language, bool root = false);
    }
}
