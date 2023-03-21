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
            describedby,
            metadata
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

            // TODO: Handle all languages to self links...
            return link;
        }

        public Link GetCodelistLink(LinkRelationEnum relation, string id)
        {
            var link = new Link();
            link.Rel = relation.ToString();
            link.Href = _urlBase + $"codeLists/{id}"; 

            return link;
        }
    }
}
