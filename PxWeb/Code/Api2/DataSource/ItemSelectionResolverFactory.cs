using System.Collections.Generic;
using PxWeb.Code.Api2.DataSource.Cnmm;

namespace PxWeb.Code.Api2.DataSource
{
    public interface IItemSelectionResolverFactory
    {
        Dictionary<string, string> GetMenuLookup();
    }
    public class ItemSelectionResolverFactory : IItemSelectionResolverFactory
    {
        public Dictionary<string, string> GetMenuLookup()
         {
             return PCAxis.Sql.DbConfig.SqlDbConfigsStatic.DataBases["ssd"].GetMenuLookup();
        }
    }
}
