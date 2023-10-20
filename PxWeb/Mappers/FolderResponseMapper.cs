using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using PCAxis.Menu;
using PCAxis.Paxiom;
using PxWeb.Api2.Server.Models;
using PxWeb.Config.Api2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PxWeb.Mappers
{
    public class FolderResponseMapper : IFolderResponseMapper   
    {
        private ILinkCreator _linkCreator;
        private PxApiConfigurationOptions _configOptions;
        private string _language;

        public FolderResponseMapper(ILinkCreator linkCreator, IOptions<PxApiConfigurationOptions> configOptions)
        {
            _linkCreator = linkCreator;
            _configOptions = configOptions.Value;
            _language = _configOptions.DefaultLanguage;
        }

        public FolderResponse GetFolder(PxMenuItem currentItem, string language, bool root = false)
        {
            // Id shall not be displayed for the root folder
            string id = root == false ? Path.GetFileName(currentItem.ID.Selection) : ""; 

            _language = language;

            FolderResponse folder = new FolderResponse
            {
                Language = _language,
                Id = id, 
                Label = currentItem.Text,
                Description = currentItem.Description,
                //Tags = null // TODO: Implement later
            };

            folder.Links = new List<PxWeb.Api2.Server.Models.Link>();
            folder.Links.Add(_linkCreator.GetFolderLink(LinkCreator.LinkRelationEnum.self, folder.Id.ToUpper(), _language, true));

            folder.FolderContents = new List<FolderContentItem> { };

            foreach (Item child in currentItem.SubItems)
            {
                folder.FolderContents.Add(Map(child));
            }

            return folder;
        }

        private FolderContentItem Map(Item child)
        {
            FolderContentItem itm;

            if (child is PxMenuItem)
            {
                itm = MapFolderInformation((PxMenuItem)child);
            }
            else if (child is TableLink)
            {
                itm = MapTable((TableLink)child);
            }
            else
            {
                itm = MapHeading((Headline)child); 
            }
            
            return itm;
        }

        private FolderContentItem MapFolderInformation(PxMenuItem child)
        {
            FolderInformation fi = new FolderInformation
            {
                Id = Path.GetFileName(child.ID.Selection),
                Type = FolderContentItemTypeEnum.FolderInformationEnum,
                Description = child.Description,
                Label = child.Text,
                //Tags = null, //TODO: Implement later
                Links = new List<PxWeb.Api2.Server.Models.Link>()
            };

            fi.Links.Add(_linkCreator.GetFolderLink(LinkCreator.LinkRelationEnum.self, Path.GetFileName(fi.Id), _language, true));

            return fi;
        }

        private Table MapTable(TableLink child)
        {
            var tableId = child.TableId;

            DateTime lastUpdated = DateTime.MinValue;

            if (child.LastUpdated != null)
            {
                lastUpdated = (DateTime)child.LastUpdated;
                lastUpdated = lastUpdated.ToUniversalTime();
            }

            Table table = new Table
            {
                Id = tableId,
                Type = FolderContentItemTypeEnum.TableEnum,
                Description = child.Description,
                Label = child.Text,
                Updated = child.LastUpdated != null ? lastUpdated : child.LastUpdated,
                //Tags = null, // TODO: Implement later
                Category = GetCategory(child.Category),
                FirstPeriod = child.StartTime,
                LastPeriod = child.EndTime
            };
            table.Links = new List<PxWeb.Api2.Server.Models.Link>();

            // Links to table
            table.Links.Add(_linkCreator.GetTableLink(LinkCreator.LinkRelationEnum.self, tableId, _language, true));

            // Links to metadata
            table.Links.Add(_linkCreator.GetTableMetadataJsonLink(LinkCreator.LinkRelationEnum.metadata, tableId, _language, true));

            // Links to data
            table.Links.Add(_linkCreator.GetTableDataLink(LinkCreator.LinkRelationEnum.data, tableId, _language, true));

            return table;   
        }

        private Heading MapHeading(Headline child)
        {
            Heading heading = new Heading
            {
                Id = Path.GetFileName(child.ID.Selection),
                Type = FolderContentItemTypeEnum.HeadingEnum,
                Label = child.Text,
                Description = child.Description
            };

            return heading;
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
