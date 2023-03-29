using Px.Search;
using PxWeb.Api2.Server.Models;
using System.Runtime.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PxWeb.Mappers
{
    public class TablesResponseMapper : ITablesResponseMapper
    {
        public TablesResponse Map(IEnumerable<SearchResult> searchResult, string lang)
        {
            TablesResponse tablesResponse = new TablesResponse();
            var tableList = new List<Table>();
            tablesResponse.Language = lang;
            

            foreach (var item in searchResult)
            {
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
