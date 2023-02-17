using System.Collections.Generic;
using PCAxis.Menu;
using PxWeb.Config.Api2;

namespace PxWeb.Code.Api2.DataSource.Cnmm
{
    public class ItemSelectionResolverCnmmFactory : IItemSelectionResolverFactory
    {
        private readonly ICnmmConfigurationService _cnmmConfigurationService;

        public ItemSelectionResolverCnmmFactory(ICnmmConfigurationService cnmmConfigurationService)
        {
            _cnmmConfigurationService = cnmmConfigurationService;
        }

        public Dictionary<string, ItemSelection> GetMenuLookup(string language)
        {
            var cnmmOptions = _cnmmConfigurationService.GetConfiguration();
            return PCAxis.Sql.DbConfig.SqlDbConfigsStatic.DataBases[cnmmOptions.DatabaseID].GetMenuLookup(language);
        }
    }
}
