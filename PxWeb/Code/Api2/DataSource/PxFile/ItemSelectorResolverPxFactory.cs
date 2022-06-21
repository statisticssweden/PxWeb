using System.Collections.Generic;
using PxWeb.Code.Api2.DataSource.Cnmm;
using PxWeb.Config.Api2;

namespace PxWeb.Code.Api2.DataSource.PxFile
{
    public class ItemSelectorResolverPxFactory : IItemSelectionResolverFactory
    {
        private readonly IPxFileConfigurationService _pxFileConfigurationService;

        public ItemSelectorResolverPxFactory(IPxFileConfigurationService pxFileConfigurationService)
        {
            _pxFileConfigurationService = pxFileConfigurationService;
        }
        
        public Dictionary<string, string> GetMenuLookup()
        {
            var cnmmOptions = _pxFileConfigurationService.GetConfiguration();
            
            //todo: fetch MenuLookup from PxFile
            //return PCAxis.Sql.DbConfig.SqlDbConfigsStatic.DataBases[cnmmOptions.DatabaseID].GetMenuLookup();

            return new Dictionary<string, string>();
        }
    }
}
