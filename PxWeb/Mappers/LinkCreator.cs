using J2N.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using PxWeb.Api2.Server.Models;
using PxWeb.Config.Api2;
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

        public LinkCreator(IOptions<PxApiConfigurationOptions> configOptions)
        {
            _urlBase = configOptions.Value.BaseURL;
        }

        public Link GetTableLink(LinkRelationEnum relation, string id, string language, bool currentLanguage = true)
        {
            var link = new Link();
            link.Rel = relation.ToString();
            link.Hreflang = language;
            link.Href = CreateURL($"tables/{id}", language, currentLanguage);

            return link;
        }

        public Link GetTableMetadataJsonLink(LinkRelationEnum relation, string id, string language, bool currentLanguage = true)
        {
            var link = new Link();
            link.Rel = relation.ToString();
            link.Hreflang = language;
            link.Href = CreateURL($"tables/{id}/metadata", language, currentLanguage);

            return link;
        }

        public Link GetTableDataLink(LinkRelationEnum relation, string id, string language, bool currentLanguage = true)
        {
            var link = new Link();
            link.Rel = relation.ToString();
            link.Hreflang = language;
            link.Href = CreateURL($"tables/{id}/data", language, currentLanguage);

            return link;
        }

        public Link GetCodelistLink(LinkRelationEnum relation, string id, string language, bool currentLanguage = true)
        {
            var link = new Link();
            link.Rel = relation.ToString();
            link.Hreflang = language;
            link.Href = CreateURL($"codeLists/{id}", language, currentLanguage);

            return link;
        }

        public Link GetFolderLink(LinkRelationEnum relation, string id, string language, bool currentLanguage = true)
        {
            var link = new Link();
            link.Rel = relation.ToString();
            link.Hreflang = language;
            link.Href = CreateURL($"navigation/{id}", language, currentLanguage);

            return link;
        }
        private string CreateURL(string endpointUrl, string language, bool currentLanguage)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(_urlBase);
            sb.Append(endpointUrl);

            if (!currentLanguage)
            {
                sb.Append("?lang=");
                sb.Append(language);
            }

            return sb.ToString();
        }

    }
}
