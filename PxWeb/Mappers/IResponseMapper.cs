using Microsoft.AspNetCore.Http;
using PCAxis.Menu;
using PxWeb.Models.Api2;

namespace PxWeb.Mappers
{
    public interface IResponseMapper
    {
        public Folder GetFolder(PxMenuItem currentItem, HttpContext httpContext);
    }
}
