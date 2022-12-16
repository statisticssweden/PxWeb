using System.Collections.Generic;
using System;

namespace PxWeb.Code.Api2.NewtonsoftConfiguration
{
    /// <summary>
    /// This class is used together with BaseFirstContractResolver to control the order of object properties
    /// when serialixing to json.
    /// </summary>
    public static class TypeExtensions
    {
        public static IEnumerable<Type> BaseTypesAndSelf(this Type type)
        {
            while (type != null)
            {
                yield return type;
                type = type.BaseType;
            }
        }
    }
}
