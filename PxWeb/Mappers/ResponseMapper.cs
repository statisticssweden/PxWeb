using Microsoft.AspNetCore.Http;
using PCAxis.Menu;
using PxWeb.Api2.Server.Models;
using PxWeb.Models.Api2;
using System.Collections.Generic;
using System.IO;

namespace PxWeb.Mappers
{
    public class ResponseMapper : IResponseMapper   
    {
        public Folder GetFolder(PxMenuItem currentItem, HttpContext httpContext)
        {
            string urlBase = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/api/v2/";
            Folder folder = new Folder
            {
                Id = Path.GetFileName(currentItem.ID.Selection),
                //Id = currentItem.ID.Selection,
                ObjectType = typeof(Folder).Name,
                Label = currentItem.Text,
                Description = currentItem.Description,
                Tags = null // TODO: Implement later
            };

            folder.Links = new List<PxWeb.Api2.Server.Models.Link>();
            PxWeb.Api2.Server.Models.Link link = new PxWeb.Api2.Server.Models.Link();
            link.Rel = "self";
            link.Href = urlBase + Path.Combine("navigation/", folder.Id);
            folder.Links.Add(link);

            folder.FolderContents = new List<FolderContentItem> { };

            foreach (Item child in currentItem.SubItems)
            {
                if (child is PxMenuItem)
                {
                    FolderInformation fi = new FolderInformation
                    {
                        Id = Path.GetFileName(child.ID.Selection),
                        ObjectType = typeof(FolderInformation).Name,
                        Description = child.Description,
                        Label = child.Text,
                        Tags = null,
                        Links = new List<PxWeb.Api2.Server.Models.Link>()
                    };

                    PxWeb.Api2.Server.Models.Link childLink = new PxWeb.Api2.Server.Models.Link();
                    childLink.Rel = "folder";
                    childLink.Href = urlBase + Path.Combine("navigation/", Path.GetFileName(fi.Id));
                    fi.Links.Add(childLink);
                    folder.FolderContents.Add(fi);
                }
                else if (child is TableLink)
                {
                    var tableId = ((TableLink)child).TableId;

                    Table table = new Table
                    {
                        //Id = Path.GetFileName(child.ID.Selection),
                        Id = tableId,
                        ObjectType = typeof(Table).Name,
                        Description = child.Description,
                        Label = child.Text,
                        Updated = ((TableLink)child).Published,
                        Tags = null, // TODO: Implement later
                        Category = GetCategory(((TableLink)child).Category),
                        FirstPeriod = ((TableLink)child).StartTime,
                        LastPeriod = ((TableLink)child).EndTime
                    };
                    table.Links = new List<PxWeb.Api2.Server.Models.Link>();

                    PxWeb.Api2.Server.Models.Link childLink = new PxWeb.Api2.Server.Models.Link();
                    childLink.Rel = "self";
                    //childLink.Href = urlBase + Path.Combine($"tables/{Path.GetFileName(table.Id)}");
                    childLink.Href = urlBase + Path.Combine($"tables/{tableId}");
                    table.Links.Add(childLink);

                    childLink = new PxWeb.Api2.Server.Models.Link();
                    childLink.Rel = "metadata";
                    //childLink.Href = urlBase + Path.Combine($"tables/{Path.GetFileName(table.Id)}/metadata");
                    childLink.Href = urlBase + Path.Combine($"tables/{tableId}/metadata");
                    table.Links.Add(childLink);

                    childLink = new PxWeb.Api2.Server.Models.Link();
                    childLink.Rel = "data";
                    //childLink.Href = urlBase + Path.Combine($"tables/{Path.GetFileName(table.Id)}/data");
                    childLink.Href = urlBase + Path.Combine($"tables/{tableId}/data");
                    table.Links.Add(childLink);

                    folder.FolderContents.Add(table);
                }
                else if (child is Headline)
                {
                    Heading heading = new Heading
                    {
                        Id = Path.GetFileName(child.ID.Selection),
                        ObjectType = typeof(Heading).Name,
                        Label = child.Text,
                        Description = child.Description
                    };
                    folder.FolderContents.Add(heading);
                }
            }

            return folder;

        }

        /// <summary>
        /// Translate Menu enum to PxApi enum
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public Table.CategoryEnum GetCategory(PresCategory category)
        {
            switch (category)
            {
                case PresCategory.NotSet: //TODO: how shall we handle NotSet?
                    return Table.CategoryEnum.PrivateEnum;
                case PresCategory.Official:
                    return Table.CategoryEnum.PublicEnum;
                case PresCategory.Internal:
                    return Table.CategoryEnum.InternalEnum;
                case PresCategory.Private:
                    return Table.CategoryEnum.PrivateEnum;
                default:
                    return Table.CategoryEnum.PrivateEnum; //TODO: How shall we handle this?
            }
        }
    }
}
