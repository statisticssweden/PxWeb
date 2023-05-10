using PxWeb.Api2.Server.Models;
using System.Runtime.Serialization;
using System;
using System.Linq;

namespace PxWeb.Converters
{
    public class EnumConverter
    {
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
