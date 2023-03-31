using Px.Search;
using PxWeb.Api2.Server.Models;
using System.Runtime.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using PxWeb.Config.Api2;
using Microsoft.Extensions.Options;

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
                foreach (var language in _configOptions.Languages)
                {
                    bool current = language.Id.Equals(lang);
                    linkPageList.Add(_linkCreator.GetTablesLink(LinkCreator.LinkRelationEnum.next, language.Id, query, 
                        pageSize, pageNumber + 1, current));
                }
            }

            if (pageNumber <= totalPages && pageNumber != 1)
            {
                // Links to previous page 
                foreach (var language in _configOptions.Languages)
                {
                    bool current = language.Id.Equals(lang);
                    linkPageList.Add(_linkCreator.GetTablesLink(LinkCreator.LinkRelationEnum.previous, language.Id, query,
                        pageSize, pageNumber - 1, current));
                }
            }
            // Links to last page 
            foreach (var language in _configOptions.Languages)
            {
                bool current = language.Id.Equals(lang);
                linkPageList.Add(_linkCreator.GetTablesLink(LinkCreator.LinkRelationEnum.last, language.Id, query,
                    pageSize, totalPages, current));
            }

            PageInfo page = new PageInfo
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalElements = totalElements,
                TotalPages = totalPages,
                Links = linkPageList                    
            };

            var tableList = new List<Table>();

            tablesResponse.Page = page;
            tablesResponse.Language = lang;           

            foreach (var item in searchResultContainer.searchResults)
            {
                var linkList = new List<Link>();

                // Links to table
                foreach (var language in _configOptions.Languages)
                {
                    bool current = language.Id.Equals(lang);
                    linkList.Add(_linkCreator.GetTableLink(LinkCreator.LinkRelationEnum.self, item.Id.ToUpper(), language.Id, current));
                }

                // Links to metadata
                foreach (var language in _configOptions.Languages)
                {
                    bool current = language.Id.Equals(lang);
                    linkList.Add(_linkCreator.GetTableMetadataJsonLink(LinkCreator.LinkRelationEnum.metadata, item.Id.ToUpper(), language.Id, current));
                }

                // Links to data
                foreach (var language in _configOptions.Languages)
                {
                    bool current = language.Id.Equals(lang);
                    linkList.Add(_linkCreator.GetTableDataLink(LinkCreator.LinkRelationEnum.data, item.Id.ToUpper(), language.Id, current));
                }

                var tb = new Table()
                {
                    Id = item.Id,
                    Label = item.Label,
                    Description = item.Description,
                    Tags = item.Tags.ToList(),
                    Updated = item.Updated,
                    FirstPeriod = item.FirstPeriod,
                    LastPeriod = item.LastPeriod,
                    Category = ToCategoryEnum(item.Category),
                    Discontinued = item.Discontinued,
                    VariableNames = item.VariableNames.ToList(),
                    Links = linkList                 
                };
                tableList.Add(tb);
            }

            tablesResponse.Tables = tableList;

            var linkListTableResponse = new List<Link>();

            // Links to tablesResponse
            foreach (var language in _configOptions.Languages)
            {
                bool current = language.Id.Equals(lang);
                linkListTableResponse.Add(_linkCreator.GetTablesLink(LinkCreator.LinkRelationEnum.self, language.Id, query, page.PageSize, page.PageNumber, current));
            }
            tablesResponse.Links = linkListTableResponse;

            return tablesResponse;
        }

        public static Table.CategoryEnum ToCategoryEnum(string category)
        {
            Table.CategoryEnum enumCategory = new Table.CategoryEnum();
            var enumType = typeof(Table.CategoryEnum);

                foreach (var name in Enum.GetNames(enumType))
                {
                    var enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetField(name).GetCustomAttributes(typeof(EnumMemberAttribute), true)).Single();
                    if (enumMemberAttribute.Value == category)
                    {
                        Table.CategoryEnum categoryEnum = (Table.CategoryEnum)Enum.Parse(enumType, name);
                        return enumCategory = categoryEnum;
                    }
                }
           
            return enumCategory;
        }
    }
}
