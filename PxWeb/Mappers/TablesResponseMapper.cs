using Px.Search;
using PxWeb.Api2.Server.Models;
using System.Runtime.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using PxWeb.Config.Api2;
using Microsoft.Extensions.Options;
using PxWeb.Converters;
using PCAxis.Paxiom.Localization;

namespace PxWeb.Mappers
{
    public class TablesResponseMapper : ITablesResponseMapper
    {
        private ILinkCreator _linkCreator;
        private PxApiConfigurationOptions _configOptions;
        public TablesResponseMapper(ILinkCreator linkCreator, IOptions<PxApiConfigurationOptions> configOptions)
        {
            _linkCreator = linkCreator;
            _configOptions = configOptions.Value;
        }

        public TablesResponse Map(SearchResultContainer searchResultContainer, string lang, string query)
        {
            var tablesResponse = new TablesResponse();
            var linkPageList = new List<Link>();
            var pageNumber = searchResultContainer.pageNumber;
            var pageSize = searchResultContainer.pageSize;
            var totalElements = searchResultContainer.totalElements;
            var totalPages = searchResultContainer.totalPages;

            if (pageNumber < totalPages)
            {
                // Links to next page 
                linkPageList.Add(_linkCreator.GetTablesLink(LinkCreator.LinkRelationEnum.next, lang, query, pageSize, pageNumber + 1, true));
            }

            if (pageNumber <= totalPages && pageNumber != 1)
            {
                // Links to previous page 
                linkPageList.Add(_linkCreator.GetTablesLink(LinkCreator.LinkRelationEnum.previous, lang, query, pageSize, pageNumber - 1, true));
            }

            if (totalPages > 1)
            {
                // Links to last page 
                linkPageList.Add(_linkCreator.GetTablesLink(LinkCreator.LinkRelationEnum.last, lang, query, pageSize, totalPages, true));
            }

            PageInfo page = new PageInfo
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalElements = totalElements,
                TotalPages = totalPages == 0 ?  totalPages + 1 :totalPages,
                Links = linkPageList                    
            };

            var tableList = new List<Table>();

            tablesResponse.Page = page;
            tablesResponse.Language = lang;           

            foreach (var item in searchResultContainer.searchResults)
            {
                var linkList = new List<Link>();

                // Links to table
                linkList.Add(_linkCreator.GetTableLink(LinkCreator.LinkRelationEnum.self, item.Id.ToUpper(), lang, true));

                // Links to metadata
                linkList.Add(_linkCreator.GetTableMetadataJsonLink(LinkCreator.LinkRelationEnum.metadata, item.Id.ToUpper(), lang, true));

                // Links to data
                linkList.Add(_linkCreator.GetTableDataLink(LinkCreator.LinkRelationEnum.data, item.Id.ToUpper(), lang, true));

                var tb = new Table()
                {
                    Type = FolderContentItemTypeEnum.TableEnum,
                    Id = item.Id,
                    Label = item.Label,
                    Description = item.Description,
                    //Tags = item.Tags.ToList(), // TODO: Implement later
                    Updated = item.Updated,
                    FirstPeriod = item.FirstPeriod,
                    LastPeriod = item.LastPeriod,
                    Category = EnumConverter.ToCategoryEnum(item.Category),
                    Discontinued = item.Discontinued,
                    VariableNames = item.VariableNames.ToList(),
                    Links = linkList                 
                };
                tableList.Add(tb);
            }

            tablesResponse.Tables = tableList;

            var linkListTableResponse = new List<Link>();

            // Links to tablesResponse
            linkListTableResponse.Add(_linkCreator.GetTablesLink(LinkCreator.LinkRelationEnum.self, lang, query, page.PageSize, page.PageNumber, true));

            tablesResponse.Links = linkListTableResponse;

            return tablesResponse;
        }

        
    }
}
