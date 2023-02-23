using System.Collections.Generic;
using Microsoft.Extensions.Options;
using PCAxis.Menu;
using PxWeb.Config.Api2;

namespace PxWeb.Code.Api2.DataSource.Cnmm
{
    public class ItemSelectionResolverCnmmFactory : IItemSelectionResolverFactory
    {
        private readonly ICnmmConfigurationService _cnmmConfigurationService;
        private readonly IOptions<PxApiConfigurationOptions> _configOptions;

        public ItemSelectionResolverCnmmFactory(ICnmmConfigurationService cnmmConfigurationService, IOptions<PxApiConfigurationOptions> configOptions)
        {
            _cnmmConfigurationService = cnmmConfigurationService;
            _configOptions = configOptions;
        }

        public Dictionary<string, ItemSelection> GetMenuLookup(string language)
        {
            var cnmmOptions = _cnmmConfigurationService.GetConfiguration();
            return PCAxis.Sql.DbConfig.SqlDbConfigsStatic.DataBases[cnmmOptions.DatabaseID].GetMenuLookup(language, _configOptions);
        }
    }
}
