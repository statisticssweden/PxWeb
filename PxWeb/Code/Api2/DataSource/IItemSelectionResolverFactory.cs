using System.Collections.Generic;

namespace PxWeb.Code.Api2.DataSource.Cnmm
{
    public interface IItemSelectionResolverFactory
    {
        Dictionary<string, string> GetMenuLookup(string language);
    }
}
