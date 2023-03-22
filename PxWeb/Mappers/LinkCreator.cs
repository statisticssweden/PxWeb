using J2N.Text;
using Microsoft.AspNetCore.Http;
using PxWeb.Api2.Server.Models;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;

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

        public Link GetTableMetadataJsonLink(LinkRelationEnum relation, string id, string language = "")
        {
            var link = new Link();
            link.Rel = relation.ToString();
            link.Href = CreateLink($"tables/{id}/metadata", language); 

            return link;
        }

        public Link GetCodelistLink(LinkRelationEnum relation, string id, string language = "")
        {
            var link = new Link();
            link.Rel = relation.ToString();
            link.Href = CreateLink($"codeLists/{id}", language);

            return link;
        }

        private string CreateLink(string endpointUrl, string language)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(_urlBase);
            sb.Append(endpointUrl);

            if (!string.IsNullOrEmpty(language))
            {
                sb.Append("?lang=");
                sb.Append(language);
            }

            return sb.ToString();
        }
    }
}
