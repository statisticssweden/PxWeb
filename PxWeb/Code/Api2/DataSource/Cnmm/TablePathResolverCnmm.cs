using PCAxis.Menu;
using Px.Abstractions.Interfaces;
using PxWeb.Config.Api2;

namespace PxWeb.Code.Api2.DataSource.Cnmm
{
    public class TablePathResolverCnmm : ITablePathResolver
    {
        private readonly ICnmmConfigurationService _cnmmConfigurationService;
        private readonly IItemSelectionResolver _itemSelectionResolver;

        public TablePathResolverCnmm(ICnmmConfigurationService cnmmConfigurationService, IItemSelectionResolver itemSelectionResolver)
        {
            _cnmmConfigurationService = cnmmConfigurationService;
            _itemSelectionResolver = itemSelectionResolver;
        }

        public string Resolve(string language, string id, out bool selectionExists)
        {
            var cnmmOptions = _cnmmConfigurationService.GetConfiguration();
            ItemSelection itmSel = _itemSelectionResolver.Resolve(language, id, out selectionExists);
            string path = "";

            if (selectionExists)
            {
                path = cnmmOptions.DatabaseID + ":" + itmSel.Selection;
                selectionExists = true;
            }

            return path;
        }
    }
}
