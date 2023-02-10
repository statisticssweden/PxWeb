using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.Linq;

namespace PxWeb.Code.Api2.NewtonsoftConfiguration
{
    /// <summary>
    /// This resolver is used to control the order of object properties when serialising to json.
    /// Properties of base classes will be displayed before properties of sub classes.
    /// </summary>
    public class BaseFirstContractResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization) =>
            base.CreateProperties(type, memberSerialization)
                ?.OrderBy(p => p.DeclaringType.BaseTypesAndSelf().Count()).ToList();
    }

}
