using PCAxis.Menu;
using System.Collections.Generic;

namespace PxWeb.Code.Api2.DataSource.Cnmm
{
    public interface IItemSelectionResolverFactory
    {
        Dictionary<string, ItemSelection> GetMenuLookup(string language);
    }
}
