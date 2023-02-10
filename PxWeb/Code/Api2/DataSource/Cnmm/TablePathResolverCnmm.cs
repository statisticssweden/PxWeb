using Px.Abstractions.Interfaces;
using PxWeb.Config.Api2;

namespace PxWeb.Code.Api2.DataSource.Cnmm
{
    public class TablePathResolverCnmm : ITablePathResolver
    {
        private readonly ICnmmConfigurationService _cnmmConfigurationService;

        public TablePathResolverCnmm(ICnmmConfigurationService cnmmConfigurationService)
        {
            _cnmmConfigurationService = cnmmConfigurationService;
        }

        public string Resolve(string language, string id, out bool selectionExists)
        {
            var cnmmOptions = _cnmmConfigurationService.GetConfiguration();
            var path = cnmmOptions.DatabaseID + ":" + id;
            selectionExists = true;
            return path;
        }
    }
}
