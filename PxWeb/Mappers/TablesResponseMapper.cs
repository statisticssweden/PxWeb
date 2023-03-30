using Px.Search;
using PxWeb.Api2.Server.Models;
using System.Runtime.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using PxWeb.Config.Api2;
using Microsoft.Extensions.Options;
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

        public TablesResponse Map(IEnumerable<SearchResult> searchResult, string lang)
        {
            TablesResponse tablesResponse = new TablesResponse();
            PageInfo page = new PageInfo
                {
                    PageNumber = searchResult.Select(x => x.pageNumber).First(),
                    PageSize = searchResult.Select(x => x.pageSize).First(),
                    TotalElements = searchResult.Select(x => x.totalElements).First(),
                    TotalPages = searchResult.Select(x => x.totalPages).First()
                };
            var tableList = new List<Table>();

            tablesResponse.Page = page;
            tablesResponse.Language = lang;           

            foreach (var item in searchResult)
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
