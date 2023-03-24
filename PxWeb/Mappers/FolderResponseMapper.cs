using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using PCAxis.Menu;
using PCAxis.Paxiom;
using PxWeb.Api2.Server.Models;
using PxWeb.Config.Api2;
using System.Collections.Generic;
using System.IO;

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

        public Folder GetFolder(PxMenuItem currentItem, string language, bool root = false)
        {
            // Id shall not be displayed for the root folder
            string id = root == false ? Path.GetFileName(currentItem.ID.Selection) : "";

            _language = language;

            Folder folder = new Folder
            {
                Language = _language,
                Id = id, 
                ObjectType = typeof(Folder).Name, // TODO: Create enum in spec
                Label = currentItem.Text,
                Description = currentItem.Description,
                Tags = null // TODO: Implement later
            };

            folder.Links = new List<PxWeb.Api2.Server.Models.Link>();

            foreach (var lang in _configOptions.Languages)
            {
                bool current = lang.Id.Equals(_language);
                folder.Links.Add(_linkCreator.GetFolderLink(LinkCreator.LinkRelationEnum.self, folder.Id.ToUpper(), lang.Id, current));
            }

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
                ObjectType = typeof(FolderInformation).Name, // TODO: Create enum in spec
                Description = child.Description,
                Label = child.Text,
                Tags = null,
                Links = new List<PxWeb.Api2.Server.Models.Link>()
            };

            foreach (var lang in _configOptions.Languages)
            {
                bool current = lang.Id.Equals(_language);
                fi.Links.Add(_linkCreator.GetFolderLink(LinkCreator.LinkRelationEnum.self, Path.GetFileName(fi.Id), lang.Id, current));
            }

            return fi;
        }

        private Table MapTable(TableLink child)
        {
            var tableId = child.TableId;

            Table table = new Table
            {
                Id = tableId,
                ObjectType = typeof(Table).Name, // TODO: Create enum in spec
                Description = child.Description,
                Label = child.Text,
                Updated = child.Published,
                Tags = null, // TODO: Implement later
                Category = GetCategory(child.Category),
                FirstPeriod = child.StartTime,
                LastPeriod = child.EndTime
            };
            table.Links = new List<PxWeb.Api2.Server.Models.Link>();

            // Links to table
            foreach (var lang in _configOptions.Languages)
            {
                bool current = lang.Id.Equals(_language);
                table.Links.Add(_linkCreator.GetTableLink(LinkCreator.LinkRelationEnum.self, tableId, lang.Id, current));
            }

            // Links to metadata
            foreach (var lang in _configOptions.Languages)
            {
                bool current = lang.Id.Equals(_language);
                table.Links.Add(_linkCreator.GetTableMetadataJsonLink(LinkCreator.LinkRelationEnum.metadata, tableId, lang.Id, current));
            }

            // Links to data
            foreach (var lang in _configOptions.Languages)
            {
                bool current = lang.Id.Equals(_language);
                table.Links.Add(_linkCreator.GetTableDataLink(LinkCreator.LinkRelationEnum.data, tableId, lang.Id, current));
            }

            return table;   
        }

        private Heading MapHeading(Headline child)
        {
            Heading heading = new Heading
            {
                Id = Path.GetFileName(child.ID.Selection),
                ObjectType = typeof(Heading).Name, // TODO: Create enum in spec
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
