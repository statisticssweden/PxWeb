using System.Collections.Generic;
using PxWeb.Code.Api2.DataSource.Cnmm;
using PxWeb.Config.Api2;

namespace PxWeb.Code.Api2.DataSource
{
    public interface IItemSelectionResolverFactory
    {
        Dictionary<string, string> GetMenuLookup();
    }
    public class ItemSelectionResolverFactory : IItemSelectionResolverFactory
    {
        private readonly ICnmmConfigurationService _cnmmConfigurationService;

        public ItemSelectionResolverFactory(ICnmmConfigurationService cnmmConfigurationService)
        {
            _cnmmConfigurationService = cnmmConfigurationService;
        }

        public Dictionary<string, string> GetMenuLookup()
         {
            var cnmmOptions = _cnmmConfigurationService.GetConfiguration();
            return PCAxis.Sql.DbConfig.SqlDbConfigsStatic.DataBases[cnmmOptions.DatabaseID].GetMenuLookup();
        }
    }
}
