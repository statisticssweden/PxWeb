using Microsoft.AspNetCore.Http;
using PxWeb.Api2.Server.Models;
using System.Net.Http;

namespace PxWeb.Mappers
{
    public class LinkCreator : ILinkCreator 
    {
        public enum LinkRelationEnum
        {
            self,
            data,
            describedby
        }

        private string _urlBase;

        public LinkCreator()
        {
            //string baseUrl = $"{Request.Scheme}://{Request.Host}/api/v2/"; // TODO Get from appsetting
            // TODO: LinkCretor options med baseUrl...
            _urlBase = "https://www.api2.com/api/v2/";
        }

        public Link GetTableMetadataJsonLink(LinkRelationEnum relation, string id)
        {
            var link = new Link();
            link.Rel = relation.ToString();
            link.Href = _urlBase + $"tables/{id}/metadata";
            return link;
        }

        //public static Link GetLink(LinkRelationEnum relation, string href, string id, HttpContext httpContext)
        //{
        //    Link link = new Link();

        //    return link;
        //}
    }
}
